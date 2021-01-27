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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client.Components
{
    /// <summary>
    /// Interaction logic for UcSerialDeviceConfig.xaml
    /// </summary>
    public partial class UcSerialDeviceConfigBase : UserControl, System.ComponentModel.IDataErrorInfo
    {
        public UcSerialDeviceConfigBase()
        {
            InitializeComponent();
        }

        #region [DP]GroupBoxHeader

        public static readonly DependencyProperty GroupBoxHeaderProperty = DependencyProperty.Register
        (
            name: "GroupBoxHeader",
            propertyType: typeof(string),
            ownerType: typeof(UcSerialDeviceConfigBase),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: "串口设备配置",
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public string GroupBoxHeader
        {
            get { return (string)GetValue(GroupBoxHeaderProperty); }
            set { SetValue(GroupBoxHeaderProperty, value); }
        }

        #endregion

        #region [DP]PortNameList

        public static readonly DependencyProperty PortNameListProperty = DependencyProperty.Register
        (
            name: "PortNameList",
            propertyType: typeof(object),
            ownerType: typeof(UcSerialDeviceConfigBase),
            validateValueCallback: null, // new ValidateValueCallback((toValidate) => { return toValidate != null; }),
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public object PortNameList
        {
            get { return (object)GetValue(PortNameListProperty); }
            set { SetValue(PortNameListProperty, value); }
        }

        #endregion

        #region [DP]PortName

        public static readonly DependencyProperty PortNameProperty = DependencyProperty.Register
        (
            name: "PortName",
            propertyType: typeof(object),
            ownerType: typeof(UcSerialDeviceConfigBase),
            validateValueCallback: null, // new ValidateValueCallback((toValidate) => { return toValidate != null; }),
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public object PortName
        {
            get { return (object)GetValue(PortNameProperty); }
            set { SetValue(PortNameProperty, value); }
        }

        #endregion

        #region [DP]BaudRateList

        public static readonly DependencyProperty BaudRateListProperty = DependencyProperty.Register
        (
            name: "BaudRateList",
            propertyType: typeof(object),
            ownerType: typeof(UcSerialDeviceConfigBase),
            validateValueCallback: null, // new ValidateValueCallback((toValidate) => { return true; }),
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: null, // onBaudRateList_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public object BaudRateList
        {
            get { return (object)GetValue(BaudRateListProperty); }
            set { SetValue(BaudRateListProperty, value); }
        }

        #endregion

        #region [DP]BaudRate

        public static readonly DependencyProperty BaudRateProperty = DependencyProperty.Register
        (
            name: "BaudRate",
            propertyType: typeof(object),
            ownerType: typeof(UcSerialDeviceConfigBase),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public object BaudRate
        {
            get { return (object)GetValue(BaudRateProperty); }
            set { SetValue(BaudRateProperty, value); }
        }

        #endregion

        #region [DP]DataBitsList

        public static readonly DependencyProperty DataBitsListProperty = DependencyProperty.Register
        (
            name: "DataBitsList",
            propertyType: typeof(object),
            ownerType: typeof(UcSerialDeviceConfigBase),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public object DataBitsList
        {
            get { return (object)GetValue(DataBitsListProperty); }
            set { SetValue(DataBitsListProperty, value); }
        }

        #endregion

        #region [DP]DataBits

        public static readonly DependencyProperty DataBitsProperty = DependencyProperty.Register
        (
            name: "DataBits",
            propertyType: typeof(object),
            ownerType: typeof(UcSerialDeviceConfigBase),
            validateValueCallback: null, // new ValidateValueCallback((toValidate) => { return true; }),
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public object DataBits
        {
            get { return (object)GetValue(DataBitsProperty); }
            set { SetValue(DataBitsProperty, value); }
        }

        #endregion

        #region [DP]ParityList

        public static readonly DependencyProperty ParityListProperty = DependencyProperty.Register
        (
            name: "ParityList",
            propertyType: typeof(object),
            ownerType: typeof(UcSerialDeviceConfigBase),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public object ParityList
        {
            get { return (object)GetValue(ParityListProperty); }
            set { SetValue(ParityListProperty, value); }
        }

        #endregion

        #region [DP]Parity

        public static readonly DependencyProperty ParityProperty = DependencyProperty.Register
        (
            name: "Parity",
            propertyType: typeof(object),
            ownerType: typeof(UcSerialDeviceConfigBase),
            validateValueCallback: null, // new ValidateValueCallback((toValidate) => { return true; }),
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public object Parity
        {
            get { return (object)GetValue(ParityProperty); }
            set { SetValue(ParityProperty, value); }
        }

        #endregion

        #region [DP]StopBitsList

        public static readonly DependencyProperty StopBitsListProperty = DependencyProperty.Register
        (
            name: "StopBitsList",
            propertyType: typeof(object),
            ownerType: typeof(UcSerialDeviceConfigBase),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public object StopBitsList
        {
            get { return (object)GetValue(StopBitsListProperty); }
            set { SetValue(StopBitsListProperty, value); }
        }

        #endregion

        #region [DP]StopBits

        public static readonly DependencyProperty StopBitsProperty = DependencyProperty.Register
        (
            name: "StopBits",
            propertyType: typeof(object),
            ownerType: typeof(UcSerialDeviceConfigBase),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public object StopBits
        {
            get { return (object)GetValue(StopBitsProperty); }
            set { SetValue(StopBitsProperty, value); }
        }

        #endregion

        #region [DP]ThreadSleep_BeforeRead

        public static readonly DependencyProperty ThreadSleep_BeforeReadProperty = DependencyProperty.Register
        (
            name: "ThreadSleep_BeforeRead",
            propertyType: typeof(object),
            ownerType: typeof(UcSerialDeviceConfigBase),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public object ThreadSleep_BeforeRead
        {
            get { return (object)GetValue(ThreadSleep_BeforeReadProperty); }
            set { SetValue(ThreadSleep_BeforeReadProperty, value); }
        }

        #endregion

        #region [DP]EncodingList

        public static readonly DependencyProperty EncodingListProperty = DependencyProperty.Register
        (
            name: "EncodingList",
            propertyType: typeof(object),
            ownerType: typeof(UcSerialDeviceConfigBase),
            validateValueCallback: null, // new ValidateValueCallback((toValidate) => { return true; }),
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public object EncodingList
        {
            get { return (object)GetValue(EncodingListProperty); }
            set { SetValue(EncodingListProperty, value); }
        }

        #endregion

        #region [DP]Encoding

        public static readonly DependencyProperty EncodingProperty = DependencyProperty.Register
        (
            name: "Encoding",
            propertyType: typeof(object),
            ownerType: typeof(UcSerialDeviceConfigBase),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public object Encoding
        {
            get { return (object)GetValue(EncodingProperty); }
            set { SetValue(EncodingProperty, value); }
        }

        #endregion

        #region [DP] Command RefreshPortNameList -- 刷新 PortName 列表

        public static readonly DependencyProperty CMD_RefreshPortNameListProperty = DependencyProperty.Register
        (
            name: "CMD_RefreshPortNameList",
            propertyType: typeof(ICommand),
            ownerType: typeof(UcSerialDeviceConfigBase),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public ICommand CMD_RefreshPortNameList
        {
            get { return (ICommand)GetValue(CMD_RefreshPortNameListProperty); }
            set { SetValue(CMD_RefreshPortNameListProperty, value); }
        }

        #endregion

        #region [DP] Command ResetDefault -- 重置

        public static readonly DependencyProperty CMD_ResetDefaultProperty = DependencyProperty.Register
        (
            name: "CMD_ResetDefault",
            propertyType: typeof(ICommand),
            ownerType: typeof(UcSerialDeviceConfigBase),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public ICommand CMD_ResetDefault
        {
            get { return (ICommand)GetValue(CMD_ResetDefaultProperty); }
            set { SetValue(CMD_ResetDefaultProperty, value); }
        }

        #endregion


        public bool IsValid
        {
            get
            {
                return this.ErrorCollection.Count == 0;
            }
        }

        #region IDataErrorInfo

        public Dictionary<string, string> ErrorCollection { get; private set; } = new Dictionary<string, string>();

        public string Error
        {
            get
            {
                return $"共 {this.ErrorCollection.Values.Count} 个错误";
            }
        }

        public string this[string columnName]
        {
            get
            {
                string r = null;
                switch (columnName)
                {
                    case "PortName":
                        r = checkObjectIsNotNull(this.PortName);
                        break;
                    case "BaudRate":
                        r = checkObjectIsNotNull(this.BaudRate);
                        break;
                    case "DataBits":
                        r = checkObjectIsNotNull(this.DataBits);
                        break;
                    case "Parity":
                        r = checkObjectIsNotNull(this.Parity);
                        break;
                    case "StopBits":
                        r = checkObjectIsNotNull(this.StopBits);
                        break;
                    case "ThreadSleep_BeforeRead":
                        r = checkThreadSleep_BeforeRead();
                        break;
                    case "Encoding":
                        r = checkObjectIsNotNull(this.Encoding);
                        break;
                    default:
                        return string.Empty;
                }

                if (ErrorCollection.ContainsKey(columnName))
                {
                    if (string.IsNullOrWhiteSpace(r))
                    {
                        ErrorCollection.Remove(columnName);
                    }
                    else
                    {
                        ErrorCollection[columnName] = r;
                    }
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(r) == false)
                    {
                        ErrorCollection.Add(columnName, r);
                    }
                }

                this.OnPropertyChanged("ErrorCollection");
                this.OnPropertyChanged("IsValid");

                return r;
            }
        }

        string checkObjectIsNotNull(object obj)
        {
            return obj == null ? "空值" : string.Empty;
        }

        string checkThreadSleep_BeforeRead()
        {
            string r = string.Empty;

            if (this.ThreadSleep_BeforeRead == null || int.TryParse(this.ThreadSleep_BeforeRead.ToString(), out int temp) == false || temp < 0)
            {
                r = "不符合要求";
                return r;
            }

            return r;
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
