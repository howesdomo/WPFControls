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
using System.Windows.Shapes;

namespace WPFControls.MessageBoxExtraContent
{
    /// <summary>
    /// Interaction logic for CcDatePicker.xaml
    /// </summary>
    public partial class CcDatePicker : ContentControl
    {
        public CcDatePicker()
        {
            InitializeComponent();
        }

        #region [DP] DisplayDateStart

        public static readonly DependencyProperty DisplayDateStartProperty = DependencyProperty.Register
        (
            name: "DisplayDateStart",
            propertyType: typeof(DateTime?),
            ownerType: typeof(CcDatePicker),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public DateTime? DisplayDateStart
        {
            get { return (DateTime?)GetValue(DisplayDateStartProperty); }
            set { SetValue(DisplayDateStartProperty, value); }
        }

        #endregion

        #region [DP] DisplayDateEnd

        public static readonly DependencyProperty DisplayDateEndProperty = DependencyProperty.Register
        (
            name: "DisplayDateEnd",
            propertyType: typeof(DateTime?),
            ownerType: typeof(CcDatePicker),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public DateTime? DisplayDateEnd
        {
            get { return (DateTime?)GetValue(DisplayDateEndProperty); }
            set { SetValue(DisplayDateEndProperty, value); }
        }

        #endregion

    }
}
