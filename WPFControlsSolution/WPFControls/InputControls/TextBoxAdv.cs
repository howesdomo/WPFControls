using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Client.Components
{
    /// <summary>
    /// V 1.0.1 - 2021-09-02 17:25:48
    /// 用 Command 的方式
    /// 1. 重写以下逻辑 剪贴 / 复制 / 粘贴 / 撤销 / 重做 ( 可以绑定到 ContextMenu 的 ItemMenu 中 )
    /// 2. 实现以下逻辑 清空内容 / 去重 / 排序
    /// 3. 重新实现 ContextMenu 功能菜单
    /// 
    /// V 1.0.0 - 2021-08-03 16:30:25
    /// 对于 TextBoxAdv_V0 的重写
    /// </summary>
    public class TextBoxAdv : TextBox
    {
        // TODO 测试 IDataError 是否会出现红框标识样式问题

        static TextBoxAdv()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBoxAdv), new FrameworkPropertyMetadata(typeof(TextBoxAdv)));
        }

        public TextBoxAdv()
        {
            initCMD();
            this.ContextMenu = getContextMenu();
        }

        void initCMD()
        {
            this.CMD_Cut = new Command(cmdMethod_Cut);
            this.InputBindings.Add(new System.Windows.Input.KeyBinding(modifiers: System.Windows.Input.ModifierKeys.Control, key: System.Windows.Input.Key.X, command: CMD_Cut));

            this.CMD_Copy = new Command(cmdMethod_Copy);
            this.InputBindings.Add(new System.Windows.Input.KeyBinding(modifiers: System.Windows.Input.ModifierKeys.Control, key: System.Windows.Input.Key.C, command: CMD_Copy));

            this.CMD_Paste = new Command(cmdMethod_Paste);
            this.InputBindings.Add(new System.Windows.Input.KeyBinding(modifiers: System.Windows.Input.ModifierKeys.Control, key: System.Windows.Input.Key.V, command: CMD_Paste));

            this.CMD_Redo = new Command(cmdMethod_Redo);
            this.InputBindings.Add(new System.Windows.Input.KeyBinding(modifiers: System.Windows.Input.ModifierKeys.Control, key: System.Windows.Input.Key.Y, command: CMD_Redo));

            this.CMD_Undo = new Command(cmdMethod_Undo);
            this.InputBindings.Add(new System.Windows.Input.KeyBinding(modifiers: System.Windows.Input.ModifierKeys.Control, key: System.Windows.Input.Key.Z, command: CMD_Undo));

            this.CMD_Clear = new Command(cmdMethod_Clear);
            this.InputBindings.Add(new System.Windows.Input.KeyBinding(modifiers: System.Windows.Input.ModifierKeys.Control, key: System.Windows.Input.Key.Back, command: CMD_Clear));

            this.CMD_Distinct = new Command(cmdMethod_Distinct);
            this.InputBindings.Add(new System.Windows.Input.KeyBinding(modifiers: System.Windows.Input.ModifierKeys.Control, key: System.Windows.Input.Key.D, command: CMD_Distinct));

            this.CMD_Distinct_OrderBy = new Command(cmdMethod_Distinct_OrderBy);
            this.InputBindings.Add(new System.Windows.Input.KeyBinding(modifiers: System.Windows.Input.ModifierKeys.Control, key: System.Windows.Input.Key.Q, command: CMD_Distinct_OrderBy));
        }

        #region 用 Command 的方式重写实现 剪贴 / 复制 / 粘贴 / 清空内容 / 去重 / 排序

        public Command CMD_Cut { get; private set; }
        void cmdMethod_Cut()
        {
            if (this.IsEnabled == false) return;

            if (this.IsReadOnly == true)
            {
                this.Copy();
                return;
            }

            this.Cut();
        }

        public Command CMD_Copy { get; private set; }
        void cmdMethod_Copy()
        {
            this.Copy();
        }

        public Command CMD_Paste { get; private set; }
        void cmdMethod_Paste()
        {
            if (this.IsEnabled == false) return;
            if (this.IsReadOnly == true) return;

            this.Paste();
        }

        public Command CMD_Redo { get; private set; }
        void cmdMethod_Redo()
        {
            if (this.IsEnabled == false) return;
            if (this.IsReadOnly == true) return;

            this.Redo();
        }

        public Command CMD_Undo { get; private set; }
        void cmdMethod_Undo()
        {
            if (this.IsEnabled == false) return;
            if (this.IsReadOnly == true) return;

            this.Undo();
        }

        public Command CMD_Clear { get; private set; }
        void cmdMethod_Clear()
        {
            if (this.IsEnabled == false) return;
            if (this.IsReadOnly == true) return;

            this.Text = string.Empty;
        }

        IEnumerable<string> TextDistinct(string content)
        {
            return content.Split(separator: new string[] { "\r", "\n", "\r\n" }, options: StringSplitOptions.None)
                   .Where(i => string.IsNullOrEmpty(i) == false)
                   .Distinct();
        }

        public Command CMD_Distinct { get; private set; }
        void cmdMethod_Distinct()
        {
            if (this.IsEnabled == false) return;
            if (this.IsReadOnly == true) return;

            if (string.IsNullOrEmpty(this.Text))
            {
                return;
            }

            var r = string.Join("\r\n", TextDistinct(this.Text));
            this.Text = r;
        }

        public Command CMD_Distinct_OrderBy { get; private set; }
        void cmdMethod_Distinct_OrderBy()
        {
            if (this.IsEnabled == false) return;
            if (this.IsReadOnly == true) return;

            if (string.IsNullOrEmpty(this.Text))
            {
                return;
            }

            var r = string.Join
            (
                separator: "\r\n",
                values: TextDistinct(this.Text).OrderBy(i => i)
            );

            this.Text = r;
        }

        #endregion

        #region [DP] Placeholder

        public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register
        (
            name: "Placeholder",
            propertyType: typeof(string),
            ownerType: typeof(TextBoxAdv),
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
            ownerType: typeof(TextBoxAdv),
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
            ownerType: typeof(TextBoxAdv),
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

        #region ContextMenu

        MenuItem menuItem_Cut;
        MenuItem menuItem_Copy;
        MenuItem menuItem_Paste;

        MenuItem menuItem_Redo;
        MenuItem menuItem_Undo;

        MenuItem menuItem_Clear;
        MenuItem menuItem_Distinct;
        MenuItem menuItem_Distinct_OrderBy;

        #endregion


        ContextMenu getContextMenu()
        {
            // TODO 按照 TextBox 默认的 ContextMenu 的逻辑写好 menuItem_Cut             menuItem_Copy            menuItem_Paste 的 IsEnabled 状态
            // TODO 学习怎样做到 按 T 就执行剪贴 / 按 C 就执行复制

            ContextMenu r = new ContextMenu();

             menuItem_Cut = new MenuItem() { Header = "剪贴(T)", InputGestureText = "Ctrl+X", Command = CMD_Cut };
             menuItem_Copy = new MenuItem() { Header = "复制(C)", InputGestureText = "Ctrl+C", Command = CMD_Copy };
             menuItem_Paste = new MenuItem() { Header = "粘贴(P)", InputGestureText = "Ctrl+V", Command = CMD_Paste };

            r.Items.Add(menuItem_Cut);
            r.Items.Add(menuItem_Copy);
            r.Items.Add(menuItem_Paste);

            r.Items.Add(new Separator());

            // ***********************************************
             menuItem_Redo = new MenuItem() { Header = "重做", InputGestureText = "Ctrl+Y", Command = CMD_Redo };
             menuItem_Undo = new MenuItem() { Header = "撤销", InputGestureText = "Ctrl+Z", Command = CMD_Undo };

            r.Items.Add(menuItem_Redo);
            r.Items.Add(menuItem_Undo);

            r.Items.Add(new Separator());

            // ***********************************************
             menuItem_Clear = new MenuItem() { Header = "清空内容", InputGestureText = "Ctrl+Backspace", Command = CMD_Clear };

            r.Items.Add(menuItem_Clear);
            r.Items.Add(new Separator());

            // ***********************************************
             menuItem_Distinct = new MenuItem() { Header = "去重", InputGestureText = "Ctrl+D", Command = CMD_Distinct };
             menuItem_Distinct_OrderBy = new MenuItem() { Header = "去重并排序", InputGestureText = "Ctrl+Q", Command = CMD_Distinct_OrderBy };

            r.Items.Add(menuItem_Distinct);
            r.Items.Add(menuItem_Distinct_OrderBy);

            return r;
        }

        /// <summary>
        /// V 1.0.0
        /// WPF .net framework 4 默认 Command 代码实现
        /// </summary>
        public class Command : ICommand
        {
            readonly Func<object, bool> _canExecute;
            readonly Action<object> _execute;

            public Command(Action<object> execute)
            {
                if (execute == null)
                    throw new ArgumentNullException(nameof(execute));

                _execute = execute;
            }

            public Command(Action execute) : this(o => execute())
            {
                if (execute == null)
                    throw new ArgumentNullException(nameof(execute));
            }

            public Command(Action<object> execute, Func<object, bool> canExecute) : this(execute)
            {
                if (canExecute == null)
                    throw new ArgumentNullException(nameof(canExecute));

                _canExecute = canExecute;
            }

            public Command(Action execute, Func<bool> canExecute) : this(o => execute(), o => canExecute())
            {
                if (execute == null)
                    throw new ArgumentNullException(nameof(execute));
                if (canExecute == null)
                    throw new ArgumentNullException(nameof(canExecute));
            }

            public bool CanExecute(object parameter)
            {
                if (_canExecute != null)
                    return _canExecute(parameter);

                return true;
            }

            public event EventHandler CanExecuteChanged
            {
                add
                {
                    if (_canExecute != null)
                    {
                        CommandManager.RequerySuggested += value;
                    }
                }
                remove
                {
                    if (_canExecute != null)
                    {
                        CommandManager.RequerySuggested -= value;
                    }
                }
            }

            public void Execute(object parameter)
            {
                _execute(parameter);
            }

        }
    }
}

