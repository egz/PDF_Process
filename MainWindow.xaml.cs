using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PDF_Process
{
    /// <summary>
    /// 處理主視窗的初始化與內容載入。
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeMainWindow();
        }

        // 主視窗負責載入操作面板，匯入與匯出事件由面板處理。
        private void InitializeMainWindow()
        {

            this.Content = new PagePanel_New();

        }
    }
}
