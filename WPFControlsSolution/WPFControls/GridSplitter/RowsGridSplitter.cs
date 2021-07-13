using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Client.Components
{
    //    public class RowsGridSplitter : GridSplitter
    //    {
    //        public RowsGridSplitter()
    //        {
    //            Width = double.NaN;
    //            Height = 14;
    //            Background = System.Windows.Media.Brushes.WhiteSmoke;
    //            HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
    //            ResizeBehavior = GridResizeBehavior.PreviousAndNext;
    //            ResizeDirection = GridResizeDirection.Rows;
    //            ShowsPreview = false;

    //            //try
    //            //{
    //            //    this.Template = (ControlTemplate)Resources["ControlTemplate_RowsGridSplitter"];
    //            //}
    //            //catch (Exception ex)
    //            //{
    //            //    string msg = $"{0}";
    //            //    System.Diagnostics.Debug.WriteLine(msg);
    //            //}


    //            var template = new ControlTemplate(typeof(RowsGridSplitter)) { };

    //            var element = new System.Windows.FrameworkElementFactory(typeof(Grid));
    //            // element.AppendChild()


    //            StackPanel p = new StackPanel()
    //            {
    //                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
    //                Orientation = Orientation.Horizontal
    //            };

    //            p.Children.Add(AAAA());
    //            p.Children.Add(AAAA());
    //            p.Children.Add(AAAA());



    //            //element.SetValue(TextBlock.PaddingProperty, new System.Windows.Thickness(0, 0, 0, 0));
    //            //element.SetValue(TextBlock.HorizontalAlignmentProperty, System.Windows.HorizontalAlignment.Stretch);
    //            //element.SetValue(TextBlock.VerticalAlignmentProperty, System.Windows.VerticalAlignment.Top);
    //            //element.SetValue(TextBlock.TextProperty, "...");
    //            //element.SetValue(TextBlock.BackgroundProperty, System.Windows.Media.Brushes.Orange);
    //            // element.SetValue(TextBlock.TextAlignmentProperty, System.Windows.TextAlignment.Center);

    //            template.VisualTree = element;

    //            this.Template = template;

    //            // test2();
    //        }

    //        System.Windows.Shapes.Ellipse AAAA()
    //        {
    //            return new System.Windows.Shapes.Ellipse()
    //            {
    //                Width = 2,
    //                Height = 2,
    //                Margin = new System.Windows.Thickness(1,1,1,1),
    //                Fill = System.Windows.Media.Brushes.Black
    //            };
    //        }


    //        void test2()
    //        {
    //            // TODO 读取 XAML 方式加载 Template

    //            string xaml =
    //"<ControlTemplate>" +
    //    "<Grid>" +
    //        "<TextBlock " +
    //        "    Padding=\"0,-8,0,0\"" +
    //        "    HorizontalAlignment=\"Stretch\"" +
    //        "    Text=\"...\"" +
    //        "    TextAlignment=\"Center\" />" +
    //    "</Grid>" +
    //"</ControlTemplate>";

    //            this.Template = (ControlTemplate)System.Windows.Markup.XamlReader.Parse(xaml);
    //        }
    //    }




    #region 以 CustomControl 方式编码


    public class RowsGridSplitter : GridSplitter
    {
        static RowsGridSplitter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RowsGridSplitter), new FrameworkPropertyMetadata(typeof(RowsGridSplitter)));
        }

        public RowsGridSplitter()
        {
            Width = double.NaN;
            Height = 6;
            HorizontalAlignment = HorizontalAlignment.Stretch;

            // Panel.Zin

            ResizeBehavior = GridResizeBehavior.PreviousAndNext;
            ResizeDirection = GridResizeDirection.Rows;
            ShowsPreview = false;
        }

    }

    #endregion

}
