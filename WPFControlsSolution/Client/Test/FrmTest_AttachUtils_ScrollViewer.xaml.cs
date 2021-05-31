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
using System.Windows.Shapes;

namespace Client.Test
{
    /// <summary>
    /// Interaction logic for FrmTest_AttachUtils_ScrollViewer.xaml
    /// </summary>
    public partial class FrmTest_AttachUtils_ScrollViewer : Window
    {
        public FrmTest_AttachUtils_ScrollViewer()
        {
            InitializeComponent();
        }
    }


    public class FrmTest_AttachUtils_ScrollViewer_ViewModel : BaseViewModel
    {
        private bool _AutoScrollToBottom_IsEnabled = true;
        public bool AutoScrollToBottom_IsEnabled
        {
            get { return _AutoScrollToBottom_IsEnabled; }
            set
            {
                _AutoScrollToBottom_IsEnabled = value;
                this.OnPropertyChanged(nameof(AutoScrollToBottom_IsEnabled));
            }
        }


        public FrmTest_AttachUtils_ScrollViewer_ViewModel()
        {
            this.CMD_ListBox_Add = new Command(ListBox_Add);
            this.CMD_ListBox_ItemsSource_Change = new Command(ListBox_ItemsSource_Change);
        }

        private ObservableCollection<A> _List = new ObservableCollection<A>()
        {
            new A() {  No = 1, Name = "1"},
            new A() {  No = 2, Name = "2"},
            new A() {  No = 3, Name = "3"},
            new A() {  No = 4, Name = "4"},
            new A() {  No = 5, Name = "5"},
            new A() {  No = 6, Name = "6"},
            new A() {  No = 7, Name = "7"},
            new A() {  No = 8, Name = "8"},
            new A() {  No = 9, Name = "9"},
        };
        public ObservableCollection<A> List
        {
            get { return _List; }
            set
            {
                _List = value;
                this.OnPropertyChanged(nameof(List));
            }
        }


        Random r = new Random();

        public Command CMD_ListBox_Add { get; private set; }
        void ListBox_Add()
        {
            this.List.Add(new A()
            {
                No = r.Next(1000),
                Name = $"{r.Next(1000)}"
            });
        }

        public Command CMD_ListBox_ItemsSource_Change { get; private set; }
        void ListBox_ItemsSource_Change()
        {
            var toChange = new ObservableCollection<A>()
            {
                new A() {  No = 9, Name = "9"},
                new A() {  No = 8, Name = "8"},
                new A() {  No = 7, Name = "7"},
                new A() {  No = 6, Name = "6"},
                new A() {  No = 5, Name = "5"},
                new A() {  No = 4, Name = "4"},
                new A() {  No = 3, Name = "3"},
                new A() {  No = 2, Name = "2"},
                new A() {  No = 1, Name = "1"},
            };

            this.List = toChange;
        }
    }

    public class A : BaseViewModel
    {
        private int _No;
        public int No
        {
            get { return _No; }
            set
            {
                _No = value;
                this.OnPropertyChanged(nameof(No));
            }
        }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                this.OnPropertyChanged(nameof(Name));
            }
        }

    }
}
