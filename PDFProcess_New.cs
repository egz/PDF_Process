
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
using System.Windows;

namespace PDF_Process
{
   public class PDFProcess_New
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

        public PDFProcess_New(Dictionary<string, List<string>> datas, Dictionary<string, string> pillars)
        {
             var tasks = new List<Task>();
            // datas  來源圖、[檔名、offset]
            foreach (KeyValuePair<string, List<string>> dist in datas)
            {

                tasks.Add(Task.Run(() =>
                {
                    string name = dist.Key;
                    string origin = dist.Value[0];
                    string offset = dist.Value[1];

                    PdfDocument srcDoc = new PdfDocument(new PdfReader(origin));
                    try {
                
                    foreach (KeyValuePair<string, string> pillar in pillars)
                    {
                        string outfile = pillar.Key + "\\"+name + ".pdf";
                        PdfDocument pdfDoc = new PdfDocument(new PdfWriter(outfile));
                        List<int> pages = GetPages(pillar.Value, offset);

                        srcDoc.CopyPagesTo(pages, pdfDoc);
                        pdfDoc.Close();
                    }
                    
                }
                catch(Exception ex)
                    { MessageBox.Show(ex.Message); }
                    finally
                    {
                        srcDoc.Close();
                    }
                     }
                
                ));


            }
            Task.WaitAll(tasks.ToArray());
            // pillar 目的資料夾、頁數

            //  PdfCopyTo(OriginFilePath, OutFilePath, GetPages(PageRang));
            //   PdfCopyTo(items);
        }

        #endregion 初始

        #region 函式

        /// <summary> 頁數切割 </summary>
        /// <param name="data"> 字串</param>
        /// <param name="spiltChar"> 切割字元</param>
        /// <returns></returns>
        public string[] GetPageList(string data, char[] spiltChar)
        {
            string[] pageStrList =data.Split(spiltChar);
            return pageStrList;
        }

        /// <summary> 頁數轉換 </summary>
        /// 把-號的頁數轉換成數字
        /// <param name="list"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public List<int> GetPages(string list,string offset)
        {
            try
            {
           
            List<int> pages = new List<int>();
            int.TryParse(offset,out int offset_int);

            string[] scope = GetPageList(list, new char[] { ',' });
            foreach (string str in scope)
            {
                if (str.Contains('-'))
                {
                    string[] fromTo = GetPageList(str, new char[] { '-' });
                    if (fromTo.Length < 2) continue;
                 

                    int from = Convert.ToInt32(fromTo[0]) + offset_int;
                    int to = Convert.ToInt32(fromTo[1]) + offset_int;

                    if (to < from|| from<1|| to<1) continue;
                    for (int i = from; i <= to; i++)
                    {
                        pages.Add(i);
                    }
                }
                else
                {
                    int page = Convert.ToInt32(str) + offset_int;
                    if (page < 1) continue;
                    pages.Add(page);
                }
            }

            return pages;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return new List<int> { 1 };
            }
        }

        #endregion 函式

        #region 參數
        #endregion 參數

        #region 事件
        #endregion  事件

    }

}