﻿using Client.Components;
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

namespace Client
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region 基础控件

        private void btnTestTextboxAdv_Click(object sender, RoutedEventArgs e)
        {
            new Test.FrmTest_TextBox().ShowDialog();
        }

        private void btnTest_DatePicker_Click(object sender, RoutedEventArgs e)
        {
            new Test.FrmTest_DatePicker().ShowDialog();
        }

        private void btnTest_ListBox_Click(object sender, RoutedEventArgs e)
        {
            new Test.FrmTest_ListBox().ShowDialog();
        }

        private void btnTest_TreeView_Click(object sender, RoutedEventArgs e)
        {
            new Test.FrmTest_TreeView().ShowDialog();
        }

        private void btnTest_StandardDataGridView_Click(object sender, RoutedEventArgs e)
        {
            new Test.FrmTest_StandardDataGridView().ShowDialog();
        }

        #endregion

        #region AttachUtils - 附加属性

        private void btnAttachUtils_Click(object sender, RoutedEventArgs e)
        {
            new Test.FrmTest_AttachUtils().ShowDialog();
        }

        private void btnAttachUtilsScrollViewer_Click(object sender, RoutedEventArgs e)
        {
            new Test.FrmTest_AttachUtils_ScrollViewer().ShowDialog();
        }

        #endregion

        #region 各类常用面版整理

        private void btnTestPrinterPanel_Click(object sender, RoutedEventArgs e)
        {
            new Test.FrmTestPrinterPanel().ShowDialog();
        }

        private void btnTestFrmTest_UcNetworkDeviceConfig_Click(object sender, RoutedEventArgs e)
        {
            new Test.FrmTest_UcNetworkDeviceConfig().ShowDialog();
        }

        private void btnTestFrmTest_UcSerialDeviceConfig_Click(object sender, RoutedEventArgs e)
        {
            new Test.FrmTest_UcSerialDeviceConfig().ShowDialog();
        }

        private void btnTestUcReportXxx_Click(object sender, RoutedEventArgs e)
        {
            new Test.FrmTest_UcReportXxx().ShowDialog();
        }
        private void btnTest_ToolbarControls_Click(object sender, RoutedEventArgs e)
        {
            new Test.FrmTest_ToolbarControls().ShowDialog();
        }

        private void btnTest_SearchPanelControls_Click(object sender, RoutedEventArgs e)
        {
            new Test.FrmTest_SearchPanelControls().ShowDialog();
        }

        #endregion

        private void BtnTestUcWait_Click(object sender, RoutedEventArgs e)
        {
            new Test.FrmTestUcWait().ShowDialog();
        }


        private void btnTestBusyIndicatior_Click(object sender, RoutedEventArgs e)
        {
            new Test.FrmTestBusyIndicatior().ShowDialog();
        }

        private void BtnTestUcSelectFile_Click(object sender, RoutedEventArgs e)
        {
            new Test.FrmTestSelectFile().ShowDialog();
        }

        private void BtnTestUcConsole_Click(object sender, RoutedEventArgs e)
        {
            new Test.FrmTestUcConsole().ShowDialog();
        }


        private void btnTestUcConsolePerformance_Click(object sender, RoutedEventArgs e)
        {
            new Test.FrmTest_UcConsole_Performance().ShowDialog();
        }

        private void btnTestMarqueeLabel_Click(object sender, RoutedEventArgs e)
        {
            new Test.FrmTestMarqueeLabel().ShowDialog();
        }





        private void btnTestFrmTouchKeyboard_Click(object sender, RoutedEventArgs e)
        {
            var frm = new FrmTouchKeyboard(this);
            frm.mMoveTop = 500;
            frm.mMoveLeft = 300;
            frm.Show();
        }




        private void btnTestMessageBox_Click(object sender, RoutedEventArgs e)
        {
            new Test.FrmTest_MessageBox().ShowDialog();
        }

        private void btnTestConverter_Click(object sender, RoutedEventArgs e)
        {
            new Test.FrmTest_Converter().ShowDialog();
        }

        private void btnTestWPFControlsUtils_Click(object sender, RoutedEventArgs e)
        {
            new Test.FrmTest_WPFControlsUtils().ShowDialog();
        }

        private void btnTestMessageBox_Unblock_Click(object sender, RoutedEventArgs e)
        {
            new Test.FrmTest_MessageBox_Unblock().ShowDialog();
        }


        private void btnTest_StandardDataGridView_Template_Click(object sender, RoutedEventArgs e)
        {
            new Test.FrmTest_StandardDataGridView_Template().ShowDialog();
        }



    }
}
