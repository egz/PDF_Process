using System.Collections.Generic;

namespace PDF_Process
{
    // 解析每行 HID:頁碼，格式錯誤以原始檔案中的行號回報。
    internal static class PillarSettingsParser
    {
        public static IReadOnlyList<TextDataModel> Parse(
            IEnumerable<string> lines, out IReadOnlyList<int> invalidLineNumbers)
        {
            var entries = new List<TextDataModel>();
            var invalidLines = new List<int>();
            int lineNumber = 0;

            foreach (string line in lines)
            {
                lineNumber++;
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                // 只接受一個半形冒號。空白頁碼仍保留為一筆設定。
                string[] fields = line.Split(':');
                if (fields.Length != 2)
                {
                    invalidLines.Add(lineNumber);
                    continue;
                }

                entries.Add(new TextDataModel(fields[0].Trim(), fields[1].Trim()));
            }

            invalidLineNumbers = invalidLines;
            return entries;
        }
    }
}
