using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PDF_Process
{
    // 保留的 RAW 列印工具，直接將資料送往 Windows 列印佇列。主畫面目前未呼叫。
    class PdfTest
    {
        // Windows 列印結構與原生 API 宣告。
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class DOCINFOA
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDocName;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pOutputFile;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDataType;
        }

        [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPStr)] string szPrinter, out IntPtr hPrinter, IntPtr pd);

        [DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool ClosePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartDocPrinter(IntPtr hPrinter, Int32 level, [In, MarshalAs(UnmanagedType.LPStruct)] DOCINFOA di);

        [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndDocPrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, Int32 dwCount, out Int32 dwWritten);

        // SendBytesToPrinter()
        // 接收印表機名稱與非受控位元組陣列，
        // 再將位元組送入列印佇列。
        // 寫入成功時回傳 true，失敗時回傳 false。
        public static bool SendBytesToPrinter(string szPrinterName, IntPtr pBytes, Int32 dwCount)
        {
            Int32 dwError = 0, dwWritten = 0;
            IntPtr hPrinter = new IntPtr(0);
            DOCINFOA di = new DOCINFOA();
            bool bSuccess = false; // 寫入成功前預設為失敗。

            di.pDocName = "My C#.NET RAW Document";
            di.pDataType = "RAW";

            // 取得印表機的控制代碼。
            if (OpenPrinter(szPrinterName.Normalize(), out hPrinter, IntPtr.Zero))
            {
                // 開始列印文件。
                if (StartDocPrinter(hPrinter, 1, di))
                {
                    // 開始列印頁面。
                    if (StartPagePrinter(hPrinter))
                    {
                        // 將位元組寫入列印佇列。
                        bSuccess = WritePrinter(hPrinter, pBytes, dwCount, out dwWritten);
                        EndPagePrinter(hPrinter);
                    }
                    EndDocPrinter(hPrinter);
                }
                ClosePrinter(hPrinter);
            }
            // 寫入失敗時讀取 Windows 錯誤碼，
            // 供判斷失敗原因。
            if (bSuccess == false)
            {
                dwError = Marshal.GetLastWin32Error();
            }
            return bSuccess;
        }

        // 將檔案內容複製到非受控緩衝區，以便呼叫 Windows 列印 API。
        public static bool SendFileToPrinter(string szPrinterName, string szFileName)
        {
            // 開啟來源檔案。
            FileStream fs = new FileStream(szFileName, FileMode.Open);
            // 使用二進位讀取器讀取檔案。
            BinaryReader br = new BinaryReader(fs);
            // 配置足以容納整份檔案的位元組陣列。
            Byte[] bytes = new Byte[fs.Length];
            bool bSuccess = false;
            // 儲存非受控緩衝區的指標。
            IntPtr pUnmanagedBytes = new IntPtr(0);
            int nLength;

            nLength = Convert.ToInt32(fs.Length);
            // 將檔案內容讀入陣列。
            bytes = br.ReadBytes(nLength);
            // 配置與檔案大小相同的非受控記憶體。
            pUnmanagedBytes = Marshal.AllocCoTaskMem(nLength);
            // 將受控陣列中的位元組複製到非受控緩衝區。
            Marshal.Copy(bytes, 0, pUnmanagedBytes, nLength);
            // 將緩衝區中的位元組送往印表機。
            bSuccess = SendBytesToPrinter(szPrinterName, pUnmanagedBytes, nLength);
            // 釋放先前配置的非受控記憶體。
            Marshal.FreeCoTaskMem(pUnmanagedBytes);
            return bSuccess;
        }

        public static bool SendStringToPrinter(string szPrinterName, string szString)
        {
            IntPtr pBytes;
            Int32 dwCount;
            // 取得字串的字元數。
            dwCount = szString.Length;
            // 假設印表機接收 ANSI 文字，
            // 將字串轉成 ANSI 編碼並配置緩衝區。
            pBytes = Marshal.StringToCoTaskMemAnsi(szString);
            // 將轉換後的 ANSI 文字送往印表機。
            SendBytesToPrinter(szPrinterName, pBytes, dwCount);
            Marshal.FreeCoTaskMem(pBytes);
            return true;
        }
        // 直接傳送 PDF 位元組。印表機或佇列須能解讀 PDF。
        public static void PrintPDF(string printName, string fileName)
        {
            SendFileToPrinter(printName, fileName);
        }
    }
}
