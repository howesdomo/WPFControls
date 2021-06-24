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
    /// Interaction logic for FrmTest_MessageBox.xaml
    /// </summary>
    public partial class FrmTest_MessageBox : Window
    {
        public FrmTest_MessageBox()
        {
            InitializeComponent();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            string key = this.ToString();
            WPFControls.MessageBox.AddUserDefineFontSize(key, e.NewValue);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string m = "Info";

            WPFControls.MessageBox.ShowInformationDialog(owner: this, m);

            WPFControls.MessageBox.ShowErrorDialog(owner: this, m);

            WPFControls.MessageBox.ShowWarningDialog(owner: this, m);

            WPFControls.MessageBox.ShowConfirmDialog(owner: this, m);

            WPFControls.MessageBox.ShowQuestionDialog(owner: this, m);

            WPFControls.MessageBox.ShowDialog(owner: this, message: "确定", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            string m = Encoding.UTF8.GetString(Convert.FromBase64String("aW1wb3J0IGFuZHJvaWQuYXBwLkFjdGl2aXR5TWFuYWdlcjsNCmltcG9ydCBhbmRyb2lkLmFwcC5TZXJ2aWNlOw0KaW1wb3J0IGFuZHJvaWQuY29udGVudC5Db250ZXh0Ow0KaW1wb3J0IGFuZHJvaWQuY29udGVudC5JbnRlbnQ7DQppbXBvcnQgYW5kcm9pZC5vcy5IYW5kbGVyOw0KaW1wb3J0IGFuZHJvaWQub3MuSUJpbmRlcjsNCmltcG9ydCBhbmRyb2lkLnV0aWwuTG9nOw0KDQppbXBvcnQgamF2YS51dGlsLkxpc3Q7DQoNCi8qKg0KICogQ3JlYXRlZCBieSBnb25nd3Egb24gMjAxNy82LzE3IDAwMTcuDQogKi8NCg0KcHVibGljIGNsYXNzIE15U2VydmljZSBleHRlbmRzIFNlcnZpY2Ugew0KICAgIEFjdGl2aXR5TWFuYWdlciBhY3Rpdml0eU1hbmFnZXIgPSBudWxsOw0KICAgIEhhbmRsZXIgaGFuZGxlciA9IG5ldyBIYW5kbGVyKCk7DQogICAgU3RyaW5nIGFjdGl2aXR5TmFtZSA9IG51bGw7DQogICAgU3RyaW5nIGFjdGl2aXR5X2xhc3QgPSBudWxsOw0KICAgIFJ1bm5hYmxlIHJ1bm5hYmxlID0gbmV3IFJ1bm5hYmxlKCkgew0KDQogICAgICAgIEBPdmVycmlkZQ0KICAgICAgICBwdWJsaWMgdm9pZCBydW4oKSB7DQogICAgICAgICAgICBMaXN0PEFjdGl2aXR5TWFuYWdlci5SdW5uaW5nVGFza0luZm8+IHJ1bm5pbmdUYXNrSW5mbyA9IGFjdGl2aXR5TWFuYWdlci5nZXRSdW5uaW5nVGFza3MoMSk7DQogICAgICAgICAgICBhY3Rpdml0eU5hbWUgPSAocnVubmluZ1Rhc2tJbmZvLmdldCgwKS50b3BBY3Rpdml0eSkudG9TdHJpbmcoKTsNCiAgICAgICAgICAgIGlmICghKGFjdGl2aXR5TmFtZS5lcXVhbHMoYWN0aXZpdHlfbGFzdCkpKSB7DQogICAgICAgICAgICAgICAgTG9nLmUoIkFjdGl2aXR5TWFuYWdlcjogIiwgIuW9k+WJjWFjdGl2aXR55pivLS0tLT4iICsgYWN0aXZpdHlOYW1lKTsNCiAgICAgICAgICAgICAgICBhY3Rpdml0eV9sYXN0ID0gYWN0aXZpdHlOYW1lOw0KDQogICAgICAgICAgICB9DQogICAgICAgICAgICBoYW5kbGVyLnBvc3REZWxheWVkKHJ1bm5hYmxlLCAxMCk7DQogICAgICAgIH0NCiAgICB9Ow0KDQogICAgQE92ZXJyaWRlDQogICAgcHVibGljIGludCBvblN0YXJ0Q29tbWFuZChJbnRlbnQgaW50ZW50LCBpbnQgZmxhZ3MsIGludCBzdGFydElkKSB7DQogICAgICAgIGhhbmRsZXIucG9zdERlbGF5ZWQocnVubmFibGUsIDEwKTsNCiAgICAgICAgcmV0dXJuIHN1cGVyLm9uU3RhcnRDb21tYW5kKGludGVudCwgZmxhZ3MsIHN0YXJ0SWQpOw0KICAgIH0NCg0KICAgIEBPdmVycmlkZQ0KICAgIHB1YmxpYyB2b2lkIG9uQ3JlYXRlKCkgew0KICAgICAgICBzdXBlci5vbkNyZWF0ZSgpOw0KICAgICAgICBhY3Rpdml0eU1hbmFnZXIgPSAoQWN0aXZpdHlNYW5hZ2VyKSBnZXRTeXN0ZW1TZXJ2aWNlKENvbnRleHQuQUNUSVZJVFlfU0VSVklDRSk7DQogICAgfQ0KDQogICAgQE92ZXJyaWRlDQogICAgcHVibGljIElCaW5kZXIgb25CaW5kKEludGVudCBhcmcwKSB7DQogICAgICAgIHJldHVybiBudWxsOw0KICAgIH0NCg0KICAgIEBPdmVycmlkZQ0KICAgIHB1YmxpYyB2b2lkIG9uRGVzdHJveSgpIHsNCiAgICAgICAgaGFuZGxlci5yZW1vdmVDYWxsYmFja3MocnVubmFibGUpOw0KICAgIH0NCn0NCuKAlOKAlOKAlOKAlOKAlOKAlOKAlOKAlOKAlOKAlOKAlOKAlOKAlOKAlOKAlOKAlA0K54mI5p2D5aOw5piO77ya5pys5paH5Li6Q1NETuWNmuS4u+OAjGFuZHJvaWRfY21vc+OAjeeahOWOn+WIm+aWh+eroO+8jOmBteW+qkNDIDQuMCBCWS1TQeeJiOadg+WNj+iuru+8jOi9rOi9veivt+mZhOS4iuWOn+aWh+WHuuWkhOmTvuaOpeWPiuacrOWjsOaYjuOAgg0K5Y6f5paH6ZO+5o6l77yaaHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2FuZHJvaWRfY21vcy9hcnRpY2xlL2RldGFpbHMvNzMzODI1NzM="));

            WPFControls.MessageBox.ShowInformationDialog(owner: this, m);

            WPFControls.MessageBox.ShowErrorDialog(owner: this, m);

            WPFControls.MessageBox.ShowWarningDialog(owner: this, m);

            WPFControls.MessageBox.ShowConfirmDialog(owner: this, m);

            WPFControls.MessageBox.ShowQuestionDialog(owner: this, m);

            WPFControls.MessageBox.ShowDialog(owner: this, message: m, MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string m = "一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五";
            string d = "一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五一二三四五";

            WPFControls.MessageBox.ShowInformationDialog
            (
                owner: this,
                message: m,
                details: d
            );

            WPFControls.MessageBox.ShowQuestionDialog
            (
                owner: this,
                message: m,
                details: d
            );
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            string d = Encoding.UTF8.GetString(Convert.FromBase64String("aW1wb3J0IGFuZHJvaWQuYXBwLkFjdGl2aXR5TWFuYWdlcjsNCmltcG9ydCBhbmRyb2lkLmFwcC5TZXJ2aWNlOw0KaW1wb3J0IGFuZHJvaWQuY29udGVudC5Db250ZXh0Ow0KaW1wb3J0IGFuZHJvaWQuY29udGVudC5JbnRlbnQ7DQppbXBvcnQgYW5kcm9pZC5vcy5IYW5kbGVyOw0KaW1wb3J0IGFuZHJvaWQub3MuSUJpbmRlcjsNCmltcG9ydCBhbmRyb2lkLnV0aWwuTG9nOw0KDQppbXBvcnQgamF2YS51dGlsLkxpc3Q7DQoNCi8qKg0KICogQ3JlYXRlZCBieSBnb25nd3Egb24gMjAxNy82LzE3IDAwMTcuDQogKi8NCg0KcHVibGljIGNsYXNzIE15U2VydmljZSBleHRlbmRzIFNlcnZpY2Ugew0KICAgIEFjdGl2aXR5TWFuYWdlciBhY3Rpdml0eU1hbmFnZXIgPSBudWxsOw0KICAgIEhhbmRsZXIgaGFuZGxlciA9IG5ldyBIYW5kbGVyKCk7DQogICAgU3RyaW5nIGFjdGl2aXR5TmFtZSA9IG51bGw7DQogICAgU3RyaW5nIGFjdGl2aXR5X2xhc3QgPSBudWxsOw0KICAgIFJ1bm5hYmxlIHJ1bm5hYmxlID0gbmV3IFJ1bm5hYmxlKCkgew0KDQogICAgICAgIEBPdmVycmlkZQ0KICAgICAgICBwdWJsaWMgdm9pZCBydW4oKSB7DQogICAgICAgICAgICBMaXN0PEFjdGl2aXR5TWFuYWdlci5SdW5uaW5nVGFza0luZm8+IHJ1bm5pbmdUYXNrSW5mbyA9IGFjdGl2aXR5TWFuYWdlci5nZXRSdW5uaW5nVGFza3MoMSk7DQogICAgICAgICAgICBhY3Rpdml0eU5hbWUgPSAocnVubmluZ1Rhc2tJbmZvLmdldCgwKS50b3BBY3Rpdml0eSkudG9TdHJpbmcoKTsNCiAgICAgICAgICAgIGlmICghKGFjdGl2aXR5TmFtZS5lcXVhbHMoYWN0aXZpdHlfbGFzdCkpKSB7DQogICAgICAgICAgICAgICAgTG9nLmUoIkFjdGl2aXR5TWFuYWdlcjogIiwgIuW9k+WJjWFjdGl2aXR55pivLS0tLT4iICsgYWN0aXZpdHlOYW1lKTsNCiAgICAgICAgICAgICAgICBhY3Rpdml0eV9sYXN0ID0gYWN0aXZpdHlOYW1lOw0KDQogICAgICAgICAgICB9DQogICAgICAgICAgICBoYW5kbGVyLnBvc3REZWxheWVkKHJ1bm5hYmxlLCAxMCk7DQogICAgICAgIH0NCiAgICB9Ow0KDQogICAgQE92ZXJyaWRlDQogICAgcHVibGljIGludCBvblN0YXJ0Q29tbWFuZChJbnRlbnQgaW50ZW50LCBpbnQgZmxhZ3MsIGludCBzdGFydElkKSB7DQogICAgICAgIGhhbmRsZXIucG9zdERlbGF5ZWQocnVubmFibGUsIDEwKTsNCiAgICAgICAgcmV0dXJuIHN1cGVyLm9uU3RhcnRDb21tYW5kKGludGVudCwgZmxhZ3MsIHN0YXJ0SWQpOw0KICAgIH0NCg0KICAgIEBPdmVycmlkZQ0KICAgIHB1YmxpYyB2b2lkIG9uQ3JlYXRlKCkgew0KICAgICAgICBzdXBlci5vbkNyZWF0ZSgpOw0KICAgICAgICBhY3Rpdml0eU1hbmFnZXIgPSAoQWN0aXZpdHlNYW5hZ2VyKSBnZXRTeXN0ZW1TZXJ2aWNlKENvbnRleHQuQUNUSVZJVFlfU0VSVklDRSk7DQogICAgfQ0KDQogICAgQE92ZXJyaWRlDQogICAgcHVibGljIElCaW5kZXIgb25CaW5kKEludGVudCBhcmcwKSB7DQogICAgICAgIHJldHVybiBudWxsOw0KICAgIH0NCg0KICAgIEBPdmVycmlkZQ0KICAgIHB1YmxpYyB2b2lkIG9uRGVzdHJveSgpIHsNCiAgICAgICAgaGFuZGxlci5yZW1vdmVDYWxsYmFja3MocnVubmFibGUpOw0KICAgIH0NCn0NCuKAlOKAlOKAlOKAlOKAlOKAlOKAlOKAlOKAlOKAlOKAlOKAlOKAlOKAlOKAlOKAlA0K54mI5p2D5aOw5piO77ya5pys5paH5Li6Q1NETuWNmuS4u+OAjGFuZHJvaWRfY21vc+OAjeeahOWOn+WIm+aWh+eroO+8jOmBteW+qkNDIDQuMCBCWS1TQeeJiOadg+WNj+iuru+8jOi9rOi9veivt+mZhOS4iuWOn+aWh+WHuuWkhOmTvuaOpeWPiuacrOWjsOaYjuOAgg0K5Y6f5paH6ZO+5o6l77yaaHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2FuZHJvaWRfY21vcy9hcnRpY2xlL2RldGFpbHMvNzMzODI1NzM="));

            WPFControls.MessageBox.ShowInformationDialog
            (
                owner: this,
                message: d,
                details: d
            );

            WPFControls.MessageBox.ShowInformationDialog
            (
                owner: this,
                message: d,
                details: d
            );
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            WPFControls.MessageBox.ShowDialog(owner: this, "1", "1", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            WPFControls.MessageBox.ShowDialog(owner: this, "1", "1", MessageBoxButton.YesNo, MessageBoxImage.Question);
            WPFControls.MessageBox.ShowDialog(owner: this, "1", "1", MessageBoxButton.OK, MessageBoxImage.Question);
        }


        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            string m = "无法访问数据库";
            string d = Encoding.UTF8.GetString(Convert.FromBase64String("aW1wb3J0IGFuZHJvaWQuYXBwLkFjdGl2aXR5TWFuYWdlcjsNCmltcG9ydCBhbmRyb2lkLmFwcC5TZXJ2aWNlOw0KaW1wb3J0IGFuZHJvaWQuY29udGVudC5Db250ZXh0Ow0KaW1wb3J0IGFuZHJvaWQuY29udGVudC5JbnRlbnQ7DQppbXBvcnQgYW5kcm9pZC5vcy5IYW5kbGVyOw0KaW1wb3J0IGFuZHJvaWQub3MuSUJpbmRlcjsNCmltcG9ydCBhbmRyb2lkLnV0aWwuTG9nOw0KDQppbXBvcnQgamF2YS51dGlsLkxpc3Q7DQoNCi8qKg0KICogQ3JlYXRlZCBieSBnb25nd3Egb24gMjAxNy82LzE3IDAwMTcuDQogKi8NCg0KcHVibGljIGNsYXNzIE15U2VydmljZSBleHRlbmRzIFNlcnZpY2Ugew0KICAgIEFjdGl2aXR5TWFuYWdlciBhY3Rpdml0eU1hbmFnZXIgPSBudWxsOw0KICAgIEhhbmRsZXIgaGFuZGxlciA9IG5ldyBIYW5kbGVyKCk7DQogICAgU3RyaW5nIGFjdGl2aXR5TmFtZSA9IG51bGw7DQogICAgU3RyaW5nIGFjdGl2aXR5X2xhc3QgPSBudWxsOw0KICAgIFJ1bm5hYmxlIHJ1bm5hYmxlID0gbmV3IFJ1bm5hYmxlKCkgew0KDQogICAgICAgIEBPdmVycmlkZQ0KICAgICAgICBwdWJsaWMgdm9pZCBydW4oKSB7DQogICAgICAgICAgICBMaXN0PEFjdGl2aXR5TWFuYWdlci5SdW5uaW5nVGFza0luZm8+IHJ1bm5pbmdUYXNrSW5mbyA9IGFjdGl2aXR5TWFuYWdlci5nZXRSdW5uaW5nVGFza3MoMSk7DQogICAgICAgICAgICBhY3Rpdml0eU5hbWUgPSAocnVubmluZ1Rhc2tJbmZvLmdldCgwKS50b3BBY3Rpdml0eSkudG9TdHJpbmcoKTsNCiAgICAgICAgICAgIGlmICghKGFjdGl2aXR5TmFtZS5lcXVhbHMoYWN0aXZpdHlfbGFzdCkpKSB7DQogICAgICAgICAgICAgICAgTG9nLmUoIkFjdGl2aXR5TWFuYWdlcjogIiwgIuW9k+WJjWFjdGl2aXR55pivLS0tLT4iICsgYWN0aXZpdHlOYW1lKTsNCiAgICAgICAgICAgICAgICBhY3Rpdml0eV9sYXN0ID0gYWN0aXZpdHlOYW1lOw0KDQogICAgICAgICAgICB9DQogICAgICAgICAgICBoYW5kbGVyLnBvc3REZWxheWVkKHJ1bm5hYmxlLCAxMCk7DQogICAgICAgIH0NCiAgICB9Ow0KDQogICAgQE92ZXJyaWRlDQogICAgcHVibGljIGludCBvblN0YXJ0Q29tbWFuZChJbnRlbnQgaW50ZW50LCBpbnQgZmxhZ3MsIGludCBzdGFydElkKSB7DQogICAgICAgIGhhbmRsZXIucG9zdERlbGF5ZWQocnVubmFibGUsIDEwKTsNCiAgICAgICAgcmV0dXJuIHN1cGVyLm9uU3RhcnRDb21tYW5kKGludGVudCwgZmxhZ3MsIHN0YXJ0SWQpOw0KICAgIH0NCg0KICAgIEBPdmVycmlkZQ0KICAgIHB1YmxpYyB2b2lkIG9uQ3JlYXRlKCkgew0KICAgICAgICBzdXBlci5vbkNyZWF0ZSgpOw0KICAgICAgICBhY3Rpdml0eU1hbmFnZXIgPSAoQWN0aXZpdHlNYW5hZ2VyKSBnZXRTeXN0ZW1TZXJ2aWNlKENvbnRleHQuQUNUSVZJVFlfU0VSVklDRSk7DQogICAgfQ0KDQogICAgQE92ZXJyaWRlDQogICAgcHVibGljIElCaW5kZXIgb25CaW5kKEludGVudCBhcmcwKSB7DQogICAgICAgIHJldHVybiBudWxsOw0KICAgIH0NCg0KICAgIEBPdmVycmlkZQ0KICAgIHB1YmxpYyB2b2lkIG9uRGVzdHJveSgpIHsNCiAgICAgICAgaGFuZGxlci5yZW1vdmVDYWxsYmFja3MocnVubmFibGUpOw0KICAgIH0NCn0NCuKAlOKAlOKAlOKAlOKAlOKAlOKAlOKAlOKAlOKAlOKAlOKAlOKAlOKAlOKAlOKAlA0K54mI5p2D5aOw5piO77ya5pys5paH5Li6Q1NETuWNmuS4u+OAjGFuZHJvaWRfY21vc+OAjeeahOWOn+WIm+aWh+eroO+8jOmBteW+qkNDIDQuMCBCWS1TQeeJiOadg+WNj+iuru+8jOi9rOi9veivt+mZhOS4iuWOn+aWh+WHuuWkhOmTvuaOpeWPiuacrOWjsOaYjuOAgg0K5Y6f5paH6ZO+5o6l77yaaHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2FuZHJvaWRfY21vcy9hcnRpY2xlL2RldGFpbHMvNzMzODI1NzM="));

            WPFControls.MessageBox.ShowDialog(owner: this, m, d, MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
        }

    }
}
