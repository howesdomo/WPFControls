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

namespace Client.Test
{
    /// <summary>
    /// Interaction logic for FrmTest_UcNetworkDeviceConfig.xaml
    /// </summary>
    public partial class FrmTest_UcNetworkDeviceConfig : Window
    {
        public FrmTest_UcNetworkDeviceConfig()
        {
            InitializeComponent();
        }
    }

    public class FrmTest_UcNetworkDeviceConfig_ViewModel : BaseViewModel
    {        
        public FrmTest_UcNetworkDeviceConfig_ViewModel()
        {
            initCMD();
        }


        #region Command

        void initCMD()
        {

            this.CMD_Submit = new Command(submit);
        }

        #endregion

        public ICommand CMD_Submit { get; private set; }

        public ICommand CMD_Cancel { get; private set; }

        void submit(object args)
        {
            if (args == null || args is Client.Components.UcNetworkDeviceConfig == false)
            {
                System.Diagnostics.Debug.WriteLine("参数不是预期 Client.Components.UcNetworkDeviceConfig");
                return;
            }

            Client.Components.UcNetworkDeviceConfig uc = args as Client.Components.UcNetworkDeviceConfig;
            if (uc.IsValid == false)
            {
                System.Diagnostics.Debug.WriteLine(uc.Error);
                return;
            }

            dynamic This = uc.DeviceConfig;
            string msg = $"Host: {This.Host}\r\nPort: {This.Port}\r\nEncoding: {This.Encoding.BodyName}";
            System.Diagnostics.Debug.WriteLine(msg);
        }
    }
}
