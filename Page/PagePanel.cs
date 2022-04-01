using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public Button ButtonNumberEnter
        {
            get;
            set;

        }

        public MumberUpDown DocMumber
        {
            get;
            set;
        }

        public MumberUpDown DataMumber
        {
            get;
            set;
        }

        public int DocCount
        {
            get;
            set;
        }

        public int DataCount
        {
            get;
            set;
        }

        #endregion 屬性

        #region 初始
        public PagePanel()
        {
            InitializeBaseGrid();
            InitializedDataUpDwonPanel();
            InitializePanelDataPanel();
            InitializePanelScrollViewer();
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

            //NumberText.Text
            DocCount = Convert.ToInt32(DocMumber.NumberText.Text);
            DataCount = Convert.ToInt32(DataMumber.NumberText.Text);
            for (int i = 1; i <= DocCount; i++)
            {
                GroupPages page = new GroupPages(i);
                DataPanel.Children.Add(page);

                for (int j = 1; j <= DataCount; j++)
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
            DocMumber = new MumberUpDown("文件數");
            DataMumber = new MumberUpDown("柱位數");

            panel.Children.Add(DocMumber);
            panel.Children.Add(DataMumber);
            ButtonNumberEnter = DefaultItem.CreatButton("ButtonMumberEnter", "Enter");
            ButtonNumberEnter.IsEnabled = true;
            ButtonNumberEnter.Click += ButtonNumberEnter_Click;

            StackPanel panelbutton = new StackPanel() { Orientation = Orientation.Horizontal };
            panelbutton.Children.Add(panel);
            panelbutton.Children.Add(ButtonNumberEnter);

            BaseGrid.SetPosition(panelbutton, 1, 0);
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

        private void UpdateGroup()
        {
            int docCount = DataPanel.Children.Count;

            int docMumber = Convert.ToInt32(DocMumber.NumberText.Text);
            // 目前-未來
            int remove = docMumber - docCount;

            //add
            if (remove == 0) return;
            else if (remove > 0)
            {
                for (int i = 1; i <= remove; i++)
                {
                    Task.Run(() =>
                    {
                        DataPanel.Children.Add(new GroupPages(docCount + i));
                    
                    });  
                }
            }
            // del
            else if (remove < 0)
            {
                // count數-1=最後index，然後再去掉要減掉的數量，就是從哪邊減
                int index = docCount - 1 + remove;
                DataPanel.Children.RemoveRange(index, Math.Abs(remove));

            }

        }

        private void UdateData()
        {

            int dataMumber = Convert.ToInt32(DataMumber.NumberText.Text);
            foreach (GroupPages page in DataPanel.Children)
            {
                int count = page.ContentPanel.Children.Count;
                int remove = dataMumber - count;
                if (remove == 0) continue;
                else if (remove > 0)
                {
                    for (int i = 1; i <= remove; i++)
                    {
                        Task.Run(() =>
                        {
                            page.ContentPanel.Children.Add(new ItemData((count + i).ToString()));
                        });
                    }
                }
                else if (remove < 0)
                {
                    int index = page.ContentPanel.Children.Count - 1 + remove;
                    page.ContentPanel.Children.RemoveRange(index, Math.Abs(remove));
                }
            }

        }

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
            var tasks = new List<Task>();
            foreach (GroupPages pageGroup in DataPanel.Children)
            {
                string infile = pageGroup.FileText.Text;
                if (string.IsNullOrEmpty(infile)) break;
                int endlen = infile.LastIndexOf('\\');
                string path = infile.Remove(endlen + 1);

                Dictionary<string, object> root = new Dictionary<string, object>();
                Dictionary<string, string> dist = GetItemNamePages(pageGroup.ContentPanel.Children);

                root.Add("infile", infile);
                root.Add("path", path);
                root.Add("data", dist);
                tasks.Add(Task.Run(() => { new PDFProcess(root); }));
            }
            Task.WaitAll(tasks.ToArray());
            MessageBox.Show("完成");
        }


        private void ButtonNumberEnter_Click(object sender, RoutedEventArgs e)
        {
            UpdateGroup();
            UdateData();
        }

        #endregion  事件

    }
}
