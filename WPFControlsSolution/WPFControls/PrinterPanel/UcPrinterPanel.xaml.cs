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
    /// V 1.1 - 2019-03-15 12:05:01
    /// 1. 修复选择打印机后没有自动选择纸张
    /// 2. UI Combobox 文本对齐问题
    /// </summary>
    public partial class UcPrinterPanel : UserControl, INotifyPropertyChanged, IDataErrorInfo
    {
        public UcPrinterPanel()
        {
            InitializeComponent();
            this.Loaded += ucLoaded;
        }

        public string PriorityPrinterListStr { get; set; }

        public string PriorityPaperSizeListStr { get; set; }

        private void ucLoaded(object sender, RoutedEventArgs e)
        {
            List<string> printer = null;
            if (string.IsNullOrWhiteSpace(PriorityPrinterListStr) == false)
            {
                printer = PriorityPrinterListStr.Split(',').Select(i => i.Trim()).Where(i => string.IsNullOrWhiteSpace(i) == false).ToList();
            }

            List<string> paper = null;
            if (string.IsNullOrWhiteSpace(PriorityPaperSizeListStr) == false)
            {
                paper = PriorityPaperSizeListStr.Split(',').Select(i => i.Trim()).Where(i => string.IsNullOrWhiteSpace(i) == false).ToList();
            }

            init(printer, paper);
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
                this.PriorityPaperSizeList = priorityPaperSizeList;
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

            if (this.SelectedPrinter == null)
            {
                //// TODO [无法解决] 不自行指定 列表和选中打印机, 必定有红框框
                //var match = this.PrinterList.FirstOrDefault(i => i.DisplayName == PrinterUtils.GetDefaultPrinterName());  // 设置选中默认打印机
                //if (match != null)
                //{
                //    this.SelectedPrinter = match;
                //}
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

        /// <summary>
        /// 打印机优先列表
        /// </summary>
        public List<string> PriorityPrinterList;

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
                if (e.NewValue != null && e.NewValue is Printer value && value.DisplayName == PrinterUtils.UpdateItem)
                {
                    // 选择了 刷新 项
                    var temp = PrinterUtils.GetPrinterList(isContainUpdateListItem: true);
                    target.PrinterList = PrinterUtils.PrinterOrderBy(printerList: temp, priorityPrinterList: target.PriorityPrinterList, priorityPaperList: target.PriorityPaperSizeList);
                }

                target.OnPropertyChanged(nameof(PaperSizeList));
            }
        }

        #endregion





        public List<PaperSize> PaperSizeList
        {
            get
            {
                if (SelectedPrinter != null && SelectedPrinter.PaperSizeList != null && SelectedPrinter.PaperSizeList.Count > 0)
                {
                    SelectedPaperSize = SelectedPrinter.PaperSizeList[0]; // 设置默认纸张
                    return SelectedPrinter.PaperSizeList;
                }
                else
                {
                    return null;
                }
            }
        }

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
                    case nameof(SelectedPrinter):
                        checkSelectedPrinter(validationResults);
                        break;
                    case nameof(SelectedPaperSize):
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
            if (this.SelectedPrinter == null)
            {
                addValidationResult(l, "未选择打印机");
            }
        }

        void checkSelectedPaperSize(List<System.ComponentModel.DataAnnotations.ValidationResult> l)
        {
            if (this.SelectedPaperSize == null && PaperSizeVisibility == Visibility.Visible)
            {
                addValidationResult(l, "未选择纸张");
            }
        }

        #endregion
    }
}
