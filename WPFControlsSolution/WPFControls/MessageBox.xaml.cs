using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPFControls
{
    // V 1.0.7 - 2021-10-28 15:15:40
    // 1 界面新增 ExtraContent 类型为 ContentControl, 可以按要求传入 ContentCotrol ( 必须是实现 IDataErrorInfo 接口, 点击【确认】按钮时会校验 IDataErrorInfo.Error 属性 )
    // 可以快捷地自定义一些简单的输入 例如 账号密码 / 打印数量 等输入框
    // 2 按钮样式优化, 使用 Client.Components.ButtonBase
    // 3 优化样式, 全部使用 Key 的方式调用样式, 从而减少由于样式的向下延申导致各种怪异的样式问题 ( 例如 DatePicker 的样式被 原先 MessageBox 资源中的 Button Image 样式搞到乱晒隆 )
    // 4 提供默认按钮给程序员选择
    // 
    // V 1.0.6 - 2021-08-26 09:15:17
    // 优化传入当前窗口参数为空值时, 获取 Application.Current.Windows 中 IsActive = true 的首个 Window
    // 
    // V 1.0.5 - 2021-07-25 11:55:59
    // 设置默认 WindowStartupLocation 为 CenterOwner
    // 
    // V 1.0.4 - 2021-06-24 16:55:46
    // 优化自动关闭相关逻辑：在自动关闭窗口前确保 IsFocused 为 true
    // 
    // V 1.0.3 - 2021-06-24 13:51:48
    // 1. 增加 autoCloseTimeSpan （倒计时自动关闭）
    // 2. 优化 ShowError 显示详细异常信息逻辑（ 将 HowesDOMO.Utils 的 Exception.GetInfo 逻辑搬移到此处 ）
    // 
    // V 1.0.2 - 2021-06-23 12:16:15
    // 为了区分 Show 与 ShowDialog，新增 ShowDialog方法，修改原来的 Show 方法 messageBox.ShowDialog(); ==> messageBox.Show();
    // 
    // V 1.0.1 - 2021-03-21 17:55:22
    // 弃用System.Windows.Forms.Screen.PrimaryScreen.WorkingArea的方式获取屏幕分辨率,
    // 改用ScreenUtils( 从 github 上获取的开源项目, 已嵌入到 WPFControls )
    // 
    // V 1.0.0 - 2021-03-21 14:36:58
    // 改写 项目
    // 整理代码, 并对以下几点作了优化
    // 1. 当含有详情信息时, 可以使用 GridSplitter 上下拖动来改变 主信息与详情信息的显示空间大小
    // 2. UserDefineFontSizeDict ( 用户自定义 FontSize 字典 ), 用来自定义某些窗体下字体的自定义大小
    // 3. 采用读取当前计算机的 WorkArea 的长宽像数, 来设置窗体内各个地方对应的最大宽度或最大高度
    // 4. UI优化 ( 重改结构, 优化按钮信息(快捷键按钮加下划线) 等 )

    /// <summary>
    /// <para>重新封装的 MessageBox</para>
    /// <para>Confirm 与 Question 的区别: Confirm [Ok, Cancel]; Question [Yes, No, Cancel]</para>
    /// </summary>
    public partial class MessageBox : INotifyPropertyChanged
    {
        /// <summary>
        /// 用户自定义 FontSize 字典
        /// 程序StartUp时, 应该向本属性指定需要自定义FontSize的窗体FullName
        /// </summary>
        private static System.Collections.Generic.Dictionary<string, double> UserDefineFontSizeDict { get; set; } = new System.Collections.Generic.Dictionary<string, double>();

        public static void AddUserDefineFontSize(string key, double value)
        {
            if (UserDefineFontSizeDict.ContainsKey(key) == false)
            {
                UserDefineFontSizeDict.Add(key, value);
            }
            else
            {
                UserDefineFontSizeDict[key] = value;
            }
        }

        bool mAnimationRan { get; set; } = false;

        public MessageBox(Window owner, string message, string details, MessageBoxButton button, MessageBoxImage icon,
                          MessageBoxResult defaultResult, MessageBoxOptions options, TimeSpan? autoCloseTimeSpan = null)
        {
            InitializeComponent();

            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            try
            {
                this.Owner = owner ?? getCurrentWindow();
            }
            catch (Exception)
            {
                this.Owner = Application.Current.MainWindow;
            }

            // 用户自定义 FontSize 字典
            if (UserDefineFontSizeDict.TryGetValue(this.Owner.ToString(), out double userDefineFontSize))
            {
                this.FontSize = userDefineFontSize;
            }

            // 设置 FontSize
            this.MessageText.FontSize = this.FontSize;

            if (userDefineFontSize > 25)
            {
                // 设置 ButtonPanel 的 FontSize ( 由于StackPanel无FontSize, 使用Style进行设置 )
                Style style = new Style();
                style.Setters.Add(new Setter(FontSizeProperty, this.FontSize * 0.7d));
                this.ButtonsPanel.Style = style;
            }

            this.CreateButtons(button, defaultResult);
            this.CreateImage(icon);
            this.ApplyOptions(options);

            // Message
            this.MessageText.Text = message;

            // Details
            bool hasDetail = string.IsNullOrWhiteSpace(details) == false;
            this.DetailsExpander.Visibility = hasDetail ? Visibility.Visible : Visibility.Collapsed;
            if (hasDetail)
            {
                this.DetailsText.Text = details;
                this.gridSplitter0.Visibility = Visibility.Visible;
                this.gridSplitter1.Visibility = Visibility.Visible;
            }
            else
            {
                this.gridSplitter0.Visibility = Visibility.Collapsed;
                this.gridSplitter1.Visibility = Visibility.Collapsed;
            }

            if (button == MessageBoxButton.YesNo || button == MessageBoxButton.YesNoCancel)
            {
                this.KeyDown += new KeyEventHandler(MessageBox_KeyDown);
            }

            this.Loaded += frm_Loaded;
            this.Closed += frm_Closed;
            this.Closing += frm_Closing;
            this.MouseDown += (s, e) => { if (e.ChangedButton == MouseButton.Left) { this.DragMove(); } };

            // AutoClose
            if (autoCloseTimeSpan.HasValue == true)
            {
                mPlanTicks = autoCloseTimeSpan.Value.Ticks;
                lblAutoClose.Text = ToStringAdvSimple(autoCloseTimeSpan.Value);

                mDispatcherTimer = new System.Windows.Threading.DispatcherTimer();
                mDispatcherTimer.Interval = mInterval;
                mDispatcherTimer.Tick += timer_Tick;
                mDispatcherTimer.Start();
            }
        }

        private void frm_Closed(object sender, EventArgs e)
        {
            closeDispatcherTimer();
        }



        void MessageBox_KeyDown(object sender, KeyEventArgs e)
        {
            ModifierKeys modifiers = e.KeyboardDevice.Modifiers;

            if (e.SystemKey == Key.Y && (int)modifiers == (int)(ModifierKeys.Alt))
            {
                if (this.yesButton != null)
                {
                    this.yesButton.Focus();

                    this.MessageBoxResult = (MessageBoxResult)this.yesButton.Tag;

                    this.Close();
                }
            }

            if (e.SystemKey == Key.N && (int)modifiers == (int)(ModifierKeys.Alt))
            {
                if (this.noButton != null)
                {
                    this.noButton.Focus();

                    this.MessageBoxResult = (MessageBoxResult)this.noButton.Tag;

                    this.Close();
                }
            }

        }

        public MessageBoxResult MessageBoxResult { get; private set; }

        #region Create Buttons

        /// <summary>
        /// Create the message box's button according to the user's demand
        /// </summary>
        /// <param name="button">The user's buttons selection</param>
        /// <param name="defaultResult">The default button</param>
        private void CreateButtons(MessageBoxButton button, MessageBoxResult defaultResult)
        {
            switch (button)
            {
                case MessageBoxButton.OK:
                    this.ButtonsPanel.Children.Add(this.CreateOkButton(defaultResult));
                    break;
                case MessageBoxButton.OKCancel:
                    this.ButtonsPanel.Children.Add(this.CreateOkButton(defaultResult));
                    this.ButtonsPanel.Children.Add(this.CreateCancelButton(defaultResult));
                    break;
                case MessageBoxButton.YesNoCancel:
                    this.ButtonsPanel.Children.Add(this.CreateYesButton(defaultResult));
                    this.ButtonsPanel.Children.Add(this.CreateNoButton(defaultResult));
                    this.ButtonsPanel.Children.Add(this.CreateCancelButton(defaultResult));
                    break;
                case MessageBoxButton.YesNo:
                    this.ButtonsPanel.Children.Add(this.CreateYesButton(defaultResult));
                    this.ButtonsPanel.Children.Add(this.CreateNoButton(defaultResult));
                    break;
                default:
                    throw new ArgumentOutOfRangeException("button");
            }
        }

        static double BtnWidth = Double.NaN;
        static double BtnHeight = Double.NaN;
        static Thickness MessageBoxButtonMargin = new Thickness(0, 10, 5, 5);

        private Button okButton;

        /// <summary>
        /// Create the ok button on demand
        /// </summary>
        /// <param name="defaultResult"></param>
        /// <returns></returns>
        private Button CreateOkButton(MessageBoxResult defaultResult)
        {
            this.okButton = new Client.Components.ButtonBase
            {
                Name = "okButton",
                Width = BtnWidth,
                Height = BtnHeight,
                Margin = MessageBoxButtonMargin,
                Title = "确定", // "OK",
                IsDefault = defaultResult == MessageBoxResult.OK,
                Tag = MessageBoxResult.OK,
            };

            this.okButton.Width += this.FontSize;
            this.okButton.Height += this.FontSize;
            this.okButton.Click += ButtonClick;

            return this.okButton;
        }

        /// <summary>
        /// Create the cancel button on demand
        /// </summary>
        /// <param name="defaultResult"></param>
        /// <returns></returns>
        private Button CreateCancelButton(MessageBoxResult defaultResult)
        {
            Button cancelButton = new Client.Components.ButtonBase
            {
                Name = "cancelButton",
                Width = BtnWidth,
                Height = BtnHeight,
                Margin = MessageBoxButtonMargin,
                Title = "取消", //"Cancel",
                IsDefault = defaultResult == MessageBoxResult.Cancel,
                IsCancel = true,
                Tag = MessageBoxResult.Cancel,
            };

            cancelButton.Width += this.FontSize;
            cancelButton.Height += this.FontSize;
            cancelButton.Click += ButtonClick;

            return cancelButton;
        }

        private Button yesButton;
        /// <summary>
        /// Create the yes button on demand
        /// </summary>
        /// <param name="defaultResult"></param>
        /// <returns></returns>
        private Button CreateYesButton(MessageBoxResult defaultResult)
        {
            //var content = new TextBlock();
            //content.Inlines.Add("是(");
            //content.Inlines.Add(new Run("Y") { TextDecorations = TextDecorations.Underline });
            //content.Inlines.Add(")");

            this.yesButton = new Client.Components.ButtonBase
            {
                Name = "yesButton",
                Width = BtnWidth,
                Height = BtnHeight,
                Margin = MessageBoxButtonMargin,
                Title = "是",
                IsDefault = defaultResult == MessageBoxResult.Yes,
                Tag = MessageBoxResult.Yes,
            };

            this.yesButton.Width += this.FontSize;
            this.yesButton.Height += this.FontSize;
            this.yesButton.Click += ButtonClick;

            return this.yesButton;
        }

        private Button noButton;
        /// <summary>
        /// Create the no button on demand
        /// </summary>
        /// <param name="defaultResult"></param>
        /// <returns></returns>
        private Button CreateNoButton(MessageBoxResult defaultResult)
        {
            //var content = new TextBlock();
            //content.Inlines.Add("否(");
            //content.Inlines.Add(new Run("N") { TextDecorations = TextDecorations.Underline });
            //content.Inlines.Add(")");

            this.noButton = new Client.Components.ButtonBase
            {
                Name = "noButton",
                Width = BtnWidth,
                Height = BtnHeight,
                Margin = MessageBoxButtonMargin,
                Title = "否",
                IsDefault = defaultResult == MessageBoxResult.No,
                Tag = MessageBoxResult.No,
            };

            this.noButton.Width += this.FontSize;
            this.noButton.Height += this.FontSize;
            this.noButton.Click += ButtonClick;

            return this.noButton;
        }

        /// <summary>
        /// The event the buttons trigger. 
        /// Each button hold it's result in the tag, so here it just sets them and close the message box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var value = (MessageBoxResult)btn.Tag;

            if
            (
                // 点击[确认按钮]后
                (value == MessageBoxResult.Yes || value == MessageBoxResult.OK)

                // 若含有 ExtraContent，需要验证 IDataErrorInfo 接口的 Error 属性
                // 当 Error属性 非空，才能继续后续的逻辑
                && this.ExtraContent != null
                && this.ExtraContent is ContentControl cc
                && cc.DataContext is IDataErrorInfo dataContext
                && string.IsNullOrWhiteSpace(dataContext.Error) == false
            )
            {
                System.Diagnostics.Debug.WriteLine("未通过 IDataErrorInfo 验证");
                System.Diagnostics.Debugger.Break();
                return;
            }

            this.MessageBoxResult = value;
            this.Close();
        }

        #endregion

        private void ApplyOptions(MessageBoxOptions options)
        {
            if ((options & MessageBoxOptions.RightAlign) == MessageBoxOptions.RightAlign)
            {
                this.MessageText.TextAlignment = TextAlignment.Right;
                this.DetailsText.TextAlignment = TextAlignment.Right;
            }
            if ((options & MessageBoxOptions.RtlReading) == MessageBoxOptions.RtlReading)
            {
                this.FlowDirection = FlowDirection.RightToLeft;
            }
        }

        #region 左边图标的创建

        /// <summary>
        /// Create the image from the system's icons
        /// </summary>
        /// <param name="icon"></param>
        private void CreateImage(MessageBoxImage icon)
        {
            switch (icon)
            {
                case MessageBoxImage.None:
                    this.ImagePlaceholder.Visibility = Visibility.Collapsed;
                    break;
                case MessageBoxImage.Information:
                    // this.ImagePlaceholder.Source = SystemIcons.Information.ToImageSource();
                    this.ImagePlaceholder.Source = ToImageSource(SystemIcons.Information);
                    break;
                case MessageBoxImage.Question:
                    // this.ImagePlaceholder.Source = SystemIcons.Question.ToImageSource();
                    this.ImagePlaceholder.Source = ToImageSource(SystemIcons.Question);
                    break;
                case MessageBoxImage.Warning:
                    // this.ImagePlaceholder.Source = SystemIcons.Warning.ToImageSource();
                    this.ImagePlaceholder.Source = ToImageSource(SystemIcons.Warning);
                    break;
                case MessageBoxImage.Error:
                    // this.ImagePlaceholder.Source = SystemIcons.Error.ToImageSource();
                    this.ImagePlaceholder.Source = ToImageSource(SystemIcons.Error);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("icon");
            }
        }

        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern bool DeleteObject(IntPtr hObject);

        public static ImageSource ToImageSource(Icon icon)
        {
            Bitmap bitmap = icon.ToBitmap();
            IntPtr hbitmap = bitmap.GetHbitmap();
            ImageSource imageSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hbitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            if (!DeleteObject(hbitmap))
            {
                throw new Win32Exception();
            }
            return imageSource.GetAsFrozen() as ImageSource;
        }

        #endregion

        /// <summary>
        /// Show the startup animation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void frm_Loaded(object sender, RoutedEventArgs e)
        {
            #region  Add By howe

            // DetailsExpander.IsExpanded = false;

            // var rect = PrimaryScreenWorkingArea();
            var screen = WPFControls.ScreenUtils.Screen.FromPoint(WPFControls.ScreenUtils.MouseUtils.MousePosition);
            var rect = screen.WorkingArea;

            double maxWidth = (double)rect.Width * 0.6d;
            this.MaxWidth = maxWidth + 10;
            gridMain.MaxWidth = maxWidth;
            gridSplitter0.MaxWidth = maxWidth;
            gridSplitter1.MaxWidth = maxWidth;
            gridDetail.MaxWidth = maxWidth;

            double marginHeight = 5;
            double gridSplitterHeight = 3;

            thisWindow.MaxHeight = rect.Height - (marginHeight * 2);

            double gridMainMiniHeight = 60;
            double totalHeight = rect.Height - (marginHeight * 2) - gridSplitterHeight;
            gridMain.MinHeight = gridMainMiniHeight;
            gridMain.MaxHeight = totalHeight;


            gridDetail.MinHeight = 40;
            gridDetail.MaxHeight = totalHeight - gridSplitterHeight - gridMainMiniHeight;


            if (string.IsNullOrEmpty(this.DetailsText.Text) == false)
            {
                if (gridDetail.MinWidth < ButtonsPanel.ActualWidth)
                {
                    double minWidth = ButtonsPanel.ActualWidth;

                    minWidth += zhanweifu.ActualWidth; // 计算左边详情的实际宽度

                    this.MinWidth = minWidth + 20;
                    gridMain.MinWidth = minWidth;
                    gridDetail.MinWidth = minWidth;
                }
            }

            #endregion

            #region Loaded

            // This is set here to height after the width has been set 
            // so the details expander won't stretch the message box when it's opened
            this.SizeToContent = SizeToContent.Height;
            //var animation = TryFindResource("LoadAnimation") as Storyboard;
            //animation.Begin(this);

            // 关注按钮焦点
            if (this.yesButton != null) { this.yesButton.Focus(); }
            if (this.okButton != null) { this.okButton.Focus(); }

            #endregion
        }

        /// <summary>
        /// Show the closing animation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frm_Closing(object sender, CancelEventArgs e)
        {
            if (!this.mAnimationRan)
            {
                // The animation won't run if the window is allowed to close, 
                // so here the animation starts, and the window's closing is canceled
                e.Cancel = true;

                var animation = TryFindResource("UnloadAnimation") as Storyboard;

                animation.Completed += (s, a) =>
                {
                    this.mAnimationRan = true;
                    this.Close();
                };

                animation.Begin(this);
            }
        }

        #region Show Information

        /// <summary>
        /// Display an information message
        /// </summary>
        /// <param name="message">The message text</param>
        /// <param name="details">The details part text</param>
        /// <param name="showCancel">Display the cancel</param>
        /// <param name="options">Misc options</param>
        /// <returns>The user's selected button</returns>
        public static MessageBoxResult ShowInformation(string message, string details = "", bool showCancel = false,
                                                       MessageBoxOptions options = MessageBoxOptions.None,
                                                       TimeSpan? autoCloseTimeSpan = null)
        {
            return ShowInformation
            (
                owner: null,
                message: message,
                details: details,
                showCancel: showCancel,
                options: options,
                autoCloseTimeSpan: autoCloseTimeSpan
            );
        }

        /// <summary>
        /// Display an information message
        /// </summary>
        /// <param name="owner">The message box's parent window</param>
        /// <param name="message">The message text</param>
        /// <param name="details">The details part text</param>
        /// <param name="showCancel">Display the cancel</param>
        /// <param name="options">Misc options</param>
        /// <returns>The user's selected button</returns>
        public static MessageBoxResult ShowInformation(Window owner, string message, string details = "",
                                                       bool showCancel = false,
                                                       MessageBoxOptions options = MessageBoxOptions.None,
                                                       TimeSpan? autoCloseTimeSpan = null)
        {
            return Show
            (
                owner: owner,
                message: message,
                details: details,
                button: showCancel ? MessageBoxButton.OKCancel : MessageBoxButton.OK,
                icon: MessageBoxImage.Information,
                defaultResult: MessageBoxResult.OK,
                options: options,
                autoCloseTimeSpan: autoCloseTimeSpan
            );
        }

        #endregion

        #region ShowDialog Information

        /// <summary>
        /// Display an information message
        /// </summary>
        /// <param name="message">The message text</param>
        /// <param name="details">The details part text</param>
        /// <param name="showCancel">Display the cancel</param>
        /// <param name="options">Misc options</param>
        /// <returns>The user's selected button</returns>
        public static MessageBoxResult ShowInformationDialog(string message, string details = "", bool showCancel = false,
                                                       MessageBoxOptions options = MessageBoxOptions.None,
                                                       TimeSpan? autoCloseTimeSpan = null)
        {
            return ShowInformationDialog
            (
                owner: null,
                message: message,
                details: details,
                showCancel: showCancel,
                options: options,
                autoCloseTimeSpan: autoCloseTimeSpan
            );
        }

        /// <summary>
        /// Display an information message
        /// </summary>
        /// <param name="owner">The message box's parent window</param>
        /// <param name="message">The message text</param>
        /// <param name="details">The details part text</param>
        /// <param name="showCancel">Display the cancel</param>
        /// <param name="options">Misc options</param>
        /// <returns>The user's selected button</returns>
        public static MessageBoxResult ShowInformationDialog(Window owner, string message, string details = "",
                                                       bool showCancel = false,
                                                       MessageBoxOptions options = MessageBoxOptions.None,
                                                       TimeSpan? autoCloseTimeSpan = null)
        {
            return ShowDialog
            (
                owner: owner,
                message: message,
                details: details,
                button: showCancel ? MessageBoxButton.OKCancel : MessageBoxButton.OK,
                icon: MessageBoxImage.Information,
                defaultResult: MessageBoxResult.OK,
                options: options,
                autoCloseTimeSpan: autoCloseTimeSpan
            );
        }

        #endregion

        #region Show Confirm

        /// <summary>
        /// Display a question
        /// </summary>
        /// <param name="message">The message text</param>
        /// <param name="details">The details part text</param>
        /// <param name="showCancel">Display the cancel</param>
        /// <param name="options">Misc options</param>
        /// <returns>The user's selected button</returns>
        public static MessageBoxResult ShowConfirm(string message, string details = "",
                                                    bool showCancel = false,
                                                    MessageBoxOptions options = MessageBoxOptions.None,
                                                    TimeSpan? autoCloseTimeSpan = null)
        {
            return ShowConfirm
            (
                owner: null,
                message: message,
                details: details,
                showCancel: showCancel,
                options: options,
                autoCloseTimeSpan: autoCloseTimeSpan
            );
        }

        /// <summary>
        /// Display a question
        /// </summary>
        /// <param name="owner">The message box's parent window</param>
        /// <param name="message">The message text</param>
        /// <param name="details">The details part text</param>
        /// <param name="showCancel">Display the cancel</param>
        /// <param name="options">Misc options</param>
        /// <returns>The user's selected button</returns>
        public static MessageBoxResult ShowConfirm(Window owner, string message, string details = "",
                                                    bool showCancel = false,
                                                    MessageBoxOptions options = MessageBoxOptions.None,
                                                    TimeSpan? autoCloseTimeSpan = null)
        {
            return Show
            (
                owner: owner,
                message: message,
                details: details,
                button: showCancel ? MessageBoxButton.OKCancel : MessageBoxButton.OKCancel,
                icon: MessageBoxImage.Question,
                defaultResult: MessageBoxResult.Yes,
                options: options,
                autoCloseTimeSpan: autoCloseTimeSpan
            );
        }

        #endregion

        #region ShowDialog Confirm

        /// <summary>
        /// Display a question
        /// </summary>
        /// <param name="message">The message text</param>
        /// <param name="details">The details part text</param>
        /// <param name="showCancel">Display the cancel</param>
        /// <param name="options">Misc options</param>
        /// <returns>The user's selected button</returns>
        public static MessageBoxResult ShowConfirmDialog(string message, string details = "",
                                                    bool showCancel = false,
                                                    MessageBoxOptions options = MessageBoxOptions.None,
                                                    TimeSpan? autoCloseTimeSpan = null)
        {
            return ShowConfirmDialog
            (
                owner: null,
                message: message,
                details: details,
                showCancel: showCancel,
                options: options,
                autoCloseTimeSpan: autoCloseTimeSpan
            );
        }

        /// <summary>
        /// Display a question
        /// </summary>
        /// <param name="owner">The message box's parent window</param>
        /// <param name="message">The message text</param>
        /// <param name="details">The details part text</param>
        /// <param name="showCancel">Display the cancel</param>
        /// <param name="options">Misc options</param>
        /// <returns>The user's selected button</returns>
        public static MessageBoxResult ShowConfirmDialog(Window owner, string message, string details = "",
                                                    bool showCancel = false,
                                                    MessageBoxOptions options = MessageBoxOptions.None,
                                                    TimeSpan? autoCloseTimeSpan = null)
        {
            return ShowDialog
            (
                owner: owner,
                message: message,
                details: details,
                button: showCancel ? MessageBoxButton.OKCancel : MessageBoxButton.OKCancel,
                icon: MessageBoxImage.Question,
                defaultResult: MessageBoxResult.Yes,
                options: options,
                autoCloseTimeSpan: autoCloseTimeSpan
            );
        }

        #endregion

        #region Show Question

        /// <summary>
        /// Display a question
        /// </summary>
        /// <param name="message">The message text</param>
        /// <param name="details">The details part text</param>
        /// <param name="showCancel">Display the cancel</param>
        /// <param name="options">Misc options</param>
        /// <returns>The user's selected button</returns>
        public static MessageBoxResult ShowQuestion(string message, string details = "",
                                                    bool showCancel = false,
                                                    MessageBoxOptions options = MessageBoxOptions.None,
                                                    TimeSpan? autoCloseTimeSpan = null)
        {
            return ShowQuestion
            (
                owner: null,
                message: message,
                details: details,
                showCancel: showCancel,
                options: options,
                autoCloseTimeSpan: autoCloseTimeSpan
            );
        }

        /// <summary>
        /// Display a question
        /// </summary>
        /// <param name="owner">The message box's parent window</param>
        /// <param name="message">The message text</param>
        /// <param name="details">The details part text</param>
        /// <param name="showCancel">Display the cancel</param>
        /// <param name="options">Misc options</param>
        /// <returns>The user's selected button</returns>
        public static MessageBoxResult ShowQuestion(Window owner, string message, string details = "",
                                                    bool showCancel = false,
                                                    MessageBoxOptions options = MessageBoxOptions.None,
                                                    TimeSpan? autoCloseTimeSpan = null)
        {
            return Show
            (
                owner: owner,
                message: message,
                details: details,
                button: showCancel ? MessageBoxButton.YesNoCancel : MessageBoxButton.YesNo,
                icon: MessageBoxImage.Question,
                defaultResult: MessageBoxResult.Yes,
                options: options,
                autoCloseTimeSpan: autoCloseTimeSpan
            );
        }

        #endregion

        #region ShowDialog Question

        /// <summary>
        /// Display a question
        /// </summary>
        /// <param name="message">The message text</param>
        /// <param name="details">The details part text</param>
        /// <param name="showCancel">Display the cancel</param>
        /// <param name="options">Misc options</param>
        /// <returns>The user's selected button</returns>
        public static MessageBoxResult ShowQuestionDialog(string message, string details = "",
                                                    bool showCancel = false,
                                                    MessageBoxOptions options = MessageBoxOptions.None,
                                                    TimeSpan? autoCloseTimeSpan = null)
        {
            return ShowQuestionDialog
            (
                owner: null,
                message: message,
                details: details,
                showCancel: showCancel,
                options: options,
                autoCloseTimeSpan: autoCloseTimeSpan
            );
        }

        /// <summary>
        /// Display a question
        /// </summary>
        /// <param name="owner">The message box's parent window</param>
        /// <param name="message">The message text</param>
        /// <param name="details">The details part text</param>
        /// <param name="showCancel">Display the cancel</param>
        /// <param name="options">Misc options</param>
        /// <returns>The user's selected button</returns>
        public static MessageBoxResult ShowQuestionDialog(Window owner, string message, string details = "",
                                                    bool showCancel = false,
                                                    MessageBoxOptions options = MessageBoxOptions.None,
                                                    TimeSpan? autoCloseTimeSpan = null)
        {
            return ShowDialog
            (
                owner: owner,
                message: message,
                details: details,
                button: showCancel ? MessageBoxButton.YesNoCancel : MessageBoxButton.YesNo,
                icon: MessageBoxImage.Question,
                defaultResult: MessageBoxResult.Yes,
                options: options,
                autoCloseTimeSpan: autoCloseTimeSpan
            );
        }

        #endregion

        #region Show Warning

        /// <summary>
        /// Display a warning
        /// </summary>
        /// <param name="message">The message text</param>
        /// <param name="details">The details part text</param>
        /// <param name="showCancel">Display the cancel</param>
        /// <param name="options">Misc options</param>
        /// <returns>The user's selected button</returns>
        public static MessageBoxResult ShowWarning(string message, string details = "",
                                                   bool showCancel = false,
                                                   MessageBoxOptions options = MessageBoxOptions.None,
                                                   TimeSpan? autoCloseTimeSpan = null)
        {
            return ShowWarning
            (
                owner: null,
                message: message,
                details: details,
                showCancel: showCancel,
                options: options,
                autoCloseTimeSpan: autoCloseTimeSpan
            );
        }

        /// <summary>
        /// Display a warning
        /// </summary>
        /// <param name="owner">The message box's parent window</param>
        /// <param name="message">The message text</param>
        /// <param name="details">The details part text</param>
        /// <param name="showCancel">Display the cancel</param>
        /// <param name="options">Misc options</param>
        /// <returns>The user's selected button</returns>
        public static MessageBoxResult ShowWarning(Window owner, string message, string details = "",
                                                   bool showCancel = false,
                                                   MessageBoxOptions options = MessageBoxOptions.None,
                                                   TimeSpan? autoCloseTimeSpan = null)
        {
            return Show
            (
                owner: owner,
                message: message,
                details: details,
                button: showCancel ? MessageBoxButton.OKCancel : MessageBoxButton.OK,
                icon: MessageBoxImage.Warning,
                defaultResult: MessageBoxResult.OK,
                options: options,
                autoCloseTimeSpan: autoCloseTimeSpan
            );
        }

        #endregion

        #region ShowDialog Warning

        /// <summary>
        /// Display a warning
        /// </summary>
        /// <param name="message">The message text</param>
        /// <param name="details">The details part text</param>
        /// <param name="showCancel">Display the cancel</param>
        /// <param name="options">Misc options</param>
        /// <returns>The user's selected button</returns>
        public static MessageBoxResult ShowWarningDialog(string message, string details = "",
                                                   bool showCancel = false,
                                                   MessageBoxOptions options = MessageBoxOptions.None,
                                                   TimeSpan? autoCloseTimeSpan = null)
        {
            return ShowWarningDialog
            (
                owner: null,
                message: message,
                details: details,
                showCancel: showCancel,
                options: options,
                autoCloseTimeSpan: autoCloseTimeSpan
            );
        }

        /// <summary>
        /// Display a warning
        /// </summary>
        /// <param name="owner">The message box's parent window</param>
        /// <param name="message">The message text</param>
        /// <param name="details">The details part text</param>
        /// <param name="showCancel">Display the cancel</param>
        /// <param name="options">Misc options</param>
        /// <returns>The user's selected button</returns>
        public static MessageBoxResult ShowWarningDialog(Window owner, string message, string details = "",
                                                   bool showCancel = false,
                                                   MessageBoxOptions options = MessageBoxOptions.None,
                                                   TimeSpan? autoCloseTimeSpan = null)
        {
            return ShowDialog
            (
                owner: owner,
                message: message,
                details: details,
                button: showCancel ? MessageBoxButton.OKCancel : MessageBoxButton.OK,
                icon: MessageBoxImage.Warning,
                defaultResult: MessageBoxResult.OK,
                options: options,
                autoCloseTimeSpan: autoCloseTimeSpan
            );
        }

        #endregion

        #region Show Error

        /// <summary>
        /// Display an Error
        /// </summary>
        /// <param name="exception">Display the exception's details</param>
        /// <param name="message">The message text</param>
        /// <param name="options">Misc options</param>
        /// <returns>The user's selected button</returns>
        public static MessageBoxResult ShowError(Exception exception, string message = "",
                                                 MessageBoxOptions options = MessageBoxOptions.None,
                                                 TimeSpan? autoCloseTimeSpan = null)
        {
            return ShowError
            (
                owner: null,
                exception: exception,
                message: message,
                options: options,
                autoCloseTimeSpan: autoCloseTimeSpan
            );
        }

        /// <summary>
        /// Display an Error
        /// </summary>
        /// <param name="message">The message text</param>
        /// <param name="details">The details part text</param>
        /// <param name="showCancel">Display the cancel</param>
        /// <param name="options">Misc options</param>
        /// <returns>The user's selected button</returns>
        public static MessageBoxResult ShowError(string message, string details = "",
                                                 bool showCancel = false,
                                                 MessageBoxOptions options = MessageBoxOptions.None,
                                                 TimeSpan? autoCloseTimeSpan = null)
        {
            return ShowError
            (
                owner: null,
                message: message,
                details: details,
                showCancel: showCancel,
                options: options,
                autoCloseTimeSpan: autoCloseTimeSpan
            );
        }

        /// <summary>
        /// Display an Error
        /// </summary>
        /// <param name="owner">The message box's parent window</param>
        /// <param name="exception">Display the exception's details</param>
        /// <param name="message">The message text</param>
        /// <param name="options">Misc options</param>
        /// <returns>The user's selected button</returns>
        public static MessageBoxResult ShowError(Window owner, Exception exception, string message = "",
                                                 MessageBoxOptions options = MessageBoxOptions.None,
                                                 TimeSpan? autoCloseTimeSpan = null)
        {
            string details = string.Empty;

            if (exception != null)
            {
                details = ExceptionGetInfo(exception);
            }

            return Show
            (
                owner: owner,
                message: String.IsNullOrEmpty(message) ? exception.Message : message,
                details: details,
                button: MessageBoxButton.OK,
                icon: MessageBoxImage.Error,
                defaultResult: MessageBoxResult.OK,
                options: options,
                autoCloseTimeSpan: autoCloseTimeSpan
            );
        }

        /// <summary>
        /// Display an Error
        /// </summary>
        /// <param name="owner">The message box's parent window</param>
        /// <param name="message">The message text</param>
        /// <param name="details">The details part text</param>
        /// <param name="showCancel">Display the cancel</param>
        /// <param name="options">Misc options</param>
        /// <returns>The user's selected button</returns>
        public static MessageBoxResult ShowError(Window owner, string message, string details = "",
                                                 bool showCancel = false,
                                                 MessageBoxOptions options = MessageBoxOptions.None, TimeSpan? autoCloseTimeSpan = null)
        {
            return Show
            (
                owner: owner,
                message: message,
                details: details,
                button: showCancel ? MessageBoxButton.OKCancel : MessageBoxButton.OK,
                icon: MessageBoxImage.Error,
                defaultResult: MessageBoxResult.OK,
                options: options,
                autoCloseTimeSpan: autoCloseTimeSpan
            );
        }

        #endregion

        #region ShowDialog Error

        /// <summary>
        /// Display an Error
        /// </summary>
        /// <param name="exception">Display the exception's details</param>
        /// <param name="message">The message text</param>
        /// <param name="options">Misc options</param>
        /// <returns>The user's selected button</returns>
        public static MessageBoxResult ShowErrorDialog(Exception exception, string message = "",
                                                 MessageBoxOptions options = MessageBoxOptions.None,
                                                 TimeSpan? autoCloseTimeSpan = null)
        {
            return ShowErrorDialog
            (
                owner: null,
                exception: exception,
                message: message,
                options: options,
                autoCloseTimeSpan: autoCloseTimeSpan
            );
        }

        /// <summary>
        /// Display an Error
        /// </summary>
        /// <param name="message">The message text</param>
        /// <param name="details">The details part text</param>
        /// <param name="showCancel">Display the cancel</param>
        /// <param name="options">Misc options</param>
        /// <returns>The user's selected button</returns>
        public static MessageBoxResult ShowErrorDialog(string message, string details = "",
                                                 bool showCancel = false,
                                                 MessageBoxOptions options = MessageBoxOptions.None,
                                                 TimeSpan? autoCloseTimeSpan = null)
        {
            return ShowErrorDialog
            (
                owner: null,
                message: message,
                details: details,
                showCancel: showCancel,
                options: options,
                autoCloseTimeSpan: autoCloseTimeSpan
            );
        }

        /// <summary>
        /// Display an Error
        /// </summary>
        /// <param name="owner">The message box's parent window</param>
        /// <param name="exception">Display the exception's details</param>
        /// <param name="message">The message text</param>
        /// <param name="options">Misc options</param>
        /// <returns>The user's selected button</returns>
        public static MessageBoxResult ShowErrorDialog(Window owner, Exception exception, string message = "",
                                                 MessageBoxOptions options = MessageBoxOptions.None,
                                                 TimeSpan? autoCloseTimeSpan = null)
        {
            string details = string.Empty;

            if (exception != null)
            {
                details = ExceptionGetInfo(exception);
            }

            return ShowDialog
            (
                owner: owner,
                message: String.IsNullOrEmpty(message) ? exception.Message : message,
                details: details,
                button: MessageBoxButton.OK,
                icon: MessageBoxImage.Error,
                defaultResult: MessageBoxResult.OK,
                options: options,
                autoCloseTimeSpan: autoCloseTimeSpan
            );
        }

        /// <summary>
        /// Display an Error
        /// </summary>
        /// <param name="owner">The message box's parent window</param>
        /// <param name="message">The message text</param>
        /// <param name="details">The details part text</param>
        /// <param name="showCancel">Display the cancel</param>
        /// <param name="options">Misc options</param>
        /// <returns>The user's selected button</returns>
        public static MessageBoxResult ShowErrorDialog(Window owner, string message, string details = "",
                                                 bool showCancel = false,
                                                 MessageBoxOptions options = MessageBoxOptions.None,
                                                 TimeSpan? autoCloseTimeSpan = null)
        {
            return ShowDialog
            (
                owner: owner,
                message: message,
                details: details,
                button: showCancel ? MessageBoxButton.OKCancel : MessageBoxButton.OK,
                icon: MessageBoxImage.Error,
                defaultResult: MessageBoxResult.OK,
                options: options,
                autoCloseTimeSpan: autoCloseTimeSpan
            );
        }

        #endregion

        #region Show

        /// <summary>
        /// Show the message box with the specified parameters
        /// </summary>
        /// <param name="message">The message text</param>
        /// <param name="details">The details part text</param>
        /// <param name="button">The buttons to be displayed</param>
        /// <param name="icon">The message's severity</param>
        /// <param name="defaultResult">The default button</param>
        /// <param name="options">Misc options</param>
        /// <returns>The user's selected button</returns>
        public static MessageBoxResult Show(string message, string details = "",
                                            MessageBoxButton button = MessageBoxButton.OK,
                                            MessageBoxImage icon = MessageBoxImage.None,
                                            MessageBoxResult defaultResult = MessageBoxResult.None,
                                            MessageBoxOptions options = MessageBoxOptions.None,
                                            TimeSpan? autoCloseTimeSpan = null)
        {
            return Show
            (
                owner: null,
                message: message,
                details: details,
                button: button,
                icon: icon,
                defaultResult: defaultResult,
                options: options,
                autoCloseTimeSpan: autoCloseTimeSpan
            );
        }

        /// <summary>
        /// Show the message box with the specified parameters
        /// </summary>
        /// <param name="message">The message text</param>
        /// <param name="button">The buttons to be displayed</param>
        /// <param name="icon">The message's severity</param>
        /// <param name="defaultResult">The default button</param>
        /// <param name="options">Misc options</param>
        /// <returns>The user's selected button</returns>
        public static MessageBoxResult Show(string message,
                                            MessageBoxButton button = MessageBoxButton.OK,
                                            MessageBoxImage icon = MessageBoxImage.None,
                                            MessageBoxResult defaultResult = MessageBoxResult.None,
                                            MessageBoxOptions options = MessageBoxOptions.None,
                                            TimeSpan? autoCloseTimeSpan = null)
        {
            return Show
            (
                message: message,
                details: string.Empty,
                button: button,
                icon: icon,
                defaultResult: defaultResult,
                options: options,
                autoCloseTimeSpan: autoCloseTimeSpan
            );
        }

        /// <summary>
        /// Show the message box with the specified parameters
        /// </summary>
        /// <param name="owner">The message box's parent window</param>
        /// <param name="message">The message text</param>
        /// <param name="button">The buttons to be displayed</param>
        /// <param name="icon">The message's severity</param>
        /// <param name="defaultResult">The default button</param>
        /// <param name="options">Misc options</param>
        /// <returns>The user's selected button</returns>
        public static MessageBoxResult Show(Window owner, string message,
                                            MessageBoxButton button = MessageBoxButton.OK,
                                            MessageBoxImage icon = MessageBoxImage.None,
                                            MessageBoxResult defaultResult = MessageBoxResult.None,
                                            MessageBoxOptions options = MessageBoxOptions.None,
                                            TimeSpan? autoCloseTimeSpan = null)
        {
            return Show
            (
                owner: owner,
                message: message,
                details: string.Empty,
                button: button,
                icon: icon,
                defaultResult: defaultResult,
                options: options,
                autoCloseTimeSpan: autoCloseTimeSpan
            );
        }

        /// <summary>
        /// Show the message box with the specified parameters
        /// </summary>
        /// <param name="owner">The message box's parent window</param>
        /// <param name="message">The message text</param>
        /// <param name="details">The details part text</param>
        /// <param name="button">The buttons to be displayed</param>
        /// <param name="icon">The message's severity</param>
        /// <param name="defaultResult">The default button</param>
        /// <param name="options">Misc options</param>
        /// <returns>The user's selected button</returns>
        public static MessageBoxResult Show(Window owner, string message, string details = "",
                                            MessageBoxButton button = MessageBoxButton.OK,
                                            MessageBoxImage icon = MessageBoxImage.None,
                                            MessageBoxResult defaultResult = MessageBoxResult.None,
                                            MessageBoxOptions options = MessageBoxOptions.None,
                                            TimeSpan? autoCloseTimeSpan = null)
        {
            MessageBox messageBox = new MessageBox
            (
                owner: owner,
                message: message,
                details: details,
                button: button,
                icon: icon,
                defaultResult: defaultResult,
                options: options,
                autoCloseTimeSpan: autoCloseTimeSpan
            );

            messageBox.Show();

            return messageBox.MessageBoxResult;
        }

        #endregion

        #region ShowDialog

        /// <summary>
        /// Show the message box with the specified parameters
        /// </summary>
        /// <param name="message">The message text</param>
        /// <param name="details">The details part text</param>
        /// <param name="button">The buttons to be displayed</param>
        /// <param name="icon">The message's severity</param>
        /// <param name="defaultResult">The default button</param>
        /// <param name="options">Misc options</param>
        /// <returns>The user's selected button</returns>
        public static MessageBoxResult ShowDialog(string message, string details = "",
                                            MessageBoxButton button = MessageBoxButton.OK,
                                            MessageBoxImage icon = MessageBoxImage.None,
                                            MessageBoxResult defaultResult = MessageBoxResult.None,
                                            MessageBoxOptions options = MessageBoxOptions.None,
                                            TimeSpan? autoCloseTimeSpan = null)
        {
            return ShowDialog
            (
                owner: null,
                message: message,
                details: details,
                button: button,
                icon: icon,
                defaultResult: defaultResult,
                options: options,
                autoCloseTimeSpan: autoCloseTimeSpan
            );
        }

        /// <summary>
        /// Show the message box with the specified parameters
        /// </summary>
        /// <param name="message">The message text</param>
        /// <param name="button">The buttons to be displayed</param>
        /// <param name="icon">The message's severity</param>
        /// <param name="defaultResult">The default button</param>
        /// <param name="options">Misc options</param>
        /// <returns>The user's selected button</returns>
        public static MessageBoxResult ShowDialog(string message,
                                            MessageBoxButton button = MessageBoxButton.OK,
                                            MessageBoxImage icon = MessageBoxImage.None,
                                            MessageBoxResult defaultResult = MessageBoxResult.None,
                                            MessageBoxOptions options = MessageBoxOptions.None,
                                            TimeSpan? autoCloseTimeSpan = null)
        {
            return ShowDialog
            (
                message: message,
                details: string.Empty,
                button: button,
                icon: icon,
                defaultResult: defaultResult,
                options: options,
                autoCloseTimeSpan: autoCloseTimeSpan
            );
        }

        /// <summary>
        /// Show the message box with the specified parameters
        /// </summary>
        /// <param name="owner">The message box's parent window</param>
        /// <param name="message">The message text</param>
        /// <param name="button">The buttons to be displayed</param>
        /// <param name="icon">The message's severity</param>
        /// <param name="defaultResult">The default button</param>
        /// <param name="options">Misc options</param>
        /// <returns>The user's selected button</returns>
        public static MessageBoxResult ShowDialog(Window owner, string message,
                                            MessageBoxButton button = MessageBoxButton.OK,
                                            MessageBoxImage icon = MessageBoxImage.None,
                                            MessageBoxResult defaultResult = MessageBoxResult.None,
                                            MessageBoxOptions options = MessageBoxOptions.None,
                                            TimeSpan? autoCloseTimeSpan = null)
        {
            return ShowDialog
            (
                owner: owner,
                message: message,
                details: string.Empty,
                button: button,
                icon: icon,
                defaultResult: defaultResult,
                options: options,
                autoCloseTimeSpan: autoCloseTimeSpan
            );
        }

        /// <summary>
        /// Show the message box with the specified parameters
        /// </summary>
        /// <param name="owner">The message box's parent window</param>
        /// <param name="message">The message text</param>
        /// <param name="details">The details part text</param>
        /// <param name="button">The buttons to be displayed</param>
        /// <param name="icon">The message's severity</param>
        /// <param name="defaultResult">The default button</param>
        /// <param name="options">Misc options</param>
        /// <returns>The user's selected button</returns>
        public static MessageBoxResult ShowDialog(Window owner, string message, string details = "",
                                            MessageBoxButton button = MessageBoxButton.OK,
                                            MessageBoxImage icon = MessageBoxImage.None,
                                            MessageBoxResult defaultResult = MessageBoxResult.None,
                                            MessageBoxOptions options = MessageBoxOptions.None,
                                            TimeSpan? autoCloseTimeSpan = null)
        {
            MessageBox messageBox = new MessageBox
            (
                owner: owner,
                message: message,
                details: details,
                button: button,
                icon: icon,
                defaultResult: defaultResult,
                options: options,
                autoCloseTimeSpan: autoCloseTimeSpan
            );

            messageBox.ShowDialog();

            return messageBox.MessageBoxResult;
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler temp = this.PropertyChanged;
            if (temp != null)
            {
                temp(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region [弃用]获取屏幕分辨率

        //[Obsolete(message: "不依赖 WinForm 的 System.Windows.Forms.dll")]
        //public static System.Drawing.Rectangle PrimaryScreenWorkingArea()
        //{
        //    return System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;
        //}

        #endregion

        #region 定时自动关闭（ 常用于看板捕获异常 ）

        public readonly TimeSpan mInterval = TimeSpan.FromMilliseconds(500d);

        /// <summary>
        /// 自动关闭总 Ticks
        /// </summary>
        long mPlanTicks { get; set; }

        /// <summary>
        /// 当前自动关闭累计 Ticks
        /// </summary>
        long mSumTicks { get; set; }

        System.Windows.Threading.DispatcherTimer mDispatcherTimer { get; set; }

        void timer_Tick(object sender, EventArgs e)
        {
            mSumTicks = mSumTicks + mInterval.Ticks;

            if (mPlanTicks <= mSumTicks)
            {
                closeDispatcherTimer();

                // 弹出提示窗口后，用户有可能点击到 Owner 的某些位置，此时会导致本窗口处于失去焦点的状态，
                // 又由于在失去焦点状态下将本窗口 Close 掉，会导致 Owner 也失去焦点，导致最小化
                // 故在自动关闭窗口前确保 IsFocused 为 true
                if (this.IsFocused == false) { this.Focus(); }
                this.Close();
            }
            else
            {
                this.Owner.Dispatcher.Invoke(new Action(() =>
                {
                    lblAutoClose.Text = ToStringAdvSimple(new TimeSpan(mPlanTicks - mSumTicks));
                }));
            }
        }

        void closeDispatcherTimer()
        {
            if (mDispatcherTimer != null && mDispatcherTimer.IsEnabled == true)
            {
                mDispatcherTimer.Stop();
                mDispatcherTimer.Tick -= timer_Tick;
                mDispatcherTimer = null;
            }
        }

        /// <summary>
        /// TimeSpan转换文字信息 （ 不含毫秒信息 ）
        /// 拷贝自 Util.HowesDOMO\CommonExtensions\TimeSpanExtension.cs
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static string ToStringAdvSimple(TimeSpan ts)
        {
            string r = string.Empty;

            if (ts.Days > 0)
            {
                r += $"{ts.Days} 天";
            }

            if (ts.Hours > 0)
            {
                r += $"{ts.Hours} 小时";
            }

            if (ts.Minutes > 0)
            {
                r += $"{ts.Minutes} 分";
            }

            if (ts.Seconds > 0)
            {
                r += $"{ts.Seconds} 秒";
            }

            if (string.IsNullOrEmpty(r))
            {
                r = "0 秒";
            }

            //if (ts.Milliseconds > 0)
            //{
            //    r += $"{ts.Milliseconds}毫秒";
            //}

            return r;
        }

        #endregion

        #region Exception GetInfo

        /// <summary>
        /// 判断是否为 BusinessException
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        static bool IsBusinessException(Exception e)
        {
            string msg = e.Message;

            int indexOf_cStart = msg.IndexOf(cStart);
            int indexOf_cEnd = msg.IndexOf(cEnd);

            bool r = indexOf_cStart >= 0 && indexOf_cEnd > indexOf_cStart;
            return r;
        }

        const string cStart = "\u0002";

        const string cEnd = "\u0003";

        static void GetExceptionFullInfo(Exception ex, StringBuilder sb, int level = 1)
        {
            if (string.IsNullOrEmpty(sb.ToString()) == false)
            {
                sb.AppendLine("************** Inner Exception " + level + "**************");
            }

            sb.AppendLine(ex.Message + "\r\n" + ex.StackTrace);

            if (ex.InnerException != null)
            {
                level = level + 1;
                GetExceptionFullInfo(ex.InnerException, sb, level);
            }
        }

        static string ExceptionGetInfo(Exception e)
        {
            if (IsBusinessException(e) == true)
            {
                //string msg = e.Message;

                //int cStartIndex = msg.IndexOf(cStart) + 1;
                //int cEndIndex = msg.IndexOf(cEnd);

                //return msg.Substring(cStartIndex, cEndIndex - cStartIndex);

                return string.Empty; // 业务逻辑异常无需显示 Detail
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                GetExceptionFullInfo(e, sb);
                return sb.ToString();
            }
        }

        #endregion

        Window getCurrentWindow()
        {
            Window r = Application.Current.Windows
                                     .OfType<Window>()
                                     .Where(i => i.IsActive == true)
                                     .FirstOrDefault();

            if (r == null)
            {
                r = Application.Current.MainWindow;
            }

            return r;
        }

        #region [DP] ExtraContent - 自定义界面，可以传入 ContentControl 来实现一些简单的输入确认界面，例如 账号密码， 打印数量

        public static readonly DependencyProperty ExtraContentProperty = DependencyProperty.Register
        (
            name: "ExtraContent",
            propertyType: typeof(object),
            ownerType: typeof(MessageBox),
            validateValueCallback: new ValidateValueCallback((toValidate) =>
            {
                if (toValidate is null)
                {
                    return true;
                }
                else
                {
                    if (toValidate is ContentControl cc && cc.DataContext is IDataErrorInfo dataContext)
                        return true;
                    else
                        return false;
                }
            }),
            typeMetadata: new FrameworkPropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public object ExtraContent
        {
            get { return (object)GetValue(ExtraContentProperty); }
            set { SetValue(ExtraContentProperty, value); }
        }

        #endregion

        /// <summary>
        /// 获取窗体（ 用于传入 ExtraContent ）
        /// </summary>
        /// <param name="message"></param>
        /// <param name="owner"></param>
        /// <param name="options"></param>
        /// <param name="autoCloseTimeSpan"></param>
        /// <returns></returns>
        public static MessageBox GetMessageBox4UserDefineCc
        (
            string message,
            string details = "",
            Window owner = null,
            // bool showCancel = false,
            MessageBoxResult defaultResult = MessageBoxResult.OK,
            MessageBoxOptions options = MessageBoxOptions.None,
            TimeSpan? autoCloseTimeSpan = null
        )
        {
            MessageBox messageBox = new MessageBox
            (
                owner: owner,
                message: message,
                details: details, //string.Empty,
                // button: showCancel? MessageBoxButton.OKCancel: MessageBoxButton.OKCancel,
                button: MessageBoxButton.OKCancel,
                icon: MessageBoxImage.Information,
                defaultResult: defaultResult,
                options: options,
                autoCloseTimeSpan: autoCloseTimeSpan
            );

            return messageBox;
        }

    }
}