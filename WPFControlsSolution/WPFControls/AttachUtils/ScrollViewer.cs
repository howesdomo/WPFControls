using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Client.Controls.AttachUtils
{
    /// <summary>
    /// 启用附加属性 ( ItemsSource 的值发生改变时, 自动滚动到最底 )
    /// </summary>
    public static class ScrollToBottomOnLoad
    {
        // [DPA] 启用附加属性 ( ItemsSource 的值发生改变时, 自动滚动到最底 )

        // 附加属性 DependencyProperty.RegisterAttached
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached
        (
            name: "IsEnabled",
            propertyType: typeof(bool),
            ownerType: typeof(ScrollToBottomOnLoad),
            defaultMetadata: new FrameworkPropertyMetadata()
            {
                DefaultValue = false,
                // BindsTwoWayByDefault = true,
                DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                PropertyChangedCallback = new PropertyChangedCallback(onHandle_IsEnabled_PropertyChanged)
                // CoerceValueCallback = new CoerceValueCallback((a, b) => { return null; })
            }
        );

        public static void SetIsEnabled(DependencyObject dp, bool value)
        {
            dp.SetValue(IsEnabledProperty, value);
        }

        public static bool GetIsEnabled(DependencyObject dp)
        {
            return (bool)dp.GetValue(IsEnabledProperty);
        }

        /// <summary>
        /// 一旦你开始使用ItemsControl，你可能会遇到一个非常常见的问题：默认情况下，ItemsControl没有任何滚动条，这意味着如果内容不适合，它只是被剪裁。(From wpf-tutorial.com)
        /// 所以使用 ItemsControl 加上 ScrollViewer 有两种方法,
        /// 方法1. 外面包裹 ScrollViewer
        /// 方法2. 使用Template, 在Template设置 <ScrollViewer> <ItemsPresenter /> </ScrollViewer>
        /// </summary>
        private static void onHandle_IsEnabled_PropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is ScrollViewer scrollview)
            {
                var match = scrollview;

                if (match == null) { return; }

                if ((bool)e.NewValue)
                {
                    match.ScrollToBottom();
                }

                if (scrollview.Content == null) { return; }

                if (scrollview.Content is ItemsControl) // 对应方法1
                {
                    var dpDescriptor = System.ComponentModel.DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(ItemsControl));
                    var itemsControl = scrollview.Content as ItemsControl;

                    if (e.OldValue is bool oldValue && oldValue == true)
                    {
                        // TODO 无法移除匿名函数, 使用匿名函数是因为无法获取 ScrollViewer
                        // dpDescriptor.RemoveValueChanged(itemsControl, );
                    }

                    if (e.NewValue is bool newValue && newValue == true)
                    {
                        dpDescriptor.AddValueChanged(itemsControl, (s0, e0) =>
                        {
                            match.ScrollToBottom();
                        });
                    }
                }
            }

            // [新版] ListBox / ListView / DataGrid 基类都是 Selector
            if (sender is System.Windows.Controls.Primitives.Selector selector)
            {
                selector.Loaded += (s0, e0) =>
                {
                    var source = s0 as System.Windows.Controls.Primitives.Selector;

                    // 虽然 ListBox / ListView / DataGrid 基类都是 Selector, 并且其结构都是 Selector => Decorator => ScrollViewer
                    // Decorator border = System.Windows.Media.VisualTreeHelper.GetChild(source, 0) as Decorator;
                    // ScrollViewer target = border.Child as ScrollViewer;
                    // 但为了更加灵活, 不再规限内部的层级结构, 使用 WPFControlsUtils.FindChildOfType<T>
                    ScrollViewer target = WPFControlsUtils.FindChildOfType<ScrollViewer>(source);

                    if (target == null)
                    {
                        System.Diagnostics.Debug.WriteLine("找不到ScrollViewer");
                        System.Diagnostics.Debugger.Break();

                        return;
                    }

                    var dpDescriptor = System.ComponentModel.DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(System.Windows.Controls.Primitives.Selector));

                    if (GetIsEnabled(source) == false)
                    {
                        // 为 ItemsSource 移除监控更改事件
                        dpDescriptor.RemoveValueChanged(source, Handle_ItemsSource_Changed);
                    }
                    else
                    {
                        // 为 ItemsSource 添加监控更改事件    
                        dpDescriptor.AddValueChanged(source, Handle_ItemsSource_Changed);

                        // 先做一次滚动到最底部
                        target.ScrollToBottom();
                    }
                };
            }

            if (sender is ItemsControl) // 对应方法2
            {
                var itemsControl = sender as ItemsControl;
                itemsControl.Loaded += (s0, e0) =>
                {
                    ItemsControl source = s0 as ItemsControl;
                    ScrollViewer target = WPFControlsUtils.FindChildOfType<ScrollViewer>(source);

                    if (target == null)
                    {
                        System.Diagnostics.Debug.WriteLine("找不到ScrollViewer");
                        System.Diagnostics.Debugger.Break();

                        return;
                    }

                    var dpDescriptor = System.ComponentModel.DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(ItemsControl));

                    if (GetIsEnabled(source) == false)
                    {
                        // 为 ItemsSource 移除监控更改事件
                        dpDescriptor.RemoveValueChanged(source, Handle_ItemsSource_Changed_ForItemsControl);
                    }
                    else
                    {
                        // 为 ItemsSource 添加监控更改事件
                        dpDescriptor.AddValueChanged(source, Handle_ItemsSource_Changed_ForItemsControl);

                        // 先做一次滚动到最底部
                        target.ScrollToBottom();
                    }


                };
            }
        }

        private static void Handle_ItemsSource_Changed(object s, EventArgs e)
        {
            var source = s as System.Windows.Controls.Primitives.Selector;

            if (GetIsEnabled(source) == true)
            {
                var scrollViewer = WPFControlsUtils.FindChildOfType<ScrollViewer>(source);
                if (scrollViewer != null)
                {
                    scrollViewer.ScrollToBottom();
                }
            }
        }

        private static void Handle_ItemsSource_Changed_ForItemsControl(object s, EventArgs e)
        {
            var source = s as ItemsControl;

            if (GetIsEnabled(source) == true)
            {
                var scrollViewer = WPFControlsUtils.FindChildOfType<ScrollViewer>(source);
                if (scrollViewer != null)
                {
                    scrollViewer.ScrollToBottom();
                }
            }
        }
    }

    /// <summary>
    /// 一旦你开始使用ItemsControl，你可能会遇到一个非常常见的问题：默认情况下，ItemsControl没有任何滚动条，这意味着如果内容不适合，它只是被剪裁。(From wpf-tutorial.com)
    /// 所以使用 ItemsControl 加上 ScrollViewer 有两种方法,
    /// 方法1. 外面包裹 ScrollViewer
    /// 方法2. 使用Template, 在Template设置 <ScrollViewer> <ItemsPresenter /> </ScrollViewer>
    /// </summary>
    public static class AutoScrollToBottom
    {
        // [DPA] 启用 自动滚动到最低 附加属性

        // 附加属性 DependencyProperty.RegisterAttached
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached
        (
            name: "IsEnabled",
            propertyType: typeof(bool),
            ownerType: typeof(AutoScrollToBottom),
            defaultMetadata: new FrameworkPropertyMetadata()
            {
                DefaultValue = false,
                // BindsTwoWayByDefault = true,
                DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                PropertyChangedCallback = new PropertyChangedCallback(onHandle_IsEnabled_PropertyChanged)
                // CoerceValueCallback = new CoerceValueCallback((a, b) => { return null; })
            }
        );

        public static void SetIsEnabled(DependencyObject dp, bool value)
        {
            dp.SetValue(IsEnabledProperty, value);
        }

        public static bool GetIsEnabled(DependencyObject dp)
        {
            return (bool)dp.GetValue(IsEnabledProperty);
        }

        private static void onHandle_IsEnabled_PropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is ScrollViewer scrollview)
            {
                var match = scrollview;
                if (match == null) { return; }

                if (e.OldValue is bool oldValue && oldValue == true)
                {
                    match.ScrollChanged -= Control_ScrollChanged;
                }

                if (e.NewValue is bool newValue && newValue == true)
                {
                    match.ScrollChanged += Control_ScrollChanged;
                }
            }

            #region [旧版] ListBox / ListView / DataGrid 

            //if (sender is ListBox listbox) // ListBox 与 ListView 一致
            //{
            //    ListView k = null;
            //    var b = listbox.IsLoaded; // 需要利用 loaded 事件

            //    listbox.Loaded += (s0, e0) =>
            //    {
            //        var lb = s0 as ListBox;

            //        Decorator border = System.Windows.Media.VisualTreeHelper.GetChild(lb, 0) as Decorator;
            //        var match = border.Child as ScrollViewer;

            //        if (match == null)
            //        {
            //            return;
            //        }

            //        if (e.OldValue is bool oldValue && oldValue == true)
            //        {
            //            match.ScrollChanged -= Control_ScrollChanged;
            //        }

            //        if (e.NewValue is bool newValue && newValue == true)
            //        {
            //            match.ScrollChanged += Control_ScrollChanged;
            //        }

            //    };
            //}

            //if (sender is DataGrid dataGrid)
            //{
            //    var b = dataGrid.IsLoaded; // 需要利用 loaded 事件

            //    dataGrid.Loaded += (s0, e0) =>
            //    {
            //        var lb = s0 as DataGrid;

            //        Decorator border = System.Windows.Media.VisualTreeHelper.GetChild(lb, 0) as Decorator;
            //        var match = border.Child as ScrollViewer;

            //        if (match == null)
            //        {
            //            return;
            //        }

            //        if (e.OldValue is bool oldValue && oldValue == true)
            //        {
            //            match.ScrollChanged -= Control_ScrollChanged;
            //        }

            //        if (e.NewValue is bool newValue && newValue == true)
            //        {
            //            match.ScrollChanged += Control_ScrollChanged;
            //        }

            //    };
            //}

            #endregion

            // [新版] ListBox / ListView / DataGrid 基类都是 Selector
            if (sender is System.Windows.Controls.Primitives.Selector selector)
            {
                selector.Loaded += (s0, e0) =>
                {
                    var source = s0 as System.Windows.Controls.Primitives.Selector;

                    // 虽然 ListBox / ListView / DataGrid 基类都是 Selector, 并且其结构都是 Selector => Decorator => ScrollViewer
                    // Decorator border = System.Windows.Media.VisualTreeHelper.GetChild(source, 0) as Decorator;
                    // ScrollViewer target = border.Child as ScrollViewer;
                    // 但为了更加灵活, 不再规限内部的层级结构, 使用 WPFControlsUtils.FindChildOfType<T>
                    ScrollViewer target = WPFControlsUtils.FindChildOfType<ScrollViewer>(source);

                    if (target == null)
                    {
                        System.Diagnostics.Debug.WriteLine("找不到ScrollViewer");
                        System.Diagnostics.Debugger.Break();

                        return;
                    }

                    if (e.OldValue is bool oldValue && oldValue == true) // TODO 更改获取方式
                    {
                        target.ScrollChanged -= Control_ScrollChanged;
                    }

                    if (e.NewValue is bool newValue && newValue == true)
                    {
                        target.ScrollChanged += Control_ScrollChanged;
                    }
                };
            }

            if (sender is ItemsControl) // 对应方法2
            {
                var itemsControl = sender as ItemsControl;
                itemsControl.Loaded += (s0, e0) =>
                {
                    ItemsControl source = s0 as ItemsControl;
                    ScrollViewer target = WPFControlsUtils.FindChildOfType<ScrollViewer>(source);

                    if (target == null)
                    {
                        System.Diagnostics.Debug.WriteLine("找不到ScrollViewer");
                        System.Diagnostics.Debugger.Break();

                        return;
                    }

                    if (e.OldValue is bool oldValue && oldValue == true)
                    {
                        target.ScrollChanged -= Control_ScrollChanged;
                    }

                    if (e.NewValue is bool newValue && newValue == true)
                    {
                        target.ScrollChanged += Control_ScrollChanged;
                    }
                };
            }
        }

        private static void Control_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var scroll = sender as ScrollViewer;

            // If we are close enough to the bottom...
            if (scroll.ScrollableHeight - scroll.VerticalOffset < 2) // TODO 设置自动滚动到最低的差异值
            {
                // Scroll to the bottom
                scroll.ScrollToEnd();
            }
        }
    }

    public class WPFControlsUtils
    {
        public static T FindChildOfType<T>(DependencyObject root) where T : class
        {
            var queue = new Queue<DependencyObject>();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                DependencyObject current = queue.Dequeue();
                for (int i = System.Windows.Media.VisualTreeHelper.GetChildrenCount(current) - 1; 0 <= i; i--)
                {
                    var child = System.Windows.Media.VisualTreeHelper.GetChild(current, i);
                    var typedChild = child as T;
                    if (typedChild != null)
                    {
                        return typedChild;
                    }
                    queue.Enqueue(child);
                }
            }
            return null;
        }

    }
}
