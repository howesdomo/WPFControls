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
    /// Interaction logic for FrmTest_UcSerialDeviceConfig.xaml
    /// </summary>
    public partial class FrmTest_UcSerialDeviceConfig : Window
    {
        public FrmTest_UcSerialDeviceConfig()
        {
            InitializeComponent();
        }
    }

    public class FrmTest_UcSerialDeviceConfig_ViewModel : BaseViewModel
    {
        public FrmTest_UcSerialDeviceConfig_ViewModel()
        {
            initCMD();
        }

        void initCMD()
        {
            this.CMD_Submit = new Command(submit);
        }

        #region 调用参考代码

        public ICommand CMD_Submit { get; private set; }

        public ICommand CMD_Cancel { get; private set; }

        void submit(object args)
        {
            if (args == null || args is Client.Components.UcSerialDeviceConfig == false)
            {
                System.Diagnostics.Debug.WriteLine("参数不是预期 Client.Components.UcSerialDeviceConfig");
                return;
            }

            Client.Components.UcSerialDeviceConfig uc = args as Client.Components.UcSerialDeviceConfig;
            if (uc.IsValidated == false)
            {
                System.Diagnostics.Debug.WriteLine(uc.Error);
                return;
            }
            
            string msg = Util.JsonUtils.SerializeObjectWithFormatted(uc.DeviceConfig);
            System.Diagnostics.Debug.WriteLine(msg);
        }

        #endregion

    }
}
