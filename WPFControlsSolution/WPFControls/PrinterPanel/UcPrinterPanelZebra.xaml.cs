using Client.Components.PrinterPanel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Client.Components
{
    /// <summary>
    /// V 1.0.1 - 2021-07-02 15:07:23
    /// 修复绑定Bug, 采用 IDataErrorInfo 方式提示异常
    /// 
    /// V 1.0.0 - 2020-11-05 11:36:45
    /// 首次创建, 用于斑马打印机选项
    /// </summary>
    public partial class UcPrinterPanelZebra : UserControl, INotifyPropertyChanged, IDataErrorInfo
    {
        public UcPrinterPanelZebra()
        {
            InitializeComponent();
            init();
            this.Loaded += ucLoaded;
        }

        private void ucLoaded(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"UcPrinterPanelZebra is Loaded");
            init();
        }

        void init()
        {
            if (string.IsNullOrWhiteSpace(PriorityPrinterListStr) == false)
            {
                this.PriorityPrinterList = PriorityPrinterListStr.Split(',').Select(i => i.Trim()).Where(i => string.IsNullOrWhiteSpace(i) == false).ToList();
            }
            else
            {
                this.PriorityPrinterList = new List<string>();
            }

            if (this.PrinterList == null)
            {
                this.PrinterList = PrinterUtils.PrinterOrderBy
                (
                    printerList: PrinterUtils.GetPrinterList(isContainUpdateListItem: true),
                    priorityPrinterList: this.PriorityPrinterList,
                    priorityPaperList: null // 斑马打印机无需设置纸张选项
                );
            }

            if (this.SelectedPrinter == null)
            {
                // TODO [无法解决] 不自行指定 列表和选中打印机, 必定有红框框
                var defaultPrinterName = PrinterUtils.GetDefaultPrinterName();
                this.SelectedPrinter_InnerInner = this.PrinterList.FirstOrDefault(i => i.DisplayName == defaultPrinterName); // 设置选中默认打印机
            }
        }

        #region [DP] IsValidated -- 验证通过, 所有选项都符合验证

        public static readonly DependencyProperty IsValidatedProperty = DependencyProperty.Register
        (
            name: "IsValidated",
            propertyType: typeof(bool),
            ownerType: typeof(UcPrinterPanelZebra),
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

        public string PriorityPrinterListStr { get; set; }

        /// <summary>
        /// 打印机优先列表
        /// </summary>
        protected List<string> PriorityPrinterList { get; set; }

        #region [DP] PrinterList

        public static readonly DependencyProperty PrinterListProperty = DependencyProperty.Register
        (
            name: "PrinterList",
            propertyType: typeof(List<Printer>),
            ownerType: typeof(UcPrinterPanelZebra),
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

        private Client.Components.PrinterPanel.Printer _SelectedPrinter_InnerInner;
        public Client.Components.PrinterPanel.Printer SelectedPrinter_InnerInner
        {
            get { return _SelectedPrinter_InnerInner; }
            set
            {
                _SelectedPrinter_InnerInner = value;
                this.OnPropertyChanged(nameof(SelectedPrinter_InnerInner));

                this.SelectedPrinter = value; // !!!! 向对外绑定的SelectedPrinter赋值
            }
        }

        /// <summary>
        /// 控件外部静默赋值
        /// </summary>
        /// <param name="value"></param>
        public void SetSelectedPrinter_Inner_Slient(Printer value)
        {
            _SelectedPrinter_InnerInner = value;
            this.OnPropertyChanged(nameof(SelectedPrinter_InnerInner));
        }






        // TODO 能否进行对对象的绑定
        #region [DP] Data -- 未实现

        public static readonly DependencyProperty DataProperty = DependencyProperty.Register
        (
            name: "Data",
            propertyType: typeof(Client.Components.PrinterPanel.ZebraPrinter),
            ownerType: typeof(UcPrinterPanelZebra),
            validateValueCallback: null, // new ValidateValueCallback((toValidate) => { return true; }),
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: onData_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public Client.Components.PrinterPanel.ZebraPrinter Data
        {
            get { return (Client.Components.PrinterPanel.ZebraPrinter)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        public static void onData_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UcPrinterPanelZebra target)
            {
                // TODO 逻辑
            }
        }

        #endregion




        #region [DP] SelectedPrinter

        public static readonly DependencyProperty SelectedPrinterProperty = DependencyProperty.Register
        (
            name: "SelectedPrinter",
            propertyType: typeof(Client.Components.PrinterPanel.Printer),
            ownerType: typeof(UcPrinterPanelZebra),
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
            if (d is UcPrinterPanelZebra target)
            {
                if (target.SelectedPrinter_InnerInner == null && e.NewValue is Printer vvv) // 控件外赋值初始值
                {
                    target.SetSelectedPrinter_Inner_Slient(vvv);
                    return;
                }

                if (e.NewValue != null && e.NewValue is Printer value && value.DisplayName == PrinterUtils.UpdateItem)
                {
                    // 选择了 刷新 项
                    var temp = PrinterUtils.GetPrinterList(isContainUpdateListItem: true);
                    target.PrinterList = PrinterUtils.PrinterOrderBy(printerList: temp, priorityPrinterList: target.PriorityPrinterList, priorityPaperList: null);
                }
            }
        }

        #endregion


        #region [DP] AlignLeft

        public static readonly DependencyProperty AlignLeftProperty = DependencyProperty.Register
        (
            name: "AlignLeft",
            propertyType: typeof(object),
            ownerType: typeof(UcPrinterPanelZebra),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: "0",
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        [Required(AllowEmptyStrings = false, ErrorMessage = "数值为空")]
        [Range(minimum: 0, maximum: 9999, ErrorMessage = "校验失败")]
        public object AlignLeft
        {
            get { return (object)GetValue(AlignLeftProperty); }
            set { SetValue(AlignLeftProperty, value); }
        }

        #endregion

        #region [DP] AlignTop

        public static readonly DependencyProperty AlignTopProperty = DependencyProperty.Register
        (
            name: "AlignTop",
            propertyType: typeof(object),
            ownerType: typeof(UcPrinterPanelZebra),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: "0",
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        [Required(AllowEmptyStrings = false, ErrorMessage = "数值为空")]
        [Range(minimum: 0, maximum: 9999, ErrorMessage = "校验失败")]
        public object AlignTop
        {
            get { return (object)GetValue(AlignTopProperty); }
            set { SetValue(AlignTopProperty, value); }
        }

        #endregion

        #region [DP] AlignVisibility -- 显示/隐藏靠下靠右面版

        public static readonly DependencyProperty AlignVisibilityProperty = DependencyProperty.Register
        (
            name: "AlignVisibility",
            propertyType: typeof(Visibility),
            ownerType: typeof(UcPrinterPanelZebra),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: Visibility.Visible,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public Visibility AlignVisibility
        {
            get { return (Visibility)GetValue(AlignVisibilityProperty); }
            set { SetValue(AlignVisibilityProperty, value); }
        }

        #endregion

        private void TextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (sender != null && sender is TextBox)
            {
                (sender as TextBox).SelectAll();
            }
        }

        private void TextBox_GotMouseCapture(object sender, MouseEventArgs e)
        {
            if (sender != null && sender is TextBox)
            {
                (sender as TextBox).SelectAll();
            }
        }


        public List<string> DarknessList { get; private set; } = new List<string>()
        {
            "0","1","2","3","4","5","6","7","8","9","10",
            "11","12","13","14","15","16","17","18","19","20",
            "21","22","23","24","25","26","27","28","29","30"
        };

        #region [DP] Darkness -- 打印浓度

        public static readonly DependencyProperty DarknessProperty = DependencyProperty.Register
        (
            name: "Darkness",
            propertyType: typeof(object),
            ownerType: typeof(UcPrinterPanelZebra),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: "20",
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public object Darkness
        {
            get { return (object)GetValue(DarknessProperty); }
            set { SetValue(DarknessProperty, value); }
        }

        #endregion

        #region [DP] DarknessVisibility -- 显示/隐藏打印浓度面版

        public static readonly DependencyProperty DarknessVisibilityProperty = DependencyProperty.Register
        (
            name: "DarknessVisibility",
            propertyType: typeof(Visibility),
            ownerType: typeof(UcPrinterPanelZebra),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: Visibility.Visible,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public Visibility DarknessVisibility
        {
            get { return (Visibility)GetValue(DarknessVisibilityProperty); }
            set { SetValue(DarknessVisibilityProperty, value); }
        }

        #endregion


        public List<string> SpeedList { get; private set; } = new List<string>()
        {
            "5",
            "7.6",
            "10.1",
            "12.7",
            "15.2"
        };

        #region [DP] Speed -- 打印速度

        public static readonly DependencyProperty SpeedProperty = DependencyProperty.Register
        (
            name: "Speed",
            propertyType: typeof(object),
            ownerType: typeof(UcPrinterPanelZebra),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: "10.1",
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public object Speed
        {
            get { return (object)GetValue(SpeedProperty); }
            set { SetValue(SpeedProperty, value); }
        }

        #endregion

        #region [DP] SpeedVisibility -- 显示/隐藏打印速度面版

        public static readonly DependencyProperty SpeedVisibilityProperty = DependencyProperty.Register
        (
            name: "SpeedVisibility",
            propertyType: typeof(Visibility),
            ownerType: typeof(UcPrinterPanelZebra),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: Visibility.Visible,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public Visibility SpeedVisibility
        {
            get { return (Visibility)GetValue(SpeedVisibilityProperty); }
            set { SetValue(SpeedVisibilityProperty, value); }
        }

        #endregion



        #region INotifyPropertyChanged成员

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
                    case nameof(SelectedPrinter_InnerInner):
                        checkSelectedPrinter_InnerInner(validationResults);
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

        void checkSelectedPrinter_InnerInner(List<System.ComponentModel.DataAnnotations.ValidationResult> l)
        {
            if (this.SelectedPrinter_InnerInner == null)
            {
                addValidationResult(l, "未选择打印机");
            }
            else if (this.PrinterList != null && this.PrinterList.Contains(this.SelectedPrinter_InnerInner) == false)
            {
                addValidationResult(l, "选中的打印机不在列表中");
            }
        }

        #endregion

    }
}
