using System;
using System.Windows;
using System.Windows.Controls;

namespace PDF_Process
{
    public static class ExtensionGrid
    {
        /// <summary> Grid行列數設定。 </summary>
        /// 
        /// <param name="row">    行數 </param>
        /// <param name="column"> 列數 </param>
        public static void SetDefinition(this Grid grid, int row = 0, int column = 0)
        {
            if (row > 0)
            {
                grid.RowDefinitions.Clear();
                for (int i = 1; i <= row; i++)
                {
                    grid.RowDefinitions.Add(new RowDefinition());
                }
            }
            if (column > 0)
            {
                grid.ColumnDefinitions.Clear();
                for (int i = 1; i <= column; i++)
                {
                    grid.ColumnDefinitions.Add(new ColumnDefinition());
                }
            }
        }

        /// <summary> 設定物件在Grid行列 </summary>
        /// 
        /// <param name="grid">   Grid </param>
        /// <param name="obj">    物件 </param>
        /// <param name="row">    行   </param>
        /// <param name="column"> 列   </param>
        /// 
        public static void SetPosition(this Grid grid, UIElement obj, int row, int column)
        {
            if (row < 0 || column < 0) throw new Exception("Error:Row or Column < 0 !");
            Grid.SetRow(obj, row);
            Grid.SetColumn(obj, column);
            grid.Children.Add(obj);
        }

        /// <summary> 設定物件在Grid列 </summary>
        /// 
        /// <param name="grid"> Grid </param>
        /// <param name="obj">  物件 </param>
        /// <param name="row">  行   </param>
        /// 
        public static void SetRow(this Grid grid, UIElement obj, int row)
        {
            if (row < 0) throw new Exception("Error:Row  < 0 !");
            grid.SetPosition(obj, row, 0);
        }

        /// <summary> 設定物件在Grid列 </summary>
        /// 
        /// <param name="grid">   Grid </param>
        /// <param name="obj">    物件 </param>
        /// <param name="column"> 列   </param>
        /// 
        public static void Setcolumn(this Grid grid, UIElement obj, int column)
        {
            if (column < 0) throw new Exception("Error: Column < 0 !");
            grid.SetPosition(obj, 0, column);
        }

    }
}
