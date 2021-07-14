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

namespace Client.Components.ToolbarControls
{
    /// <summary>
    /// TODO 了解为什么在此控件下, 无法为 ToolbarButton 设置 x:Name
    /// 设置后编译会报错
    /// </summary>
    public partial class ToolbarPanelError : StackPanel
    {
        public ToolbarPanelError()
        {
            InitializeComponent();
        }
    }
}