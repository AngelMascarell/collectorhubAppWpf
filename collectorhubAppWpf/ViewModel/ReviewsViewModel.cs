using collectorhubAppWpf.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace collectorhubAppWpf.ViewModel
{
    public class ReviewsViewModel : ViewModelBase
    {
        private readonly HttpClient _httpClient;

        private ObservableCollection<ReviewModel> _appReviews;

        [JsonProperty("appReviews")]
        public ObservableCollection<ReviewModel> AppReviews
        {
            get { return _appReviews; }
            set { _appReviews = value; OnPropertyChanged(nameof(AppReviews)); }
        }

        public ReviewsViewModel()
        {
            _httpClient = new HttpClient();
            AppReviews = new ObservableCollection<ReviewModel>();
            LoadReviewsAsync();
        }

        private async Task LoadReviewsAsync()
        {
            try
            {
                string apiUrl = "http://localhost:8080/appReview/getAll";
                //_httpClient.DefaultRequestHeaders.Add("Bearer", "Bearer " + Properties.Settings.Default.AccessToken);
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer eyJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJBbmdlbCIsImlhdCI6MTcyOTE4NTA3MSwiZXhwIjoxNzI5MTg2NTExfQ.TXfpd3KfM275NE1fGKQF8kDELWKZinCX8Pp2yr3HQpo");

                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var reviewsJson = await response.Content.ReadAsStringAsync();
                    var jObject = JObject.Parse(reviewsJson); // Analiza el JSON en un JObject

                    // Extrae la lista de reseñas
                    var reviewsList = jObject["appReviewResponseList"].ToObject<List<ReviewModel>>();

                    AppReviews.Clear();

                    foreach (var review in reviewsList)
                    {
                        AppReviews.Add(review);
                        await LoadUsernameForReview(review);  // Cargar el username si es necesario
                    }
                }
                else
                {
                    Console.WriteLine($"Error al cargar las reviews: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }


        private async Task LoadUsernameForReview(ReviewModel review)
        {
            try
            {
                string apiUrl = $"http://localhost:8080/user/{review.UserId}/username";
                //_httpClient.DefaultRequestHeaders.Add("Bearer", "Bearer " + Properties.Settings.Default.AccessToken);
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer eyJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJBbmdlbCIsImlhdCI6MTcyOTE4NTA3MSwiZXhwIjoxNzI5MTg2NTExfQ.TXfpd3KfM275NE1fGKQF8kDELWKZinCX8Pp2yr3HQpo");

                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string username = await response.Content.ReadAsStringAsync();
                    review.Username = username; 
                }
                else
                {
                    review.Username = "Desconocido";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el nombre de usuario: {ex.Message}");
                review.Username = "Desconocido";
            }
        }
    }
}
