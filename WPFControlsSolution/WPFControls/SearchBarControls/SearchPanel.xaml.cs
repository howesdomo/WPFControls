using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Client.Components.SearchBarControls
{
    /// <summary>
    /// V 1.0.0 - 2021-08-23 11:03:07
    /// HowesDOMO 编写
    /// 
    /// </summary>
    public partial class SearchPanel : UserControl, System.ComponentModel.INotifyPropertyChanged
    {
        public const double PanelMaxWidth = 250d;
        public const double PanelMinWidth = 25d;

        public SearchPanel()
        {
            InitializeComponent();
            initEvent();
            initCMD();
            
        }

        private bool _MiniMode;
        public bool MiniMode
        {
            get { return _MiniMode; }
            set
            {
                _MiniMode = value;
                this.OnPropertyChanged(nameof(MiniMode));
            }
        }


        void initEvent()
        {
            this.SizeChanged += SearchPanel_SizeChanged;
        }

        private void SearchPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.IsInitialized == false)
            {
                return;
            }

            System.Diagnostics.Debug.WriteLine(e.NewSize);

            if (e.NewSize.Width <= 80d)
            {
                this.MiniMode = true;
            }
            else 
            {
                this.MiniMode = false;
            }
        }

        protected ObservableCollection<SearchCriteia> _searchCriterion = new ObservableCollection<SearchCriteia>();
        public ObservableCollection<SearchCriteia> SearchCriterion
        {
            get
            {
                return this._searchCriterion;
            }
        }

        #region [DP] ResetCommand

        public static readonly DependencyProperty ResetCommandProperty = DependencyProperty.Register
        (
            name: "ResetCommand",
            propertyType: typeof(object),
            ownerType: typeof(SearchPanel),
            validateValueCallback: null, // new ValidateValueCallback((toValidate) => { return true; }),
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: onResetCommand_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public object ResetCommand
        {
            get { return (object)GetValue(ResetCommandProperty); }
            set { SetValue(ResetCommandProperty, value); }
        }

        public static void onResetCommand_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SearchPanel target)
            {
                if (e.NewValue == null)
                {
                    target.btnReset.Command = null;
                    return;
                }

                target.btnReset.Command = (ICommand)e.NewValue;
            }
        }

        #endregion

        #region [DP] ResetCommandParameter


        public static readonly DependencyProperty ResetCommandParameterProperty = DependencyProperty.Register
        (
            name: "ResetCommandParameter",
            propertyType: typeof(object),
            ownerType: typeof(SearchPanel),
            validateValueCallback: null, // new ValidateValueCallback((toValidate) => { return true; }),
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: onResetCommandParameter_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public object ResetCommandParameter
        {
            get { return (object)GetValue(ResetCommandParameterProperty); }
            set { SetValue(ResetCommandParameterProperty, value); }
        }

        public static void onResetCommandParameter_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SearchPanel target)
            {
                target.btnReset.CommandParameter = e.NewValue;
            }
        }

        #endregion

        #region [DP] SearchCommand

        public static readonly DependencyProperty SearchCommandProperty = DependencyProperty.Register
        (
            name: "SearchCommand",
            propertyType: typeof(object),
            ownerType: typeof(SearchPanel),
            validateValueCallback: null, // new ValidateValueCallback((toValidate) => { return true; }),
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: onSearchCommand_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public object SearchCommand
        {
            get { return (object)GetValue(SearchCommandProperty); }
            set { SetValue(SearchCommandProperty, value); }
        }

        public static void onSearchCommand_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SearchPanel target)
            {
                if (e.NewValue == null)
                {
                    target.btnSearch.Command = null;
                    return;
                }

                target.btnSearch.Command = (ICommand)e.NewValue;
            }
        }

        #endregion

        #region [DP] SearchCommandParameter

        public static readonly DependencyProperty SearchCommandParameterProperty = DependencyProperty.Register
        (
            name: "SearchCommandParameter",
            propertyType: typeof(object),
            ownerType: typeof(SearchPanel),
            validateValueCallback: null, // new ValidateValueCallback((toValidate) => { return true; }),
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: onSearchCommandParameter_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public object SearchCommandParameter
        {
            get { return (object)GetValue(SearchCommandParameterProperty); }
            set { SetValue(SearchCommandParameterProperty, value); }
        }

        public static void onSearchCommandParameter_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SearchPanel target)
            {
                target.btnSearch.CommandParameter = e.NewValue;
            }
        }

        #endregion





        void initCMD()
        { 
        
        }

        

        #region INotifyPropertyChanged成员

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            this.PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
