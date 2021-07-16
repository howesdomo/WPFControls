using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Collections;
using System.Collections.ObjectModel;
using System.Reflection;


namespace Client.Components.SearchBarControls
{
    public class SearchCriteia : UserControl
    {
        #region Title

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(SearchCriteia));
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        #endregion

        #region [弃用](旧版) Value Property 

        //public static readonly DependencyProperty ValueProperty = DependencyProperty.Register
        //(
        //    "Value", typeof(object), typeof(SearchCriteia)
        //);
        //public object Value
        //{
        //    get
        //    {
        //        // Edit By Howe : 不会进入 if 的, 依赖属性不能够这样写逻辑去控制返回值
        //        #region 没用的
        //        //if (this.ItemsSource != null)
        //        //{
        //        //    if (this.ItemsSource is IBaseCollection)
        //        //    {
        //        //        return ((IBaseCollection)this.ItemsSource).ICheckedList;
        //        //    }
        //        //    //this.Value = new ObservableCollection<VirtualModel>();

        //        ////    //ObservableCollection<VirtualModel> list = (ObservableCollection<VirtualModel>)this.Value;
        //        ////    //foreach (var item in this.ItemsSource)
        //        ////    //{
        //        ////    //    if (((VirtualModel)item).IsChecked)
        //        ////    //    {
        //        ////    //        list.Add((VirtualModel)item);
        //        ////    //    }
        //        ////    //}
        //        ////}
        //        #endregion

        //        return (object)GetValue(ValueProperty);
        //    }
        //    set
        //    {
        //        SetValue(ValueProperty, value);
        //    }
        //}

        #endregion

        #region (新版) Value Property

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register
        (
            name: "Value",
            propertyType: typeof(object),
            ownerType: typeof(SearchCriteia),
            validateValueCallback: new ValidateValueCallback((target) => { return true; }),
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
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
            if ((d is SearchCriteia) == false) { return; }

            SearchCriteia target = d as SearchCriteia;
            
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

        #region IListView - Add By Howe 待测试

        public static readonly DependencyProperty IListValueProperty = DependencyProperty.Register
        (
            "IListValue", typeof(object), typeof(SearchCriteia)
        );

        public object IListValue
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        #endregion

        #region ItemsSource

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register
        (
            "ItemsSource", typeof(IEnumerable), typeof(SearchCriteia)
        );

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        #endregion

        /// <summary>
        /// 搜索助手-重置按钮 具体实现
        /// </summary>
        public virtual void Reset()
        {
            this.Value = null; // TODO Value 与 IListView 都需要清空

            //if (this.ItemsSource != null && this.ItemsSource is IBaseCollection)
            //{
            //    this.ItemsSource.GetType().InvokeMember("UncheckAll", BindingFlags.InvokeMethod, null, this.ItemsSource, null);
            //}
        }

    }
}
