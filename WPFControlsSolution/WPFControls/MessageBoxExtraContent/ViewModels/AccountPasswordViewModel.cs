using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFControls.MessageBoxExtraContent.ViewModels
{
    public class AccountPasswordViewModel : System.ComponentModel.INotifyPropertyChanged, System.ComponentModel.IDataErrorInfo
    {
        #region INotifyPropertyChanged成员

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            this.PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }

        #endregion

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

        private string _Password;
        public string Password
        {
            get { return _Password; }
            set
            {
                _Password = value;
                this.OnPropertyChanged(nameof(Password));
            }
        }

        #region 实现 IDataError _ 代码版本 2021_10_27

        public bool IsValidated
        {
            get
            {
                return this.ErrorCollection.Count == 0;
            }
        }

        public string Error
        {
            get
            {
                if (this.ErrorCollection.Values.Count <= 0)
                    return string.Empty;
                else
                    return $"共 {this.ErrorCollection.Values.Count} 个错误";
            }
        }

        public System.Collections.Generic.Dictionary<string, string> ErrorCollection { get; private set; } = new System.Collections.Generic.Dictionary<string, string>();

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
                string errorMsg = null;

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
                    case "LoginAccount":
                        // 方式一 将 validationResults 传进去 ( 推荐 可以分别将不符合的点一个个添加到集合里面, 方便最后一项一项地罗列出来 )
                        this.checkLoginAccount(validationResults);
                        break;

                    case "Password":
                        // 方式一 将 validationResults 传进去 ( 推荐 可以分别将不符合的点一个个添加到集合里面, 方便最后一项一项地罗列出来 )
                        this.checkPassword(validationResults);
                        break;

                    //case "Nickname2":
                    //// 方式二 写一个验证方法返回验证信息
                    //    addValidationResult(validationResults, this.checkNickname2());
                    //    break;

                    default:
                        break;
                }

                if (validationResults.Count > 0)
                {
                    errorMsg = WPFControls.LinqToString.CombineStringWithSeq(validationResults, isShowSeqEvenOnlyOneItem: false);
                }

                executeErrorCollection(columnName, errorMsg);

                return errorMsg;
            }
        }
        void checkLoginAccount(List<System.ComponentModel.DataAnnotations.ValidationResult> l)
        {
            if (string.IsNullOrEmpty(this.LoginAccount) == true)
            {
                addValidationResult(l, "请输入账号");
            }
        }

        void checkPassword(List<System.ComponentModel.DataAnnotations.ValidationResult> l)
        {
            if (string.IsNullOrEmpty(this.Password) == true)
            {
                addValidationResult(l, "请输入密码");
            }
        }


        #endregion
    }
}
