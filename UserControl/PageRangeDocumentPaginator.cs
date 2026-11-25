using System;using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace PDF_Process
{
    /// 

    /// 包裝既有的 DocumentPaginator，
    /// 以 PageRange 指定要取用的頁面，
    /// 並依所選範圍提供分頁。
    ///  (c) Thomas Claudius Huber 2010 
    ///      http://www.thomasclaudiushuber.com
    /// 

    // 保留的 WPF 列印分頁工具。主畫面的 PDF 匯出流程目前未呼叫此類別。
    public class PageRangeDocumentPaginator : DocumentPaginator
    {

        private int _startIndex;
        private int _endIndex;
        private DocumentPaginator _paginator;
        public PageRangeDocumentPaginator(
          DocumentPaginator paginator,
          PageRange pageRange)
        {
            // 列印對話框的頁次從 1 開始，DocumentPaginator 索引從 0 開始。
            _startIndex = pageRange.PageFrom - 1;
            _endIndex = pageRange.PageTo - 1;
            _paginator = paginator;

            // 結束索引最多只能到原始文件的最後一頁。
            _endIndex = Math.Min(_endIndex, _paginator.PageCount - 1);
        }
        public override DocumentPage GetPage(int pageNumber)
        {
            // 將所選範圍的頁次加上起始索引，
            // 再向原始分頁器取得對應頁面。
            return _paginator.GetPage(pageNumber + _startIndex);
        }

        public override bool IsPageCountValid
        {
            get { return true; }
        }

        public override int PageCount
        {
            get
            {
                if (_startIndex > _paginator.PageCount - 1)
                    return 0;
                if (_startIndex > _endIndex)
                    return 0;

                return _endIndex - _startIndex + 1;
            }
        }

        public override Size PageSize
        {
            get { return _paginator.PageSize; }
            set { _paginator.PageSize = value; }
        }

        public override IDocumentPaginatorSource Source
        {
            get { return _paginator.Source; }
        }

        //  public override Size PageSize { set => throw new NotImplementedException(); }
    
    }




    //void PrintButtonClick(object sender, RoutedEventArgs e)
    //{
    //    var dlg = new PrintDialog();

    //    // 允許使用者選取列印頁碼範圍。
    //    dlg.UserPageRangeEnabled = true;

    //    if (dlg.ShowDialog() == true)
    //    {
    //        DocumentPaginator paginator =
    //          _fixedDocument.DocumentPaginator;

    //        if (dlg.PageRangeSelection == PageRangeSelection.UserPages)
    //        {
    //            paginator = new PageRangeDocumentPaginator(
    //                             _fixedDocument.DocumentPaginator,
    //                             dlg.PageRange);
    //        }

    //        dlg.PrintDocument(paginator, "Yes, it works");
    //    }
    //}
}
