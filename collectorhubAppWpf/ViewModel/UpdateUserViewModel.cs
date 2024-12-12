using collectorhubAppWpf.Model;
using collectorhubAppWpf.Stores;
using collectorhubAppWpf.View;
using MVVM.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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

        public ICommand UpdateUserCommand { get; set; }
        public ICommand ShowManageUsersViewCommand { get; set; }

        private NavigationStore navigationStore;
        public InicioViewModel InicioViewModel { get; set; }

        private UserControl _currentView;

        public UserControl CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        public UpdateUserViewModel(UserModel selectedUser)
        {
            _httpClient = new HttpClient();

            if (selectedUser != null)
            {
                Username = selectedUser.Username;
                Email = selectedUser.Email;
                Birthdate = selectedUser.Birthdate;
                Password = selectedUser.Password;
            }

            UpdateUserCommand = new RelayCommand(async param => await UpdateUserAsync(selectedUser), CanUpdateUser);
            ShowManageUsersViewCommand = new RelayCommand(param => Cancel(new InicioViewModel(navigationStore)));
        }

        private void Cancel(InicioViewModel inicioViewModel)
        {

            CurrentView = new UsersView(inicioViewModel);
        }

        private bool CanUpdateUser(object param)
        {
            return !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Email);
        }

        private async Task UpdateUserAsync(UserModel selectedUser)
        {
            try
            {
                var updatedFields = new Dictionary<string, object>();

                if (!string.IsNullOrWhiteSpace(Username))
                {
                    updatedFields.Add("username", Username);
                }

                if (!string.IsNullOrWhiteSpace(Email))
                {
                    updatedFields.Add("email", Email);
                }

                if (Birthdate != null)
                {
                    updatedFields.Add("birthdate", Birthdate);
                }

               /* if (!string.IsNullOrWhiteSpace(Password))
                {
                    updatedFields.Add("password", Password);
                }
               */

                if (updatedFields.Count == 0)
                {
                    MessageBox.Show("No hay cambios para actualizar.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var json = JsonConvert.SerializeObject(updatedFields);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                string apiUrl = $"http://localhost:8080/user/upd/{selectedUser.Id}";

                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Properties.Settings.Default.AccessToken);

                var updateResponse = await _httpClient.PutAsync(apiUrl, content);

                if (updateResponse.IsSuccessStatusCode)
                {
                    MessageBox.Show("El usuario ha sido actualizado exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show($"Error al actualizar el usuario. Código de respuesta: {updateResponse.StatusCode}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Error al conectar con el servidor: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



    }
}
