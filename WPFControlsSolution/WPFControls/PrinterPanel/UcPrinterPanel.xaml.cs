using Client.Components.PrinterPanel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


/// <summary>
/// V 1.1 - 2019-03-15 12:05:01
/// 1. 修复选择打印机后没有自动选择纸张
/// 2. UI Combobox 文本对齐问题
/// </summary>
namespace Client.Components
{
    /// <summary>
    /// UcPrinterPanel.xaml 的交互逻辑
    /// </summary>
    public partial class UcPrinterPanel : UserControl, INotifyPropertyChanged
    {
        public UcPrinterPanel()
        {
            InitializeComponent();
            this.Loaded += UcPrinterPanel_Loaded;
        }

        public UcPrinterPanel(List<string> priorityPrinterList)
        {
            // TODO 优先打印机
        }

        public UcPrinterPanel(List<string> priorityPrinterList, List<string> priorityPaperSizeList)
        {
            // TODO XAML 传参
            // 优先打印机
            // 优先纸张
        }

        public string PriorityPrinterListStr { get; set; }

        public string PriorityPaperSizeListStr { get; set; }

        private void UcPrinterPanel_Loaded(object sender, RoutedEventArgs e)
        {
            // TODO 不能赋值
            List<string> p = null;
            if (string.IsNullOrWhiteSpace(PriorityPrinterListStr) == false)
            {
                p = PriorityPrinterListStr.Split(',').Select(i => i.Trim()).Where(i => string.IsNullOrWhiteSpace(i) == false).ToList();
            }

            List<string> s = null;
            if (string.IsNullOrWhiteSpace(PriorityPaperSizeListStr) == false)
            {
                p = PriorityPaperSizeListStr.Split(',').Select(i => i.Trim()).Where(i => string.IsNullOrWhiteSpace(i) == false).ToList();
            }

            init(p, s);
        }

        public void init(List<string> priorityPrinterList = null, List<string> priorityPaperSizeList = null)
        {
            if (priorityPrinterList != null && priorityPrinterList.Count > 0)
            {
                this.PriorityPrinterList = priorityPrinterList;
            }
            else
            {
                this.PriorityPrinterList = new List<string>();
            }

            if (priorityPaperSizeList != null && priorityPaperSizeList.Count > 0)
            {
                this.PriorityPaperSizeList = priorityPrinterList;
            }
            else
            {
                this.PriorityPaperSizeList = new List<string>() { "A4" }; // 根据不同项目需求排列优先顺序
            }

            var temp = PrinterUtils.GetPrinterList(isContainUpdateListItem: true);
            this.PrinterList = PrinterUtils.PrinterOrderBy(temp, this.PriorityPrinterList, this.PriorityPaperSizeList);

            var defaultPrinterName = new System.Drawing.Printing.PrintDocument().PrinterSettings.PrinterName;
            this.UcSelectedPrinter = this.PrinterList.FirstOrDefault(i => i.DisplayName == defaultPrinterName);
        }

        /// <summary>
        /// 打印机优先列表
        /// </summary>
        public List<string> PriorityPrinterList;

        /// <summary>
        /// 纸张优先
        /// </summary>
        public List<string> PriorityPaperSizeList;

        private List<Printer> _PrinterList;
        public List<Printer> PrinterList
        {
            get { return _PrinterList; }
            set
            {
                _PrinterList = value;
                this.OnPropertyChanged("PrinterList");
            }
        }

        private Printer _SelectedPrinter;

        public Printer UcSelectedPrinter
        {
            get { return this._SelectedPrinter; }
            set
            {
                if (value != null && value.DisplayName == PrinterUtils.sUpdateName) // 选择了 刷新 项
                {
                    var temp = PrinterUtils.GetPrinterList(isContainUpdateListItem: true);
                    this.PrinterList = PrinterUtils.PrinterOrderBy(temp, this.PriorityPrinterList, this.PriorityPaperSizeList);
                }
                else
                {
                    this._SelectedPrinter = value;
                    this.SelectedPrinter = value;
                    this.OnPropertyChanged("UcSelectedPrinter");
                    this.OnPropertyChanged("PaperSizeList");
                }
            }
        }


        public static readonly DependencyProperty SelectedPrinterProperty = DependencyProperty.Register
        (
            name: "SelectedPrinter",
            propertyType: typeof(Printer),
            ownerType: typeof(UcPrinterPanel),
            validateValueCallback: null, // new ValidateValueCallback((toValidate) => { return true; }),
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: null, // onSelectedPrinter_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public Printer SelectedPrinter
        {
            get { return (Printer)GetValue(SelectedPrinterProperty); }
            set { SetValue(SelectedPrinterProperty, value); }
        }

        






        public List<PaperSize> PaperSizeList
        {
            get
            {
                if (this.PrinterList != null && this.UcSelectedPrinter != null)
                {
                    UcSelectedPaperSize = UcSelectedPrinter.PaperSizeList[0];
                    return UcSelectedPrinter.PaperSizeList.ToList();
                }
                else
                {
                    return null;
                }
            }
        }

        private PaperSize _SelectedPaperSize;

        public PaperSize UcSelectedPaperSize
        {
            get { return _SelectedPaperSize; }
            set
            {
                _SelectedPaperSize = value;
                SelectedPaperSize = value;
                this.OnPropertyChanged("UcSelectedPaperSize");
            }
        }


        public static readonly DependencyProperty SelectedPaperSizeProperty = DependencyProperty.Register
        (
            name: "SelectedPaperSize",
            propertyType: typeof(PaperSize),
            ownerType: typeof(UcPrinterPanel),
            validateValueCallback: null, // new ValidateValueCallback((toValidate) => { return true; }),
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: null, // onSelectedPaperSize_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public PaperSize SelectedPaperSize
        {
            get { return (PaperSize)GetValue(SelectedPaperSizeProperty); }
            set { SetValue(SelectedPaperSizeProperty, value); }
        }


        public static readonly DependencyProperty PaperSizeVisibilityProperty = DependencyProperty.Register
        (
            name: "PaperSizeVisibility",
            propertyType: typeof(Visibility),
            ownerType: typeof(UcPrinterPanel),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: Visibility.Visible,
                propertyChangedCallback: null, // onPaperSizeVisibility_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public Visibility PaperSizeVisibility
        {
            get { return (Visibility)GetValue(PaperSizeVisibilityProperty); }
            set { SetValue(PaperSizeVisibilityProperty, value); }
        }



        #region INotifyPropertyChanged成员

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
