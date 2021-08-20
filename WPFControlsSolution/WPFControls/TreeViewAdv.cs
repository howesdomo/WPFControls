﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Client.Components
{

    /// <summary>
    /// TreeViewAdv
    /// 
    /// V 1.0.0 - 2021-08-19 17:22:59
    /// 首次创建
    /// </summary>
    public class TreeViewAdv : System.Windows.Controls.TreeView, System.ComponentModel.INotifyPropertyChanged
    {
        public TreeViewAdv()
        {

        }

        public bool mIsUIEditing { get; set; }

        Type mThisType { get; set; }

        Type mItemsSourceOverrideGenericType { get; set; }

        #region [DP] ItemsSourceOverride

        public static readonly DependencyProperty ItemsSourceOverrideProperty = DependencyProperty.Register
        (
            name: "ItemsSourceOverride",
            propertyType: typeof(object),
            ownerType: typeof(TreeViewAdv),
            validateValueCallback: null, // new ValidateValueCallback((toValidate) => { return true; }),
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: onItemsSourceOverride_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public object ItemsSourceOverride
        {
            get { return (object)GetValue(ItemsSourceOverrideProperty); }
            set { SetValue(ItemsSourceOverrideProperty, value); }
        }

        public static void onItemsSourceOverride_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TreeViewAdv target)
            {
                var typeOfDataSource = e.NewValue.GetType();

                if (typeOfDataSource.IsGenericType == false)
                {
                    throw new ArgumentException("数据源必须为 IEnumerable<T> 。");
                }

                var genericArgArr = typeOfDataSource.GetGenericArguments(); // 获取泛型类型集合               

                target.mItemsSourceOverrideGenericType = genericArgArr[0]; // 获取数据源实际的泛型 T

                // Type typeOfTreeViewAdv = target.GetType();
                target.mThisType = target.GetType();
                var methodInfo = target.mThisType
                                .GetMethod
                                (
                                    name: "BuildTreeViewByItemsSouceOverride",
                                    bindingAttr: System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance // 加上能获取非公开的方法
                                )
                                .MakeGenericMethod(target.mItemsSourceOverrideGenericType); // 找出 SetItemsSouceOverride<T> 这个方法
                methodInfo.Invoke(target, new object[] { e.NewValue }); // 执行 SetItemsSouceOverride<T> 方法
            }
        }

        /// <summary>
        /// 根据 ItemsSourceOverride 创建
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataSource"></param>
        void BuildTreeViewByItemsSouceOverride<T>(IEnumerable<T> dataSource)
        {
            if (dataSource == null)
            {
                this.FlatList = null;
                this.ItemsSource = null;
                return;
            }

            var r = BuildTree<T>(dataSource);

            this.FlatList = r.Item2;
            this.ItemsSource = r.Item1;
        }

        int mSeq { get; set; } = 1;

        Tuple<System.Collections.ObjectModel.ObservableCollection<TreeViewItemModel<T>>, IEnumerable<TreeViewItemModel<T>>> BuildTree<T>(IEnumerable<T> dataSource)
        {
            mSeq = 1; // 重新初始化 mSeq

            List<TreeViewItemModel<T>> flatList = new List<TreeViewItemModel<T>>();

            var itemsSource = new System.Collections.ObjectModel.ObservableCollection<TreeViewItemModel<T>>();
            var matchRootList = dataSource.Where(i => i.GetType().GetProperty("ParentId").GetValue(i, null) == null);

            foreach (var item in matchRootList)
            {
                TreeViewItemModel<T> toAdd = digui<T>
                (
                    dataSource: dataSource,
                    item: item,
                    parent: null,
                    flatList: flatList,
                    level: 1,
                    isCascade: this.IsCascade
                );
                itemsSource.Add(toAdd);
                flatList.Add(toAdd);
            }

            return new Tuple<System.Collections.ObjectModel.ObservableCollection<TreeViewItemModel<T>>, IEnumerable<TreeViewItemModel<T>>>(itemsSource, flatList);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataSource"></param>
        /// <param name="item"></param>
        /// <param name="parent"></param>
        /// <param name="level"></param>
        /// <param name="isCascade"></param>
        /// <returns></returns>
        TreeViewItemModel<T> digui<T>(IEnumerable<T> dataSource, T item, TreeViewItemModel<T> parent, System.Collections.IList flatList, int level, bool isCascade)
        {
            dynamic dItem = item;

            TreeViewItemModel<T> toAdd = new TreeViewItemModel<T>(item);
            toAdd.Level = level;
            toAdd.Id = dItem.Id;
            toAdd.IsCascade = isCascade;
            toAdd.IsChecked = false;
            toAdd.Children = null;
            toAdd.Seq = mSeq++;

            if (level == 1)
            {
                toAdd.Parent = null;
                toAdd.ParentId = null;
            }
            else
            {
                toAdd.Parent = parent;
                toAdd.ParentId = parent.Id;
            }

            toAdd.PropertyChanged += TreeViewItemModel_PropertyChanged;

            var children = dataSource.Where(i => i.GetType().GetProperty("ParentId").GetValue(i, null) == dItem.Id);

            toAdd.IsBranch = false;
            foreach (var childItem in children)
            {
                if (toAdd.IsBranch == false)
                {
                    toAdd.Children = new System.Collections.ObjectModel.ObservableCollection<TreeViewItemModel<T>>();
                    toAdd.IsBranch = true;
                }

                var child = digui
                (
                    dataSource: dataSource,
                    item: childItem,
                    parent: toAdd,
                    flatList: flatList,
                    level: level + 1,
                    isCascade: isCascade
                );

                toAdd.Children.Add(child);
                flatList.Add(child);
            }

            return toAdd;
        }

        DebounceAction mDebounceAction { get; set; } = new DebounceAction();

        private void TreeViewItemModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsChecked")
            {
                System.Diagnostics.Debug.WriteLine($"IsChecked Changed and Debounce...");

                mIsUIEditing = true;

                // 减少由于级联导致的执行方法次数过多, 采用 Debounce 延迟执行的方式进行赋值
                mDebounceAction.Debounce
                (
                    interval: 50d,
                    action: () =>
                    {
                        System.Diagnostics.Debug.WriteLine($"IsChecked 值有修改, 执行 SelectedItems 赋值。执行时间：{DateTime.Now.ToString("s")}");

                        if (this.FlatList == null)
                        {
                            return;
                        }

                        #region 设置 SelectedItems

                        // 找出 GetSelectedItems<T> 这个方法
                        var methodInfo_GetSelectedItems =
                                        mThisType
                                        .GetMethod
                                        (
                                            name: "GetSelectedItems"
                                        // , bindingAttr: System.Reflection.BindingFlags.Public // [未能理解] 加上了反而找不到
                                        )
                                        .MakeGenericMethod(mItemsSourceOverrideGenericType);

                        SelectedItems = (System.Collections.IList)methodInfo_GetSelectedItems.Invoke(this, null);

                        #endregion

                        #region 设置 SelectedItemsWithNull                        

                        // 找出 GetSelectedItemsWithNull<T> 这个方法
                        var methodInfo_GetSelectedItemsWithNull =
                                        mThisType
                                        .GetMethod
                                        (
                                            name: "GetSelectedItemsWithNull"
                                        // , bindingAttr: System.Reflection.BindingFlags.Public// [未能理解] 加上了反而找不到
                                        )
                                        .MakeGenericMethod(mItemsSourceOverrideGenericType);
                        SelectedItemsWithNull = (System.Collections.IList)methodInfo_GetSelectedItemsWithNull.Invoke(this, null);

                        #endregion

                        mIsUIEditing = false;
                    },
                    dispatcher: Application.Current.Dispatcher
                );
            }
        }



        #endregion

        #region [DP] ExpandedLevel

        public static readonly DependencyProperty ExpandedLevelProperty = DependencyProperty.Register
        (
            name: "ExpandedLevel",
            propertyType: typeof(int),
            ownerType: typeof(TreeViewAdv),
            validateValueCallback: null, // new ValidateValueCallback((toValidate) => { return true; }),
            typeMetadata: new PropertyMetadata
            (
                defaultValue: 0,
                propertyChangedCallback: onExpandedLevel_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public int ExpandedLevel
        {
            get { return (int)GetValue(ExpandedLevelProperty); }
            set { SetValue(ExpandedLevelProperty, value); }
        }

        public static void onExpandedLevel_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TreeViewAdv target && e.NewValue is int level)
            {
                target.ExpandedInto(level);
            }
        }

        /// <summary>
        /// 展开到Level
        /// </summary>
        /// <param name="level"></param>
        void ExpandedInto(int level)
        {
            if (FlatList == null)
            {
                return;
            }

            this.FlatList.Where(i => i.Level <= level).ToList().ForEach(i => i.IsExpanded = true);
            this.FlatList.Where(i => i.Level > level).ToList().ForEach(i => i.IsExpanded = false);
        }


        #endregion

        #region [DP] IsCascade

        public static readonly DependencyProperty IsCascadeProperty = DependencyProperty.Register
        (
            name: "IsCascade",
            propertyType: typeof(bool),
            ownerType: typeof(TreeViewAdv),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: true,
                propertyChangedCallback: onIsCascade_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public bool IsCascade
        {
            get { return (bool)GetValue(IsCascadeProperty); }
            set { SetValue(IsCascadeProperty, value); }
        }

        public static void onIsCascade_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TreeViewAdv target && target.FlatList != null)
            {
                foreach (var item in target.FlatList)
                {
                    item.IsCascade = (bool)e.NewValue;
                }
            }
        }

        #endregion

        #region [DP] SelectedItems ( 集合只包含 IsChecked == true 的项 )

        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register
        (
            name: "SelectedItems",
            propertyType: typeof(System.Collections.IList),
            ownerType: typeof(TreeViewAdv),
            validateValueCallback: null,
            typeMetadata: new FrameworkPropertyMetadata
            (
                defaultValue: null,
                flags: FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                propertyChangedCallback: onSelectedItems_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public System.Collections.IList SelectedItems
        {
            get { return (System.Collections.IList)GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }

        public static void onSelectedItems_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TreeViewAdv target)
            {
                target.initSelectedItemsData(target, e);
            }
        }

        #endregion

        #region [DP] SelectedItemsWithNull ( 集合包含树干 IsChecked == null 的项 )

        public static readonly DependencyProperty SelectedItemsWithNullProperty = DependencyProperty.Register
        (
            name: "SelectedItemsWithNull",
            propertyType: typeof(System.Collections.IList),
            ownerType: typeof(TreeViewAdv),
            validateValueCallback: null,
            typeMetadata: new FrameworkPropertyMetadata
            (
                defaultValue: null,
                flags: FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                propertyChangedCallback: onSelectedItemsWithNull_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public System.Collections.IList SelectedItemsWithNull
        {
            get { return (System.Collections.IList)GetValue(SelectedItemsWithNullProperty); }
            set { SetValue(SelectedItemsWithNullProperty, value); }
        }

        public static void onSelectedItemsWithNull_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TreeViewAdv target)
            {
                target.initSelectedItemsData(target, e);
            }
        }

        #endregion

        /// <summary>
        /// 初始化 SelectedItems 选中数据
        /// </summary>
        /// <param name="target"></param>
        /// <param name="e"></param>
        void initSelectedItemsData(TreeViewAdv target, DependencyPropertyChangedEventArgs e)
        {
            if (target.mIsUIEditing)
            {
                return;
            }

            if (e.NewValue != null && target.FlatList != null)
            {
                // TODO 待测试大量数据时的效率
                bool is_Plan1_Enabled = false;
                if (is_Plan1_Enabled)
                {
                    #region 方案一 找到一个处理一个

                    foreach (var item in e.NewValue as System.Collections.IList)
                    {
                        var match = target.FlatList.FirstOrDefault(i => i.Model == item);
                        if (match != null)
                        {
                            if (match.IsChecked != true)
                            {
                                match.IsChecked = true;
                            }
                        }
                    }

                    #endregion
                }
                else
                {
                    #region 方案二 分开两部处理

                    // TODO 可以通过 Seq 排序 再次减少更新次数
                    // TODO ArrayList 没有办法使用 linq Seq 排序
                    var arrToUpdate = new System.Collections.ArrayList();

                    // 1. 先找出所有需要勾选的项
                    foreach (var item in e.NewValue as System.Collections.IList)
                    {
                        var match = target.FlatList.FirstOrDefault(i => i.Model == item);
                        if (match != null)
                        {
                            arrToUpdate.Add(match);
                        }
                    }

                    // 2. 一次过进行更新
                    foreach (dynamic item in arrToUpdate)
                    {
                        if (item.IsChecked != true)
                        {
                            item.IsChecked = true;
                        }
                    }

                    arrToUpdate.Clear();
                    arrToUpdate = null;

                    #endregion
                }
            }
        }


        public IEnumerable<T> GetSelectedItems<T>()
        {
            if (this.FlatList == null)
            {
                return null;
            }
            else
            {
                return this.FlatList.Where(i => i.IsChecked == true)
                                    .OrderBy(i => i.Seq) // 通过 Seq 排序出来的结果集更容易核对结果
                                    .Select(i => (T)i.Model)
                                    .ToList<T>();
            }
        }

        public IEnumerable<T> GetSelectedItemsWithNull<T>()
        {
            if (this.FlatList == null)
            {
                return null;
            }
            else
            {
                return this.FlatList.Where(i => i.IsChecked == true || i.IsChecked == null)
                                    .OrderBy(i => i.Seq) // 通过 Seq 排序出来的结果集更容易核对结果
                                    .Select(i => (T)i.Model)
                                    .ToList<T>();
            }
        }


        private IEnumerable<dynamic> _FlatList;
        /// <summary>
        /// 平面集合(树状结构的平面集合) 方便用于
        /// </summary>
        public IEnumerable<dynamic> FlatList
        {
            get { return _FlatList; }
            set
            {
                _FlatList = value;
                this.OnPropertyChanged(nameof(FlatList));
            }
        }

        /// <summary>
        /// 全选
        /// </summary>
        public void CheckAll()
        {
            if (this.FlatList == null)
            {
                return;
            }

            // Application.Current.MainWindow.Cursor = System.Windows.Input.Cursors.Wait;

            if (this.IsCascade == true)
            {
                this.FlatList.Where(i => i.Parent == null).ToList().ForEach(i => i.IsChecked = true);
            }
            else
            {
                this.FlatList.ToList().ForEach(i => i.IsChecked = true);
            }

            // Application.Current.MainWindow.Cursor = System.Windows.Input.Cursors.Arrow;
        }

        /// <summary>
        /// 反选
        /// </summary>
        public void ResverseCheckAll()
        {
            if (this.FlatList == null)
            {
                return;
            }

            // Application.Current.MainWindow.Cursor = System.Windows.Input.Cursors.Wait;

            if (this.IsCascade == true)
            {
                this.FlatList.Where(i => i.IsLeaf == true).ToList().ForEach(i => i.IsChecked = !i.IsChecked);
            }
            else
            {
                this.FlatList.ToList().ForEach(i => i.IsChecked = !i.IsChecked);
            }

            // Application.Current.MainWindow.Cursor = System.Windows.Input.Cursors.Arrow;
        }

        #region INotifyPropertyChanged成员

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            this.PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region [内部类] TreeViewItemModel

        public class TreeViewItemModel<T> : System.ComponentModel.INotifyPropertyChanged
        {
            public TreeViewItemModel(T model)
            {
                this.Model = model;
            }

            private T _Model;
            public T Model
            {
                get { return _Model; }
                private set
                {
                    _Model = value;
                    this.OnPropertyChanged(nameof(Model));
                }
            }

            private string _Id;
            public string Id
            {
                get { return _Id; }
                set
                {
                    _Id = value;
                    this.OnPropertyChanged(nameof(Id));
                }
            }

            private string _ParentId;
            public string ParentId
            {
                get { return _ParentId; }
                set
                {
                    _ParentId = value;
                    this.OnPropertyChanged(nameof(ParentId));
                }
            }

            private TreeViewItemModel<T> _Parent;
            public TreeViewItemModel<T> Parent
            {
                get { return _Parent; }
                set
                {
                    _Parent = value;
                    this.OnPropertyChanged(nameof(Parent));
                }
            }


            private bool _IsBranch;
            /// <summary>
            /// 是树枝
            /// </summary>
            public bool IsBranch
            {
                get { return _IsBranch; }
                set
                {
                    _IsBranch = value;
                    this.OnPropertyChanged(nameof(IsBranch));
                    this.OnPropertyChanged(nameof(IsLeaf));
                }
            }

            /// <summary>
            /// 是叶子
            /// </summary>
            public bool IsLeaf
            {
                get
                {
                    return !IsBranch;
                }
            }




            private int _Level;
            public int Level
            {
                get { return _Level; }
                set
                {
                    _Level = value;
                    this.OnPropertyChanged(nameof(Level));
                }
            }


            private int _Seq;
            /// <summary>
            /// 排列顺序
            /// 通过 Seq 排序出来的结果集更容易核对结果
            /// </summary>
            public int Seq
            {
                get { return _Seq; }
                set
                {
                    _Seq = value;
                    this.OnPropertyChanged(nameof(Seq));
                }
            }


            private bool _IsCascade = true;
            /// <summary>
            /// 级联
            /// </summary>
            public bool IsCascade
            {
                get { return _IsCascade; }
                set
                {
                    _IsCascade = value;
                    this.OnPropertyChanged(nameof(IsCascade));
                }
            }

            private bool? _IsChecked;
            public bool? IsChecked
            {
                get
                {
                    //if (this.Children.Count <= 0)
                    //{
                    //    return this.IsChecked;
                    //}

                    //if (!this.IsCascade)
                    //{
                    //    return base.IsChecked;
                    //}
                    //else
                    //{
                    //    return this.Children.IsChecked;
                    //}


                    return Calc_IsChecked;
                }
                set
                {
                    _IsChecked = value;


                    if (this.IsCascade == false)
                    {
                        this.OnPropertyChanged(nameof(IsChecked));
                        return;
                    }

                    if (value.HasValue)
                    {
                        if (value.Value == true)
                        {
                            digui_Parent_ToOn(this);
                            this.CheckChildern();
                        }
                        else
                        {
                            digui_Parent_ToOn(this);
                            this.UncheckChildern();
                        }
                    }

                    this.OnPropertyChanged(nameof(IsChecked));
                }
            }

            void digui_Parent_ToOn(TreeViewItemModel<T> m)
            {
                if (m.Parent != null)
                {
                    m.Parent?.OnPropertyChanged("IsChecked");
                    digui_Parent_ToOn(m.Parent);
                }
            }


            private bool _IsExpanded;
            public bool IsExpanded
            {
                get { return _IsExpanded; }
                set
                {
                    _IsExpanded = value;
                    this.OnPropertyChanged(nameof(IsExpanded));
                }
            }




            private System.Collections.ObjectModel.ObservableCollection<TreeViewItemModel<T>> _Children;
            public System.Collections.ObjectModel.ObservableCollection<TreeViewItemModel<T>> Children
            {
                get { return _Children; }
                set
                {
                    _Children = value;
                    this.OnPropertyChanged(nameof(Children));
                }
            }


            #region Childern

            public bool? Calc_IsChecked
            {
                get
                {
                    if (this.IsLeaf)
                    {
                        return this._IsChecked;
                    }

                    if (this.IsCascade == false)
                    {
                        return this._IsChecked;
                    }

                    int nullCount = 0;
                    int trueCount = 0;
                    int falseCount = 0;

                    foreach (var item in this.Children)
                    {
                        bool? temp = item.IsChecked;
                        if (!temp.HasValue)
                        {
                            nullCount++;
                        }
                        else if (temp.Value)
                        {
                            trueCount++;
                        }
                        else
                        {
                            falseCount++;
                        }
                    }

                    if (nullCount > 0)
                    {
                        return null;
                    }
                    else if (trueCount == this.Children.Count && trueCount > 0)
                    {
                        return true;
                    }
                    else if (falseCount == this.Children.Count)
                    {
                        return false;
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            protected virtual void CheckChildern()
            {
                if (this.Children != null)
                {
                    foreach (var item in this.Children)
                    {
                        item.IsChecked = true;
                    }
                }
            }

            protected virtual void UncheckChildern()
            {
                if (this.Children != null)
                {
                    foreach (var item in this.Children)
                    {
                        item.IsChecked = false;
                    }
                }
            }

            //protected virtual void SelectChildern()
            //{
            //    foreach (var item in this.Children)
            //    {
            //        item.IsOnSelect = true;
            //    }
            //}

            //protected virtual void UnselectChildern()
            //{
            //    foreach (var item in this.Children)
            //    {
            //        item.IsOnSelect = false;
            //    }
            //}

            #endregion

            #region INotifyPropertyChanged成员

            public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged(string propertyName = "")
            {
                this.PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }

            #endregion
        }

        #endregion

        #region [内部类] DebounceAction

        /// <summary>
        /// 连续的多次调用，只有在调用停止之后的一段时间内不再调用，然后才执行一次处理过程。
        /// </summary>
        class DebounceAction
        {
            System.Timers.Timer mDebounceTimer;

            /// <summary>
            ///  ( WPF 适用 ) 延迟指定时间后执行。 在此期间如果再次调用，则重新计时
            /// </summary>
            /// <param name="dispatcher"></param>
            public void Debounce(double interval, Action action, System.Windows.Threading.Dispatcher dispatcher = null)
            {
                lock (this)
                {
                    if (mDebounceTimer == null)
                    {
                        mDebounceTimer = new System.Timers.Timer(interval);
                        mDebounceTimer.AutoReset = false;
                        mDebounceTimer.Elapsed += (o, e) =>
                        {
                            mDebounceTimer.Stop();
                            mDebounceTimer.Close();
                            mDebounceTimer = null;

                            if (dispatcher != null && dispatcher.Thread.IsBackground == false)
                            {
                                dispatcher.Invoke(action, null);
                            }
                            else
                            {
                                action.Invoke();
                            }
                        };
                    }
                    mDebounceTimer.Stop();
                    mDebounceTimer.Start();
                }
            }

            /// <summary>
            /// ( Winform 适用 ) 延迟指定时间后执行。 在此期间如果再次调用，则重新计时
            /// </summary>
            /// <param name="syncInvoke">同步对象，一般为控件。 如不需同步可传null</param>
            public void Debounce(double interval, Action action, System.ComponentModel.ISynchronizeInvoke syncInvoke)
            {
                lock (this)
                {
                    if (mDebounceTimer == null)
                    {
                        mDebounceTimer = new System.Timers.Timer(interval);
                        mDebounceTimer.AutoReset = false;
                        mDebounceTimer.Elapsed += (o, e) =>
                        {
                            mDebounceTimer.Stop();
                            mDebounceTimer.Close();
                            mDebounceTimer = null;

                            if (syncInvoke != null && syncInvoke.InvokeRequired == true)
                            {
                                syncInvoke.Invoke(action, null);
                            }
                            else
                            {
                                action.Invoke();
                            }
                        };
                    }
                    mDebounceTimer.Stop();
                    mDebounceTimer.Start();
                }
            }
        }

        #endregion
        
    }
}