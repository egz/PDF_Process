
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Documents;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Utils;


namespace PDF_Process
{
   public class PDFProcess
    {
        #region 屬性
        #endregion 屬性

        #region 初始
        public PDFProcess()
        {


                 iTextSharpPdfSplit(@"D:\chia\_all_.pdf","",0,0);
          

        }
        #endregion 初始

        #region 函式

        public void GetPages(string list)
        {
            char[] splitList = new char[] { '/' };
            string[] Scope = list.Split(splitList);
            foreach(string str in Scope)
            {
                if(str.IndexOf('-')!=-1)
                {

                }
                else
                {

                }
            }


        }


        /// <summary>  </summary>
        /// <param name="inFilePath">來源檔案路徑</param>
        ///  <param name="outFilePath">目的檔案路徑</param>
        ///  <param name="pagefrom">目的檔案</param>
        ///  <param name="pagefrom">目的檔案</param>
        ///  
        static void iTextSharpPdfSplit(string inFilePath, string outFilePath,int pagefrom,int pageto)
        {
            inFilePath = @"D:\test.pdf";
            outFilePath = @"D:\test1.pdf";
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(outFilePath));

            PdfDocument srcDoc = new PdfDocument(new PdfReader(inFilePath));

            try
            {
                srcDoc.CopyPagesTo(7,11 , pdfDoc);
            }
            catch(Exception ex)
            {
                var a = 10;
            }
            //  srcDoc.(1, 1, pdfDoc,2);

            pdfDoc.Close();
            srcDoc.Close();

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