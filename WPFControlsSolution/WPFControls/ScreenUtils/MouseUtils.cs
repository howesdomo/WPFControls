using System.Windows;

namespace WPFControls.ScreenUtils
{
    public static class MouseUtils
    {
        public static Point MousePosition
        {
            get
            {
                NativeMethods.POINT pt = new NativeMethods.POINT();
                NativeMethods.GetCursorPos(pt);
                return new Point(pt.x, pt.y);
            }
        }
    }
}
