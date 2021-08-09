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
using System.Windows.Shapes;

namespace Client.Test
{
    /// <summary>
    /// Interaction logic for FrmTest_ListBox.xaml
    /// </summary>
    public partial class FrmTest_ListBox : Window
    {
        public FrmTest_ListBox()
        {
            InitializeComponent();
        }
    }

    public class FrmTest_ListBox_ViewModel : BaseViewModel
    {
        public FrmTest_ListBox_ViewModel()
        {
            this.CMD_Check = new Command(Check);

            this.CMD_Show_IntList_SelectedItems = new Command(Show_IntList_SelectedItems);
            this.CMD_Show_StringList_SelectedItems = new Command(Show_StringList_SelectedItems);
            this.CMD_Show_ListBoxAdv_SelectedItems = new Command(Show_ListBoxAdv_SelectedItems);

        }


        private List<Location> _LocationList = Location.GetList();
        public List<Location> LocationList
        {
            get { return _LocationList; }
            set
            {
                _LocationList = value;
                this.OnPropertyChanged(nameof(LocationList));
            }
        }


        private System.Collections.IList _SelectedItems = new List<Location>() { Location.GetList()[2], Location.GetList()[0] };
        public System.Collections.IList SelectedItems
        {
            get { return _SelectedItems; }
            set
            {
                _SelectedItems = value;
                this.OnPropertyChanged(nameof(SelectedItems));
            }
        }


        private List<int> _IntList = new List<int>() { 1,2,3,4,5,6 };
        public List<int> IntList
        {
            get { return _IntList; }
            set
            {
                _IntList = value;
                this.OnPropertyChanged(nameof(IntList));
            }
        }

        private System.Collections.IList _IntList_SelectedItems = new List<int>() { 1,3,5 };
        public System.Collections.IList IntList_SelectedItems
        {
            get { return _IntList_SelectedItems; }
            set
            {
                _IntList_SelectedItems = value;
                this.OnPropertyChanged(nameof(IntList_SelectedItems));
            }
        }

        public Command CMD_Show_IntList_SelectedItems { get; private set; }
        void Show_IntList_SelectedItems()
        {
            WPFControls.MessageBox.ShowInformationDialog(Util.JsonUtils.SerializeObjectWithFormatted(this.IntList_SelectedItems));
        }




        private List<string> _StringList = new List<string>() { "A-0", "B-1", "C-2", "D-3", "E-4", "F-5", "G-6", };
        public List<string> StringList
        {
            get { return _StringList; }
            set
            {
                _StringList = value;
                this.OnPropertyChanged(nameof(StringList));
            }
        }

        // !!! 必须绑定 System.Collections.ObjectModel.ObservableCollection 吗?
        private System.Collections.IList _StringList_SelectedItems = new System.Collections.ObjectModel.ObservableCollection<string>() { "B-1", "D-3", "F-5" };
        public System.Collections.IList StringList_SelectedItems
        {
            get { return _StringList_SelectedItems; }
            set
            {
                _StringList_SelectedItems = value;
                this.OnPropertyChanged(nameof(StringList_SelectedItems));
            }
        }

        public Command CMD_Show_StringList_SelectedItems { get; private set; }
        void Show_StringList_SelectedItems()
        {
            WPFControls.MessageBox.ShowInformationDialog(Util.JsonUtils.SerializeObjectWithFormatted(this.StringList_SelectedItems));
        }



        private System.Collections.IList _ListBoxAdv_SelectedItems = new System.Collections.ObjectModel.ObservableCollection<Location>();
        public System.Collections.IList ListBoxAdv_SelectedItems
        {
            get { return _ListBoxAdv_SelectedItems; }
            set
            {
                _ListBoxAdv_SelectedItems = value;
                this.OnPropertyChanged(nameof(ListBoxAdv_SelectedItems));
            }
        }

        public Command CMD_Show_ListBoxAdv_SelectedItems { get; private set; }
        void Show_ListBoxAdv_SelectedItems()
        {
            WPFControls.MessageBox.ShowInformationDialog(Util.JsonUtils.SerializeObjectWithFormatted(this.ListBoxAdv_SelectedItems));
        }



        public Command CMD_Check { get; private set; }
        void Check()
        {
            WPFControls.MessageBox.ShowInformationDialog
            (
                Util.JsonUtils.SerializeObjectWithFormatted(this.SelectedItems)
            );
        }


        public class Location
        {
            private static List<Location> _List_ = new List<Location>()
                {
                    new Location(){ Code = -1, Name =string.Empty },
                    new Location(){ Code = 0, Name ="广州" },
                    new Location(){ Code = 1, Name ="深圳" },
                    new Location(){ Code = 2, Name ="北京" },
                };

            public static List<Location> GetList()
            {
                return _List_.Skip(1).ToList();
            }

            public static List<Location> GetListWithEmpty()
            {
                return _List_;
            }

            private int _Code;
            public int Code
            {
                get { return _Code; }
                set { _Code = value; }
            }

            private string _Name;
            public string Name
            {
                get { return _Name; }
                set { _Name = value; }
            }
        }
    }
}
