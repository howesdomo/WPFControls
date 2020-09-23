using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// UcSelectFile.xaml 的交互逻辑
    /// </summary>
    public partial class UcSelectFile : UserControl
    {
        #region 控件属性

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(UcSelectFile), new PropertyMetadata(new PropertyChangedCallback(sTitlePropertyChangedCallback)));

        static void sTitlePropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            UcSelectFile uc = (sender as UcSelectFile);
            if (uc != null)
            {
                uc.lblLeftTitle.Text = e.NewValue.ToString();
            }
        }

        public string Title
        {
            get
            {
                // *** CLR Wrapper *** 
                // -- 不会进入这里的 Get Set , 不要尝试在这里进行编码, 需要赋值请使用 PropertyChangedCallback
                //string r = (string)GetValue(LeftTitleProperty);
                //if (string.IsNullOrWhiteSpace(r))
                //{
                //    r = "选择";
                //}
                //return r;
                return (string)GetValue(TitleProperty);
            }
            set
            {
                SetValue(TitleProperty, value);
            }
        }


        public static readonly DependencyProperty FilterProperty = DependencyProperty.Register("Filter", typeof(string), typeof(UcSelectFile));

        public string Filter
        {
            get
            {
                return (string)GetValue(FilterProperty);
            }
            set
            {
                SetValue(FilterProperty, value);
            }
        }

        public static readonly DependencyProperty MultiselectProperty = DependencyProperty.Register("Multiselect", typeof(bool), typeof(UcSelectFile));

        public bool Multiselect
        {
            get
            {
                return (bool)GetValue(MultiselectProperty);
            }
            set
            {
                SetValue(MultiselectProperty, value);
            }
        }

        public static readonly DependencyProperty FileNameProperty = DependencyProperty.Register("FileName", typeof(string), typeof(UcSelectFile));

        public string FileName
        {
            get
            {
                return (string)GetValue(FileNameProperty);
            }
            set
            {
                SetValue(FileNameProperty, value);
            }
        }

        public static readonly DependencyProperty FileNamesProperty = DependencyProperty.Register("FileNames", typeof(string[]), typeof(UcSelectFile));

        public string[] FileNames
        {
            get
            {
                return (string[])GetValue(FileNamesProperty);
            }
            set
            {
                SetValue(FileNamesProperty, value);
            }
        }


        public static readonly DependencyProperty RegularExpressionFilterProperty = DependencyProperty.Register("RegularExpressionFilter", typeof(string), typeof(UcSelectFile));

        public string RegularExpressionFilter
        {
            get
            {
                return (string)GetValue(RegularExpressionFilterProperty);
            }
            set
            {
                SetValue(RegularExpressionFilterProperty, value);
            }
        }



        public static readonly DependencyProperty IsSuccessProperty = DependencyProperty.Register("IsSuccess", typeof(bool), typeof(UcSelectFile));

        /// <summary>
        /// 通过校验
        /// 1 FileNames 至少有1个元素
        /// 2 FileNames 所有元素都通过 File.Exists
        /// 3 FileNames 所有元素都通过 RegularExpressionFilter 正则表达式的校验
        /// </summary>
        public bool IsSuccess
        {
            get
            {
                return (bool)GetValue(IsSuccessProperty);
            }
            private set
            {
                SetValue(IsSuccessProperty, value);
            }
        }

        #endregion

        public UcSelectFile()
        {
            InitializeComponent();
            initUI();
            initEvent();
        }

        private void initUI()
        {
            string base64_Error = "iVBORw0KGgoAAAANSUhEUgAAAIAAAACACAMAAAD04JH5AAAAA3NCSVQICAjb4U/gAAAACXBIWXMAACNvAAAjbwE1/Af7AAAAGXRFWHRTb2Z0d2FyZQB3d3cuaW5rc2NhcGUub3Jnm+48GgAAANVQTFRF////11pK11pK11pK11pK11pK11pK11pK11pK11pK11pK11pK11pK11pK11pK11pK11pK11pK11pK11pK11pK11pK11pK11pK11pK11pK11pK11pK11pK11pK11pK11pK11pK11pK11pK11pK11pK11pK11pK11pK11pK11pK11pK11pK11pK11pK11pK11pK11pK11pK11pK11pK11pK11pK11pK11pK2F9P2WBR2WJT2mVW33pt5ZSJ5peN6J+V6KGY/PHv/PHw/PPy/PTz/fb1////fP+U/wAAADd0Uk5TAAEDBAYLDA4VHB4fNDg8PUJDR0pbc3yCg4SGiImVlqWnq661uLq/wsPFy9DT2d7m6+zt8/n6/uYUS2MAAANwSURBVHjazVvnWiJBEBwjBjwRCSfgkWHJectAUnTf/5FuDeehIhN3euv3fFu1aaa7upsxFUSiiXS+7LS6/fG432055Xw6EY0wKziI52o9bESvlosfBEq+H7tqTLAVk8bv2H5A9GfXQwhheH1mnv001YEEOqlTo/TRkgtJuKWoMfrzCpRQOTdCf1GHMuoX2vTHRWiheKxFv5ccQROj5J46/682DKD9S5F+N+PCCNzMrtLbd2AMjsKXcDmAQQwuJel3si6Mws3uSJ06BRhHQeKMOqwiAFQPRfmPmggEzSMx/pMOAkLnROj+A+P3FQg8g8MmAkST+x3sVxEoqpx/YaeAgFHYvh9kETiyW/dfN3gB7pZd+XgACxj8eDLtOrAC56fTOQNLyPwQ/7i2BLgbY6S9NqyhvSlOTMIikhv+gJFNAaPvf0IRVlH8lv/AMr7mTHXbAupf8k9Yx+fMtWJfQOVT/g8CrPsHJe7q28XsRvzad8v5PXdRac1/4W7C9yvPe5iK8k8fPW91y92Q/7s4Ke4l556PJ0EF06eX1QvuutSHAH4cvPTEFbzxezN+jPzhvwm81EdhBe/8DwKfzD8371r8tvgKhBf6+PMeiQ9hToEMP4ZvMXpM5tPiXFqKH4i9CriCMQWS/Lh6FdCAKQWy/Gi8+u8TGFIgzY/Ji7sfl9i+t1LI8wNxX0AOZhSo8CPnC6jBiAIlftR8AT2YUKDGjx5jEdljfCOVIj8QUYhFNpAp8/tRSQL6CtT5kWBpaCvQ4Eea5aGrQIcfeVaGpgItfpSZoinxQavHD4e1oKXg+VmLHy3WhZ4CPX50WR/6CtT50Wdj9dRm+vb4vWd1fozpBZC/AvKPkPw3JN+IyLdi8sOI/DgmD0jIQzLyoJQ8LKdPTMhTM/LklDw9Jzco6C0acpOK3KYjNypFrNqbB1mr9vGOv/RM3KyeyZvVS+7CjoRdv5C36+fcdSmJgsXtyn+o4gUL/4WtuCWTtYKFQMnmfr68E97gcDNbcAsm6yUb+qIVedmOvnBJXrqlL16Tl+/pGxjIWzjom1jI23joG5noW7nIm9no2/noGxrpWzrJm1rp23rpG5vpW7vpm9vp2/vpBxzoRzzoh1xCMOZDP+hEP+oVgmE3+nG/EAw8hmDkMwRDryEY+w3B4HMYRr9DMPwexPj/X+Yt8H/u7e31AAAAAElFTkSuQmCC";
            string base64_Success = "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";

            imgCheck_Error.Source = this.getImageSource(base64_Error);
            imgCheck_Success.Source = this.getImageSource(base64_Success);
        }

        private ImageSource getImageSource(string base64Str)
        {
            byte[] buffer = Convert.FromBase64String(base64Str);
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(buffer);

            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.DecodePixelWidth = 120;
            bitmapImage.DecodePixelHeight = 120;
            bitmapImage.StreamSource = memoryStream;
            bitmapImage.EndInit();
            bitmapImage.Freeze();

            return bitmapImage;
        }

        private void initEvent()
        {
            this.Loaded += UcSelectFile_Loaded;
            this.btnSelect.Click += BtnSelect_Click;
        }

        private void UcSelectFile_Loaded(object sender, RoutedEventArgs e)
        {
            updateUI_imgCheck();
        }

        private void updateUI_imgCheck()
        {
            this.IsSuccess = false;
            this.imgCheck_Success.Visibility = Visibility.Hidden;

            // 未选择文件
            if (this.FileNames == null || this.FileNames.Length <= 0)
            {
                imgCheck_Error.ToolTip = $"{this.Title}";
                return;
            }

            // 验证文件存在
            if (this.FileNames.Any(i => System.IO.File.Exists(i) == false))
            {
                StringBuilder sb = new StringBuilder();
                foreach (string item in this.FileNames.Where(i => System.IO.File.Exists(i) == false))
                {
                    sb.Append(item).AppendLine(";");
                }

                imgCheck_Error.ToolTip = $"以下文件不存在。\r\n{sb.ToString()}";
                return;
            }

            // 若配置了正则表达式的校验, 进行正则表达式的校验
            if (string.IsNullOrWhiteSpace(this.RegularExpressionFilter) == false)
            {
                StringBuilder sb = new StringBuilder();
                foreach (string item in this.FileNames)
                {
                    bool match = System.Text.RegularExpressions.Regex.IsMatch
                    (
                        input: this.txtFilePaths.Text,
                        pattern: this.RegularExpressionFilter,
                        options: System.Text.RegularExpressions.RegexOptions.IgnoreCase
                    );

                    if (match == false)
                    {
                        sb.Append(item).AppendLine(";");
                    }
                }

                if (string.IsNullOrWhiteSpace(sb.ToString()) == false)
                {
                    imgCheck_Error.ToolTip = $"以下文件不符合要求。(正则表达式:[{this.RegularExpressionFilter}])\r\n{sb.ToString()}";
                    return;
                }
            }

            // 通过所有验证
            imgCheck_Error.ToolTip = null;
            this.imgCheck_Success.Visibility = Visibility.Visible;
            this.IsSuccess = true;
            this.OnSuccess();
        }

        private void BtnSelect_Click(object sender, RoutedEventArgs e)
        {
            btnSelect_ActualMethod();
        }

        private void btnSelect_ActualMethod()
        {
            Microsoft.Win32.OpenFileDialog openFile = new Microsoft.Win32.OpenFileDialog();
            openFile.Multiselect = this.Multiselect;
            openFile.Filter = this.Filter;

            if (openFile.ShowDialog() == true)
            {
                this.FileName = openFile.FileName;
                this.FileNames = openFile.FileNames;
                this.txtFilePaths.Text = getFilePathsText(openFile.FileNames);
            }

            updateUI_imgCheck();
        }

        private string getFilePathsText(string[] fileNames)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < fileNames.Length; i++)
            {
                sb.Append(fileNames[i]);
                if (fileNames.Length > i + 1)
                {
                    sb.Append(";");
                    sb.AppendLine();
                }
            }

            return sb.ToString();
        }


        public void SetFileName(string fileaName)
        {
            string[] fileNames = new string[1] { fileaName };
            this.SetFileNames(fileNames);
        }

        public void SetFileNames(string[] fileNames)
        {
            if (fileNames.Length > 0)
            {
                this.FileName = fileNames[0];
            }
            this.FileNames = fileNames;
            this.txtFilePaths.Text = this.getFilePathsText(fileNames);
            this.updateUI_imgCheck();
        }

        public EventHandler<EventArgs> SuccessEventHandler;

        private void OnSuccess()
        {
            if (SuccessEventHandler != null)
            {
                this.SuccessEventHandler.Invoke(this, new EventArgs());
            }
        }
    }
}
