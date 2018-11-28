using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace DynamicXamlAndVM
{
    class CustViewModel : INotifyPropertyChanged
    {
        private string _id;
        private string _name;
        private string _address;
        private string _city;
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public CustViewModel()
        {
            ID = "007";
            Name = "James Bond";
            Address = "MI6";
            City = "London";

        }
        public string ID
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged();
            }
        }

        public string City
        {
            get => _city;
            set
            {
                _city = value;
                OnPropertyChanged();
            }
        }

        public ICommand ClearCommand => new RelayCommand(
            o =>
            {
                ID = "";
                Name = "";
                Address = "";
                City = "";
            },
            o => true);
    }
}
