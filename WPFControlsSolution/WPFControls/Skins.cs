namespace WPFControls
{
    /// <summary>
    /// 本类用于创建 ComponentResourceKey
    /// 本类内部不含有任何代码,只是为了在XAML中创建 ComponentResourceKey 使用
    /// </summary>
    public class Skins
    {
        // 当加载新的资源字典时，窗口中的所有DynamicResource引用都会被自动重新评估。
        // 如果不想编写任何代码，还有另一种选择。可使用ComponentResourceKey标记扩展，该标记扩展是专门针对这种情况而设计的。使用ComponentResourceKey为资源创建键名。通过执行这一步骤，告知WPF准备在程序集之间共享资源。


        // 注册资源
        // <SolidColorBrush x:Key="{ComponentResourceKey local:Skins, ImageBrush}" Color="#FF313431" />


        // 使用资源
        // <Path Data = "M0,0 L6,6 M6,0 L0,6" Fill="Transparent"
        //        Stroke="{DynamicResource {ComponentResourceKey local:Skins, ImageBrush}}" />
    }
}
