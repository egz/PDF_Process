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
        public static async Task<IReadOnlyList<PdfExportResult>> ExportAsync(
            string outputRoot, IReadOnlyList<PdfSource> sources, IReadOnlyList<PdfExportTarget> targets)
        {
            var plan = PdfOutputPlan.Create(outputRoot, sources, targets);

            // 不同來源各自處理。同一份來源 PDF 只開啟一次，依序匯出各柱位。
            var batches = await Task.WhenAll(plan.Select(batch =>
                Task.Run(() => ExportSource(batch)))).ConfigureAwait(false);

            return batches.SelectMany(batch => batch).ToArray();
        }

        private static IReadOnlyList<PdfExportResult> ExportSource(PdfExportBatch batch)
        {
            PdfSource source = batch.Source;
            PdfDocument document;
            try
            {
                document = new PdfDocument(new PdfReader(source.FilePath));
            }
            catch (Exception ex)
            {
                return batch.Files.Select(file => new PdfExportResult(
                    file.FilePath, false, $"無法讀取來源 PDF「{source.FilePath}」：{ex.Message}")).ToArray();
            }

            using (document)
            {
                return batch.Files.Select(file => ExportFile(document, source.Offset, file)).ToArray();
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
