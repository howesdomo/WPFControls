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

namespace Client.Components
{
    /// <summary>
    /// Interaction logic for UcSerialDeviceConfig.xaml
    /// </summary>
    public partial class UcSerialDeviceConfig : UserControl
    {
        public UcSerialDeviceConfig()
        {
            InitializeComponent();
        }

        #region [DP]Title

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register
        (
            name: "Title",
            propertyType: typeof(string),
            ownerType: typeof(UcSerialDeviceConfig),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: "串口设备配置",
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        #endregion

        public bool IsValid
        {
            get
            {
                if (this.ucDeviceConfig == null)
                {
                    return false;
                }

                return this.ucDeviceConfig.IsValid;
            }
        }

        public string Error
        {
            get
            {
                if (this.ucDeviceConfig == null)
                {
                    return "UcNetworkDeviceConfigBase is null";
                }

                return this.ucDeviceConfig.Error;
            }
        }

        public object DeviceConfig
        {
            get
            {
                dynamic r = new System.Dynamic.ExpandoObject();
                r.PortName = ucDeviceConfig.PortName;
                r.BaudRate = ucDeviceConfig.BaudRate;
                r.DataBits = ucDeviceConfig.DataBits;
                r.Parity = ucDeviceConfig.Parity;
                r.StopBits = ucDeviceConfig.StopBits;
                
                r.ThreadSleep_BeforeRead = ucDeviceConfig.ThreadSleep_BeforeRead;
                
                r.Encoding = ucDeviceConfig.Encoding;
                
                return r;
            }
        }
    }

    public class UcSerialDeviceConfig_ViewModel : BaseViewModel
    {
        #region Props

        private string _PortName;
        public string PortName
        {
            get { return _PortName; }
            set
            {
                _PortName = value;
                this.OnPropertyChanged("PortName");
            }
        }


        private List<string> _PortNameList;
        public List<string> PortNameList
        {
            get { return _PortNameList; }
            set
            {
                _PortNameList = value;
                this.OnPropertyChanged("PortNameList");
            }
        }



        private Util.IO.BaudRate _BaudRate;
        public Util.IO.BaudRate BaudRate
        {
            get { return _BaudRate; }
            set
            {
                _BaudRate = value;
                this.OnPropertyChanged("BaudRate");
            }
        }

        private List<Util.IO.BaudRate> _BaudRateList;
        public List<Util.IO.BaudRate> BaudRateList
        {
            get { return _BaudRateList; }
            set
            {
                _BaudRateList = value;
                this.OnPropertyChanged("BaudRateList");
            }
        }

        private Util.IO.DataBits _DataBits;
        public Util.IO.DataBits DataBits
        {
            get { return _DataBits; }
            set
            {
                _DataBits = value;
                this.OnPropertyChanged("DataBits");
            }
        }

        private List<Util.IO.DataBits> _DataBitsList;
        public List<Util.IO.DataBits> DataBitsList
        {
            get { return _DataBitsList; }
            set
            {
                _DataBitsList = value;
                this.OnPropertyChanged("DataBitsList");
            }
        }


        private Util.IO.Parity _Parity;
        public Util.IO.Parity Parity
        {
            get { return _Parity; }
            set
            {
                _Parity = value;
                this.OnPropertyChanged("Parity");
            }
        }


        private List<Util.IO.Parity> _ParityList;
        public List<Util.IO.Parity> ParityList
        {
            get { return _ParityList; }
            set
            {
                _ParityList = value;
                this.OnPropertyChanged("ParityList");
            }
        }


        private Util.IO.StopBits _StopBits;
        public Util.IO.StopBits StopBits
        {
            get { return _StopBits; }
            set
            {
                _StopBits = value;
                this.OnPropertyChanged("StopBits");
            }
        }

        private List<Util.IO.StopBits> _StopBitsList;
        public List<Util.IO.StopBits> StopBitsList
        {
            get { return _StopBitsList; }
            set
            {
                _StopBitsList = value;
                this.OnPropertyChanged("StopBitsList");
            }
        }


        private int _ThreadSleep_BeforeRead;
        public int ThreadSleep_BeforeRead
        {
            get { return _ThreadSleep_BeforeRead; }
            set
            {
                _ThreadSleep_BeforeRead = value;
                this.OnPropertyChanged("ThreadSleep_BeforeRead");
            }
        }



        private List<Encoding> _EncodingList;
        public List<Encoding> EncodingList
        {
            get { return _EncodingList; }
            set
            {
                _EncodingList = value;
                this.OnPropertyChanged("EncodingList");
            }
        }


        private Encoding _Encoding;
        public Encoding Encoding
        {
            get { return _Encoding; }
            set
            {
                _Encoding = value;
                this.OnPropertyChanged("Encoding");
            }
        }

        #endregion

        public UcSerialDeviceConfig_ViewModel()
        {
            initCMD();
            initData();
            resetDefault();
        }

        void initData()
        {
            this.PortNameList = Util.IO.SerialPortUtil.GetPortNameList();

            if (this.PortNameList != null && this.PortNameList.Count > 0)
            {
                this.PortName = this.PortNameList[0];
            }

            this.BaudRateList = Util.IO.SerialPortUtil.GetBaudRateList();
            this.DataBitsList = Util.IO.SerialPortUtil.GetDataBitsList();
            this.ParityList = Util.IO.SerialPortUtil.GetParityList();
            this.StopBitsList = Util.IO.SerialPortUtil.GetStopBitsList();

            Encoding[] t = new Encoding[2];
            t[0] = Encoding.UTF8;
            t[1] = Encoding.GetEncoding("gb2312");

            this.EncodingList = t.ToList();
        }

        void initCMD()
        {
            this.CMD_ResetDefault = new Command(resetDefault);
            this.CMD_RefreshPortNameList = new Command(refleshPortNameList);
        }

        #region Command ResetDefault

        public Command CMD_ResetDefault { get; private set; }

        void resetDefault()
        {
            resetSerialDevice();
            resetThreadSleep_BeforeReading();
            resetEncoding();
        }

        /// <summary>
        /// 重置串口设备配置
        /// </summary>
        void resetSerialDevice()
        {
            this.BaudRate = this.BaudRateList.First(i => i.XMLValue == "9600");
            this.DataBits = this.DataBitsList.First(i => i.XMLValue == "8");
            this.Parity = this.ParityList.First(i => i.XMLValue == "NONE");
            this.StopBits = this.StopBitsList.First(i => i.XMLValue == "ONE");
        }

        void resetThreadSleep_BeforeReading()
        {
            this.ThreadSleep_BeforeRead = 80;
        }

        void resetEncoding()
        {
            this.Encoding = this.EncodingList.First(i => i.BodyName == "gb2312");
        }

        #endregion

        #region Command RefreshPortNameList

        public Command CMD_RefreshPortNameList { get; private set; }

        void refleshPortNameList()
        {
            this.PortNameList = Util.IO.SerialPortUtil.GetPortNameList();
        }

        #endregion

    }
}
