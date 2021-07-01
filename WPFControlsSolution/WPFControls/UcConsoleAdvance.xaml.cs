using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
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

namespace Client.Components
{

    /// <summary>
    /// V 1.0.0 - 2021-06-30 17:58:48
    /// 拷贝自 UcConsole, 增加过滤搜索条件
    /// </summary>
    public partial class UcConsoleAdvance : UserControl, System.ComponentModel.INotifyPropertyChanged
    {   
        public static readonly DependencyProperty ContentFontFamilyProperty = DependencyProperty.Register
        (
            name: "ContentFontFamily",
            propertyType: typeof(System.Windows.Media.FontFamily),
            ownerType: typeof(UcConsoleAdvance),
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

        //public static void onContentFontFamily_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    if (d is UcConsoleAdvance target) 
        //    {

        //    }            
        //}


        // public System.Collections.ObjectModel.ObservableCollection<dynamic> ConsoleList { get; set; }

        // public ObservableQueue<dynamic> ConsoleList { get; set; }
        public ObservableQueue<Util.Model.ConsoleData> ConsoleList { get; set; }

        // public ObservableQueue<dynamic> FilteredConsoleList { get; set; }
        // public ObservableQueue<Util.Model.ConsoleData> FilteredConsoleList { get; set; }

        private List<Util.Model.ConsoleData> _FilteredConsoleList;
        public List<Util.Model.ConsoleData> FilteredConsoleList
        {
            get { return _FilteredConsoleList; }
            set
            {
                _FilteredConsoleList = value;
                this.OnPropertyChanged(nameof(FilteredConsoleList));
            }
        }


        public static readonly DependencyProperty MaxCapacityProperty = DependencyProperty.Register
        (
            name: "MaxCapacity",
            propertyType: typeof(int),
            ownerType: typeof(UcConsoleAdvance),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: 99999,
                propertyChangedCallback: onMaxCapacity_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public int MaxCapacity
        {
            get { return (int)GetValue(MaxCapacityProperty); }
            set { SetValue(MaxCapacityProperty, value); }
        }

        public static void onMaxCapacity_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UcConsoleAdvance target)
            {
                if (e.NewValue != null && int.TryParse(e.NewValue.ToString(), out int args))
                {
                    target.ConsoleList.QueueMaxCapacity = args;
                }
            }
        }


        public UcConsoleAdvance()
        {
            InitializeComponent();
            initEvent();

            this.DataContext = this;
            this.ConsoleList = new ObservableQueue<Util.Model.ConsoleData>(maxCapacity: MaxCapacity);
            this.FilteredConsoleList = new List<Util.Model.ConsoleData>();

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
            CalcFilterConsoleList();
        }

        void CalcFilterConsoleList()
        {
            if (HasFilterConditions == false)
            {
                this.FilteredConsoleList = new List<Util.Model.ConsoleData>(collection: this.ConsoleList);
            }
            else
            {
                this.FilteredConsoleList.Clear();

                var query = this.ConsoleList.AsQueryable();

                if (this.SelectedConsoleMsgType is Util.Model.ConsoleMsgType selectConsoleMsyType)
                {
                    query = query.Where(i => i.ConsoleMsgType == selectConsoleMsyType);
                }

                if (this.FilterStartDateTime.HasValue)
                {
                    query = query.Where(i => this.FilterStartDateTime.Value <= i.EntryTime);
                }

                if (this.FilterEndDateTime.HasValue)
                {
                    query = query.Where(i => i.EntryTime <= this.FilterEndDateTime.Value);
                }

                if (string.IsNullOrEmpty(this.ContentFilter) == false)
                {
                    query = query.Where(i => i.Content.Contains(this.ContentFilter));
                }

                var result = query.ToArray();

                this.FilteredConsoleList = new List<Util.Model.ConsoleData>(collection: result);
            }
        }

        public void Add(Util.Model.ConsoleData d)
        {
            this.ConsoleList.Enqueue(d);
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

        #region [弃用] Queue 无法轻易地获取选中或未选中项, 暂不实现下列功能

        //private void menuItemClearSelected(object sender, RoutedEventArgs e)
        //{
        //    if (listBox.SelectedItems == null || listBox.SelectedItems.Count <= 0)
        //    {
        //        return;
        //    }

        //    var dr = MessageBox.Show("确认清理[选中]项？", "提示", MessageBoxButton.YesNo);
        //    if (dr == MessageBoxResult.Yes)
        //    {
        //        List<int> toDelete = new List<int>();

        //        foreach (var item in listBox.SelectedItems)
        //        {
        //            toDelete.Add(this.ConsoleList.IndexOf(item));
        //        }

        //        foreach (int index in toDelete.OrderByDescending(i => i))
        //        {
        //            this.ConsoleList.RemoveAt(index);
        //        }
        //    }
        //}

        //private void menuItemClearUnSelected(object sender, RoutedEventArgs e)
        //{
        //    var dr = MessageBox.Show("确认清理[未选中]项？", "提示", MessageBoxButton.YesNo);
        //    if (dr == MessageBoxResult.Yes)
        //    {
        //        List<int> toRemain = new List<int>();
        //        List<int> toDelete = new List<int>();

        //        int total = this.ConsoleList.Count;

        //        foreach (var item in listBox.SelectedItems)
        //        {
        //            toRemain.Add(this.ConsoleList.IndexOf(item));
        //        }

        //        for (int index = total - 1; index > -1; index--)
        //        {
        //            if (toRemain.Contains(index) == false)
        //            {
        //                toDelete.Add(index);
        //            }
        //        }

        //        foreach (int index in toDelete)
        //        {                   
        //            this.ConsoleList.RemoveAt(index);
        //        }
        //    }
        //}

        #endregion

        private void menuItemClearAll(object sender, RoutedEventArgs e)
        {
            var dr = MessageBox.Show("确认清理[全部]？", "提示", MessageBoxButton.YesNo);
            if (dr == MessageBoxResult.Yes)
            {
                Clear();
            }
        }
        
        #region 过滤搜索条件 Advance 新增特性

        public List<object> ConsoleMsgTypeList { get; set; } = new List<object>()
        {
            string.Empty,
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

            CalcFilterConsoleList();
        }

        private object _SelectedConsoleMsgType;
        public object SelectedConsoleMsgType
        {
            get { return _SelectedConsoleMsgType; }
            set
            {
                _SelectedConsoleMsgType = value;

                this.OnPropertyChanged(nameof(SelectedConsoleMsgType));

                filterConditionNotify();
            }
        }

        private string _FilterStartDateTimeInfo;
        public string FilterStartDateTimeInfo
        {
            get { return _FilterStartDateTimeInfo; }
            set
            {
                _FilterStartDateTimeInfo = value;
                this.OnPropertyChanged(nameof(FilterStartDateTimeInfo));

                // TODO debounce
                if (string.IsNullOrEmpty(value) == false && DateTime.TryParse(value, out DateTime dt))
                {
                    FilterStartDateTime = dt;
                }
                else
                {
                    FilterStartDateTime = null;
                }
            }
        }



        private DateTime? _FilterStartDateTime;
        public DateTime? FilterStartDateTime
        {
            get { return _FilterStartDateTime; }
            set
            {
                _FilterStartDateTime = value;
                this.OnPropertyChanged(nameof(FilterStartDateTime));

                filterConditionNotify();
            }
        }



        private string _FilterEndDateTimeInfo;
        public string FilterEndDateTimeInfo
        {
            get { return _FilterEndDateTimeInfo; }
            set
            {
                _FilterEndDateTimeInfo = value;
                this.OnPropertyChanged(nameof(FilterEndDateTimeInfo));

                // TODO debounce
                if (string.IsNullOrEmpty(value) == false && DateTime.TryParse(value, out DateTime dt))
                {
                    FilterEndDateTime = dt;
                }
                else
                {
                    FilterEndDateTime = null;
                }
            }
        }


        private DateTime? _FilterEndDateTime;
        public DateTime? FilterEndDateTime
        {
            get { return _FilterEndDateTime; }
            set
            {
                _FilterEndDateTime = value;
                this.OnPropertyChanged(nameof(FilterEndDateTime));

                filterConditionNotify();
            }
        }



        private string _ContentFilter;
        public string ContentFilter
        {
            get { return _ContentFilter; }
            set
            {
                // TODO Debounce
                _ContentFilter = value;

                this.OnPropertyChanged(nameof(ContentFilter));

                filterConditionNotify();
            }
        }

        public bool HasFilterConditions
        {
            get
            {
                if
                (
                        this.SelectedConsoleMsgType is Util.Model.ConsoleMsgType
                    || this.FilterStartDateTime.HasValue == true
                    || this.FilterEndDateTime.HasValue == true
                    || string.IsNullOrEmpty(this.ContentFilter) == false

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
            this._SelectedConsoleMsgType = string.Empty;
            this._FilterStartDateTimeInfo = string.Empty;
            this._FilterStartDateTime = null;
            this._FilterEndDateTimeInfo = string.Empty;
            this._FilterEndDateTime = null;
            this._ContentFilter = string.Empty;

            this.OnPropertyChanged(nameof(SelectedConsoleMsgType));
            this.OnPropertyChanged(nameof(FilterStartDateTimeInfo));
            this.OnPropertyChanged(nameof(FilterStartDateTime));
            this.OnPropertyChanged(nameof(FilterEndDateTimeInfo));
            this.OnPropertyChanged(nameof(FilterEndDateTime));
            this.OnPropertyChanged(nameof(ContentFilter));

            this.filterConditionNotify();
        }


        #endregion

        #region 内部的 ObservableQueue<T> - 来自 Utils.HowesDOMO

        /// <summary>
        /// V 1.0.1 - 2021-02-22 17:14:00
        /// 修复 CollectionChanged 为 null 却执行通知事件
        /// 
        /// V 1.0.0 - 2021-01-28 17:00:00
        /// 首次创建
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class ObservableQueue<T> : Queue<T>, INotifyCollectionChanged
        {
            private int? _QueueMaxCapacity;
            /// <summary>
            /// 队列最大容量 ( 默认为空, 不设定最大容量 )
            /// </summary>
            public int? QueueMaxCapacity
            {
                get { return _QueueMaxCapacity; }
                set
                {
                    _QueueMaxCapacity = value;
                    fix();
                }
            }

            public ObservableQueue(int? maxCapacity = null)
            {
                QueueMaxCapacity = maxCapacity;
            }

            public ObservableQueue(int capacity, int? maxCapacity = null) : base(capacity)
            {
                QueueMaxCapacity = maxCapacity;
            }

            public ObservableQueue(IEnumerable<T> collection, int? maxCapacity = null) : base(collection)
            {
                QueueMaxCapacity = maxCapacity;
            }

            public event System.Collections.Specialized.NotifyCollectionChangedEventHandler CollectionChanged;

            public new void Clear()
            {
                base.Clear();
                if (this.CollectionChanged != null)
                {
                    this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                }
            }

            public new void Enqueue(T item)
            {
                base.Enqueue(item);
                fix();
                if (this.CollectionChanged != null)
                {
                    this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                }
            }

            public new T Dequeue()
            {
                T item = base.Dequeue();
                fix();
                if (this.CollectionChanged != null)
                {
                    this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                }
                return item;
            }

            /// <summary>
            /// 根据队列最大容量, 重新计算队列内的元素
            /// </summary>
            void fix()
            {
                if (QueueMaxCapacity.HasValue == false)
                {
                    return;
                }

                // 被移出队列的数量 = 当前数量 - 最大容量
                int toDequeueCount = this.Count - QueueMaxCapacity.Value;
                if (toDequeueCount <= 0)
                {
                    return;
                }

                for (int i = 0; i < toDequeueCount; i++)
                {
                    base.Dequeue();
                }

                if (this.CollectionChanged != null)
                {
                    this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                }
            }
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
