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
using System.Diagnostics;
using System.Collections;
using System.ComponentModel;

namespace Client.Components.SearchBarControls
{

    /// <summary>
    /// 折叠导航控件
    /// </summary>
    [TemplatePart(Name = partMinimizedButtonContainer)]
    public class OutlookBar : HeaderedItemsControl
    {
        const string partMinimizedButtonContainer = "PART_MinimizedContainer";
        const string partPopup = "PART_Popup";
        static OutlookBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(OutlookBar), new FrameworkPropertyMetadata(typeof(OutlookBar)));
        }

        private Collection<OutlookSection> maximizedSections;
        private Collection<OutlookSection> minimizedSections;
        private FrameworkElement minimizedButtonContainer;


        public OutlookBar()
            : base()
        {
            overflowMenu = new Collection<object>();
            SetValue(OutlookBar.OverflowMenuItemsPropertyKey, overflowMenu);
            SetValue(OutlookBar.OptionButtonsPropertyKey, new Collection<ButtonBase>());

            CommandBindings.Add(new CommandBinding(CollapseCommand, CollapseCommandExecuted));
            CommandBindings.Add(new CommandBinding(StartDraggingCommand, StartDraggingCommandExecuted));
            CommandBindings.Add(new CommandBinding(ShowPopupCommand, ShowPopupCommandExecuted));
            CommandBindings.Add(new CommandBinding(ResizeCommand, ResizeCommandExecuted));
            CommandBindings.Add(new CommandBinding(CloseCommand, CloseCommandExecuted));

            maximizedSections = new Collection<OutlookSection>();
            minimizedSections = new Collection<OutlookSection>();
            sections = new ObservableCollection<OutlookSection>();
            sections.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(SectionsCollectionChanged);

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                this.SizeChanged += new SizeChangedEventHandler(OutlookBar_SizeChanged);
            }
        }

        void OutlookBar_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ApplySections();
        }



        private void CollapseCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            IsMaximized ^= true;
        }

        private void ShowPopupCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (!IsMaximized)
            {
                IsPopupVisible = true;
            }
        }

        private void ResizeCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Control c = e.OriginalSource as Control;
            if (c != null)
            {
                c.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(DragMouseLeftButtonUp);
            }
            this.PreviewMouseMove += new MouseEventHandler(PreviewMouseMoveResize);
        }

        private void CloseCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
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

            if (w < 80)
            {
                w = double.NaN;
                IsMaximized = false;
            }
            else
            {
                IsMaximized = true;
            }
            if (MaxWidth != double.NaN && w > MaxWidth) w = MaxWidth;
            Width = w;
        }
        private void ResizeFromRight(MouseEventArgs e)
        {
            Point pos = e.GetPosition(this);
            double w = pos.X;

            if (w < 80)
            {
                w = double.NaN;
                IsMaximized = false;
            }
            else
            {
                IsMaximized = true;
            }
            if (MaxWidth != double.NaN && w > MaxWidth) w = MaxWidth;
            Width = w;
        }

        private void StartDraggingCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Control c = e.OriginalSource as Control;
            if (c != null)
            {
                c.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(DragMouseLeftButtonUp);
            }
            this.PreviewMouseMove += new MouseEventHandler(PreviewMouseMoveButtons);
        }

        /// <summary>
        /// 删除所有PreviewMouseMove事件,outlookbar引发命令。
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
                double h = this.ActualHeight - 1 - ButtonHeight - pos.Y;
                MaxNumberOfButtons = (int)(h / ButtonHeight);
            }
            else this.PreviewMouseMove -= PreviewMouseMoveButtons;
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            ApplySections();
        }



        /// <summary>
        /// 确定MinimizedSections MaximizedSections的集合,根据MaxVisibleButtons属性。
        /// </summary>
        protected virtual void ApplySections()
        {
            if (this.IsInitialized)
            {
                maximizedSections = new Collection<OutlookSection>();
                minimizedSections = new Collection<OutlookSection>();
                int max = MaxNumberOfButtons;
                int index = 0;
                int selectedIndex = SelectedSectionIndex;
                OutlookSection selectedContent = null;

                int n = GetNumberOfMinimizedButtons();

                foreach (OutlookSection e in sections)
                {
                    e.OutlookBar = this;
                    e.Height = ButtonHeight;
                    if (max-- > 0)
                    {
                        e.IsMaximized = true;
                        maximizedSections.Add(e);
                    }
                    else
                    {
                        e.IsMaximized = false;
                        if (minimizedSections.Count < n)
                        {
                            minimizedSections.Add(e);
                        }
                    }
                    bool selected = index++ == selectedIndex;
                    e.IsSelected = selected;
                    if (selected) selectedContent = e;
                }
                SetValue(OutlookBar.MaximizedSectionsPropertyKey, maximizedSections);
                SetValue(OutlookBar.MinimizedSectionsPropertyKey, minimizedSections);
                SelectedSection = selectedContent;
            }

        }


        private Collection<object> overflowMenu;

        /// <summary>
        /// 返回或者设置为默认项的溢出菜单
        /// </summary>
        [Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Collection<object> OverflowMenuItems
        {
            get { return overflowMenu; }
            //    private set { overflowMenu = value; }
        }

        public static readonly DependencyProperty OverflowMenuProperty =
            DependencyProperty.Register("OverflowMenu", typeof(ItemCollection), typeof(OutlookBar), new UIPropertyMetadata(null));

        private void ApplyOverflowMenu()
        {
            Collection<object> overflowItems = new Collection<object>();
            if (OverflowMenuItems.Count > 0)
            {
                foreach (object item in OverflowMenuItems)
                {
                    overflowItems.Add(item);
                }
            }

            bool separatorAdded = false;
            int visibleButtons = maximizedSections.Count + (IsMaximized ? minimizedSections.Count : 0);

            for (int i = visibleButtons; i < sections.Count; i++)
            {
                if (!separatorAdded)
                {
                    overflowItems.Add(new Separator());
                    separatorAdded = true;
                }
                OutlookSection section = sections[i];
                MenuItem item = new MenuItem();
                item.Header = section.Header;
                Image image = new Image();
                image.Source = section.Image;
                item.Icon = image;
                item.Tag = section;
                item.Click += new RoutedEventHandler(item_Click);
                overflowItems.Add(item);
            }

            SetValue(OutlookBar.OverflowMenuItemsPropertyKey, overflowItems);
        }

        private int GetNumberOfMinimizedButtons()
        {
            if (minimizedButtonContainer != null)
            {
                const double width = 32;
                const double overflowWidth = 18;
                double fraction = (minimizedButtonContainer.ActualWidth - overflowWidth) / width;
                int minimizedButtons = (int)Math.Truncate(fraction);
                int visibleButtons = MaxNumberOfButtons + minimizedButtons;
                return visibleButtons;
            }
            return 0;
        }

        public event EventHandler<OverflowMenuCreatedEventArgs> OverflowMenuCreated;

        protected virtual void OnOverflowMenuCreated(Collection<object> menuItems)
        {
            if (OverflowMenuCreated != null)
            {
                OverflowMenuCreatedEventArgs e = new OverflowMenuCreatedEventArgs(menuItems);
                OverflowMenuCreated(this, e);
            }
        }

        void item_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = e.OriginalSource as MenuItem;
            OutlookSection section = item.Tag as OutlookSection;
            this.SelectedSection = section;
        }


        void SectionsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ApplySections();
        }

        private Popup popup;

        public override void OnApplyTemplate()
        {
            if (popup != null)
            {
                popup.Closed -= OnPopupClosed;
                popup.Opened -= OnPopupOpened;
            }
            minimizedButtonContainer = this.GetTemplateChild(partMinimizedButtonContainer) as FrameworkElement;
            popup = this.GetTemplateChild(partPopup) as Popup;
            if (popup != null)
            {
                popup.Closed += new EventHandler(OnPopupClosed);
                popup.Opened += new EventHandler(OnPopupOpened);
            }

            ToggleButton btn = GetTemplateChild("PART_ToggleButton") as ToggleButton;
            if (btn != null)
            {
                btn.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(btn_PreviewMouseLeftButtonUp);
            }

            base.OnApplyTemplate();
        }

        void btn_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(null);
            popup.StaysOpen = false;
        }



        protected virtual void OnPopupOpened(object sender, EventArgs e)
        {
            IsPopupVisible = true;
            Mouse.Capture(this, CaptureMode.SubTree);
        }

        protected virtual void OnPopupClosed(object sender, EventArgs e)
        {
            IsPopupVisible = false;
            Mouse.Capture(null);
        }



        private ObservableCollection<OutlookSection> sections;

        /// <summary>
        /// 获取sections集合.
        /// </summary>
        [Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Collection<OutlookSection> Sections
        {
            get { return sections; }
            //            private set { sections = value as ObservableCollection<OutlookSection>; }
        }


        private ObservableCollection<OutlookSection> sideButtons = new ObservableCollection<OutlookSection>();

        /// <summary>
        /// 获取按钮，下拉设为false,ShowSideButtons被设置为true。
        /// </summary>
        public Collection<OutlookSection> SideButtons
        {
            get { return sideButtons; }
        }



        /// <summary>
        /// 返回或者设置是否显示SideButtons当下拉单被设为false。
        /// </summary>
        public bool ShowSideButtons
        {
            get { return (bool)GetValue(ShowSideButtonsProperty); }
            set { SetValue(ShowSideButtonsProperty, value); }
        }

        public static readonly DependencyProperty ShowSideButtonsProperty =
            DependencyProperty.Register("ShowSideButtons", typeof(bool), typeof(OutlookBar), new UIPropertyMetadata(true));


        /// <summary>
        /// 返回或者设置Outlookbar是否是最大化或最小化。
        /// </summary>
        public bool IsMaximized
        {
            get { return (bool)GetValue(IsMaximizedProperty); }
            set { SetValue(IsMaximizedProperty, value); }
        }

        public static readonly DependencyProperty IsMaximizedProperty =
            DependencyProperty.Register("IsMaximized", typeof(bool), typeof(OutlookBar), new UIPropertyMetadata(true, MaximizedPropertyChanged));

        private static void MaximizedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            OutlookBar bar = (OutlookBar)d;
            bar.OnMaximizedChanged((bool)e.NewValue);
        }

        private double previousMaxWidth = double.PositiveInfinity;

        /// <summary>
        /// 发生在IsMaximized财产已经改变了。
        /// </summary>
        /// <param name="isExpanded"></param>
        protected virtual void OnMaximizedChanged(bool isExpanded)
        {
            if (isExpanded) IsPopupVisible = false;
            EnsureSectionContentIsVisible();

            if (isExpanded)
            {
                MaxWidth = previousMaxWidth;
                RaiseEvent(new RoutedEventArgs(ExpandedEvent));
            }
            else
            {
                previousMaxWidth = MaxWidth;
                MaxWidth = MinimizedWidth + (CanResize ? 4 : 0);
                RaiseEvent(new RoutedEventArgs(CollapsedEvent));
            }
        }



        /// <summary>
        /// 后发生OutlookBar已经崩溃。
        /// </summary>
        public event RoutedEventHandler Collapsed
        {
            add { AddHandler(OutlookBar.CollapsedEvent, value); }
            remove { RemoveHandler(OutlookBar.CollapsedEvent, value); }
        }

        /// <summary>
        /// OutlookBar后发生已经扩大。
        /// </summary>
        public event RoutedEventHandler Expanded
        {
            add { AddHandler(OutlookBar.ExpandedEvent, value); }
            remove { RemoveHandler(OutlookBar.ExpandedEvent, value); }
        }

        public static readonly RoutedEvent CollapsedEvent = EventManager.RegisterRoutedEvent("CollapsedEvent",
            RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(OutlookBar));

        public static readonly RoutedEvent ExpandedEvent = EventManager.RegisterRoutedEvent("ExpandedEvent",
            RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(OutlookBar));

        /// <summary>
        /// 此代码确保段内容可见当扩大已经改变了。
        /// </summary>
        private void EnsureSectionContentIsVisible()
        {
            object content = SelectedSection != null ? SelectedSection.Content : null;
            SectionContent = null;  // 设置暂时为null,那么重新设置为当前内容是有影响的.
            CollapsedSectionContent = IsMaximized ? null : content;
            SectionContent = IsMaximized ? content : null;
        }


        /// <summary>
        /// 返回或者设置宽度设置为false时扩大。
        /// </summary>
        public double MinimizedWidth
        {
            get { return (double)GetValue(MinimizedWidthProperty); }
            set { SetValue(MinimizedWidthProperty, value); }
        }

        public static readonly DependencyProperty MinimizedWidthProperty =
            DependencyProperty.Register("MinimizedWidth", typeof(double), typeof(OutlookBar), new UIPropertyMetadata((double)32));




        /// <summary>
        /// 返回或者设置如何排列的OutlookBar模板。
        /// </summary>
        public HorizontalAlignment DockPosition
        {
            get { return (HorizontalAlignment)GetValue(DockPositionProperty); }
            set { SetValue(DockPositionProperty, value); }
        }

        public static readonly DependencyProperty DockPositionProperty =
            DependencyProperty.Register("DockPosition", typeof(HorizontalAlignment), typeof(OutlookBar), new UIPropertyMetadata(HorizontalAlignment.Left));

        private static readonly DependencyPropertyKey MaximizedSectionsPropertyKey =
            DependencyProperty.RegisterReadOnly("MaximizedSections", typeof(Collection<OutlookSection>), typeof(OutlookBar), new UIPropertyMetadata(null));
        public static readonly DependencyProperty MaximizedSectionsProperty = MaximizedSectionsPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey MinimizedSectionsPropertyKey =
            DependencyProperty.RegisterReadOnly("MinimizedSections", typeof(Collection<OutlookSection>), typeof(OutlookBar), new UIPropertyMetadata(null));
        public static readonly DependencyProperty MinimizedSectionsProperty = MinimizedSectionsPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey OverflowMenuItemsPropertyKey =
            DependencyProperty.RegisterReadOnly("OverflowMenuItems", typeof(Collection<object>), typeof(OutlookBar), new UIPropertyMetadata(null));
        public static readonly DependencyProperty OverflowMenuItemsProperty = OverflowMenuItemsPropertyKey.DependencyProperty;


        /// <summary>
        /// 返回或者设置有多少按钮是完全可见的。
        /// </summary>
        public int MaxNumberOfButtons
        {
            get { return (int)GetValue(MaxNumberOfButtonsProperty); }
            set { SetValue(MaxNumberOfButtonsProperty, value); }
        }

        public static readonly DependencyProperty MaxNumberOfButtonsProperty =
            DependencyProperty.Register("MaxNumberOfButtons", typeof(int), typeof(OutlookBar),
            new FrameworkPropertyMetadata(int.MaxValue,
                FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure,
                MaxNumberOfButtonsPropertyChanged));

        private static void MaxNumberOfButtonsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            OutlookBar bar = (OutlookBar)d;
            bar.ApplySections();
        }


        /// <summary>
        /// 返回或者设置是否弹出面板都可见。
        /// </summary>
        public bool IsPopupVisible
        {
            get { return (bool)GetValue(IsPopupVisibleProperty); }
            set { SetValue(IsPopupVisibleProperty, value); }
        }

        public static readonly DependencyProperty IsPopupVisibleProperty =
            DependencyProperty.Register("IsPopupVisible", typeof(bool), typeof(OutlookBar), new UIPropertyMetadata(false, PopupVisiblePropertyChanged));

        private static void PopupVisiblePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            OutlookBar bar = (OutlookBar)d;
            bar.OnPopupVisibleChanged((bool)e.NewValue);
        }

        /// <summary>
        /// 发生在IsPopupVisible已经改变了。
        /// </summary>
        /// <param name="isPopupVisible"></param>
        protected virtual void OnPopupVisibleChanged(bool isPopupVisible)
        {
            if (popup != null)
            {
                popup.StaysOpen = true;
                popup.IsOpen = isPopupVisible;
            }
            if (isPopupVisible)
            {
                RaiseEvent(new RoutedEventArgs(PopupOpenedEvent));
            }
            else
            {
                RaiseEvent(new RoutedEventArgs(PopupClosedEvent));
            }
        }

        public event RoutedEventHandler PopupOpened
        {
            add { AddHandler(OutlookBar.PopupOpenedEvent, value); }
            remove { RemoveHandler(OutlookBar.PopupOpenedEvent, value); }
        }

        public event RoutedEventHandler PopupClosed
        {
            add { AddHandler(OutlookBar.PopupClosedEvent, value); }
            remove { RemoveHandler(OutlookBar.PopupClosedEvent, value); }
        }

        public static readonly RoutedEvent PopupOpenedEvent = EventManager.RegisterRoutedEvent("PopupOpenedEvent",
            RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(OutlookBar));

        public static readonly RoutedEvent PopupClosedEvent = EventManager.RegisterRoutedEvent("PopupClosedEvent",
            RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(OutlookBar));

        /// <summary>
        /// 返回或者设置选中的索引。
        /// </summary>
        public int SelectedSectionIndex
        {
            get { return (int)GetValue(SelectedSectionIndexProperty); }
            set { SetValue(SelectedSectionIndexProperty, value); }
        }

        public static readonly DependencyProperty SelectedSectionIndexProperty =
            DependencyProperty.Register("SelectedSectionIndex", typeof(int), typeof(OutlookBar), new
                FrameworkPropertyMetadata(
                0,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                SelectedIndexPropertyChanged));

        private static void SelectedIndexPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            OutlookBar bar = (OutlookBar)d;
            bar.ApplySections();
        }



        /// <summary>
        /// 返回或者设置选定的部分。
        /// </summary>
        public OutlookSection SelectedSection
        {
            get { return (OutlookSection)GetValue(SelectedSectionProperty); }
            set { SetValue(SelectedSectionProperty, value); }
        }

        public static readonly DependencyProperty SelectedSectionProperty =
            DependencyProperty.Register("SelectedSection", typeof(OutlookSection), typeof(OutlookBar),
            new UIPropertyMetadata(null, SelectedSectionPropertyChanged));


        private static void SelectedSectionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            OutlookBar bar = (OutlookBar)d;
            bar.OnSelectedSectionChanged((OutlookSection)e.OldValue, (OutlookSection)e.NewValue);
        }

        /// <summary>
        /// 发生在SelectedSection已经改变了。
        /// </summary>
        protected virtual void OnSelectedSectionChanged(OutlookSection oldSection, OutlookSection newSection)
        {

            for (int index = 0; index < sections.Count; index++)
            {
                OutlookSection section = sections[index];
                bool selected = newSection == section;
                section.IsSelected = newSection == section;
                if (selected)
                {
                    SelectedSectionIndex = index;
                    SectionContent = IsMaximized ? section.Content : null;
                    CollapsedSectionContent = IsMaximized ? null : section.Content;
                }
            }
            RaiseEvent(new RoutedPropertyChangedEventArgs<OutlookSection>(oldSection, newSection, SelectedSectionChangedEvent));
        }


        /// <summary>
        /// 发生在SelectedSection已经改变了。
        /// </summary>
        public event RoutedPropertyChangedEventHandler<OutlookSection> SelectedSectionChanged
        {
            add { AddHandler(OutlookBar.SelectedSectionChangedEvent, value); }
            remove { RemoveHandler(OutlookBar.SelectedSectionChangedEvent, value); }
        }

        public static readonly RoutedEvent SelectedSectionChangedEvent = EventManager.RegisterRoutedEvent("SelectedSectionChangedEvent",
            RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<OutlookSection>), typeof(OutlookBar));


        internal object SectionContent
        {
            get { return (object)GetValue(SectionContentProperty); }
            set { SetValue(SectionContentPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey SectionContentPropertyKey =
            DependencyProperty.RegisterReadOnly("SectionContent", typeof(object), typeof(OutlookBar), new UIPropertyMetadata(null));
        public static readonly DependencyProperty SectionContentProperty = SectionContentPropertyKey.DependencyProperty;


        internal object CollapsedSectionContent
        {
            get { return (object)GetValue(CollapsedSectionContentProperty); }
            set { SetValue(CollapsedSectionContentPropertyKey, value); }
        }


        private static readonly DependencyPropertyKey CollapsedSectionContentPropertyKey =
            DependencyProperty.RegisterReadOnly("CollapsedSectionContent", typeof(object), typeof(OutlookBar), new UIPropertyMetadata(null));
        public static readonly DependencyProperty CollapsedSectionContentProperty = SectionContentPropertyKey.DependencyProperty;



        /// <summary>
        /// 返回或者设置的溢出菜单是否可用的部分是可见的。
        /// </summary>
        public bool IsOverflowVisible
        {
            get { return (bool)GetValue(IsOverflowVisibleProperty); }
            set { SetValue(IsOverflowVisibleProperty, value); }
        }

        public static readonly DependencyProperty IsOverflowVisibleProperty =
            DependencyProperty.Register("IsOverflowVisible", typeof(bool), typeof(OutlookBar), new UIPropertyMetadata(false, OverflowVisiblePropertyChanged));


        private static void OverflowVisiblePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            OutlookBar bar = (OutlookBar)d;
            bool newValue = (bool)e.NewValue;
            bar.OnOverflowVisibleChanged(newValue);
        }

        /// <summary>
        /// 发生在IsOverflowVisible已经改变了。
        /// </summary>
        /// <param name="newValue"></param>
        protected virtual void OnOverflowVisibleChanged(bool newValue)
        {

            if (newValue == true)
            {
                ApplyOverflowMenu();
            }
        }

        public static RoutedUICommand CollapseCommand
        {
            get { return collapseCommand; }
        }

        /// <summary>
        /// 开始拖splitter可见部分按钮(用于xaml模板)。
        /// </summary>
        public static RoutedUICommand StartDraggingCommand
        {
            get { return startDraggingCommand; }
        }

        /// <summary>
        /// 显示弹出式窗口。
        /// </summary>
        public static RoutedUICommand ShowPopupCommand
        {
            get { return showPopupCommand; }
        }

        /// <summary>
        /// 开始调整OutlookBar的宽度(用于xaml模板启动大小)。
        /// </summary>
        public static RoutedUICommand ResizeCommand
        {
            get { return resizeCommand; }
        }
        private static RoutedUICommand resizeCommand = new RoutedUICommand("Resize", "ResizeCommand", typeof(OutlookBar));


        public static RoutedUICommand CloseCommand
        {
            get { return closeCommand; }
        }
        private static RoutedUICommand collapseCommand = new RoutedUICommand("Collapse", "CollapseCommand", typeof(OutlookBar));
        private static RoutedUICommand startDraggingCommand = new RoutedUICommand("Drag", "StartDraggingCommand", typeof(OutlookBar));
        private static RoutedUICommand showPopupCommand = new RoutedUICommand("ShowPopup", "ShowPopupCommand", typeof(OutlookBar));
        private static RoutedUICommand closeCommand = new RoutedUICommand("Close", "CloseCommand", typeof(OutlookBar));


        /// <summary>
        /// 返回或者设置高度的部分按钮。
        /// </summary>
        public double ButtonHeight
        {
            get { return (double)GetValue(ButtonHeightProperty); }
            set { SetValue(ButtonHeightProperty, value); }
        }

        public static readonly DependencyProperty ButtonHeightProperty =
            DependencyProperty.Register("ButtonHeight", typeof(double), typeof(OutlookBar), new UIPropertyMetadata((double)28.0));





        /// <summary>
        /// 返回或者设置的弹出式窗口。
        /// </summary>
        public double PopupWidth
        {
            get { return (double)GetValue(PopupWidthProperty); }
            set { SetValue(PopupWidthProperty, value); }
        }

        public static readonly DependencyProperty PopupWidthProperty =
            DependencyProperty.Register("PopupWidth", typeof(double), typeof(OutlookBar), new UIPropertyMetadata((double)double.NaN));




        /// <summary>
        ///返回或者设置分割的部分是否按钮是可见的
        /// </summary>
        public bool IsButtonSplitterVisible
        {
            get { return (bool)GetValue(IsButtonSplitterVisibleProperty); }
            set { SetValue(IsButtonSplitterVisibleProperty, value); }
        }

        public static readonly DependencyProperty IsButtonSplitterVisibleProperty =
            DependencyProperty.Register("IsButtonSplitterVisible", typeof(bool), typeof(OutlookBar), new UIPropertyMetadata(true));


        /// <summary>
        /// 返回或者设置按钮的部分是否可见。
        /// </summary>
        public bool ShowButtons
        {
            get { return (bool)GetValue(ShowButtonsProperty); }
            set { SetValue(ShowButtonsProperty, value); }
        }

        public static readonly DependencyProperty ShowButtonsProperty =
            DependencyProperty.Register("ShowButtons", typeof(bool), typeof(OutlookBar), new UIPropertyMetadata(true));


        [Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Collection<ButtonBase> OptionButtons
        {
            get { return (Collection<ButtonBase>)GetValue(OptionButtonsProperty); }
            //  private set { SetValue(OptionButtonsProperty, value); }
        }

        private static readonly DependencyPropertyKey OptionButtonsPropertyKey =
            DependencyProperty.RegisterReadOnly("OptionButtons", typeof(Collection<ButtonBase>), typeof(OutlookBar), new UIPropertyMetadata(null));
        public static readonly DependencyProperty OptionButtonsProperty = OptionButtonsPropertyKey.DependencyProperty;




        /// <summary>
        /// 返回或者设置是否OutlookBar的宽度可以手动缩放的钳子在正确的(或左)。
        /// </summary>
        public bool CanResize
        {
            get { return (bool)GetValue(CanResizeProperty); }
            set { SetValue(CanResizeProperty, value); }
        }

        public static readonly DependencyProperty CanResizeProperty =
            DependencyProperty.Register("CanResize", typeof(bool), typeof(OutlookBar), new UIPropertyMetadata(true));



        /// <summary>
        /// 返回或者设置是否关闭按钮是可见的。
        /// </summary>
        public bool IsCloseButtonVisible
        {
            get { return (bool)GetValue(IsCloseButtonVisibleProperty); }
            set { SetValue(IsCloseButtonVisibleProperty, value); }
        }

        public static readonly DependencyProperty IsCloseButtonVisibleProperty =
            DependencyProperty.Register("IsCloseButtonVisible", typeof(bool), typeof(OutlookBar), new UIPropertyMetadata(false));



        /// <summary>
        /// 返回或者设置文本或内容显示在最小化OutlookBar在按钮以打开导航窗格。
        /// </summary>
        public object NavigationPaneText
        {
            get { return (object)GetValue(NavigationPaneTextProperty); }
            set { SetValue(NavigationPaneTextProperty, value); }
        }

        public static readonly DependencyProperty NavigationPaneTextProperty =
            DependencyProperty.Register("NavigationPaneText", typeof(object), typeof(OutlookBar), new UIPropertyMetadata("Navigation Pane"));


        protected override IEnumerator LogicalChildren
        {
            get
            {
                return GetLogicalChildren().GetEnumerator();
            }
        }

        protected virtual IEnumerable GetLogicalChildren()
        {
            foreach (var section in Sections) yield return section;
            if (SelectedSection != null) yield return SelectedSection.Content;
        }
    }

    public class OverflowMenuCreatedEventArgs : EventArgs
    {
        public OverflowMenuCreatedEventArgs(Collection<object> menuItems)
            : base()
        {
            this.MenuItems = menuItems;
        }

        public Collection<object> MenuItems { get; private set; }
    }
}
