using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace PDF_Process
{
    public class PagePanel : UserControl
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
        {
            get;
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
            InitializeBaseGrid();
            InitializePanelDataPanel();
            InitializePanelScrollViewer();
            InitializedDataUpDwonPanel();
            InitializeButtonEnter();

        }

        private void InitializeBaseGrid()
        {
            BaseGrid = new Grid();
            BaseGrid.SetDefinition(2, 2);
            BaseGrid.RowDefinitions[1].Height = new GridLength(100);
            this.Content = BaseGrid;
        }

        private void InitializePanelDataPanel()
        {
            DataPanel = new StackPanel();
            DataPanel.Orientation = Orientation.Vertical;


            #region Add
            // 2022 先寫死，到時候需要改成可產生的
            for (int i = 1; i <= 5; i++)
            {
                GroupPages page = new GroupPages(i);
                DataPanel.Children.Add(page);
                
                for (int j = 1; j <= 5; j++)
                {
                    page.ContentPanel.Children.Add(new ItemData(j.ToString()));
                }
            }

            #endregion Add
        }

        private void InitializePanelScrollViewer()
        {
            PanelScrollViewer = new ScrollViewer();
            PanelScrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
            PanelScrollViewer.Content = DataPanel;
            BaseGrid.SetPosition(PanelScrollViewer, 0, 0);
            Grid.SetColumnSpan(PanelScrollViewer, 2);
        }

        private void InitializedDataUpDwonPanel()
        {
            StackPanel panel = new StackPanel() { Orientation = Orientation.Vertical };
            var doc = new MumberUpDown("文件數");
            var data= new MumberUpDown("柱位數");
            panel.Children.Add(doc);
            panel.Children.Add(data);
            BaseGrid.SetPosition(panel, 1,0);
        }

        private void InitializeButtonEnter()
        {
            StackPanel panel = new StackPanel() { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right };
            ButtonEnter = DefaultItem.CreatButton("ButtonEnter", "Enter");
            ButtonEnter.IsEnabled = true;
            ButtonEnter.Click += ButtonEnter_Click;

            ButtonLoadSetting = DefaultItem.CreatButton("ButtonLoad", "Load");
            ButtonLoadSetting.Click += ButtonLoadSetting_Click;

            panel.Children.Add(ButtonLoadSetting);

            panel.Children.Add(ButtonEnter);

            BaseGrid.SetPosition(panel, 1, 1);
        }

        #endregion 初始

        #region 函式

        private Dictionary<string, string> GetItemNamePages(UIElementCollection datas)
        {
            Dictionary<string, string> dist = new Dictionary<string, string>();

            foreach (ItemData items in datas)
            {
                string filename = items.PillarName.Text;
                string pages = items.PillarData.Text;
                if (items.DataCheckBox.IsChecked != true) break;
                if (!string.IsNullOrEmpty(filename) || !string.IsNullOrEmpty(pages))
                    dist.Add(filename, pages);
            }


            return dist;
        }

        #endregion 函式

        #region 參數
        #endregion 參數

        #region 事件

        /// <summary> 抓取txt檔填柱名和頁數 </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonLoadSetting_Click(object sender, RoutedEventArgs e)
        {
            // 載入寫好的txt檔
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                // 讀檔抓取，待完成
               //var file= openFileDialog.FileName;
               // var filename = openFileDialog.SafeFileName;
            }
        }

        private void ButtonEnter_Click(object sender, RoutedEventArgs e)
        {
            foreach (GroupPages pageGroup in DataPanel.Children)
            {
                string infile = pageGroup.FileText.Text;
                if (string.IsNullOrEmpty(infile)) break;
                int endlen = infile.LastIndexOf('\\');
                string path = infile.Remove(endlen + 1);

                Dictionary<string, object> root = new Dictionary<string, object>();
                Dictionary<string, string> dist = new Dictionary<string, string>();


                GetItemNamePages(pageGroup.ContentPanel.Children);

                root.Add("infile", infile);
                root.Add("path", path);
                root.Add("data", dist);
                new PDFProcess(root);
            }

        }

        #endregion  事件

    }
}
