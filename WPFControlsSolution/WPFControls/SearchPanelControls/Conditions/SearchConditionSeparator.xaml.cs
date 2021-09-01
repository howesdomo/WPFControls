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
    public partial class SearchConditionSeparator : SearchConditionBase
    {        
        public SearchConditionSeparator()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty SeparatorMarginProperty = DependencyProperty.Register
        (
            name: "SeparatorMargin",
            propertyType: typeof(Thickness),
            ownerType: typeof(SearchConditionSeparator),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: new Thickness(0d, 6d, 0d, 6d),
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public Thickness SeparatorMargin
        {
            get { return (Thickness)GetValue(SeparatorMarginProperty); }
            set { SetValue(SeparatorMarginProperty, value); }
        }
    }
}
