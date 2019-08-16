﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Client.Test
{
    /// <summary>
    /// FrmTestUcConsole.xaml 的交互逻辑
    /// </summary>
    public partial class FrmTestUcConsole : Window, System.ComponentModel.INotifyPropertyChanged
    {
        #region INotifyPropertyChanged成员

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        public FrmTestUcConsole()
        {
            InitializeComponent();
            initEvent();
        }

        private void initEvent()
        {
            btnAdd.Click += btnAdd_Click;
            btnClear.Click += BtnClear_Click;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            string msg = this.txtLog.Text;
            if (string.IsNullOrWhiteSpace(msg))
            {
                return;
            }

            string msgTypeStr = this.txtConsoleMsgType.Text;
            int consoleMsgType = 0;
            int.TryParse(msgTypeStr, out consoleMsgType);

            ucConsole.Add(new Util.Model.ConsoleData
            (
                consoleMsgType: (Util.Model.ConsoleMsgType)consoleMsgType,
                content: $"{msg}\r\n{msg}\r\n{msg}",
                entryTime: DateTime.Now
            ));
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            ucConsole.Clear();
        }
    }

}
