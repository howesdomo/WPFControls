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
using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

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
            this.Loaded += ucLoaded;
        }

        public UcPrinterPanelZebra(List<string> priorityPrinterList)
        {
            InitializeComponent();
            this.PriorityPrinterList = priorityPrinterList;
            this.Loaded += ucLoaded;
        }

        private void ucLoaded(object sender, RoutedEventArgs e)
        {
            init();
        }

        void init(List<string> priorityPrinterList = null)
        {
            if (priorityPrinterList != null && priorityPrinterList.Count > 0)
            {
                this.PriorityPrinterList = priorityPrinterList;
            }
            else
            {
                this.PriorityPrinterList = new List<string>();
            }

            this.PriorityPaperSizeList = new List<string>();
            
            if (this.PrinterList == null)
            {
                var temp = PrinterUtils.GetPrinterList(isContainUpdateListItem: true);
                this.PrinterList = PrinterUtils.PrinterOrderBy(temp, this.PriorityPrinterList, this.PriorityPaperSizeList);
            }

            if (this.SelectedPrinter == null)
            {
                var defaultPrinterName = PrinterUtils.GetDefaultPrinterName();
                this.SelectedPrinter = PrinterList.FirstOrDefault(i => i.DisplayName == defaultPrinterName); // 设置选中默认打印机
            }
        }

        /// <summary>
        /// 打印机优先列表
        /// </summary>
        public List<string> PriorityPrinterList { get; set; }

        /// <summary>
        /// 纸张优先
        /// </summary>        
        public List<string> PriorityPaperSizeList { get; set; }




        public static readonly DependencyProperty PrinterListProperty = DependencyProperty.Register
        (
            name: "PrinterList",
            propertyType: typeof(List<Printer>),
            ownerType: typeof(UcPrinterPanelZebra),
            validateValueCallback: null, // new ValidateValueCallback((toValidate) => { return true; }),
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null, // PrinterUtils.GetPrinterList(isContainUpdateListItem: true),
                propertyChangedCallback: onPrinterList_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public List<Printer> PrinterList
        {
            get { return (List<Printer>)GetValue(PrinterListProperty); }
            set { SetValue(PrinterListProperty, value); }
        }

        public static void onPrinterList_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UcPrinterPanelZebra target)
            {

            }
        }




        #region SelectedPrinter


        public static readonly DependencyProperty SelectedPrinterProperty = DependencyProperty.Register
        (
            name: "SelectedPrinter",
            propertyType: typeof(Printer),
            ownerType: typeof(UcPrinterPanelZebra),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: onSelectedPrinter_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public Printer SelectedPrinter
        {
            get { return (Printer)GetValue(SelectedPrinterProperty); }
            set { SetValue(SelectedPrinterProperty, value); }
        }

        public static void onSelectedPrinter_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((d is UcPrinterPanelZebra) == false) { return; }
            var target = d as UcPrinterPanelZebra;

            var value = e.NewValue as Printer;

            if (value != null && value.DisplayName == PrinterUtils.sUpdateName) // 选择了 刷新 项
            {
                var temp = PrinterUtils.GetPrinterList(isContainUpdateListItem: true);
                target.PrinterList = PrinterUtils.PrinterOrderBy(temp, target.PriorityPrinterList, target.PriorityPaperSizeList);
            }

            // target.OnPropertyChanged("SelectedPrinter");
        }

        //private Printer _SelectedItem;
        //public Printer SelectedItem
        //{
        //    get { return this._SelectedItem; }
        //    set
        //    {
        //        if (value != null && value.DisplayName == PrinterUtils.sUpdateName) // 选择了 刷新 项
        //        {
        //            var temp = PrinterUtils.GetPrinterList(isContainUpdateListItem: true);
        //            this.PrinterList = PrinterUtils.PrinterOrderBy(temp, this.PriorityPrinterList, this.PriorityPaperSizeList);
        //        }
        //        else
        //        {
        //            this._SelectedItem = value;
        //            this.SelectedPrinter = value;
        //            this.OnPropertyChanged("SelectedItem");
        //        }
        //    }
        //}

        #endregion

        #region AlignLeft

        public static readonly DependencyProperty AlignLeftProperty = DependencyProperty.Register
        (
            name: "AlignLeft",
            propertyType: typeof(object),
            ownerType: typeof(UcPrinterPanelZebra),
            validateValueCallback: null, // new ValidateValueCallback((toValidate) => { return true; }),
            typeMetadata: new PropertyMetadata
            (
                defaultValue: "0",
                propertyChangedCallback: onAlignLeft_PropertyChangedCallback,
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

        public static void onAlignLeft_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((d is UcPrinterPanelZebra) == false) { return; }
            var target = d as UcPrinterPanelZebra;

            //var vm = target.DataContext as UcPrinterPanelZebra_ViewModel;
            //if (e.NewValue != null && vm.AlignLeft != (string)e.NewValue)
            //{
            //    vm.AlignLeft = (string)e.NewValue;
            //}
        }



        #endregion

        #region AlignTop

        public static readonly DependencyProperty AlignTopProperty = DependencyProperty.Register
        (
            name: "AlignTop",
            propertyType: typeof(object),
            ownerType: typeof(UcPrinterPanelZebra),
            validateValueCallback: null, // new ValidateValueCallback((toValidate) => { return true; }),
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: onAlignTop_PropertyChangedCallback,
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

        public static void onAlignTop_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((d is UcPrinterPanelZebra) == false) { return; }
            var target = d as UcPrinterPanelZebra;

            //var vm = target.DataContext as UcPrinterPanelZebra_ViewModel;
            //if (e.NewValue != null && vm.AlignTop != (string)e.NewValue)
            //{
            //    vm.AlignTop = (string)e.NewValue;
            //}
        }



        #endregion

        public static readonly DependencyProperty AlignVisibilityProperty = DependencyProperty.Register
        (
            name: "AlignVisibility",
            propertyType: typeof(Visibility),
            ownerType: typeof(UcPrinterPanelZebra),
            validateValueCallback: null, // new ValidateValueCallback((toValidate) => { return true; }),
            typeMetadata: new PropertyMetadata
            (
                defaultValue: Visibility.Visible,
                propertyChangedCallback: null, // onAlignVisibility_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public Visibility AlignVisibility
        {
            get { return (Visibility)GetValue(AlignVisibilityProperty); }
            set { SetValue(AlignVisibilityProperty, value); }
        }

        public List<string> DarknessList { get; private set; } = new List<string>()
        {
            "0","1","2","3","4","5","6","7","8","9","10",
            "11","12","13","14","15","16","17","18","19","20",
            "21","22","23","24","25","26","27","28","29","30"
        };

        #region Darkness

        public static readonly DependencyProperty DarknessProperty = DependencyProperty.Register
        (
            name: "Darkness",
            propertyType: typeof(object),
            ownerType: typeof(UcPrinterPanelZebra),
            validateValueCallback: null, // new ValidateValueCallback((toValidate) => { return true; }),
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: onDarkness_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public object Darkness
        {
            get { return (object)GetValue(DarknessProperty); }
            set { SetValue(DarknessProperty, value); }
        }

        public static void onDarkness_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((d is UcPrinterPanelZebra) == false) { return; }
            var target = d as UcPrinterPanelZebra;


            //var vm = target.DataContext as UcPrinterPanelZebra_ViewModel;
            //vm.Darkness = (string)e.NewValue;
        }




        public static readonly DependencyProperty DarknessVisibilityProperty = DependencyProperty.Register
        (
            name: "DarknessVisibility",
            propertyType: typeof(Visibility),
            ownerType: typeof(UcPrinterPanelZebra),
            validateValueCallback: null, // new ValidateValueCallback((toValidate) => { return true; }),
            typeMetadata: new PropertyMetadata
            (
                defaultValue: Visibility.Visible,
                propertyChangedCallback: null, // onDarknessVisibility_PropertyChangedCallback,
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

        #region Speed

        public static readonly DependencyProperty SpeedProperty = DependencyProperty.Register
        (
            name: "Speed",
            propertyType: typeof(object),
            ownerType: typeof(UcPrinterPanelZebra),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: onSpeed_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public object Speed
        {
            get { return (object)GetValue(SpeedProperty); }
            set { SetValue(SpeedProperty, value); }
        }

        public static void onSpeed_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((d is UcPrinterPanelZebra) == false) { return; }
            var target = d as UcPrinterPanelZebra;

            //var vm = target.DataContext as UcPrinterPanelZebra_ViewModel;

            //if (target.FirstSetSpeed == true)
            //{
            //    vm.Speed = (string)e.NewValue;
            //    target.FirstSetSpeed = false;
            //}
        }

        public static readonly DependencyProperty SpeedVisibilityProperty = DependencyProperty.Register
        (
            name: "SpeedVisibility",
            propertyType: typeof(Visibility),
            ownerType: typeof(UcPrinterPanelZebra),
            validateValueCallback: null, // new ValidateValueCallback((toValidate) => { return true; }),
            typeMetadata: new PropertyMetadata
            (
                defaultValue: Visibility.Visible,
                propertyChangedCallback: null, // onSpeedVisibility_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public Visibility SpeedVisibility
        {
            get { return (Visibility)GetValue(SpeedVisibilityProperty); }
            set { SetValue(SpeedVisibilityProperty, value); }
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



        public static readonly DependencyProperty FirstSetSpeedProperty = DependencyProperty.Register
        (
            name: "FirstSetSpeed",
            propertyType: typeof(bool),
            ownerType: typeof(UcPrinterPanelZebra),
            validateValueCallback: null, // new ValidateValueCallback((toValidate) => { return true; }),
            typeMetadata: new PropertyMetadata
            (
                defaultValue: true,
                propertyChangedCallback: null, // onFirstSetSpeed_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public bool FirstSetSpeed
        {
            get { return (bool)GetValue(FirstSetSpeedProperty); }
            set { SetValue(FirstSetSpeedProperty, value); }
        }









        #region INotifyPropertyChanged成员

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion


        public static readonly DependencyProperty IsValidatedProperty = DependencyProperty.Register
        (
            name: "IsValidated",
            propertyType: typeof(bool),
            ownerType: typeof(UcPrinterPanelZebra),
            validateValueCallback: null, // new ValidateValueCallback((toValidate) => { return true; }),
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
                    //case "Nickname":
                    //// 方式一 将 validationResults 传进去 ( 推荐 可以分别将不符合的点一个个添加到集合里面, 方便最后一项一项地罗列出来 )
                    //    this.checkNickname(validationResults);
                    //    break;

                    //case "Nickname2":
                    //// 方式二 写一个验证方法返回验证信息
                    //    addValidationResult(validationResults, this.checkNickname2());
                    //    break;

                    case nameof(SelectedPrinter):
                        checkSelectedPrinter(validationResults);
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

        void checkSelectedPrinter(List<ValidationResult> l)
        {
            if (this.SelectedPrinter == null)
            {
                addValidationResult(l, "未选择打印机");
            }
        }

        //string checkNickname2()
        //{
        //    string errorMsg = string.Empty;
        //    return errorMsg;
        //}

        #endregion

    }

    //public class UcPrinterPanelZebra_ViewModel : INotifyPropertyChanged, IDataErrorInfo
    //{



    //    UcPrinterPanelZebra mUc { get; set; }

    //    public UcPrinterPanelZebra_ViewModel(UcPrinterPanelZebra uc)
    //    {
    //        this.mUc = uc;
    //    }



    //    private List<Printer> _PrinterList;
    //    public List<Printer> PrinterList
    //    {
    //        get { return _PrinterList; }
    //        set
    //        {
    //            _PrinterList = value;
    //            this.OnPropertyChanged("PrinterList");
    //        }
    //    }




    //    private string _AlignLeft;
    //    public string AlignLeft
    //    {
    //        get { return _AlignLeft; }
    //        set
    //        {
    //            #region 逻辑判断

    //            string errorMsg = string.Empty;

    //            if (string.IsNullOrWhiteSpace(value) == true)
    //            {
    //                errorMsg = "空值";
    //            }
    //            else if (Regex.IsMatch(value, "^[0-9]{1,}$") == false)
    //            {
    //                string t = "不符合要求";
    //                errorMsg = string.IsNullOrWhiteSpace(errorMsg) ? t : $"{errorMsg}\r\n{t}";
    //            }

    //            if (string.IsNullOrWhiteSpace(errorMsg) == false)
    //            {
    //                _AlignLeft = null;
    //                mUc.AlignLeft = null;
    //                throw new ArgumentException(errorMsg);
    //            }

    //            #endregion

    //            if (_AlignLeft != value)
    //            {
    //                _AlignLeft = value;
    //                mUc.AlignLeft = value;
    //                this.OnPropertyChanged("AlignLeft");
    //            }
    //        }
    //    }








    //    private string _AlignTop;
    //    public string AlignTop
    //    {
    //        get { return _AlignTop; }
    //        set
    //        {
    //            #region 逻辑判断
    //            string errorMsg = string.Empty;

    //            if (string.IsNullOrWhiteSpace(value) == true)
    //            {
    //                errorMsg = "空值";
    //            }
    //            else if (Regex.IsMatch(value, "^[0-9]{1,}$") == false)
    //            {
    //                string t = "不符合要求";
    //                errorMsg = string.IsNullOrWhiteSpace(errorMsg) ? t : $"{errorMsg}\r\n{t}";
    //            }

    //            if (string.IsNullOrWhiteSpace(errorMsg) == false)
    //            {
    //                _AlignTop = null;
    //                mUc.AlignTop = null;
    //                throw new ArgumentException(errorMsg);
    //            }

    //            #endregion

    //            //_AlignTop = value;
    //            //if (mUc.AlignTop == null || mUc.AlignTop.ToString() != value.ToString())
    //            //{
    //            //    mUc.AlignTop = value;
    //            //}
    //            //this.OnPropertyChanged("AlignTop");
    //            if (_AlignTop != value)
    //            {
    //                _AlignTop = value;
    //                mUc.AlignTop = value;
    //                this.OnPropertyChanged("AlignTop");
    //            }
    //        }
    //    }





    //    private string _Darkness;
    //    public string Darkness
    //    {
    //        get { return _Darkness; }
    //        set
    //        {
    //            #region 逻辑判断

    //            string errorMsg = string.Empty;

    //            //if (string.IsNullOrWhiteSpace(value) == true)
    //            //{
    //            //    errorMsg = "空值";
    //            //}

    //            //if (Regex.IsMatch(value, "(^[0-9]$)|(^[1-3][0-9]$)") == false)
    //            //{
    //            //    string t = "不符合要求";
    //            //    errorMsg = string.IsNullOrWhiteSpace(errorMsg) ? t : $"{errorMsg}\r\n{t}";
    //            //}

    //            //if (string.IsNullOrWhiteSpace(errorMsg) == false)
    //            //{
    //            //    _Darkness = null;
    //            //    mUc.Darkness = null;
    //            //    throw new ArgumentException(errorMsg);
    //            //}

    //            #endregion

    //            _Darkness = value;
    //            mUc.Darkness = value;
    //            this.OnPropertyChanged("Darkness");
    //        }
    //    }





    //    private string _Speed;
    //    public string Speed
    //    {
    //        get { return _Speed; }
    //        set
    //        {
    //            #region 逻辑判断

    //            string errorMsg = string.Empty;

    //            //if (string.IsNullOrWhiteSpace(value) == true)
    //            //{
    //            //    errorMsg = "空值";
    //            //}

    //            //if (Regex.IsMatch(value, "^[0-9]{1,}$") == false)
    //            //{
    //            //    string t = "不符合要求";
    //            //    errorMsg = string.IsNullOrWhiteSpace(errorMsg) ? t : $"{errorMsg}\r\n{t}";
    //            //}

    //            //if (string.IsNullOrWhiteSpace(errorMsg) == false)
    //            //{
    //            //    _Speed = null;
    //            //    mUc.Speed = null;
    //            //    throw new ArgumentException(errorMsg);
    //            //}

    //            #endregion

    //            // mUc.Speed = value;
    //            _Speed = value;
    //            this.OnPropertyChanged("Speed");
    //        }
    //    }
    //}
}
