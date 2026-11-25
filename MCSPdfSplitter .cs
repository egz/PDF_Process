using iText.Kernel.Pdf;
using iText.Kernel.Utils;
using System;
using System.Collections.Generic;

namespace PDF_Process
{
    // 保留的 iText 分割工具。主畫面的 PDF 匯出流程目前未呼叫此類別。
    public class MCSPdfSplitter : PdfSplitter
    {

        private int _pageNumber;
        private string _rawPdfFile;
        public List<string> SplitPdfNamesList = new List<string>();
        public MCSPdfSplitter(PdfDocument pdfDocument, string rawPdfFile) : base(pdfDocument)
        {

            _rawPdfFile = rawPdfFile;
        }

        protected override PdfWriter GetNextPdfWriter(PageRange documentPageRange)
        {

            // 以流水號與 GUID 組成分割檔名，並記錄每次產生的路徑。
            _pageNumber++;
            string splitPDFFileName = _rawPdfFile.Substring(0, _rawPdfFile.LastIndexOf(".pdf")) + _pageNumber + "split" + Guid.NewGuid().ToString().Replace("-", "") + ".pdf";
            SplitPdfNamesList.Add(splitPDFFileName);
            return new PdfWriter(splitPDFFileName);
        }
    }
}
