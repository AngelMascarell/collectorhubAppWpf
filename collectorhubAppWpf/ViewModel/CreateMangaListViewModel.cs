using collectorhubAppWpf.Model;
using MVVM.Commands;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace collectorhubAppWpf.ViewModel
{
    internal class CreateMangaListViewModel : ViewModelBase
    {
        public ObservableCollection<MangaModel> Mangas { get; set; } = new ObservableCollection<MangaModel>();
        public ObservableCollection<MangaModel> SelectedMangas { get; set; } = new ObservableCollection<MangaModel>();
        public ICommand AddSelectedMangasCommand { get; }

        private string _name;
        private string _description;

        private readonly HttpClient _httpClient;

        [JsonProperty("name")]
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }

        [JsonProperty("description")]
        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(nameof(Description)); }
        }

        public CreateMangaListViewModel()
        {
            _httpClient = new HttpClient();
            LoadMangas();
            AddSelectedMangasCommand = new RelayCommand(param => AddSelectedMangas());
        }

        private async void AddSelectedMangas()
        {
            SelectedMangas.Clear();
            foreach (var manga in Mangas.Where(m => m.IsSelected))
            {
                SelectedMangas.Add(manga);
            }

            if (SelectedMangas.Count == 0)
            {
                MessageBox.Show("No hay mangas seleccionados para añadir a la lista.");
                return;
            }

            if (string.IsNullOrWhiteSpace(Name))
            {
                MessageBox.Show("El campo 'Nombre' no puede estar vacío.");
                return;
            }

            if (string.IsNullOrWhiteSpace(Description))
            {
                MessageBox.Show("El campo 'Descripción' no puede estar vacío.");
                return;
            }

            var mangaListRequest = new 
            {
                listName = Name,
                description = Description,
                mangaIds = SelectedMangas.Select(m => m.Id).ToList()
            };

            var jsonContent = new StringContent(JsonConvert.SerializeObject(mangaListRequest), Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("http://localhost:8080/mangaList/crear", jsonContent);
                response.EnsureSuccessStatusCode();

                MessageBox.Show("Lista creada exitosamente.");
                ResetForm();
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Error al crear la lista: {ex.Message}");
            }
        }

        private async void LoadMangas()
        {
            var mangas = await FetchMangasFromApi();

           
            Mangas.Clear();
            foreach (var manga in mangas)
            {
                Mangas.Add(manga);
            }

            MessageBox.Show($"Mangas cargados: {Mangas.Count}");

        }

        public async Task<List<MangaModel>> FetchMangasFromApi()
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Properties.Settings.Default.AccessToken);
                var response = await _httpClient.GetAsync("http://localhost:8080/manga/getAll");
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var jsonObject = JObject.Parse(jsonResponse);

                var mangasArray = jsonObject["mangaResponseList"] as JArray;
                return mangasArray?.ToObject<List<MangaModel>>() ?? new List<MangaModel>();
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Error al obtener los mangas: {e.Message}");
                return new List<MangaModel>();
            }
        }

        private void ResetForm()
        {
            Name = string.Empty;
            Description = string.Empty;
        }
    }
}
