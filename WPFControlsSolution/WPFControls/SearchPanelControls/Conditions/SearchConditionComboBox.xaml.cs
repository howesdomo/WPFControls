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
using System.Collections;

namespace Client.Components.SearchPanelControls
{
    public partial class SearchConditionComboBox : SearchConditionBase
    {
        public SearchConditionComboBox()
        {
            InitializeComponent();
        }

        #region [DP] DisplayMemberPath

        public static readonly DependencyProperty DisplayMemberPathProperty = DependencyProperty.Register
        (
            name: "DisplayMemberPath",
            propertyType: typeof(string),
            ownerType: typeof(SearchConditionComboBox),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public string DisplayMemberPath
        {
            get { return (string)GetValue(DisplayMemberPathProperty); }
            set { SetValue(DisplayMemberPathProperty, value); }
        }

        #endregion

        #region [DP] ComboBoxIsEnabled        

        public static readonly DependencyProperty ComboBoxIsEnabledProperty = DependencyProperty.Register
        (
            name: "ComboBoxIsEnabled",
            propertyType: typeof(bool),
            ownerType: typeof(SearchConditionComboBox),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: true,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public bool ComboBoxIsEnabled
        {
            get { return (bool)GetValue(ComboBoxIsEnabledProperty); }
            set { SetValue(ComboBoxIsEnabledProperty, value); }
        }

        #endregion

        #region [DP] IsEditable

        public static readonly DependencyProperty IsEditableProperty = DependencyProperty.Register
        (
            name: "IsEditable",
            propertyType: typeof(bool),
            ownerType: typeof(SearchConditionComboBox),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: false,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public bool IsEditable
        {
            get { return (bool)GetValue(IsEditableProperty); }
            set { SetValue(IsEditableProperty, value); }
        }

        #endregion

        #region [DP] IsReadOnly -- 配合 ComboBox 的 IsEditable 来使用

        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register
        (
            name: "IsReadOnly",
            propertyType: typeof(bool),
            ownerType: typeof(SearchConditionComboBox),
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
