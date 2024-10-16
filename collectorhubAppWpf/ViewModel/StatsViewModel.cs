using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace collectorhubAppWpf.ViewModel
{
    internal class StatsViewModel : ViewModelBase
    {
        private int _totalMangas;
        private int _totalGenres;
        private int _totalUsers;
        private int _totalRatings;
        private double _avgRating;
        private int _totalUsersSubscribed;

        private readonly HttpClient _httpClient;

        public int TotalMangas
        {
            get => _totalMangas;
            set { _totalMangas = value; OnPropertyChanged(nameof(TotalMangas)); }
        }

        public int TotalGenres
        {
            get => _totalGenres;
            set { _totalGenres = value; OnPropertyChanged(nameof(TotalGenres)); }
        }

        public int TotalUsers
        {
            get => _totalUsers;
            set { _totalUsers = value; OnPropertyChanged(nameof(TotalUsers)); }
        }

        public int TotalRatings
        {
            get => _totalRatings;
            set { _totalRatings = value; OnPropertyChanged(nameof(TotalRatings)); }
        }

        public double AvgRating
        {
            get => _avgRating;
            set { _avgRating = value; OnPropertyChanged(nameof(AvgRating)); }
        }

        public int TotalUsersSubscribed
        {
            get => _totalUsersSubscribed;
            set { _totalUsersSubscribed = value; OnPropertyChanged(nameof(TotalUsersSubscribed)); }
        }

        public StatsViewModel()
        {
            _httpClient = new HttpClient();
            LoadStatsAsync();
        }

        private async Task LoadStatsAsync()
        {
            try
            {
                TotalMangas = await GetCountFromApiAsync("http://localhost:8080/manga/countAll");

                TotalGenres = await GetCountFromApiAsync("http://localhost:8080/api/genre/countAll");

                TotalUsers = await GetCountFromApiAsync("http://localhost:8080/user/countAll");

                TotalRatings = await GetCountFromApiAsync("http://localhost:8080/appReview/countAll");

                AvgRating = await GetDoubleFromApiAsync("http://localhost:8080/appReview/avgReviews");

                TotalUsersSubscribed = await GetCountFromApiAsync("http://localhost:8080/user/countUsersSubscribed");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error obteniendo estadísticas: {ex.Message}");
            }
        }

        private async Task<int> GetCountFromApiAsync(string url)
        {
            var response = await _httpClient.GetStringAsync(url);
            return JsonConvert.DeserializeObject<int>(response);
        }

        private async Task<double> GetDoubleFromApiAsync(string url)
        {
            var response = await _httpClient.GetStringAsync(url);
            return JsonConvert.DeserializeObject<double>(response);
        }
    }
}
