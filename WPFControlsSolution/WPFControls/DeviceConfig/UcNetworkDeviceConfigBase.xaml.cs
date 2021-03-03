using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    /// V 1.0.2 2021-03-03 15:53:27
    /// 1 加入了 DataAnnotations, 并且与 IDataError 结合, 进行数据的验证
    /// 2 优化 UI ErrorContent, 采用 ErrorContent 显示验证异常信息
    /// </summary>
    public partial class UcNetworkDeviceConfigBase : UserControl, System.ComponentModel.IDataErrorInfo
    {
        public UcNetworkDeviceConfigBase()
        {
            InitializeComponent();
        }

        #region [DP]GroupBoxHeader

        public static readonly DependencyProperty GroupBoxHeaderProperty = DependencyProperty.Register
        (
            name: "GroupBoxHeader",
            propertyType: typeof(string),
            ownerType: typeof(UcNetworkDeviceConfigBase),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: "网口设备配置",
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

        #region [DP]Host

        public static readonly DependencyProperty HostProperty = DependencyProperty.Register
        (
            name: "Host",
            propertyType: typeof(object),
            ownerType: typeof(UcNetworkDeviceConfigBase),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        [Required(ErrorMessage = "空值")]
        public object Host
        {
            get { return (object)GetValue(HostProperty); }
            set { SetValue(HostProperty, value); }
        }

        #endregion

        #region [DP] Regex Pattern - Host

        public static readonly DependencyProperty HostPatternProperty = DependencyProperty.Register
        (
            name: "HostPattern",
            propertyType: typeof(string),
            ownerType: typeof(UcNetworkDeviceConfigBase),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: @"^(\d|[1-9]\d|1\d{2}|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d{2}|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d{2}|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d{2}|2[0-4]\d|25[0-5])$",
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public string HostPattern
        {
            get { return (string)GetValue(HostPatternProperty); }
            set { SetValue(HostPatternProperty, value); }
        }

        #endregion

        #region [DP]Port

        public static readonly DependencyProperty PortProperty = DependencyProperty.Register
        (
            name: "Port",
            propertyType: typeof(object),
            ownerType: typeof(UcNetworkDeviceConfigBase),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        [Required(ErrorMessage = "空值")]
        public object Port
        {
            get { return (object)GetValue(PortProperty); }
            set { SetValue(PortProperty, value); }
        }

        #endregion

        #region [DP] Regex Pattern - Port

        public static readonly DependencyProperty PortPatternProperty = DependencyProperty.Register
        (
            name: "PortPattern",
            propertyType: typeof(string),
            ownerType: typeof(UcNetworkDeviceConfigBase),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: @"^([0-9]|[1-9]\d{1,3}|[1-5]\d{4}|6[0-4]\d{3}|65[0-4]\d{2}|655[0-2]\d|6553[0-5])$",
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public string PortPattern
        {
            get { return (string)GetValue(PortPatternProperty); }
            set { SetValue(PortPatternProperty, value); }
        }

        #endregion

        #region [DP]EncodingList

        public static readonly DependencyProperty EncodingListProperty = DependencyProperty.Register
        (
            name: "EncodingList",
            propertyType: typeof(object),
            ownerType: typeof(UcNetworkDeviceConfigBase),
            validateValueCallback: null,
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
            propertyType: typeof(System.Text.Encoding),
            ownerType: typeof(UcNetworkDeviceConfigBase),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        [Required(ErrorMessage = "空值")]
        public System.Text.Encoding Encoding
        {
            get { return (System.Text.Encoding)GetValue(EncodingProperty); }
            set { SetValue(EncodingProperty, value); }
        }

        #endregion

        #region [DP] Command ResetDefault -- 重置

        public static readonly DependencyProperty CMD_ResetDefaultProperty = DependencyProperty.Register
        (
            name: "CMD_ResetDefault",
            propertyType: typeof(ICommand),
            ownerType: typeof(UcNetworkDeviceConfigBase),
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

        public bool IsValidated
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

        protected void executeErrorCollection(string columnName, string errorMsg)
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
            this.OnPropertyChanged("IsValidated");
        }

        /// <summary>
        /// 判断errorMessage, 若符合条件则添加到 validationResultList 中
        /// </summary>
        /// <param name="validationResultList"></param>
        /// <param name="errorMessage"></param>
        protected void addValidationResult
        (
            ICollection<System.ComponentModel.DataAnnotations.ValidationResult> validationResultList,
            string errorMessage
        )
        {
            if (string.IsNullOrEmpty(errorMessage) == false)
            {
                validationResultList.Add(new System.ComponentModel.DataAnnotations.ValidationResult(errorMessage));
            }
        }

        public string this[string columnName]
        {
            get
            {
                string r = null;

                // Step 1 根据 System.ComponentModel.DataAnnotations 进行校验
                var vc = new System.ComponentModel.DataAnnotations.ValidationContext(this, null, null);
                vc.MemberName = columnName;
                var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
                System.ComponentModel.DataAnnotations.Validator.TryValidateProperty
                (
                    value: this.GetType().GetProperty(columnName).GetValue(this, null),
                    validationContext: vc,
                    validationResults: validationResults
                );

                // Step 2 根据额外编写的校验逻辑进行校验
                switch (columnName)
                {
                    case "Host":
                        checkHost(validationResults);
                        break;
                    case "Port":
                        checkPort(validationResults);
                        break;
                    default:
                        break;
                }

                if (validationResults.Count > 0)
                {
                    r = WPFControls.LinqToString.CombineStringWithSeq(validationResults, isShowSeqEvenOnlyOneItem: false);
                }

                executeErrorCollection(columnName, r);

                return r;
            }
        }

        string checkHost(List<System.ComponentModel.DataAnnotations.ValidationResult> l)
        {
            string r = string.Empty;

            if (this.Host != null && string.IsNullOrEmpty(this.HostPattern) == false)
            {
                bool b = System.Text.RegularExpressions.Regex.IsMatch
                (
                    input: this.Host.ToString(),
                    pattern: this.HostPattern
                );

                if (b == false)
                {
                    l.Add(new System.ComponentModel.DataAnnotations.ValidationResult("不符合正则表达式校验"));
                }
            }

            return r;
        }

        string checkPort(List<System.ComponentModel.DataAnnotations.ValidationResult> l)
        {
            string r = string.Empty;

            if (this.Port != null && string.IsNullOrEmpty(this.PortPattern) == false)
            {
                bool b = System.Text.RegularExpressions.Regex.IsMatch
                (
                    input: this.Port.ToString(),
                    pattern: this.PortPattern
                );

                if (b == false)
                {
                    l.Add(new System.ComponentModel.DataAnnotations.ValidationResult("端口必须在 0-65535 范围中"));
                }
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
