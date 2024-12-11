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

            string loginUrl = "http://localhost:8080/auth/login";
            string userDetailsUrl = "http://localhost:8080/user/getOne/{0}";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var loginRequest = new
                    {
                        username = Username,
                        password = Password
                    };

                    var loginContent = new StringContent(JsonConvert.SerializeObject(loginRequest), Encoding.UTF8, "application/json");

                    HttpResponseMessage loginResponse = await client.PostAsync(loginUrl, loginContent);

                    if (loginResponse.IsSuccessStatusCode)
                    {
                        string loginJsonContent = await loginResponse.Content.ReadAsStringAsync();
                        dynamic loginResponseObject = JsonConvert.DeserializeObject(loginJsonContent);

                        string accessToken = loginResponseObject.accessToken;
                        string userId = loginResponseObject.userId;

                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

                        HttpResponseMessage userDetailsResponse = await client.GetAsync(string.Format(userDetailsUrl, userId));

                        if (userDetailsResponse.IsSuccessStatusCode)
                        {
                            string userDetailsJsonContent = await userDetailsResponse.Content.ReadAsStringAsync();
                            dynamic userDetailsResponseObject = JsonConvert.DeserializeObject(userDetailsJsonContent);

                            string roleName = userDetailsResponseObject.role.name;

                            if (roleName == "ADMIN")
                            {
                                Properties.Settings.Default.AccessToken = accessToken;
                                Properties.Settings.Default.Username = Username;
                                Properties.Settings.Default.Save();

                                _navigationStore.CurrentViewModel = new InicioViewModel(_navigationStore);
                            }
                            else
                            {
                                MessageBox.Show("No tienes permisos de administrador para iniciar sesión.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Error al recuperar los detalles del usuario.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Username o Password incorrectos.");
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
