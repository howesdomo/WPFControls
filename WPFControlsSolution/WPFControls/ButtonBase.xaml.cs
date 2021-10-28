using System;
using System.Collections.Generic;
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
    /// <para>
    /// V 1.0.1 - 2021-07-14 17:43:20
    /// 优化 对于 ImageUri 的赋值 使用 Uri2ImageSourceConverter 进行转换
    /// </para>
    /// <para>
    /// V 1.0.0 - 2021-07-14 17:31:23
    /// 首次创建
    /// </para>
    /// </summary>
    public partial class ButtonBase : Button
    {
        public ButtonBase()
        {
            InitializeComponent();
        }

        #region [DP] Title

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register
        (
            name: "Title",
            propertyType: typeof(string),
            ownerType: typeof(ButtonBase),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
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
    }
}
