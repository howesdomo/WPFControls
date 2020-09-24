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
using System.Windows.Navigation;
using System.Windows.Shapes;

/// <summary>
/// V 1.0.0 - 2020-09-23 18:01:44
/// 首次创建, 用来代替 UcWait, 功能更强大 更灵活
/// </summary>
namespace WPFControls
{
    /// <summary>
    /// Interaction logic for UcBusyIndicator.xaml
    /// </summary>
    public partial class UcBusyIndicator : UserControl
    {
        public UcBusyIndicator()
        {
            InitializeComponent();
            if (string.IsNullOrWhiteSpace(this.txtMsg.Text))
            {
                this.txtMsg.Text = DefalutBusyContent;
            }
        }

        private void execute()
        {
            if (this.IsBusy)
            {
                this.gWait.Visibility = Visibility.Visible;
            }
            else
            {
                this.gWait.Visibility = Visibility.Hidden;
            }

            if (IsResetBusyContentPerExecute)
            {
                this.BusyContent = DefalutBusyContent;
            }
        }

        #region [DP] IsBusy

        public static readonly DependencyProperty IsBusyProperty = DependencyProperty.Register
        (
            name: "IsBusy",
            propertyType: typeof(bool),
            ownerType: typeof(UcBusyIndicator),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: false,
                propertyChangedCallback: onIsBusy_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }

        public static void onIsBusy_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((d is UcBusyIndicator) == false) { return; }
            var target = d as UcBusyIndicator;
            target.execute();
        }

        #endregion

        private static string sbc { get { return "请稍候..."; } }

        #region [DP] Defalut_BusyContent

        public static readonly DependencyProperty DefalutBusyContentProperty = DependencyProperty.Register
        (
            name: "DefalutBusyContent",
            propertyType: typeof(string),
            ownerType: typeof(UcBusyIndicator),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: sbc,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        /// <summary>
        /// 若设置 IsResetBusyContentPerExecute = True 时 (默认值为 True )
        /// 每次执行都会重置 BusyContent 信息成为 DefaultBusyContent
        /// </summary>
        public string DefalutBusyContent
        {
            get { return (string)GetValue(DefalutBusyContentProperty); }
            set { SetValue(DefalutBusyContentProperty, value); }
        }

        #endregion

        #region [DP] BusyContent

        public static readonly DependencyProperty BusyContentProperty = DependencyProperty.Register
        (
            name: "BusyContent",
            propertyType: typeof(string),
            ownerType: typeof(UcBusyIndicator),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: sbc,
                propertyChangedCallback: onBusyContent_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public string BusyContent
        {
            get { return (string)GetValue(BusyContentProperty); }
            set { SetValue(BusyContentProperty, value); }
        }

        public static void onBusyContent_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((d is UcBusyIndicator) == false) { return; }
            var target = d as UcBusyIndicator;
            target.txtMsg.Text = e.NewValue.ToString();
        }

        #endregion

        #region [DP] IsResetBusyContentPerExecute

        public static readonly DependencyProperty IsResetBusyContentPerExecuteProperty = DependencyProperty.Register
        (
            name: "IsResetBusyContentPerExecute",
            propertyType: typeof(bool),
            ownerType: typeof(UcBusyIndicator),
            validateValueCallback: null, // new ValidateValueCallback((toValidate) => { return true; }),
            typeMetadata: new PropertyMetadata
            (
                defaultValue: true,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        /// <summary>
        /// 每次显示完毕后是否重置 BusyContent 信息
        /// </summary>
        public bool IsResetBusyContentPerExecute
        {
            get { return (bool)GetValue(IsResetBusyContentPerExecuteProperty); }
            set { SetValue(IsResetBusyContentPerExecuteProperty, value); }
        }

        #endregion





        // 将 BusyIndicator 的 DP 暴露给程序员使用

        #region [DP] BusyIndicatorPathData <- BusyIndicator [DP] PathData

        public static readonly DependencyProperty BusyIndicatorPathDataProperty = DependencyProperty.Register
        (
            name: "BusyIndicatorPathData",
            propertyType: typeof(string),
            ownerType: typeof(UcBusyIndicator),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: onBusyIndicatorPathData_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public string BusyIndicatorPathData
        {
            get { return (string)GetValue(BusyIndicatorPathDataProperty); }
            set { SetValue(BusyIndicatorPathDataProperty, value); }
        }

        public static void onBusyIndicatorPathData_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((d is UcBusyIndicator) == false) { return; }
            var target = d as UcBusyIndicator;
            target.busyIndicator.PathData = e.NewValue.ToString();
        }

        #endregion

        #region [DP] BusyIndicatorStroke <- BusyIndicator [DP] PathStroke

        public static readonly DependencyProperty BusyIndicatorStrokeProperty = DependencyProperty.Register
        (
            name: "BusyIndicatorStroke",
            propertyType: typeof(System.Windows.Media.SolidColorBrush),
            ownerType: typeof(UcBusyIndicator),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: onBusyIndicatorStroke_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public System.Windows.Media.SolidColorBrush BusyIndicatorStroke
        {
            get { return (System.Windows.Media.SolidColorBrush)GetValue(BusyIndicatorStrokeProperty); }
            set { SetValue(BusyIndicatorStrokeProperty, value); }
        }

        public static void onBusyIndicatorStroke_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((d is UcBusyIndicator) == false) { return; }
            var target = d as UcBusyIndicator;
            target.busyIndicator.PathStroke = (System.Windows.Media.SolidColorBrush)e.NewValue;
        }

        #endregion

        #region [DP] BusyIndicatorFill <- BusyIndicator [DP] PathFill

        public static readonly DependencyProperty BusyIndicatorFillProperty = DependencyProperty.Register
        (
            name: "BusyIndicatorFill",
            propertyType: typeof(System.Windows.Media.SolidColorBrush),
            ownerType: typeof(UcBusyIndicator),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: onBusyIndicatorFill_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public System.Windows.Media.SolidColorBrush BusyIndicatorFill
        {
            get { return (System.Windows.Media.SolidColorBrush)GetValue(BusyIndicatorFillProperty); }
            set { SetValue(BusyIndicatorFillProperty, value); }
        }

        public static void onBusyIndicatorFill_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((d is UcBusyIndicator) == false) { return; }
            var target = d as UcBusyIndicator;
            target.busyIndicator.PathFill = (System.Windows.Media.SolidColorBrush)e.NewValue;
        }

        #endregion

        #region [DP] BusyIndicatorScale <- BusyIndicator [DP] IndicatorScale


        public static readonly DependencyProperty BusyIndicatorScaleProperty = DependencyProperty.Register
        (
            name: "BusyIndicatorScale",
            propertyType: typeof(double),
            ownerType: typeof(UcBusyIndicator),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: 1d,
                propertyChangedCallback: onBusyIndicatorScale_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public double BusyIndicatorScale
        {
            get { return (double)GetValue(BusyIndicatorScaleProperty); }
            set { SetValue(BusyIndicatorScaleProperty, value); }
        }

        public static void onBusyIndicatorScale_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((d is UcBusyIndicator) == false) { return; }
            var target = d as UcBusyIndicator;

            if (double.TryParse(e.NewValue.ToString(), out double value) == true)
            {
                target.busyIndicator.PathScale = value;
            }
        }

        #endregion

    }
}
