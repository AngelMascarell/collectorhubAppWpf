using collectorhubAppWpf.Stores;
using collectorhubAppWpf.ViewModel;
using System.Net.Http;
using System.Windows;

namespace collectorhubAppWpf
{
    public partial class MainWindow : Window
    {
        private readonly NavigationStore _navigationStore;

        public MainWindow()
        {
            _navigationStore = new NavigationStore();

            InitializeComponent();
            loginWithTokenAsync();

            // Establecer el DataContext con el NavigationStore
            DataContext = new MainWindowViewModel(_navigationStore);

            // Intentar iniciar sesión con el token si está disponible
            
            

        }

        private async Task loginWithTokenAsync()
        {
            if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.AccessToken))
            {
                string apiUrl = "http://localhost:8080/auth/login";

                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Properties.Settings.Default.AccessToken);
                        await client.GetStringAsync(apiUrl);

                        // Cambiar a InicioViewModel si el token es válido
                        _navigationStore.CurrentViewModel = new InicioViewModel(_navigationStore);
                        return;
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

            // Si no hay token, establecer el LoginViewModel como CurrentViewModel
            _navigationStore.CurrentViewModel = new LoginViewModel(_navigationStore);
        }
    }
}
