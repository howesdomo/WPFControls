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
    /// Interaction logic for FrmTest_ToolbarControls.xaml
    /// </summary>
    public partial class FrmTest_ToolbarControls : Window
    {
        public FrmTest_ToolbarControls()
        {
            InitializeComponent();
        }
    }

    public class FrmTest_ToolbarControls_ViewModel : BaseViewModel
    {
        private string _ControlName;
        public string ControlName
        {
            get { return _ControlName; }
            set
            {
                _ControlName = value;
                this.OnPropertyChanged(nameof(ControlName));
            }
        }


        private string _CommandParamInfo;
        public string CommandParamInfo
        {
            get { return _CommandParamInfo; }
            set
            {
                _CommandParamInfo = value;
                this.OnPropertyChanged(nameof(CommandParamInfo));
            }
        }

        public Command CMD_Click { get; private set; }
        void Click()
        {
            this.ControlName = DateTime.Now.ToString("s");
        }

        public Command CMD_Click_WithParameter { get; private set; }
        void Click_WithParameter(object o)
        {
            this.ControlName = DateTime.Now.ToString("s");
            this.CommandParamInfo = (o as Client.Components.ToolbarControls.ToolbarButtonBase).Name;
        }


        public FrmTest_ToolbarControls_ViewModel()
        {
            this.CMD_Click = new Command(Click);
            this.CMD_Click_WithParameter = new Command(Click_WithParameter);
        }
    }
}
