using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Client.Controls.AttachUtils
{
    /// <summary>
    /// <para>DataGrid附加属性工具类</para>
    /// 
    /// <para>
    /// V 1.0.0 - 2021-06-01 15:24:09
    /// 首次创建
    /// 使用附加属性 ColumnStyle 对所有列一次性进行Style设置
    /// </para>
    /// </summary>
    public class DataGrid
    {
        public static readonly DependencyProperty ColumnStyleProperty = DependencyProperty.RegisterAttached
        (
            name: "ColumnStyle",
            propertyType: typeof(Style),
            ownerType: typeof(DataGrid),
            defaultMetadata: new PropertyMetadata()
            {
                PropertyChangedCallback = Handle_ColumnStyle_Changed
            }
        );

        static void Handle_ColumnStyle_Changed(object s, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = s as System.Windows.Controls.DataGrid;

            if (dataGrid == null) { return; }

            if (e.OldValue == null && e.NewValue != null)
            {
                dataGrid.Columns.CollectionChanged += (s0, e0) =>
                {
                    UpdateColumnStyles(dataGrid);
                };
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
            var originStyle = GetColumnStyle(dataGrid);

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
