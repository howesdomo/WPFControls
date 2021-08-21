using Models;
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
    /// Interaction logic for FrmTest_StandardDataGridView_Template.xaml
    /// </summary>
    public partial class FrmTest_StandardDataGridView_Template : Window
    {
        public FrmTest_StandardDataGridView_Template()
        {
            InitializeComponent();
        }
    }
}


namespace Client.ViewModels
{
    public class FrmTest_StandardDataGridView_Template_ViewModel : BaseViewModel
    {
        private System.Collections.ObjectModel.ObservableCollection<DeliveryOrder> _ResultList;
        public System.Collections.ObjectModel.ObservableCollection<DeliveryOrder> ResultList
        {
            get { return _ResultList; }
            set
            {
                _ResultList = value;
                this.OnPropertyChanged(nameof(ResultList));
                this.OnPropertyChanged(nameof(ResultListDescription));
            }
        }

        public string ResultListDescription
        {
            get
            {
                string r = string.Empty;

                if (this.ResultList != null)
                {
                    r = $"共 {this.ResultList.Count} 项";
                }

                return r;
            }
        }


        private System.Collections.IList _SelectedResultList;
        public System.Collections.IList SelectedResultList
        {
            get { return _SelectedResultList; }
            set
            {
                _SelectedResultList = value;
                this.OnPropertyChanged(nameof(SelectedResultList));

                // TODO 做成一个事件
                searchSubResult();
            }
        }

        void searchSubResult()
        {
            this.SubResultList = null;



            var temp = new System.Collections.ObjectModel.ObservableCollection<DeliveryOrderItem>();

            foreach (DeliveryOrder item in this.SelectedResultList)
            {
                this.Items.Where(i => i.OrderNo == item.OrderNo).ToList().ForEach(i => temp.Add(i));
            }

            if (temp.Count > 0)
            {
                this.SubResultList = temp;
            }
        }



        private System.Collections.ObjectModel.ObservableCollection<DeliveryOrderItem> _SubResultList;
        public System.Collections.ObjectModel.ObservableCollection<DeliveryOrderItem> SubResultList
        {
            get { return _SubResultList; }
            set
            {
                _SubResultList = value;
                this.OnPropertyChanged(nameof(SubResultList));
                this.OnPropertyChanged(nameof(SubResultListDescription));
            }
        }

        public string SubResultListDescription
        {
            get
            {
                string r = string.Empty;

                if (this.SubResultList != null)
                {
                    r = $"共 {this.SubResultList.Count} 项";
                }

                return r;
            }
        }


        private System.Collections.IList _SelectedSubResultList;
        public System.Collections.IList SelectedSubResultList
        {
            get { return _SelectedSubResultList; }
            set
            {
                _SelectedSubResultList = value;
                this.OnPropertyChanged(nameof(SelectedSubResultList));

                // TODO 做成一个事件
                if (this.SelectedSubResultList != null)
                {
                    searchThirdResult();
                }
            }
        }

        void searchThirdResult()
        {
            this.ThirdResultList = null;


            // Do db Search...
            var temp = new System.Collections.ObjectModel.ObservableCollection<DeliveryOrderDetail>();

            foreach (DeliveryOrderItem item in this.SelectedSubResultList)
            {
                this.Details.Where(i => i.OrderItemNo == item.OrderItemNo).ToList().ForEach(i => temp.Add(i));
            }

            if (temp.Count > 0)
            {
                this.ThirdResultList = temp;
            }
        }


        private System.Collections.ObjectModel.ObservableCollection<DeliveryOrderDetail> _ThirdResultList;
        public System.Collections.ObjectModel.ObservableCollection<DeliveryOrderDetail> ThirdResultList
        {
            get { return _ThirdResultList; }
            set
            {
                _ThirdResultList = value;
                this.OnPropertyChanged(nameof(ThirdResultList));
                this.OnPropertyChanged(nameof(ThirdResultListDescription));
            }
        }

        public string ThirdResultListDescription
        {
            get
            {
                string r = string.Empty;

                if (this.ThirdResultList != null)
                {
                    r = $"共 {this.ThirdResultList.Count} 项";
                }

                return r;
            }
        }





        public FrmTest_StandardDataGridView_Template_ViewModel()
        {
            initCMD();
            initData();

            this.ResultList = new System.Collections.ObjectModel.ObservableCollection<DeliveryOrder>();
            this.Orders.ForEach(i => this.ResultList.Add(i));
        }

        public List<DeliveryOrder> Orders { get; set; }
        public List<DeliveryOrderItem> Items { get; set; }
        public List<DeliveryOrderDetail> Details { get; set; }

        void initData()
        {
            User user1 = new User() { LoginAccount = "A003", UserName = "张三" };
            User user2 = new User() { LoginAccount = "A004", UserName = "李四" };

            this.Orders = new List<DeliveryOrder>()
            {
                new DeliveryOrder() { OrderNo = "A", CreateDateTime = DateTime.Now, PlanQty = 10, ScanQty = 5, CreateUser = user1 },
                new DeliveryOrder() { OrderNo = "B", CreateDateTime = DateTime.Now, PlanQty = 10, ScanQty = null, CreateUser = user1 },
                new DeliveryOrder() { OrderNo = "C", CreateDateTime = DateTime.Now, PlanQty = 10, ScanQty = null, CreateUser = user1 },
                new DeliveryOrder() { OrderNo = "D", CreateDateTime = DateTime.Now, PlanQty = 8, ScanQty = 6, CreateUser = user2 },
                new DeliveryOrder() { OrderNo = "E", CreateDateTime = DateTime.Now, PlanQty = 10, ScanQty = null, CreateUser = user1 },
                new DeliveryOrder() { OrderNo = "F", CreateDateTime = DateTime.Now, PlanQty = 10, ScanQty = null, CreateUser = user2 },
                new DeliveryOrder() { OrderNo = "G", CreateDateTime = DateTime.Now, PlanQty = 10, ScanQty = null, CreateUser = user1 },
            };

            this.Items = new List<DeliveryOrderItem>()
            {
                new DeliveryOrderItem() { OrderItemNo = "A1", OrderNo = "A", ProductCode = "XSX", PlanQty = 6, ScanQty = 4 },
                new DeliveryOrderItem() { OrderItemNo = "A2", OrderNo = "A", ProductCode = "PS4", PlanQty = 4, ScanQty = 1 },
                new DeliveryOrderItem() { OrderItemNo = "D1", OrderNo = "D", ProductCode = "Switch", PlanQty = 2, ScanQty = 2 },
                new DeliveryOrderItem() { OrderItemNo = "D2", OrderNo = "D", ProductCode = "PS4", PlanQty = 2, ScanQty = 1 },
                new DeliveryOrderItem() { OrderItemNo = "D3", OrderNo = "D", ProductCode = "PS5", PlanQty = 2, ScanQty = 2 },
                new DeliveryOrderItem() { OrderItemNo = "D4", OrderNo = "D", ProductCode = "XSX", PlanQty = 2, ScanQty = 1 },
            };

            this.Details = new List<DeliveryOrderDetail>()
            {
                new DeliveryOrderDetail(){ OrderItemNo = "A1", ScanQty = 1, ScanDateTime = DateTime.Now, ScanUser = user1, ScanBarcodeContent = "AB001"},
                new DeliveryOrderDetail(){ OrderItemNo = "A1" ,ScanQty = 1, ScanDateTime = DateTime.Now, ScanUser = user1, ScanBarcodeContent = "AB0"},
                new DeliveryOrderDetail(){ OrderItemNo = "A1", ScanQty = 1, ScanDateTime = DateTime.Now, ScanUser = user1, ScanBarcodeContent = "AB01"},
                new DeliveryOrderDetail(){ OrderItemNo = "A1", ScanQty = 1, ScanDateTime = DateTime.Now, ScanUser = user1, ScanBarcodeContent = "AB010"},

                new DeliveryOrderDetail(){ OrderItemNo = "A2", ScanQty = 1, ScanDateTime = DateTime.Now, ScanUser = user1},

                new DeliveryOrderDetail(){ OrderItemNo = "D1", ScanQty = 1, ScanDateTime = DateTime.Now, ScanUser = user1},
                new DeliveryOrderDetail(){ OrderItemNo = "D1", ScanQty = 1, ScanDateTime = DateTime.Now, ScanUser = user1},

                new DeliveryOrderDetail(){ OrderItemNo = "D2", ScanQty = 1, ScanDateTime = DateTime.Now, ScanUser = user1},

                new DeliveryOrderDetail(){ OrderItemNo = "D3", ScanQty = 1, ScanDateTime = DateTime.Now, ScanUser = user1},
                new DeliveryOrderDetail(){ OrderItemNo = "D3", ScanQty = 1, ScanDateTime = DateTime.Now, ScanUser = user1},

                new DeliveryOrderDetail(){ OrderItemNo = "D4", ScanQty = 1, ScanDateTime = DateTime.Now, ScanUser = user1},
            };
        }

        void initCMD()
        {
            this.CMD_Reset = new Command(Reset);
            this.CMD_Search = new Command(Search);
        }

        public Command CMD_Reset { get; private set; }
        void Reset()
        {

        }

        public Command CMD_Search { get; private set; }
        void Search()
        {
            WPFControls.MessageBox.ShowInformation(Util.JsonUtils.SerializeObjectWithFormatted(this.SearchArgs));
        }


        private SearchArgs _SearchArgs = new SearchArgs();
        public SearchArgs SearchArgs
        {
            get { return _SearchArgs; }
            set
            {
                _SearchArgs = value;
                this.OnPropertyChanged(nameof(SearchArgs));
            }
        }

    }

    public class DeliveryOrder : BaseViewModel
    {
        private string _OrderNo;
        public string OrderNo
        {
            get { return _OrderNo; }
            set
            {
                _OrderNo = value;
                this.OnPropertyChanged(nameof(OrderNo));
            }
        }

        private DateTime _CreateDateTime;
        public DateTime CreateDateTime
        {
            get { return _CreateDateTime; }
            set
            {
                _CreateDateTime = value;
                this.OnPropertyChanged("CreateDateTime");
            }
        }


        private User _CreateUser;
        public User CreateUser
        {
            get { return _CreateUser; }
            set
            {
                _CreateUser = value;
                this.OnPropertyChanged(nameof(CreateUser));
            }
        }


        private int _PlanQty;
        public int PlanQty
        {
            get { return _PlanQty; }
            set
            {
                _PlanQty = value;
                this.OnPropertyChanged(nameof(PlanQty));
            }
        }

        private int? _ScanQty;
        public int? ScanQty
        {
            get { return _ScanQty; }
            set
            {
                _ScanQty = value;
                this.OnPropertyChanged(nameof(ScanQty));
            }
        }


    }

    public class DeliveryOrderItem : BaseViewModel
    {
        private string _OrderItemNo;
        public string OrderItemNo
        {
            get { return _OrderItemNo; }
            set
            {
                _OrderItemNo = value;
                this.OnPropertyChanged(nameof(OrderItemNo));
            }
        }


        private string _OrderNo;
        public string OrderNo
        {
            get { return _OrderNo; }
            set
            {
                _OrderNo = value;
                this.OnPropertyChanged(nameof(OrderNo));
            }
        }



        private string _ProductCode;
        public string ProductCode
        {
            get { return _ProductCode; }
            set
            {
                _ProductCode = value;
                this.OnPropertyChanged(nameof(ProductCode));
            }
        }

        private int _PlanQty;
        public int PlanQty
        {
            get { return _PlanQty; }
            set
            {
                _PlanQty = value;
                this.OnPropertyChanged(nameof(PlanQty));
            }
        }

        private int? _ScanQty;
        public int? ScanQty
        {
            get { return _ScanQty; }
            set
            {
                _ScanQty = value;
                this.OnPropertyChanged(nameof(ScanQty));
            }
        }

    }

    public class DeliveryOrderDetail : BaseViewModel
    {

        private string _OrderItemNo;
        public string OrderItemNo
        {
            get { return _OrderItemNo; }
            set
            {
                _OrderItemNo = value;
                this.OnPropertyChanged(nameof(OrderItemNo));
            }
        }

        private string _ScanBarcodeContent;
        public string ScanBarcodeContent
        {
            get { return _ScanBarcodeContent; }
            set
            {
                _ScanBarcodeContent = value;
                this.OnPropertyChanged(nameof(ScanBarcodeContent));
            }
        }


        private int _ScanQty;
        public int ScanQty
        {
            get { return _ScanQty; }
            set
            {
                _ScanQty = value;
                this.OnPropertyChanged(nameof(ScanQty));
            }
        }

        private User _ScanUser;
        public User ScanUser
        {
            get { return _ScanUser; }
            set
            {
                _ScanUser = value;
                this.OnPropertyChanged(nameof(ScanUser));
            }
        }

        private DateTime _ScanDateTime;
        public DateTime ScanDateTime
        {
            get { return _ScanDateTime; }
            set
            {
                _ScanDateTime = value;
                this.OnPropertyChanged(nameof(ScanDateTime));
            }
        }

    }

    public class User : BaseViewModel
    {
        private string _LoginAccount;
        public string LoginAccount
        {
            get { return _LoginAccount; }
            set
            {
                _LoginAccount = value;
                this.OnPropertyChanged(nameof(LoginAccount));
            }
        }

        private string _UserName;
        public string UserName
        {
            get { return _UserName; }
            set
            {
                _UserName = value;
                this.OnPropertyChanged(nameof(UserName));
            }
        }

    }


}

namespace Models
{
    public class SearchArgs : BaseViewModel
    {
        public SearchArgs()
        {

        }

        private string _OrderNo;
        public string OrderNo
        {
            get { return _OrderNo; }
            set
            {
                _OrderNo = value;
                this.OnPropertyChanged(nameof(OrderNo));
            }
        }

        private string _CartonNos;
        public string CartonNos
        {
            get { return _CartonNos; }
            set
            {
                _CartonNos = value;
                this.OnPropertyChanged(nameof(CartonNos));
            }
        }



        private DateTime? _LockStartDate;
        public DateTime? LockStartDate
        {
            get { return _LockStartDate; }
            set
            {
                _LockStartDate = value;
                this.OnPropertyChanged(nameof(LockStartDate));
            }
        }


        private DateTime? _LockEndDate;
        public DateTime? LockEndDate
        {
            get { return _LockEndDate; }
            set
            {
                _LockEndDate = value;
                this.OnPropertyChanged(nameof(LockEndDate));
            }
        }

        private DateTime? _CartonWorkshopScan_StartDateTime;
        public DateTime? CartonWorkshopScan_StartDateTime
        {
            get { return _CartonWorkshopScan_StartDateTime; }
            set
            {
                _CartonWorkshopScan_StartDateTime = value;
                this.OnPropertyChanged(nameof(CartonWorkshopScan_StartDateTime));
            }
        }

        private DateTime? _CartonWorkshopScan_EndDateTime;
        public DateTime? CartonWorkshopScan_EndDateTime
        {
            get { return _CartonWorkshopScan_EndDateTime; }
            set
            {
                _CartonWorkshopScan_EndDateTime = value;
                this.OnPropertyChanged(nameof(CartonWorkshopScan_EndDateTime));
            }
        }

        private DateTime? _CartonWorkshopScan_StartDateTime_1;
        public DateTime? CartonWorkshopScan_StartDateTime_1
        {
            get { return _CartonWorkshopScan_StartDateTime_1; }
            set
            {
                _CartonWorkshopScan_StartDateTime_1 = value;
                this.OnPropertyChanged(nameof(CartonWorkshopScan_StartDateTime_1));
            }
        }

        private DateTime? _CartonWorkshopScan_EndDateTime_1;
        public DateTime? CartonWorkshopScan_EndDateTime_1
        {
            get { return _CartonWorkshopScan_EndDateTime_1; }
            set
            {
                _CartonWorkshopScan_EndDateTime_1 = value;
                this.OnPropertyChanged(nameof(CartonWorkshopScan_EndDateTime_1));
            }
        }


        public List<string> ListString { get; set; } = new List<string>() { "A", "B", "C" };

        private object _ListString_SelectedValue;
        public object ListString_SelectedValue
        {
            get { return _ListString_SelectedValue; }
            set
            {
                _ListString_SelectedValue = value;
                this.OnPropertyChanged(nameof(ListString_SelectedValue));
            }
        }


        public string[] ListArr { get; set; } = new string[] { "CC", "BB", "AA" };

        private object _ListArr_SelectedValue;
        public object ListArr_SelectedValue
        {
            get { return _ListArr_SelectedValue; }
            set
            {
                _ListArr_SelectedValue = value;
                this.OnPropertyChanged(nameof(ListArr_SelectedValue));
            }
        }

        public List<Location> LocationList { get; set; } = Location.GetListWithEmpty();

        private Location _SelectedLocation;
        public Location SelectedLocation
        {
            get { return _SelectedLocation; }
            set
            {
                _SelectedLocation = value;
                this.OnPropertyChanged(nameof(SelectedLocation));
            }
        }


        private List<Location> _TemplateListBox_Location_ItemsSource = Location.GetList();
        public List<Location> TemplateListBox_Location_ItemsSource
        {
            get
            {
                return _TemplateListBox_Location_ItemsSource;
            }
            set
            {
                _TemplateListBox_Location_ItemsSource = value;
                this.OnPropertyChanged(nameof(TemplateListBox_Location_ItemsSource));
            }
        }

        private System.Collections.IList _TemplateListBox_Location_SelectedItems = new System.Collections.ObjectModel.ObservableCollection<Location>() { Location.GetList()[0], Location.GetList()[2] };
        public System.Collections.IList TemplateListBox_Location_SelectedItems
        {
            get
            {
                return _TemplateListBox_Location_SelectedItems;
            }
            set
            {
                _TemplateListBox_Location_SelectedItems = value;
                this.OnPropertyChanged(nameof(TemplateListBox_Location_SelectedItems));
            }
        }


        public List<Location> ListBox_Location_ItemsSource { get; set; } = Location.GetList();
        public System.Collections.IList ListBox_Location_SelectedItems { get; set; } = new System.Collections.ObjectModel.ObservableCollection<Location>() { Location.GetList()[0], Location.GetList()[2] };



        #region TreeView 数据

        public List<Region> TreeView_Region_ItemsSource { get; set; } = Region.GetList();

        public List<Region> TreeView_Region_CheckedItems { get; set; } = new List<Region>();

        public List<Region> TreeView_Region_CheckedItemsWithNull { get; set; } = new List<Region>();

        #endregion

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
                return new List<Location>()
                {
                    new Location(){ Code = -1, Name =string.Empty },
                    new Location(){ Code = 0, Name ="广州" },
                    new Location(){ Code = 1, Name ="深圳" },
                    new Location(){ Code = 2, Name ="北京" },
                };
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

        public class Region 
        {            
            public Guid Id { get; set; }

            public Guid? ParentId { get; set; }

            public string Name { get; set; }

            public static List<Region> GetList()
            {
                List<Region> r = new List<Region>();

                var r0 = new Region() { Id = Guid.NewGuid(), ParentId = null, Name = "广州" };
                var r0_0 = new Region() { Id = Guid.NewGuid(), ParentId = r0.Id, Name = "荔湾区" };
                var r0_0_0 = new Region() { Id = Guid.NewGuid(), ParentId = r0_0.Id, Name = "金花社区" };
                var r0_0_1 = new Region() { Id = Guid.NewGuid(), ParentId = r0_0.Id, Name = "多宝社区" };

                var r0_1 = new Region() { Id = Guid.NewGuid(), ParentId = r0.Id, Name = "黄埔区" };

                var r1 = new Region() { Id = Guid.NewGuid(), ParentId = null, Name = "韶关" };
                var r1_0 = new Region() { Id = Guid.NewGuid(), ParentId = r1.Id, Name = "浈江区" };
                var r1_1 = new Region() { Id = Guid.NewGuid(), ParentId = r1.Id, Name = "武江区" };

                r.Add(r0);
                r.Add(r0_0);
                r.Add(r0_0_0);
                r.Add(r0_0_1);

                r.Add(r0_1);
                
                r.Add(r1);
                r.Add(r1_0);
                r.Add(r1_1);

                return r;
            }
        }
    }


}