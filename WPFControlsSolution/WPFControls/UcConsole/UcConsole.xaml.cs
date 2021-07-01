using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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

/// <summary>
/// V 1.0.3 - 2021-07-01 09:19:16
/// 1 增加自动滚动到底部功能
/// 2 新增过滤新信息功能
/// 
/// V 1.0.2 - 2021-05-17 13:32:50
/// 1 ConsoleMsgType 使用转换器 UcConsole_ConsoleMsgType_Converter, 将内容固定为 10 位
/// 2 使用系统自带的 Monospace(等宽)字体来显示 ConsoleMsgType（信息标识）
/// 3 新增依赖属性 ContentFontFamily
/// 
/// V 1.0.1 - 2020-9-23 14:38:01
/// 1. 修正 UI 字眼
/// 2. 导出时间信息精确到最后毫秒位数
/// 3. 优化代码结构
/// </summary>
namespace Client.Components
{
    /// <summary>
    /// UcConsole.xaml 的交互逻辑
    /// </summary>
    public partial class UcConsole : UserControl, System.ComponentModel.INotifyPropertyChanged
    {
        #region [DP] ContentFontFamily

        public static readonly DependencyProperty ContentFontFamilyProperty = DependencyProperty.Register
        (
            name: "ContentFontFamily",
            propertyType: typeof(System.Windows.Media.FontFamily),
            ownerType: typeof(UcConsole),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: new System.Windows.Media.FontFamily(),
                propertyChangedCallback: null, //onContentFontFamily_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public System.Windows.Media.FontFamily ContentFontFamily
        {
            get { return (System.Windows.Media.FontFamily)GetValue(ContentFontFamilyProperty); }
            set { SetValue(ContentFontFamilyProperty, value); }
        }

        #endregion

        #region [DP] AutoScroolToBottom_DiffValue

        public static readonly DependencyProperty AutoScroolToBottom_DiffValueProperty = DependencyProperty.Register
        (
            name: "AutoScroolToBottom_DiffValue",
            propertyType: typeof(double),
            ownerType: typeof(UcConsole),
            validateValueCallback: new ValidateValueCallback((toValidate) => 
            {
                if (toValidate is null) { return false; }

                return double.TryParse(toValidate.ToString(), out double temp);
            }),
            typeMetadata: new PropertyMetadata
            (
                defaultValue: Client.Controls.AttachUtils.AutoScrollToBottom._DiffValue_Default_Value_,
                propertyChangedCallback: null, // onAutoScroolToBottom_DiffValue_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public double AutoScroolToBottom_DiffValue
        {
            get { return (double)GetValue(AutoScroolToBottom_DiffValueProperty); }
            set { SetValue(AutoScroolToBottom_DiffValueProperty, value); }
        }

        #endregion




        public System.Collections.ObjectModel.ObservableCollection<dynamic> ConsoleList { get; set; }

        public UcConsole()
        {
            InitializeComponent();
            initEvent();

            this.DataContext = this;
            this.ConsoleList = new System.Collections.ObjectModel.ObservableCollection<dynamic>();
            this.ConsoleList.CollectionChanged += mConsoleList_CollectionChanged;
        }

        void initEvent()
        {
            listBox_Ctrl_C_Copy();
        }

        /// <summary>
        /// 由于 ListBox 选中一或多项后, 按 Ctrl + C 并不会复制数据到系统剪贴板
        /// 故本方法为其实现复制数据功能
        /// </summary>
        void listBox_Ctrl_C_Copy() 
        {
            ExecutedRoutedEventHandler handler = (sender, args) =>
            {
                if (listBox.SelectedItems == null || listBox.SelectedItems.Count <= 0)
                {
                    return;
                }

                copyJSONActualMethod(listBox.SelectedItems);
            };

            var command = new RoutedCommand("Copy", typeof(ListBox));
            command.InputGestures.Add(new KeyGesture(Key.C, ModifierKeys.Control, "Copy"));
            listBox.CommandBindings.Add(new CommandBinding(command, handler));
        }

        private void mConsoleList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.OnPropertyChanged("ConsoleList");
        }
       
        public void Add(Util.Model.ConsoleData d)
        {
            if (HasFilterConditions)
            {
                if (this.SelectedConsoleMsgType != Util.Model.ConsoleMsgType.NONE)
                {
                    if (d.ConsoleMsgType != SelectedConsoleMsgType)
                    {
                        return;
                    }
                }

                if (this.SelectedRegexCondition != Util.Model.ContentTextFilterCondition.None && string.IsNullOrEmpty(this.RegexPattern) == false)
                {
                    switch (this.SelectedRegexCondition)
                    {
                        case Util.Model.ContentTextFilterCondition.Equals:
                            if (System.Text.RegularExpressions.Regex.IsMatch(input: d.Content, pattern: this.RegexPattern) == false)
                            {
                                return;
                            }
                            break;
                        case Util.Model.ContentTextFilterCondition.NotEquals:
                            if (System.Text.RegularExpressions.Regex.IsMatch(input: d.Content, pattern: this.RegexPattern) == true)
                            {
                                return;
                            }
                            break;
                        default:
                            throw new ArgumentException("不属于给定的条件");
                    }
                }

                if (this.SelectedTextCondition != Util.Model.ContentTextFilterCondition.None && string.IsNullOrEmpty(this.TextPattern) == false)
                {
                    switch (this.SelectedTextCondition)
                    {
                        case Util.Model.ContentTextFilterCondition.StartsWith:
                            if (d.Content.StartsWith(this.TextPattern) == false) { return; }
                            break;                            
                        case Util.Model.ContentTextFilterCondition.EndsWith:
                            if (d.Content.EndsWith(this.TextPattern) == false) { return; }
                            break;
                        case Util.Model.ContentTextFilterCondition.Contains:
                            if (d.Content.Contains(this.TextPattern) == false) { return; }
                            break;
                        case Util.Model.ContentTextFilterCondition.Equals:
                            if (d.Content.Equals(this.TextPattern) == false) { return; }
                            break;
                        case Util.Model.ContentTextFilterCondition.NotEquals:
                            if (d.Content.Equals(this.TextPattern) == true) { return; }
                            break;
                    }
                }
            }

            this.ConsoleList.Add(d);
        }

        public void Clear()
        {
            this.ConsoleList.Clear();
        }

        #region 复制

        private void menuItemCopy(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedItems == null || listBox.SelectedItems.Count <= 0)
            {
                return;
            }

            copyActualMethod(listBox.SelectedItems);
        }

        private void menuItemAllCopy(object sender, RoutedEventArgs e)
        {
            if (ConsoleList == null || ConsoleList.Count <= 0)
            {
                return;
            }

            copyActualMethod(listBox.Items);
        }

        private void copyActualMethod(IList list)
        {
            StringBuilder sb = new StringBuilder();

            foreach (object obj in list)
            {
                if (obj is Util.Model.ConsoleData)
                {
                    var item = obj as Util.Model.ConsoleData;
                    var dtMsg = item.EntryTime.ToString("yyyy-MM-dd HH:mm:ss.fffffff");
                    if (menuItem_OutputNewLineSymbol.IsChecked == false)
                    {
                        sb.AppendLine($"{dtMsg}\t[{item.ConsoleMsgType}]\t{item.Content}");
                    }
                    else
                    {
                        sb.AppendLine($"{dtMsg}\t[{item.ConsoleMsgType}]\t{item.Content.Replace("\r", "\\r").Replace("\n", "\\n")}");
                    }
                }
            }

            var msg = sb.ToString();
            msg = msg.Substring(0, msg.Length - 2);

            Clipboard.SetText(msg);
        }

        #endregion

        #region 复制内容

        private void menuItemCopyMsg(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedItems == null || listBox.SelectedItems.Count <= 0)
            {
                return;
            }

            copyMsgActualMethod(listBox.SelectedItems);
        }

        private void menuItemAllCopyMsg(object sender, RoutedEventArgs e)
        {
            if (ConsoleList == null || ConsoleList.Count <= 0)
            {
                return;
            }

            copyMsgActualMethod(listBox.Items);
        }

        private void copyMsgActualMethod(IList list)
        {
            StringBuilder sb = new StringBuilder();

            foreach (object obj in list)
            {
                if (obj is Util.Model.ConsoleData)
                {
                    var item = obj as Util.Model.ConsoleData;
                    if (menuItem_OutputNewLineSymbol.IsChecked == false)
                    {
                        sb.AppendLine(item.Content);
                    }
                    else 
                    {
                        sb.AppendLine(item.Content.Replace("\r", "\\r").Replace("\n", "\\n"));
                    }
                }
            }

            var msg = sb.ToString();
            msg = msg.Substring(0, msg.Length - 2);

            Clipboard.SetText(msg);
        }

        #endregion

        #region 复制CSV

        private void menuItemCopyCSV(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedItems == null || listBox.SelectedItems.Count <= 0)
            {
                return;
            }

            copyCSVActualMethod(listBox.SelectedItems);
        }

        private void menuItemAllCopyCSV(object sender, RoutedEventArgs e)
        {
            if (ConsoleList == null || ConsoleList.Count <= 0)
            {
                return;
            }

            copyCSVActualMethod(listBox.Items);
        }

        private void copyCSVActualMethod(IList list)
        {
            string separator = menuItem_CSVSeparator.Text;

            StringBuilder sb = new StringBuilder();

            foreach (object obj in list)
            {
                if (obj is Util.Model.ConsoleData)
                {
                    var item = obj as Util.Model.ConsoleData;
                    var dtMsg = item.EntryTime.ToString("yyyy-MM-dd HH:mm:ss.fffffff");
                    if (menuItem_OutputNewLineSymbol.IsChecked == false)
                    {
                        sb.AppendLine($"{dtMsg}{separator}[{item.ConsoleMsgType}]{separator}{item.Content}");
                    }
                    else 
                    {
                        sb.AppendLine($"{dtMsg}{separator}[{item.ConsoleMsgType}]{separator}{item.Content.Replace("\r", "\\r").Replace("\n", "\\n")}");
                    }
                }
            }

            var msg = sb.ToString();
            msg = msg.Substring(0, msg.Length - 2);

            Clipboard.SetText(msg);
        }

        #endregion

        #region 复制JSON

        private void menuItemCopyJson(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedItems == null || listBox.SelectedItems.Count <= 0)
            {
                return;
            }

            copyJSONActualMethod(listBox.SelectedItems);
        }

        private void menuItemAllCopyJson(object sender, RoutedEventArgs e)
        {
            if (ConsoleList == null || ConsoleList.Count <= 0)
            {
                return;
            }

            copyJSONActualMethod(listBox.Items);
        }

        private void copyJSONActualMethod(IList list)
        {
            StringBuilder sb = new StringBuilder();

            foreach (object obj in list)
            {
                if (obj is Util.Model.ConsoleData)
                {
                    var item = obj as Util.Model.ConsoleData;
                    var dtMsg = item.EntryTime.ToString("yyyy-MM-dd HH:mm:ss.fffffff");
                    var content = item.Content.Replace("\r", "\\r").Replace("\n", "\\n");
                    sb.Append("{");
                    sb.Append($"\"EntryTime\":\"{dtMsg}\", \"ConsleMsgType\":\"{item.ConsoleMsgType}\", \"Content\":\"{content}\"");
                    sb.Append("},");
                    sb.AppendLine();
                }
            }

            var msg = sb.ToString();
            msg = msg.Substring(0, msg.Length - 3);
            msg = $"[{msg}]";

            Clipboard.SetDataObject(msg);
        }

        #endregion

        private void menuItemClearSelected(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedItems == null || listBox.SelectedItems.Count <= 0)
            {
                return;
            }

            var dr = MessageBox.Show("确认清理[选中]项？", "提示", MessageBoxButton.YesNo);
            if (dr == MessageBoxResult.Yes)
            {
                List<int> toDelete = new List<int>();

                foreach (var item in listBox.SelectedItems)
                {
                    toDelete.Add(this.ConsoleList.IndexOf(item));
                }

                foreach (int index in toDelete.OrderByDescending(i => i))
                {
                    this.ConsoleList.RemoveAt(index);
                }
            }
        }

        private void menuItemClearUnSelected(object sender, RoutedEventArgs e)
        {
            var dr = MessageBox.Show("确认清理[未选中]项？", "提示", MessageBoxButton.YesNo);
            if (dr == MessageBoxResult.Yes)
            {
                List<int> toRemain = new List<int>();
                List<int> toDelete = new List<int>();

                int total = this.ConsoleList.Count;

                foreach (var item in listBox.SelectedItems)
                {
                    toRemain.Add(this.ConsoleList.IndexOf(item));
                }

                for (int index = total - 1; index > -1; index--)
                {
                    if (toRemain.Contains(index) == false)
                    {
                        toDelete.Add(index);
                    }
                }

                foreach (int index in toDelete)
                {
                    this.ConsoleList.RemoveAt(index);
                }
            }
        }

        private void menuItemClearAll(object sender, RoutedEventArgs e)
        {
            var dr = MessageBox.Show("确认清理[全部]？", "提示", MessageBoxButton.YesNo);
            if (dr == MessageBoxResult.Yes)
            {
                Clear();
            }
        }

        #region 过滤搜索条件 Advance 新增特性


        public static readonly DependencyProperty FilterConditionsVisibilityProperty = DependencyProperty.Register
        (
            name: "FilterConditionsVisibility",
            propertyType: typeof(Visibility),
            ownerType: typeof(UcConsole),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: Visibility.Collapsed,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public Visibility FilterConditionsVisibility
        {
            get { return (Visibility)GetValue(FilterConditionsVisibilityProperty); }
            set { SetValue(FilterConditionsVisibilityProperty, value); }
        }

        public List<Util.Model.ConsoleMsgType> ConsoleMsgTypeList { get; set; } = new List<Util.Model.ConsoleMsgType>()
        {
            Util.Model.ConsoleMsgType.NONE,
            Util.Model.ConsoleMsgType.DEFAULT,
            Util.Model.ConsoleMsgType.DEBUG ,
            Util.Model.ConsoleMsgType.INFO,
            Util.Model.ConsoleMsgType.WARNING ,
            Util.Model.ConsoleMsgType.ERROR ,
            Util.Model.ConsoleMsgType.BUSINESSERROR
        };

        void filterConditionNotify()
        {
            this.OnPropertyChanged(nameof(HasFilterConditions));
            this.OnPropertyChanged(nameof(HasFilterConditionsInfo));
        }

        private Util.Model.ConsoleMsgType _SelectedConsoleMsgType = Util.Model.ConsoleMsgType.NONE;
        public Util.Model.ConsoleMsgType SelectedConsoleMsgType
        {
            get { return _SelectedConsoleMsgType; }
            set
            {
                _SelectedConsoleMsgType = value;
                this.OnPropertyChanged(nameof(SelectedConsoleMsgType));

                filterConditionNotify();
            }
        }

        public List<Util.Model.ContentTextFilterCondition> RegexConditionList { get; set; } = new List<Util.Model.ContentTextFilterCondition>()
        {
            Util.Model.ContentTextFilterCondition.None,
            Util.Model.ContentTextFilterCondition.Equals,
            Util.Model.ContentTextFilterCondition.NotEquals,
        };

        private Util.Model.ContentTextFilterCondition _SelectedRegexCondition = Util.Model.ContentTextFilterCondition.None;
        public Util.Model.ContentTextFilterCondition SelectedRegexCondition
        {
            get { return _SelectedRegexCondition; }
            set
            {
                _SelectedRegexCondition = value;                
                this.OnPropertyChanged(nameof(SelectedRegexCondition));

                filterConditionNotify();
            }
        }


        private string _RegexPattern;
        public string RegexPattern
        {
            get { return _RegexPattern; }
            set
            {
                _RegexPattern = value;                
                this.OnPropertyChanged(nameof(RegexPattern));

                filterConditionNotify();
            }
        }

        public List<Util.Model.ContentTextFilterCondition> TextConditionList { get; set; } = new List<Util.Model.ContentTextFilterCondition>()
        {
            
            Util.Model.ContentTextFilterCondition.None,
            Util.Model.ContentTextFilterCondition.StartsWith,
            Util.Model.ContentTextFilterCondition.EndsWith,
            Util.Model.ContentTextFilterCondition.Contains,
            Util.Model.ContentTextFilterCondition.Equals,
            Util.Model.ContentTextFilterCondition.NotEquals,
        };

        private Util.Model.ContentTextFilterCondition _SelectedTextCondition = Util.Model.ContentTextFilterCondition.None;
        public Util.Model.ContentTextFilterCondition SelectedTextCondition
        {
            get { return _SelectedTextCondition; }
            set
            {
                _SelectedTextCondition = value;
                this.OnPropertyChanged(nameof(SelectedTextCondition));

                filterConditionNotify();
            }
        }


        private string _TextPattern;
        public string TextPattern
        {
            get { return _TextPattern; }
            set
            {
                _TextPattern = value;
                this.OnPropertyChanged(nameof(TextPattern));

                filterConditionNotify();
            }
        }



        public bool HasFilterConditions
        {
            get
            {
                if
                (
                       this.SelectedConsoleMsgType != Util.Model.ConsoleMsgType.NONE
                    || ( this.SelectedRegexCondition != Util.Model.ContentTextFilterCondition.None && string.IsNullOrEmpty(this.RegexPattern) == false )
                    || ( this.SelectedTextCondition != Util.Model.ContentTextFilterCondition.None && string.IsNullOrEmpty(this.TextPattern) == false )

                )
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public string HasFilterConditionsInfo
        {
            get
            {
                string r = string.Empty;
                if (HasFilterConditions)
                {
                    r = "含过滤条件";
                }
                return r;
            }
        }


        private void btnClearFilterConditions_Click(object sender, RoutedEventArgs e)
        {
            this._SelectedConsoleMsgType = Util.Model.ConsoleMsgType.NONE;
            this._SelectedRegexCondition = Util.Model.ContentTextFilterCondition.None;
            this._RegexPattern = string.Empty;
            this._SelectedTextCondition = Util.Model.ContentTextFilterCondition.None;
            this._TextPattern = string.Empty;

            this.OnPropertyChanged(nameof(SelectedConsoleMsgType));
            this.OnPropertyChanged(nameof(SelectedRegexCondition));
            this.OnPropertyChanged(nameof(RegexPattern));
            this.OnPropertyChanged(nameof(SelectedTextCondition));
            this.OnPropertyChanged(nameof(TextPattern));
            
            this.filterConditionNotify();
        }


        #endregion

        #region INotifyPropertyChanged成员

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
        
    }


}
