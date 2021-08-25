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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections;

namespace Client.Components.SearchPanelControls
{
    /// <summary>
    /// V 1.0.2 - 2021-08-20 17:15:16
    /// TreeView 升级为 TreeViewAdv
    /// 
    /// V 1.0.1 - 2019-11-15 16:48:12 
    /// 修改人 Howe
    /// 修改 TreeViewHeight, TreeViewWidth 的 绑定, 由原来的 TreeViewHeight 对应 MinHeight 改为 Height
    /// 新增 TreeViewMinHeight 绑定 MinHeight
    /// 以上改动可以让使用者设定固定的 Width 和 Height, 或设置最小高度 (最小宽度(或宽度)一般跟随搜索助手)
    /// </summary>
    public partial class SearchConditionTreeView : SearchConditionBase
    {
        // Style 样式

        #region [DP] TreeViewWidth


        public static readonly DependencyProperty TreeViewWidthProperty = DependencyProperty.Register
        (
            name: "TreeViewWidth",
            propertyType: typeof(double),
            ownerType: typeof(SearchConditionTreeView),
            typeMetadata: new FrameworkPropertyMetadata
            (
                defaultValue: double.NaN
            )
        );

        public double TreeViewWidth
        {
            get { return (double)GetValue(TreeViewWidthProperty); }
            set { SetValue(TreeViewWidthProperty, value); }
        }

        #endregion

        #region [DP] TreeViewHeight



        public static readonly DependencyProperty TreeViewHeightProperty = DependencyProperty.Register
        (
            name: "TreeViewHeight",
            propertyType: typeof(double),
            ownerType: typeof(SearchConditionTreeView),
            typeMetadata: new FrameworkPropertyMetadata
            (
                defaultValue: double.NaN
            )
        );

        public double TreeViewHeight
        {
            get
            {
                return (double)GetValue(TreeViewHeightProperty);
            }
            set
            {
                SetValue(TreeViewHeightProperty, value);
            }
        }

        #endregion

        #region [DP] TreeViewMinHeight

        public static readonly DependencyProperty TreeViewMinHeightProperty = DependencyProperty.Register
        (
            name: "TreeViewMinHeight",
            propertyType: typeof(double),
            ownerType: typeof(SearchConditionTreeView),
            typeMetadata: new FrameworkPropertyMetadata
            (
                defaultValue: 150d
            )
        );


        public double TreeViewMinHeight
        {
            get { return (double)GetValue(TreeViewMinHeightProperty); }
            set { SetValue(TreeViewMinHeightProperty, value); }
        }

        #endregion

        #region [DP] TreeViewItemTemplate

        public static readonly DependencyProperty TreeViewItemTemplateProperty = DependencyProperty.Register
        (
            name: "TreeViewItemTemplate",
            propertyType: typeof(DataTemplate),
            ownerType: typeof(SearchConditionTreeView),
            validateValueCallback: null,
            typeMetadata: new FrameworkPropertyMetadata
            (
                defaultValue: null,
                flags: FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public DataTemplate TreeViewItemTemplate
        {
            get { return (DataTemplate)GetValue(TreeViewItemTemplateProperty); }
            set { SetValue(TreeViewItemTemplateProperty, value); }
        }

        #endregion

        #region [DP] ExpandedLevel

        public static readonly DependencyProperty ExpandedLevelProperty = DependencyProperty.Register
        (
            name: "ExpandedLevel",
            propertyType: typeof(int),
            ownerType: typeof(SearchConditionTreeView),
            validateValueCallback: new ValidateValueCallback((toValidate) =>
            {
                return toValidate != null && int.TryParse(toValidate.ToString(), out int level) && level >= 0;
            }),
            typeMetadata: new PropertyMetadata
            (
                defaultValue: 2,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public int ExpandedLevel
        {
            get { return (int)GetValue(ExpandedLevelProperty); }
            set { SetValue(ExpandedLevelProperty, value); }
        }

        #endregion

        #region [DP] IsCascade

        public static readonly DependencyProperty IsCascadeProperty = DependencyProperty.Register
        (
            name: "IsCascade",
            propertyType: typeof(bool),
            ownerType: typeof(SearchConditionTreeView),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: true,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public bool IsCascade
        {
            get { return (bool)GetValue(IsCascadeProperty); }
            set { SetValue(IsCascadeProperty, value); }
        }

        #endregion


        public static readonly DependencyProperty TreeViewIsEnabledProperty = DependencyProperty.Register
        (
            name: "TreeViewIsEnabled",
            propertyType: typeof(bool),
            ownerType: typeof(SearchConditionTreeView),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: true,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public bool TreeViewIsEnabled
        {
            get { return (bool)GetValue(TreeViewIsEnabledProperty); }
            set { SetValue(TreeViewIsEnabledProperty, value); }
        }


        public static readonly DependencyProperty DisplayMemberPathProperty = DependencyProperty.Register
        (
            name: "DisplayMemberPath",
            propertyType: typeof(string),
            ownerType: typeof(SearchConditionTreeView),
            validateValueCallback: null,
            typeMetadata: new FrameworkPropertyMetadata
            (
                defaultValue: null,
                flags: FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public string DisplayMemberPath
        {
            get { return (string)GetValue(DisplayMemberPathProperty); }
            set { SetValue(DisplayMemberPathProperty, value); }
        }

        #region [DP] TreeView_ScrollViewer_HorizontalScrollBarVisibility

        public static readonly DependencyProperty TreeView_ScrollViewer_HorizontalScrollBarVisibilityProperty = DependencyProperty.Register
        (
            name: "TreeView_ScrollViewer_HorizontalScrollBarVisibility",
            propertyType: typeof(ScrollBarVisibility),
            ownerType: typeof(SearchConditionTreeView),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: ScrollBarVisibility.Auto,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public ScrollBarVisibility TreeView_ScrollViewer_HorizontalScrollBarVisibility
        {
            get { return (ScrollBarVisibility)GetValue(TreeView_ScrollViewer_HorizontalScrollBarVisibilityProperty); }
            set { SetValue(TreeView_ScrollViewer_HorizontalScrollBarVisibilityProperty, value); }
        }

        #endregion

        #region [DP] TreeView_ScrollViewer_VerticalScrollBarVisibility

        public static readonly DependencyProperty TreeView_ScrollViewer_VerticalScrollBarVisibilityProperty = DependencyProperty.Register
        (
            name: "TreeView_ScrollViewer_VerticalScrollBarVisibility",
            propertyType: typeof(ScrollBarVisibility),
            ownerType: typeof(SearchConditionTreeView),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: ScrollBarVisibility.Auto,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public ScrollBarVisibility TreeView_ScrollViewer_VerticalScrollBarVisibility
        {
            get { return (ScrollBarVisibility)GetValue(TreeView_ScrollViewer_VerticalScrollBarVisibilityProperty); }
            set { SetValue(TreeView_ScrollViewer_VerticalScrollBarVisibilityProperty, value); }
        }

        #endregion




        // Data 数据

        // !! TreeViewAdv ItemsSourceOverride 能够自动创建树结构

        #region [DP] CheckedItems

        public static readonly DependencyProperty CheckedItemsProperty = DependencyProperty.Register
        (
            name: "CheckedItems",
            propertyType: typeof(IList),
            ownerType: typeof(SearchConditionTreeView),
            validateValueCallback: null,
            typeMetadata: new FrameworkPropertyMetadata
            (
                defaultValue: null,
                flags: FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public IList CheckedItems
        {
            get { return (IList)GetValue(CheckedItemsProperty); }
            set { SetValue(CheckedItemsProperty, value); }
        }

        #endregion

        #region [DP] CheckedItemsWithNull

        public static readonly DependencyProperty CheckedItemsWithNullProperty = DependencyProperty.Register
        (
            name: "CheckedItemsWithNull",
            propertyType: typeof(IList),
            ownerType: typeof(SearchConditionTreeView),
            validateValueCallback: null,
            typeMetadata: new FrameworkPropertyMetadata
            (
                defaultValue: null,
                flags: FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public IList CheckedItemsWithNull
        {
            get { return (IList)GetValue(CheckedItemsWithNullProperty); }
            set { SetValue(CheckedItemsWithNullProperty, value); }
        }

        #endregion        

        public SearchConditionTreeView()
        {
            InitializeComponent();
        }

    }
}
