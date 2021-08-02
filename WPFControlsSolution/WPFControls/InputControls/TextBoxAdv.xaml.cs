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

namespace Client.Components
{
    /// <summary>
    /// Interaction logic for TextBoxAdv.xaml
    /// </summary>
    public partial class TextBoxAdv : TextBox
    {
        public TextBlock mPlaceHolderTextBlock { get; set; }

        #region [DP] Placeholder

        public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register
        (
            name: "Placeholder",
            propertyType: typeof(string),
            ownerType: typeof(TextBoxAdv),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: string.Empty,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        #endregion

        #region [DP] PlaceholderColor

        public static readonly DependencyProperty PlaceholderColorProperty = DependencyProperty.Register
        (
            name: "PlaceholderColor",
            propertyType: typeof(System.Windows.Media.Brush),
            ownerType: typeof(TextBoxAdv),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: System.Windows.Media.Brushes.Gray,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public System.Windows.Media.Brush PlaceholderColor
        {
            get { return (System.Windows.Media.Brush)GetValue(PlaceholderColorProperty); }
            set { SetValue(PlaceholderColorProperty, value); }
        }

        #endregion

        #region [DP] PlaceholderFontSize

        public static readonly DependencyProperty PlaceholderFontSizeProperty = DependencyProperty.Register
        (
            name: "PlaceholderFontSize",
            propertyType: typeof(double),
            ownerType: typeof(TextBoxAdv),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (                
                defaultValue: 12d,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public double PlaceholderFontSize
        {
            get { return (double)GetValue(PlaceholderFontSizeProperty); }
            set { SetValue(PlaceholderFontSizeProperty, value); }
        }

        #endregion

        public TextBoxAdv()
        {
            InitializeComponent();
            
        }
    }
}
