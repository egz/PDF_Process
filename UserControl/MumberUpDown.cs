using System;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PDF_Process
{
   public class MumberUpDown: StackPanel
    {
        #region 屬性

        public TextBox NumberText
        {
            get;
            set;
        }

        public Button Up_Button
        {
            get;
            set;
        }

        public Button Down_Button
        {
            get;
            set;
        }


        private Label LabelName
        {
            get;
            set;
        }

        private int NumberValue
        {
            get;
            set;
        }

        private string Panelname
        { 
            get;
            set;

        }

        #endregion 屬性

        #region 初始

        public MumberUpDown(string name)
        {
            Panelname = name;
            this.Orientation = Orientation.Horizontal;
            this.Margin = new Thickness(5);
            InitializedLayout();
        }

        private void InitializedLayout()
        {
            NumberText = InserTextBox("numberTxt", "5");
            Up_Button = InsertButton("Up", "+");
            Down_Button = InsertButton("Down", "-");
            LabelName = InserLabel("LabelName", Panelname);
            this.Children.Add(LabelName);
            this.Children.Add(Down_Button);
            this.Children.Add(NumberText);
            this.Children.Add(Up_Button);
        }


        #endregion 初始


        #region 函式

     

        #endregion 函式

        #region 參數

        private Button InsertButton(string name,string txt)
        {
            Button button = new Button();
            button.Name = name;
            button.Content = txt;
            button.Margin =new Thickness(2);
            button.Height = 20;
            button.Width = 20;
            button.Click += Button_Click;
            return button;

        }

        private TextBox InserTextBox(string name, string txt)
        {
            TextBox TextBox = new TextBox();
            TextBox.Name = name;
            TextBox.Text = txt;
            TextBox.Margin = new Thickness(2);
            TextBox.Height = 20;
            TextBox.Width = 20;
            TextBox.PreviewKeyUp += TextBox_PreviewKeyUp;
            return TextBox;

        }

        private Label InserLabel(string name, string txt)
        {
            Label label = new Label();
            label.Name = name;
            label.Content = txt+":";
            label.Margin = new Thickness(2,2,30,2);
            label.Width = 50;
            label.PreviewKeyUp += TextBox_PreviewKeyUp;
            return label;

        }

        #endregion 參數

        #region 事件

        private void TextBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            bool isNumber= 
                e.Key >= Key.D0 && e.Key <= Key.D9 || 
                e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || e.Key == Key.Left ||
                e.Key == Key.Right || e.Key == Key.Up || e.Key == Key.Down || e.Key == Key.Back
                || e.Key == Key.Tab || e.Key == Key.Delete; 

            e.Handled = !isNumber;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            if (button.Name== "Up")
            {
               // if (NumberText.Text == "10") return;
                NumberText.Text = (Convert.ToInt32(NumberText.Text) + 1).ToString();
            }
            else if (button.Name == "Down")
            {
                if (NumberText.Text == "0") return;
                NumberText.Text = (Convert.ToInt32(NumberText.Text) - 1).ToString();

            }
          

        }

        #endregion  事件
    }
}
