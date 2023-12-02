using System.ComponentModel;

namespace DnDManager.ViewModels
{
    /// <summary>
    /// Base class for all ViewModels
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {
        //Call when the property is changed, so UI pick the change up.
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void Dispose()
        {

        }
    }
}
