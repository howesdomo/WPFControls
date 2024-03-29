﻿using System;
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
    /// V 1.0.1 - 2021-09-02 09:13:13
    /// 新增功能:
    /// 在 Title 的右下角添加数量汇总信息 ( {IsChecked数量} / {总数量} )
    /// 
    /// V 1.0.0 - 2021-08-25 17:56:14
    /// 重写并整理代码
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
            this.treeView.GetLatestTreeViewAdvInfoEvent += new EventHandler<EventArgs>(onHandle_GetLatestTreeViewAdvInfo);
        }

        void onHandle_GetLatestTreeViewAdvInfo(object o, EventArgs e)
        {
            this.txtInfo.Text = this.treeView.TreeViewAdvInfo;
        }

    }
}
