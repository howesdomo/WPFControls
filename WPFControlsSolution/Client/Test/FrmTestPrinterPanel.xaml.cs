using Client.Components.PrinterPanel;
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
    /// Interaction logic for FrmTestPrinterPanel.xaml
    /// </summary>
    public partial class FrmTestPrinterPanel : Window
    {
        public FrmTestPrinterPanel()
        {
            InitializeComponent();
        }
    }

    public class FrmTestPrinterPanel_ViewModel : BaseViewModel
    {
        public FrmTestPrinterPanel_ViewModel()
        {
            initCommand();
        }

        public Command CMD_GetPanel0Info { get; private set; }

        public Command CMD_GetPanel1Info { get; private set; }

        void initCommand()
        {
            this.CMD_GetPanel0Info = new Command(getPanel0Info);
            this.CMD_GetPanel1Info = new Command(getPanel1Info);
        }

        void getPanel0Info()
        {
            MessageBox.Show($"{this.PrinterPanel0_SelectedPrinter.OriginValue}; {this.PrinterPanel0_SelectedPagerSize.DisplayName}");
        }

        private Printer _PrinterPanel0_SelectedPrinter;
        public Printer PrinterPanel0_SelectedPrinter
        {
            get { return _PrinterPanel0_SelectedPrinter; }
            set
            {
                _PrinterPanel0_SelectedPrinter = value;
                this.OnPropertyChanged("PrinterPanel0_SelectedPrinter");
            }
        }

        private PaperSize _PrinterPanel0_SelectedPagerSize;
        public PaperSize PrinterPanel0_SelectedPagerSize
        {
            get { return _PrinterPanel0_SelectedPagerSize; }
            set
            {
                _PrinterPanel0_SelectedPagerSize = value;
                this.OnPropertyChanged("PrinterPanel0_SelectedPagerSize");
            }
        }



        void getPanel1Info()
        {
             MessageBox.Show($"选择打印机:{this.PrinterPanel1_SelectedPrinter.OriginValue}\r\n向右偏移:{this.AlignLeft}\r\n向下偏移:{this.AlignTop}\r\n打印浓度:{this.Darkness}\r\n速度:{this.Speed}");
        }


        private Printer _PrinterPanel1_SelectedPrinter;
        public Printer PrinterPanel1_SelectedPrinter
        {
            get { return _PrinterPanel1_SelectedPrinter; }
            set
            {
                _PrinterPanel1_SelectedPrinter = value;
                this.OnPropertyChanged("PrinterPanel1_SelectedPrinter");
            }
        }


        private string _AlignLeft = "1";
        public string AlignLeft
        {
            get { return _AlignLeft; }
            set
            {
                _AlignLeft = value;
                this.OnPropertyChanged("AlignLeft");
            }
        }

        private string _AlignTop = "2";
        public string AlignTop
        {
            get { return _AlignTop; }
            set
            {
                _AlignTop = value;
                this.OnPropertyChanged("AlignTop");
            }
        }

        private string _Darkness = "20";
        public string Darkness
        {
            get { return _Darkness; }
            set
            {
                _Darkness = value;
                this.OnPropertyChanged("Darkness");
            }
        }

        private string _Speed = "10.1";
        public string Speed
        {
            get { return _Speed; }
            set
            {
                _Speed = value;
                this.OnPropertyChanged("Speed");
            }
        }


    }
}
