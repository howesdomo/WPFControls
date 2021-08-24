using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Client.Components.SearchBarControls
{
    /// <summary>
    /// V 1.0.0 - 2021-08-23 11:03:07
    /// HowesDOMO 编写
    /// 
    /// </summary>
    public partial class SearchPanel : UserControl, System.ComponentModel.INotifyPropertyChanged
    {
        // TODO 还是需要借鉴 原搜索助手 左右拉动 Button, 来控制搜索助手的大小

        public const double PanelMaxWidth = 250d;
        public const double PanelMinWidth = 25d;

        public SearchPanel()
        {
            InitializeComponent();
            initEvent();
            initCMD();

        }

        private bool _MiniMode;
        public bool MiniMode
        {
            get { return _MiniMode; }
            set
            {
                _MiniMode = value;
                this.OnPropertyChanged(nameof(MiniMode));
            }
        }


        void initEvent()
        {
            // this.SizeChanged += SearchPanel_SizeChanged;
        }

        //private void SearchPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        //{
        //    if (this.IsInitialized == false)
        //    {
        //        return;
        //    }

        //    System.Diagnostics.Debug.WriteLine(e.NewSize);

        //    if (e.NewSize.Width <= 80d)
        //    {
        //        this.MiniMode = true;
        //    }
        //    else
        //    {
        //        this.MiniMode = false;
        //    }
        //}

        protected ObservableCollection<SearchCriteia> _searchCriterion = new ObservableCollection<SearchCriteia>();
        public ObservableCollection<SearchCriteia> SearchCriterion
        {
            get
            {
                return this._searchCriterion;
            }
        }

        #region [DP] ResetCommand

        public static readonly DependencyProperty ResetCommandProperty = DependencyProperty.Register
        (
            name: "ResetCommand",
            propertyType: typeof(object),
            ownerType: typeof(SearchPanel),
            validateValueCallback: null, // new ValidateValueCallback((toValidate) => { return true; }),
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: onResetCommand_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public object ResetCommand
        {
            get { return (object)GetValue(ResetCommandProperty); }
            set { SetValue(ResetCommandProperty, value); }
        }

        public static void onResetCommand_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SearchPanel target)
            {
                if (e.NewValue == null)
                {
                    target.btnReset.Command = null;
                    return;
                }

                target.btnReset.Command = (ICommand)e.NewValue;
            }
        }

        #endregion

        #region [DP] ResetCommandParameter


        public static readonly DependencyProperty ResetCommandParameterProperty = DependencyProperty.Register
        (
            name: "ResetCommandParameter",
            propertyType: typeof(object),
            ownerType: typeof(SearchPanel),
            validateValueCallback: null, // new ValidateValueCallback((toValidate) => { return true; }),
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: onResetCommandParameter_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public object ResetCommandParameter
        {
            get { return (object)GetValue(ResetCommandParameterProperty); }
            set { SetValue(ResetCommandParameterProperty, value); }
        }

        public static void onResetCommandParameter_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SearchPanel target)
            {
                target.btnReset.CommandParameter = e.NewValue;
            }
        }

        #endregion

        #region [DP] SearchCommand

        public static readonly DependencyProperty SearchCommandProperty = DependencyProperty.Register
        (
            name: "SearchCommand",
            propertyType: typeof(object),
            ownerType: typeof(SearchPanel),
            validateValueCallback: null, // new ValidateValueCallback((toValidate) => { return true; }),
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: onSearchCommand_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public object SearchCommand
        {
            get { return (object)GetValue(SearchCommandProperty); }
            set { SetValue(SearchCommandProperty, value); }
        }

        public static void onSearchCommand_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SearchPanel target)
            {
                if (e.NewValue == null)
                {
                    target.btnSearch.Command = null;
                    return;
                }

                target.btnSearch.Command = (ICommand)e.NewValue;
            }
        }

        #endregion

        #region [DP] SearchCommandParameter

        public static readonly DependencyProperty SearchCommandParameterProperty = DependencyProperty.Register
        (
            name: "SearchCommandParameter",
            propertyType: typeof(object),
            ownerType: typeof(SearchPanel),
            validateValueCallback: null, // new ValidateValueCallback((toValidate) => { return true; }),
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: onSearchCommandParameter_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public object SearchCommandParameter
        {
            get { return (object)GetValue(SearchCommandParameterProperty); }
            set { SetValue(SearchCommandParameterProperty, value); }
        }

        public static void onSearchCommandParameter_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SearchPanel target)
            {
                target.btnSearch.CommandParameter = e.NewValue;
            }
        }

        #endregion





        void initCMD()
        {
            initUICMD();
        }


        void initUICMD()
        {
            CommandBindings.Add(new CommandBinding(ResizeCommand, ResizeCommandExecuted));
        }


        /// <summary>
        /// 返回或者设置如何排列的SearchPanel模板。
        /// </summary>
        public HorizontalAlignment DockPosition
        {
            get { return (HorizontalAlignment)GetValue(DockPositionProperty); }
            set { SetValue(DockPositionProperty, value); }
        }

        public static readonly DependencyProperty DockPositionProperty =
            DependencyProperty.Register("DockPosition", typeof(HorizontalAlignment), typeof(SearchPanel), new UIPropertyMetadata(HorizontalAlignment.Left));

        ///// <summary>
        ///// 返回或者设置SearchPanel是否是最大化或最小化。
        ///// </summary>
        //public bool IsMaximized
        //{
        //    get { return (bool)GetValue(IsMaximizedProperty); }
        //    set { SetValue(IsMaximizedProperty, value); }
        //}

        //public static readonly DependencyProperty IsMaximizedProperty =
        //    DependencyProperty.Register("IsMaximized", typeof(bool), typeof(SearchPanel), new UIPropertyMetadata(true, MaximizedPropertyChanged));

        //private static void MaximizedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    if (d is SearchPanel target)
        //    {
        //        target.OnMaximizedChanged((bool)e.NewValue);
        //    }

        //}

        //private double previousMaxWidth = double.PositiveInfinity;

        ///// <summary>
        ///// 发生在IsMaximized财产已经改变了。
        ///// </summary>
        ///// <param name="isExpanded"></param>
        //protected virtual void OnMaximizedChanged(bool isExpanded)
        //{
        //    //if (isExpanded) IsPopupVisible = false;
        //    //EnsureSectionContentIsVisible();

        //    //if (isExpanded)
        //    //{
        //    //    MaxWidth = previousMaxWidth;
        //    //    RaiseEvent(new RoutedEventArgs(ExpandedEvent));
        //    //}
        //    //else
        //    //{
        //    //    previousMaxWidth = MaxWidth;
        //    //    MaxWidth = MinimizedWidth + (CanResize ? 4 : 0);
        //    //    RaiseEvent(new RoutedEventArgs(CollapsedEvent));
        //    //}
        //}








        /// <summary>
        /// 开始调整SearchPanel的宽度(用于xaml模板启动大小)。
        /// </summary>
        public static RoutedUICommand ResizeCommand
        {
            get { return resizeCommand; }
        }
        private static RoutedUICommand resizeCommand = new RoutedUICommand("Resize", "ResizeCommand", typeof(SearchPanel));

        private void ResizeCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Control c = e.OriginalSource as Control;
            if (c != null)
            {
                c.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(DragMouseLeftButtonUp);
            }

            this.PreviewMouseMove += new MouseEventHandler(PreviewMouseMoveResize);
        }



        /// <summary>
        /// 删除所有PreviewMouseMove事件,SearchPanel引发命令。
        /// </summary>
        void DragMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Control c = e.OriginalSource as Control;
            if (c != null)
            {
                c.PreviewMouseLeftButtonUp -= DragMouseLeftButtonUp;
            }
            this.PreviewMouseMove -= PreviewMouseMoveButtons;
            this.PreviewMouseMove -= PreviewMouseMoveResize;
        }

        void PreviewMouseMoveButtons(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point pos = e.GetPosition(this);
                //double h = this.ActualHeight - 1 - ButtonHeight - pos.Y;
                //MaxNumberOfButtons = (int)(h / ButtonHeight);
            }
            else
            {
                this.PreviewMouseMove -= PreviewMouseMoveButtons;
            }
        }

        void PreviewMouseMoveResize(object sender, MouseEventArgs e)
        {
            Control c = e.OriginalSource as Control;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (DockPosition == HorizontalAlignment.Left)
                {
                    ResizeFromRight(e);
                }
                else
                {
                    ResizeFromLeft(e);
                }
            }
            else this.PreviewMouseMove -= PreviewMouseMoveResize;
        }


        private void ResizeFromLeft(MouseEventArgs e)
        {
            Point pos = e.GetPosition(this);
            double w = this.ActualWidth - pos.X;

            if (w < 80d)
            {
                w = double.NaN;
                // IsMaximized = false;
                MiniMode = true;
            }
            else
            {
                // IsMaximized = true;
                MiniMode = false;
            }

            if (MaxWidth != double.NaN && w > MaxWidth)
            {
                w = MaxWidth;
            }

            Width = w;
        }
        private void ResizeFromRight(MouseEventArgs e)
        {
            Point pos = e.GetPosition(this);
            double w = pos.X;

            if (w < 80d)
            {
                w = double.NaN;
                // IsMaximized = false;
                MiniMode = true;

                Width = 35d;

                return;
            }
            else
            {
                // IsMaximized = true;
                MiniMode = false;
            }

            if (MaxWidth != double.NaN && w > MaxWidth)
            {
                w = MaxWidth;
            }
            Width = w;
        }



        #region INotifyPropertyChanged成员

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            this.PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
