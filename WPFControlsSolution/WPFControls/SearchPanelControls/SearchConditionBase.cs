using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Collections;
using System.Collections.ObjectModel;
using System.Reflection;


namespace Client.Components.SearchPanelControls
{
    public class SearchConditionBase : UserControl
    {
        #region [DP] Title

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(SearchConditionBase));
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        #endregion

        #region [DP] ItemsSource

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register
        (
            "ItemsSource", typeof(IEnumerable), typeof(SearchConditionBase)
        );

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        #endregion

        #region [DP] Value

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register
        (
            name: "Value",
            propertyType: typeof(object),
            ownerType: typeof(SearchConditionBase),
            validateValueCallback: new ValidateValueCallback((target) => { return true; }),
            typeMetadata: new FrameworkPropertyMetadata
            (
                defaultValue: null,
                flags: FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                propertyChangedCallback: onValue_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public object Value
        {
            get { return GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static void onValue_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((d is SearchConditionBase) == false) { return; }

            SearchConditionBase target = d as SearchConditionBase;

            //if (target.ItemsSource != null) // 办不到想要的效果 不能根据类型来判断需要返回到 Value 的值
            //{
            //    if (target.ItemsSource is IBaseCollection)
            //    {
            //        target.IListValue = (target.ItemsSource as IBaseCollection).ICheckedList;
            //        return;
            //    }
            //    else
            //    {
            //        var tempCollection = new ObservableCollection<VirtualModel>();
            //        foreach (object item in target.ItemsSource)
            //        {
            //            if (item is VirtualModel)
            //            {
            //                var toAdd = item as VirtualModel;
            //                if (toAdd.IsChecked == true) { tempCollection.Add(toAdd); }
            //            }
            //        }

            //        target.IListValue = tempCollection;
            //    }
            //}
        }

        #endregion

        /// <summary>
        /// 搜索助手-重置按钮 具体实现
        /// </summary>
        public virtual void Reset()
        {
            this.Value = null;
        }
    }
}
