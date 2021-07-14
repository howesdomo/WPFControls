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

namespace Client.Components.ToolbarContols
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WPFControls.ToolbarControls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WPFControls.ToolbarControls;assembly=WPFControls.ToolbarControls"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:ToolbarButtonBase/>
    ///
    /// </summary>
    public class ToolbarButtonBase : Button
    {
        static ToolbarButtonBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ToolbarButtonBase), new FrameworkPropertyMetadata(typeof(ToolbarButtonBase)));
        }

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

        //public static void onImageUri_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    if (d is ToolbarButtonBase target)
        //    {
        //        if (string.IsNullOrWhiteSpace(e.NewValue.ToString()) == false)
        //        {
        //            BitmapImage image = new BitmapImage();
        //            image.BeginInit();
        //            image.UriSource = e.NewValue as Uri;
        //            image.EndInit();
        //            target.image.Source = image;
        //        }
        //    }
        //}

        #endregion
    }


    public class AddButton : ToolbarButtonBase
    {
        public AddButton()
        {
            base.Title = "添加";
            base.ImageUri = new Uri("pack://application:,,,/WPFControls;component/ToolbarControls/Resources/add.png");
        }
    }


    //#region 增删改查

    //public class AddButton : ToolbarButtonBase
    //{
    //    public AddButton()
    //    {
    //        base.Title = "添加";
    //        base.ImageUri = new Uri("pack://application:,,,/WPFControls;component/ToolbarControls/Resources/add.png");
    //    }
    //}

    //public class EditButton : ToolbarButtonBase
    //{
    //    public EditButton()
    //    {
    //        base.Title = "编辑";
    //        base.ImageUri = new Uri("pack://application:,,,/WPFControls;component/ToolbarControls/Resources/edit.png");
    //    }
    //}

    //public class DeleteButton : ToolbarButtonBase
    //{
    //    public DeleteButton()
    //    {
    //        base.Title = "删除";
    //        base.ImageUri = new Uri("pack://application:,,,/WPFControls;component/ToolbarControls/Resources/delete.png");
    //    }
    //}

    //public class SaveButton : ToolbarButtonBase
    //{
    //    public SaveButton()
    //    {
    //        base.Title = "保存";
    //        base.ImageUri = new Uri("pack://application:,,,/WPFControls;component/ToolbarControls/Resources/save.png");
    //    }
    //}

    //#endregion

    //#region 全选反选

    //public class CheckAllButton : ToolbarButtonBase
    //{
    //    public CheckAllButton()
    //    {
    //        base.Title = "全选";
    //        base.ImageUri = new Uri("pack://application:,,,/WPFControls;component/ToolbarControls/Resources/checkAll.png");
    //    }
    //}

    //public class ReverseCheckButton : ToolbarButtonBase
    //{
    //    public ReverseCheckButton()
    //    {
    //        base.Title = "反选";
    //        base.ImageUri = new Uri("pack://application:,,,/WPFControls;component/ToolbarControls/Resources/reverseCheck.png");
    //    }
    //}

    //#endregion

    //#region 导入导出

    //public class FileImportButton : ToolbarButtonBase
    //{
    //    public FileImportButton()
    //    {
    //        base.Title = "导入";
    //        base.ImageUri = new Uri("pack://application:,,,/WPFControls;component/ToolbarControls/Resources/fileImport.png");
    //    }
    //}

    //public class FileExportButton : ToolbarButtonBase
    //{
    //    public FileExportButton()
    //    {
    //        base.Title = "导出";
    //        // TODO 找寻导出图标（非Excel文件）
    //        base.ImageUri = new Uri("pack://application:,,,/WPFControls;component/ToolbarControls/Resources/excelExport.png");
    //    }
    //}

    //public class FileReloadButton : ToolbarButtonBase
    //{
    //    public FileReloadButton()
    //    {
    //        base.Title = "重新加载";
    //        base.ImageUri = new Uri("pack://application:,,,/WPFControls;component/ToolbarControls/Resources/reset.png");
    //    }
    //}

    //public class ExcelExportButton : ToolbarButtonBase
    //{
    //    public ExcelExportButton()
    //    {
    //        base.Title = "导出";
    //        base.ImageUri = new Uri("pack://application:,,,/WPFControls;component/ToolbarControls/Resources/excelExport.png");
    //    }
    //}

    //public class PrintButton : ToolbarButtonBase
    //{
    //    public PrintButton()
    //    {
    //        base.Title = "打印";
    //        base.ImageUri = new Uri("pack://application:,,,/WPFControls;component/ToolbarControls/Resources/print.png");
    //    }
    //}

    //#endregion

}