using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Client.Components.SearchPanelControls
{
    /// <summary>
    /// V 1.0.1 - 2021-09-01 17:05:22
    /// 新增功能:
    /// 在 Title 的右下角添加数量汇总信息
    /// 若 SelectedItems 对象是基于 System.Collections.Specialized.INotifyCollectionChanged 接口, 汇总信息 {选中数量} / {总数量}, 并具有选中更新信息
    /// 否则只显示 共 x 项
    /// 
    /// V 1.0.0 - 2021-08-25 17:56:14
    /// 重写并整理代码
    /// </summary>
    public partial class SearchConditionListBox : SearchConditionBase
    {
        #region [DP] ItemsSource ( 覆盖 SearchConditionBase 已有的依赖属性 )

        public static new readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register
        (
            name: "ItemsSource",
            propertyType: typeof(System.Collections.IEnumerable),
            ownerType: typeof(SearchConditionListBox),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: onItemsSource_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public new System.Collections.IEnumerable ItemsSource
        {
            get { return (System.Collections.IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static void onItemsSource_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SearchConditionListBox target)
            {
                target.mDebounceAction.Debounce
                (
                    interval: 500d,
                    action: target.calcTxtInfo,
                    dispatcher: target.Dispatcher
                );
            }
        }

        #endregion

        #region [DP] DisplayMemberPath

        public static readonly DependencyProperty DisplayMemberPathProperty = DependencyProperty.Register
        (
            name: "DisplayMemberPath",
            propertyType: typeof(string),
            ownerType: typeof(SearchConditionListBox),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public string DisplayMemberPath
        {
            get { return (string)GetValue(DisplayMemberPathProperty); }
            set { SetValue(DisplayMemberPathProperty, value); }
        }

        #endregion

        #region [DP] SelectionMode

        public static readonly DependencyProperty SelectionModeProperty = DependencyProperty.Register
        (
            name: "SelectionMode",
            propertyType: typeof(System.Windows.Controls.SelectionMode),
            ownerType: typeof(SearchConditionListBox),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: System.Windows.Controls.SelectionMode.Single,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public System.Windows.Controls.SelectionMode SelectionMode
        {
            get { return (System.Windows.Controls.SelectionMode)GetValue(SelectionModeProperty); }
            set { SetValue(SelectionModeProperty, value); }
        }

        #endregion

        #region [DP] ListBox_ScrollViewer_HorizontalScrollBarVisibility

        public static readonly DependencyProperty ListBox_ScrollViewer_HorizontalScrollBarVisibilityProperty = DependencyProperty.Register
        (
            name: "ListBox_ScrollViewer_HorizontalScrollBarVisibility",
            propertyType: typeof(ScrollBarVisibility),
            ownerType: typeof(SearchConditionListBox),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: ScrollBarVisibility.Auto,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public ScrollBarVisibility ListBox_ScrollViewer_HorizontalScrollBarVisibility
        {
            get { return (ScrollBarVisibility)GetValue(ListBox_ScrollViewer_HorizontalScrollBarVisibilityProperty); }
            set { SetValue(ListBox_ScrollViewer_HorizontalScrollBarVisibilityProperty, value); }
        }

        #endregion

        #region [DP] ListBox_ScrollViewer_VerticalScrollBarVisibility

        public static readonly DependencyProperty ListBox_ScrollViewer_VerticalScrollBarVisibilityProperty = DependencyProperty.Register
        (
            name: "ListBox_ScrollViewer_VerticalScrollBarVisibility",
            propertyType: typeof(ScrollBarVisibility),
            ownerType: typeof(SearchConditionListBox),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: ScrollBarVisibility.Auto,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public ScrollBarVisibility ListBox_ScrollViewer_VerticalScrollBarVisibility
        {
            get { return (ScrollBarVisibility)GetValue(ListBox_ScrollViewer_VerticalScrollBarVisibilityProperty); }
            set { SetValue(ListBox_ScrollViewer_VerticalScrollBarVisibilityProperty, value); }
        }

        #endregion

        #region [DP] ListBoxIsEnabled

        public static readonly DependencyProperty ListBoxIsEnabledProperty = DependencyProperty.Register
        (
            name: "ListBoxIsEnabled",
            propertyType: typeof(bool),
            ownerType: typeof(SearchConditionListBox),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: true,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public bool ListBoxIsEnabled
        {
            get { return (bool)GetValue(ListBoxIsEnabledProperty); }
            set { SetValue(ListBoxIsEnabledProperty, value); }
        }

        #endregion

        #region [DP] ListBoxItemTemplate

        public static readonly DependencyProperty ListBoxItemTemplateProperty = DependencyProperty.Register
        (
            name: "ListBoxItemTemplate",
            propertyType: typeof(DataTemplate),
            ownerType: typeof(SearchConditionListBox),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public DataTemplate ListBoxItemTemplate
        {
            get { return (DataTemplate)GetValue(ListBoxItemTemplateProperty); }
            set { SetValue(ListBoxItemTemplateProperty, value); }
        }

        #endregion

        #region [DP] SelectedItems - (重点) 支持选中多项

        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register
        (
            name: "SelectedItems",
            propertyType: typeof(System.Collections.IList),
            ownerType: typeof(SearchConditionListBox),
            validateValueCallback: null,
            typeMetadata: new FrameworkPropertyMetadata
            (
                defaultValue: null,
                flags: FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                propertyChangedCallback: onSelectedItemsProperty_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public System.Collections.IList SelectedItems
        {
            get { return (System.Collections.IList)GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }

        public static void onSelectedItemsProperty_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SearchConditionListBox target)
            {
                if (e.OldValue != null && e.OldValue is System.Collections.Specialized.INotifyCollectionChanged oldCollection)
                {
                    oldCollection.CollectionChanged -= target.onHandle_CollectionChanged;
                }

                if (e.NewValue != null && e.NewValue is System.Collections.Specialized.INotifyCollectionChanged newCollection)
                {
                    newCollection.CollectionChanged += target.onHandle_CollectionChanged;
                }
            }
        }

        void onHandle_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            mDebounceAction.Debounce
            (
                interval: 500d,
                action: calcTxtInfo,
                dispatcher: this.Dispatcher
            );
        }

        #endregion

        public SearchConditionListBox()
        {
            InitializeComponent();
        }

        public override void Reset()
        {
            if (this.SelectedItems != null)
            {
                this.SelectedItems.Clear();
            }
            else
            {
                this.listBox.SelectedItem = null;
            }
        }

        WPFControls.ActionUtils.DebounceAction mDebounceAction { get; set; } = new WPFControls.ActionUtils.DebounceAction();

        /// <summary>
        /// 计算数量
        /// </summary>
        void calcTxtInfo()
        {
            if (this.ItemsSource == null)
            {
                return;
            }

            int total = this.listBox.Items.Count;
            
            if 
            (
                this.SelectedItems != null && 
                this.SelectedItems is System.Collections.Specialized.INotifyCollectionChanged // 含有增加/删除事件通知
            )
            {
                txtInfo.Text = $"{this.SelectedItems.Count} / {total}";
            }
            else 
            {
                txtInfo.Text = $"共 {total} 项";
            }            
        }

    }
}