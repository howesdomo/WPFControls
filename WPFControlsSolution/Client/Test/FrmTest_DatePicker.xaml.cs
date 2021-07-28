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
    /// Interaction logic for FrmTest_DatePicker.xaml
    /// </summary>
    public partial class FrmTest_DatePicker : Window
    {
        public FrmTest_DatePicker()
        {
            InitializeComponent();
        }
    }

    public class FrmTest_DatePicker_ViewModel : BaseViewModel
    {
        public FrmTest_DatePicker_ViewModel()
        {
            this.CMD_Show = new Command(Show);
        }

        private DateTime? _SelectedTime0;
        public DateTime? SelectedTime0
        {
            get { return _SelectedTime0; }
            set
            {
                _SelectedTime0 = value;
                this.OnPropertyChanged(nameof(SelectedTime0));
            }
        }

        private DateTime? _SelectedDateTime0;
        public DateTime? SelectedDateTime0
        {
            get { return _SelectedDateTime0; }
            set
            {
                _SelectedDateTime0 = value;
                this.OnPropertyChanged(nameof(SelectedDateTime0));
            }
        }


        private DateTime? _SelectedDateTime1;
        public DateTime? SelectedDateTime1
        {
            get { return _SelectedDateTime1; }
            set
            {
                _SelectedDateTime1 = value;
                this.OnPropertyChanged(nameof(SelectedDateTime1));
            }
        }


        public Command CMD_Show { get; private set; }
        void Show(object o)
        {
            var json = Util.JsonUtils.SerializeObjectWithFormatted(this);
            WPFControls.MessageBox.ShowInformation(owner: o as Window, json);
        }

    }
}
