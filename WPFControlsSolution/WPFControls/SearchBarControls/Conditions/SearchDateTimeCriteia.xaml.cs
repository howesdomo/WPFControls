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

        #region [DP] FromDate

        public static readonly DependencyProperty FromDateProperty = DependencyProperty.Register
        (
            name: "FromDate",
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

        public DateTime? FromDate
        {
            get { return (DateTime?)GetValue(FromDateProperty); }
            set { SetValue(FromDateProperty, value); }
        }

        public static void onFromDate_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SearchDateTimeCriteia sdc)
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
            ownerType: typeof(SearchDateTimeCriteia),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
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
            if (d is SearchDateTimeCriteia sdc)
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
            this.FromDate = null;
            this.ToDate = null;
        }

        public SearchDateTimeCriteia()
        {
            InitializeComponent();
        }

        public event PropertyChangedCallback FromDateError;
        public event PropertyChangedCallback ToDateError;
    }
}
