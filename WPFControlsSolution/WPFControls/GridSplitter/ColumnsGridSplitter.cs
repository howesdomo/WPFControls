using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client.Components
{

    public class ColumnsGridSplitter : GridSplitter
    {
        static ColumnsGridSplitter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColumnsGridSplitter), new FrameworkPropertyMetadata(typeof(ColumnsGridSplitter)));
        }

        public ColumnsGridSplitter()
        {
            // Width = Auto;
            Height = double.NaN;

            Background = Brushes.WhiteSmoke;

            VerticalAlignment = VerticalAlignment.Stretch;

            ResizeBehavior = GridResizeBehavior.PreviousAndNext;
            ResizeDirection = GridResizeDirection.Columns;
            ShowsPreview = false;

            Panel.SetZIndex(this, 10);
        }
    }
}
