using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Client.Components.ToolbarControls
{
    public class ToolbarPanel : StackPanel
    {
        public static LinearGradientBrush ToolbarPannelBackgroundBrush { get; set; }

        public ToolbarPanel()
        {
            MinHeight = 36d;            
            HorizontalAlignment = HorizontalAlignment.Stretch;
            VerticalAlignment = VerticalAlignment.Stretch;
            Orientation = Orientation.Horizontal;

            if (ToolbarPannelBackgroundBrush == null)
            {
                ToolbarPannelBackgroundBrush = new LinearGradientBrush()
                {
                    StartPoint = new Point(0.5d, 0d),
                    EndPoint = new Point(0.5d, 1d),
                    GradientStops = new GradientStopCollection()
                    {
                        new GradientStop() { Offset = 0d, Color = Color.FromRgb(255, 255, 255) },
                        new GradientStop() { Offset = 0.35d, Color = Color.FromRgb(255, 255, 255) },
                        new GradientStop() { Offset = 1d, Color = Color.FromRgb(212, 219, 229) },
                    }
                };
            }

            Background = ToolbarPannelBackgroundBrush;
        }
    }
}
