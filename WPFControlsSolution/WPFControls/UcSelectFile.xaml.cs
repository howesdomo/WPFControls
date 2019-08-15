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



        #endregion

        public UcSelectFile()
        {
            InitializeComponent();
            initEvent();
        }

        private void initEvent()
        {
            this.btnSelect.Click += BtnSelect_Click;
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

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < openFile.FileNames.Length; i++)
                {
                    sb.Append(openFile.FileNames[i]);
                    if (openFile.FileNames.Length > i + 1)
                    {
                        sb.Append(";");
                        sb.AppendLine();
                    }
                }
                this.txtFilePaths.Text = sb.ToString();
            }
        }
    }
}
