using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace PDF_Process
{
    internal sealed record PdfSource(string Name, string FilePath, string Offset);
    internal sealed record PdfExportTarget(string Name, string Pages);
    internal sealed record PdfExportFile(string FilePath, string Pages);
    internal sealed record PdfExportBatch(PdfSource Source, IReadOnlyList<PdfExportFile> Files);

    internal static class PdfOutputPlan
    {
        public static IReadOnlyList<PdfExportBatch> Create(
            string outputRoot, IReadOnlyList<PdfSource> sources, IReadOnlyList<PdfExportTarget> targets)
        {
            if (sources.Count == 0 || targets.Count == 0)
                return Array.Empty<PdfExportBatch>();

            if (string.IsNullOrWhiteSpace(outputRoot) || !Path.IsPathFullyQualified(outputRoot))
                throw new ArgumentException("請選擇輸出資料夾，或輸入完整的資料夾路徑。");

            string root = Path.GetFullPath(outputRoot);
            var folders = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var target in targets)
            {
                ValidateName(target.Name, "柱位名稱");
                string folder = Path.Combine(root, target.Name);
                if (!folders.TryAdd(target.Name, folder))
                    throw new ArgumentException($"柱位名稱重複：「{target.Name}」。");

                // 避免透過現有的柱位連結將 PDF 寫到所選根資料夾之外。
                if (Directory.Exists(folder) && (File.GetAttributes(folder) & FileAttributes.ReparsePoint) != 0)
                    throw new ArgumentException($"柱位資料夾「{folder}」是連結，請改用一般資料夾。");
            }

            // 先列出整批輸出檔，再開始複製頁面，避免同批工作互相覆寫。
            var paths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var batches = new List<PdfExportBatch>();
            foreach (var source in sources)
            {
                ValidateName(source.Name, "來源名稱");
                var files = new List<PdfExportFile>();
                foreach (var target in targets)
                {
                    string path = Path.Combine(folders[target.Name], $"{target.Name}_{source.Name}.pdf");
                    if (!paths.Add(path))
                        throw new ArgumentException($"輸出檔名重複：「{path}」。請確認來源名稱。");

                    files.Add(new PdfExportFile(path, target.Pages));
                }

                batches.Add(new PdfExportBatch(source, files));
            }

            return batches;
        }

        private static void ValidateName(string name, string field)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException($"請填寫{field}。");

            int invalidIndex = name.IndexOfAny(Path.GetInvalidFileNameChars());
            if (invalidIndex >= 0)
                throw new ArgumentException($"{field}「{name}」含有 Windows 不允許的字元「{name[invalidIndex]}」。");

            if (name.EndsWith('.') || name.EndsWith(' '))
                throw new ArgumentException($"{field}「{name}」不能以句點或空格結尾。");

            string firstPart = name.Split('.')[0].TrimEnd(' ');
            if (Regex.IsMatch(firstPart, @"\A(?:CON|PRN|AUX|NUL|COM[1-9¹²³]|LPT[1-9¹²³]|CONIN\$|CONOUT\$)\z",
                RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
                throw new ArgumentException($"{field}「{name}」是 Windows 保留名稱。");
        }
    }
}
