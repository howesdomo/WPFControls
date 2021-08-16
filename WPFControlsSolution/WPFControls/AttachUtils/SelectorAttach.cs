using System;
using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Client.Controls.AttachUtils
{
    /// <summary>
    /// 用于 ListBox 或 MultiSelector 的 Selecteditems 双向绑定的附加属性代码
    /// 
    /// V 1.0.0 - 2021-08-16 17:50:05
    /// 首次编写 与 测试
    /// </summary>
    public class SelectorAttach : DependencyObject
    {
        #region [DPA] SelectedItems

        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.RegisterAttached
        (
            name: "SelectedItems",
            propertyType: typeof(IList),
            ownerType: typeof(SelectorAttach),
            defaultMetadata: new PropertyMetadata
            (
                defaultValue: default(IList),
                propertyChangedCallback: onSelectedItems_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public static void SetSelectedItems(DependencyObject element, IList value)
        {
            element.SetValue(SelectedItemsProperty, value);
        }

        public static IList GetSelectedItems(DependencyObject element)
        {
            return (IList)element.GetValue(SelectedItemsProperty);
        }

        #endregion

        #region [DPA] IsEditing

        public static readonly DependencyProperty IsEditingProperty = DependencyProperty.RegisterAttached
        (
            name: "IsEditing",
            propertyType: typeof(bool),
            ownerType: typeof(SelectorAttach),
            defaultMetadata: new PropertyMetadata
            (
                defaultValue: false,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public static bool GetIsEditing(DependencyObject element)
        {
            return (bool)element.GetValue(IsEditingProperty);
        }

        public static void SetIsEditing(DependencyObject element, bool value)
        {
            element.SetValue(IsEditingProperty, value);
        }

        #endregion

        static NotifyCollectionChangedEventHandler myDelegate_CollectionChanged { get; set; }

        /// <summary>
        /// Attaches a list or observable collection to the grid or listbox, syncing both lists (one way sync for simple lists).
        /// </summary>
        /// <param name="sender">The DataGrid or ListBox</param>
        /// <param name="e">The list to sync to.</param>
        static void onSelectedItems_PropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is Selector selector)
            {
                var oldValueList = e.OldValue as IList;
                if (oldValueList != null)
                {
                    var obs = oldValueList as INotifyCollectionChanged;
                    if (obs != null)
                    {
                        obs.CollectionChanged -= myDelegate_CollectionChanged;
                    }

                    if (e.NewValue == null) // 新值若为null, 取消 SelectionChanged
                    {
                        selector.SelectionChanged -= selector_onSelectionChanged;
                    }
                }

                var newValueList = (IList)e.NewValue;
                if (newValueList != null)
                {
                    var obs = newValueList as INotifyCollectionChanged;
                    if (obs != null)
                    {
                        // obs.CollectionChanged += onCollectionChanged; // 弃用原因 A
                        // 解释弃用原因 A
                        // 由于 obs.CollectionChanged 的 Sender 是集合自己本身, 无法获取到实际的控件

                        // 故采用委托的形式编写 CollectionChanged 事件逻辑, 获取实际的控件
                        myDelegate_CollectionChanged = (s1, e1) =>
                        {
                            if (GetIsEditing(sender) == true)
                            {
                                return;
                            }

                            if (sender is ListBox listBox)
                            {
                                #region Handle ListBox                

                                listBox.SelectionChanged -= selector_onSelectionChanged;
                                if (e1.Action == NotifyCollectionChangedAction.Reset)
                                {
                                    listBox.SelectedItems.Clear();
                                }
                                else
                                {
                                    if (e1.OldItems != null)
                                    {
                                        foreach (var oldItem in e1.OldItems)
                                        {
                                            if (listBox.SelectedItems.Contains(oldItem) == true)
                                            {
                                                listBox.SelectedItems.Remove(oldItem);
                                            }
                                        }
                                    }

                                    if (e1.NewItems != null)
                                    {
                                        foreach (var newItem in e1.NewItems)
                                        {
                                            if (listBox.SelectedItems.Contains(newItem) == false)
                                            {
                                                listBox.SelectedItems.Add(newItem);
                                            }
                                        }
                                    }
                                }

                                listBox.SelectionChanged += selector_onSelectionChanged;

                                #endregion

                                return;
                            }

                            if (sender is MultiSelector multiSelector)
                            {
                                #region Handle MultiSelector

                                multiSelector.SelectionChanged -= selector_onSelectionChanged;

                                if (e1.Action == NotifyCollectionChangedAction.Reset)
                                {
                                    multiSelector.SelectedItems.Clear();
                                }
                                else
                                {
                                    if (e1.OldItems != null)
                                    {
                                        foreach (var oldItem in e1.OldItems)
                                        {
                                            if (multiSelector.SelectedItems.Contains(oldItem) == true)
                                            {
                                                multiSelector.SelectedItems.Remove(oldItem);
                                            }
                                        }
                                    }

                                    if (e1.NewItems != null)
                                    {
                                        foreach (var newItem in e1.NewItems)
                                        {
                                            if (multiSelector.SelectedItems.Contains(newItem) == false)
                                            {
                                                multiSelector.SelectedItems.Add(newItem);
                                            }
                                        }
                                    }
                                }

                                multiSelector.SelectionChanged += selector_onSelectionChanged;

                                #endregion

                                return;
                            }
                        };

                        obs.CollectionChanged += myDelegate_CollectionChanged;
                    }

                    initData_SelectedItems(selector, newValueList);
                    selector.SelectionChanged += selector_onSelectionChanged;
                }
                else
                {
                    initData_SelectedItems(selector, new ArrayList());
                }
            }
            else
            {
                throw new ArgumentException("Only Support Selector");
            }
        }

        /// <summary>
        /// Initially set the selected items to the items in the newly connected collection,
        /// unless the new collection has no selected items and the listbox/grid does, in which case
        /// the flow is reversed. The data holder sets the state. If both sides hold data, then the
        /// bound IList wins and dominates the helpless wpf control.
        /// </summary>
        /// <param name="obs">The list to sync to</param>
        /// <param name="sender">The grid or listbox</param>
        static void initData_SelectedItems(DependencyObject sender, IList obs)
        {
            if (!(sender is ListBox || sender is MultiSelector))
            {
                throw new ArgumentException("Only Support ListBox and MultiSelector");
            }

            // if (sender is ListBox listBox)
            if (sender is ListBox listBox) // 因为 ListBox 继承的是 Selector, Selector 没有 SelectedItems
            {
                if (obs.Count > 0)
                {
                    var exObs = new ArrayList(); // 记录选中的
                    foreach (var item in obs)
                    {
                        exObs.Add(item);
                    }

                    listBox.SelectedItems.Clear(); // obs 有值, 清理掉 SelectedItems
                    foreach (var ob in exObs)
                    {
                        if (listBox.SelectedItems.Contains(ob) == false)
                        {
                            listBox.SelectedItems.Add(ob);
                        }
                    }
                }
                else
                {
                    //foreach (var ob in listBox.SelectedItems)
                    //{
                    //    obs.Add(ob);
                    //}
                    listBox.SelectedItems.Clear();

                }
                return;
            }

            if (sender is MultiSelector multiSelector)
            {
                if (obs.Count > 0)
                {
                    var exObs = new ArrayList();
                    foreach (var item in obs)
                    {
                        exObs.Add(item);
                    }

                    multiSelector.SelectedItems.Clear(); // obs 有值, 清理掉 SelectedItems
                    foreach (var ob in exObs)
                    {
                        if (multiSelector.SelectedItems.Contains(ob) == false)
                        {
                            multiSelector.SelectedItems.Add(ob);
                        }
                    }
                }
                else
                {
                    foreach (var ob in multiSelector.SelectedItems)
                    {
                        obs.Add(ob);
                    }
                }

                return;
            }
        }

        /// <summary>
        /// When the listbox or grid fires a selectionChanged even, we update the attached list to
        /// match it.
        /// </summary>
        /// <param name="sender">The listbox or grid</param>
        /// <param name="e">Items added and removed.</param>
        static void selector_onSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is DependencyObject dObj)
            {
                IList matchSelectedItems = GetSelectedItems(dObj);

                if (matchSelectedItems == null)
                {
                    return;
                }

                SetIsEditing(dObj, true);

                foreach (var oldItem in e.RemovedItems)
                {
                    if (matchSelectedItems.Contains(oldItem) == true)
                    {
                        matchSelectedItems.Remove(oldItem);
                    }
                }

                foreach (var newItem in e.AddedItems)
                {
                    if (matchSelectedItems.Contains(newItem) == false)
                    {
                        matchSelectedItems.Add(newItem);
                    }
                }

                SetIsEditing(dObj, false);
            }
        }


        [Obsolete]
        static void onCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("onCollectionChanged");
            System.Diagnostics.Debugger.Break();

            // 没有任何效果, 因为 sender 就是个集合, 并不会命中以下任何的代码
            // 已采用委托的方式获取具体的控件

            if (sender is ListBox listBox)
            {
                #region Handle ListBox                

                listBox.SelectionChanged -= selector_onSelectionChanged;
                if (e.Action == NotifyCollectionChangedAction.Reset)
                {
                    listBox.SelectedItems.Clear();
                }
                else
                {
                    foreach (var oldItem in e.OldItems)
                    {
                        if (listBox.SelectedItems.Contains(oldItem) == true)
                        {
                            listBox.SelectedItems.Remove(oldItem);
                        }
                    }

                    foreach (var newItem in e.NewItems)
                    {
                        if (listBox.SelectedItems.Contains(newItem) == false)
                        {
                            listBox.SelectedItems.Add(newItem);
                        }
                    }
                }

                listBox.SelectionChanged += selector_onSelectionChanged;

                #endregion

                return;
            }

            if (sender is MultiSelector multiSelector)
            {
                #region Handle MultiSelector

                multiSelector.SelectionChanged -= selector_onSelectionChanged;

                if (e.Action == NotifyCollectionChangedAction.Reset)
                {
                    multiSelector.SelectedItems.Clear();
                }
                else
                {
                    foreach (var oldItem in e.OldItems)
                    {
                        if (multiSelector.SelectedItems.Contains(oldItem) == true)
                        {
                            multiSelector.SelectedItems.Remove(oldItem);
                        }
                    }

                    foreach (var newItem in e.NewItems)
                    {
                        if (multiSelector.SelectedItems.Contains(newItem) == false)
                        {
                            multiSelector.SelectedItems.Add(newItem);
                        }
                    }
                }

                multiSelector.SelectionChanged += selector_onSelectionChanged;

                #endregion

                return;
            }
        }
    }
}

