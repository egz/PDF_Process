
using System;
using System.Collections.Generic;
using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Utils;
using System.Printing;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PDF_Process
{
   public class PDFProcess
    {
        #region 屬性

       private String PageRang
        {
            get;
            set;
        }

        private String OriginFilePath
        {
            get;
            set;
        }


        private String OutFilePath
        {
            get;
            set;
        }

        private string FilePath
        {
            get;
            set;
        }

        #endregion 屬性

        #region 初始
      

        public PDFProcess(Dictionary<string,object>datas)
        {
            OriginFilePath = datas["infile"].ToString();
            FilePath = datas["path"].ToString();

            Dictionary<string, string> items = datas["data"] as Dictionary<string, string>;
            
            //  PdfCopyTo(OriginFilePath, OutFilePath, GetPages(PageRang));
            PdfCopyTo(items);
        }

        #endregion 初始

        #region 函式


        /// <summary> </summary>
        /// <param name="orginFile"> 來源檔案</param>
        /// <param name="toFile">    目的檔案</param>
        /// <param name="pages">     頁碼    </param>
        private void PdfCopyTo(Dictionary<string, string> items)
        {
            PdfDocument srcDoc = new PdfDocument(new PdfReader(OriginFilePath));
         //   var tasks = new List<Task>();
            foreach (KeyValuePair<string, string> dist in items)
            {
              //  tasks.Add(Task.Run(() =>
              //  {
                    string outfile = FilePath + dist.Key + ".pdf";
                    PdfDocument pdfDoc = new PdfDocument(new PdfWriter(outfile));
                    List<int> pages = GetPages(dist.Value);

                    srcDoc.CopyPagesTo(pages, pdfDoc);
                    pdfDoc.Close();
             //   }));
            }
          //  Task.WaitAll(tasks.ToArray());
            srcDoc.Close();

        }

        /// <summary> </summary>
        /// <param name="orginFile"> 來源檔案</param>
        /// <param name="toFile">    目的檔案</param>
        /// <param name="pages">     頁碼    </param>
        private  void PdfCopyTo(string orginFile, string toFile, List<int> pages)
        {
            PdfDocument srcDoc = new PdfDocument(new PdfReader(orginFile));
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(toFile));


            try
            {
                srcDoc.CopyPagesTo(pages, pdfDoc);
            }
            catch (Exception ex)
            {
             
            }

            pdfDoc.Close();
            srcDoc.Close();

        }

        /// <summary> 頁數切割 </summary>
        /// <param name="data"> 字串</param>
        /// <param name="spiltChar"> 切割字元</param>
        /// <returns></returns>
        public string[] GetPageList(string data, char[] spiltChar)
        {
            string[] pageStrList =data.Split(spiltChar);
            return pageStrList;
        }

        /// <summary> 頁碼文字轉數字 </summary>
        /// <param name="list">頁碼文字</param>
        /// <returns></returns>
        public List<int> GetPages(string list)
        {
            List<int> page = new List<int>();

            string[] scope = GetPageList(list, new char[] { ',' });
            foreach (string str in scope)
            {
                if (str.IndexOf('-') != -1)
                {
                    string[] fromTo = GetPageList(str, new char[] { '-' });
                    if (fromTo.Length < 2)
                    {

                    }

                    int from = Convert.ToInt32(fromTo[0]);
                    int to = Convert.ToInt32(fromTo[1]);

                    if (to< from) break;
                    for (int i = from; i <= to; i++)
                    {
                        page.Add(i);
                    }
                }
                else
                {
                    page.Add(Convert.ToInt32(str));
                }
            }

            return page;
        }
               

        #endregion 函式

        #region 參數
        #endregion 參數

        #region 事件
        #endregion  事件

    }

}