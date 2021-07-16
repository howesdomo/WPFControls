using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Collections;
using System.Collections.ObjectModel;

namespace Client.Components.SearchBarControls
{
    public class Searcher : UserControl
    {

        protected ObservableCollection<SearchCriteia> _searchCriterion = new ObservableCollection<SearchCriteia>();
        public ObservableCollection<SearchCriteia> SearchCriterion
        {
            get
            {
                return this._searchCriterion;
            }
        }


        public event EventHandler<EventArgs> Search;
        public event EventHandler<EventArgs> Reset;

        public Searcher()
        {
            this.Reset += new EventHandler<EventArgs>(Searcher_Reset);
        }

        public virtual void Searcher_Reset(object sender, EventArgs e)
        {
            foreach (SearchCriteia item in this.SearchCriterion)
            {
                item.Reset();
            }
        }


        protected void OnReset(object sender, EventArgs args)
        {
            if (this.Reset != null)
            {
                this.Reset(sender, args);
            }
        }


        protected void OnSearch(object sender, EventArgs args)
        {
            if (this.Search != null)
            {
                this.Search(sender, args);
            }
        }



    }
}
