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
using System.Collections.ObjectModel;

namespace Client.Components.SearchBarControls
{
    /// <summary>
    /// SearchHelper.xaml 的交互逻辑
    /// </summary>
    public partial class SearchHelper : UserControl
    {
        public SearchHelper()
        {
            InitializeComponent();
        }

        public event EventHandler<EventArgs> Search
        {
            add
            {
                this.panel.Search += value;
            }
            remove
            {
                this.panel.Search -= value;
            }
        }

        #region SearchCommand

        public static readonly DependencyProperty SearchCommandProperty = DependencyProperty.Register
        (
            name: "SearchCommand",
            propertyType: typeof(ICommand),
            ownerType: typeof(SearchHelper),
            validateValueCallback: new ValidateValueCallback((target) => { return target is null || target is ICommand; }),
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: SearchCommandProperty_onPropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public ICommand SearchCommand
        {
            get { return (ICommand)GetValue(SearchCommandProperty); }
            set { SetValue(SearchCommandProperty, value); }
        }

        public static void SearchCommandProperty_onPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (d as SearchHelper);
            if (e.NewValue == null)
            {
                target.panel.SetBtnSearchCommand(null);
            }
            else
            {
                ICommand toUpdate = e.NewValue as ICommand;
                target.panel.SetBtnSearchCommand(toUpdate);
            }
        }

        #endregion


        public event EventHandler<EventArgs> Reset
        {
            add
            {
                this.panel.Reset += value;
            }
            remove
            {
                this.panel.Reset -= value;
            }
        }

        #region ResetCommand

        public static readonly DependencyProperty ResetCommandProperty = DependencyProperty.Register
        (
            name: "ResetCommand",
            propertyType: typeof(ICommand),
            ownerType: typeof(SearchHelper),
            validateValueCallback: new ValidateValueCallback((target) => { return target is null || target is ICommand; }),
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: ResetCommandProperty_onPropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public ICommand ResetCommand
        {
            get { return (ICommand)GetValue(ResetCommandProperty); }
            set { SetValue(ResetCommandProperty, value); }
        }

        public static void ResetCommandProperty_onPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (d as SearchHelper);
            if (e.NewValue == null)
            {
                target.panel.SetBtnResetCommand(null);
            }
            else
            {
                ICommand toUpdate = e.NewValue as ICommand;
                target.panel.SetBtnResetCommand(toUpdate);
            }
        }

        #endregion

        public ObservableCollection<SearchCriteia> SearchCriterion
        {
            get
            {
                return this.panel.SearchCriterion;
            }
        }


    }
}
