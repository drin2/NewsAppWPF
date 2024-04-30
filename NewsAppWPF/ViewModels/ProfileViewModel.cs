using NewsAppWPF.Commands;
using NewsAppWPF.Models;
using NewsAppWPF.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NewsAppWPF.ViewModels
{
    public class ProfileViewModel : INotifyPropertyChanged
    {
        private User _user;

        public string Name => _user?.Name;
        public string Email => _user?.Email;
        public string SubscriptionType => _user?.SubscriptionType;

        public ICommand EditCommand { get; }

        public ProfileViewModel()
        {
            EditCommand = new RelayCommand(EditProfile);
            _user = SessionManager.CurrentUser;
            OnPropertyChanged("Name");
            OnPropertyChanged("Email");
            OnPropertyChanged("SubscriptionType");
        }

        private void EditProfile()
        {
            var editWindow = new EditProfileWindow(_user);
            if (editWindow.ShowDialog() == true)
            {
                // Optionally refresh data or handle changes post-save
                OnPropertyChanged("Name");
                OnPropertyChanged("Email");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
                return false;
            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
