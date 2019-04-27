using System.ComponentModel;
using System.Runtime.CompilerServices;
using WiredBrainCoffee.CustomersApp.Annotations;

namespace WiredBrainCoffee.CustomersApp.Base
{
    public class Observable : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}