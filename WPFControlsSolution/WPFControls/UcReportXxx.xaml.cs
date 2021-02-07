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
    /// V 1.0.0 - 2021-02-07
    /// 报头/终端控件代码首次编写
    /// </summary>
    public partial class UcReportXxx : UserControl, System.ComponentModel.INotifyPropertyChanged, System.ComponentModel.IDataErrorInfo
    {
        public UcReportXxx()
        {
            InitializeComponent();
            initEvent();
        }

        private void initEvent()
        {
            this.Loaded += UcReportXxx_Loaded;

            this.rdBtn_UserDefined_String.Checked += rdBtn_UserDefined_String_Checked;
            this.rdBtn_UserDefined_HexString.Checked += rdBtn_UserDefined_HexString_Checked;

            this.txt_UserDefined_String.IsEnabledChanged += txt_UserDefined_Xxx_IsEnabledChanged;
            this.txt_UserDefined_HexString.IsEnabledChanged += txt_UserDefined_Xxx_IsEnabledChanged;
        }

        private void UcReportXxx_Loaded(object sender, RoutedEventArgs e)
        {
            initUI();
        }

        void gRadioButtonsClear()
        {
            this.gRadioButtons.Children.Clear();
            this.gRadioButtons.ColumnDefinitions.Clear();
            this.gRadioButtons.RowDefinitions.Clear();
        }

        void initUI()
        {
            gRadioButtonsClear();

            if (this.ItemsSource == null)
            {
                throw new ArgumentNullException("ItemsSource 不能为 null");
            }

            if (this.Encoding == null)
            {
                throw new ArgumentNullException("Encoding 不能为 null");
            }

            if (this.Result == null)
            {
                throw new ArgumentNullException("Result 不能为 null");
            }

            System.Collections.IList l = this.ItemsSource;

            int total = l.Count;
            int columnNumber = 2;

            int rowNumber = (total / columnNumber) + (total % columnNumber > 0 ? 1 : 0);

            for (int i = 0; i < columnNumber; i++) { gRadioButtons.ColumnDefinitions.Add(new ColumnDefinition()); }
            for (int i = 0; i < rowNumber; i++) { gRadioButtons.RowDefinitions.Add(new RowDefinition()); }

            bool isMatch = false; // Result 当前值与列表中的选项相同
            int listIndex = 0;
            for (int rowIndex = 0; rowIndex < rowNumber; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < columnNumber; columnIndex++)
                {
                    
                    dynamic match = l[listIndex];

                    RadioButton rdBtn = new RadioButton();
                    rdBtn.GroupName = this.GroupName;
                    rdBtn.Content = match.DisplayName;
                    // rdBtn.DataContext = match;
                    rdBtn.Tag = match;

                    gRadioButtons.Children.Add(rdBtn);
                    Grid.SetRow(rdBtn, rowIndex);
                    Grid.SetColumn(rdBtn, columnIndex);

                    if (match.Value == this.Result.Value)
                    {
                        rdBtn.IsChecked = true;
                        isMatch = true;
                    }

                    rdBtn.Checked += (s, e) =>
                    {
                        RadioButton target = s as RadioButton;
                        dynamic report = target.Tag;

                        this.Result.Value = report.Value;
                        this.Result.DisplayName = report.DisplayName;
                        this.Result.HexString = report.HexString;
                    };

                    listIndex = listIndex + 1;

                    if (listIndex == total) break;
                }
            }


            if (isMatch == false && this.Result != null)
            {
                this.UserDefineHex = this.Result.HexString;
                rdBtn_UserDefined_HexString.IsChecked = true;
            }

        }

        private void rdBtn_UserDefined_String_Checked(object sender, RoutedEventArgs e)
        {
            this.Result.Update(this.UserDefineString);
        }

        private void rdBtn_UserDefined_HexString_Checked(object sender, RoutedEventArgs e)
        {
            this.Result.UpdateByHexString(this.UserDefineHex);
        }

        private void txt_UserDefined_Xxx_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is bool && (bool)e.NewValue == true) // IsEnabled 设置成 true 
            {
                var target = sender as TextBox;
                // step1 : textbox 获取焦点
                target.Focus();
                // step2 : 光标移动到最后
                target.CaretIndex = target.Text.Length;
            }
        }




        #region [DP] Title

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register
        (
            name: "Title",
            propertyType: typeof(string),
            ownerType: typeof(UcReportXxx),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: "标题",
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

        #region [DP] ItemsSources

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register
        (
            name: "ItemsSource",
            propertyType: typeof(System.Collections.IList),
            ownerType: typeof(UcReportXxx),
            validateValueCallback: null, // new ValidateValueCallback((toValidate) => { return true; }),
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: null, // onItemsSource_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public System.Collections.IList ItemsSource
        {
            get { return (System.Collections.IList)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static void onItemsSource_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((d is UcReportXxx) == false) { return; }
            var target = d as UcReportXxx;
            // TODO 其他逻辑
        }

        #endregion

        #region [DP] Result

        public static readonly DependencyProperty ResultProperty = DependencyProperty.Register
        (
            name: "Result",
            propertyType: typeof(object),
            ownerType: typeof(UcReportXxx),
            validateValueCallback: null, // new ValidateValueCallback((toValidate) => { return true; }),
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: null, // onResult_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public dynamic Result
        {
            get { return (dynamic)GetValue(ResultProperty); }
            set { SetValue(ResultProperty, value); }
        }

        public static void onResult_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((d is UcReportXxx) == false) { return; }
            var target = d as UcReportXxx;
            // TODO 其他逻辑
        }

        #endregion

        #region [DP] GroupName

        public static readonly DependencyProperty GroupNameProperty = DependencyProperty.Register
        (
            name: "GroupName",
            propertyType: typeof(string),
            ownerType: typeof(UcReportXxx),
            validateValueCallback: null, // new ValidateValueCallback((toValidate) => { return true; }),
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: onGroupName_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public string GroupName
        {
            get { return (string)GetValue(GroupNameProperty); }
            set { SetValue(GroupNameProperty, value); }
        }

        public static void onGroupName_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((d is UcReportXxx) == false) { return; }
            var target = d as UcReportXxx;

            target.rdBtn_UserDefined_String.GroupName = e.NewValue.ToString();
            target.rdBtn_UserDefined_HexString.GroupName = e.NewValue.ToString();
        }

        #endregion

        #region [DP] Encoding

        public static readonly DependencyProperty EncodingProperty = DependencyProperty.Register
        (
            name: "Encoding",
            propertyType: typeof(Encoding),
            ownerType: typeof(UcReportXxx),
            validateValueCallback: null, // new ValidateValueCallback((toValidate) => { return true; }),
            typeMetadata: new PropertyMetadata
            (
                defaultValue: Encoding.Default,
                propertyChangedCallback: null, // onEncoding_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public Encoding Encoding
        {
            get { return (Encoding)GetValue(EncodingProperty); }
            set { SetValue(EncodingProperty, value); }
        }

        public static void onEncoding_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((d is UcReportXxx) == false) { return; }
            var target = d as UcReportXxx;
            // TODO 其他逻辑
        }

        #endregion

        #region UserDefineString

        private string _UserDefineString;
        public string UserDefineString
        {
            get { return _UserDefineString; }
            set
            {
                _UserDefineString = value;
                this.OnPropertyChanged("UserDefineString");
                this.Result.Update(this.UserDefineString);
            }
        }

        #endregion

        #region UserDefineHex

        private string _UserDefineHex;
        public string UserDefineHex
        {
            get { return _UserDefineHex; }
            set
            {
                _UserDefineHex = value;
                this.OnPropertyChanged("UserDefineHex");

                var errorMsg = checkUserDefineHex();

                if (string.IsNullOrEmpty(errorMsg))
                {
                    this.Result.UpdateByHexString(this.UserDefineHex);
                }
            }
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
                    case "UserDefineHex":
                        errorMsg = this.checkUserDefineHex();
                        break;
                    default:
                        return errorMsg;
                }

                executeErrorCollection(columnName, errorMsg);

                return errorMsg;
            }
        }

        #endregion

        string checkUserDefineHex()
        {
            string errorMsg = null;

            if (string.IsNullOrEmpty(this.UserDefineHex) == false)
            {
                int modNumber = 2;

                if (this.Encoding.CodePage == Encoding.Unicode.CodePage) { modNumber = 4; }

                if (this.UserDefineHex.Length % modNumber != 0)
                {
                    errorMsg = "不符合Hex长度验证";
                }

                if (System.Text.RegularExpressions.Regex.IsMatch(this.UserDefineHex, "^[0-9A-Fa-f]*$") == false)
                {
                    errorMsg = "不符合Hex验证";
                }
            }

            return errorMsg;
        }




        #region INotifyPropertyChanged成员

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            this.PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }

}