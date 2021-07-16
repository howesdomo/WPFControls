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

namespace Client.Components.SearchBarControls
{
    /// <summary>
    /// V 1.0.1 - 2019-11-15 16:48:12 
    /// 修改人 Howe
    /// 修改 TreeViewHeight, TreeViewWidth 的 绑定, 由原来的 TreeViewHeight 对应 MinHeight 改为 Height
    /// 新增 TreeViewMinHeight 绑定 MinHeight
    /// 以上改动可以让使用者设定固定的 Width 和 Height, 或设置最小高度 (最小宽度(或宽度)一般跟随搜索助手)
    /// </summary>
    public partial class SearchTreeViewCriteia : SearchCriteia
    {
        public static readonly DependencyProperty CheckBoxVisibilityProperty = DependencyProperty.Register
        (
            name: "CheckBoxVisibility",
            propertyType: typeof(Visibility),
            ownerType: typeof(SearchTreeViewCriteia),
            typeMetadata: new FrameworkPropertyMetadata
            (
                defaultValue: Visibility.Hidden
            )
        );

        public Visibility CheckBoxVisibility
        {
            get
            {
                return (Visibility)GetValue(CheckBoxVisibilityProperty);
            }
            set
            {
                SetValue(CheckBoxVisibilityProperty, value);
            }
        }

        public static readonly DependencyProperty TreeViewHeightProperty = DependencyProperty.Register
        (
            name: "TreeViewHeight",
            propertyType: typeof(double),
            ownerType: typeof(SearchTreeViewCriteia),
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

        public static readonly DependencyProperty TreeViewWidthProperty = DependencyProperty.Register
        (
            name: "TreeViewWidth",
            propertyType: typeof(double),
            ownerType: typeof(SearchTreeViewCriteia),
            typeMetadata: new FrameworkPropertyMetadata
            (
                defaultValue: double.NaN
            )
        );

        public double TreeViewWidth
        {
            get
            {
                return (double)GetValue(TreeViewWidthProperty);
            }
            set
            {
                SetValue(TreeViewWidthProperty, value);
            }
        }

        public static readonly DependencyProperty TreeViewMinHeightProperty = DependencyProperty.Register
        (
            name: "TreeViewMinHeight",
            propertyType: typeof(double),
            ownerType: typeof(SearchTreeViewCriteia),
            typeMetadata: new FrameworkPropertyMetadata
            (
                defaultValue: 150d
            )
        );

        public double TreeViewMinHeight
        {
            get
            {
                return (double)GetValue(TreeViewMinHeightProperty);
            }
            set
            {
                SetValue(TreeViewMinHeightProperty, value);
            }
        }

        public SearchTreeViewCriteia()
        {
            InitializeComponent();
        }

        //public static readonly DependencyProperty TreeViewWidthProperty = DependencyProperty.Register
        //(
        //    name: "TreeViewWidth",
        //    propertyType: typeof(double),
        //    ownerType: typeof(SearchTreeViewCriteia),
        //    typeMetadata: new FrameworkPropertyMetadata
        //    (
        //        defaultValue: Double.NaN,
        //        flags: FrameworkPropertyMetadataOptions.AffectsRender,
        //        propertyChangedCallback: new PropertyChangedCallback(treeViewWidthPropertyChangedHandle)
        //    )
        //);

        //private static void treeViewWidthPropertyChangedHandle(System.Windows.DependencyObject d, System.Windows.DependencyPropertyChangedEventArgs e)
        //{

        //}
    }
}
