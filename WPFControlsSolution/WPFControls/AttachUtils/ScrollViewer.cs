using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Client.Controls.AttachUtils
{
    // V 1.0.0 - 2021-06-01 11:23:14
    // 首次创建

    /// <summary>
    /// <para>启用附加属性 ( ItemsSource 的值发生改变时, 自动滚动到最底 )</para>
    /// </summary>
    public class ScrollToBottomOnLoad
    {
        // 附加属性 DependencyProperty.RegisterAttached

        /// <summary>
        /// <para>[DPA] 启用附加属性 ( ItemsSource 的值发生改变时, 自动滚动到最底 )</para>
        /// </summary>
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

        /// <summary>
        /// Set [DPA] 启用附加属性 ( ItemsSource 的值发生改变时, 自动滚动到最底 )
        /// </summary>
        /// <param name="dp"></param>
        /// <param name="value"></param>
        public static void SetIsEnabled(DependencyObject dp, bool value)
        {
            dp.SetValue(IsEnabledProperty, value);
        }

        /// <summary>
        /// Get [DPA] 启用附加属性 ( ItemsSource 的值发生改变时, 自动滚动到最底 )
        /// </summary>
        /// <param name="dp"></param>
        /// <returns></returns>
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
            if (e.NewValue == null)
            {
                return;
            }

            if (sender is ScrollViewer)
            {
                var scrollViewer = sender as ScrollViewer;

                if ((bool)e.NewValue == true)
                {
                    if (e.OldValue != null && (bool)e.OldValue == true) { return; }

                    if (scrollViewer.Content == null)
                    {
                        scrollViewer.Loaded += (s0, e0) =>
                        {
                            var target = s0 as ScrollViewer;
                            var itemsControl = WPFControlsUtils.FindChildOfType<ItemsControl>(target);

                            if (itemsControl != null) // 对应方法1
                            {
                                var dpDescriptor = System.ComponentModel.DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, itemsControl.GetType());
                                dpDescriptor.AddValueChanged(itemsControl, Handle_ItemsSource_Changed_ForItemsControl);

                                target.ScrollToEnd();
                            }
                        };
                        return;
                    }
                    else
                    {
                        var dpDescriptor = System.ComponentModel.DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, scrollViewer.Content.GetType());
                        dpDescriptor.AddValueChanged(scrollViewer.Content, Handle_ItemsSource_Changed_ForItemsControl);

                        scrollViewer.ScrollToEnd();
                        return;
                    }
                }
                else
                // if ((bool)e.NewValue == false)
                {
                    if (e.OldValue == null || (bool)e.OldValue == false) { return; }

                    var dpDescriptor = System.ComponentModel.DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, scrollViewer.Content.GetType());
                    dpDescriptor.RemoveValueChanged(scrollViewer.Content, Handle_ItemsSource_Changed_ForItemsControl);

                    return;
                }
            }

            // [新版] 合并
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

                    UnSub(control);
                    return;
                }
            }
        }

        static void Handle_ItemsSource_Changed_ForItemsControl(object sender, EventArgs e)
        {
            var ss = WPFControlsUtils.FindParentOfType<System.Windows.Controls.ScrollContentPresenter>(sender as ItemsControl);
            ss.ScrollOwner.ScrollToEnd();
        }

        static void Handle_ItemsSource_Changed(object s, EventArgs e)
        {
            // var source = s as System.Windows.Controls.Primitives.Selector;
            var scrollViewer = WPFControlsUtils.FindChildOfType<ScrollViewer>(s as System.Windows.Controls.Control);
            scrollViewer?.ScrollToEnd();
        }

        static void Sub(object sender)
        {
            // 又由于要兼容 ItemsControl, 所以将 obj 转换为 System.Windows.Controls.Control
            // var source = s0 as System.Windows.Controls.Primitives.Selector; 
            // var source = s0 as System.Windows.Controls.Control;

            // 虽然 ListBox / ListView / DataGrid 基类都是 Selector, 并且其结构都是 Selector => Decorator => ScrollViewer
            // Decorator border = System.Windows.Media.VisualTreeHelper.GetChild(source, 0) as Decorator;
            // ScrollViewer target = border.Child as ScrollViewer;
            // 但为了更加灵活, 不再规限内部的层级结构, 使用 WPFControlsUtils.FindChildOfType<T>
            ScrollViewer target = WPFControlsUtils.FindChildOfType<ScrollViewer>(sender as System.Windows.Controls.Control);
            if (target == null)
            {
                System.Diagnostics.Debug.WriteLine("找不到ScrollViewer");
                System.Diagnostics.Debugger.Break();

                return;
            }

            target.ScrollToEnd();

            var dpDescriptor = System.ComponentModel.DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, sender.GetType());

            // 为 ItemsSource 添加监控更改事件
            dpDescriptor.AddValueChanged(sender, Handle_ItemsSource_Changed);
        }

        static void UnSub(object sender)
        {
            var dpDescriptor = System.ComponentModel.DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, sender.GetType());

            // 为 ItemsSource 添加监控更改事件
            dpDescriptor.RemoveValueChanged(sender, Handle_ItemsSource_Changed);
        }
    }

    // V 1.0.1 - 2021-12-26 19:18:34
    // 修复Bug 开启自动滚动到底部功能后, 在底部时滚动条无法横向移动
    //
    // V 1.0.0 - 2021-06-01 11:23:14
    // 首次创建

    /// <summary>
    /// <para>自动滚动到最底</para>
    /// </summary>
    public class AutoScrollToBottom
    {
        #region [DPA] 启用 自动滚动到最底 附加属性

        // 附加属性 DependencyProperty.RegisterAttached

        /// <summary>
        /// <para>[DPA] 启用 自动滚动到最底 附加属性</para>
        /// </summary>
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

        /// <summary>
        /// Set [DPA] 启用 自动滚动到最底
        /// </summary>
        /// <param name="dp"></param>
        /// <param name="value"></param>
        public static void SetIsEnabled(DependencyObject dp, bool value)
        {
            dp.SetValue(IsEnabledProperty, value);
        }

        /// <summary>
        /// Get [DPA] 启用 自动滚动到最底
        /// </summary>
        /// <param name="dp"></param>
        /// <returns></returns>
        public static bool GetIsEnabled(DependencyObject dp)
        {
            return (bool)dp.GetValue(IsEnabledProperty);
        }

        /// 一旦你开始使用ItemsControl，你可能会遇到一个非常常见的问题：默认情况下，ItemsControl没有任何滚动条，这意味着如果内容不适合，它只是被剪裁。(From wpf-tutorial.com)
        /// 所以使用 ItemsControl 加上 ScrollViewer 有两种方法,
        /// 方法1. 外面包裹 ScrollViewer
        /// 方法2. 使用Template, 在Template设置 <ScrollViewer> <ItemsPresenter /> </ScrollViewer>
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

            // 设置自动滚动的最低的差异值
            double diffValue = _DiffValue_Default_Value_;
            try
            {
                // 对应 ListBox / ListView / DataGrid 等 Selector
                var matchParent = WPFControlsUtils.FindParentOfType<System.Windows.Controls.Primitives.Selector>(scrollViewer);
                if (matchParent != null)
                {
                    diffValue = GetDiffValue(matchParent);
                    goto CalcLogic;
                }

                // 对应方法2 (ItemsControl)
                var matchItemsControl = WPFControlsUtils.FindParentOfType<System.Windows.Controls.ItemsControl>(scrollViewer);
                if (matchItemsControl != null)
                {
                    diffValue = GetDiffValue(matchItemsControl);
                    goto CalcLogic;
                }

                // 对应方法1 (ItemsControl)
                diffValue = GetDiffValue(scrollViewer);
                goto CalcLogic;
            }
            catch (Exception)
            {
                diffValue = _DiffValue_Default_Value_;
                goto CalcLogic;
            }

        CalcLogic:
            if
            (
                   scrollViewer.HorizontalOffset <= 0 // 没有进行横向移动
                && scrollViewer.ScrollableHeight - scrollViewer.VerticalOffset < diffValue // 如果接近底部, 执行滚动逻辑
            )
            {
                scrollViewer.ScrollToEnd();
            }
#if DEBUG
            string msg = $"实际:{scrollViewer.ScrollableHeight - scrollViewer.VerticalOffset}, 设置:{diffValue}";
            System.Diagnostics.Debug.WriteLine(msg);
#endif
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

        #endregion 

        /// <summary>
        /// <para>默认差异值, 默认值等于 2</para>
        /// </summary>
        public const double _DiffValue_Default_Value_ = 2;

        #region [DPA] 差异值 ( 少于差异值时, 自动执行滚动最底逻辑 )

        // 附加属性 DependencyProperty.RegisterAttached

        /// <summary>
        /// <para>[DPA] 差异值 ( 少于差异值时, 自动执行滚动最底逻辑 )</para>
        /// </summary>
        public static readonly DependencyProperty DiffValueProperty = DependencyProperty.RegisterAttached
        (
            name: "DiffValue",
            propertyType: typeof(double),
            ownerType: typeof(AutoScrollToBottom),
            defaultMetadata: new FrameworkPropertyMetadata()
            {
                DefaultValue = _DiffValue_Default_Value_,
                // BindsTwoWayByDefault = true,
                // DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                // PropertyChangedCallback = new PropertyChangedCallback(),
                // CoerceValueCallback = new CoerceValueCallback((a, b) => { return null; })
            }
        );

        /// <summary>
        /// Set [DPA] 差异值
        /// </summary>
        /// <param name="dp"></param>
        /// <param name="value"></param>
        public static void SetDiffValue(DependencyObject dp, double value)
        {
            dp.SetValue(DiffValueProperty, value);
        }

        /// <summary>
        /// Get [DPA] 差异值
        /// </summary>
        /// <param name="dp"></param>
        /// <returns></returns>
        public static double GetDiffValue(DependencyObject dp)
        {
            return (double)dp.GetValue(DiffValueProperty);
        }

        #endregion
    }
}