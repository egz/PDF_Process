using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PDF_Process
{
    public class PagePanel_New : UserControl
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

        // public StackPanel OriginPanel
        public StackPanel OriginPanel
        {
            get;
            set;

        }

        public StackPanel DataPanel
        {
            get;
            set;
        }

        public FilePathPanel SaveRootFolder
        {
            get;
            set;

        }

        public MumberUpDown FolderNumber
        {
            get;
            set;
        }
       

        public MumberUpDown DataMumber
        {
            get;
            set;
        }

        #region 功能按鈕

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

        #endregion 功能按鈕

        #region number button
        public Button ButtonNumberFolerEnter
        {
            get;
            set;

        }

        public Button ButtonNumberDataEnter
        {
            get;
            set;

        }

        #endregion number button

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

        public PagePanel_New()
        {
            InitializeBaseGrid();
            InitializeOriginPanel();
            InitializeDataPanel();
            InitializeSaveRootFolderPanel();
            InitializeFeatures();


        }


        private void InitializeBaseGrid()
        {
            BaseGrid = new Grid();
            BaseGrid.SetDefinition(2, 2);
            BaseGrid.RowDefinitions[1].Height = new GridLength(100);
            this.Content = BaseGrid;
        }

        /// <summary> 總圖PDF板塊 </summary>
        /// 
        private void InitializeOriginPanel()
        {

            FolderNumber = new MumberUpDown("文件數");
            ButtonNumberFolerEnter = DefaultItem.CreatButton("FolderMumberEnter", "Enter");
            ButtonNumberFolerEnter.Click += FolderMumberEnter_Click; ;
            StackPanel panel = new StackPanel() { Orientation = Orientation.Horizontal, VerticalAlignment = VerticalAlignment.Bottom };
            panel.Children.Add(FolderNumber);
            panel.Children.Add(ButtonNumberFolerEnter);

            OriginPanel = new StackPanel() { Orientation = Orientation.Vertical, Name = "OriginPanel" };
            ScrollViewer scrollViewer = new ScrollViewer() { VerticalScrollBarVisibility=ScrollBarVisibility.Auto,HorizontalScrollBarVisibility=ScrollBarVisibility.Auto};
            scrollViewer.Content = OriginPanel;
            OriginPanel.Children.Add(panel);
          
            BaseGrid.SetPosition(scrollViewer, 0, 0);
        }

        /// <summary> 資料PDF板塊 </summary>
        /// 
        private void InitializeDataPanel()
        {

            DataMumber = new MumberUpDown("柱位數");
            ButtonNumberDataEnter = DefaultItem.CreatButton("DataMumberEnter", "Enter");
            ButtonNumberDataEnter.Click += ButtonNumbeEnter_Click;
            StackPanel panel = new StackPanel() { Orientation = Orientation.Horizontal, VerticalAlignment = VerticalAlignment.Bottom };
            panel.Children.Add(DataMumber);
            panel.Children.Add(ButtonNumberDataEnter);

            ScrollViewer scrollViewer = new ScrollViewer() { VerticalScrollBarVisibility = ScrollBarVisibility.Auto, HorizontalScrollBarVisibility = ScrollBarVisibility.Auto };
         
            DataPanel = new StackPanel() { Orientation = Orientation.Vertical, Name = "DataPanel"};
            DataPanel.Children.Add(panel);
            scrollViewer.Content = DataPanel;
            BaseGrid.SetPosition(scrollViewer, 0, 1);

        }

        /// <summary> DPF Path </summary>
        /// 
        private void InitializeSaveRootFolderPanel()
        {
            SaveRootFolder = new FilePathPanel("RootFoloder", Visibility.Collapsed,true);
            BaseGrid.SetPosition(SaveRootFolder, 1, 0);

        }

        /// <summary> 功能區 </summary>
        /// 
        private void InitializeFeatures()
        {
            StackPanel panel = new StackPanel() { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right };
            ButtonEnter = DefaultItem.CreatButton("ButtonEnter", "Enter");
            ButtonEnter.Click += ButtonEnter_Click;

            ButtonLoadSetting = DefaultItem.CreatButton("ButtonLoad", "Load");
            ButtonLoadSetting.Click += ButtonLoadSetting_Click;

            panel.Children.Add(ButtonLoadSetting);
            panel.Children.Add(ButtonEnter);

            BaseGrid.SetPosition(panel, 1, 1);
        }


        #endregion 初始

        #region 函式

        /// <summary> 變更Bay數量 </summary>
        /// 
        private void UpdateBayNumber()
        {
            int docCount = DataPanel.Children.Count - 1;
            int dataMumber = Convert.ToInt32(DataMumber.NumberText.Text);
            // 目前-未來
            int count = dataMumber - docCount;

            //update
            if (count == 0) return;
            UpdatePanelData(DataPanel, count);
        }

        #endregion 函式

        /// <summary>
        /// 更新Panel資料
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="count"></param>
        private void UpdatePanelData(StackPanel panel, int count)
        {
            //add
            if (count > 0)
            {
                for (int i = count; i >= 1; i--)
                {
                    if (panel.Name == "OriginPanel")
                        panel.Children.Insert(0, new FilePathPanel("File"));
                    else
                        panel.Children.Insert(0, new ItemData_New("File"));

                }
            }
            // del
            else if (count < 0)
            {
               
                int docCount = DataPanel.Children.Count - 1;
                // count數-1=最後index，然後再去掉要減掉的數量，就是從哪邊減
                int index = Math.Abs(docCount  + count - 1);
                panel.Children.RemoveRange(index, Math.Abs(count));

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
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "選擇文字檔",
                Filter = "文字檔 (*.txt)|*.txt",   // 限定副檔名為 .txt
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == true)
            {
                // 讀檔抓取，待完成
                string filePath = openFileDialog.FileName;
                try
                {
                    string[] lines = File.ReadAllLines(filePath);
                    if (lines.Length == 0) return;
                    List<TextDataModel> textData = new();
                    foreach (var line in lines)
                    {
                        var data = line.Trim();
                        if (string.IsNullOrEmpty(data) || !data.Contains(":")) continue;
                        string[] splitData = data.Split(':');
                        if (splitData.Length==2)
                        {
                            textData.Add(new TextDataModel(splitData[0].Trim(), splitData[1].Trim()));
                        }
                        
                    }
                    DataMumber.NumberText.Text= textData.Count.ToString();
                    UpdateBayNumber();
                    int dirCount = DataPanel.Children.Count - 1;
                    for(int i = 0; i < dirCount; i++)
                    {
                        ItemData_New Pillar = (DataPanel.Children[i] as ItemData_New);
                        Pillar.DataCheckBox.IsChecked = true;
                        Pillar.PillarName.Text = textData[i].HID;
                        Pillar.PillarData.Text = textData[i].Pages;
                   
                    }
             
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    // var filename = openFileDialog.SafeFileName;
                }
            }
        }

        /// <summary> 進入PDF處理(儲存) </summary>
        /// 
        private void ButtonEnter_Click(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            // 1.先抓root資料夾
           
            Dictionary<string, List<string>> originData = new Dictionary<string, List<string>>();
            Dictionary<string, string> numberData = new Dictionary<string, string>();
            int originCount = OriginPanel.Children.Count - 1;
          
            // 來源圖、[檔名、offset]
            for (int i = 0; i < originCount; i++)
            {
                FilePathPanel fileData = OriginPanel.Children[i] as FilePathPanel;
                if (fileData.DataCheckBox.IsChecked == false) continue;
                originData.Add(fileData.NameText.Text, new List<string>() { fileData.FileText.Text, fileData.Offset.Text });
            }

            // 2.抓 PillarName子資料夾 沒有->創，有->跳過
            // 去掉最後一個number item
            // 目的資料夾、頁數

            string savePath = SaveRootFolder.FileText.Text + "\\";
            int dirCount = DataPanel.Children.Count - 1;
            for (int i = 0; i < dirCount; i++)
            {
                ItemData_New Pillar = (DataPanel.Children[i] as ItemData_New);
                if (Pillar.DataCheckBox.IsChecked != true) continue;
                //string name = savePath.Replace(".pdf", "") + "\\" + Pillar.PillarName.Text;
                string name = savePath  + Pillar.PillarName.Text;
                numberData.Add(name, Pillar.PillarData.Text);
                if (!Directory.Exists(name))
                {
                    // creat Dir
                    Directory.CreateDirectory(name);
                }
            }

            new PDFProcess_New(originData, numberData);
            Cursor = null;
            MessageBox.Show("完成");
        }

        /// <summary> PDF來源文件數變更 </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void FolderMumberEnter_Click(object sender, RoutedEventArgs e)
        {
            int docCount = OriginPanel.Children.Count - 1;
            int docMumber = Convert.ToInt32(FolderNumber.NumberText.Text);
            // 目前-未來
            int count = docMumber - docCount;
            //update
            if (count == 0) return;
            UpdatePanelData(OriginPanel, count);
        }

        /// <summary> 柱位數變更 </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonNumbeEnter_Click(object sender, RoutedEventArgs e)
        {
            UpdateBayNumber();
        }



        #endregion  事件

    }
}
