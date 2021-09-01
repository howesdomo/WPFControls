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

namespace Client.Components.SearchPanelControls
{
    /// <summary>
    /// V 1.0.0 - 2021-08-25 17:56:14
    /// 重写并整理代码
    /// </summary>
    public partial class SearchConditionDateArea : SearchConditionBase
    {
        #region [DP] FromTitle

        public static readonly DependencyProperty FromTitleProperty = DependencyProperty.Register
        (
            name: "FromTitle",
            propertyType: typeof(string),
            ownerType: typeof(SearchConditionDateArea),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: "从",
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public string FromTitle
        {
            get { return (string)GetValue(FromTitleProperty); }
            set { SetValue(FromTitleProperty, value); }
        }

        #endregion

        #region [DP] ToTitle

        public static readonly DependencyProperty ToTitleProperty = DependencyProperty.Register
        (
            name: "ToTitle",
            propertyType: typeof(string),
            ownerType: typeof(SearchConditionDateArea),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: "至",
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public string ToTitle
        {
            get { return (string)GetValue(ToTitleProperty); }
            set { SetValue(ToTitleProperty, value); }
        }

        #endregion

        #region [DP] FromDate

        public static readonly DependencyProperty FromDateProperty = DependencyProperty.Register
        (
            name: "FromDate",
            propertyType: typeof(DateTime?),
            ownerType: typeof(SearchConditionDateArea),
            validateValueCallback: null,
            typeMetadata: new FrameworkPropertyMetadata
            (
                defaultValue: null,
                flags: FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                propertyChangedCallback: onFromDate_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public DateTime? FromDate
        {
            get { return (DateTime?)GetValue(FromDateProperty); }
            set { SetValue(FromDateProperty, value); }
        }

        public static void onFromDate_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SearchConditionDateArea sdc)
            {
                if ((DateTime?)e.NewValue > sdc.ToDate && sdc.ToDate.HasValue)
                {
                    if (sdc.FromDateError != null)
                    {
                        sdc.FromDateError(d, e);
                    }
                    sdc.FromDate = (DateTime?)e.OldValue;
                    //sdc.ValidatingError = "结束日期不能小于开始日期";
                }
            }
        }

        #endregion

        #region [DP] ToDate

        public static readonly DependencyProperty ToDateProperty = DependencyProperty.Register
        (
            name: "ToDate",
            propertyType: typeof(DateTime?),
            ownerType: typeof(SearchConditionDateArea),
            validateValueCallback: null,
            typeMetadata: new FrameworkPropertyMetadata
            (
                defaultValue: null,
                flags: FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                propertyChangedCallback: onToDate_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public DateTime? ToDate
        {
            get { return (DateTime?)GetValue(ToDateProperty); }
            set { SetValue(ToDateProperty, value); }
        }

        public static void onToDate_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SearchConditionDateArea sdc)
            {
                if ((DateTime?)e.NewValue < sdc.FromDate && sdc.FromDate.HasValue)
                {
                    if (sdc.ToDateError != null)
                    {
                        sdc.ToDateError(d, e);
                    }
                    sdc.ToDate = (DateTime?)e.OldValue;
                }
            }
        }

        #endregion

        #region [DP] Max~Min - From~ToDate

        public static readonly DependencyProperty MinFromDateProperty = DependencyProperty.Register("MinFromDate", typeof(DateTime?), typeof(SearchConditionDateArea));
        public DateTime? MinFromDate
        {
            get { return (DateTime?)GetValue(MinFromDateProperty); }
            set { SetValue(MinFromDateProperty, value); }
        }

        public static readonly DependencyProperty MaxFromDateProperty = DependencyProperty.Register("MaxFromDate", typeof(DateTime?), typeof(SearchConditionDateArea));
        public DateTime? MaxFromDate
        {
            get { return (DateTime?)GetValue(MaxFromDateProperty); }
            set { SetValue(MaxFromDateProperty, value); }
        }

        public static readonly DependencyProperty MinToDateProperty = DependencyProperty.Register("MinToDate", typeof(DateTime?), typeof(SearchConditionDateArea));
        public DateTime? MinToDate
        {
            get { return (DateTime?)GetValue(MinToDateProperty); }
            set { SetValue(MinToDateProperty, value); }
        }

        public static readonly DependencyProperty MaxToDateProperty = DependencyProperty.Register("MaxToDate", typeof(DateTime?), typeof(SearchConditionDateArea));
        public DateTime? MaxToDate
        {
            get { return (DateTime?)GetValue(MaxToDateProperty); }
            set { SetValue(MaxToDateProperty, value); }
        }

        #endregion

        #region [DP] FromDateIsEnabled

        public static readonly DependencyProperty FromDateIsEnabledProperty = DependencyProperty.Register
        (
            name: "FromDateIsEnabled",
            propertyType: typeof(bool),
            ownerType: typeof(SearchConditionDateArea),
            validateValueCallback: null, // new ValidateValueCallback((toValidate) => { return true; }),
            typeMetadata: new PropertyMetadata
            (
                defaultValue: true,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public bool FromDateIsEnabled
        {
            get { return (bool)GetValue(FromDateIsEnabledProperty); }
            set { SetValue(FromDateIsEnabledProperty, value); }
        }

        #endregion

        #region [DP] ToDateIsEnabled

        public static readonly DependencyProperty ToDateIsEnabledProperty = DependencyProperty.Register
        (
            name: "ToDateIsEnabled",
            propertyType: typeof(bool),
            ownerType: typeof(SearchConditionDateArea),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: true,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public bool ToDateIsEnabled
        {
            get { return (bool)GetValue(ToDateIsEnabledProperty); }
            set { SetValue(ToDateIsEnabledProperty, value); }
        }

        #endregion        

        public override void Reset()
        {
            clearDatePicker(this.dpFrom);
            this.FromDate = null;

            clearDatePicker(this.dpTo);
            this.ToDate = null;
        }

        /// <summary>
        /// 若用户在界面中胡乱输入不正确的Text值后, 界面会标识红框提示异常
        /// 执行重置方法 (Reset) 若只对绑定的 FromDate ToDate 设置 null 值, 无法修改Text值
        /// 需要用本方法清除Text值
        /// </summary>
        /// <param name="dp"></param>
        void clearDatePicker(DatePicker dp)
        {
            dp.SelectedDate = null;
            dp.SelectedDate = DateTime.Today;
        }

        public SearchConditionDateArea()
        {
            InitializeComponent();
        }

        public event PropertyChangedCallback FromDateError;
        public event PropertyChangedCallback ToDateError;
    }
}
