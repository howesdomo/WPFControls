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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client.Components
{
    /// <summary>
    /// Interaction logic for MarqueeLabel.xaml
    /// </summary>
    public class MarqueeLabel : UserControl
    {
        Canvas mCanvas { get; set; }

        TextBlock mLabel { get; set; }

        #region Text

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register
        (
            name: "Text",
            propertyType: typeof(string),
            ownerType: typeof(MarqueeLabel),
            typeMetadata: new FrameworkPropertyMetadata
            (
                propertyChangedCallback: new PropertyChangedCallback(textPropertyChanged)
            ),
            validateValueCallback: null
        );

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        private static void textPropertyChanged(DependencyObject bindable, DependencyPropertyChangedEventArgs eventArgs)
        {
            object newValue = eventArgs.NewValue;
            object oldValue = eventArgs.OldValue;

            string value = newValue as string;
            if (oldValue as string == value)
            {
                return;
            }

            var target = bindable as MarqueeLabel;
            target.mLabel.Text = value;
            target.beginAnimation();
        }

        #endregion

        #region [弃用]FontSize

        //public new static readonly DependencyProperty FontSizeProperty = DependencyProperty.Register
        //(
        //    name: "FontSize",
        //    propertyType: typeof(double),
        //    ownerType: typeof(MarqueeLabel),
        //    typeMetadata: new FrameworkPropertyMetadata
        //    (
        //        defaultValue: 30d,
        //        propertyChangedCallback: new PropertyChangedCallback(fontSizePropertyChanged)
        //    ),
        //    validateValueCallback: new ValidateValueCallback(fontSize_IsValidValue)
        //);

        //public new double FontSize
        //{
        //    get { return (double)GetValue(FontSizeProperty); }
        //    set { SetValue(FontSizeProperty, value); }
        //}

        //private static bool fontSize_IsValidValue(object value)
        //{
        //    double v;

        //    if (double.TryParse(value.ToString(), out v) == false)
        //    {
        //        return false;
        //    }

        //    return v > 0 ? true : false;
        //}

        //private static void fontSizePropertyChanged(DependencyObject bindable, DependencyPropertyChangedEventArgs eventArgs)
        //{
        //    object newValue = eventArgs.NewValue;
        //    object oldValue = eventArgs.OldValue;

        //    double value = (double)newValue;
        //    if ((double)oldValue == value)
        //    {
        //        return;
        //    }

        //    var target = bindable as MarqueeLabel;
        //    target.mLabel.FontSize = value;

        //    string msg = $"fontSize change to :{value}";
        //    System.Diagnostics.Debug.WriteLine(msg);

        //    target.beginAnimation();
        //}

        #endregion

        #region Padding

        public new static readonly DependencyProperty PaddingProperty = DependencyProperty.Register
        (
            name: "Padding",
            propertyType: typeof(Thickness),
            ownerType: typeof(MarqueeLabel),
            typeMetadata: new FrameworkPropertyMetadata
            (
                propertyChangedCallback: new PropertyChangedCallback(paddingPropertyChanged)
            ),
            validateValueCallback: new ValidateValueCallback(padding_IsValidValue)
        );

        public new Thickness Padding
        {
            get { return (Thickness)GetValue(PaddingProperty); }
            set { SetValue(PaddingProperty, value); }
        }

        private static bool padding_IsValidValue(object value)
        {
            return value is Thickness;
        }

        private static void paddingPropertyChanged(DependencyObject bindable, DependencyPropertyChangedEventArgs eventArgs)
        {
            object newValue = eventArgs.NewValue;
            object oldValue = eventArgs.OldValue;

            Thickness value = (Thickness)newValue;
            if ((Thickness)oldValue == value)
            {
                return;
            }

            (bindable as MarqueeLabel).mLabel.Padding = value;
        }

        #endregion

        #region TextColor

        public static readonly DependencyProperty TextColorProperty = DependencyProperty.Register
        (
            name: "TextColor",
            propertyType: typeof(Brush),
            ownerType: typeof(MarqueeLabel),
            typeMetadata: new FrameworkPropertyMetadata
            (
                defaultValue: Brushes.Black,
                propertyChangedCallback: new PropertyChangedCallback(textColorPropertyChanged)
            ),
            validateValueCallback: new ValidateValueCallback(textColor_IsValidValue)
        );

        public Brush TextColor
        {
            get { return (Brush)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        private static bool textColor_IsValidValue(object value)
        {
            return value is Brush;
        }

        private static void textColorPropertyChanged(DependencyObject bindable, DependencyPropertyChangedEventArgs eventArgs)
        {
            object newValue = eventArgs.NewValue;
            object oldValue = eventArgs.OldValue;

            Brush value = (Brush)newValue;
            if ((Brush)oldValue == value)
            {
                return;
            }

            (bindable as MarqueeLabel).mLabel.Foreground = value;
        }

        #endregion

        #region [弃用]FontFamily

        //public new static readonly DependencyProperty FontFamilyProperty = DependencyProperty.Register
        //(
        //    name: "FontFamily",
        //    propertyType: typeof(System.Windows.Media.FontFamily),
        //    ownerType: typeof(MarqueeLabel),
        //    typeMetadata: new FrameworkPropertyMetadata
        //    (
        //        defaultValue: new System.Windows.Media.FontFamily(),
        //        propertyChangedCallback: new PropertyChangedCallback(fontFamilyPropertyChanged)
        //    ),
        //    validateValueCallback: new ValidateValueCallback(fontFamily_IsValidValue)
        //);

        //public new System.Windows.Media.FontFamily FontFamily
        //{
        //    get { return (System.Windows.Media.FontFamily)GetValue(FontFamilyProperty); }
        //    set { SetValue(FontFamilyProperty, value); }
        //}

        //private static bool fontFamily_IsValidValue(object value)
        //{
        //    return value is System.Windows.Media.FontFamily;
        //}

        //private static void fontFamilyPropertyChanged(DependencyObject bindable, DependencyPropertyChangedEventArgs eventArgs)
        //{
        //    object newValue = eventArgs.NewValue;
        //    object oldValue = eventArgs.OldValue;

        //    System.Windows.Media.FontFamily value = (System.Windows.Media.FontFamily)newValue;
        //    if ((System.Windows.Media.FontFamily)oldValue == value)
        //    {
        //        return;
        //    }

        //    (bindable as MarqueeLabel).mLabel.FontFamily = value;
        //}

        #endregion

        #region [弃用]FontWeight

        //public new static readonly DependencyProperty FontWeightProperty = DependencyProperty.Register
        //(
        //    name: "FontWeight",
        //    propertyType: typeof(FontWeight),
        //    ownerType: typeof(MarqueeLabel),
        //   typeMetadata: new FrameworkPropertyMetadata
        //    (
        //        propertyChangedCallback: new PropertyChangedCallback(fontWeightPropertyChanged)
        //    ),
        //    validateValueCallback: new ValidateValueCallback(fontWeight_IsValidValue)
        //);

        //public new FontWeight FontWeight
        //{
        //    get { return (FontWeight)GetValue(FontFamilyProperty); }
        //    set { SetValue(FontFamilyProperty, value); }
        //}

        //private static bool fontWeight_IsValidValue(object value)
        //{
        //    return value is FontWeight;
        //}

        //private static void fontWeightPropertyChanged(DependencyObject bindable, DependencyPropertyChangedEventArgs eventArgs)
        //{
        //    object newValue = eventArgs.NewValue;
        //    object oldValue = eventArgs.OldValue;

        //    FontWeight value = (FontWeight)newValue;
        //    if ((FontWeight)oldValue == value)
        //    {
        //        return;
        //    }

        //    (bindable as MarqueeLabel).mLabel.FontWeight = value;
        //}

        #endregion

        #region TextDecorations

        public static readonly DependencyProperty TextDecorationsProperty = DependencyProperty.Register
        (
            name: "TextDecorations",
            propertyType: typeof(TextDecorationCollection),
            ownerType: typeof(MarqueeLabel),
            typeMetadata: new FrameworkPropertyMetadata
            (
                defaultValue: new TextDecorationCollection(0),
                propertyChangedCallback: new PropertyChangedCallback(textDecorationsPropertyChanged)
            ),
            validateValueCallback: new ValidateValueCallback(textDecorations_IsValidValue)
        );

        public TextDecorationCollection TextDecorations
        {
            get { return (TextDecorationCollection)GetValue(TextDecorationsProperty); }
            set { SetValue(TextDecorationsProperty, value); }
        }

        private static bool textDecorations_IsValidValue(object value)
        {
            return value is TextDecorationCollection;
        }

        private static void textDecorationsPropertyChanged(DependencyObject bindable, DependencyPropertyChangedEventArgs eventArgs)
        {
            object newValue = eventArgs.NewValue;
            object oldValue = eventArgs.OldValue;

            TextDecorationCollection value = (TextDecorationCollection)newValue;
            if ((TextDecorationCollection)oldValue == value)
            {
                return;
            }

            (bindable as MarqueeLabel).mLabel.TextDecorations = value;
        }

        #endregion

        #region WordsPerSecond -- 每秒阅读字数

        public static readonly DependencyProperty WordsPerSecondProperty = DependencyProperty.Register
        (
            name: "WordsPerSecond",
            propertyType: typeof(int),
            ownerType: typeof(MarqueeLabel),
            typeMetadata: new FrameworkPropertyMetadata
            (
                defaultValue: 7,
                propertyChangedCallback: new PropertyChangedCallback(wordsPerSecondPropertyChanged)
            ),
            validateValueCallback: new ValidateValueCallback(wordsPerSecond_IsValidValue)
        );

        public int WordsPerSecond
        {
            get { return (int)GetValue(WordsPerSecondProperty); }
            set { SetValue(WordsPerSecondProperty, value); }
        }

        private static bool wordsPerSecond_IsValidValue(object value)
        {
            if (int.TryParse(value.ToString(), out int v) == false)
            {
                return false;
            }
            // throw new Exception("每秒阅读字数不能小于1");
            return v > 1 ? true : false;
        }

        private static void wordsPerSecondPropertyChanged(DependencyObject bindable, DependencyPropertyChangedEventArgs eventArgs)
        {
            object newValue = eventArgs.NewValue;
            object oldValue = eventArgs.OldValue;

            if ((int)oldValue == (int)newValue)
            {
                return;
            }

            (bindable as MarqueeLabel).beginAnimation();
        }

        #endregion

        #region StartBreakSecond -- 开始阅读停顿秒数

        public static readonly DependencyProperty StartBreakSecondProperty = DependencyProperty.Register
        (
            name: "StartBreakSecond",
            propertyType: typeof(double),
            ownerType: typeof(MarqueeLabel),
            typeMetadata: new FrameworkPropertyMetadata
            (
                defaultValue: 1d,
                propertyChangedCallback: new PropertyChangedCallback(startBreakSecondPropertyChanged)
            ),
            validateValueCallback: new ValidateValueCallback(startBreakSecond_IsValidValue)
        );

        public double StartBreakSecond
        {
            get { return (double)GetValue(StartBreakSecondProperty); }
            set { SetValue(StartBreakSecondProperty, value); }
        }

        private static bool startBreakSecond_IsValidValue(object value)
        {
            if (double.TryParse(value.ToString(), out double v) == false)
            {
                return false;
            }
            // throw new Exception("开始阅读停顿秒数不能小于零");
            return v >= 0 ? true : false;
        }

        private static void startBreakSecondPropertyChanged(DependencyObject bindable, DependencyPropertyChangedEventArgs eventArgs)
        {
            object newValue = eventArgs.NewValue;
            object oldValue = eventArgs.OldValue;

            if ((double)oldValue == (double)newValue)
            {
                return;
            }

            (bindable as MarqueeLabel).beginAnimation();
        }

        #endregion

        #region EndBreakSecond -- 完成阅读停顿秒数

        public static readonly DependencyProperty EndBreakSecondProperty = DependencyProperty.Register
        (
            name: "EndBreakSecond",
            propertyType: typeof(double),
            ownerType: typeof(MarqueeLabel),
            typeMetadata: new FrameworkPropertyMetadata
            (
                defaultValue: 1d,
                propertyChangedCallback: new PropertyChangedCallback(endBreakSecondPropertyChanged)
            ),
            validateValueCallback: new ValidateValueCallback(endBreakSecond_IsValidValue)
        );

        public double EndBreakSecond
        {
            get { return (double)GetValue(EndBreakSecondProperty); }
            set { SetValue(EndBreakSecondProperty, value); }
        }

        private static bool endBreakSecond_IsValidValue(object value)
        {
            if (double.TryParse(value.ToString(), out double v) == false)
            {
                return false;
            }
            // throw new Exception("完成阅读停顿秒数不能小于零");
            return v >= 0 ? true : false;
        }

        private static void endBreakSecondPropertyChanged(DependencyObject bindable, DependencyPropertyChangedEventArgs eventArgs)
        {
            object newValue = eventArgs.NewValue;
            object oldValue = eventArgs.OldValue;

            if ((double)oldValue == (double)newValue)
            {
                return;
            }

            (bindable as MarqueeLabel).beginAnimation();
        }

        #endregion

        #region ResetSecond -- 回滚重置持续秒数

        public static readonly DependencyProperty ResetSecondProperty = DependencyProperty.Register
        (
            name: "ResetSecond",
            propertyType: typeof(double),
            ownerType: typeof(MarqueeLabel),
            typeMetadata: new FrameworkPropertyMetadata
            (
                defaultValue: 1d,
                propertyChangedCallback: new PropertyChangedCallback(resetSecondPropertyChanged)
            ),
            validateValueCallback: new ValidateValueCallback(resetSecond_IsValidValue)
        );

        public double ResetSecond
        {
            get { return (double)GetValue(ResetSecondProperty); }
            set { SetValue(ResetSecondProperty, value); }
        }

        private static bool resetSecond_IsValidValue(object value)
        {
            if (double.TryParse(value.ToString(), out double v) == false)
            {
                return false;
            }
            // throw new Exception("回滚重置持续秒数不能小于零");
            return v >= 0 ? true : false;
        }

        private static void resetSecondPropertyChanged(DependencyObject bindable, DependencyPropertyChangedEventArgs eventArgs)
        {
            object newValue = eventArgs.NewValue;
            object oldValue = eventArgs.OldValue;

            if ((double)oldValue == (double)newValue)
            {
                return;
            }

            (bindable as MarqueeLabel).beginAnimation();
        }

        #endregion

        /// <summary>
        /// 当前状态是否已跑马灯形式呈现 
        /// true 跑马灯 | false 静态 
        /// </summary>
        public bool IsMarqueeState
        {
            get { return mStoryBoard != null ? true : false; }
        }

        #region 完成阅读通知事件

        /// <summary>
        /// 完成阅读通知
        /// </summary>
        public event EventHandler<EventArgs> ReadCompleted;

        private void onReadCompleted()
        {
            if (ReadCompleted != null)
            {
                this.ReadCompleted.Invoke(this, new EventArgs());
            }
        }

        #endregion

        #region 回滚重置通知事件

        /// <summary>
        /// 回滚重置通知事件, 信息回滚到最开始后通知
        /// </summary>
        public EventHandler<EventArgs> ResetCompleted;

        private void onResetCompleted()
        {
            if (ResetCompleted != null)
            {
                this.ResetCompleted.Invoke(this, new EventArgs());
            }
        }

        #endregion

        public MarqueeLabel()
        {
            initUI();
            initEvent();
        }

        void initUI()
        {
            this.mCanvas = new Canvas()
            {
                ClipToBounds = true // 设置允许超出 canvas 界限
            };

            this.mLabel = new TextBlock();

            this.mCanvas.Children.Add(this.mLabel);

            this.Content = mCanvas;
        }

        void initEvent()
        {
            this.SizeChanged += marqueeLabel_SizeChanged;
            this.mLabel.SizeChanged += mLabel_SizeChanged;
        }

        private void marqueeLabel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("MarqueeLabel_SizeChanged");
            beginAnimation();
        }

        private void mLabel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("mLabel_SizeChanged");
            beginAnimation();
        }

        private void label_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Label_SizeChanged");
            beginAnimation();
        }

        WPFControls.ActionUtils.DebounceAction DebounceAction { get; set; } = new WPFControls.ActionUtils.DebounceAction();

        void beginAnimation()
        {
            DebounceAction.Debounce
            (
                interval: 300,
                action: () =>
                {
                    beginAnimation_ActualMethod();
                },
                dispatcher: Application.Current.Dispatcher
            );
        }

        void beginAnimation_ActualMethod()
        {
            double height = mCanvas.ActualHeight - mLabel.ActualHeight;
            if (mCanvas.ActualWidth >= mLabel.ActualWidth) // 有足够的长度显示所有信息, 将信息居中后跳出本方法
            {
                string msg = $"无需使用滚动方式显示";
                System.Diagnostics.Debug.WriteLine(msg);
                double width = mCanvas.ActualWidth - mLabel.ActualWidth;
                mLabel.Margin = new Thickness(width / 2, height / 2, 0, 0);
                return;
            }

            if (mStoryBoard != null)
            {
                try
                {
                    // label.AbortAnimation(mAnimationName);
                    mStoryBoard.Stop();
                }
                finally
                {
                    mStoryBoard = null;
                }
            }

            NewsTicker();
        }

        #region 新闻滚动条

        /// <summary>
        /// 新闻滚动条动画名称, 用于取消动画
        /// </summary>
        string mAnimationName
        {
            get
            {
                return "NewsTicker";
            }
        }

        Storyboard mStoryBoard { get; set; }

        private void NewsTicker()
        {
            if (mStoryBoard != null)
            {
                return;
            }

            System.Diagnostics.Debug.WriteLine("执行 NewsTicker");

            double height = mCanvas.ActualHeight - mLabel.ActualHeight;

            if (mLabel.ActualWidth < mCanvas.ActualWidth) // 有足够的长度显示所有信息, 将信息居中后跳出本方法
            {
                double width = mCanvas.ActualWidth - mLabel.ActualWidth;
                mLabel.Margin = new Thickness(width / 2, height / 2, 0, 0);
                return;
            }

            mLabel.Margin = new Thickness(0, height / 2, 0, 0);

            double wordsPerSecode = double.Parse(this.WordsPerSecond.ToString());
            double wordsTotalSeconds = Convert.ToDouble(mLabel.Text.Length) / wordsPerSecode;
            double startBreakSecond = this.StartBreakSecond;
            double endBreakSecond = this.EndBreakSecond;
            double resetSecond = this.ResetSecond;

            // 计算阅读完毕所需时间
            double totalSeconds = startBreakSecond + wordsTotalSeconds + endBreakSecond + resetSecond;

            mStoryBoard = new Storyboard();
            mStoryBoard.RepeatBehavior = RepeatBehavior.Forever;

            // 1. 开始阅读停顿
            DoubleAnimation s0 = new DoubleAnimation();
            //                              阅读开始停顿时间
            s0.BeginTime = TimeSpan.FromTicks(0);
            s0.Duration = new Duration(TimeSpan.FromSeconds(startBreakSecond));
            s0.From = -mLabel.ActualWidth + mCanvas.ActualWidth; ;
            s0.To = -mLabel.ActualWidth + mCanvas.ActualWidth; ;
            Storyboard.SetTarget(s0, mLabel);
            Storyboard.SetTargetProperty(s0, new PropertyPath(Canvas.RightProperty));
            mStoryBoard.Children.Add(s0);


            // 2. 开始阅读            
            DoubleAnimation daStartReading = new DoubleAnimation();
            //                              阅读开始停顿时间
            daStartReading.BeginTime = TimeSpan.FromSeconds(startBreakSecond);
            daStartReading.Duration = new Duration(TimeSpan.FromSeconds(wordsTotalSeconds));
            daStartReading.From = -mLabel.ActualWidth + mCanvas.ActualWidth;
            daStartReading.To = 0;
            Storyboard.SetTarget(daStartReading, mLabel);
            Storyboard.SetTargetProperty(daStartReading, new PropertyPath(Canvas.RightProperty));
            mStoryBoard.Children.Add(daStartReading);


            // 3. 完成阅读开始阅读后停顿
            DoubleAnimation daStopReading = new DoubleAnimation();

            //                               阅读开始停顿时间                 +     阅读完毕所需时间
            daStopReading.BeginTime = TimeSpan.FromSeconds(startBreakSecond) + TimeSpan.FromSeconds(wordsTotalSeconds);
            daStopReading.Duration = new Duration(TimeSpan.FromSeconds(endBreakSecond));
            daStopReading.From = 0;
            daStopReading.To = 0;
            daStopReading.Completed += ((s, e) => { onReadCompleted(); });
            Storyboard.SetTarget(daStopReading, mLabel);
            Storyboard.SetTargetProperty(daStopReading, new PropertyPath(Canvas.RightProperty));
            mStoryBoard.Children.Add(daStopReading);


            // 4. 回滚到最开头
            DoubleAnimation daReset = new DoubleAnimation();
            //                                    阅读开始停顿时间      +             阅读完毕所需时间              +           阅读完毕停顿时间
            daReset.BeginTime = TimeSpan.FromSeconds(startBreakSecond) + TimeSpan.FromSeconds(wordsTotalSeconds) + TimeSpan.FromSeconds(endBreakSecond);
            daReset.Duration = new Duration(TimeSpan.FromSeconds(resetSecond));
            daReset.From = 0;
            daReset.To = -mLabel.ActualWidth + mCanvas.ActualWidth;
            daReset.Completed += ((s, e) => { onResetCompleted(); });
            Storyboard.SetTarget(daReset, mLabel);
            Storyboard.SetTargetProperty(daReset, new PropertyPath(Canvas.LeftProperty));
            mStoryBoard.Children.Add(daReset);


            mStoryBoard.Begin();
        }

        #endregion
    }
}
