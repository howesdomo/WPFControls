using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Client.Components
{
    public class RowsGridSplitter : GridSplitter
    {
        public RowsGridSplitter()
        {
            Width = double.NaN;
            Height = 6;
            Background = System.Windows.Media.Brushes.WhiteSmoke;
            HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            ResizeBehavior = GridResizeBehavior.PreviousAndNext;
            ResizeDirection = GridResizeDirection.Rows;
            ShowsPreview = false;

            var template = new ControlTemplate(typeof(RowsGridSplitter)) { };

            var element = new System.Windows.FrameworkElementFactory(typeof(TextBlock));
            template.VisualTree = element;

            element.SetValue(TextBlock.PaddingProperty, new System.Windows.Thickness(0, 0, 0, 0));
            element.SetValue(TextBlock.HorizontalAlignmentProperty, System.Windows.HorizontalAlignment.Stretch);
            element.SetValue(TextBlock.TextProperty, "...");
            element.SetValue(TextBlock.TextAlignmentProperty, System.Windows.TextAlignment.Center);
        }


        void test2()
        {
            // TODO 读取 XAML 方式加载 Template

            string xaml =
"<ControlTemplate>" +
    "<Grid>" +
        "<TextBlock " +
        "    Padding=\"0,-8,0,0\"" +
        "    HorizontalAlignment=\"Stretch\"" +
        "    Text=\"...\"" +
        "    TextAlignment=\"Center\" />" +
    "</Grid>" +
"</ControlTemplate>";

            this.Template = (ControlTemplate)System.Windows.Markup.XamlReader.Parse(xaml);
        }
    }
}
