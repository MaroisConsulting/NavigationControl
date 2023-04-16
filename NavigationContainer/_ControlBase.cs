using System.ComponentModel;
using System.Windows.Controls;

namespace NavigationContainer
{
    public class _ControlBase : Control, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void RaisePropertyChanged(string properyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(properyName));
        }
    }
}
