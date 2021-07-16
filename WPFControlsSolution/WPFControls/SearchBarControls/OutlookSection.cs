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
using System.Collections.ObjectModel;
using System.Windows.Controls.Primitives;

namespace Client.Components.SearchBarControls
{
    /// <summary>
    /// 折叠子菜单控件
    /// </summary>
    public class OutlookSection : HeaderedContentControl
    {
        static OutlookSection()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(OutlookSection), new FrameworkPropertyMetadata(typeof(OutlookSection)));
        }

        public OutlookSection()
            : base()
        {
            AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(buttonClickedEvent));
        }

        private void buttonClickedEvent(object sender, RoutedEventArgs e)
        {
            OutlookBar bar = OutlookBar;
            ToggleButton b = e.OriginalSource as ToggleButton;
            if (b != null) b.IsChecked = true;
            if (bar != null)
            {
                bar.SelectedSection = this;
            }
            OnClick();
        }

        /// <summary>
        /// 返回或者设置图像的部分的按钮。
        /// </summary>
        public ImageSource Image
        {
            get { return (ImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(ImageSource), typeof(OutlookSection), new UIPropertyMetadata(null));


        /// <summary>
        /// 得到部分OutlookBar选中。
        /// </summary>
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            internal set { SetValue(IsSelectedProperty, value); }
        }

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(OutlookSection), new UIPropertyMetadata(false, IsSelectedPropertyChanged));

        private static void IsSelectedPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ((OutlookSection)o).OnSelectedPropertyChanged((bool)e.OldValue, (bool)e.NewValue);
        }

        protected virtual void OnSelectedPropertyChanged(bool oldValue, bool newValue)
        {
            if (newValue) OutlookBar.SelectedSection = this;
        }

        protected virtual void OnClick()
        {
            this.RaiseEvent(new RoutedEventArgs(OutlookSection.ClickEvent));
        }

        /// <summary>
        /// 发生在部分按钮被单击,。
        /// </summary>
        public event RoutedEventHandler Click
        {
            add { AddHandler(OutlookSection.ClickEvent, value); }
            remove { RemoveHandler(OutlookSection.ClickEvent, value); }
        }

        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click",
            RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(OutlookSection));




        /// <summary>
        /// 返回或者设置部分是否显示为一个完整的按钮和文本(真正的),否则只小按钮与图像。
        /// </summary>
        public bool IsMaximized
        {
            get { return (bool)GetValue(IsMaximizedProperty); }
            set { SetValue(IsMaximizedProperty, value); }
        }

        public static readonly DependencyProperty IsMaximizedProperty =
            DependencyProperty.Register("IsMaximized", typeof(bool), typeof(OutlookSection), new UIPropertyMetadata(true));

        /// <summary>
        /// 返回或者设置OutlookBar的这个部分被分配。
        /// </summary>
        internal OutlookBar OutlookBar { get; set; }

    }
}
