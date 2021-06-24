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
    /// <summary>
    /// V 1.0.2 - 2021-06-23 12:16:15
    /// 为了区分 Show 与 ShowDialog，新增 ShowDialog方法，修改原来的 Show 方法 messageBox.ShowDialog(); ==> messageBox.Show();
    /// 
    /// V 1.0.1 - 2021-03-21 17:55:22
    /// 弃用System.Windows.Forms.Screen.PrimaryScreen.WorkingArea的方式获取屏幕分辨率,
    /// 改用ScreenUtils( 从 github 上获取的开源项目, 已嵌入到 WPFControls )
    /// 
    /// V 1.0.0 - 2021-03-21 14:36:58
    /// 改写 项目
    /// 整理代码, 并对以下几点作了优化
    /// 1. 当含有详情信息时, 可以使用 GridSplitter 上下拖动来改变 主信息与详情信息的显示空间大小
    /// 2. UserDefineFontSizeDict ( 用户自定义 FontSize 字典 ), 用来自定义某些窗体下字体的自定义大小
    /// 3. 采用读取当前计算机的 WorkArea 的长宽像数, 来设置窗体内各个地方对应的最大宽度或最大高度
    /// 4. UI优化 ( 重改结构, 优化按钮信息(快捷键按钮加下划线) 等 )
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
                          MessageBoxResult defaultResult, MessageBoxOptions options)
        {
            InitializeComponent();

            try
            {
                this.Owner = owner ?? Application.Current.MainWindow;
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
            TimeSpan? autoClose = null; // TODO 作为参数
            autoClose = TimeSpan.FromSeconds(4d); // 去掉
            if (autoClose.HasValue)
            {
                mPlanTicks = autoClose.Value.Ticks;

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

        public MessageBoxResult MessageBoxResult { get; set; }

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

        private Button okButton;
        /// <summary>
        /// Create the ok button on demand
        /// </summary>
        /// <param name="defaultResult"></param>
        /// <returns></returns>
        private Button CreateOkButton(MessageBoxResult defaultResult)
        {
            this.okButton = new Button
            {
                Name = "okButton",
                Content = "确定", //"OK",
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
            Button cancelButton = new Button
            {
                Name = "cancelButton",
                Content = "取消", //"Cancel",
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
            var content = new TextBlock();
            content.Inlines.Add("是(");
            content.Inlines.Add(new Run("Y") { TextDecorations = TextDecorations.Underline });
            content.Inlines.Add(")");

            this.yesButton = new Button
            {
                Name = "yesButton",
                Content = content,
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
            var content = new TextBlock();
            content.Inlines.Add("否(");
            content.Inlines.Add(new Run("N") { TextDecorations = TextDecorations.Underline });
            content.Inlines.Add(")");

            this.noButton = new Button
            {
                Name = "noButton",
                Content = content,
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
            this.MessageBoxResult = (MessageBoxResult)(sender as Button).Tag;

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

        // Tips By Howe Confirm 与 Question 的区别
        // Confirm ==> Ok Cancel
        // Question => Yes No Cancel

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
                                                       MessageBoxOptions options = MessageBoxOptions.None)
        {
            return MessageBox.ShowInformation
            (
                owner: null,
                message: message,
                details: details,
                showCancel: showCancel,
                options: options
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
                                                       MessageBoxOptions options = MessageBoxOptions.None)
        {
            return MessageBox.Show
            (
                owner: owner,
                message: message,
                details: details,
                button: showCancel ? MessageBoxButton.OKCancel : MessageBoxButton.OK,
                icon: MessageBoxImage.Information,
                defaultResult: MessageBoxResult.OK,
                options: options
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
                                                       MessageBoxOptions options = MessageBoxOptions.None)
        {
            return MessageBox.ShowInformationDialog
            (
                owner: null,
                message: message,
                details: details,
                showCancel: showCancel,
                options: options
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
                                                       MessageBoxOptions options = MessageBoxOptions.None)
        {
            return MessageBox.ShowDialog
            (
                owner: owner,
                message: message,
                details: details,
                button: showCancel ? MessageBoxButton.OKCancel : MessageBoxButton.OK,
                icon: MessageBoxImage.Information,
                defaultResult: MessageBoxResult.OK,
                options: options
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
                                                    MessageBoxOptions options = MessageBoxOptions.None)
        {
            return MessageBox.ShowConfirm
            (
                owner: null,
                message: message,
                details: details,
                showCancel: showCancel,
                options: options
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
                                                    MessageBoxOptions options = MessageBoxOptions.None)
        {
            return MessageBox.Show
            (
                owner: owner,
                message: message,
                details: details,
                button: showCancel ? MessageBoxButton.OKCancel : MessageBoxButton.OKCancel,
                icon: MessageBoxImage.Question,
                defaultResult: MessageBoxResult.Yes,
                options: options
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
                                                    MessageBoxOptions options = MessageBoxOptions.None)
        {
            return MessageBox.ShowConfirmDialog
            (
                owner: null,
                message: message,
                details: details,
                showCancel: showCancel,
                options: options
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
                                                    MessageBoxOptions options = MessageBoxOptions.None)
        {
            return MessageBox.ShowDialog
            (
                owner: owner,
                message: message,
                details: details,
                button: showCancel ? MessageBoxButton.OKCancel : MessageBoxButton.OKCancel,
                icon: MessageBoxImage.Question,
                defaultResult: MessageBoxResult.Yes,
                options: options
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
                                                    MessageBoxOptions options = MessageBoxOptions.None)
        {
            return MessageBox.ShowQuestion
            (
                owner: null,
                message: message,
                details: details,
                showCancel: showCancel,
                options: options
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
                                                    MessageBoxOptions options = MessageBoxOptions.None)
        {
            return MessageBox.Show
            (
                owner: owner,
                message: message,
                details: details,
                button: showCancel ? MessageBoxButton.YesNoCancel : MessageBoxButton.YesNo,
                icon: MessageBoxImage.Question,
                defaultResult: MessageBoxResult.Yes,
                options: options
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
                                                    MessageBoxOptions options = MessageBoxOptions.None)
        {
            return MessageBox.ShowQuestionDialog
            (
                owner: null,
                message: message,
                details: details,
                showCancel: showCancel,
                options: options
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
                                                    MessageBoxOptions options = MessageBoxOptions.None)
        {
            return MessageBox.ShowDialog
            (
                owner: owner,
                message: message,
                details: details,
                button: showCancel ? MessageBoxButton.YesNoCancel : MessageBoxButton.YesNo,
                icon: MessageBoxImage.Question,
                defaultResult: MessageBoxResult.Yes,
                options: options
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
                                                   MessageBoxOptions options = MessageBoxOptions.None)
        {
            return MessageBox.ShowWarning
            (
                owner: null,
                message: message,
                details: details,
                showCancel: showCancel,
                options: options
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
                                                   MessageBoxOptions options = MessageBoxOptions.None)
        {
            return MessageBox.Show
            (
                owner: owner,
                message: message,
                details: details,
                button: showCancel ? MessageBoxButton.OKCancel : MessageBoxButton.OK,
                icon: MessageBoxImage.Warning,
                defaultResult: MessageBoxResult.OK,
                options: options
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
                                                   MessageBoxOptions options = MessageBoxOptions.None)
        {
            return MessageBox.ShowWarningDialog
            (
                owner: null,
                message: message,
                details: details,
                showCancel: showCancel,
                options: options
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
                                                   MessageBoxOptions options = MessageBoxOptions.None)
        {
            return MessageBox.ShowDialog
            (
                owner: owner,
                message: message,
                details: details,
                button: showCancel ? MessageBoxButton.OKCancel : MessageBoxButton.OK,
                icon: MessageBoxImage.Warning,
                defaultResult: MessageBoxResult.OK,
                options: options
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
                                                 MessageBoxOptions options = MessageBoxOptions.None)
        {
            return MessageBox.ShowError
            (
                owner: null,
                exception: exception,
                message: message,
                options: options
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
                                                 MessageBoxOptions options = MessageBoxOptions.None)
        {
            return MessageBox.ShowError
            (
                owner: null,
                message: message,
                details: details,
                showCancel: showCancel,
                options: options
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
                                                 MessageBoxOptions options = MessageBoxOptions.None)
        {
            string details = string.Empty;

            if (exception != null)
            {
                // TODO 完善 Details 信息
                details = exception.ToString();
            }

            return MessageBox.Show
            (
                owner: owner,
                message: String.IsNullOrEmpty(message) ? exception.Message : message,
                details: details,
                button: MessageBoxButton.OK,
                icon: MessageBoxImage.Error,
                defaultResult: MessageBoxResult.OK,
                options: options
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
                                                 MessageBoxOptions options = MessageBoxOptions.None)
        {
            return MessageBox.Show
            (
                owner: owner,
                message: message,
                details: details,
                button: showCancel ? MessageBoxButton.OKCancel : MessageBoxButton.OK,
                icon: MessageBoxImage.Error,
                defaultResult: MessageBoxResult.OK,
                options: options
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
                                                 MessageBoxOptions options = MessageBoxOptions.None)
        {
            return MessageBox.ShowErrorDialog
            (
                owner: null,
                exception: exception,
                message: message,
                options: options
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
                                                 MessageBoxOptions options = MessageBoxOptions.None)
        {
            return MessageBox.ShowErrorDialog
            (
                owner: null,
                message: message,
                details: details,
                showCancel: showCancel,
                options: options
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
                                                 MessageBoxOptions options = MessageBoxOptions.None)
        {
            string details = string.Empty;

            if (exception != null)
            {
                // TODO 完善 Details 信息
                details = exception.ToString();
            }

            return MessageBox.ShowDialog
            (
                owner: owner,
                message: String.IsNullOrEmpty(message) ? exception.Message : message,
                details: details,
                button: MessageBoxButton.OK,
                icon: MessageBoxImage.Error,
                defaultResult: MessageBoxResult.OK,
                options: options
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
                                                 MessageBoxOptions options = MessageBoxOptions.None)
        {
            return MessageBox.ShowDialog
            (
                owner: owner,
                message: message,
                details: details,
                button: showCancel ? MessageBoxButton.OKCancel : MessageBoxButton.OK,
                icon: MessageBoxImage.Error,
                defaultResult: MessageBoxResult.OK,
                options: options
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
                                            MessageBoxOptions options = MessageBoxOptions.None)
        {
            return MessageBox.Show
            (
                owner: null,
                message: message,
                details: details,
                button: button,
                icon: icon,
                defaultResult: defaultResult,
                options: options
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
                                            MessageBoxOptions options = MessageBoxOptions.None)
        {
            return MessageBox.Show
            (
                message: message,
                details: string.Empty,
                button: button,
                icon: icon,
                defaultResult: defaultResult,
                options: options
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
                                            MessageBoxOptions options = MessageBoxOptions.None)
        {
            return MessageBox.Show
            (
                owner: owner,
                message: message,
                details: string.Empty,
                button: button,
                icon: icon,
                defaultResult: defaultResult,
                options: options
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
                                            MessageBoxOptions options = MessageBoxOptions.None)
        {
            MessageBox messageBox = new MessageBox
            (
                owner: owner,
                message: message,
                details: details,
                button: button,
                icon: icon,
                defaultResult: defaultResult,
                options: options
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
                                            MessageBoxOptions options = MessageBoxOptions.None)
        {
            return MessageBox.ShowDialog
            (
                owner: null,
                message: message,
                details: details,
                button: button,
                icon: icon,
                defaultResult: defaultResult,
                options: options
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
                                            MessageBoxOptions options = MessageBoxOptions.None)
        {
            return MessageBox.ShowDialog
            (
                message: message,
                details: string.Empty,
                button: button,
                icon: icon,
                defaultResult: defaultResult,
                options: options
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
                                            MessageBoxOptions options = MessageBoxOptions.None)
        {
            return MessageBox.ShowDialog
            (
                owner: owner,
                message: message,
                details: string.Empty,
                button: button,
                icon: icon,
                defaultResult: defaultResult,
                options: options
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
                                            MessageBoxOptions options = MessageBoxOptions.None)
        {
            MessageBox messageBox = new MessageBox
            (
                owner: owner,
                message: message,
                details: details,
                button: button,
                icon: icon,
                defaultResult: defaultResult,
                options: options
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

        #region 定时自动关闭（ 常用于看板 ）

        public readonly TimeSpan mInterval = TimeSpan.FromMilliseconds(500d);

        long mPlanTicks { get; set; }

        long mSumTicks { get; set; }

        System.Windows.Threading.DispatcherTimer mDispatcherTimer { get; set; }

        void timer_Tick(object sender, EventArgs e)
        {
            mSumTicks = mSumTicks + mInterval.Ticks;

            if (mPlanTicks <= mSumTicks)
            {
                closeDispatcherTimer();
                this.Close();
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

        #endregion
    }
}