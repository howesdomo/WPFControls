using Client.Components.PrinterPanel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Client.Components
{
    /// <summary>
    /// V 1.0.0 - 2020-11-05 11:36:45
    /// 首次创建, 用于斑马打印机选项
    /// </summary>
    public partial class UcPrinterPanelZebra : UserControl, INotifyPropertyChanged
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

            var temp = PrinterUtils.GetPrinterList(isContainUpdateListItem: true);
            this.PrinterList = PrinterUtils.PrinterOrderBy(temp, this.PriorityPrinterList, this.PriorityPaperSizeList);

            var defaultPrinterName = new System.Drawing.Printing.PrintDocument().PrinterSettings.PrinterName;
            this.SelectedItem = this.PrinterList.FirstOrDefault(i => i.DisplayName == defaultPrinterName);
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

        #region SelectedPrinter

        public static readonly DependencyProperty SelectedPrinterProperty = DependencyProperty.Register
        (
            name: "SelectedPrinter",
            propertyType: typeof(Printer),
            ownerType: typeof(UcPrinterPanelZebra)
        );

        public Printer SelectedPrinter
        {
            get { return (Printer)GetValue(SelectedPrinterProperty); }
            set { SetValue(SelectedPrinterProperty, value); }
        }

        private Printer _SelectedItem;
        public Printer SelectedItem
        {
            get { return this._SelectedItem; }
            set
            {
                if (value != null && value.DisplayName == PrinterUtils.sUpdateName) // 选择了 刷新 项
                {
                    var temp = PrinterUtils.GetPrinterList(isContainUpdateListItem: true);
                    this.PrinterList = PrinterUtils.PrinterOrderBy(temp, this.PriorityPrinterList, this.PriorityPaperSizeList);
                }
                else
                {
                    this._SelectedItem = value;
                    this.SelectedPrinter = value;
                    this.OnPropertyChanged("SelectedItem");
                }
            }
        }

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
                defaultValue: null,
                propertyChangedCallback: onAlignLeft_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public object AlignLeft
        {
            get { return (object)GetValue(AlignLeftProperty); }
            set { SetValue(AlignLeftProperty, value); }
        }

        public static void onAlignLeft_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((d is UcPrinterPanelZebra) == false) { return; }
            var target = d as UcPrinterPanelZebra;

            target.UcAlignLeft = (string)e.NewValue;
        }


        private string _AlignLeft;
        public string UcAlignLeft
        {
            get { return _AlignLeft; }
            set
            {
                #region 逻辑判断

                string errorMsg = string.Empty;

                if (string.IsNullOrWhiteSpace(value) == true)
                {
                    errorMsg = "空值";
                }
                
                if (Regex.IsMatch(value, "^[0-9]{1,}$") == false)
                {
                    string t = "不符合要求";
                    errorMsg = string.IsNullOrWhiteSpace(errorMsg) ? t : $"{errorMsg}\r\n{t}";
                }

                if (string.IsNullOrWhiteSpace(errorMsg) == false)
                {
                    _AlignLeft = null;
                    AlignLeft = null;
                    throw new ArgumentException(errorMsg);
                }

                #endregion

                _AlignLeft = value;
                AlignLeft = value;
                this.OnPropertyChanged("UcAlignLeft");
            }
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

        public object AlignTop
        {
            get { return (object)GetValue(AlignTopProperty); }
            set { SetValue(AlignTopProperty, value); }
        }

        public static void onAlignTop_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((d is UcPrinterPanelZebra) == false) { return; }
            var target = d as UcPrinterPanelZebra;

            target.UcAlignTop = (string)e.NewValue;
        }


        private string _AlignTop;
        public string UcAlignTop
        {
            get { return _AlignTop; }
            set
            {
                #region 逻辑判断
                string errorMsg = string.Empty;

                if (string.IsNullOrWhiteSpace(value) == true)
                {
                    errorMsg = "空值";
                }

                if (Regex.IsMatch(value, "^[0-9]{1,}$") == false)                    
                {
                    string t = "不符合要求";
                    errorMsg = string.IsNullOrWhiteSpace(errorMsg) ? t : $"{errorMsg}\r\n{t}";
                }

                if (string.IsNullOrWhiteSpace(errorMsg) == false)
                {
                    _AlignTop = null;
                    AlignTop = null;
                    throw new ArgumentException(errorMsg);
                }

                #endregion

                _AlignTop = value;
                if (AlignTop == null || AlignTop.ToString() != value.ToString())
                {
                    AlignTop = value;
                }
                this.OnPropertyChanged("UcAlignTop");
            }
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

            target.UcDarkness = (string)e.NewValue;
        }

        private string _Darkness;
        public string UcDarkness
        {
            get { return _Darkness; }
            set
            {
                #region 逻辑判断

                string errorMsg = string.Empty;

                if (string.IsNullOrWhiteSpace(value) == true)
                {
                    errorMsg = "空值";
                }

                if (Regex.IsMatch(value, "(^[0-9]$)|(^[1-3][0-9]$)") == false)
                {
                    string t = "不符合要求";
                    errorMsg = string.IsNullOrWhiteSpace(errorMsg) ? t : $"{errorMsg}\r\n{t}";
                }

                if (string.IsNullOrWhiteSpace(errorMsg) == false)
                {
                    _Darkness = null;
                    Darkness = null;
                    throw new ArgumentException(errorMsg);
                }

                #endregion

                _Darkness = value;
                Darkness = value;
                this.OnPropertyChanged("UcDarkness");
            }
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

            target.UcSpeed = (string)e.NewValue;
        }

        private string _Speed;
        public string UcSpeed
        {
            get { return _Speed; }
            set
            {
                #region 逻辑判断

                string errorMsg = string.Empty;

                if (string.IsNullOrWhiteSpace(value) == true)
                {
                    errorMsg = "空值";
                }

                if (Regex.IsMatch(value, "^[0-9]{1,}$") == false)
                {
                    string t = "不符合要求";
                    errorMsg = string.IsNullOrWhiteSpace(errorMsg) ? t : $"{errorMsg}\r\n{t}";
                }

                if (string.IsNullOrWhiteSpace(errorMsg) == false)
                {
                    _Speed = null;
                    Speed = null;
                    throw new ArgumentException(errorMsg);
                }

                #endregion

                _Speed = value;
                Speed = value;
                this.OnPropertyChanged("UcSpeed");
            }
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

        #region INotifyPropertyChanged成员

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
