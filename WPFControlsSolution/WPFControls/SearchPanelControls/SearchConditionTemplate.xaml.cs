namespace Client.Components.SearchPanelControls
{
    /// <summary>
    /// SearchTextCriteia.xaml 的交互逻辑
    /// </summary>
    public partial class SearchConditionTemplate : SearchConditionBase
    {
        public SearchConditionTemplate()
        {
            InitializeComponent();            
        }

        //public object CriteiaControl
        //{
        //    get { return this.presenter.Content; }
        //    set { this.presenter.Content = value; }
        //}

        public object ContentPresenter
        {
            get { return this.presenter.Content; }
            set { this.presenter.Content = value; }
        }
    }
}
