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

namespace WPFControls.MessageBoxExtraContent
{
    /// <summary>
    /// Interaction logic for CcTextBox.xaml
    /// </summary>
    public partial class CcTextBox : ContentControl
    {
        System.ComponentModel.BackgroundWorker mBgWorker { get; set; }

        public CcTextBox()
        {
            InitializeComponent();

            this.Loaded += (s,e)=> 
            {
                // CcSingleTextBox 需要对预设值进行 Foucs 与 SelectAll
                // 在 Loaded 事件，TextBox.Text 仍然是空值，故无法对全选 TextBox

                // TODO 权宜之计 采用 BackgroundWorker 等待 100 毫秒
                if (mBgWorker != null && mBgWorker.IsBusy == true)
                {
                    return;
                }

                mBgWorker = new System.ComponentModel.BackgroundWorker();
                mBgWorker.DoWork += (bgSender, bgArgs) =>
                {
                    System.Threading.Thread.Sleep(100);
                };

                mBgWorker.RunWorkerCompleted += (bgSender, bgResult) =>
                {
                    txt.Focus();
                    txt.SelectAll();
                };

                mBgWorker.RunWorkerAsync(new object[] { });
            };
        }

        #region [DP] Title

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register
            (
                name: "Title",
                propertyType: typeof(string),
                ownerType: typeof(CcTextBox),
                validateValueCallback: null,
                typeMetadata: new PropertyMetadata
                (
                    defaultValue: string.Empty,
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

        #region [DP] Placeholder

        public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register
        (
            name: "Placeholder",
            propertyType: typeof(string),
            ownerType: typeof(CcTextBox),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: string.Empty,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        #endregion

        #region [DP] PlaceholderColor

        public static readonly DependencyProperty PlaceholderColorProperty = DependencyProperty.Register
        (
            name: "PlaceholderColor",
            propertyType: typeof(System.Windows.Media.Brush),
            ownerType: typeof(CcTextBox),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: System.Windows.Media.Brushes.Gray,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public System.Windows.Media.Brush PlaceholderColor
        {
            get { return (System.Windows.Media.Brush)GetValue(PlaceholderColorProperty); }
            set { SetValue(PlaceholderColorProperty, value); }
        }

        #endregion

        #region [DP] PlaceholderFontSize


        public static readonly DependencyProperty PlaceholderFontSizeProperty = DependencyProperty.Register
        (
            name: "PlaceholderFontSize",
            propertyType: typeof(double),
            ownerType: typeof(CcTextBox),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: 12d,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public double PlaceholderFontSize
        {
            get { return (double)GetValue(PlaceholderFontSizeProperty); }
            set { SetValue(PlaceholderFontSizeProperty, value); }
        }


        #endregion

        #region [DP] TextBoxHeight

        public static readonly DependencyProperty TextBoxHeightProperty = DependencyProperty.Register
        (
            name: "TextBoxHeight",
            propertyType: typeof(double),
            ownerType: typeof(CcTextBox),
            validateValueCallback: null, // new ValidateValueCallback((toValidate) => { return true; }),
            typeMetadata: new PropertyMetadata
            (
                defaultValue: 80d,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public double TextBoxHeight
        {
            get { return (double)GetValue(TextBoxHeightProperty); }
            set { SetValue(TextBoxHeightProperty, value); }
        }

        #endregion

        #region [DP] TextBoxBackground

        public static readonly DependencyProperty TextBoxBackgroundProperty = DependencyProperty.Register
        (
            name: "TextBoxBackground",
            propertyType: typeof(System.Windows.Media.Brush),
            ownerType: typeof(CcTextBox),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(245, 245, 245)),
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public System.Windows.Media.Brush TextBoxBackground
        {
            get { return (System.Windows.Media.Brush)GetValue(TextBoxBackgroundProperty); }
            set { SetValue(TextBoxBackgroundProperty, value); }
        }

        #endregion

        #region [DP] IsReadOnly

        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register
        (
            name: "IsReadOnly",
            propertyType: typeof(bool),
            ownerType: typeof(CcTextBox),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: false,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        #endregion

        #region [DP] TextBoxIsEnabled

        public static readonly DependencyProperty TextBoxIsEnabledProperty = DependencyProperty.Register
        (
            name: "TextBoxIsEnabled",
            propertyType: typeof(bool),
            ownerType: typeof(CcTextBox),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: true,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public bool TextBoxIsEnabled
        {
            get { return (bool)GetValue(TextBoxIsEnabledProperty); }
            set { SetValue(TextBoxIsEnabledProperty, value); }
        }

        #endregion
    }
}
