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
    /// Interaction logic for FrmTest_WPFControlsUtils.xaml
    /// </summary>
    public partial class FrmTest_WPFControlsUtils : Window
    {
        public FrmTest_WPFControlsUtils()
        {
            InitializeComponent();
        }
    }

    public class FrmTest_WPFControlsUtils_ViewModel : BaseViewModel
    {
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

        public FrmTest_WPFControlsUtils_ViewModel()
        {
            this.CMD_Test1 = new Command(Test1);
            this.CMD_Test2 = new Command(Test2);
            this.CMD_Test3 = new Command(Test3);
            this.CMD_Test4 = new Command(Test4);

        }

        public Command CMD_Test1 { get; private set; }
        void Test1(object obj)
        {
            Button o = obj as Button;
            var r = WPFControlsUtils.FindParentOfType<Grid>(o);
        }

        public Command CMD_Test2 { get; private set; }
        void Test2(object obj)
        {
            Button o = obj as Button;
            // 指定找上上级的 Grid
            var r = WPFControlsUtils.FindParentOfType<Grid>(o, parentName: "g0");
        }

        public Command CMD_Test3 { get; private set; }
        void Test3(object obj)
        {
            var scrollViewer = WPFControlsUtils.FindChildOfType<System.Windows.Controls.ScrollViewer>(obj as DataGrid);
        }

        public Command CMD_Test4 { get; private set; }
        void Test4(object obj)
        {
            var buttons = WPFControlsUtils.FindChilrenOfType<System.Windows.Controls.Button>(obj as Grid);
            if(buttons.Count == 4)
            {
                WPFControls.MessageBox.ShowInformation("成功");
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
}
