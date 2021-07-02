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
            initData_PrinterPanel();
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

















        void getPanel1Info(object objWindow)
        {
            string msg = $"控件数据验证结果:{this.UcPrinterPanelZebra_IsValidated}\r\n选择打印机:{this.UcPrinterPanelZebra_SelectedPrinter?.OriginValue}\r\n向右偏移:{this.UcPrinterPanelZebra_AlignLeft}\r\n向下偏移:{this.UcPrinterPanelZebra_AlignTop}\r\n打印浓度:{this.UcPrinterPanelZebra_Darkness}\r\n速度:{this.UcPrinterPanelZebra_Speed}";
            WPFControls.MessageBox.ShowInformation(objWindow as Window, msg);
        }

        void initData_PrinterPanel()
        {
            //// 可以在后台指定打印机列表只显示哪些打印机；也可以不设置控件会帮你设置
            //this.UcPrinterPanelZebra_PrinterList = Client.Components.PrinterPanel.PrinterUtils.GetPrinterList(isContainUpdateListItem: true)
            //    // .Skip(2)
            //    .ToList();

            //// 1. 设置选中默认打印机；也可以不设置，控件会帮你设置默认打印机。
            //// this.UcPrinterPanelZebra_SelectedPrinter = this.UcPrinterPanelZebra_PrinterList.FirstOrDefault(i => i.DisplayName == Client.Components.PrinterPanel.PrinterUtils.GetDefaultPrinterName());

            //// 2. 假设这是读取配置文件的打印机配置
            //string configPrinterName = "Fax";
            //this.UcPrinterPanelZebra_SelectedPrinter = this.UcPrinterPanelZebra_PrinterList.FirstOrDefault(i => i.DisplayName == configPrinterName); 
        }

        private bool _UcPrinterPanelZebra_IsValidated;
        public bool UcPrinterPanelZebra_IsValidated
        {
            get { return _UcPrinterPanelZebra_IsValidated; }
            set
            {
                _UcPrinterPanelZebra_IsValidated = value;
                this.OnPropertyChanged(nameof(UcPrinterPanelZebra_IsValidated));
            }
        }


        private List<Client.Components.PrinterPanel.Printer> _UcPrinterPanelZebra_PrinterList;
        public List<Client.Components.PrinterPanel.Printer> UcPrinterPanelZebra_PrinterList
        {
            get { return _UcPrinterPanelZebra_PrinterList; }
            set
            {
                _UcPrinterPanelZebra_PrinterList = value;
                this.OnPropertyChanged("UcPrinterPanelZebra_PrinterList");
            }
        }


        private Client.Components.PrinterPanel.Printer _UcPrinterPanelZebra_SelectedPrinter;
        public Client.Components.PrinterPanel.Printer UcPrinterPanelZebra_SelectedPrinter
        {
            get { return _UcPrinterPanelZebra_SelectedPrinter; }
            set
            {
                _UcPrinterPanelZebra_SelectedPrinter = value;
                this.OnPropertyChanged("UcPrinterPanelZebra_SelectedPrinter");
            }
        }


        private string _UcPrinterPanelZebra_AlignLeft = "0";
        public string UcPrinterPanelZebra_AlignLeft
        {
            get { return _UcPrinterPanelZebra_AlignLeft; }
            set
            {
                _UcPrinterPanelZebra_AlignLeft = value;
                this.OnPropertyChanged("UcPrinterPanelZebra_AlignLeft");
            }
        }

        private string _UcPrinterPanelZebra_AlignTop = "0";
        public string UcPrinterPanelZebra_AlignTop
        {
            get { return _UcPrinterPanelZebra_AlignTop; }
            set
            {
                _UcPrinterPanelZebra_AlignTop = value;
                this.OnPropertyChanged("UcPrinterPanelZebra_AlignTop");
            }
        }

        private string _UcPrinterPanelZebra_Darkness = "20";
        public string UcPrinterPanelZebra_Darkness
        {
            get { return _UcPrinterPanelZebra_Darkness; }
            set
            {
                _UcPrinterPanelZebra_Darkness = value;
                this.OnPropertyChanged("UcPrinterPanelZebra_Darkness");
            }
        }

        private string _UcPrinterPanelZebra_Speed = "10.1";
        public string UcPrinterPanelZebra_Speed
        {
            get { return _UcPrinterPanelZebra_Speed; }
            set
            {
                _UcPrinterPanelZebra_Speed = value;
                this.OnPropertyChanged("UcPrinterPanelZebra_Speed");
            }
        }



    }
}
