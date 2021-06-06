using Client.Components;
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
    /// Interaction logic for FrmTest_EnpotControls_DataGridView.xaml
    /// </summary>
    public partial class FrmTest_StandardDataGridView : Window
    {
        public FrmTest_StandardDataGridView()
        {
            InitializeComponent();
        }
    }

    public class FrmTest_StandardDataGridView_ViewModel : BaseViewModel
    {
        private List<DataGridSelectMode> _DataGridSelectModeList;
        public List<DataGridSelectMode> DataGridSelectModeList
        {
            get { return _DataGridSelectModeList; }
            set
            {
                _DataGridSelectModeList = value;
                this.OnPropertyChanged(nameof(DataGridSelectModeList));
            }
        }


        private DataGridSelectMode _DataGridSelectModeList_SelectedItem;
        public DataGridSelectMode DataGridSelectModeList_SelectedItem
        {
            get { return _DataGridSelectModeList_SelectedItem; }
            set
            {
                _DataGridSelectModeList_SelectedItem = value;
                this.OnPropertyChanged(nameof(DataGridSelectModeList_SelectedItem));
            }
        }

        private ObservableCollection<A> _List = new ObservableCollection<A>()
        {
            new A() { IsChecked = false, CreateDate = DateTime.Now.Date, OrderNo = 1001, Name = "MSLM-sMISQALLLJS-SJIC<S<LKS>-A01"},
            new A() { IsChecked = false, CreateDate = DateTime.Now.Date, OrderNo = 1002, Name = "MSLM-sMISQALLLJS-SJIC<S<LKS>-A121"},
            new A() { IsChecked = false, CreateDate = DateTime.Now.Date, OrderNo = 1003, Name = "MSLM-sMISQALLLJS-SJIC<S<LKS>-A301"},
            new A() { IsChecked = false, CreateDate = DateTime.Now.Date, OrderNo = 1004, Name = "MSLM-sMISQALLLJS-SJIC<S<LKS>-B02"},
            new A() { IsChecked = false, CreateDate = DateTime.Now.Date, OrderNo = 1005, Name = "MSLM-sMISQALLLJS-SJIC<S<LKS>-C24"},
            new A() { IsChecked = false, CreateDate = DateTime.Now.Date, OrderNo = 1006, Name = "MSLM-sMISQALLLJS-SJIC<S<LKS>-C25"},
            new A() { IsChecked = false, CreateDate = DateTime.Now.Date, OrderNo = 1007, Name = "MSLM-sMISQALLLJS-SJIC<S<LKS>-C51"},
            new A() { IsChecked = false, CreateDate = DateTime.Now.Date, OrderNo = 1008, Name = "MSLM-sMISQALLLJS-SJIC<S<LKS>-A123"},
            new A() { IsChecked = false, CreateDate = DateTime.Now.Date, OrderNo = 1009, Name = "MSLM-sMISQALLLJS-SJIC<S<LKS>-A001"},
            new A() { IsChecked = false, CreateDate = DateTime.Now.Date, OrderNo = 10010, Name = "A031"},
            new A() { IsChecked = false, CreateDate = DateTime.Now.Date, OrderNo = 10011, Name = "A101"},
            new A() { IsChecked = false, CreateDate = DateTime.Now.Date, OrderNo = 10012, Name = "A021"},
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


        public string ListInfo
        {
            get
            {
                string r = string.Empty;
                if (List != null && List.Count > 0)
                {
                    r = string.Format("共 {0} 条", List.Count);
                }

                if (this.SelectedItem != null)
                {
                    r += string.Format("，Item 选中单号 {0}", this.SelectedItem.OrderNo);
                }

                if (this.SelectedCell != null && this.SelectedCell.IsValid == true)
                {
                    r += string.Format("，Cell 选中列名 {0}", this.SelectedCell.Column.Header);
                }

                return r;
            }
        }

        //public EventHandler<EventArgs> SelectedItemEvent;

        //private void OnSelectedItem(EventArgs args)
        //{
        //    if (this.SelectedItemEvent != null)
        //    {
        //        this.SelectedItemEvent.Invoke(null, args);
        //    }
        //}

        public A _SelectedItem;
        public A SelectedItem
        {
            get { return _SelectedItem; }
            set
            {
                this._SelectedItem = value;
                if (this.SelectedItem != null && this.ItemAllList != null)
                {
                    var matchQuery = this.ItemAllList
                        .Where(i => i.OrderNo == this.SelectedItem.OrderNo);

                    if (matchQuery.Count() > 0)
                    {
                        this.ItemList = matchQuery.ToList();
                    }
                    else
                    {
                        this.ItemList = new List<A>();
                    }
                }

                this.OnPropertyChanged(nameof(SelectedItem));
                this.OnPropertyChanged(nameof(SelectedItemInfo));
                this.OnPropertyChanged(nameof(ListInfo));

                // !!!!!!!!! 为 SelectedItems 进行更新 !!!!!!!!!
                this.OnPropertyChanged(nameof(SelectedItems));
                this.OnPropertyChanged(nameof(SelectedItemsCount));
                this.OnPropertyChanged(nameof(SelectedItemsInfo));
            }
        }

        public string SelectedItemInfo
        {
            get
            {
                string r = string.Empty;
                if (this.SelectedItems != null)
                {
                    r = Util.JsonUtils.SerializeObjectWithFormatted(this.SelectedItem);
                }
                return r;
            }
        }



        private System.Windows.Controls.DataGridCellInfo _SelectedCell;
        public System.Windows.Controls.DataGridCellInfo SelectedCell
        {
            get { return _SelectedCell; }
            set
            {
                this._SelectedCell = value;
                this.OnPropertyChanged(nameof(SelectedCell));
                this.OnPropertyChanged(nameof(SelectedCellInfo));
            }
        }

        public string SelectedCellInfo
        {
            get
            {
                string r = string.Empty;
                if (this.SelectedCell.IsValid)
                {
                    r += $"Column: {Util.JsonUtils.SerializeObjectWithFormatted(this.SelectedCell.Column.Header)}\r\n";
                    r += $"Item:{Util.JsonUtils.SerializeObjectWithFormatted(this.SelectedCell.Item)}";
                }
                return r;
            }
        }



        private System.Collections.IList _SelectedItems;
        public System.Collections.IList SelectedItems
        {
            get { return _SelectedItems; }
            set
            {
                _SelectedItems = value;
                this.OnPropertyChanged(nameof(SelectedItems));
                this.OnPropertyChanged(nameof(SelectedItemsCount));
                this.OnPropertyChanged(nameof(SelectedItemsInfo));
            }
        }

        public string SelectedItemsCount
        {
            get
            {
                string r = string.Empty;
                if (this.SelectedItems != null)
                {
                    r = $"共 {this.SelectedItems.Count} 项";
                }
                return r;
            }
        }

        public string SelectedItemsInfo
        {
            get
            {
                string r = string.Empty;
                if (SelectedItems != null && SelectedItems.Count > 0)
                {
                    r = Util.JsonUtils.SerializeObjectWithFormatted(this.SelectedItems);
                }
                return r;
            }
        }



        private IList<DataGridCellInfo> _SelectedCells;
        public IList<DataGridCellInfo> SelectedCells
        {
            get { return _SelectedCells; }
            set
            {
                _SelectedCells = value;
                this.OnPropertyChanged(nameof(SelectedCells));
                this.OnPropertyChanged(nameof(SelectedCellsCount));
                this.OnPropertyChanged(nameof(SelectedCells_ColumnsInfo));
                this.OnPropertyChanged(nameof(SelectedCells_ItemsInfo));
            }
        }

        public string SelectedCellsCount
        {
            get
            {
                string r = string.Empty;
                if (this.SelectedCells != null)
                {
                    r = $"共 {this.SelectedCells.Count} 项";
                }
                return r;
            }
        }

        public string SelectedCells_ColumnsInfo
        {
            get
            {
                string r = string.Empty;
                if (SelectedCells != null && SelectedCells.Count > 0)
                {
                    foreach (DataGridCellInfo item in this.SelectedCells)
                    {
                        r += item.Column.Header.ToString();
                    }
                }
                return r;
            }
        }

        public string SelectedCells_ItemsInfo
        {
            get
            {
                string r = string.Empty;
                if (SelectedCells != null && SelectedCells.Count > 0)
                {
                    r = Util.JsonUtils.SerializeObjectWithFormatted(toJsonStrList());
                }
                return r;
            }
        }


        IEnumerable<string> toJsonStrList()
        {
            foreach (DataGridCellInfo cell in this.SelectedCells)
            {
                yield return Util.JsonUtils.SerializeObjectWithFormatted(cell.Item);
            }
        }







        public List<A> ItemAllList { get; set; }

        public List<A> _ItemList;

        public List<A> ItemList
        {
            get { return _ItemList; }
            set
            {
                _ItemList = value;
                this.OnPropertyChanged(nameof(ItemList));
                this.OnPropertyChanged(nameof(ItemListInfo));
            }
        }

        public string ItemListInfo
        {
            get
            {
                string r = string.Empty;
                if (ItemList != null && ItemList.Count > 0)
                {
                    r = string.Format("单号 {0}，共 {1} 条", ItemList[0].OrderNo, ItemList.Count);
                }

                if (this.DetailSelectedItem != null)
                {
                    r += string.Format("，Item 选中单号 {0}", this.DetailSelectedItem.OrderNo);
                }

                if (this.DetailSelectedCell != null && this.DetailSelectedCell.IsValid == true)
                {
                    r += string.Format("，Cell 选中列名 {0}", this.DetailSelectedCell.Column.Header);
                }
                return r;
            }
        }

        private System.Windows.Controls.DataGridCellInfo _DetailSelectedCell;

        public System.Windows.Controls.DataGridCellInfo DetailSelectedCell
        {
            get
            {
                return this._DetailSelectedCell;
            }
            set
            {
                _DetailSelectedCell = value;
                this.OnPropertyChanged(nameof(DetailSelectedCell));
            }
        }

        private A _DetailSelectedItem;

        public A DetailSelectedItem
        {
            get
            {
                return this._DetailSelectedItem;
            }
            set
            {
                _DetailSelectedItem = value;
                this.OnPropertyChanged(nameof(DetailSelectedItem));
                this.OnPropertyChanged(nameof(ItemListInfo));
            }
        }


        public FrmTest_StandardDataGridView_ViewModel()
        {
            this.DataGridSelectModeList = new List<DataGridSelectMode>() { DataGridSelectMode.Row, DataGridSelectMode.Rows, DataGridSelectMode.Cell, DataGridSelectMode.Cells };
            this.DataGridSelectModeList_SelectedItem = DataGridSelectMode.Row;
        }



        public class A : BaseViewModel
        {
            private bool _IsChecked;
            public bool IsChecked
            {
                get { return _IsChecked; }
                set
                {
                    _IsChecked = value;
                    this.OnPropertyChanged(nameof(IsChecked));
                }
            }


            private int _OrderNo;
            public int OrderNo
            {
                get { return _OrderNo; }
                set
                {
                    _OrderNo = value;
                    this.OnPropertyChanged(nameof(OrderNo));
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

            private DateTime _CreateDate;
            public DateTime CreateDate
            {
                get { return _CreateDate; }
                set
                {
                    _CreateDate = value;
                    this.OnPropertyChanged(nameof(CreateDate));
                }
            }


        }

    }
}
