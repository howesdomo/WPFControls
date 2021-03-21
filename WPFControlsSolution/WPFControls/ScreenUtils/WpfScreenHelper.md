源码来自于 https://github.com/micdenny/WpfScreenHelper

About
Porting of Windows Forms Screen helper for Windows Presentation Foundation (WPF). 
It avoids dependencies on Windows Forms libraries when developing in WPF.

由于WPF的获取屏幕代码太过弱鸡(PS:想获取非主屏幕的信息做不到; 根据当前鼠标位置获取屏幕名称又做不到),
一般只能使用 WinForm 的 System.Windows.Forms.dll 来实现以上想做的功能。

而利用 WpfScreenHelper 这个类库能实现想要的功能，并且能不依赖 WinForm 的类库真棒