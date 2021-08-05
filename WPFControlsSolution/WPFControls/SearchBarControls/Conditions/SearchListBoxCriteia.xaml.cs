using System;

using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Client.Components.SearchBarControls
{
    /// <summary>
    /// SearchListBoxCriteia.xaml 的交互逻辑
    /// </summary>
    public partial class SearchListBoxCriteia : SearchCriteia
    {
        // TODO 待优化 双向绑定 SelectedItems, 无法初始化时指定选中某些项

        public SearchListBoxCriteia()
        {
            InitializeComponent();
            initEvent();
        }

        void initEvent()
        {
            this.listBox.SelectionChanged += ListBox_SelectionChanged;
        }

        private bool _viewHandled;
        private bool _modelHandled;

        /// <summary>
        /// 为 SelectedItems 属性赋值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_modelHandled == true)
            {
                return;
            }

            _viewHandled = true;
            
            if (this.listBox.SelectedItem != null)
            {
                var temp = new System.Collections.ArrayList();
                temp.AddRange(this.listBox.SelectedItems);
                this.SelectedItems = temp;
            }
            //else
            //{
            //    this.SelectedItems = null;
            //}

            _viewHandled = false;
        }

        #region [DP] DisplayMemberPath

        public static readonly DependencyProperty DisplayMemberPathProperty = DependencyProperty.Register
        (
            name: "DisplayMemberPath",
            propertyType: typeof(string),
            ownerType: typeof(SearchListBoxCriteia),
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
            ownerType: typeof(SearchListBoxCriteia),
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
            ownerType: typeof(SearchListBoxCriteia),
            validateValueCallback: null, // new ValidateValueCallback((toValidate) => { return true; }),
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
            ownerType: typeof(SearchListBoxCriteia),
            validateValueCallback: null, // new ValidateValueCallback((toValidate) => { return true; }),
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
            ownerType: typeof(SearchListBoxCriteia),
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
            ownerType: typeof(SearchListBoxCriteia),
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

        //#region [DP] SelectedItems

        //public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register
        //(
        //    name: "SelectedItems",
        //    propertyType: typeof(System.Collections.IList),
        //    ownerType: typeof(SearchListBoxCriteia)
        //);

        //public System.Collections.IList SelectedItems
        //{
        //    get { return (System.Collections.IList)GetValue(SelectedItemsProperty); }
        //    set { SetValue(SelectedItemsProperty, value); }
        //}

        //#endregion


        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register
        (
            name: "SelectedItems",
            propertyType: typeof(System.Collections.IList),
            ownerType: typeof(SearchListBoxCriteia),
            validateValueCallback: null, // new ValidateValueCallback((toValidate) => { return true; }),
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
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
            if (d is SearchListBoxCriteia target)
            {
                //if (target.IsInitialized)
                //{
                //    System.Diagnostics.Debugger.Break();
                //}

                if(target._viewHandled == true)
                {
                    return;
                }

                if (e.NewValue != null && e.NewValue is System.Collections.IList tempList && tempList.Count > 0)
                {
                    target._modelHandled = true;

                    foreach (var item in tempList)
                    {
                        target.listBox.SelectedItem = item;
                    }

                    target._modelHandled = false;
                }
            }
        }



    }
}