using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PDF_Process
{
  public  class DefaultItem
    {
        public static double DefaultFontSize
        {
            get;
            set;
        } = 20;

        public static TextBox CreateTextBox(string name)
        {
            TextBox textBox = new TextBox();
            textBox.FontSize = DefaultFontSize;
            textBox.Name = name;
            textBox.Margin = new Thickness(10);
            textBox.IsEnabled = false;
            textBox.MinWidth = 100;
            textBox.MinHeight = 22;

            return textBox;
        }

        public static CheckBox CreateCheckBox(string name)
        {
            CheckBox checkBox = new CheckBox();
            checkBox.FontSize = DefaultFontSize;
            checkBox.VerticalAlignment = VerticalAlignment.Center;
            checkBox.Name = name;
            checkBox.Margin = new Thickness(10);
            checkBox.IsChecked = false;
         
            return checkBox;
        }

        public static Button CreatButton(string name,string text)
        {
            Button button = new Button();
            button.FontSize = DefaultFontSize;
            button.Name = name;
            button.Content = text;
            button.Margin = new Thickness(10);
            button.IsEnabled = false;
            button.Width = 100;
            button.Height = 30;
            return button;
        }

        #region 事件

     
        #endregion  事件

    }
}
