using collectorhubAppWpf.Model;
using collectorhubAppWpf.View;
using MVVM.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace collectorhubAppWpf.ViewModel
{
    internal class UpdateUserViewModel : ViewModelBase
    {

        private string _username;
        private string _email;
        private string _password;
        private DateTime? _birthdate;


        private readonly HttpClient _httpClient;

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public DateTime? Birthdate
        {
            get => _birthdate;
            set
            {
                _birthdate = value;
                OnPropertyChanged(nameof(Birthdate));
            }
        }

        public ICommand NavigateToEditUserCommand { get; }

        public UpdateUserViewModel(UserModel selectedUser)
        {
            if (selectedUser != null)
            {
                Username = selectedUser.Username;
                Email = selectedUser.Email;
                Birthdate = selectedUser.Birthdate;
                Password = selectedUser.Password;
            }
        }
    }
}
