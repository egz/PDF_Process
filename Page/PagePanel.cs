using iText.Kernel.Pdf;
using iText.Kernel.Utils;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace PDF_Process
{
   public class PagePanel:UserControl
    {
        #region 屬性

        public Grid BaseGrid
        {
            get;
            set;
        }

        public ScrollViewer PanelScrollViewer
        {
            get;
            set;
        }

        public StackPanel DataPanel
        { get;
            set;

        }

        public Button ButtonEnter
        {
            get;
            set;
        }

        public Button ButtonLoadSetting
        {
            get;
            set;
        }


        #endregion 屬性

        #region 初始
        public PagePanel()
        {
            //InitializeBaseGrid();
            //InitializePanelDataPanel();
            //InitializePanelScrollViewer();
            //InitializeButtonEnter();
            var a =new  PDFProcess();
          
        }

        

        private void InitializeBaseGrid()
        {
            BaseGrid = new Grid();
            BaseGrid.SetDefinition(2, 1);
            BaseGrid.RowDefinitions[1].Height = new GridLength(50);
            this.Content = BaseGrid;
        }

        private void InitializePanelDataPanel()
        {
            DataPanel = new StackPanel();
            DataPanel.Orientation = Orientation.Vertical;
           

            #region Add
            for (int i=1;i<=5;i++)
            {
                DataPanel.Children.Add(new PillarGroup("PillarGroup" + i, "PillarGroup" + i));
            }

            #endregion Add
        }

        private void InitializePanelScrollViewer()
        {
            PanelScrollViewer = new ScrollViewer();
            PanelScrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
            PanelScrollViewer.Content = DataPanel;
            BaseGrid.SetPosition(PanelScrollViewer, 0, 0);
        }

        private void InitializeButtonEnter()
        {
            StackPanel panel = new StackPanel() { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right };
            ButtonEnter = DefaultItem.CreatButton("ButtonEnter","Enter");
            ButtonEnter.Click += ButtonEnter_Click;

            ButtonLoadSetting = DefaultItem.CreatButton("ButtonLoad", "Load");
            ButtonLoadSetting.Click += ButtonLoadSetting_Click;

            panel.Children.Add(ButtonLoadSetting);

            panel.Children.Add(ButtonEnter);

            BaseGrid.SetPosition(panel, 1, 0);
        }



        #endregion 初始

        #region 函式



        #endregion 函式

        #region 參數
        #endregion 參數

        #region 事件

        private void ButtonLoadSetting_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonEnter_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion  事件

    }
}
