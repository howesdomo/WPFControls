using Models;
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
    /// Interaction logic for FrmTest_SearchBarControls.xaml
    /// </summary>
    public partial class FrmTest_SearchBarControls : Window
    {
        public FrmTest_SearchBarControls()
        {
            InitializeComponent();
        }
    }

    public class FrmTest_SearchBarControls_ViewModel : BaseViewModel
    {
        public FrmTest_SearchBarControls_ViewModel()
        {
            initCMD();
        }

        void initCMD()
        {
            this.CMD_Reset = new Command(Reset);
            this.CMD_Search = new Command(Search);
        }

        public Command CMD_Reset { get; private set; }
        void Reset(object objWindow)
        {
            if (objWindow != null)
            {
                System.Diagnostics.Debugger.Break();
            }
        }

        public Command CMD_Search { get; private set; }
        void Search(object objWindow)
        {
            if (objWindow is Window owner)
            {
                WPFControls.MessageBox.ShowInformation(owner, Util.JsonUtils.SerializeObjectWithFormatted(this.SearchArgs));
            }
        }


        private SearchArgs _SearchArgs = new SearchArgs();
        public SearchArgs SearchArgs
        {
            get { return _SearchArgs; }
            set
            {
                _SearchArgs = value;
                this.OnPropertyChanged(nameof(SearchArgs));
            }
        }

    }
}
