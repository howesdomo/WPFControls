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
    /// Interaction logic for FrmTest_PasswordBoxBinding.xaml
    /// </summary>
    public partial class FrmTest_PasswordBoxBinding : Window
    {
        public FrmTest_PasswordBoxBinding()
        {
            InitializeComponent();
        }
    }

    public class FrmTest_PasswordBoxBinding_ViewModel : BaseViewModel
    {
        public FrmTest_PasswordBoxBinding_ViewModel()
        {
            this.CMD_Show = new Command((objWindow) =>
            {
                if (objWindow is FrmTest_PasswordBoxBinding owner)
                {
                    string msg = $"P1:{this.P1}\r\nP2:{this.P2}\r\nP3:{this.P3}\r\nP4:{this.P4}";
                    MessageBox.Show(owner, msg);
                }
            });
        }

        private string _P1;
        public string P1
        {
            get { return _P1; }
            set
            {
                _P1 = value;
                this.OnPropertyChanged("P1");
            }
        }


        private string _P2 = "I'm P2.";
        public string P2
        {
            get { return _P2; }
            set
            {
                _P2 = value;
                this.OnPropertyChanged("P2");
            }
        }

        private string _P3;
        public string P3
        {
            get { return _P3; }
            set
            {
                _P3 = value;
                this.OnPropertyChanged("P3");
            }
        }


        private string _P4 = "P4 is me";
        public string P4
        {
            get { return _P4; }
            set
            {
                _P4 = value;
                this.OnPropertyChanged("P4");
            }
        }


        public Command CMD_Show { get; private set; }

    }
}