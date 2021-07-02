using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Components.PrinterPanel
{
    /// <summary>
    /// V 1.0.1 - 2021-07-02 17:21:35
    /// 新增获取系统打印机名称
    /// </summary>
    public class PrinterUtils
    {
        public static string sUpdateName
        {
            get
            {
                return "刷新...";
            }
        }

        /// <summary>
        /// 从注册表，获取打印机的列表, 并且获取打印机的纸张列表
        /// </summary>
        /// <typeparam name="MyPrinter"></typeparam>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        public static List<Printer> GetPrinterList(bool isContainUpdateListItem = false)
        {
            List<Printer> r = new List<Printer>();
            Microsoft.Win32.RegistryKey devices = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows NT\CurrentVersion\Devices");

            foreach (string name in devices.GetValueNames())
            {
                var value = (String)devices.GetValue(name);
                var port = System.Text.RegularExpressions.Regex.Match(value, @"(Ne\d+:)", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Value;

                r.Add(new Printer()
                {
                    DisplayName = name,
                    OriginValue = name,
                    ExcelValue = $"{name} 在 {port}"
                });
            }

            // 根据打印机名称, 获取其纸张列表
            foreach (Printer item in r)
            {
                item.PaperSizeList = GetPageSizeName(item);
            }

            if (isContainUpdateListItem == true) // UcPrinterPanel 中, 刷新列表
            {
                // 增加刷新打印机列表项
                r.Add(new Printer()
                {
                    DisplayName = sUpdateName,
                    PaperSizeList = new List<PaperSize>()
                });
            }

            return r;
        }

        /// <summary>
        /// 根据打印机名称, 获取其纸张列表
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static List<PaperSize> GetPageSizeName(Printer args)
        {
            List<PaperSize> r = new List<PaperSize>();

            System.Drawing.Printing.PrinterSettings printer = new System.Drawing.Printing.PrinterSettings();
            printer.PrinterName = args.DisplayName;
            foreach (System.Drawing.Printing.PaperSize pageSize in printer.PaperSizes)
            {
                PaperSize toAdd = new PaperSize();
                toAdd.DisplayName = pageSize.PaperName;
                toAdd.Value = pageSize.RawKind;

                r.Add(toAdd);
            }

            return r;
        }

        public static List<Printer> PrinterOrderBy(List<Printer> temp, List<string> priorityPrinterList, List<string> priorityPaperList)
        {
            List<Printer> final = new List<Printer>();

            foreach (string item in priorityPrinterList)
            {
                var match = temp.FirstOrDefault(i => i.DisplayName.Equals(item, StringComparison.CurrentCultureIgnoreCase));
                if (match != null)
                {
                    match.PaperSizeList = PaperSizeOrderBy(match.PaperSizeList, priorityPaperList);
                    final.Add(match);
                }
            }

            foreach (var item in temp)
            {
                var match = final.FirstOrDefault(i => i == item);
                if (match == null)
                {
                    item.PaperSizeList = PaperSizeOrderBy(item.PaperSizeList, priorityPaperList);
                    final.Add(item);
                }
            }

            return final;
        }

        public static List<PaperSize> PaperSizeOrderBy(List<PaperSize> temp, List<string> priorityPaperList)
        {
            List<PaperSize> final = new List<PaperSize>();

            foreach (string item in priorityPaperList)
            {
                var match = temp.FirstOrDefault(i => i.DisplayName.Equals(item, StringComparison.CurrentCultureIgnoreCase));
                if (match != null)
                {
                    final.Add(match);
                }
            }

            foreach (var item in temp)
            {
                var match = final.FirstOrDefault(i => i == item);
                if (match == null)
                {
                    final.Add(item);
                }
            }

            return final;
        }

        /// <summary>
        /// 获取系统默认打印机名称
        /// </summary>
        /// <returns></returns>
        public static string GetDefaultPrinterName()
        {
            return new System.Drawing.Printing.PrintDocument().PrinterSettings.PrinterName;
        }
    }
}
