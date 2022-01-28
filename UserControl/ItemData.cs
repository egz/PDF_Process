using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;


namespace PDF_Process
{
    public class ItemData : System.Windows.Controls.UserControl
    {
        #region 屬性

        public StackPanel PillarPaenl
        {
            get;
            set;
        }

        public System.Windows.Controls.CheckBox DataCheckBox
        {
            get;
            set;
        }

        public System.Windows.Controls.TextBox PillarName
        {
            get;
            set;
        }

        public System.Windows.Controls.TextBox PillarData
        {
            get;
            set;
        }

        public System.Windows.Controls.TextBox FileOrgin
        {
            get;
            set;
        }
        public System.Windows.Controls.Button FileButton
        {
            get;
            set;
        }

        #endregion 屬性


        #region 初始

        public ItemData(string name)
        {
            this.Name = name;
            InitializedPanel();
            InitializedItem();
            InitializedPanelContent();
           
        }


        public void InitializedPanel()
        {
            PillarPaenl = new StackPanel();
            PillarPaenl.Orientation = System.Windows.Controls.Orientation.Horizontal;
            PillarPaenl.Margin = new Thickness(10);
            this.Content = PillarPaenl;
        }

        private void InitializedItem()
        {
            DataCheckBox = DefaultItem.CreateCheckBox("ItemAllow");
            PillarName = DefaultItem.CreateTextBox("PillarName");
            PillarData = DefaultItem.CreateTextBox("PillarData");
            FileOrgin = DefaultItem.CreateTextBox("FileOrgin");
            FileButton = DefaultItem.CreatButton("FileButton","Open");
        }


        public void InitializedPanelContent()
        {

                PillarPaenl.Children.Add(DataCheckBox);
                PillarPaenl.Children.Add(PillarName);
                PillarPaenl.Children.Add(PillarData);
                PillarPaenl.Children.Add(FileOrgin);
                PillarPaenl.Children.Add(FileButton);

                PillarName.SetBinding(IsEnabledProperty, new System.Windows.Data.Binding("IsChecked") { Source = DataCheckBox });
                PillarData.SetBinding(IsEnabledProperty, new System.Windows.Data.Binding("IsChecked") { Source = DataCheckBox });
                FileOrgin.SetBinding(IsEnabledProperty, new System.Windows.Data.Binding("IsChecked") { Source = DataCheckBox });
                FileButton.SetBinding(IsEnabledProperty, new System.Windows.Data.Binding("IsChecked") { Source = DataCheckBox });
                FileButton.Click += FileButton_Click;

   
        }


        #endregion 初始

        #region 函式

        #endregion 函式

        #region 參數
        #endregion 參數

        #region 事件

        private void FileButton_Click(object sender, RoutedEventArgs e)
        {

   
        }


        #endregion  事件

    }
}
