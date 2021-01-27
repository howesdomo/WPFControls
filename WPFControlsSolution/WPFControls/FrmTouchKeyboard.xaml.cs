using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

/// <summary>
/// V 1.0.4 2020-11-26 14:24:04
/// 迁移到 WPFControls
/// 
/// V 3
/// 修改命名空间 Client.Components
/// 
/// Version 2
/// 增加输入符号
/// 改为常用键盘位置
/// 
/// Version 1
/// 软键盘首个版本
/// </summary>
namespace Client.Components
{
    /// <summary>
    /// Interaction logic for FrmTouchKeyboard.xaml
    /// </summary>
    public partial class FrmTouchKeyboard : Window
    {
        public FrmTouchKeyboard(Window owner)
        {
            InitializeComponent();
            this.Owner = owner;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            this.initEvent();
        }


        bool mIsClose = false;
        public bool mIsFollowOwner = true;
        public int mControlIndex;

        ObservableCollection<Control> ControlList { get; set; }

        void initEvent()
        {
            this.MouseLeftButtonDown += new MouseButtonEventHandler(FrmTouchPermit_MouseLeftButtonDown);
            this.Closing += new System.ComponentModel.CancelEventHandler(FrmTouchKeyboard_Closing);
            this.Loaded += new RoutedEventHandler(FrmTouchKeyboard_Loaded);
        }

        void FrmTouchKeyboard_Loaded(object sender, RoutedEventArgs e)
        {
            this.InitLocation();
        }

        /// <summary>
        /// 用于程序员微调键盘弹出位置
        /// </summary>
        public double mMoveTop { get; set; } = 0d;

        public double mMoveLeft { get; set; } = 0d;

        public void InitLocation()
        {
            if (this.mIsFollowOwner)
            {
                // 源码
                //this.Left = this.Owner.Left;
                //this.Top = this.Owner.Top - this.Owner.Height;
                
                this.Left = this.Owner.Left + mMoveLeft;
                this.Top = this.Owner.Top - this.Height + mMoveTop;
            }
        }

        void FrmTouchKeyboard_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!this.mIsClose)
            {
                e.Cancel = true;
                this.Hide();
            }
        }

        public void CloseKeyboard()
        {
            this.mIsClose = true;
            this.Close();
        }

        public void ShowKeyboard()
        {
            this.InitLocation();
            this.Show();
        }

        public void HideKeyboard()
        {
            this.Close();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        void FrmTouchPermit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        public void LoadControl(ObservableCollection<Control> collection)
        {
            this.ControlList = new ObservableCollection<Control>(collection);
            this.mControlIndex = 0;
            foreach (Control item in this.ControlList)
            {
                try
                {
                    item.GotFocus -= this.item_GotFocus;
                }
                catch (Exception)
                {

                }
                item.GotFocus += this.item_GotFocus;
            }
        }

        void item_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)
            {
                TextBox currentItem = (sender as TextBox);
                this.mControlIndex = this.ControlList.IndexOf(currentItem);
                currentItem.CaretIndex = currentItem.Text.Length;
                return;
            }
            if (sender is PasswordBox)
            {
                PasswordBox currentItem = (sender as PasswordBox);
                this.mControlIndex = this.ControlList.IndexOf(currentItem);
            }
        }

        private void Input_Click(object sender, RoutedEventArgs e)
        {
            this.input(sender);
        }

        void input(object sender)
        {
            Button btn = sender as Button;
            var input = btn.Content.ToString();
            InputEventArgs arg = new InputEventArgs();
            try
            {
                arg.InputValue = true;
                arg.Value = (input);
                //this.SetFocus();
            }
            catch (Exception ex)
            {
                arg.Message = ex.Message;
            }
            this.OnInput(arg);
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            this.next();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.back();
        }

        private void btnSpace_Click(object sender, RoutedEventArgs e)
        {
            InputEventArgs arg = new InputEventArgs();
            arg.InputValue = true;
            arg.Value = " ";
            this.OnInput(arg);
        }


        bool isUpper = true;

        private void btnSwitch_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in this.gridButton.Children)
            {
                if (item is Button)
                {
                    Button btn = item as Button;
                    if (btn.Tag != null)
                    {
                        if (Convert.ToInt32(btn.Tag) == 0)
                        {
                            continue;
                        }
                    }
                    btn.Content = this.isUpper ? btn.Content.ToString().ToLower() : btn.Content.ToString().ToUpper();
                }
            }
            this.isUpper = !this.isUpper;
        }


        void back()
        {
            InputEventArgs arg = new InputEventArgs();
            arg.IsBack = true;
            this.OnInput(arg);
        }

        void next()
        {
            if (this.ControlList != null && this.ControlList.Count > 0)
            {
                this.mControlIndex++;
                if (this.mControlIndex >= this.ControlList.Count)
                {
                    this.mControlIndex = 0;
                }
                this.SetControlFocus();
            }
        }

        public void SetControlFocus()
        {
            if (this.ControlList != null && this.ControlList.Count > 0)
            {
                this.ControlList[this.mControlIndex].Focus();
                if (this.ControlList[this.mControlIndex] is TextBox)
                {
                    (this.ControlList[this.mControlIndex] as TextBox).CaretIndex = (this.ControlList[this.mControlIndex] as TextBox).Text.Length;
                }
            }
        }

        public event EventHandler<InputEventArgs> InputResult;

        protected virtual void OnInput(InputEventArgs arg)
        {
            if (this.InputResult != null)
            {
                this.InputResult(this, arg);
            }
        }

        public class InputEventArgs : EventArgs
        {
            public string Message { get; set; }

            public string Value { get; set; }

            public bool InputValue { get; set; }

            public bool IsBack { get; set; }

            public bool IsNext { get; set; }

        }
    }
}