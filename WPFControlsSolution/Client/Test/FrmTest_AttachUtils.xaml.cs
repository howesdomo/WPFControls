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
    /// Interaction logic for FrmTest_AttachUtils.xaml
    /// </summary>
    public partial class FrmTest_AttachUtils : Window
    {
        public FrmTest_AttachUtils()
        {
            InitializeComponent();
        }
    }

    public class FrmTest_AttachUtils_ViewModel : BaseViewModel
    {
        private bool _TextBox1_IsFocus;
        public bool TextBox1_IsFocus
        {
            get { return _TextBox1_IsFocus; }
            set
            {
                _TextBox1_IsFocus = value;
                this.OnPropertyChanged(nameof(TextBox1_IsFocus));
            }
        }


        private bool _TextBox2_IsFocus;
        public bool TextBox2_IsFocus
        {
            get { return _TextBox2_IsFocus; }
            set
            {
                _TextBox2_IsFocus = value;
                this.OnPropertyChanged(nameof(TextBox2_IsFocus));
            }
        }


        private string _Password;
        public string Password
        {
            get { return _Password; }
            set
            {
                _Password = value;
                this.OnPropertyChanged(nameof(Password));
            }
        }


        public FrmTest_AttachUtils_ViewModel()
        {

        }

        
    }
}
