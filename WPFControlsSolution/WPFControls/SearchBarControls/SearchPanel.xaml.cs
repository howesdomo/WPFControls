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
        public const double PanelMaxWidth = 250d;
        public const double PanelMinWidth = 35d;

        #region [DP] ResetCommand

        public static readonly DependencyProperty ResetCommandProperty = DependencyProperty.Register
        (
            name: "ResetCommand",
            propertyType: typeof(object),
            ownerType: typeof(SearchPanel),
            validateValueCallback: null,
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
            validateValueCallback: null,
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
            validateValueCallback: null,
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
            validateValueCallback: null,
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

        protected ObservableCollection<SearchCriteia> _searchCriterion = new ObservableCollection<SearchCriteia>();
        public ObservableCollection<SearchCriteia> SearchCriterion
        {
            get
            {
                return this._searchCriterion;
            }
        }

        public SearchPanel()
        {
            InitializeComponent();
            initEvent();
            initCMD();

        }

        void initEvent()
        {

        }

        void initCMD()
        {
            initUICMD();
        }

        #region 左右拖动 & 最小化搜索助手

        void initUICMD()
        {
            CommandBindings.Add(new CommandBinding(ResizeCommand, ResizeCommandExecuted));
        }


        private bool _IsMiniMode;
        public bool IsMiniMode
        {
            get { return _IsMiniMode; }
            set
            {
                _IsMiniMode = value;
                this.OnPropertyChanged(nameof(IsMiniMode));
            }
        }


        /// <summary>
        /// 开始调整SearchPanel的宽度(用于xaml模板启动大小)。
        /// </summary>
        public RoutedUICommand ResizeCommand
        {
            get { return resizeCommand; }
        }

        private RoutedUICommand resizeCommand = new RoutedUICommand("Resize", "ResizeCommand", typeof(SearchPanel));

        private void ResizeCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            (e.OriginalSource as System.Windows.Controls.Control).PreviewMouseLeftButtonUp += new MouseButtonEventHandler(dragMouseLeftButtonUp);
            this.PreviewMouseMove += new MouseEventHandler(onHandle_PreviewMouseMove);
        }

        /// <summary>
        /// 当释放鼠标左键时, 将事件注释掉
        /// </summary>
        void dragMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            (e.OriginalSource as System.Windows.Controls.Control).PreviewMouseLeftButtonUp -= dragMouseLeftButtonUp;
            this.PreviewMouseMove -= onHandle_PreviewMouseMove;

            System.Diagnostics.Debug.WriteLine("拖拽鼠标左键释放");
        }

        void onHandle_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                resizeFromRight(e);
            }
        }

        void resizeFromRight(MouseEventArgs e)
        {
            Point p = e.GetPosition(this);

            System.Diagnostics.Debug.WriteLine($"拖拽鼠标左键移动 x:{p.X}, y:{p.Y}");

            if (p.X < 80d)
            {
                IsMiniMode = true;
                Width = this.MinWidth;
                return;
            }

            // 解除迷你模式状态, 并根据鼠标拉动长度设置控件宽度
            IsMiniMode = false;

            double tempWidth = p.X;

            if (this.MaxWidth != double.NaN && p.X > this.MaxWidth) // 若 p.X 超越最大宽度, 更改宽度的值等于最大宽度值
            {
                tempWidth = this.MaxWidth;
            }

            Width = tempWidth;
        }

        void btnMin_Click(object sender, RoutedEventArgs e)
        {
            if (this.IsMiniMode == false)
            {
                this.IsMiniMode = true;
            }
            this.Width = this.MinWidth;
        }

        void btnMax_Click(object sender, RoutedEventArgs e)
        {
            if (this.IsMiniMode == true)
            {
                this.IsMiniMode = false;
            }
            this.Width = this.MaxWidth;
        }

        #endregion

        #region INotifyPropertyChanged成员

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            this.PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }

        #endregion

    }
}
