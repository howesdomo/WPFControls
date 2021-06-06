using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Client.Components
{
    public static class DataGridExtension
    {
        #region D:\Enpot_Project\Enpot\ENPOT.Base\SC\ENPOT.Controls\Common\DataGridExtension.cs

        public static DataGridRow GetGridRow(this DataGrid dataGrid, int rowIndex)
        {
            dataGrid.ScrollIntoView(dataGrid.Items[rowIndex]);
            return (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex);
        }

        public static DataGridCell GetGridCell(this DataGrid dataGrid, int rowIndex, int colIndex)
        {

            DataGridRow rowContainer = GetGridRow(dataGrid, rowIndex);
            if (rowContainer != null)
            {
                dataGrid.ScrollIntoView(rowContainer, dataGrid.Columns[colIndex]);
                var presenter = WPFControlsUtils.FindChildOfType<System.Windows.Controls.Primitives.DataGridCellsPresenter>(rowContainer);

                if (presenter != null)
                {
                    dataGrid.ScrollIntoView(rowContainer, dataGrid.Columns[colIndex]);
                    return (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(colIndex);
                }
            }

            return null;
        }

        //public static T GetVisualChild<T>(System.Windows.Media.Visual parent)
        //    where T : System.Windows.Media.Visual
        //{
        //    T child = default(T);
        //    int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
        //    for (int i = 0; i < numVisuals; i++)
        //    {
        //        Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
        //        child = v as T;
        //        if (child == null)
        //        {
        //            child = GetVisualChild<T>(v);
        //        }

        //        if (child != null)
        //        {
        //            break;
        //        }
        //    }
        //    return child;
        //}

        #endregion


        #region D:\Enpot_Project\Enpot\ENPOT.Base\SC\ENPOT.Controls\System.Winodws.Controls.Extension\DataGridExtension.cs

        /// <summary>
        /// 右键菜单
        /// </summary>
        /// <param name="datagrid"></param>
        public static void SetRightClickMenu(this System.Windows.Controls.DataGrid datagrid)
        {
            ContextMenu ctMenu = new ContextMenu();
            var matchIcon = new System.Windows.Media.Imaging.BitmapImage(new Uri("    ", UriKind.Relative)); // 自定义图标

            List<MenuItem> l = new List<MenuItem>();
            MenuItem pasteMenuItem = new MenuItem();
            pasteMenuItem.Icon = matchIcon;
            pasteMenuItem.Header = "区域粘贴";
            pasteMenuItem.InputGestureText = "";
            pasteMenuItem.Click += (o, e) =>
            {
                try
                {
                    datagrid.AreaPaste();
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
            };

            l.Add(pasteMenuItem);

            // 分割线
            System.Windows.Controls.MenuItem lineMenuItem = new System.Windows.Controls.MenuItem();
            lineMenuItem.IsEnabled = false;
            lineMenuItem.Height = 3;
            System.Windows.Media.Color color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("Black");
            System.Windows.Media.SolidColorBrush backgroundColor = new System.Windows.Media.SolidColorBrush(color);
            lineMenuItem.Background = backgroundColor;
            l.Add(lineMenuItem);

            MenuItem setTagMenuItem = new MenuItem();
            setTagMenuItem.Icon = matchIcon;
            setTagMenuItem.Header = "复制单元格";
            setTagMenuItem.InputGestureText = "";
            setTagMenuItem.Click += (o, e) =>
            {
                try
                {
                    datagrid.SetTag_SourceCell();
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
            };

            l.Add(setTagMenuItem);

            MenuItem copyValueMenuItem = new MenuItem();
            copyValueMenuItem.Icon = matchIcon;
            copyValueMenuItem.Header = "粘贴单元格";
            copyValueMenuItem.InputGestureText = "";
            copyValueMenuItem.Click += (o, e) =>
            {
                try
                {
                    datagrid.CopyValueFromSourceCell();
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
            };

            l.Add(copyValueMenuItem);


            ctMenu.ItemsSource = l;
            datagrid.ContextMenu = ctMenu;
        }

        /// <summary>
        /// 区域粘贴
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="datagrid"></param>
        public static void AreaPaste(this System.Windows.Controls.DataGrid datagrid)
        {
            if (datagrid == null)
            {
                throw new Exception("DataGrid为空");
            }

            if (datagrid.SelectedCells == null || datagrid.SelectedCells.Count <= 0)
            {
                throw new Exception("未选中任何单元格。");

            }

            if (datagrid.CurrentCell == null)
            {
                throw new Exception("未选中任何单元格。");
            }

            int datagridSelectedCellsCount = datagrid.SelectedCells.Count;

            List<string> clipBoardContentList = new List<string>();

            #region 粘贴区域

            string pasteText = System.Windows.Clipboard.GetText();

            if (string.IsNullOrEmpty(pasteText) == true)
            {
                throw new Exception("剪贴板无信息。");
            }

            if (pasteText.Contains('\r') == true || pasteText.Contains('\n') == true)
            {
                List<string> splited = pasteText.Split('\r', '\n')
                .Select(i => i.Trim())
                .Where(i => string.IsNullOrEmpty(i) == false)
                .ToList();
                ;

                clipBoardContentList.Clear();
                clipBoardContentList.AddRange(splited);
            }
            else
            {
                clipBoardContentList.Add(pasteText);
            }

            #endregion 粘贴区域

            if (datagridSelectedCellsCount != clipBoardContentList.Count)
            {
                var errorMsg = string.Format("无法粘贴。选中{0}格, 粘贴{1}格。", datagridSelectedCellsCount, clipBoardContentList.Count);
                throw new Exception(errorMsg);
            }

            // Cell ==> Prop
            for (int index = 0; index < clipBoardContentList.Count; index++)
            {
                DataGridCellInfo cellInfo = datagrid.SelectedCells[index];
                string matchField = GetBindingPath(cellInfo.Column);
                string value = clipBoardContentList[index];
                object model = cellInfo.Item;
                SetValue_DiGui(model, matchField, value, index);
            }
        }

        /// <summary>
        /// 设置目标单元格
        /// </summary>
        /// <param name="datagrid"></param>
        public static void SetTag_SourceCell(this System.Windows.Controls.DataGrid datagrid)
        {
            if (datagrid.SelectedCells == null || datagrid.SelectedCells.Count <= 0)
            {
                throw new Exception("请先选择需要复制的单元格。");
            }

            if (datagrid.Tag == null)
            {
                datagrid.Tag = new DataGridTag();
            }

            DataGridTag matchTag = datagrid.Tag as DataGridTag;
            matchTag.SourceCell = datagrid.SelectedCells[0];
        }

        /// <summary>
        /// 根据目标单元格的值, 填充到选中的单元格
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="datagrid"></param>
        public static void CopyValueFromSourceCell(this System.Windows.Controls.DataGrid datagrid)
        {
            if (datagrid == null)
            {
                throw new Exception("DataGrid为空");
            }

            if (datagrid.SelectedCells == null || datagrid.SelectedCells.Count <= 0)
            {
                throw new Exception("未选中任何单元格。");

            }

            if (datagrid.CurrentCell == null)
            {
                throw new Exception("未选中任何单元格。");
            }

            object valueToFillAll = null;

            if (datagrid.Tag == null)
            {
                throw new Exception("请先设置复制目标单元格。");
            }

            var matchTag = datagrid.Tag as DataGridTag;

            if (matchTag.SourceCell == null)
            {
                throw new Exception("请先设置目标单元格。");
            }
            else
            {
                string matchField = GetBindingPath(matchTag.SourceCell.Column);
                var model = matchTag.SourceCell.Item;
                valueToFillAll = GetValue_DiGui(model, matchField);
            }

            if (valueToFillAll == null)
            {
                throw new Exception("请先设置目标单元格。");
            }

            for (int index = 0; index < datagrid.SelectedCells.Count; index++)
            {
                DataGridCellInfo cellInfo = datagrid.SelectedCells[index];
                string matchField = GetBindingPath(cellInfo.Column);
                object model = cellInfo.Item;
                SetValue_DiGui(model, matchField, valueToFillAll, index);
            }

        }


        /// <summary>
        /// 获取DataGridColumn BindingPath属性
        /// </summary>
        /// <param name="datagridcolumn"></param>
        /// <returns>BindingPath属性</returns>
        private static string GetBindingPath(DataGridColumn datagridcolumn)
        {
            string r = string.Empty;
            try
            {
                System.Windows.Controls.DataGridBoundColumn column = (System.Windows.Controls.DataGridBoundColumn)datagridcolumn;
                System.Windows.Data.Binding matchBinding = (System.Windows.Data.Binding)column.Binding;
                r = matchBinding.Path.Path;
            }
            catch (Exception ex)
            {
                // TODO 如何获取 System.Windows.Controls.DataGridTemplateColumn 的 Binding 属性
                if (ex.Message.Contains("System.Windows.Controls.DataGridTemplateColumn") &&
                    ex.Message.Contains("System.Windows.Controls.DataGridBoundColumn"))
                {
                    r = datagridcolumn.SortMemberPath; // 暂时先取 SortMemberPath
                }
                else
                {
                    throw ex;
                }
            }
            return r;
        }

        /// <summary>
        /// 递归获取Value ( 获取绑定设置 Model.Carton 这种形式的值)
        /// </summary>
        /// <param name="model"></param>
        /// <param name="matchField"></param>
        private static object GetValue_DiGui(object model, string matchField)
        {
            Type type = model.GetType();

            int indexOfDot = matchField.IndexOf('.');
            if (indexOfDot > 0)
            {
                // ClassA.ClassB.ClassC
                string matchField_Part1 = matchField.Substring(0, indexOfDot); // ClassA
                string matchField_Part2 = matchField.Substring(indexOfDot + 1); // ClassB.ClassC

                System.Reflection.PropertyInfo propInfo = type.GetProperty(matchField_Part1);

                object subModel = propInfo.GetValue(model, null);

                // 进入下层递归
                return GetValue_DiGui(subModel, matchField_Part2);
            }
            else
            {
                System.Reflection.PropertyInfo propInfo = type.GetProperty(matchField);
                if (propInfo == null)
                {
                    throw new Exception("System.Reflection.PropertyInfo is null");
                }

                return propInfo.GetValue(model, null);
            }
        }

        /// <summary>
        /// 递归设置Value ( 设置绑定设置 Model.Carton 这种形式的值)
        /// </summary>
        /// <param name="model"></param>
        /// <param name="matchField"></param>
        /// <param name="value"></param>
        /// <param name="valueIndex"></param>
        private static void SetValue_DiGui(object model, string matchField, object value, int valueIndex)
        {
            Type type = model.GetType();

            int indexOfDot = matchField.IndexOf('.');
            if (indexOfDot > 0)
            {
                // ClassA.ClassB.ClassC
                string matchField_Part1 = matchField.Substring(0, indexOfDot); // ClassA
                string matchField_Part2 = matchField.Substring(indexOfDot + 1); // ClassB.ClassC

                System.Reflection.PropertyInfo propInfo = type.GetProperty(matchField_Part1);

                object subModel = propInfo.GetValue(model, null);

                // 进入下层递归
                SetValue_DiGui(subModel, matchField_Part2, value, valueIndex);
            }
            else
            {
                System.Reflection.PropertyInfo propInfo = type.GetProperty(matchField);
                if (propInfo == null)
                {
                    throw new Exception("System.Reflection.PropertyInfo is null");
                }

                string argExMessage = string.Empty;
                int convertToXIndex = -1;

                try
                {
                    propInfo.SetValue
                    (
                        obj: model,
                        value: value,
                        index: null
                    );
                }
                catch (System.ArgumentException argEx)
                {
                    // 报错信息 "类型“System.String”的对象无法转换为类型“System.Decimal”。"
                    // 报错信息 "类型“System.String”的对象无法转换为类型“System.Int32”。" 等等
                    convertToXIndex = argEx.Message.LastIndexOf("System.");

                    if (convertToXIndex < 0)
                    {
                        // 不是以上无法转换的报错，抛出错误
                        throw argEx;
                    }
                    else
                    {
                        argExMessage = argEx.Message;
                    }
                }

                #region 需要转换类型的赋值处理

                if (string.IsNullOrEmpty(argExMessage) == false)
                {
                    // 类型X 转换到 Y类型
                    string convertFromX = string.Empty;
                    #region 被转换的类型

                    string templateSystemDot = "System.";
                    int convertFromXIndex = argExMessage.IndexOf(templateSystemDot);
                    string temp = argExMessage.Substring(convertFromXIndex + templateSystemDot.Length);
                    convertFromX = "System.";
                    foreach (char c in temp)
                    {
                        if (char.IsLetterOrDigit(c))
                        {
                            convertFromX += c.ToString();
                        }
                        else
                        {
                            break;
                        }
                    }

                    #endregion

                    string convertToY = string.Empty;
                    #region 转换到Y类型
                    convertToY = argExMessage.Substring(convertToXIndex);
                    convertToY = convertToY
                        .Replace(',', ' ')
                        .Replace('"', ' ')
                        .Replace('”', ' ')
                        .Replace('。', ' ')
                        ;

                    convertToY = convertToY.Trim();
                    #endregion

                    try
                    {
                        SetPropValue(argExMessage, convertToY, model, propInfo, value);
                    }
                    catch (Exception toAddInfoEx)
                    {
                        throw new Exception
                        (
                            //string.Format("{0}\r\n剪贴板第 {1} 格\r\n输入的字符串：{2}\r\n转换的格式：{3}",
                            //    toAddInfoEx.Message,
                            //    (clipBoardIndex + 1),
                            //    clipBoardValue,
                            //    convertToY)

                            string.Format("{0}\r\n被转换的类型：{1}\r\n被转换的类型的值：{2}\r\n转换到类型：{3}",
                            toAddInfoEx.Message,
                            convertFromX,
                            value,
                            convertToY
                            )
                        );
                    }
                }

                #endregion 需要转换类型的赋值处理

            }
        }

        /// <summary>
        /// 对属性赋值 ( 慢慢完善各种类型 )
        /// </summary>
        /// <param name="argExMessage">直接转换时报错信息</param>
        /// <param name="convertToY">转换到 X 类型</param>
        /// <param name="model">赋值的对象</param>
        /// <param name="propInfo">赋值到哪个属性</param>
        /// <param name="value">赋值的字符串内容</param>
        private static void SetPropValue
        (
            string argExMessage,
            string convertToY,
            object model,
            System.Reflection.PropertyInfo propInfo,
            object value
        )
        {
            switch (convertToY.ToUpper())
            {
                case "SYSTEM.STRING":
                    {
                        string valueAfterConvert = value.ToString();
                        propInfo.SetValue(model, valueAfterConvert, null);
                        break;
                    }

                case "SYSTEM.INT32":
                    {
                        int valueAfterConvert = Convert.ToInt32(value);
                        propInfo.SetValue(model, valueAfterConvert, null);
                        break;
                    }

                case "SYSTEM.DECIMAL":
                    {
                        decimal valueAfterConvert = Convert.ToDecimal(value);
                        propInfo.SetValue(model, valueAfterConvert, null);
                    }
                    break;

                case "SYSTEM.BOOLEAN":
                    {
                        bool valueAfterConvert = Convert.ToBoolean(value);
                        propInfo.SetValue(model, valueAfterConvert, null);
                    }
                    break;

                default:
                    throw new Exception(argExMessage);
            }
        }

        #endregion
    }

    public class DataGridTag
    {
        /// <summary>
        /// 目标单元格
        /// </summary>
        public DataGridCellInfo SourceCell { get; set; }
    }
}

