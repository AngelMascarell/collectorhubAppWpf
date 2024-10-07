﻿using collectorhubAppWpf.Model;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;
using System.Text;
using System.IO;
using Intermodular2DAMGrupoCInterfaces.Models;
using MVVM.Commands;
using collectorhubAppWpf.ViewModels;

namespace Intermodular2DAMGrupoCInterfaces.ViewModels
{
    public class GenreViewModel : ViewModelBase
    {
        private readonly HttpClient _httpClient;
        private GenreModel _genreModel;

        public GenreViewModel()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:8080/api/");
            _genreModel = new GenreModel();
        }

        public GenreViewModel(GenreModel genreModel)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:8080/api/");
            _genreModel = genreModel;
        }

        // Properties
        public Guid Id
        {
            get { return _genreModel.Id; }
            set
            {
                if (_genreModel.Id != value)
                {
                    _genreModel.Id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        public string Name
        {
            get { return _genreModel.Name; }
            set
            {
                if (_genreModel.Name != value)
                {
                    _genreModel.Name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        // HTTP Methods
        public async Task CreateGenreAsync()
        {
            var genreRequest = new GenreModel
            {
                Name = Name
            };

            var jsonContent = JsonConvert.SerializeObject(genreRequest);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("genre", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task<GenreModel> GetGenreByIdAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"api/genre/{id}");
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<GenreModel>(jsonResponse);
        }

        public async Task<List<GenreModel>> GetAllGenresAsync()
        {
            var response = await _httpClient.GetAsync("api/genre");
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<GenreModel>>(jsonResponse);
        }

        public async Task UpdateGenreAsync(Guid id)
        {
            var genreRequest = new GenreModel
            {
                Id = Id,
                Name = Name
            };

            var jsonContent = JsonConvert.SerializeObject(genreRequest);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/genre/{id}", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteGenreAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/genre/{id}");
            response.EnsureSuccessStatusCode();
        }

        // Example Command
        private ICommand _saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new RelayCommand(
                        async param => await this.CreateGenreAsync(),
                        param => this.CanSave()
                    );
                }
                return _saveCommand;
            }
        }

        private bool CanSave()
        {
            // Implement save validation logic here
            return !string.IsNullOrEmpty(Name);
        }
    }
}