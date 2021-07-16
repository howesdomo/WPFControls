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

namespace Client.Components.SearchBarControls
{
    /// <summary>
    /// SearchListBoxCriteia.xaml 的交互逻辑
    /// </summary>
    public partial class SearchListBoxCriteia : SearchCriteia
    {
        public SearchListBoxCriteia()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty CheckBoxVisibilityProperty = DependencyProperty.Register("CheckBoxVisibility", typeof(Visibility), typeof(SearchListBoxCriteia));

        public Visibility CheckBoxVisibility
        {
            get
            {
                return (Visibility)GetValue(CheckBoxVisibilityProperty);
            }
            set
            {
                SetValue(CheckBoxVisibilityProperty, value);
            }
        }


        #region ListBoxHeightProperty

        public static readonly DependencyProperty ListBoxHeightProperty = DependencyProperty.Register
        (
            name: "ListBoxHeight",
            propertyType: typeof(double),
            ownerType: typeof(SearchListBoxCriteia),
            validateValueCallback: new ValidateValueCallback((target) => { return double.TryParse(target.ToString(), out double t) && t >= 0; }),
            typeMetadata: new PropertyMetadata
            (
                defaultValue: 100d,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public double ListBoxHeight
        {
            get { return (double)GetValue(ListBoxHeightProperty); }
            set { SetValue(ListBoxHeightProperty, value); }
        }

        #endregion

    }
}
