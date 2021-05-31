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

                if (scrollview.Content == null)
                {
                    scrollview.Loaded += (s0, e0) =>
                    {
                        var target = s0 as ScrollViewer;
                        var itemsControl = WPFControlsUtils.FindChildOfType<ItemsControl>(target);

                        if (itemsControl != null) // 对应方法1
                        {
                            var dpDescriptor = System.ComponentModel.DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(ItemsControl));

                            if (GetIsEnabled(target) == false)
                            {
                                // TODO 无法移除匿名函数, 使用匿名函数是因为无法获取 ScrollViewer
                                // dpDescriptor.RemoveValueChanged(itemsControl, );
                            }
                            else
                            {
                                dpDescriptor.AddValueChanged(itemsControl, (s1, e1) =>
                                {
                                    target.ScrollToBottom();
                                });
                            }
                        }
                    };
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
            if (e.NewValue == null)
            {
                return;
            }

            if (sender is ScrollViewer) // 对应方法1
            {
                var scrollViewer = sender as ScrollViewer;

                if ((bool)e.NewValue == true)
                {
                    if (e.OldValue != null && (bool)e.OldValue == true) { return; }

                    scrollViewer.ScrollChanged += Control_ScrollChanged;
                    scrollViewer.ScrollToEnd();
                    return;
                }
                else
                // if ((bool)e.NewValue == false)
                {
                    if (e.OldValue == null || (bool)e.OldValue == false) { return; }

                    scrollViewer.ScrollChanged -= Control_ScrollChanged;
                    return;
                }
            }

            #region [旧版]

            //// ListBox / ListView / DataGrid 基类都是 Selector
            //if (sender is System.Windows.Controls.Primitives.Selector selector)
            //{
            //    ScrollViewer scrollViewer = WPFControlsUtils.FindChildOfType<ScrollViewer>(selector);
            //    if ((bool)e.NewValue == true)
            //    {
            //        if (e.OldValue != null && (bool)e.OldValue == true) { return; }

            //        if (scrollViewer == null)
            //        {
            //            // 在 ListBox / ListView / DataGrid 中找不到 ScrollViewer
            //            // 必须要等待加载完毕后才能找到旗下的 ScrollViewer
            //            selector.Loaded += (s0, e0) => { Sub(s0); };
            //            return;
            //        }
            //        else
            //        {
            //            Sub(selector);
            //            return;
            //        }
            //    }
            //    else
            //    // if ((bool)e.NewValue == false)
            //    {
            //        if (e.OldValue == null || (bool)e.OldValue == false) { return; }

            //        UnSub(scrollViewer);
            //        return;
            //    }
            //}

            //if (sender is ItemsControl itemsControl) // 对应方法2 // 此处其实可以合并到上面的 if 中 ==> if
            //{
            //    ScrollViewer scrollViewer = WPFControlsUtils.FindChildOfType<ScrollViewer>(itemsControl);
            //    if ((bool)e.NewValue == true)
            //    {
            //        if (e.OldValue != null && (bool)e.OldValue == true) { return; }

            //        if (scrollViewer == null)
            //        {
            //            // 在 ItemsControl 中找不到 ScrollViewer
            //            // 必须要等待加载完毕后才能找到旗下的 ScrollViewer
            //            itemsControl.Loaded += (s0, e0) => { Sub(s0); };
            //            return;
            //        }
            //        else
            //        {
            //            Sub(itemsControl);
            //            return;
            //        }
            //    }
            //    else
            //    // if ((bool)e.NewValue == false)
            //    {
            //        if (e.OldValue == null || (bool)e.OldValue == false) { return; }

            //        UnSub(scrollViewer);
            //        return;
            //    }
            //}

            #endregion

            // 新版 将旧版2种情况合并起来
            // 1. ListBox / ListView / DataGrid 基类都是 Selector
            // 2. ItemsControl
            if (sender is System.Windows.Controls.Primitives.Selector || sender is ItemsControl)
            {
                var control = sender as System.Windows.Controls.Control;

                ScrollViewer scrollViewer = WPFControlsUtils.FindChildOfType<ScrollViewer>(control);
                if ((bool)e.NewValue == true)
                {
                    if (e.OldValue != null && (bool)e.OldValue == true) { return; }

                    if (scrollViewer == null)
                    {
                        // 在 1. ListBox / ListView / DataGrid 中找不到 ScrollViewer
                        // 或 2. ItemsControl 中找不到 ScrollViewer
                        // 必须要等待加载完毕后才能找到旗下的 ScrollViewer
                        control.Loaded += (s0, e0) => { Sub(s0); };
                        return;
                    }
                    else
                    {
                        Sub(control);
                        return;
                    }
                }
                else
                // if ((bool)e.NewValue == false)
                {
                    if (e.OldValue == null || (bool)e.OldValue == false) { return; }

                    UnSub(scrollViewer);
                    return;
                }
            }

        }

        private static void Control_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var scrollViewer = sender as ScrollViewer;

            // If we are close enough to the bottom...
            if (scrollViewer.ScrollableHeight - scrollViewer.VerticalOffset < 2) // TODO 设置自动滚动到最低的差异值
            {
                // Scroll to the bottom
                scrollViewer.ScrollToEnd();
            }
        }

        static void Sub(object s0)
        {
            // 又由于要兼容 ItemsControl, 所以将 obj 转换为 System.Windows.Controls.Control
            // var source = s0 as System.Windows.Controls.Primitives.Selector; 
            var source = s0 as System.Windows.Controls.Control;

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

            target.ScrollChanged += Control_ScrollChanged;
            target.ScrollToEnd();
        }

        static void UnSub(ScrollViewer target)
        {
            if (target == null)
            {
                return;
            }

            target.ScrollChanged -= Control_ScrollChanged;
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
