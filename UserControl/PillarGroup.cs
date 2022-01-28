using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PDF_Process
{
    public class PillarGroup : GroupBox
    {
        public PillarGroup(string name, string herader)
        {
            this.Name = name;
            this.Header = herader;
            InitializeDefaultSet();
            InitializeContent();

        }

        public void InitializeDefaultSet()
        {

            this.FontSize = DefaultItem.DefaultFontSize;
            this.Margin = new Thickness(10);
        }

        public void InitializeContent()
        {
        
                StackPanel stack = new StackPanel();
                stack.Orientation = Orientation.Vertical;
                this.Content = stack;
                for (int i = 1; i <= 5; i++)
                {
                    stack.Children.Add(new ItemData("Data" + i));
                }


        }
    }

}
