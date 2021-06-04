using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Client.Controls.AttachUtils
{
    /// <summary>
    /// <para>DataGrid附加属性工具类</para>
    /// 
    /// <para>
    /// V 1.0.0 - 2021-06-01 15:24:09
    /// 拷贝自 DataGrid，让 ENPOT.Base 的 StandardDataGridView / DetailDataGridView 可以使用此附加属性
    /// 使用附加属性 ColumnStyle 对所有列一次性进行Style设置
    /// </para>
    /// </summary>
    public class StandardDataGridView
    {
        public static readonly DependencyProperty ColumnStyleProperty = DependencyProperty.RegisterAttached
        (
            name: "ColumnStyle",
            propertyType: typeof(Style),
            ownerType: typeof(StandardDataGridView),
            defaultMetadata: new PropertyMetadata()
            {
                PropertyChangedCallback = Handle_ColumnStyle_Changed
            }
        );

        static void Handle_ColumnStyle_Changed(object s, DependencyPropertyChangedEventArgs e)
        {
            if (s is System.Windows.Controls.UserControl uc)
            {
                if (e.OldValue == null && e.NewValue != null)
                {
                    if (uc.IsLoaded == false)
                    {
                        uc.Loaded += Grid_Loaded;
                    }
                    else
                    {
                        Grid_Loaded(uc, new RoutedEventArgs());
                    }
                }
            }
        }

        private static void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            var query = WPFControlsUtils.FindChilrenOfType<System.Windows.Controls.DataGrid>(sender as UserControl);

            if (query == null || query.Count == 0)
            {
                return;
            }

            foreach (System.Windows.Controls.DataGrid dataGrid in query)
            {
                if (dataGrid == null) { return; }

                //dataGrid.Columns.CollectionChanged += (s0, e0) =>
                //{
                    UpdateColumnStyles(dataGrid);
                //};
            }
        }

        public static void SetColumnStyle(DependencyObject element, Style value)
        {
            element.SetValue(ColumnStyleProperty, value);
        }
        public static Style GetColumnStyle(DependencyObject element)
        {
            return (Style)element.GetValue(ColumnStyleProperty);
        }

        static void UpdateColumnStyles(System.Windows.Controls.DataGrid dataGrid)
        {
            var parent = WPFControlsUtils.FindParentOfType<UserControl>(dataGrid);

            var originStyle = GetColumnStyle(parent);

            // foreach (var column in dataGrid.Columns.OfType<System.Windows.Controls.DataGridTextColumn>()) // TODO 优化参数 DataGridTextColumn
            foreach (var column in dataGrid.Columns.OfType<System.Windows.Controls.DataGridBoundColumn>()) // 待测试 DataGridBoundColumn
            {
                var newStyle = new Style();
                newStyle.BasedOn = column.ElementStyle;
                newStyle.TargetType = originStyle.TargetType;

                foreach (var setter in originStyle.Setters.OfType<Setter>())
                {
                    newStyle.Setters.Add(setter);
                }

                column.ElementStyle = newStyle;
            }
        }
    }
}
