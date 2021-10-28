using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Client.Components.SearchPanelControls
{
    /// <summary>
    /// V 1.0.1 - 2021-09-01 17:05:22
    /// 新增功能:
    /// 1. 在 Title 的右下角添加数量汇总信息(排除空值项)
    /// 2. 双击 汇总信息 弹出清空输入内容的对话框, 简化大量数据(1W条以上)时, 执行全选需要消耗很长的时间
    /// 
    /// V 1.0.0 - 2021-08-25 17:56:14
    /// 重写并整理代码
    /// </summary>
    public partial class SearchConditionTextarea : SearchConditionBase
    {
        public SearchConditionTextarea()
        {
            InitializeComponent();
            initEvent();

            calcTxtInfo();
        }

        void initEvent()
        {
            this.txt.KeyUp += txt_KeyUp;
            this.txt.TextChanged += txt_TextChanged;

            // TextBoxAdv 可以使用 Ctrl + Backspace 或者 使用自定义 ContextMenu 中的清空内容功能实现内容的快速清除
            // this.txtInfo.MouseDown += txtInfo_MouseDown;
        }

        #region [DP] Placeholder

        public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register
        (
            name: "Placeholder",
            propertyType: typeof(string),
            ownerType: typeof(SearchConditionTextarea),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: string.Empty,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        #endregion

        #region [DP] PlaceholderColor

        public static readonly DependencyProperty PlaceholderColorProperty = DependencyProperty.Register
        (
            name: "PlaceholderColor",
            propertyType: typeof(System.Windows.Media.Brush),
            ownerType: typeof(SearchConditionTextarea),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: System.Windows.Media.Brushes.Gray,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public System.Windows.Media.Brush PlaceholderColor
        {
            get { return (System.Windows.Media.Brush)GetValue(PlaceholderColorProperty); }
            set { SetValue(PlaceholderColorProperty, value); }
        }

        #endregion

        #region [DP] PlaceholderFontSize

        public static readonly DependencyProperty PlaceholderFontSizeProperty = DependencyProperty.Register
        (
            name: "PlaceholderFontSize",
            propertyType: typeof(double),
            ownerType: typeof(SearchConditionTextarea),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: 12d,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public double PlaceholderFontSize
        {
            get { return (double)GetValue(PlaceholderFontSizeProperty); }
            set { SetValue(PlaceholderFontSizeProperty, value); }
        }

        #endregion

        #region [DP] TextBoxHeight

        public static readonly DependencyProperty TextBoxHeightProperty = DependencyProperty.Register
        (
            name: "TextBoxHeight",
            propertyType: typeof(double),
            ownerType: typeof(SearchConditionTextarea),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: 80d,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public double TextBoxHeight
        {
            get { return (double)GetValue(TextBoxHeightProperty); }
            set { SetValue(TextBoxHeightProperty, value); }
        }

        #endregion

        #region [DP] TextBoxBackground

        public static readonly DependencyProperty TextBoxBackgroundProperty = DependencyProperty.Register
        (
            name: "TextBoxBackground",
            propertyType: typeof(System.Windows.Media.Brush),
            ownerType: typeof(SearchConditionTextarea),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(245, 245, 245)),
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public System.Windows.Media.Brush TextBoxBackground
        {
            get { return (System.Windows.Media.Brush)GetValue(TextBoxBackgroundProperty); }
            set { SetValue(TextBoxBackgroundProperty, value); }
        }

        #endregion

        #region [DP] IsReadOnly

        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register
        (
            name: "IsReadOnly",
            propertyType: typeof(bool),
            ownerType: typeof(SearchConditionTextarea),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: false,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        #endregion

        #region [DP] TextBoxIsEnabled

        public static readonly DependencyProperty TextBoxIsEnabledProperty = DependencyProperty.Register
        (
            name: "TextBoxIsEnabled",
            propertyType: typeof(bool),
            ownerType: typeof(SearchConditionTextarea),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: true,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public bool TextBoxIsEnabled
        {
            get { return (bool)GetValue(TextBoxIsEnabledProperty); }
            set { SetValue(TextBoxIsEnabledProperty, value); }
        }

        #endregion

        Util.ActionUtils.DebounceAction mDebounceAction { get; set; } = new Util.ActionUtils.DebounceAction();

        #region [DP] DebounceInterval

        public static readonly DependencyProperty DebounceIntervalProperty = DependencyProperty.Register
        (
            name: "DebounceInterval",
            propertyType: typeof(double),
            ownerType: typeof(SearchConditionTextarea),
            validateValueCallback: null, // new ValidateValueCallback((toValidate) => { return true; }),
            typeMetadata: new PropertyMetadata
            (
                defaultValue: 500d,
                propertyChangedCallback: onDebounceInterval_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public double DebounceInterval
        {
            get { return (double)GetValue(DebounceIntervalProperty); }
            set { SetValue(DebounceIntervalProperty, value); }
        }

        public static void onDebounceInterval_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SearchConditionTextarea target)
            {
                // TODO 逻辑
            }
        }

        #endregion

        private void txt_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            mDebounceAction.Debounce
            (
                interval: DebounceInterval,
                action: calcTxtInfo,
                dispatcher: this.Dispatcher
            );
        }

        private void txt_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            mDebounceAction.Debounce
            (
                interval: DebounceInterval,
                action: calcTxtInfo,
                dispatcher: this.Dispatcher
            );
        }

        /// <summary>
        /// 计算项数 ( 排除空值项 )
        /// </summary>
        void calcTxtInfo()
        {
            string tmp = this.txt.Text;
            if (string.IsNullOrEmpty(tmp))
            {
                string msg = $"0";
                System.Diagnostics.Debug.WriteLine(msg);

                txtInfo.Text = string.Empty;
            }
            else
            {
                var qCount =
                tmp.Split(separator: new string[] { "\r\n" }, options: StringSplitOptions.None)
                    .Where(i => string.IsNullOrEmpty(i) == false)
                    .Count();
                ;

                string msg = $"{qCount}";
                System.Diagnostics.Debug.WriteLine(msg);

                if (qCount <= 0)
                    txtInfo.Text = string.Empty;
                else
                    txtInfo.Text = $"共 {qCount} 项";
            }
        }

        // TextBoxAdv 可以使用 Ctrl + Backspace 或者 使用自定义 ContextMenu 中的清空内容功能实现内容的快速清除
        // 故已停用 txtInfo.MouseDown 事件
        #region [已停用] 双击 txtInfo 调出 确认清空对话框

        long mMouseDownLatestTicks { get; set; } = DateTime.MaxValue.Ticks;

        Point mMouseDownLatestPoint { get; set; }

        void txtInfo_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            long time = DateTime.Now.Ticks;

            Point p = e.GetPosition(this);

            // 判断是否为双击
            if
            (
                Math.Abs(p.X - mMouseDownLatestPoint.X) < 4 && Math.Abs(p.Y - mMouseDownLatestPoint.Y) < 4 // 两次单机距离不超过4像素
                && time - mMouseDownLatestTicks < TimeSpan.FromMilliseconds(500).Ticks // 时间在 0.5 秒以内
            )
            {
                var dr = WPFControls.MessageBox.ShowConfirmDialog(message: $"确认清空？\r\n清空【{this.Title}】内容");
                if (dr == MessageBoxResult.OK)
                {
                    this.Reset();
                }
            }

            mMouseDownLatestTicks = time;
            mMouseDownLatestPoint = p;
        }

        #endregion
    }
}
