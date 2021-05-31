using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private bool _PasswordBox1_IsFocus;
        public bool PasswordBox1_IsFocus
        {
            get { return _PasswordBox1_IsFocus; }
            set
            {
                _PasswordBox1_IsFocus = value;
                this.OnPropertyChanged(nameof(PasswordBox1_IsFocus));
            }
        }

        private bool _PasswordBox2_IsFocus;
        public bool PasswordBox2_IsFocus
        {
            get { return _PasswordBox2_IsFocus; }
            set
            {
                _PasswordBox2_IsFocus = value;
                this.OnPropertyChanged(nameof(PasswordBox2_IsFocus));
            }
        }


        private string _Password1;
        public string Password1
        {
            get { return _Password1; }
            set
            {
                _Password1 = value;
                this.OnPropertyChanged(nameof(Password1));
            }
        }


        private string _Password2;
        public string Password2
        {
            get { return _Password2; }
            set
            {
                _Password2 = value;
                this.OnPropertyChanged(nameof(Password2));
            }
        }

        public FrmTest_AttachUtils_ViewModel()
        {
            this.CMD_TextBox_Focus = new Command(TextBox_Focus);
            this.CMD_TextBox_FocusSelecAll = new Command(TextBox_FocusSelecAll);

            this.CMD_PasswordBox_Focus = new Command(PasswordBox_Focus);
            this.CMD_PasswordBox_FocusSelectAll = new Command(PasswordBox_FocusSelectAll);
        }

        public Command CMD_TextBox_Focus { get; private set; }
        void TextBox_Focus()
        {
            this.TextBox1_IsFocus = true;
        }


        public Command CMD_TextBox_FocusSelecAll { get; private set; }
        void TextBox_FocusSelecAll()
        {
            TextBox2_IsFocus = true;
        }



        public Command CMD_PasswordBox_Focus { get; private set; }
        void PasswordBox_Focus()
        {
            this.PasswordBox1_IsFocus = true;
        }

        public Command CMD_PasswordBox_FocusSelectAll { get; private set; }
        void PasswordBox_FocusSelectAll()
        {
            this.PasswordBox2_IsFocus = true;
        }


    }
}
