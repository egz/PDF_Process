using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using iText.Kernel.Pdf;

namespace PDF_Process
{
    // 每個來源與柱位組合回傳一筆結果，Detail 可包含警告或失敗原因。
    internal sealed record PdfExportResult(string FilePath, bool Exported, string Detail);

    // 執行 PDF I/O 並回傳結果，由呼叫端決定如何顯示。
    internal static class PdfExportService
    {
        // 兩個數字分別限制每組來源數和每輪柱位數，勾選更多項目時會自動分批。
        private const int SourcesPerGroup = 5;
        private const int TargetsPerRound = 10;

        public static async Task<IReadOnlyList<PdfExportResult>> ExportAsync(
            string outputRoot, IReadOnlyList<PdfSource> sources, IReadOnlyList<PdfExportTarget> targets)
        {
            var plan = PdfOutputPlan.Create(outputRoot, sources, targets);
            var results = new List<PdfExportResult>();

            // 等前一組完成匯出並關閉來源後，才開啟下一組。
            // 例如 6 份來源、8 個柱位會分成 5 份及 1 份兩組，各跑一輪。
            for (int index = 0; index < plan.Count; index += SourcesPerGroup)
            {
                var group = plan.Skip(index).Take(SourcesPerGroup).ToArray();
                var groupResults = await Task.Run(() => ExportGroupAsync(group)).ConfigureAwait(false);
                results.AddRange(groupResults);
            }

            return results;
        }

        private static async Task<IReadOnlyList<PdfExportResult>> ExportGroupAsync(PdfExportBatch[] group)
        {
            var documents = new PdfDocument[group.Length];
            var results = group.Select(_ => new List<PdfExportResult>()).ToArray();
            try
            {
                // 每份來源只開啟一次。讀取失敗時記錄該來源對應的逐檔結果，其餘來源照常處理。
                for (int index = 0; index < group.Length; index++)
                {
                    var batch = group[index];
                    try
                    {
                        documents[index] = new PdfDocument(new PdfReader(batch.Source.FilePath));
                    }
                    catch (Exception ex)
                    {
                        results[index].AddRange(batch.Files.Select(file => new PdfExportResult(
                            file.FilePath, false, $"無法讀取來源 PDF「{batch.Source.FilePath}」：{ex.Message}")));
                    }
                }

                // 等本輪所有來源處理完畢才進入下一輪。同一組來源在各輪共用已開啟的 PDF。
                // 例如 3 份來源、11 個柱位會在同一組先處理 10 個，再處理剩下的 1 個。
                int targetCount = group[0].Files.Count;
                for (int start = 0; start < targetCount; start += TargetsPerRound)
                {
                    int end = Math.Min(start + TargetsPerRound, targetCount);
                    var round = new List<Task>();
                    for (int index = 0; index < group.Length; index++)
                    {
                        var document = documents[index];
                        if (document == null)
                            continue;

                        var batch = group[index];
                        var sourceResults = results[index];
                        int first = start;
                        // 不同來源可並行。同一份 PDF 依序處理本輪柱位，同時只寫一份輸出。
                        round.Add(Task.Run(() =>
                        {
                            for (int target = first; target < end; target++)
                                sourceResults.Add(ExportFile(document, batch.Source.Offset, batch.Files[target]));
                        }));
                    }

                    await Task.WhenAll(round).ConfigureAwait(false);
                }
            }
            finally
            {
                // 所有柱位輪次結束後才關閉本組來源，發生例外時也會執行。
                for (int index = 0; index < documents.Length; index++)
                    CloseSource(documents[index], group[index].Source, results[index]);
            }

            return results.SelectMany(sourceResults => sourceResults).ToArray();
        }

        private static void CloseSource(PdfDocument document, PdfSource source, List<PdfExportResult> results)
        {
            try
            {
                document?.Close();
            }
            catch (Exception ex)
            {
                // 關閉來源失敗時保留已匯出的結果，並附加警告。
                if (results.Count > 0)
                {
                    var first = results[0];
                    string warning = $"無法關閉來源 PDF「{source.FilePath}」：{ex.Message}";
                    results[0] = first with { Detail = first.Detail == null ? warning : first.Detail + Environment.NewLine + warning };
                }
            }
        }

        private static PdfExportResult ExportFile(PdfDocument document, string offset, PdfExportFile file)
        {
            string outputPath = file.FilePath;
            string temporaryPath = null;
            try
            {
                List<int> pages;
                string detail = null;
                try
                {
                    pages = GetPages(file.Pages.TrimEnd(','), offset);
                }
                catch (Exception ex) when (ex is FormatException || ex is OverflowException)
                {
                    // 延續原本的解析失敗行為，並附上原因供結果視窗顯示。
                    pages = new List<int> { 1 };
                    detail = $"頁碼解析失敗，依原規則輸出第 1 頁（{ex.Message}）";
                }

                if (pages.Count == 0)
                    return new PdfExportResult(outputPath, false, "沒有可匯出的頁碼");

                string folder = Path.GetDirectoryName(outputPath);
                Directory.CreateDirectory(folder);
                // 在同一資料夾完成暫存 PDF，再替換正式檔，避免覆蓋成半成品。
                temporaryPath = Path.Combine(folder, Path.GetRandomFileName());
                using (var output = new PdfDocument(new PdfWriter(temporaryPath)))
                {
                    document.CopyPagesTo(pages, output);
                }

                File.Move(temporaryPath, outputPath, overwrite: true);
                return new PdfExportResult(outputPath, true, detail);
            }
            catch (Exception ex)
            {
                string detail = ex.Message;
                if (temporaryPath != null)
                {
                    try
                    {
                        File.Delete(temporaryPath);
                    }
                    catch (Exception cleanupError) when (cleanupError is IOException || cleanupError is UnauthorizedAccessException)
                    {
                        detail += $"；無法清除暫存檔「{temporaryPath}」：{cleanupError.Message}";
                    }
                }

                return new PdfExportResult(outputPath, false, detail);
            }
        }

        private static List<int> GetPages(string pageRanges, string offsetText)
        {
            var pages = new List<int>();
            // PDF 頁碼從 1 開始，偏移無法解析時沿用 0。
            int.TryParse(offsetText, out int offset);

            // 逗號分隔各段，連字號表示包含起訖頁的範圍。
            foreach (string range in pageRanges.Split(','))
            {
                if (range.Contains('-'))
                {
                    string[] bounds = range.Split('-');
                    int first = Convert.ToInt32(bounds[0]) + offset;
                    int last = Convert.ToInt32(bounds[1]) + offset;

                    if (last < first || first < 1)
                        continue;

                    for (long page = first; page <= last; page++)
                        pages.Add((int)page);
                }
                else
                {
                    int page = Convert.ToInt32(range) + offset;
                    if (page >= 1)
                        pages.Add(page);
                }
            }

            return pages;
        }
    }
}
