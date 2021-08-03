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

namespace Client.Components.SearchBarControls
{
    /// <summary>
    /// SearchTextCriteia.xaml 的交互逻辑
    /// </summary>
    public partial class SearchDateTimeCriteia : SearchCriteia
    {
        #region [DP] FromTitle

        public static readonly DependencyProperty FromTitleProperty = DependencyProperty.Register
        (
            name: "FromTitle",
            propertyType: typeof(string),
            ownerType: typeof(SearchDateTimeCriteia),
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
            ownerType: typeof(SearchDateTimeCriteia),
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

        #region [DP] FromDateTime

        public static readonly DependencyProperty FromDateTimeProperty = DependencyProperty.Register
        (
            name: "FromDateTime",
            propertyType: typeof(DateTime?),
            ownerType: typeof(SearchDateTimeCriteia),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: onFromDate_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public DateTime? FromDateTime
        {
            get { return (DateTime?)GetValue(FromDateTimeProperty); }
            set { SetValue(FromDateTimeProperty, value); }
        }

        public static void onFromDate_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SearchDateTimeCriteia target)
            {
                if ((DateTime?)e.NewValue > target.ToDateTime && target.ToDateTime.HasValue)
                {
                    if (target.FromDateError != null)
                    {
                        target.FromDateError(d, e);
                    }
                    target.FromDateTime = (DateTime?)e.OldValue;
                }
            }
        }

        #endregion

        #region [DP] ToDateTime

        public static readonly DependencyProperty ToDateTimeProperty = DependencyProperty.Register
        (
            name: "ToDateTime",
            propertyType: typeof(DateTime?),
            ownerType: typeof(SearchDateTimeCriteia),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: onToDate_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public DateTime? ToDateTime
        {
            get { return (DateTime?)GetValue(ToDateTimeProperty); }
            set { SetValue(ToDateTimeProperty, value); }
        }

        public static void onToDate_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SearchDateTimeCriteia target)
            {
                if ((DateTime?)e.NewValue < target.FromDateTime && target.FromDateTime.HasValue)
                {
                    if (target.ToDateError != null)
                    {
                        target.ToDateError(d, e);
                    }
                    target.ToDateTime = (DateTime?)e.OldValue;
                }
            }
        }

        #endregion

        #region [DP] Max~Min - From~ToDate

        public static readonly DependencyProperty MinFromDateProperty = DependencyProperty.Register("MinFromDate", typeof(DateTime?), typeof(SearchDateTimeCriteia));
        public DateTime? MinFromDate
        {
            get { return (DateTime?)GetValue(MinFromDateProperty); }
            set { SetValue(MinFromDateProperty, value); }
        }

        public static readonly DependencyProperty MaxFromDateProperty = DependencyProperty.Register("MaxFromDate", typeof(DateTime?), typeof(SearchDateTimeCriteia));
        public DateTime? MaxFromDate
        {
            get { return (DateTime?)GetValue(MaxFromDateProperty); }
            set { SetValue(MaxFromDateProperty, value); }
        }

        public static readonly DependencyProperty MinToDateProperty = DependencyProperty.Register("MinToDate", typeof(DateTime?), typeof(SearchDateTimeCriteia));
        public DateTime? MinToDate
        {
            get { return (DateTime?)GetValue(MinToDateProperty); }
            set { SetValue(MinToDateProperty, value); }
        }

        public static readonly DependencyProperty MaxToDateProperty = DependencyProperty.Register("MaxToDate", typeof(DateTime?), typeof(SearchDateTimeCriteia));
        public DateTime? MaxToDate
        {
            get { return (DateTime?)GetValue(MaxToDateProperty); }
            set { SetValue(MaxToDateProperty, value); }
        }

        #endregion

        #region [DP] TextBoxBackground

        public static readonly DependencyProperty TextBoxBackgroundProperty = DependencyProperty.Register
        (
            name: "TextBoxBackground",
            propertyType: typeof(System.Windows.Media.Brush),
            ownerType: typeof(SearchDateTimeCriteia),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(245, 245, 245)),
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public System.Windows.Media.Brush TextBoxBackground
        {
            get { return (System.Windows.Media.Brush)GetValue(TextBoxBackgroundProperty); }
            set { SetValue(TextBoxBackgroundProperty, value); }
        }

        #endregion

        #region [DP] FromIsReadOnly

        public static readonly DependencyProperty FromIsReadOnlyProperty = DependencyProperty.Register
        (
            name: "FromIsReadOnly",
            propertyType: typeof(bool),
            ownerType: typeof(SearchDateTimeCriteia),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: false,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public bool FromIsReadOnly
        {
            get { return (bool)GetValue(FromIsReadOnlyProperty); }
            set { SetValue(FromIsReadOnlyProperty, value); }
        }

        #endregion

        #region [DP] ToIsReadOnly

        public static readonly DependencyProperty ToIsReadOnlyProperty = DependencyProperty.Register
        (
            name: "ToIsReadOnly",
            propertyType: typeof(bool),
            ownerType: typeof(SearchDateTimeCriteia),
            validateValueCallback: null, 
            typeMetadata: new PropertyMetadata
            (
                defaultValue: false,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public bool ToIsReadOnly
        {
            get { return (bool)GetValue(ToIsReadOnlyProperty); }
            set { SetValue(ToIsReadOnlyProperty, value); }
        }

        #endregion

        #region [DP] FromIsEnabled

        public static readonly DependencyProperty FromIsEnabledProperty = DependencyProperty.Register
        (
            name: "FromIsEnabled",
            propertyType: typeof(bool),
            ownerType: typeof(SearchDateTimeCriteia),
            validateValueCallback: null, // new ValidateValueCallback((toValidate) => { return true; }),
            typeMetadata: new PropertyMetadata
            (
                defaultValue: true,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public bool FromIsEnabled
        {
            get { return (bool)GetValue(FromIsEnabledProperty); }
            set { SetValue(FromIsEnabledProperty, value); }
        }

        #endregion

        #region [DP] ToIsEnabled

        public static readonly DependencyProperty ToIsEnabledProperty = DependencyProperty.Register
        (
            name: "ToIsEnabled",
            propertyType: typeof(bool),
            ownerType: typeof(SearchDateTimeCriteia),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: true,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public bool ToIsEnabled
        {
            get { return (bool)GetValue(ToIsEnabledProperty); }
            set { SetValue(ToIsEnabledProperty, value); }
        }

        #endregion

        //public static readonly DependencyProperty ValidatingErrorProperty = DependencyProperty.Register("ValidatingError", typeof(string), typeof(SearchDateTimeCriteia));
        //public string ValidatingError
        //{
        //    get
        //    {
        //        return (string)GetValue(ValidatingErrorProperty);
        //    }
        //    set
        //    {
        //        return;
        //        //SetValue(ValidatingErrorProperty, value);
        //    }
        //}

        public override void Reset()
        {
            // TODO 输入一个错误的值 ( 例如 321 ), 失去焦点后提示值异常(显示红框), 点击[重置]按钮红框不消失, 需要输入一个正确的 DateTime 值, 红框才会消失

            this.FromDateTime = null;
            this.ToDateTime = null;

            this.txtFromDateTime.Text = string.Empty;
            this.txtToDateTime.Text = string.Empty;
        }

        public SearchDateTimeCriteia()
        {
            InitializeComponent();
        }

        public event PropertyChangedCallback FromDateError;
        public event PropertyChangedCallback ToDateError;
    }
}
