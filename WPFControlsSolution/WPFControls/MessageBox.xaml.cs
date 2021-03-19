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
    /// Interaction logic for MessageBox.xaml
    /// </summary>
    public partial class MessageBox : INotifyPropertyChanged
    {
        // TODO
        [Obsolete(message: "应该在 appStartup 中向MessageBox中填入列外列表")]
        System.Collections.Generic.List<string> mSpecialList { get; set; } = new System.Collections.Generic.List<string>()
        {
            // 小包称重
            "Client.View.FrmPartWeightV2",
            "Client.View.FrmPartWeightEditData",
            "Client.View.FrmPartPlanScanWeightReset",
            "Client.View.FrmPartPlanWeightReset",
            "Client.View.FrmSelectCartonPartNo",

            // 整箱称重
            "Client.View.FrmCartonWeight",
            "Client.View.FrmCartonWeightEditData",
            "Client.View.FrmCartonWeightEditDataV2",

            // 整箱拆箱
            "Client.View.FrmDeleteCarton",

            // Other
            "Client.View.FrmInputOrderNo",
            "Client.View.FrmInputAccountPassword",
        };


        private bool _animationRan;

        public MessageBox(Window owner, string message, string details, MessageBoxButton button, MessageBoxImage icon,
                          MessageBoxResult defaultResult, MessageBoxOptions options)
        {
            this._animationRan = false;

            InitializeComponent();

            try
            {
                // this.Owner = owner ?? Client.Common.StaticInfo.FrmMain;
                this.Owner = owner ?? Application.Current.MainWindow;
            }
            catch (Exception)
            {
                this.Owner = Application.Current.MainWindow; // Client.Common.StaticInfo.FrmMain;
            }

            double defaultFontSize = this.FontSize;
            //this.FontSize = this.Owner.FontSize;


            if (mSpecialList.Contains(this.Owner.ToString()))
            {
                this.FontSize = 40;
            }
            else
            {
                this.MessageText.FontSize = this.FontSize + (this.MessageText.FontSize - defaultFontSize);
            }

            this.MessageText.FontSize = this.FontSize + (this.MessageText.FontSize - defaultFontSize);

            if (this.FontSize - defaultFontSize > 1)
            {
                this.MaxWidth += (this.FontSize - defaultFontSize) * 10;
            }

            this.CreateButtons(button, defaultResult);

            this.CreateImage(icon);

            this.MessageText.Text = message;
            this.DetailsExpander.Visibility = string.IsNullOrEmpty(details) ? Visibility.Collapsed : Visibility.Visible;

            this.DetailsText.Text = details;

            this.ApplyOptions(options);
            if (button == MessageBoxButton.YesNo || button == MessageBoxButton.YesNoCancel)
            {
                this.KeyDown += new KeyEventHandler(MessageBox_KeyDown);
            }

            this.Loaded += new RoutedEventHandler(MessageBox_Loaded);
        }

        void MessageBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.yesButton != null)
            {
                this.yesButton.Focus();
            }
            if (this.okButton != null)
            {
                this.okButton.Focus();
            }
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

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

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
            this.yesButton = new Button
            {
                Name = "yesButton",
                Content = "是(Y)",//"Yes",
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
            this.noButton = new Button
            {
                Name = "noButton",
                Content = "否(N)",//"No",
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

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler temp = this.PropertyChanged;
            if (temp != null)
            {
                temp(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Enable dragging
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        /// <summary>
        /// Show the startup animation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            #region  Add By howe

            // DetailsExpander.IsExpanded = false;

            var rect = PrimaryScreenWorkingArea();

            double maxWidth = (double)rect.Width * 0.6d;
            this.MaxWidth = maxWidth + 10;
            gridMain.MaxWidth = maxWidth;
            gridSplitter0.MaxWidth = maxWidth;
            gridSplitter1.MaxWidth = maxWidth;
            gridDetail.MaxWidth = maxWidth;

            int marginHeight = 5;
            int gridSplitterHeight = 3;
            
            MessageBoxWindow.MaxHeight = rect.Height - (marginHeight * 2);

            int gridMainMiniHeight = 60;
            int totalHeight = rect.Height - (marginHeight * 2) - gridSplitterHeight;
            gridMain.MinHeight = gridMainMiniHeight;
            gridMain.MaxHeight = totalHeight;
            

            gridDetail.MinHeight = 40;
            gridDetail.MaxHeight = totalHeight - gridSplitterHeight - gridMainMiniHeight;

            #endregion

            // This is set here to height after the width has been set 
            // so the details expander won't stretch the message box when it's opened
            this.SizeToContent = SizeToContent.Height;

            var animation = TryFindResource("LoadAnimation") as Storyboard;

            animation.Begin(this);
        }

        /// <summary>
        /// Show the closing animation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MessageBoxWindow_Closing(object sender, CancelEventArgs e)
        {
            if (!this._animationRan)
            {
                // The animation won't run if the window is allowed to close, 
                // so here the animation starts, and the window's closing is canceled
                e.Cancel = true;

                var animation = TryFindResource("UnloadAnimation") as Storyboard;

                animation.Completed += AnimationCompleted;

                animation.Begin(this);
            }
        }

        /// <summary>
        /// Signals the closing animation ran, and close the window (for real this time)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AnimationCompleted(object sender, EventArgs e)
        {
            this._animationRan = true;

            this.Close();
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
                                                       MessageBoxOptions options = MessageBoxOptions.None)
        {
            return MessageBox.ShowInformation(null, message, details, showCancel, options);
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
            return MessageBox.Show(owner, message, details, showCancel ? MessageBoxButton.OKCancel : MessageBoxButton.OK,
                        MessageBoxImage.Information, MessageBoxResult.OK, options);
        }

        #endregion

        #region Show OKCancel

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
            return MessageBox.ShowConfirm(null, message, details, showCancel, options);
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
            return MessageBox.Show(owner, message, details, showCancel ? MessageBoxButton.OKCancel : MessageBoxButton.OKCancel,
                        MessageBoxImage.Question, MessageBoxResult.Yes, options);
        }

        #endregion

        // Tips By Howe Confirm 与 Question 的区别
        // Confirm ==> Ok Cancel
        // Question => Yes No Cancel

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
            return MessageBox.ShowQuestion(null, message, details, showCancel, options);
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
            return MessageBox.Show(owner, message, details, showCancel ? MessageBoxButton.YesNoCancel : MessageBoxButton.YesNo,
                        MessageBoxImage.Question, MessageBoxResult.Yes, options);
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
            return MessageBox.ShowWarning(null, message, details, showCancel, options);
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
            return MessageBox.Show(owner, message, details, showCancel ? MessageBoxButton.OKCancel : MessageBoxButton.OK,
                        MessageBoxImage.Warning, MessageBoxResult.OK, options);
        }

        #endregion


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
            return MessageBox.ShowError(null, exception, message, options);
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
            return MessageBox.ShowError(null, message, details, showCancel, options);
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

#if DEBUG
            details = exception.ToString();
#endif

            return MessageBox.Show(owner, String.IsNullOrEmpty(message) ? exception.Message : message, details, MessageBoxButton.OK,
                        MessageBoxImage.Error, MessageBoxResult.OK, options);
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
            return MessageBox.Show(owner, message, details, showCancel ? MessageBoxButton.OKCancel : MessageBoxButton.OK,
                        MessageBoxImage.Error, MessageBoxResult.OK, options);
        }

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
            return MessageBox.Show(null, message, details, button, icon, defaultResult, options);
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
            return MessageBox.Show(message, string.Empty, button, icon, defaultResult, options);
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
            return MessageBox.Show(owner, message, string.Empty, button, icon, defaultResult, options);
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
            MessageBox messageBox = new MessageBox(owner, message, details, button, icon, defaultResult, options);

            messageBox.ShowDialog();

            return messageBox.MessageBoxResult;
        }

        #endregion


        //public static System.Windows.Rect getDisplayResolution()
        //{
        //    System.Windows.SystemParameters.WorkArea
        //    return System.Windows.SystemParameters.WorkArea;
        //}

        public static System.Drawing.Rectangle PrimaryScreenWorkingArea()
        {
            return System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;
        }
    }
}