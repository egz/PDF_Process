using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDF_Process
{
    /// <summary> HID及頁數 </summary>
    public class TextDataModel
    {
        // 柱位識別碼，對應 TXT 冒號左側的欄位。
        public string HID { get; set; }
        // 保留原始頁碼文字，供畫面顯示及匯出時套用來源 PDF 的偏移。
        public string Pages { get; set; }

        public TextDataModel(string hid, string pages)
        {
            HID = hid;
            Pages = pages;
        }
    }


}
