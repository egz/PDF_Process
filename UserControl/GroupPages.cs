using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;


namespace PDF_Process
{
    public class GroupPages : GroupBox
    {
        #region 屬性

        public TextBox FileText
        {
            get;
            set;
        }

        private Button OpenButton
        {
            get;
            set;
        }

        public StackPanel ContentPanel
        {
            get;
            set;
        }

        #endregion 屬性

        #region 初始

        public GroupPages(int i)
        {
            this.Name = "PageGroupPages" + i;
  
            InitializedHearder();
            InitializedContent();
        }


        private void InitializedHearder()
        {
            FileText = new TextBox();
            FileText.Name = "textOriginFile";
            FileText.Width = 200;
            FileText.Height = 20;
            FileText.Margin = new Thickness(10);
            OpenButton = new Button();
            OpenButton.Height = 20;
            OpenButton.Content = "開啟檔案";
            OpenButton.Click += OpenButton_Click;

            StackPanel hearderstack = new StackPanel();
            hearderstack.Orientation = Orientation.Horizontal;
            hearderstack.Children.Add(FileText);
            hearderstack.Children.Add(OpenButton);
            this.Header = hearderstack;

        }

        private void InitializedContent()
        {
            ContentPanel = new StackPanel();
            ContentPanel.Name = "GroupContent";
            ContentPanel.Orientation = Orientation.Vertical;
          
            this.Content = ContentPanel;
        }

        #endregion 初始

        #region 函式
        #endregion 函式

        #region 參數
        #endregion 參數

        #region 事件
        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == true)
            {
                FileText.Text = openFileDialog.FileName;
                var filename = openFileDialog.SafeFileName;
                var path = openFileDialog.FileName.Replace(filename, "");
            }
        }

        #endregion  事件

    }
}
