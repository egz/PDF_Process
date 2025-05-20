
using System.Windows;
using System.Windows.Controls;


namespace PDF_Process
{
    public class ItemData_New :UserControl
    {
        #region 屬性

        public StackPanel PillarPaenl
        {
            get;
            set;
        }

        public CheckBox DataCheckBox
        {
            get;
            set;
        }

        public TextBox PillarName
        {
            get;
            set;
        }


        public TextBox PillarData
        {
            get;
            set;
        }


        #endregion 屬性


        #region 初始

        public ItemData_New(string name)
        {
            this.Name = "ItemData"+ name;
            InitializedPanel();
            InitializedItem();
            InitializedPanelContent();
           
        }


        public void InitializedPanel()
        {
            PillarPaenl = new StackPanel();
            PillarPaenl.Orientation = Orientation.Horizontal;
            PillarPaenl.HorizontalAlignment = HorizontalAlignment.Left;
            PillarPaenl.VerticalAlignment = VerticalAlignment.Center;
            PillarPaenl.Margin = new Thickness(5,2,5,2);
            this.Content = PillarPaenl;
        }

        private void InitializedItem()
        {
            DataCheckBox = DefaultItem.CreateCheckBox("ItemAllow");
            PillarName = DefaultItem.CreateTextBox("PillarName");
            PillarData = DefaultItem.CreateTextBox("PillarPage");
        }


        public void InitializedPanelContent()
        {

            PillarPaenl.Children.Add(DataCheckBox);
            PillarPaenl.Children.Add(PillarName);
            PillarPaenl.Children.Add(PillarData);

            PillarName.SetBinding(IsEnabledProperty, new System.Windows.Data.Binding("IsChecked") { Source = DataCheckBox });
            PillarData.SetBinding(IsEnabledProperty, new System.Windows.Data.Binding("IsChecked") { Source = DataCheckBox });
        }


        #endregion 初始

        #region 函式

        #endregion 函式

        #region 參數
        #endregion 參數

        #region 事件



        #endregion  事件

    }
}
