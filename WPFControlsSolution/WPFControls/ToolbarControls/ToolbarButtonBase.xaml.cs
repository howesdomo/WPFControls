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

namespace Client.Components.ToolbarControls
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
    public partial class ToolbarButtonBase : Button
    {
        public ToolbarButtonBase()
        {
            InitializeComponent();
        }

        #region [DP] ImageWidth

        public static readonly DependencyProperty ImageWidthProperty = DependencyProperty.Register
        (
            name: "ImageWidth",
            propertyType: typeof(double),
            ownerType: typeof(ToolbarButtonBase),
            validateValueCallback: null, // new ValidateValueCallback((toValidate) => { return true; }),
            typeMetadata: new PropertyMetadata
            (
                defaultValue: 20d,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public double ImageWidth
        {
            get { return (double)GetValue(ImageWidthProperty); }
            set { SetValue(ImageWidthProperty, value); }
        }

        #endregion

        #region [DP] ImageHeight

        public static readonly DependencyProperty ImageHeightProperty = DependencyProperty.Register
        (
            name: "ImageHeight",
            propertyType: typeof(double),
            ownerType: typeof(ToolbarButtonBase),
            validateValueCallback: null, // new ValidateValueCallback((toValidate) => { return true; }),
            typeMetadata: new PropertyMetadata
            (
                defaultValue: 20d,
                propertyChangedCallback: null,
                coerceValueCallback: null
            )
        );

        public double ImageHeight
        {
            get { return (double)GetValue(ImageHeightProperty); }
            set { SetValue(ImageHeightProperty, value); }
        }

        #endregion

        #region [DP] ImageUri

        public static readonly DependencyProperty ImageUriProperty = DependencyProperty.Register
        (
            name: "ImageUri",
            propertyType: typeof(Uri),
            ownerType: typeof(ToolbarButtonBase),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: null, // onImageUri_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public Uri ImageUri
        {
            get { return (Uri)GetValue(ImageUriProperty); }
            set { SetValue(ImageUriProperty, value); }
        }

        public static void onImageUri_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //// 优化 对于 ImageUri 的赋值 使用 Uri2ImageSourceConverter 进行转换
            ///
            //if (d is ToolbarButtonBase target)
            //{
            //    if (string.IsNullOrWhiteSpace(e.NewValue.ToString()) == false)
            //    {
            //        BitmapImage image = new BitmapImage();
            //        image.BeginInit();
            //        image.UriSource = e.NewValue as Uri;
            //        image.EndInit();
            //        target.image.Source = image;
            //    }
            //}
        }

        #endregion

        #region [DP] Title

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register
        (
            name: "Title",
            propertyType: typeof(string),
            ownerType: typeof(ToolbarButtonBase),
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

    #region 增删改查

    public class AddButton : ToolbarButtonBase
    {
        public AddButton()
        {
            base.Title = "添加";
            base.ImageUri = new Uri("pack://application:,,,/WPFControls;component/ToolbarControls/Resources/add.png");
        }
    }

    public class EditButton : ToolbarButtonBase
    {
        public EditButton()
        {
            base.Title = "编辑";
            base.ImageUri = new Uri("pack://application:,,,/WPFControls;component/ToolbarControls/Resources/edit.png");
        }
    }

    public class DeleteButton : ToolbarButtonBase
    {
        public DeleteButton()
        {
            base.Title = "删除";
            base.ImageUri = new Uri("pack://application:,,,/WPFControls;component/ToolbarControls/Resources/delete.png");
        }
    }

    public class SaveButton : ToolbarButtonBase
    {
        public SaveButton()
        {
            base.Title = "保存";
            base.ImageUri = new Uri("pack://application:,,,/WPFControls;component/ToolbarControls/Resources/save.png");
        }
    }

    #endregion

    #region 全选反选

    public class CheckAllButton : ToolbarButtonBase
    {
        public CheckAllButton()
        {
            base.Title = "全选";
            base.ImageUri = new Uri("pack://application:,,,/WPFControls;component/ToolbarControls/Resources/checkAll.png");
        }
    }

    public class ReverseCheckButton : ToolbarButtonBase
    {
        public ReverseCheckButton()
        {
            base.Title = "反选";
            base.ImageUri = new Uri("pack://application:,,,/WPFControls;component/ToolbarControls/Resources/reverseCheck.png");
        }
    }

    #endregion

    #region 导入导出

    public class FileImportButton : ToolbarButtonBase
    {
        public FileImportButton()
        {
            base.Title = "导入";
            base.ImageUri = new Uri("pack://application:,,,/WPFControls;component/ToolbarControls/Resources/fileImport.png");
        }
    }

    public class FileExportButton : ToolbarButtonBase
    {
        public FileExportButton()
        {
            base.Title = "导出";
            // TODO 找寻导出图标（非Excel文件）
            base.ImageUri = new Uri("pack://application:,,,/WPFControls;component/ToolbarControls/Resources/excelExport.png");
        }
    }

    public class FileReloadButton : ToolbarButtonBase
    {
        public FileReloadButton()
        {
            base.Title = "重新加载";
            base.ImageUri = new Uri("pack://application:,,,/WPFControls;component/ToolbarControls/Resources/reset.png");
        }
    }

    public class ExcelExportButton : ToolbarButtonBase
    {
        public ExcelExportButton()
        {
            base.Title = "导出";
            base.ImageUri = new Uri("pack://application:,,,/WPFControls;component/ToolbarControls/Resources/excelExport.png");
        }
    }

    public class PrintButton : ToolbarButtonBase
    {
        public PrintButton()
        {
            base.Title = "打印";
            base.ImageUri = new Uri("pack://application:,,,/WPFControls;component/ToolbarControls/Resources/print.png");
        }
    }

    #endregion

}
