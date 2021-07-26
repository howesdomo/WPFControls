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
using System.Windows.Shapes;

namespace Client.Test
{
    /// <summary>
    /// Interaction logic for FrmTest_UcConsole_Performance.xaml
    /// </summary>
    public partial class FrmTest_UcConsole_Performance : Window, INotifyPropertyChanged
    {
        System.IO.Ports.SerialPort mSerialPort { get; set; }

        // TODO 了解 event System.IO.Ports.SerialDataReceivedEventHandler mSerialPort_DataReceived 的正确使用方法
        public event System.IO.Ports.SerialDataReceivedEventHandler mSerialPort_DataReceived
        {
            add
            {
                //Add method call to delegate
                mSerialPort.DataReceived += value;
                DataReceivedIsReg = true;
            }
            remove
            {
                //Remove method call to delegate
                mSerialPort.DataReceived -= value;
                DataReceivedIsReg = false;
            }
        }

        private bool _DataReceivedIsReg;
        public bool DataReceivedIsReg
        {
            get { return _DataReceivedIsReg; }
            set
            {
                _DataReceivedIsReg = value;
                this.OnPropertyChanged(nameof(DataReceivedIsReg));
            }
        }

        public int MinRSSI { get; set; } = -44;

        public int MaxRSSI { get; set; } = 0;

        public FrmTest_UcConsole_Performance()
        {
            InitializeComponent();
            initSerialPort("COM4");
            initCMD();
            this.Closed += FrmTest_UcConsole_Performance_Closed;
        }

        private void FrmTest_UcConsole_Performance_Closed(object sender, EventArgs e)
        {
            if (mSerialPort != null)
            {
                mSerialPort_DataReceived -= mSerialPort_DataReceived_OnHandle;
                mSerialPort.Close();
                mSerialPort = null;
            }
        }

        void initSerialPort(string com)
        {
            try
            {
                mSerialPort = new System.IO.Ports.SerialPort();

                mSerialPort.PortName = com;
                mSerialPort.BaudRate = 115200;
                mSerialPort.DataBits = 8;
                mSerialPort.Parity = System.IO.Ports.Parity.None;
                mSerialPort.StopBits = System.IO.Ports.StopBits.One;

                // mSerialPort.DataReceived += mSerialPort_DataReceived;
                mSerialPort_DataReceived += mSerialPort_DataReceived_OnHandle;

                mSerialPort.Open();
            }
            catch (Exception)
            {
                if (mSerialPort != null)
                {
                    mSerialPort_DataReceived -= mSerialPort_DataReceived_OnHandle;
                    mSerialPort = null;
                }
            }
        }

        void initCMD()
        {
            this.CMD_Pause = new Command(Pause);
            this.CMD_Resume = new Command(Resume);

        }

        private void btnPause(object sender, RoutedEventArgs e)
        {
            Pause();
        }

        public Command CMD_Pause { get; private set; }
        void Pause()
        {
            if (this.DataReceivedIsReg == true)
            {
                mSerialPort_DataReceived -= mSerialPort_DataReceived_OnHandle;
            }
        }


        private void btnResume(object sender, RoutedEventArgs e)
        {
            Resume();
        }


        public Command CMD_Resume { get; private set; }
        void Resume()
        {
            if (this.DataReceivedIsReg == false)
            {
                mSerialPort_DataReceived += mSerialPort_DataReceived_OnHandle;
            }
        }


        private void mSerialPort_DataReceived_OnHandle(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            string content = mSerialPort.ReadLine();

            content = content.Trim();

            if (BlueToothModel.IsPass(content) == false)
            {
                return;
            }

            BlueToothModel m = new BlueToothModel(content);

            if (MinRSSI <= m.RSSI && m.RSSI <= MaxRSSI)
            {
                //this.q.Enqueue(m);

                //if (mCurrentBlueToothModel == null)
                //{
                //    mCurrentBlueToothModel = m;
                //}

                var toAdd = new Util.Model.ConsoleData(m.ToString(), Util.Model.ConsoleMsgType.INFO, m.ScanDateTime);

                //MessagingCenter.Send<Util.Model.ConsoleData>(message: "ucSerialPortFilterConsole", sender: toAdd);

                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    ucConsole.Add(toAdd);
                }));
            }
            else
            {
                var toAdd = new Util.Model.ConsoleData(m.ToString(), Util.Model.ConsoleMsgType.DEBUG, m.ScanDateTime);
                // MessagingCenter.Send<Util.Model.ConsoleData>(message: "ucSerialPortConsole", sender: toAdd );

                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    ucConsole.Add(toAdd);
                }));
            }
        }


        #region INotifyPropertyChanged成员

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion


        /// <summary>
        /// 2017-8-3 11:00:06
        /// ===============
        /// V1.01初版，
        /// 		
        /// 		1.波特率115200，TXD：P0.09 ，RXD：P0.11
        /// 		2.LED指示：500ms翻转一次，P0.21
        /// 		3.扫描间隔30ms，扫描窗口30ms，全速扫描
        /// 		4.输出格式(ASCII码格式)：
        /// 			$AA:BB:CC:DD:EE:FF,0,-45,XXXXXXXXXX...XX\r\n
        /// 			$AA:BB:CC:DD:EE:FF,1,-45,XXXXXXXXXX...XX\r\n
        /// 		
        /// 		说明：$ 为起始符
        /// 					0 表示当前为一般广播包
        /// 					1 表示当前为扫描请求响应包
        /// 					AA:BB:CC:DD:EE:FF 为MAC地址
        /// 					-45 为信号强度，大于或等于-99，小于-100过滤
        /// 					XXXXXXXXXX...XX为数据内容，固定31字节的ASCII格式（不够补00）
        /// 					\r\n 结束符
        /// </summary>
        public class BlueToothModel : BaseViewModel
        {
            public static bool IsPass(string content)
            {
                return content.StartsWith("$") && content.Split(',').Length == 4;
            }

            public BlueToothModel(string content)
            {
                string[] arr = content.Split(',');

                if (arr[0].Length == 18)
                {
                    MAC = arr[0].Substring(1);
                }

                if (int.TryParse(arr[2], out int temp))
                {
                    RSSI = temp;
                }

                ScanDateTime = DateTime.Now;
            }


            private string _MAC;
            public string MAC
            {
                get { return _MAC; }
                set
                {
                    _MAC = value;
                    this.OnPropertyChanged(nameof(MAC));
                }
            }

            private int _RSSI = -9999;
            public int RSSI
            {
                get { return _RSSI; }
                set
                {
                    _RSSI = value;
                    this.OnPropertyChanged(nameof(RSSI));
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


            public override string ToString()
            {
                return $"{MAC},{RSSI}";
            }

            public override bool Equals(object obj)
            {
                if (obj is BlueToothModel)
                {
                    return MAC.Equals(obj);
                }
                else
                {
                    return false;
                }
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }
        
    }
}
