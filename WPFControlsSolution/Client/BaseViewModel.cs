using System.ComponentModel;
/// <summary>
/// 1.0.2 - 2019-10-28 10:36:44
/// 整合 NotifyPropertyChanged 到 OnPropertyChanged
/// 
/// 1.0.1 - 2019-9-26 15:46:09
/// 增加 NotifyPropertyChanged 方法
/// </summary>
public class BaseViewModel : INotifyPropertyChanged
{
    #region INotifyPropertyChanged成员

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName = "")
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
}