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
        public string HID { get; set; }
        public string Pages { get; set; }

        public TextDataModel(string hid, string pages)
        {
            HID = hid;
            Pages = pages;
        }
    }


}
