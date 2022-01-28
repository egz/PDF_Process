using System;using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace PDF_Process
{
    /// 

    /// Encapsulates a DocumentPaginator and allows
    /// to paginate just some specific pages (a "PageRange")
    /// of the encapsulated DocumentPaginator
    ///  (c) Thomas Claudius Huber 2010 
    ///      http://www.thomasclaudiushuber.com
    /// 

    public class PageRangeDocumentPaginator : DocumentPaginator
    {

        private int _startIndex;
        private int _endIndex;
        private DocumentPaginator _paginator;
        public PageRangeDocumentPaginator(
          DocumentPaginator paginator,
          PageRange pageRange)
        {
            _startIndex = pageRange.PageFrom - 1;
            _endIndex = pageRange.PageTo - 1;
            _paginator = paginator;

            // Adjust the _endIndex
            _endIndex = Math.Min(_endIndex, _paginator.PageCount - 1);
        }
        public override DocumentPage GetPage(int pageNumber)
        {
            // Just return the page from the original
            // paginator by using the "startIndex"
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
        public void test()
        {
            var dlg = new PrintDialog();

            // Allow the user to select a PageRange
            dlg.UserPageRangeEnabled = true;

            if (dlg.ShowDialog() == true)
            {
                //DocumentPaginator paginator =
                //  _fixedDocument.DocumentPaginator;

                if (dlg.PageRangeSelection == PageRangeSelection.UserPages)
                {
                    var c = 10;
                    //paginator = new PageRangeDocumentPaginator(
                    //                 _fixedDocument.DocumentPaginator,
                    //                 dlg.PageRange);
                }

         //       dlg.PrintDocument(paginator, "Yes, it works");
            }
        }
    }




    //void PrintButtonClick(object sender, RoutedEventArgs e)
    //{
    //    var dlg = new PrintDialog();

    //    // Allow the user to select a PageRange
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
