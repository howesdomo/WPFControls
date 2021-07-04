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

namespace Client.Components
{
    /// <summary>
    /// V 2.0.0 - 2021-07-04 22:34:58
    /// 重写大部分功能
    /// 
    /// V 1.0.1 - 2019-03-15 12:05:01
    /// 1. 修复选择打印机后没有自动选择纸张
    /// 2. UI Combobox 文本对齐问题
    /// </summary>
    public partial class UcPrinterPanel : UserControl, INotifyPropertyChanged, IDataErrorInfo
    {
        public UcPrinterPanel()
        {
            InitializeComponent();
            init();
            this.Loaded += ucLoaded;
        }

        private void ucLoaded(object sender, RoutedEventArgs e)
        {
            init();
        }

        public void init()
        {
            if (string.IsNullOrWhiteSpace(PriorityPrinterListStr) == false)
            {
                this.PriorityPrinterList = PriorityPrinterListStr.Split(',').Select(i => i.Trim()).Where(i => string.IsNullOrWhiteSpace(i) == false).ToList();
            }
            else
            {
                this.PriorityPrinterList = new List<string>();
            }


            if (string.IsNullOrWhiteSpace(PriorityPaperSizeListStr) == false)
            {
                this.PriorityPaperSizeList = PriorityPaperSizeListStr.Split(',').Select(i => i.Trim()).Where(i => string.IsNullOrWhiteSpace(i) == false).ToList();
            }
            else
            {
                this.PriorityPaperSizeList = new List<string>();
            }

            if (this.PrinterList == null)
            {
                this.PrinterList = PrinterUtils.PrinterOrderBy
                (
                    printerList: PrinterUtils.GetPrinterList(isContainUpdateListItem: true),
                    priorityPrinterList: this.PriorityPrinterList,
                    priorityPaperList: this.PriorityPaperSizeList
                );
            }

            if (this.SelectedPrinter_Inner == null)
            {
                // TODO [无法解决] 不自行指定 列表和选中打印机, 必定有红框框
                var match = this.PrinterList.FirstOrDefault(i => i.DisplayName == PrinterUtils.GetDefaultPrinterName());  // 设置选中默认打印机
                if (match != null)
                {
                    this.SelectedPrinter_Inner = match;
                }
            }
        }

        #region [DP] IsValidated -- 验证通过, 所有选项都符合验证

        public static readonly DependencyProperty IsValidatedProperty = DependencyProperty.Register
        (
            name: "IsValidated",
            propertyType: typeof(bool),
            ownerType: typeof(UcPrinterPanel),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: false,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public bool IsValidated
        {
            get { return (bool)GetValue(IsValidatedProperty); }
            set { SetValue(IsValidatedProperty, value); }
        }

        #endregion

        #region [DP] PriorityPrinterListStr

        public static readonly DependencyProperty PriorityPrinterListStrProperty = DependencyProperty.Register
        (
            name: "PriorityPrinterListStr",
            propertyType: typeof(string),
            ownerType: typeof(UcPrinterPanel),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public string PriorityPrinterListStr
        {
            get { return (string)GetValue(PriorityPrinterListStrProperty); }
            set { SetValue(PriorityPrinterListStrProperty, value); }
        }

        #endregion

        /// <summary>
        /// 打印机优先列表
        /// </summary>
        public List<string> PriorityPrinterList;

        #region [DP] PriorityPaperSizeListStr

        public static readonly DependencyProperty PriorityPaperSizeListStrProperty = DependencyProperty.Register
        (
            name: "PriorityPaperSizeListStr",
            propertyType: typeof(string),
            ownerType: typeof(UcPrinterPanel),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public string PriorityPaperSizeListStr
        {
            get { return (string)GetValue(PriorityPaperSizeListStrProperty); }
            set { SetValue(PriorityPaperSizeListStrProperty, value); }
        }

        #endregion

        /// <summary>
        /// 纸张优先
        /// </summary>
        public List<string> PriorityPaperSizeList;





        #region [DP] PrinterList

        public static readonly DependencyProperty PrinterListProperty = DependencyProperty.Register
        (
            name: "PrinterList",
            propertyType: typeof(List<Printer>),
            ownerType: typeof(UcPrinterPanel),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public List<Printer> PrinterList
        {
            get { return (List<Printer>)GetValue(PrinterListProperty); }
            set { SetValue(PrinterListProperty, value); }
        }

        #endregion

        private Printer _SelectedPrinter_Inner;
        public Printer SelectedPrinter_Inner
        {
            get { return _SelectedPrinter_Inner; }
            set
            {
                _SelectedPrinter_Inner = value;
                this.OnPropertyChanged(nameof(SelectedPrinter_Inner));

                this.SelectedPrinter = value; // !!!! 向对外绑定的SelectedPrinter赋值
            }
        }

        /// <summary>
        /// 控件外部静默赋值
        /// </summary>
        /// <param name="value"></param>
        public void SetSelectedPrinter_Inner_Slient(Printer value)
        {
            _SelectedPrinter_Inner = value;
            this.OnPropertyChanged(nameof(SelectedPrinter_Inner));
        }

        #region [DP] SelectedPrinter

        public static readonly DependencyProperty SelectedPrinterProperty = DependencyProperty.Register
        (
            name: "SelectedPrinter",
            propertyType: typeof(Client.Components.PrinterPanel.Printer),
            ownerType: typeof(UcPrinterPanel),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: onSelectedPrinter_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public Client.Components.PrinterPanel.Printer SelectedPrinter
        {
            get { return (Client.Components.PrinterPanel.Printer)GetValue(SelectedPrinterProperty); }
            set { SetValue(SelectedPrinterProperty, value); }
        }

        public static void onSelectedPrinter_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UcPrinterPanel target)
            {
                if (target.SelectedPrinter_Inner == null && e.NewValue is Printer vvv) // 控件外赋值初始值
                {
                    target.SetSelectedPrinter_Inner_Slient(vvv);
                    target.SelectedPaperSize_Inner = target.SelectedPrinter_Inner.PaperSizeList[0];
                    return;
                }

                if (e.NewValue != null && e.NewValue is Printer value && value.DisplayName == PrinterUtils.UpdateItem)
                {
                    // 选择了 刷新 项
                    var temp = PrinterUtils.GetPrinterList(isContainUpdateListItem: true);
                    target.PrinterList = PrinterUtils.PrinterOrderBy(printerList: temp, priorityPrinterList: target.PriorityPrinterList, priorityPaperList: target.PriorityPaperSizeList);
                }

                if (target.SelectedPrinter_Inner != null)
                {
                    target.SelectedPaperSize_Inner = target.SelectedPrinter_Inner.PaperSizeList[0];
                }
            }
        }

        #endregion

        // PaperSizeList 在 XAML 采用控件直接绑定的方式
        //public List<PaperSize> PaperSizeList

        #region [DP] SelectedPaperSize

        public static readonly DependencyProperty SelectedPaperSizeProperty = DependencyProperty.Register
        (
            name: "SelectedPaperSize",
            propertyType: typeof(PaperSize),
            ownerType: typeof(UcPrinterPanel),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public PaperSize SelectedPaperSize
        {
            get { return (PaperSize)GetValue(SelectedPaperSizeProperty); }
            set { SetValue(SelectedPaperSizeProperty, value); }
        }

        #endregion



        private PaperSize _SelectedPaperSize_Inner;
        public PaperSize SelectedPaperSize_Inner
        {
            get { return _SelectedPaperSize_Inner; }
            set
            {
                _SelectedPaperSize_Inner = value;
                this.OnPropertyChanged(nameof(SelectedPaperSize_Inner));

                this.SelectedPaperSize = value;
            }
        }

        #region [DP] PaperSizeVisibility

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

        #endregion



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

        #region IDataInfo

        public virtual bool _IsValidated_
        {
            get
            {
                return this.ErrorCollection.Count == 0;
            }
        }

        public string Error
        {
            get
            {
                return $"共 {this.ErrorCollection.Values.Count} 个错误";
            }
        }

        public System.Collections.Generic.Dictionary<string, string> ErrorCollection { get; private set; } = new System.Collections.Generic.Dictionary<string, string>();

        protected void executeErrorCollection(string columnName, string errorMsg)
        {
            if (ErrorCollection.ContainsKey(columnName))
            {
                if (string.IsNullOrWhiteSpace(errorMsg))
                {
                    ErrorCollection.Remove(columnName);
                }
                else
                {
                    ErrorCollection[columnName] = errorMsg;
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(errorMsg) == false)
                {
                    ErrorCollection.Add(columnName, errorMsg);
                }
            }

            IsValidated = _IsValidated_;

            this.OnPropertyChanged(nameof(ErrorCollection));
            this.OnPropertyChanged(nameof(Error));
            this.OnPropertyChanged(nameof(_IsValidated_));
        }

        /// <summary>
        /// 判断errorMessage, 若符合条件则添加到 validationResultList 中
        /// </summary>
        /// <param name="validationResultList"></param>
        /// <param name="errorMessage"></param>
        protected void addValidationResult
        (
            ICollection<System.ComponentModel.DataAnnotations.ValidationResult> validationResultList,
            string errorMessage
        )
        {
            if (string.IsNullOrEmpty(errorMessage) == false)
            {
                validationResultList.Add(new System.ComponentModel.DataAnnotations.ValidationResult(errorMessage));
            }
        }

        public virtual string this[string columnName]
        {
            get
            {
                string errorMsg = null;

                // Step 1 根据 System.ComponentModel.DataAnnotations 进行校验
                var vc = new System.ComponentModel.DataAnnotations.ValidationContext(this, null, null);
                vc.MemberName = columnName;
                var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
                System.ComponentModel.DataAnnotations.Validator.TryValidateProperty
                (
                    value: this.GetType().GetProperty(columnName).GetValue(this, null),
                    validationContext: vc,
                    validationResults: validationResults
                );

                // Step 2 根据额外编写的校验逻辑进行校验
                switch (columnName)
                {
                    case nameof(SelectedPrinter_Inner):
                        checkSelectedPrinter(validationResults);
                        break;
                    case nameof(SelectedPaperSize_Inner):
                        checkSelectedPaperSize(validationResults);
                        break;

                    default:
                        break;
                }

                if (validationResults.Count > 0)
                {
                    errorMsg = string.Join(";", validationResults);
                }

                executeErrorCollection(columnName, errorMsg);

                return errorMsg;
            }
        }

        void checkSelectedPrinter(List<System.ComponentModel.DataAnnotations.ValidationResult> l)
        {
            if (this.SelectedPrinter_Inner == null)
            {
                addValidationResult(l, "未选择打印机");
            }
            else if (this.PrinterList != null && this.PrinterList.Contains(this.SelectedPrinter_Inner) == false)
            {
                addValidationResult(l, "选中的打印机不在列表中");
            }
        }

        void checkSelectedPaperSize(List<System.ComponentModel.DataAnnotations.ValidationResult> l)
        {
            if (PaperSizeVisibility == Visibility.Visible)
            {
                if (this.SelectedPaperSize_Inner == null)
                {
                    addValidationResult(l, "未选择纸张");
                }
                //else if (this.PaperSizeList != null && this.PaperSizeList.Contains(this.SelectedPaperSize_Inner) == false)
                //{
                //    addValidationResult(l, "选中的纸张不在列表中");
                //}
                else if (this.SelectedPrinter_Inner != null && this.SelectedPrinter_Inner.PaperSizeList.Contains(this.SelectedPaperSize_Inner) == false)
                {
                    addValidationResult(l, "选中的纸张不在列表中");
                }
            }
        }

        #endregion
    }
}
