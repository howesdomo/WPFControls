using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Client.Components
{
    #region [不推荐使用, 但需要了解] 在 C# 中编写 ControlTemplate
    
    public class RowsGridSplitter : GridSplitter
    {
        public RowsGridSplitter()
        {
            Width = double.NaN;

            Background = System.Windows.Media.Brushes.WhiteSmoke;
            HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            ResizeBehavior = GridResizeBehavior.PreviousAndNext;
            ResizeDirection = GridResizeDirection.Rows;
            ShowsPreview = false;

            #region ControlTemplate

            // 用 C# 代码的方式, 实现以下的 XAML 代码
            //  1 //<ControlTemplate TargetType="{x:Type local:RowsGridSplitter}">
            //  2 //    <Grid Background="Transparent">
            //  3 //        <StackPanel
            //  4 //            HorizontalAlignment="Center"
            //  5 //            Orientation="Horizontal">
            //  6 //            <Ellipse Width="2" Height="2" Margin="1" Fill="Gray" />
            //  7 //            <Ellipse Width="2" Height="2" Margin="1" Fill="Gray" />
            //  8 //            <Ellipse Width="2" Height="2" Margin="1" Fill="Gray" />
            //  9 //        </StackPanel>
            // 10 //    </Grid>
            // 11 //</ControlTemplate>

            var controlTemplate = new ControlTemplate(typeof(RowsGridSplitter));

            var gridFactory = new FrameworkElementFactory(typeof(Grid));

            gridFactory.SetValue(Grid.BackgroundProperty, System.Windows.Media.Brushes.Transparent);

            var stackPanelFactory = new FrameworkElementFactory(typeof(StackPanel));

            stackPanelFactory.SetValue(StackPanel.HorizontalAlignmentProperty, System.Windows.HorizontalAlignment.Center);
            stackPanelFactory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

            for (int i = 0; i < 3; i++)
            {
                var ellipse = new FrameworkElementFactory(typeof(Ellipse));
                ellipse.SetValue(Ellipse.WidthProperty, 2d);
                ellipse.SetValue(Ellipse.HeightProperty, 2d);
                ellipse.SetValue(Ellipse.MarginProperty, new System.Windows.Thickness(1, 1, 1, 1));
                ellipse.SetValue(Ellipse.FillProperty, System.Windows.Media.Brushes.Gray);

                stackPanelFactory.AppendChild(ellipse);
            }

            gridFactory.AppendChild(stackPanelFactory);

            controlTemplate.VisualTree = gridFactory;

            #endregion

            this.Template = controlTemplate;
        }
    }

    #endregion

    #region [推荐使用] 以 CustomControl 方式编码

    //public class RowsGridSplitter : GridSplitter
    //{
    //    static RowsGridSplitter()
    //    {
    //        DefaultStyleKeyProperty.OverrideMetadata(typeof(RowsGridSplitter), new FrameworkPropertyMetadata(typeof(RowsGridSplitter)));
    //    }

    //    public RowsGridSplitter()
    //    {
    //        Width = double.NaN;
    //        Height = 6;
    //        HorizontalAlignment = HorizontalAlignment.Stretch;

    //        // Panel.Zin

    //        ResizeBehavior = GridResizeBehavior.PreviousAndNext;
    //        ResizeDirection = GridResizeDirection.Rows;
    //        ShowsPreview = false;
    //    }

    //}

    #endregion
}
