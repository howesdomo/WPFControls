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
using System.Windows.Shapes;

namespace Client.Test
{
    /// <summary>
    /// Interaction logic for FrmTest_Converter.xaml
    /// </summary>
    public partial class FrmTest_Converter : Window
    {
        public FrmTest_Converter()
        {
            InitializeComponent();
        }
    }

    public class FrmTest_Converter_ViewModel : BaseViewModel
    {
        public object MyData { get; set; }

        public bool IsTrue { get; set; } = true;

        public bool IsFalse { get; set; } = false;
    }
}
