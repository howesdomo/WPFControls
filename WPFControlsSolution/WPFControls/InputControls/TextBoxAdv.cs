using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using WPFControls; // WPFControls.Command

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
            initEvent();
            initCMD();
            this.ContextMenu = getContextMenu();
        }

        void initEvent()
        {
            this.SelectionChanged += TextBoxAdv_SelectionChanged;

            this.TextChanged += TextBoxAdv_TextChanged;
        }

        WPFControls.ActionUtils.DebounceAction mDebounceAction { get; set; } = new WPFControls.ActionUtils.DebounceAction();

        private void TextBoxAdv_SelectionChanged(object sender, RoutedEventArgs e)
        {
            string msg = $"SelectionChanged";
            System.Diagnostics.Debug.WriteLine(msg);

            mDebounceAction.Debounce
            (
                interval: 300d,
                action: bindUI,
                dispatcher: this.Dispatcher
            );
        }

        private void TextBoxAdv_TextChanged(object sender, TextChangedEventArgs e)
        {
            string msg = $"TextChanged";
            System.Diagnostics.Debug.WriteLine(msg);

            mDebounceAction.Debounce
            (
                interval: 300d,
                action: bindUI,
                dispatcher: this.Dispatcher
            );
        }

        // TODD Rename
        void bindUI()
        {
            bool hasSelectedText = MenuItem_CutOrCopy_IsEnabled;
            menuItem_Cut.IsEnabled = hasSelectedText;
            menuItem_Copy.IsEnabled = hasSelectedText;

            // menuItem_Paste.IsEnabled // 没有好的时机触发状态检测

            menuItem_Redo.IsEnabled = this.CanRedo;
            menuItem_Undo.IsEnabled = this.CanUndo;

            bool isTextNotNull = MenuItem_Clear_IsEnabled;
            menuItem_Clear.IsEnabled = isTextNotNull;
            menuItem_Distinct.IsEnabled = isTextNotNull;
            menuItem_Distinct_OrderBy.IsEnabled = isTextNotNull;
        }

        public bool MenuItem_CutOrCopy_IsEnabled
        {
            get
            {
                bool r = false;

                if (this.SelectedText.Length > 0)
                {
                    r = true;
                }

                return r;
            }
        }

        public bool MenuItem_Clear_IsEnabled
        {
            get
            {
                bool r = false;

                if (string.IsNullOrEmpty(this.Text) == false)
                {
                    r = true;
                }

                return r;
            }
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
            this.InputBindings.Add(new System.Windows.Input.KeyBinding(modifiers: System.Windows.Input.ModifierKeys.Control, key: System.Windows.Input.Key.D0, command: CMD_Distinct_OrderBy));

            this.CMD_Distinct_OrderByDesc = new Command(cmdMethod_Distinct_OrderByDesc);
            this.InputBindings.Add(new System.Windows.Input.KeyBinding(modifiers: System.Windows.Input.ModifierKeys.Control, key: System.Windows.Input.Key.D9, command: CMD_Distinct_OrderByDesc));
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

            // this.Text = string.Empty;
            this.Clear();
        }

        Tuple<IEnumerable<string>, int, int> TextDistinct(string content)
        {
            var arr = content.Split(separator: new string[] { "\r\n", "\r", "\n" }, options: StringSplitOptions.None);

            int oldArrCount = arr.Length;

            var enumerable = arr.Where(i => string.IsNullOrEmpty(i) == false)
                                .Distinct();

            int newArrCount = enumerable.Count();

            return new Tuple<IEnumerable<string>, int, int>(enumerable, oldArrCount, newArrCount);
        }

        /// <summary>
        /// 设置 this.Text 
        /// 数量量小的时候使用, 这样设置 Text值 可以用 撤销功能 ( 做到还原到去重前的 Text值 )
        /// </summary>
        /// <param name="content"></param>
        void setText(string content)
        {
            // ???????? 有什么办法可以不用粘贴板, 做到赋值的效果

            string backupClipboardText = Clipboard.GetText(); // 先备份当前剪贴板的 Text 值

            #region this.Text = content;

            Clipboard.SetText(content);
            this.SelectAll();
            this.Paste();

            #endregion

            // 先清空本次操作剪贴板的内容, 然后还原备份值到剪贴板
            Clipboard.Clear();

            if (string.IsNullOrEmpty(backupClipboardText) == false)
            {
                Clipboard.SetText(backupClipboardText);
            }
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

            var tupleResult = TextDistinct(this.Text);

            var r = string.Join("\r\n", tupleResult.Item1);

            setText(r);
        }

        StrLogicalComparer mStrLogicalComparer { get; set; } = new StrLogicalComparer();

        public Command CMD_Distinct_OrderBy { get; private set; }
        void cmdMethod_Distinct_OrderBy()
        {
            if (this.IsEnabled == false) return;
            if (this.IsReadOnly == true) return;

            if (string.IsNullOrEmpty(this.Text))
            {
                return;
            }

            var tupleResult = TextDistinct(this.Text);
            
            var r = string.Join
            (
                separator: "\r\n",
                values: tupleResult.Item1.OrderBy
                (
                    keySelector: i => i,
                    comparer: mStrLogicalComparer
                )
            );

            setText(r);
        }

        public Command CMD_Distinct_OrderByDesc { get; private set; }
        void cmdMethod_Distinct_OrderByDesc()
        {
            if (this.IsEnabled == false) return;
            if (this.IsReadOnly == true) return;

            if (string.IsNullOrEmpty(this.Text))
            {
                return;
            }

            var tupleResult = TextDistinct(this.Text);

            var r = string.Join
            (
                separator: "\r\n",
                values: tupleResult.Item1.OrderByDescending
                (
                    keySelector: i => i,
                    comparer: mStrLogicalComparer
                )
            );

            setText(r);
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
        MenuItem menuItem_Distinct_OrderByDesc;

        #endregion

        ContextMenu getContextMenu()
        {
            // TODO 学习怎样像默认的右键菜单做到 按 T 就执行剪贴 / 按 C 就执行复制

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
            menuItem_Distinct_OrderBy = new MenuItem() { Header = "去重并顺排序", InputGestureText = "Ctrl+0", Command = CMD_Distinct_OrderBy };
            menuItem_Distinct_OrderByDesc = new MenuItem() { Header = "去重并逆排序", InputGestureText = "Ctrl+9", Command = CMD_Distinct_OrderByDesc };

            r.Items.Add(menuItem_Distinct);
            r.Items.Add(menuItem_Distinct_OrderBy);
            r.Items.Add(menuItem_Distinct_OrderByDesc);

            return r;
        }


        /// <summary>
        /// 使用 Shlwapi.dll 的方法 StrCmpLogicalW 进行比较, 
        /// 类似Windows资源管理的文字排序方式
        /// </summary>
        class StrLogicalComparer : Comparer<object>
        {
            [DllImport("Shlwapi.dll", CharSet = CharSet.Unicode)]
            private static extern int StrCmpLogicalW(string x, string y);

            public override int Compare(object x, object y)
            {
                return StrCmpLogicalW(x.ToString(), y.ToString());
            }
        }
    }
}

