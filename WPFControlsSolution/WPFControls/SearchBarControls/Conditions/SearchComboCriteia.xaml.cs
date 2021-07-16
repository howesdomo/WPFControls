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

namespace Client.Components.SearchBarControls
{
    /// <summary>
    /// SearchComboCriteia.xaml 的交互逻辑
    /// </summary>
    public partial class SearchComboCriteia : SearchCriteia
    {
        public SearchComboCriteia()
        {
            InitializeComponent();
        }

        //public static readonly DependencyProperty DisplayNameProperty = DependencyProperty.Register("DisplayName", typeof(string), typeof(SearchComboCriteia));
        //public string DisplayName
        //{
        //    get
        //    {
        //        return (string)GetValue(DisplayNameProperty);
        //    }
        //    set
        //    {
        //        SetValue(DisplayNameProperty, value);
        //    }
        //}

        public static readonly DependencyProperty DisplayNameProperty = DependencyProperty.Register
        (
            name: "DisplayName",
            propertyType: typeof(string),
            ownerType: typeof(SearchComboCriteia),
            validateValueCallback: new ValidateValueCallback((target) => { return target is null || target is string; }),
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: onDisplayName_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public string DisplayName
        {
            get { return (string)GetValue(DisplayNameProperty); }
            set { SetValue(DisplayNameProperty, value); }
        }

        public static void onDisplayName_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((d is SearchComboCriteia) == false) { return; }

            var target = (d as SearchComboCriteia);
            if (e.NewValue == null || string.IsNullOrEmpty(e.NewValue.ToString()))
            {
                target.cbx.DisplayMemberPath = "DisplayName"; // 显示路径默认路径为 DisplayName
            }
            else
            {
                target.cbx.DisplayMemberPath = e.NewValue.ToString();
            }
        }





        //public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(SearchComboCriteia));
        //public object SelectedItem
        //{
        //    get
        //    {
        //        return (object)GetValue(SelectedItemProperty);
        //    }
        //    set
        //    {
        //        SetValue(SelectedItemProperty, value);
        //    }
        //}

    }
}
