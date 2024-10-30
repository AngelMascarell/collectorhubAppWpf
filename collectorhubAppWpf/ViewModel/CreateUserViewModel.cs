using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MVVM.Commands;
using System.Windows.Input;
using System.Windows;
using Newtonsoft.Json;
using collectorhubAppWpf.Model;
using System.Windows.Controls;
using collectorhubAppWpf.View;
using collectorhubAppWpf.Stores;
using Microsoft.Win32;
using System.IO;

namespace collectorhubAppWpf.ViewModel
{
    internal class CreateUserViewModel : ViewModelBase
    {
        private string _username;
        private string _email;
        private string _password;
        private DateTime? _birthdate;

        private bool _isPremium;

        private DateTime? _premiumStartDate;

        private DateTime? _premiumEndDate;

        private string _profileImageUrl;

        public string ProfileImageUrl
        {
            get => _profileImageUrl;
            set
            {
                _profileImageUrl = value;
                OnPropertyChanged();
            }
        }

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

        public ICommand SaveCommand { get; }
        public ICommand ShowManageUsersViewCommand { get; }

        public ICommand SelectImageCommand { get; }

        private readonly HttpClient _httpClient;

        private UserControl _currentView;

        public InicioViewModel InicioViewModel { get; set; }

        public UserControl CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        private NavigationStore navigationStore;

        public CreateUserViewModel(InicioViewModel inicioViewModel)
        {
            _httpClient = new HttpClient();
            SaveCommand = new RelayCommand(async param => await SaveUser());
            ShowManageUsersViewCommand = new RelayCommand(param => Cancel(new InicioViewModel(navigationStore)));
            SelectImageCommand = new RelayCommand(param => SelectImage());

            InicioViewModel = inicioViewModel;
        }

        private void Cancel(InicioViewModel inicioViewModel)
        {
            
            CurrentView = new UsersView(inicioViewModel); // Pasar 'this' como el InicioViewModel actual
        }

        private async Task SaveUser()
        {
            if (!string.IsNullOrEmpty(ProfileImageUrl))
            {
                var imageUrl = await UploadImageAsync(ProfileImageUrl);
                if (!string.IsNullOrEmpty(imageUrl)) // Cierre de paréntesis añadido
                {
                    var newUser = new
                    {
                        username = Username,
                        email = Email,
                        birthdate = Birthdate?.ToString("yyyy-MM-dd"),
                        registerDate = DateTime.Now.ToString("yyyy-MM-dd"),
                        mangas = new List<object>(),
                        password = Password,
                        isPremium = false,
                        premiumStartDate = "",
                        premiumEndDate = "",
                        profileImageUrl = imageUrl  // Asegúrate de que coincide con el nombre de la propiedad en tu entidad.
                    };

                    var json = JsonConvert.SerializeObject(newUser);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    try
                    {
                        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Properties.Settings.Default.AccessToken);

                        var response = await _httpClient.PostAsync("http://localhost:8080/user/new", content);

                        if (response.IsSuccessStatusCode)
                        {
                            MessageBox.Show("Usuario creado exitosamente", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                            ResetForm();
                        }
                        else
                        {
                            var errorMessage = await response.Content.ReadAsStringAsync();
                            MessageBox.Show($"Error al crear el usuario: {errorMessage}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error de conexión: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Error al subir la imagen de perfil.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        private void SelectImage()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;",
                Title = "Select a Profile Image"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                ProfileImageUrl = openFileDialog.FileName;
            }
        }

        private async Task<string> UploadImageAsync(string imagePath)
        {
            using var content = new MultipartFormDataContent();
            using var fileStream = File.OpenRead(imagePath);
            var fileName = Path.GetFileName(imagePath);

            var fileContent = new StreamContent(fileStream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

            content.Add(fileContent, "file", fileName);

            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Properties.Settings.Default.AccessToken);

            var response = await _httpClient.PostAsync("http://localhost:8080/gamification/upload-image", content);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }


        private void ResetForm()
        {
            Username = string.Empty;
            Email = string.Empty;
            Birthdate = null;
            Password = string.Empty;
        }
    }
}
