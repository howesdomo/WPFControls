using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Client.Components
{
    /// <summary>
    /// UcConsole.xaml 的交互逻辑
    /// </summary>
    public partial class UcConsole : UserControl, System.ComponentModel.INotifyPropertyChanged
    {
        #region INotifyPropertyChanged成员

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        public System.Collections.ObjectModel.ObservableCollection<dynamic> ConsoleList { get; set; }

        public UcConsole()
        {
            InitializeComponent();
            this.DataContext = this;
            this.ConsoleList = new System.Collections.ObjectModel.ObservableCollection<dynamic>();
            this.ConsoleList.CollectionChanged += ConsoleList_CollectionChanged;
        }

        private void ConsoleList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.OnPropertyChanged("ConsoleList");
        }

        public EventHandler<ABCEventArgs> AddConsole;

        public void Add(Util.Model.ConsoleData d)
        {
            this.ConsoleList.Add(d);

            int lastIndex = this.ConsoleList.Count - 1;
            ucConsole.ScrollIntoView(this.ConsoleList[lastIndex]);
        }

        public void Clear()
        {
            this.ConsoleList.Clear();
        }

    }

    public class ABCEventArgs : EventArgs
    {

    }
}
