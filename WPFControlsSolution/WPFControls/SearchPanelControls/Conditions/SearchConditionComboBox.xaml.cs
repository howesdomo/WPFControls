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
    /// <summary>
    /// V 1.0.0 - 2021-08-25 17:56:14
    /// 重写并整理代码
    /// </summary>
    public partial class SearchConditionComboBox : SearchConditionBase
    {
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

        public SearchConditionComboBox()
        {
            InitializeComponent();
        }

        public override void Reset()
        {
            // 若用户在界面中胡乱输入不正确的Text值后
            // 执行重置方法 (Reset) 若只对绑定的 Value 设置 null 值, 无法修改Text值
            // 需要清除ComboBox的Text属性
            this.comboBox.Text = string.Empty;
            this.Value = null;
        }
    }
}
