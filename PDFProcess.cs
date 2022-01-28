
using System;
using System.Collections.Generic;
using System.IO;
using iText.Layout;
using iText.Kernel.Pdf;
using iText.Kernel.Utils;
using System.Printing;
using System.Diagnostics;

namespace PDF_Process
{
   public class PDFProcess
    {
        #region 屬性
        #endregion 屬性

        #region 初始
        public PDFProcess()
        {
                 iTextSharpPdfSplit(@"D:\chia\_all_.pdf");
          

        }
        #endregion 初始

        #region 函式


        /// <summary>
        /// 合并PDF文件
        /// </summary>
        /// <param name="inFiles">待合并文件列表</param>
        /// <param name="outFile">合并生成的文件名称</param>
        static void iTextSharpPdfMerge(List<String> inFiles, String outFile)
        {

        }

        /// <summary>
        /// 按每页拆分PDF文件
        /// </summary>
        /// <param name="inFile">待拆分PDF文件名称</param>
        static void iTextSharpPdfSplit(string inFile)
        {
            string splitPDFFileName = inFile.Substring(0, inFile.LastIndexOf(".pdf"));

            using (var reader = new PdfReader(inFile))
            {
                using (var pdfDoc = new PdfDocument(reader))
                {

                   
                    string range = "1, 4, 8";
                    var split = new ImprovedSplitter(pdfDoc, pageRange => new PdfWriter(@"D:\Extracted.pdf"));
                    var result = split.ExtractPageRange(new PageRange(range));
                    result.Close();

                }
            }



        }
       

        #endregion 函式

        #region 參數
        #endregion 參數

        #region 事件
        #endregion  事件

    }

    public class ImprovedSplitter : PdfSplitter
    {
        private Func<PageRange, PdfWriter> nextWriter;
        public ImprovedSplitter(PdfDocument pdfDocument, Func<PageRange, PdfWriter> nextWriter) : base(pdfDocument)
        {
            this.nextWriter = nextWriter;
        }

        protected override PdfWriter GetNextPdfWriter(PageRange documentPageRange)
        {
            return nextWriter.Invoke(documentPageRange);
        }
    }
}