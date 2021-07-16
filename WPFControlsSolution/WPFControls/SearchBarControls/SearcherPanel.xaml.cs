using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client.Components.SearchBarControls
{
    /// <summary>
    /// SearcherPanel.xaml 的交互逻辑
    /// </summary>
    public partial class SearcherPanel : Searcher
    {
        public SearcherPanel()
        {
            InitializeComponent();            
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            base.OnSearch(sender, e);
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            base.OnReset(sender, e);
        }

        /// <summary>
        /// 用于绑定 重置按钮 的 Command
        /// </summary>
        public void SetBtnResetCommand(ICommand cmd)
        {
            this.btnReset.Command = cmd;
        }

        /// <summary>
        /// 用于绑定 搜索按钮 的 Command
        /// </summary>
        public void SetBtnSearchCommand(ICommand cmd)
        {
            this.btnSearch.Command = cmd;
        }
    }
}
