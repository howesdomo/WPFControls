using System;

using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Client.Controls.AttachUtils
{
    public class ListBoxAt
    {

        // 附加属性 DependencyProperty.RegisterAttached
        public static readonly DependencyProperty SelectedItemsOverrideProperty = DependencyProperty.RegisterAttached
        (
            name: "SelectedItemsOverride",
            propertyType: typeof(System.Collections.IList),
            ownerType: typeof(ListBoxAt),
            validateValueCallback: null,
            defaultMetadata: new FrameworkPropertyMetadata()
            {
                DefaultValue = null,
                BindsTwoWayByDefault = true,
                DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                PropertyChangedCallback = new PropertyChangedCallback((d, e) =>
                {
                    if (d != null && d is FrameworkElement element)
                    {
                        if (e.NewValue is System.Collections.IList l)
                        {
                            initListBoxItem(element, l);
                        }
                    }
                })
                // CoerceValueCallback = new CoerceValueCallback((a, b) => { return null; })
            }
        );

        public static System.Collections.IList GetSelectedItemsOverride(DependencyObject view)
        {
            return (System.Collections.IList)view.GetValue(SelectedItemsOverrideProperty);
        }

        public static void SetSelectedItemsOverride(DependencyObject view, System.Collections.IList value)
        {
            view.SetValue(SelectedItemsOverrideProperty, value);
        }


        // 附加属性 DependencyProperty.RegisterAttached
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached
        (
            name: "IsEnabled",
            propertyType: typeof(bool),
            ownerType: typeof(ListBoxAt),
            defaultMetadata: new FrameworkPropertyMetadata()
            {
                DefaultValue = false,
                BindsTwoWayByDefault = true,
                DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                PropertyChangedCallback = new PropertyChangedCallback(onHandle_IsEnabled_PropertyChanged)
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
            if (sender is System.Windows.Controls.ListBox target)
            {
                if (e.OldValue is bool oldValue && oldValue == true)
                {
                    target.Loaded -= onHandle_Loaded;
                    target.SelectionChanged -= onHandle_SelectionChanged;
                }

                if (e.NewValue is bool newValue && newValue == true)
                {
                    target.Loaded += onHandle_Loaded;
                    target.SelectionChanged += onHandle_SelectionChanged;
                    
                }
            }
        }

        private static void onHandle_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element)
            {                
                var l = GetSelectedItemsOverride(element);
                initListBoxItem(element, l);
            }
        }

        private static void onHandle_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (sender is FrameworkElement element && element.Tag is bool isInitListBoxItem && isInitListBoxItem == false)
            {
                System.Collections.IList l = GetSelectedItemsOverride(element);

                if (e.AddedItems.Count > 0)
                {
                    foreach (var item in e.AddedItems)
                    {
                        l.Add(item);
                    }
                }

                if (e.RemovedItems.Count > 0)
                {
                    foreach (var item in e.RemovedItems)
                    {
                        l.Remove(item);
                    }
                }
            }            
        }

        static void initListBoxItem(FrameworkElement element, System.Collections.IList list)
        {
            if (element is System.Windows.Controls.ListBox target)
            {
                if (list == null) return;

                target.Tag = true;

                var matchList = System.Windows.Controls.WPFControlsUtils.FindChilrenOfType<System.Windows.Controls.ListBoxItem>(target);

                for (int i = 0; i < matchList.Count; i++)
                {
                    var match = matchList[i];

                    if (list.Contains(match.DataContext) == true)
                    {
                        match.IsSelected = true;
                    }
                    else
                    {
                        match.IsSelected = false;
                    }
                }

                target.Tag = false;
            }
        }


    }
}
