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

            var dr = WPFControls.MessageBox.ShowInformationDialog(owner: this, m);
            System.Diagnostics.Debug.WriteLine(dr);

            dr = WPFControls.MessageBox.ShowErrorDialog(owner: this, m);
            System.Diagnostics.Debug.WriteLine(dr);

            dr = WPFControls.MessageBox.ShowWarningDialog(owner: this, m);
            System.Diagnostics.Debug.WriteLine(dr);

            dr = WPFControls.MessageBox.ShowConfirmDialog(owner: this, m);
            System.Diagnostics.Debug.WriteLine(dr);

            dr = WPFControls.MessageBox.ShowQuestionDialog(owner: this, m);
            System.Diagnostics.Debug.WriteLine(dr);

            dr = WPFControls.MessageBox.ShowQuestionDialog(owner: this, m, showCancel: true);
            System.Diagnostics.Debug.WriteLine(dr);

            //// 尽量使用上面的方式
            
            //dr = WPFControls.MessageBox.ShowDialog
            //(
            //    owner: this,
            //    message: "确定",
            //    button: MessageBoxButton.YesNoCancel,
            //    icon: MessageBoxImage.Question,
            //    defaultResult: MessageBoxResult.Yes, // 重点, 命中 Yes No Cancel, 因为默认值是 None
            //    options: MessageBoxOptions.None,
            //    autoCloseTimeSpan: null
            //);
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

        private void Button_Test_DefaultMessageBoxResult(object sender, RoutedEventArgs e)
        {
            // 模仿 MessageBox 的默认宣州 Cancel
            var dr0 = MessageBox.Show("确认关闭", "Tips", icon: MessageBoxImage.Question, button: MessageBoxButton.OKCancel, defaultResult: MessageBoxResult.Cancel);
            System.Diagnostics.Debug.WriteLine(dr0);


            var dr = WPFControls.MessageBox.ShowConfirmDialog("确认关闭", defaultResult: MessageBoxResult.Cancel);
            System.Diagnostics.Debug.WriteLine(dr);

        }

        #region Extra Content

        private void Button_ExtraContent(object sender, RoutedEventArgs e)
        {
            var msgBox = WPFControls.MessageBox.GetMessageBox4UserDefineCc
            (
                "请输入需要补打的数量"
            );

            var content = new WPFControls.MessageBoxExtraContent.CcTextBox() { Title = "打印数量" };
            var vm = new WPFControls.MessageBoxExtraContent.ViewModels.SingleItemViewModel();
            content.DataContext = vm; // ** 重点 **

            // 对 预设值 进行赋值
            vm.Value = "10"; // 预设补打 10 张

            // 编写校验业务逻辑
            vm.CheckValueLogic_UserDefine = new Func<string>(() =>
            {
                string errorMsg = string.Empty;

                if (vm.Value is null)
                {
                    return "空值";
                }

                if (int.TryParse(vm.Value.ToString(), out int qty) == false)
                {
                    return "数值校验错误";
                }

                if (qty <= 0)
                {
                    return "数值校验错误";
                }

                return errorMsg;
            });

            msgBox.ExtraContent = content;

            msgBox.ShowDialog();

            if (msgBox.MessageBoxResult == MessageBoxResult.OK)
            {
                var data = content.DataContext as WPFControls.MessageBoxExtraContent.ViewModels.SingleItemViewModel;
                MessageBox.Show(Util.JsonUtils.SerializeObject(data.Value));
            }
        }

        private void Button_ExtraContent_CcTextarea(object sender, RoutedEventArgs e)
        {
            var msgBox = WPFControls.MessageBox.GetMessageBox4UserDefineCc("请输入你的的建议");
            var cc = new WPFControls.MessageBoxExtraContent.CcTextarea() { Title = "填写你的建议" };
            var vm = new WPFControls.MessageBoxExtraContent.ViewModels.SingleItemViewModel();
            cc.DataContext = vm; // ** 重点 **

            // 编写校验业务逻辑
            vm.CheckValueLogic_UserDefine = new Func<string>(() =>
            {
                string r = string.Empty;

                if (vm.Value == null || vm.Value.ToString().IndexOf("建议") < 0)
                {
                    r = "请输入你的建议";
                }

                return r;
            });

            msgBox.ExtraContent = cc;
            msgBox.ShowDialog();

            if (msgBox.MessageBoxResult == MessageBoxResult.OK)
            {
                MessageBox.Show(vm.Value.ToString());
            }
        }

        private void Button_ExtraContent_CcDatePicker(object sender, RoutedEventArgs e)
        {
            var msgBox = WPFControls.MessageBox.GetMessageBox4UserDefineCc("请选择入住日期");
            var cc = new WPFControls.MessageBoxExtraContent.CcDatePicker()
            // { Title = "填写你的建议" }
            ;
            var vm = new WPFControls.MessageBoxExtraContent.ViewModels.SingleItemViewModel();
            cc.DataContext = vm; // ** 重点 **

            // 编写校验业务逻辑
            vm.CheckValueLogic_UserDefine = new Func<string>(() =>
            {
                string r = string.Empty;

                if (vm.Value == null)
                {
                    r = "请选择入住日期";
                }
                else if
                (
                    DateTime.TryParse(vm.Value.ToString(), out DateTime dt) == false ||
                    dt.Date <= DateTime.Now.Date
                )
                {
                    r = "日期不能晚于明天";
                }

                return r;
            });

            msgBox.ExtraContent = cc;
            msgBox.ShowDialog();

            if (msgBox.MessageBoxResult == MessageBoxResult.OK)
            {
                MessageBox.Show(vm.Value.ToString());
            }
        }

        private void Button_ExtraContent_AccountPassword(object sender, RoutedEventArgs e)
        {
            abp();
        }

        void abp(string argU = "", string argP = "")
        {
            var msgBox = WPFControls.MessageBox.GetMessageBox4UserDefineCc
            (
                "请输入管理员账号密码",
                "Hello\r\nWorld"
            );

            var vm = new WPFControls.MessageBoxExtraContent.ViewModels.AccountPasswordViewModel();

            if (argU.IsNullOrWhiteSpace() == false)
                vm.LoginAccount = argU;

            if (argP.IsNullOrEmpty() == false)
                vm.Password = argP;

            var content = new WPFControls.MessageBoxExtraContent.CcAccountPassword(vm);

            msgBox.ExtraContent = content;

            msgBox.ShowDialog();

            if (msgBox.MessageBoxResult == MessageBoxResult.OK)
            {
                var u = content.DataContext as WPFControls.MessageBoxExtraContent.ViewModels.AccountPasswordViewModel;
                if (u.LoginAccount == "a2222" && u.Password == "2")
                {
                    MessageBox.Show(Util.JsonUtils.SerializeObjectWithFormatted(u));
                }
                else
                {
                    MessageBox.Show("密码错误");
                    // 验证失败了，再次调用 并且 预设好上一次的账号
                    abp(u.LoginAccount, u.Password);
                }
            }
        }

        private void Button_ExtraContent_Password(object sender, RoutedEventArgs e)
        {
            ps();
        }

        void ps(string p = "")
        {
            var msgBox = WPFControls.MessageBox.GetMessageBox4UserDefineCc
            (
                "请输入管理员密码",
                "咨询管理员处理"
            );


            var vm = new WPFControls.MessageBoxExtraContent.ViewModels.AccountPasswordViewModel();
            if (p.IsNullOrEmpty() == false)
            {
                vm.Password = p;
            }

            var content = new WPFControls.MessageBoxExtraContent.CcPassword(vm);

            msgBox.ExtraContent = content;

            msgBox.ShowDialog();

            if (msgBox.MessageBoxResult == MessageBoxResult.OK)
            {
                var u = content.ViewModel;
                MessageBox.Show(Util.JsonUtils.SerializeObjectWithFormatted(u));

                // 如果验证失败了，再次调用 MessageBox.GetMessageBox4UserDefineCc，并且 预设好上一次的账号
                if (u.Password != "654321")
                {
                    ps(u.Password);
                }
                else
                {
                    // 继续下面的逻辑
                }
            }
        }

        #endregion
    }
}
