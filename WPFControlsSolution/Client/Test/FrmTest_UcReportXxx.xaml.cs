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
    /// Interaction logic for FrmTest_UcReportXxx.xaml
    /// </summary>
    public partial class FrmTest_UcReportXxx : Window
    {
        public FrmTest_UcReportXxx()
        {
            InitializeComponent();

        }
    }

    public class FrmTest_UcReportXxx_ViewModel : BaseViewModel, System.ComponentModel.IDataErrorInfo
    {
        public Command CMD_Show { get; private set; }

        public FrmTest_UcReportXxx_ViewModel()
        {
            // 报头
            this.ReportHeadList = Report.GetReportHeadList(Encoding.UTF8);
            // 1 初次选择模式
            // this.ReportHead = new Report(Encoding.UTF8);

            // 2 编辑模式
            var report1 = new Report(Encoding.UTF8);
            report1.Update(this.ReportHeadList[1].Value); // [STX]
            this.ReportHead = report1;


            // 终端
            this.ReportEndList = Report.GetReportEndList(Encoding.Unicode);
            // 1 初次选择模式
            // this.ReportEnd = new Report(Encoding.Unicode);

            // 2 编辑模式
            var report2 = new Report(Encoding.Unicode);
            report2.Update("\n\r");
            this.ReportEnd = report2; // 自定义 LF + CR

            this.CMD_Show = new Command(() =>
            {
                System.Diagnostics.Debug.WriteLine(Util.JsonUtils.SerializeObjectWithFormatted(this.ReportHead));
                System.Diagnostics.Debug.WriteLine(Util.JsonUtils.SerializeObjectWithFormatted(this.ReportEnd));
                System.Diagnostics.Debug.WriteLine(HexValue);
            });
        }

        private List<Report> _ReportHeadList;
        public List<Report> ReportHeadList
        {
            get { return _ReportHeadList; }
            set
            {
                _ReportHeadList = value;
                this.OnPropertyChanged("ReportHeadList");
            }
        }


        private Report _ReportHead;
        public Report ReportHead
        {
            get { return _ReportHead; }
            set
            {
                _ReportHead = value;
                this.OnPropertyChanged("ReportHead");
            }
        }


        private List<Report> _ReportEndList;
        public List<Report> ReportEndList
        {
            get { return _ReportEndList; }
            set
            {
                _ReportEndList = value;
                this.OnPropertyChanged("ReportEndList");
            }
        }


        private Report _ReportEnd;
        public Report ReportEnd
        {
            get { return _ReportEnd; }
            set
            {
                _ReportEnd = value;
                this.OnPropertyChanged("ReportEnd");
            }
        }

        private Encoding _ReportEndEncoding = Encoding.Unicode;
        public Encoding ReportEndEncoding
        {
            get { return _ReportEndEncoding; }
            set
            {
                _ReportEndEncoding = value;
                this.OnPropertyChanged("ReportEndEncoding");
            }
        }



        private string _HexValue;
        public string HexValue
        {
            get { return _HexValue; }
            set
            {
                _HexValue = value;
                this.OnPropertyChanged("HexValue");
            }
        }

        public bool IsValid
        {
            get
            {
                return this.ErrorCollection.Count == 0;
            }
        }

        #region IDataErrorInfo

        public string Error
        {
            get
            {
                return $"共 {this.ErrorCollection.Values.Count} 个错误";
            }
        }

        public Dictionary<string, string> ErrorCollection { get; private set; } = new Dictionary<string, string>();

        void executeErrorCollection(string columnName, string errorMsg)
        {
            if (ErrorCollection.ContainsKey(columnName))
            {
                if (string.IsNullOrWhiteSpace(errorMsg))
                {
                    ErrorCollection.Remove(columnName);
                }
                else
                {
                    ErrorCollection[columnName] = errorMsg;
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(errorMsg) == false)
                {
                    ErrorCollection.Add(columnName, errorMsg);
                }
            }

            this.OnPropertyChanged("ErrorCollection");
            this.OnPropertyChanged("Error");
            this.OnPropertyChanged("IsValid");
        }

        public string this[string columnName]
        {
            get
            {
                string errorMsg = null;
                switch (columnName)
                {
                    case "HexValue":
                        errorMsg = this.checkHexValue();
                        break;
                    default:
                        return errorMsg;
                }

                executeErrorCollection(columnName, errorMsg);

                return errorMsg;
            }
        }

        string checkHexValue()
        {
            string r = null;

            if (string.IsNullOrEmpty(this.HexValue))
            {
                return r;
            }

            if (System.Text.RegularExpressions.Regex.IsMatch(this.HexValue, "^[0-9A-Fa-f]*$") == false)
            {
                r = "请输入正确的Hex值";
            }

            return r;

        }

        #endregion


    }


    /// <summary>
    /// 发送信息的报头 OR 终端
    /// </summary>
    public class Report : System.ComponentModel.INotifyPropertyChanged
    {
        public Report(Encoding e)
        {
            this.Encoding = e;
            this.Value = string.Empty;
            this.DisplayName = "无";
            this.HexString = string.Empty;
        }

        public Report(Encoding e, string value)
        {
            this.Encoding = e;
            this.Value = value;
            this.DisplayName = Client.ValueConverters.StringShowSpecialSymbolConverter.StringShowSpecialSymbol(this.Value);
            this.HexString = Report.String2HexString(this.Value, e);
        }


        private string _Value;
        public string Value
        {
            get { return _Value; }
            set
            {
                _Value = value;
                this.OnPropertyChanged("Value");
            }
        }


        private Encoding _Encoding;
        [Newtonsoft.Json.JsonIgnore]
        public Encoding Encoding
        {
            get { return _Encoding; }
            set
            {
                _Encoding = value;
                this.OnPropertyChanged("Encoding");
            }
        }


        public int EncodingCodePage
        {
            get
            {
                int codePage = -1;
                if (Encoding != null)
                {
                    codePage = this.Encoding.CodePage;
                }
                return codePage;
            }
            set
            {
                this.Encoding = Encoding.GetEncoding(value);
            }
        }


        private string _DisplayName;
        public string DisplayName
        {
            get { return _DisplayName; }
            set
            {
                _DisplayName = value;
                this.OnPropertyChanged("DisplayName");
            }
        }


        private string _HexString;
        public string HexString
        {
            get { return _HexString; }
            set
            {
                _HexString = value;
                this.OnPropertyChanged("HexString");
            }
        }




        public void Update(string strValue)
        {
            this.Value = strValue;
            this.DisplayName = strValue;
            this.HexString = String2HexString(strValue, this.Encoding);
        }



        public void UpdateByHexString(string hexValue)
        {
            byte[] byteArr = HexString2ByteArray(hexValue);

            string args = string.Empty;
            if (byteArr != null)
            {
                args = this.Encoding.GetString(byteArr);
            }

            this.Update(args);
        }





        public static byte[] HexString2ByteArray(string hexString)
        {
            if (string.IsNullOrEmpty(hexString))
            {
                return null;
            }

            if (hexString.Length % 2 != 0)
            {
                throw new ArgumentException(String.Format(System.Globalization.CultureInfo.InvariantCulture, "The binary key cannot have an odd number of digits: {0}", hexString));
            }

            byte[] data = new byte[hexString.Length / 2];

            for (int index = 0; index < data.Length; index++)
            {
                string byteValue = hexString.Substring(index * 2, 2);
                data[index] = byte.Parse(byteValue, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture);
            }

            return data;
        }


        public static string String2HexString(string msg, Encoding encoding)
        {
            if (string.IsNullOrEmpty(msg))
            {
                return null;
            }

            byte[] buf = encoding.GetBytes(msg);
            StringBuilder sb = new StringBuilder();

            foreach (byte i in buf)
            {
                // sb.Append($" {string.Format("{0:X}", i).PadLeft(2, '0')}"); // [Space]0A
                sb.Append($"{string.Format("{0:X}", i).PadLeft(2, '0')}"); // [Space]0A
            }

            return sb.ToString().Trim();
        }


        #region 获取 ReportHeadList 或者 ReportEndList

        public static List<Report> GetReportHeadList(Encoding encoding)
        {
            List<Report> l = new List<Report>();
            l.Add(new Report(encoding, string.Empty)
            {
                DisplayName = "无"
            });

            // STX 
            // ASCII 与 UTF8 : 0x02
            l.Add(new Report(encoding, char.ConvertFromUtf32(2)));

            // ESC 
            // ASCII 与 UTF8 : 0x1B
            l.Add(new Report(encoding, char.ConvertFromUtf32(27)));

            return l;
        }

        public static List<Report> GetReportEndList(Encoding encoding)
        {
            List<Report> l = new List<Report>();

            l.Add(new Report(encoding, string.Empty)
            {
                DisplayName = "无"
            });

            // CR 
            // ASCII 与 UTF8 : 0x0D 
            l.Add(new Report(encoding, char.ConvertFromUtf32(13)));

            // ETX 
            // ASCII 与 UTF8 : 0x03
            l.Add(new Report(encoding, char.ConvertFromUtf32(3)));

            // CR LF 
            // ASCII 与 UTF8 : 0x0D 0x0A
            l.Add(new Report(encoding, "\r\n"));

            return l;
        }

        #endregion

        #region INotifyPropertyChanged成员

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            this.PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
