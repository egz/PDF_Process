using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace PDF_Process
{

    public class FilePathPanel : StackPanel
    {
        #region 屬性

        public CheckBox DataCheckBox
        {
            get;
            set;
        }
        public TextBox FileText
        {
            get;
            set;
        }
        public Button OpenButton
        {
            get;
            set;
        }

        public TextBox NameText
        {
            get;
            set;
        }

        public TextBox Offset
        {
            get;
            set;
        }

        /// <summary> 資料夾路徑 </summary>
        /// 
        public String Path
        {
            get;
            set;
        }

        #endregion 屬性

        #region 初始
        public FilePathPanel(string name, Visibility isVisible = Visibility.Visible, bool isBrowser = false)
        {
            DataCheckBox = DefaultItem.CreateCheckBox("ItemAllow");
            DataCheckBox.Visibility = isVisible;

            FileText = new TextBox();
            FileText.Name = "FileText";
            FileText.Width = 200;
            FileText.Height = 20;
            FileText.Margin = new Thickness(10);

            NameText = new TextBox();
            NameText.Width = 200;
            NameText.Height = 20;
            NameText.Margin = new Thickness(30, 10, 10, 10);
            NameText.Visibility = isVisible;

            Offset = new TextBox();
            Offset.Width = 30;
            Offset.Height = 20;
            Offset.Margin = new Thickness(30, 10, 10, 10);
            Offset.Visibility = isVisible;

            OpenButton = new Button();
            OpenButton.Height = 20;
            OpenButton.Content = "開啟檔案";
            if (!isBrowser)
                OpenButton.Click += OpenButton_Click;
            else
                OpenButton.Click += OpenButtonRoot_Click;

            this.Name = name;
            this.Orientation = Orientation.Horizontal;
            this.Children.Add(DataCheckBox);
            this.Children.Add(FileText);
            this.Children.Add(OpenButton);
            this.Children.Add(NameText);
            this.Children.Add(Offset);

            FileText.SetBinding(IsEnabledProperty, new System.Windows.Data.Binding("IsChecked") { Source = DataCheckBox });
            NameText.SetBinding(IsEnabledProperty, new System.Windows.Data.Binding("IsChecked") { Source = DataCheckBox });
        }

        #endregion 初始

        #region 函式
        #endregion 函式

        #region 參數
        #endregion 參數

        #region 事件
        
        /// <summary> 開啟PDF檔 </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == true)
            {
                FileText.Text = openFileDialog.FileName;
                var filename = openFileDialog.SafeFileName;

                Path = openFileDialog.FileName.Replace(filename, "");
            }


        }

        /// <summary> 開啟儲存pdf資料夾 </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenButtonRoot_Click(object sender, RoutedEventArgs e)
        {


            using (var fbd = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = fbd.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    FileText.Text = fbd.SelectedPath;
                    // var filename = openFileDialog.SafeFileName;
                    Path = fbd.SelectedPath;

                }
            }
        }

        #endregion  事件



    }

}
