using iText.Kernel.Pdf;
using iText.Kernel.Utils;
using System;
using System.Collections.Generic;

namespace PDF_Process
{
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

            _pageNumber++;
            string splitPDFFileName = _rawPdfFile.Substring(0, _rawPdfFile.LastIndexOf(".pdf")) + _pageNumber + "split" + Guid.NewGuid().ToString().Replace("-", "") + ".pdf";
            SplitPdfNamesList.Add(splitPDFFileName);
            return new PdfWriter(splitPDFFileName);
        }
    }
}
