﻿using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Collections;
using System;
using System.Linq;

namespace Client.Components
{
    /// <summary>
    /// StandardGridView.xaml 的交互逻辑
    /// </summary>
    public partial class StandardDataGridView : UserControl
    {
        // TODO 开放 AutoGenerateColumns 给我使用

        public const int DebugMode = 1;

        public StandardDataGridView()
        {
            InitializeComponent();
            initEvent();

        }

        void initEvent()
        {
            //this._columns = new ObservableCollection<DataGridColumn>();
            this._columns.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Columns_CollectionChanged);

            this.DataGrid.CurrentCellChanged += DataGrid_CurrentCellChanged;
            this.DataGrid.SelectedCellsChanged += DataGrid_SelectedCellsChanged;
        }

        protected void Columns_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (DataGridColumn item in e.NewItems)
                {
                    this.dataGrid.Columns.Add(item);
                }
            }

            if (e.OldItems != null)
            {
                foreach (DataGridColumn item in e.OldItems)
                {
                    this.dataGrid.Columns.Remove(item);
                }
            }

        }


        private void DataGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            if (DebugMode > 0)
            {
                System.Diagnostics.Debug.WriteLine($"{DateTime.Now.ToString("mm:ss.fffffff")} - CurrentCellChanged");
                // 测试结果 先 DataGrid_CurrentCellChanged, 后 DataGrid_SelectedCellsChanged
            }

            this.SelectedCell = this.DataGrid.CurrentCell;

            switch (DataGridSelectMode)
            {
                case DataGridSelectMode.Row:
                    {
                        // Nothing To Do
                    }
                    break;
                case DataGridSelectMode.Rows:
                    {
                        getSelectedItem();
                    }
                    break;
                case DataGridSelectMode.Cell:
                    {
                        if (this.DataGrid.CurrentCell != null && this.DataGrid.CurrentCell.Column != null)
                        {
                            this.SelectedCells = DataGrid.SelectedCells;
                            this.SelectedItems = new ArrayList() { this.DataGrid.CurrentItem };
                            this.SelectedItem = this.DataGrid.CurrentItem;
                        }
                    }
                    break;
                case DataGridSelectMode.Cells:
                    {
                        getSelectedItems();

                        getSelectedItem();
                    }
                    break;
                default:
                    break;
            }
        }

        void getSelectedItem()
        {
            // if (this.DataGrid.CurrentCell != null && this.DataGrid.CurrentCell.Column != null)
            if (this.DataGrid.CurrentCell != null && this.DataGrid.CurrentCell.IsValid == true)
            {
                this.SelectedItem = this.DataGrid.CurrentCell.Item;
            }
            else
            {
                // this.SelectedItem = null;
            }
        }

        void getSelectedItems()
        {
            ArrayList r = null;

            if (this.SelectedCells != null)
            {
                var addList = new System.Collections.Generic.List<object>();

                System.Collections.Generic.List<int> hashCodeList = new System.Collections.Generic.List<int>();

                for (int i = 0; i < this.SelectedCells.Count; i++)
                {
                    int hashCode = this.SelectedCells[i].Item.GetHashCode();
                    if (hashCodeList.Any(q => q == hashCode) == false)
                    {
                        hashCodeList.Add(hashCode);
                        addList.Add(this.SelectedCells[i].Item);
                    }
                }

                if (hashCodeList.Count > 0)
                {
                    r = new ArrayList();
                    foreach (var item in addList)
                    {
                        r.Add(item);
                    }
                }
            }

            this.SelectedItems = r;
        }

        class ObjComparer : System.Collections.Generic.IComparer<object>
        {
            public int Compare(object x, object y)
            {
                if (x != null && y != null)
                {
                    int a = x.GetHashCode();
                    int b = y.GetHashCode();

                    return a.CompareTo(b);
                }
                else if (x == null)
                {
                    return -1;
                }
                else // if (y == null)
                {
                    return 1;
                }
            }
        }

        // 考泰斯项目专用 -- 由于 KMMS 项目大量运用了 Selector.SelectionChanged 事件来 监测 DataGridItem 的改变
        // 弃用
        [Obsolete]
        private void DataGrid_Selector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.SelectedItems = this.DataGrid.SelectedItems;

            this.SelectedCells = this.DataGrid.SelectedCells;

            this.SelectedItem = this.DataGrid.SelectedItem;

            //if (this.ItemsSource is IBaseCollection) // 更新 IBaseCollection 的 SelectedItem
            //{
            //    this.ItemsSource.GetType().InvokeMember("SelectedItem", BindingFlags.SetProperty, null, this.ItemsSource, new object[] { this.SelectedItem });
            //}
        }

        private void DataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (DebugMode > 0)
            {
                System.Diagnostics.Debug.WriteLine($"{DateTime.Now.ToString("mm:ss.fffffff")} - SelectedCellsChanged");
                // 测试结果 先 DataGrid_CurrentCellChanged, 后 DataGrid_SelectedCellsChanged
            }

            ////// 先简化赋值逻辑, 虽然 SelectedItems 与 SelectedCells 他们引用的地址是一样的, 但也对其重新赋值
            ////if (this.DataGrid.SelectedItems == null)
            ////{
            ////    this.SelectedItems = null;
            ////}
            ////else if (this.SelectedItems == null || this.SelectedItems != this.DataGrid.SelectedItems)
            ////{
            ////    // 进行赋值的条件
            ////    // 1. DataGrid.SelectedItems 非空 
            ////    // 2-A DP SelectedItems 为空
            ////    // 2-B DP SelectedItems 引用地址 与 DataGrid.SelectedItems 地址不同
            ////    this.SelectedItems = this.DataGrid.SelectedItems;

            ////    // 又由于 SelectedItems 的赋值频率低, 
            ////    // 所以先进行 SelectedItems 的赋值, 再进行 SelectedItem 的赋值, 
            ////    // 在 Selecteditem 处通知界面相关 SelectedItems 的属性进行刷新
            ////}

            switch (DataGridSelectMode)
            {
                case DataGridSelectMode.Row:
                    {
                        this.SelectedCells = this.DataGrid.SelectedCells;
                        this.SelectedItems = this.DataGrid.SelectedItems;
                        this.SelectedItem = this.DataGrid.SelectedItem;
                    }
                    break;
                case DataGridSelectMode.Rows:
                    {
                        this.SelectedItems = this.DataGrid.SelectedItems;
                        this.SelectedCells = this.DataGrid.SelectedCells;
                    }
                    break;
                case DataGridSelectMode.Cell:
                    {
                        this.SelectedCells = DataGrid.SelectedCells;

                        this.SelectedItem = this.DataGrid.CurrentItem;
                    }
                    break;
                case DataGridSelectMode.Cells:
                    {
                        this.SelectedCells = this.DataGrid.SelectedCells;

                        getSelectedItems();
                    }
                    break;
                default:
                    break;
            }

            //if (this.ItemsSource is IBaseCollection) // 更新 IBaseCollection 的 SelectedItem
            //{
            //    this.ItemsSource.GetType().InvokeMember("SelectedItem", BindingFlags.SetProperty, null, this.ItemsSource, new object[] { this.SelectedItem });
            //}
        }

        public DataGrid DataGrid
        {
            get
            {
                return this.dataGrid;
            }
        }

        public ResourceDictionary DataGridResources
        {
            get
            {
                return this.DataGrid.Resources;
            }
            set
            {
                if (DataGrid.Resources.Count <= 0)
                {
                    this.DataGrid.Resources = value;
                }
                else
                {
                    // 因为 XAML 中创建 ResourceDictionary 先将一个 null 传过来,
                    // 故以下代码是将原有的 Resources 先拷贝到新赋值过来的 ResourceDictionary 中
                    foreach (object keyObj in this.DataGrid.Resources.Keys)
                    {
                        var v = this.DataGrid.Resources[keyObj.ToString()];
                        value.Add(keyObj, v);
                    }

                    this.DataGrid.Resources = value;
                }
            }
        }


        #region RowHeaderStyle


        public static readonly DependencyProperty RowHeaderStyleProperty = DependencyProperty.Register("RowHeaderStyle", typeof(Style), typeof(StandardDataGridView)
            , new PropertyMetadata(null, OnRowHeaderStyleChanged));

        private static void OnRowHeaderStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            StandardDataGridView view = (StandardDataGridView)d;
            view.dataGrid.RowHeaderStyle = e.NewValue as Style;
        }

        public Style RowHeaderStyle
        {
            get
            {
                return (Style)GetValue(RowHeaderStyleProperty);
            }
            set
            {
                SetValue(RowHeaderStyleProperty, value);
            }
        }

        #endregion

        #region Description ( 标题栏 )

        #region 标题信息

        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register("Description", typeof(string), typeof(StandardDataGridView));
        public string Description
        {
            get
            {
                return (string)GetValue(DescriptionProperty);
            }
            set
            {
                SetValue(DescriptionProperty, value);
            }
        }

        #endregion


        //<!--  为了能够使用 资源的 Style, 这些属性不直接绑定到控件上, 采用 PropertyChanged 的方式复制到控件上  -->
        //<!--
        //    FontSize="{Binding ElementName=Form, Path=DescriptionFontSize}"
        //    FontStyle="{Binding ElementName=Form, Path=DescriptionFontStyle}"
        //    FontWeight="{Binding ElementName=Form, Path=DescriptionFontWeight}"
        //    Foreground="{Binding ElementName=Form, Path=DescriptionForeground}"
        //-->

        #region 标题FontSize

        public static readonly DependencyProperty DescriptionFontSizeProperty = DependencyProperty.Register
        (
            name: "DescriptionFontSize",
            propertyType: typeof(double?),
            ownerType: typeof(StandardDataGridView),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: onDescriptionFontSize_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public double? DescriptionFontSize
        {
            get { return (double?)GetValue(DescriptionFontSizeProperty); }
            set { SetValue(DescriptionFontSizeProperty, value); }
        }

        private static void onDescriptionFontSize_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((d is StandardDataGridView) == false) { return; }

            var target = d as StandardDataGridView;
            if (e.NewValue != null && double.TryParse(e.NewValue.ToString(), out double fontSize) && fontSize > 0)
            {
                target.txtDescription.FontSize = fontSize;
            }
        }

        #endregion

        #region 标题Foreground

        public static readonly DependencyProperty DescriptionForegroundProperty = DependencyProperty.Register
        (
            name: "DescriptionForeground",
            propertyType: typeof(System.Windows.Media.Brush),
            ownerType: typeof(StandardDataGridView),
            validateValueCallback: new ValidateValueCallback((toValidate) => { return toValidate == null || toValidate is System.Windows.Media.Brush; }),
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: onDescriptionForeground_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public System.Windows.Media.Brush DescriptionForeground
        {
            get { return (System.Windows.Media.Brush)GetValue(DescriptionForegroundProperty); }
            set { SetValue(DescriptionForegroundProperty, value); }
        }

        private static void onDescriptionForeground_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((d is StandardDataGridView) == false) { return; }

            var target = d as StandardDataGridView;
            if (e.NewValue != null)
            {
                target.txtDescription.Foreground = e.NewValue as System.Windows.Media.Brush;
            }
        }

        #endregion

        #region 标题FontStyle

        public static readonly DependencyProperty DescriptionFontStyleProperty = DependencyProperty.Register
        (
            name: "DescriptionFontStyle",
            propertyType: typeof(FontStyle?),
            ownerType: typeof(StandardDataGridView),
            validateValueCallback: new ValidateValueCallback((toValidate) => { return toValidate == null || toValidate is FontStyle; }),
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: onDescriptionFontStyle_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public FontStyle? DescriptionFontStyle
        {
            get { return (FontStyle?)GetValue(DescriptionFontStyleProperty); }
            set { SetValue(DescriptionFontStyleProperty, value); }
        }

        private static void onDescriptionFontStyle_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((d is StandardDataGridView) == false) { return; }

            var target = d as StandardDataGridView;
            if (e.NewValue != null)
            {
                target.txtDescription.FontStyle = ((FontStyle?)e.NewValue).Value;
            }
        }

        #endregion

        #region 标题FontWeight


        public static readonly DependencyProperty DescriptionFontWeightProperty = DependencyProperty.Register
        (
            name: "DescriptionFontWeight",
            propertyType: typeof(FontWeight?),
            ownerType: typeof(StandardDataGridView),
            validateValueCallback: null, // new ValidateValueCallback((toValidate) => { return true; }),
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: onDescriptionFontWeight_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public FontWeight? DescriptionFontWeight
        {
            get { return (FontWeight?)GetValue(DescriptionFontWeightProperty); }
            set { SetValue(DescriptionFontWeightProperty, value); }
        }

        private static void onDescriptionFontWeight_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((d is StandardDataGridView) == false) { return; }

            var target = d as StandardDataGridView;
            if (e.NewValue != null)
            {
                target.txtDescription.FontWeight = ((FontWeight?)e.NewValue).Value;
            }
        }

        #endregion

        #endregion

        protected ObservableCollection<DataGridColumn> _columns = new ObservableCollection<DataGridColumn>();
        public ObservableCollection<DataGridColumn> Columns
        {
            get { return this._columns; }
        }

        #region [DP] AutoGenerateColumns

        public static readonly DependencyProperty AutoGenerateColumnsProperty = DependencyProperty.Register
        (
            name: "AutoGenerateColumns",
            propertyType: typeof(bool),
            ownerType: typeof(StandardDataGridView),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: false,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public bool AutoGenerateColumns
        {
            get { return (bool)GetValue(AutoGenerateColumnsProperty); }
            set { SetValue(AutoGenerateColumnsProperty, value); }
        }

        #endregion





        #region [DP] ItemsSource

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register
        (
            name: "ItemsSource",
            propertyType: typeof(IEnumerable),
            ownerType: typeof(StandardDataGridView),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: onItemsSource_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static void onItemsSource_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is StandardDataGridView target)
            {
                target.SelectedItem = null;
                target.SelectedItems = null;
            }
        }

        #endregion

        #region SelectedItem[DP]

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register
        (
            "SelectedItem",
            typeof(object),
            typeof(StandardDataGridView)
        );

        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        #endregion

        #region SelectedItems[DP]

        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register
        (
            name: "SelectedItems",
            propertyType: typeof(IList),
            ownerType: typeof(StandardDataGridView)
        );

        public IList SelectedItems
        {
            get { return (IList)GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }

        #endregion

        #region SelectedCell[DP]

        public static readonly DependencyProperty SelectedCellProperty = DependencyProperty.Register
        (
            name: "SelectedCell",
            propertyType: typeof(DataGridCellInfo),
            ownerType: typeof(StandardDataGridView)
        );

        public DataGridCellInfo SelectedCell
        {
            get { return (DataGridCellInfo)GetValue(SelectedCellProperty); }
            set { SetValue(SelectedCellProperty, value); }
        }

        #endregion

        #region SelectedCells[DP]

        public static readonly DependencyProperty SelectedCellsProperty = DependencyProperty.Register
        (
            name: "SelectedCells",
            propertyType: typeof(System.Collections.Generic.IList<DataGridCellInfo>),
            ownerType: typeof(StandardDataGridView)
        );

        public System.Collections.Generic.IList<DataGridCellInfo> SelectedCells
        {
            get { return (System.Collections.Generic.IList<DataGridCellInfo>)GetValue(SelectedCellsProperty); }
            set { SetValue(SelectedCellsProperty, value); }
        }

        #endregion

        #region DataGridSelectMode[DP] DataGrid选择模式

        public static readonly DependencyProperty DataGridSelectModeProperty = DependencyProperty.Register
        (
            name: "DataGridSelectMode",
            propertyType: typeof(DataGridSelectMode),
            ownerType: typeof(StandardDataGridView),
            validateValueCallback: new ValidateValueCallback((toValidate) => { return toValidate is DataGridSelectMode; }),
            typeMetadata: new PropertyMetadata
            (
                defaultValue: DataGridSelectMode.Row,
                propertyChangedCallback: onDataGridSelectMode_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public DataGridSelectMode DataGridSelectMode
        {
            get { return (DataGridSelectMode)GetValue(DataGridSelectModeProperty); }
            set { SetValue(DataGridSelectModeProperty, value); }
        }

        public static void onDataGridSelectMode_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((d is StandardDataGridView) == false) { return; }
            var target = d as StandardDataGridView;
            DataGridSelectMode value = (DataGridSelectMode)e.NewValue;
            switch (value)
            {
                case DataGridSelectMode.Cell:
                    target.SetDataGridStyle_SelectedCell();
                    break;
                case DataGridSelectMode.Cells:
                    target.SetDataGridStyle_SelectedCells();
                    break;
                case DataGridSelectMode.Rows:
                    target.SetDetailDataGridStyle_SelectedRows();
                    break;
                case DataGridSelectMode.Row:
                default:
                    target.SetDetailDataGridStyle_SelectedRow();
                    break;
            }
        }

        private void SetDataGridStyle_SelectedCell()
        {
            // 由于 DataGrid.SelectionUnit 需要设置为 Cell, 但设置后若对DataGrid的SelectedItem进行设置会报错, 故取消 SelectedItem 的绑定
            // System.Windows.Data.BindingOperations.ClearBinding(this.DataGrid, DataGrid.SelectedItemProperty);

            this.DataGrid.SelectionUnit = DataGridSelectionUnit.Cell;
            this.DataGrid.SelectionMode = DataGridSelectionMode.Single;
            // this.setSelectedCellStyle(this.DataGrid);
        }

        private void SetDataGridStyle_SelectedCells()
        {
            // 由于 DataGrid.SelectionUnit 需要设置为 Cell, 但设置后若对DataGrid的SelectedItem进行设置会报错, 故取消 SelectedItem 的绑定
            // System.Windows.Data.BindingOperations.ClearBinding(this.DataGrid, DataGrid.SelectedItemProperty);

            this.DataGrid.SelectionUnit = DataGridSelectionUnit.Cell;
            this.DataGrid.SelectionMode = DataGridSelectionMode.Extended;
            // this.setSelectedCellStyle(this.DataGrid);
        }

        private void SetDetailDataGridStyle_SelectedRows()
        {
            this.DataGrid.SelectionUnit = DataGridSelectionUnit.FullRow;
            this.DataGrid.SelectionMode = DataGridSelectionMode.Extended;
        }

        private void SetDetailDataGridStyle_SelectedRow()
        {
            this.DataGrid.SelectionUnit = DataGridSelectionUnit.FullRow;
            this.DataGrid.SelectionMode = DataGridSelectionMode.Single;
        }

        private void setSelectedCellStyle(DataGrid dg)
        {
            var matchResource = TryFindResource("SelectCellStyle");

            if (matchResource == null)
            {
                throw new Exception("找不到SelectCellStyle");
            }
            else
            {
                Style matchStyle = matchResource as Style;
                dg.CellStyle = matchStyle;
            }
        }

        #endregion

        public void CellBeginEdit(int colIndex, int rowIndex)
        {
            this.dataGrid.SelectedIndex = rowIndex;
            //this.dataGrid.ScrollIntoView(this.grid.SelectedItem, this.grid.Columns[colIndex]);

            DataGridCell cell = this.dataGrid.GetGridCell(rowIndex, colIndex);
            if (cell != null)
            {
                cell.Focus();
            }

            this.dataGrid.BeginEdit();
        }

        public void CellBeginEdit(int colIndex)
        {
            this.CellBeginEdit(colIndex, this.dataGrid.Items.Count - 1);
        }

        #region Grid 开启右键菜单 - Add By Howe

        public bool GridRightClickMenuEnabled
        {
            get
            {
                return this.DataGrid.ContextMenu != null;
            }
            set
            {
                bool temp = value;
                if (temp == true)
                {
                    this.DataGrid.SetRightClickMenu();
                }
                else
                {
                    this.DataGrid.ContextMenu?.ClearValue(DataGrid.ContextMenuProperty);
                }
            }
        }

        #endregion Grid 开启右键菜单 - Add By Howe


    }



}
