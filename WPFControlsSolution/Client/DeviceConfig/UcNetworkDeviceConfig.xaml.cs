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

/// <summary>
/// V 1.0.0 - 2021-01-27 11:03:00
/// 首次创建
/// </summary>
namespace Client.Components
{
    /// <summary>
    /// Interaction logic for UcNetworkDeviceConfig.xaml
    /// </summary>
    public partial class UcNetworkDeviceConfig : UserControl
    {
        public UcNetworkDeviceConfig()
        {
            InitializeComponent();
        }

        #region [DP]Title

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register
        (
            name: "Title",
            propertyType: typeof(string),
            ownerType: typeof(UcNetworkDeviceConfig),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: "网口设备配置",
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

        public bool IsValidated
        {
            get
            {
                if (this.ucDeviceConfig == null)
                {
                    return false;
                }

                return this.ucDeviceConfig.IsValidated;
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
                r.Host = ucDeviceConfig.Host;
                r.Port = ucDeviceConfig.Port;
                r.Encoding = ucDeviceConfig.Encoding;
                return r;
            }
        }
    }

    public class UcNetworkDeviceConfig_ViewModel : BaseViewModel
    {
        #region Props

        private string _Host;
        public string Host
        {
            get { return _Host; }
            set
            {
                _Host = value;
                this.OnPropertyChanged("Host");
            }
        }


        private string _PortPattern = Util.RegexUtils.Port;
        public string PortPattern
        {
            get { return _PortPattern; }
            set
            {
                _PortPattern = value;
                this.OnPropertyChanged("PortPattern");
            }
        }


        private int _Port;
        public int Port
        {
            get { return _Port; }
            set
            {
                _Port = value;
                this.OnPropertyChanged("Port");
            }
        }


        private string _HostPattern = Util.RegexUtils.IPAddress;
        public string HostPattern
        {
            get { return _HostPattern; }
            set
            {
                _HostPattern = value;
                this.OnPropertyChanged("HostPattern");
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


        private Encoding[] _EncodingList;
        public Encoding[] EncodingList
        {
            get { return _EncodingList; }
            set
            {
                _EncodingList = value;
                this.OnPropertyChanged("EncodingList");
            }
        }

        #endregion

        public UcNetworkDeviceConfig_ViewModel()
        {
            initData();
            initCMD();
            reset();
        }

        void initData()
        {
            Encoding[] t = new Encoding[2];
            t[0] = Encoding.UTF8;
            t[1] = Encoding.GetEncoding("gb2312");

            this._EncodingList = t;
        }

        void initCMD()
        {
            this.CMD_ResetDefault = new Command(reset);
        }

        #region Command Reset

        public Command CMD_ResetDefault { get; private set; }        

        void reset()
        {
            resetDefault();
            resetEncoding();
        }

        void resetDefault()
        {
            this.Host = "";
            this.Port = 0;
        }

        void resetEncoding()
        {
            this.Encoding = EncodingList[0];
        }

        #endregion

        #region 调用参考代码

        public ICommand CMD_Submit { get; private set; }

        public ICommand CMD_Cancel { get; private set; }

        void submit(object args)
        {
            if (args == null || args is Client.Components.UcNetworkDeviceConfigBase == false)
            {
                System.Diagnostics.Debug.WriteLine("参数不是预期 Client.Components.UcNetworkDeviceConfig");
                return;
            }

            Client.Components.UcNetworkDeviceConfigBase uc = args as Client.Components.UcNetworkDeviceConfigBase;
            if (uc.IsValidated == false)
            {
                System.Diagnostics.Debug.WriteLine(uc.Error);
                return;
            }

            string msg = $"Host: {this.Host}\r\nPort: {this.Port}\r\nEncoding: {this.Encoding.BodyName}";
            System.Diagnostics.Debug.WriteLine(msg);
        }

        #endregion

    }
}
