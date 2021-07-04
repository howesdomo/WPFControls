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

        void getPanel0Info(object objWindow)
        {
            string msg = $"控件数据验证结果:{this.UcPrinterPanel_IsValidated}\r\n选择打印机:{this.UcPrinterPanel_SelectedPrinter?.DisplayName}\r\n选择纸张:{this.UcPrinterPanel_SelectedPaperSize?.DisplayName}";
            WPFControls.MessageBox.ShowInformationDialog(objWindow as Window, msg);
        }
        
        #region UcPrinterPanelZebra 代码示例
        
        private bool _UcPrinterPanel_IsValidated;
        public bool UcPrinterPanel_IsValidated
        {
            get { return _UcPrinterPanel_IsValidated; }
            set
            {
                _UcPrinterPanel_IsValidated = value;
                this.OnPropertyChanged(nameof(UcPrinterPanel_IsValidated));
            }
        }

        private Printer _UcPrinterPanel_SelectedPrinter;
        public Printer UcPrinterPanel_SelectedPrinter
        {
            get { return _UcPrinterPanel_SelectedPrinter; }
            set
            {
                _UcPrinterPanel_SelectedPrinter = value;
                this.OnPropertyChanged(nameof(UcPrinterPanel_SelectedPrinter));
            }
        }

        private PaperSize _UcPrinterPanel_SelectedPaperSize;
        public PaperSize UcPrinterPanel_SelectedPaperSize
        {
            get { return _UcPrinterPanel_SelectedPaperSize; }
            set
            {
                _UcPrinterPanel_SelectedPaperSize = value;
                this.OnPropertyChanged(nameof(UcPrinterPanel_SelectedPaperSize));
            }
        }

        #endregion



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

        private PaperSize _PrinterPanel1_SelectedPagerSize;
        public PaperSize PrinterPanel1_SelectedPagerSize
        {
            get { return _PrinterPanel1_SelectedPagerSize; }
            set
            {
                _PrinterPanel1_SelectedPagerSize = value;
                this.OnPropertyChanged("PrinterPanel1_SelectedPagerSize");
            }
        }



        void getPanel1Info(object objWindow)
        {
            string msg = $"控件数据验证结果:{this.UcPrinterPanelZebra_IsValidated}\r\n选择打印机:{this.UcPrinterPanelZebra_SelectedPrinter?.OriginValue}\r\n向右偏移:{this.UcPrinterPanelZebra_AlignLeft}\r\n向下偏移:{this.UcPrinterPanelZebra_AlignTop}\r\n打印浓度:{this.UcPrinterPanelZebra_Darkness}\r\n速度:{this.UcPrinterPanelZebra_Speed}";            
            WPFControls.MessageBox.ShowInformationDialog(objWindow as Window, msg);
        }

        #region UcPrinterPanelZebra 代码示例

        void initData_PrinterPanel(string configPrinterName = "")
        {
            //// 可以在后台指定打印机列表只显示哪些打印机；也可以不设置控件会帮你设置
            //this.UcPrinterPanelZebra_PrinterList = Client.Components.PrinterPanel.PrinterUtils.GetPrinterList(isContainUpdateListItem: true)
            //    .Skip(2) // 自定义打印列表
            //    .ToList();

            //// 1. 设置选中默认打印机；也可以不设置，控件会帮你设置默认打印机。
            //// this.UcPrinterPanelZebra_SelectedPrinter = this.UcPrinterPanelZebra_PrinterList.FirstOrDefault(i => i.DisplayName == Client.Components.PrinterPanel.PrinterUtils.GetDefaultPrinterName());

            //// 2. 假设这是读取配置文件的打印机配置            
            //configPrinterName = "Fax";
            //var matchPrinter = this.UcPrinterPanelZebra_PrinterList.FirstOrDefault(i => i.DisplayName == configPrinterName);
            //if (matchPrinter != null)
            //{
            //    this.UcPrinterPanelZebra_SelectedPrinter = matchPrinter;
            //}
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

        const string ZebraPrinter_DefaultPrinter = "斑马默认打印机";

        const string ZebraPrinter_Speed_Key = "斑马打印机打印速度";

        const string ZebraPrinter_Darkness_Key = "斑马打印机打印浓度";

        const string ZebraPrinter_AlignTop_Key = "斑马打印机向下偏移量";

        const string ZebraPrinter_AlignLeft_Key = "斑马打印机向右偏移量";


        // 读取配置
        void readConfig()
        {
            try
            {
                string temp = Common.ConfigHandler.GetValueFromNativeSettings(ZebraPrinter_DefaultPrinter);
                this.UcPrinterPanelZebra_Speed = temp;
                initData_PrinterPanel(configPrinterName: temp);
            }
            catch (Exception)
            {

            }

            try
            {
                string temp = Common.ConfigHandler.GetValueFromNativeSettings(ZebraPrinter_Speed_Key);
                this.UcPrinterPanelZebra_Speed = temp;
            }
            catch (Exception)
            {

            }

            try
            {
                string temp = Common.ConfigHandler.GetValueFromNativeSettings(ZebraPrinter_Darkness_Key);
                this.UcPrinterPanelZebra_Darkness = temp;
            }
            catch (Exception)
            {

            }

            try
            {
                string temp = Common.ConfigHandler.GetValueFromNativeSettings(ZebraPrinter_AlignLeft_Key);
                this.UcPrinterPanelZebra_AlignLeft = temp;
            }
            catch (Exception)
            {

            }

            try
            {
                string temp = Common.ConfigHandler.GetValueFromNativeSettings(ZebraPrinter_AlignTop_Key);
                this.UcPrinterPanelZebra_AlignTop = temp;
            }
            catch
            {

            }
        }

        // 保存配置
        void saveConfig()
        {
            //// Step 1 XAML 的 UcPrinterPanelZebra_IsValidated 开启 IDataError

            //// Step 2 IDataError 增加校验逻辑 (1)
            //case nameof(UcPrinterPanelZebra_IsValidated):
            //            this.checkUcPrinterPanelZebra_IsValidated(validationResults);
            //break;

            //// Step 3 IDataError 增加校验逻辑 (2)
            //void checkUcPrinterPanelZebra_IsValidated(List<System.ComponentModel.DataAnnotations.ValidationResult> l)
            //{
            //    if (UcPrinterPanelZebra_IsValidated == false)
            //    {
            //        l.Add(new System.ComponentModel.DataAnnotations.ValidationResult("UcPrinterPanelZebra验证失败"));
            //    }
            //}

            //// Step 4 在 public override string this[string columnName] 最后加上保存逻辑, 如果校验没有任何异常就保存所有配置
            //if (errorMsg.IsNullOrWhiteSpace())
            //{
            //    switch (columnName)
            //    {
            //        // ...

            //        case nameof(UcPrinterPanelZebra_IsValidated):
            //            {
            //                Common.ConfigHandler.SetValueToNativeSettings(ZebraPrinter_DefaultPrinter, UcPrinterPanelZebra_SelectedPrinter.DisplayName);
            //                Common.ConfigHandler.SetValueToNativeSettings(ZebraPrinter_Speed_Key, UcPrinterPanelZebra_Speed.ToString());
            //                Common.ConfigHandler.SetValueToNativeSettings(ZebraPrinter_Darkness_Key, UcPrinterPanelZebra_Darkness.ToString());
            //                Common.ConfigHandler.SetValueToNativeSettings(ZebraPrinter_AlignLeft_Key, UcPrinterPanelZebra_AlignLeft.ToString());
            //                Common.ConfigHandler.SetValueToNativeSettings(ZebraPrinter_AlignTop_Key, UcPrinterPanelZebra_AlignTop.ToString());
            //            }
            //            break;

            //            // ...
            //        }
            //    }
        }

        #endregion

    }
}