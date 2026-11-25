using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PDF_Process
{
    // PDF 操作面板，管理來源列、柱位列、設定匯入與結果顯示。
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


        // 上方放來源與柱位清單，下方放輸出位置及操作按鈕。
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

        /// <summary> 初始化 PDF 輸出根資料夾選擇器。 </summary>
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

        /// <summary> 依輸入的柱位數調整清單列數。 </summary>
        /// 
        private void UpdateBayNumber()
        {
            int docCount = DataPanel.Children.Count - 1;
            int dataMumber = Convert.ToInt32(DataMumber.NumberText.Text);
            // 計算目標列數與目前列數的差額。
            int count = dataMumber - docCount;

            // 差額為零時保留目前的清單。
            if (count == 0) return;
            UpdatePanelData(DataPanel, count);
        }

        #endregion 函式

        /// <summary>
        /// 依指定的列數差額更新清單。
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="count"></param>
        // 兩種清單的最後一個子項都是數量控制列，增減時保留該列。
        private void UpdatePanelData(StackPanel panel, int count)
        {
            // 增加清單列。
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
            // 從清單末端移除多出的列。
            else if (count < 0)
            {
                int itemCount = panel.Children.Count - 1;
                panel.Children.RemoveRange(itemCount + count, -count);
            }
        }

        #region 參數
        #endregion 參數

        #region 事件

        // 先完成整份 TXT 的解析，再用有效資料更新柱位列。
        private void ButtonLoadSetting_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Title = "選擇文字檔",
                Filter = "文字檔 (*.txt)|*.txt",
                Multiselect = false
            };

            if (dialog.ShowDialog() != true)
                return;

            try
            {
                var entries = PillarSettingsParser.Parse(
                    File.ReadLines(dialog.FileName), out var invalidLines);

                if (entries.Count > 0)
                {
                    DataMumber.NumberText.Text = entries.Count.ToString();
                    UpdateBayNumber();
                    for (int i = 0; i < entries.Count; i++)
                    {
                        var pillar = (ItemData_New)DataPanel.Children[i];
                        pillar.PillarName.Text = entries[i].HID;
                        pillar.PillarData.Text = entries[i].Pages;
                        // 空白頁碼仍可顯示柱位，但預設略過匯出。
                        pillar.DataCheckBox.IsChecked = entries[i].Pages != string.Empty;
                    }
                }

                if (invalidLines.Count > 0)
                {
                    MessageBox.Show($"已略過格式錯誤的行：{string.Join("、", invalidLines)}。每行請使用 HID:頁碼。");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"無法匯入設定：{ex.Message}");
            }
        }

        // 先取得本次選取資料，再交給背景服務處理，避免重複啟動匯出。
        private async void ButtonEnter_Click(object sender, RoutedEventArgs e)
        {
            ButtonEnter.IsEnabled = false;
            Cursor = Cursors.Wait;

            try
            {
                var sources = new List<PdfSource>();
                for (int i = 0; i < OriginPanel.Children.Count - 1; i++)
                {
                    var file = (FilePathPanel)OriginPanel.Children[i];
                    if (file.DataCheckBox.IsChecked == false)
                        continue;

                    sources.Add(new PdfSource(file.NameText.Text, file.FileText.Text, file.Offset.Text));
                }

                var targets = new List<PdfExportTarget>();
                string saveRoot = SaveRootFolder.FileText.Text;
                for (int i = 0; i < DataPanel.Children.Count - 1; i++)
                {
                    var pillar = (ItemData_New)DataPanel.Children[i];
                    if (pillar.DataCheckBox.IsChecked != true)
                        continue;

                    targets.Add(new PdfExportTarget(pillar.PillarName.Text, pillar.PillarData.Text));
                }

                // 服務接收一般資料物件，背景工作不讀取 WPF 控制項。
                var results = await PdfExportService.ExportAsync(saveRoot, sources, targets);
                Cursor = null;
                ShowExportResults(results);
            }
            catch (Exception ex)
            {
                Cursor = null;
                MessageBox.Show($"無法完成匯出：{ex.Message}");
            }
            finally
            {
                Cursor = null;
                ButtonEnter.IsEnabled = true;
            }
        }

        // 逐檔顯示輸出位置及詳細原因，結果較多時可捲動查閱。
        private void ShowExportResults(IReadOnlyList<PdfExportResult> results)
        {
            if (results.Count == 0)
            {
                MessageBox.Show("請至少選取一份來源 PDF 和一個柱位。");
                return;
            }

            int exported = 0;
            var details = new System.Text.StringBuilder();
            foreach (var result in results)
            {
                if (result.Exported)
                    exported++;

                details.AppendLine($"{(result.Exported ? "已匯出" : "未匯出")}  {result.FilePath}");
                if (result.Detail != null)
                    details.AppendLine(result.Detail);
            }

            var output = new TextBox
            {
                Text = details.ToString(),
                IsReadOnly = true,
                TextWrapping = TextWrapping.Wrap,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                Margin = new Thickness(12)
            };
            var dialog = new Window
            {
                Title = $"PDF 匯出結果（成功 {exported}／共 {results.Count}）",
                Owner = Window.GetWindow(this),
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Width = 720,
                Height = 420,
                Content = output
            };
            dialog.ShowDialog();
        }

        /// <summary> PDF來源文件數變更 </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void FolderMumberEnter_Click(object sender, RoutedEventArgs e)
        {
            int docCount = OriginPanel.Children.Count - 1;
            int docMumber = Convert.ToInt32(FolderNumber.NumberText.Text);
            // 計算目標列數與目前列數的差額。
            int count = docMumber - docCount;
            // 差額為零時保留目前的清單。
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
