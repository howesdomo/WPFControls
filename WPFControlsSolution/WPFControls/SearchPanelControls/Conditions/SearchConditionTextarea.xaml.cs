﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Client.Components.SearchPanelControls
{
    public partial class SearchConditionTextarea : SearchConditionBase
    {
        public SearchConditionTextarea()
        {
            InitializeComponent();
        }

        #region [DP] Placeholder

        public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register
        (
            name: "Placeholder",
            propertyType: typeof(string),
            ownerType: typeof(SearchConditionTextarea),
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
            ownerType: typeof(SearchConditionTextarea),
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
            ownerType: typeof(SearchConditionTextarea),
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

        #region [DP] TextBoxHeight

        public static readonly DependencyProperty TextBoxHeightProperty = DependencyProperty.Register
        (
            name: "TextBoxHeight",
            propertyType: typeof(double),
            ownerType: typeof(SearchConditionTextarea),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: 80d,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public double TextBoxHeight
        {
            get { return (double)GetValue(TextBoxHeightProperty); }
            set { SetValue(TextBoxHeightProperty, value); }
        }

        #endregion

        #region [DP] TextBoxBackground

        public static readonly DependencyProperty TextBoxBackgroundProperty = DependencyProperty.Register
        (
            name: "TextBoxBackground",
            propertyType: typeof(System.Windows.Media.Brush),
            ownerType: typeof(SearchConditionTextarea),
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

        #region [DP] IsReadOnly

        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register
        (
            name: "IsReadOnly",
            propertyType: typeof(bool),
            ownerType: typeof(SearchConditionTextarea),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: false,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        #endregion
    }
}
