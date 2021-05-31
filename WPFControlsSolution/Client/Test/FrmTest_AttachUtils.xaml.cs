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
    /// Interaction logic for FrmTest_AttachUtils.xaml
    /// </summary>
    public partial class FrmTest_AttachUtils : Window
    {
        public FrmTest_AttachUtils()
        {
            InitializeComponent();
        }
    }

    public class FrmTest_AttachUtils_ViewModel : BaseViewModel
    {
        private bool _TextBox1_IsFocus;
        public bool TextBox1_IsFocus
        {
            get { return _TextBox1_IsFocus; }
            set
            {
                _TextBox1_IsFocus = value;
                this.OnPropertyChanged(nameof(TextBox1_IsFocus));
            }
        }


        private bool _TextBox2_IsFocus;
        public bool TextBox2_IsFocus
        {
            get { return _TextBox2_IsFocus; }
            set
            {
                _TextBox2_IsFocus = value;
                this.OnPropertyChanged(nameof(TextBox2_IsFocus));
            }
        }

        private bool _PasswordBox1_IsFocus;
        public bool PasswordBox1_IsFocus
        {
            get { return _PasswordBox1_IsFocus; }
            set
            {
                _PasswordBox1_IsFocus = value;
                this.OnPropertyChanged(nameof(PasswordBox1_IsFocus));
            }
        }

        private bool _PasswordBox2_IsFocus;
        public bool PasswordBox2_IsFocus
        {
            get { return _PasswordBox2_IsFocus; }
            set
            {
                _PasswordBox2_IsFocus = value;
                this.OnPropertyChanged(nameof(PasswordBox2_IsFocus));
            }
        }


        private string _Password1;
        public string Password1
        {
            get { return _Password1; }
            set
            {
                _Password1 = value;
                this.OnPropertyChanged(nameof(Password1));
            }
        }


        private string _Password2;
        public string Password2
        {
            get { return _Password2; }
            set
            {
                _Password2 = value;
                this.OnPropertyChanged(nameof(Password2));
            }
        }




        public FrmTest_AttachUtils_ViewModel()
        {
            this.CMD_TextBox_Focus = new Command(TextBox_Focus);
            this.CMD_PasswordBox_Focus = new Command(PasswordBox_Focus);


            this.CMD_ListBox_Add = new Command(ListBox_Add);
            this.CMD_ListBox_ItemsSource_Change = new Command(ListBox_ItemsSource_Change);

        }

        public Command CMD_TextBox_Focus { get; private set; }
        void TextBox_Focus()
        {
            this.TextBox1_IsFocus = true;
        }

        public Command CMD_PasswordBox_Focus { get; private set; }
        void PasswordBox_Focus()
        {
            this.PasswordBox1_IsFocus = true;
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
