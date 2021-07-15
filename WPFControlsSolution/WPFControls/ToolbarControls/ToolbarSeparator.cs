using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;

namespace Client.Components.ToolbarControls
{
   public class ToolbarSeparator : System.Windows.Controls.UserControl
    {
        public ToolbarSeparator()
        {
            var r = new Rectangle();
            r.Width = 1d;
            // a.Height = Auto

            r.Margin = new System.Windows.Thickness(3, 7, 3, 7);
            r.Fill = System.Windows.Media.Brushes.Black;

            this.Content = r;
        }
    }
}
