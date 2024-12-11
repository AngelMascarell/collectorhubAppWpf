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

            DataContext = new MainWindowViewModel(_navigationStore);

            
            

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

            _navigationStore.CurrentViewModel = new LoginViewModel(_navigationStore);
        }
    }
}
