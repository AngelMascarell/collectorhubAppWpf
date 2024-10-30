using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using collectorhubAppWpf.Model;

namespace collectorhubAppWpf.ViewModel
{
    public class AllGamificationViewModel : ViewModelBase
    {
        private readonly HttpClient _httpClient;
        private ObservableCollection<GamificationModel> _gamificationResponses;

        public ObservableCollection<GamificationModel> GamificationResponses
        {
            get => _gamificationResponses;
            set
            {
                _gamificationResponses = value;
                OnPropertyChanged();
            }
        }

        public AllGamificationViewModel()
        {
            _httpClient = new HttpClient();
            _gamificationResponses = new ObservableCollection<GamificationModel>();
            LoadGamificationsAsync();
        }

        private async Task LoadGamificationsAsync()
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Properties.Settings.Default.AccessToken);
            try
            {
                var response = await _httpClient.GetAsync("http://localhost:8080/gamification/getAll");
                if (response.IsSuccessStatusCode)
                {
                    var gamificationListResponse = await response.Content.ReadFromJsonAsync<GamificationListResponse>();
                    if (gamificationListResponse?.GamificationResponseList != null)
                    {
                        GamificationResponses.Clear();

                        foreach (var gamification in gamificationListResponse.GamificationResponseList)
                        {
                            if (!string.IsNullOrWhiteSpace(gamification.ImageUrl))
                            {
                                string imageUrl = $"http://localhost:8080{gamification.ImageUrl}";

                                var imageBytes = await LoadImageAsync(imageUrl);

                                if (imageBytes != null)
                                {
                                    gamification.ImageSource = ConvertBytesToImage(imageBytes);
                                }
                            }
                            GamificationResponses.Add(gamification);
                        }
                    }
                }
                else
                {
                    MessageBox.Show($"Error al cargar gamificaciones: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar gamificaciones: {ex.Message}");
            }
        }


        private async Task<byte[]> LoadImageAsync(string imageUrl)
        {
            try
            {
                var response = await _httpClient.GetAsync(imageUrl);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsByteArrayAsync();
                }
                else
                {
                    MessageBox.Show($"Error al cargar la imagen: {response.ReasonPhrase}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar la imagen: {ex.Message}");
                return null;
            }
        }

        private BitmapImage ConvertBytesToImage(byte[] imageBytes)
        {
            using (var stream = new MemoryStream(imageBytes))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = stream;
                image.EndInit();
                image.Freeze();
                return image;
            }
        }




    }
}
