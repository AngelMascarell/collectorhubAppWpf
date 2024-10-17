using collectorhubAppWpf.Stores;
using MVVM.Commands;
using Newtonsoft.Json;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using System.Text;

namespace collectorhubAppWpf.ViewModel
{
    internal class LoginViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private string _username;
        private string _password;

        public ICommand LoginCommand { get; }

        public string Username
        {
            get { return _username; }
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged();
                }
            }
        }

        public LoginViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            LoginCommand = new RelayCommand(param => Login());
        }


        private async Task Login()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Te falta username/password");
                return;
            }

            string apiUrl = "http://localhost:8080/auth/login";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Crear el objeto que quieres enviar como JSON
                    var loginRequest = new
                    {
                        username = Username,
                        password = Password
                    };

                    // Convertir a JSON y enviar con el Content-Type correcto
                    var content = new StringContent(JsonConvert.SerializeObject(loginRequest), Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonContent = await response.Content.ReadAsStringAsync();

                        // Deserialize JSON to dynamic object
                        dynamic responseObject = JsonConvert.DeserializeObject(jsonContent);

                        string accessToken = responseObject.accessToken;
                        string refreshToken = responseObject.refreshToken;

                        // Guardar los tokens en Properties (o donde lo necesites)
                        Properties.Settings.Default.AccessToken = accessToken;
                       // Properties.Settings.Default.RefreshToken = refreshToken;
                        Properties.Settings.Default.Username = Username;
                        Properties.Settings.Default.Save();

                        // Navegar a la siguiente vista (Inicio)
                        _navigationStore.CurrentViewModel = new InicioViewModel(_navigationStore);
                    }
                    else
                    {
                        MessageBox.Show($"Username or Password incorrect");
                    }
                }
                catch (HttpRequestException ex)
                {
                    MessageBox.Show($"Error al realizar la solicitud HTTP: {ex.Message}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }
    }
}
