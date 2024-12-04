using collectorhubAppWpf.Model;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;
using System.Text;
using System.IO;
using MVVM.Commands;
using collectorhubAppWpf.ViewModel;
using Microsoft.Win32;
using System.Net.Http.Headers;

namespace collectorhubAppWpf.ViewModel
{
    public class MangaViewModel : ViewModelBase
    {
        private readonly HttpClient _httpClient;
        private MangaModel _mangaModel;
        private bool _isMangaTitleUnique = true;

        public ICommand SelectImageCommand { get; }
        private string _imageUrl;
        public MangaViewModel()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:8080/");
            _mangaModel = new MangaModel();
            SelectImageCommand = new RelayCommand(param => SelectImage());
        }

        public MangaViewModel(MangaModel mangaModel)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:8080/");
            _mangaModel = mangaModel;
            SelectImageCommand = new RelayCommand(param => SelectImage());
        }

        public string ImageUrl
        {
            get { return _imageUrl; }
            set { _imageUrl = value; OnPropertyChanged(nameof(ImageUrl)); }
        }

        public long Id
        {
            get { return _mangaModel.Id; }
            set
            {
                if (_mangaModel.Id != value)
                {
                    _mangaModel.Id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        public string Title
        {
            get { return _mangaModel.Title; }
            set
            {
                if (_mangaModel.Title != value)
                {
                    _mangaModel.Title = value;
                    OnPropertyChanged(nameof(Title));

                    CheckIfTitleIsUniqueAsync();
                }
            }
        }

        public string Author
        {
            get { return _mangaModel.Author; }
            set
            {
                if (_mangaModel.Author != value)
                {
                    _mangaModel.Author = value;
                    OnPropertyChanged(nameof(Author));
                }
            }
        }

        public long GenreId
        {
            get { return _mangaModel.GenreId; }
            set
            {
                if (_mangaModel.GenreId != value)
                {
                    _mangaModel.GenreId = value;
                    OnPropertyChanged(nameof(GenreId));
                }
            }
        }

        public int Chapters
        {
            get { return _mangaModel.Chapters; }
            set
            {
                if (_mangaModel.Chapters != value)
                {
                    _mangaModel.Chapters = value;
                    OnPropertyChanged(nameof(Chapters));
                }
            }
        }

        public bool Completed
        {
            get { return _mangaModel.Completed; }
            set
            {
                if (_mangaModel.Completed != value)
                {
                    _mangaModel.Completed = value;
                    OnPropertyChanged(nameof(Completed));
                }
            }
        }

        public string Synopsis
        {
            get { return _mangaModel.Synopsis; }
            set
            {
                if (_mangaModel.Synopsis != value)
                {
                    _mangaModel.Synopsis = value;
                    OnPropertyChanged(nameof(Synopsis));
                }
            }
        }

        public DateTime? ReleaseDate
        {
            get { return _mangaModel.ReleaseDate; }
            set
            {
                if (_mangaModel.ReleaseDate != value)
                {
                    _mangaModel.ReleaseDate = value;
                    OnPropertyChanged(nameof(ReleaseDate));
                }
            }
        }

        private string _errorMessage;

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                if (_errorMessage != value)
                {
                    _errorMessage = value;
                    OnPropertyChanged(nameof(ErrorMessage));
                }
            }
        }

        public async Task<bool> CheckIfTitleExistsAsync(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                return false;
            }

            if (_httpClient.DefaultRequestHeaders.Authorization == null)
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Properties.Settings.Default.AccessToken);
            }

            var response = await _httpClient.GetAsync($"manga/existsByTitle/{title}");
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<bool>(jsonResponse);
        }


        private async void CheckIfTitleIsUniqueAsync()
        {
            _isMangaTitleUnique = !await CheckIfTitleExistsAsync(Title);
            OnPropertyChanged(nameof(CanSave));
        }

        // HTTP Methods
        public async Task CreateMangaAsync()
        {
            try
            {
                //var imageUrl = "http://localhost:8080/gamification/images/3494d2e5-9c8a-46e0-a970-7bce4c7523e2_Miyamoto-Musashi.jpg";
                if (!string.IsNullOrEmpty(ImageUrl))
                {
                    var imageUrl = await UploadImageAsync(ImageUrl);
                    if (!string.IsNullOrEmpty(imageUrl))
                    {
                        ImageUrl = imageUrl;
                    }

                    var mangaRequest = new MangaModel
                    {
                        Id = Id,
                        Title = Title,
                        Author = Author,
                        GenreId = GenreId,
                        Chapters = Chapters,
                        Completed = Completed,
                        ImageUrl = imageUrl,
                        Synopsis = Synopsis,
                        ReleaseDate = ReleaseDate,
                    };

                    var jsonContent = JsonConvert.SerializeObject(mangaRequest);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Properties.Settings.Default.AccessToken);

                    var response = await _httpClient.PostAsync("manga", content);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Manga creado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                        ResetForm();
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                    {
                        MessageBox.Show("Ya existe un manga con ese título. Intente con otro título.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        MessageBox.Show("Ocurrió un error al crear el manga.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        public async Task<MangaModel> GetMangaByIdAsync(Guid id)
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Properties.Settings.Default.AccessToken);

            var response = await _httpClient.GetAsync($"manga/{id}");
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<MangaModel>(jsonResponse);
        }

        public async Task<List<MangaModel>> GetAllMangasAsync()
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Properties.Settings.Default.AccessToken);

            var response = await _httpClient.GetAsync("manga");
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<MangaModel>>(jsonResponse);
        }

        public async Task UpdateMangaAsync(Guid id)
        {
            var mangaRequest = new MangaModel
            {
                Id = Id,
                Title = Title,
                Author = Author,
                GenreId = GenreId,
                Chapters = Chapters,
                Completed = Completed
            };

            var jsonContent = JsonConvert.SerializeObject(mangaRequest);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Properties.Settings.Default.AccessToken);

            var response = await _httpClient.PutAsync($"manga/{id}", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteMangaAsync(Guid id)
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Properties.Settings.Default.AccessToken);

            var response = await _httpClient.DeleteAsync($"manga/{id}");
            response.EnsureSuccessStatusCode();
        }

        private ICommand _saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new RelayCommand(
                        async param => await this.CreateMangaAsync(),
                        param => this.CanSave()
                    );
                }
                return _saveCommand;
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
                ImageUrl = openFileDialog.FileName;
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

            //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Properties.Settings.Default.AccessToken);

            var response = await _httpClient.PostAsync("http://localhost:8080/gamification/upload-image", content);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        private bool CanSave()
        {
            ErrorMessage = string.Empty;

            if (string.IsNullOrEmpty(Title))
            {
                ErrorMessage = "El título es obligatorio.";
            }
            else if (!_isMangaTitleUnique)
            {
                ErrorMessage = "Ya existe un manga con ese título.";
            }
            else if (string.IsNullOrEmpty(Author))
            {
                ErrorMessage = "El autor es obligatorio.";
            }
            else if (GenreId <= 0)
            {
                ErrorMessage = "Debes seleccionar un género válido.";
            }
            else if (Chapters < 0)
            {
                ErrorMessage = "El número de capítulos no puede ser negativo.";
            }

            // Si no hay ningún error, se puede guardar
            return string.IsNullOrEmpty(ErrorMessage);
        }

        public void ResetForm()
        {
            Title = string.Empty;
            Author = string.Empty;
            GenreId = 0;
            Chapters = 0;
            Synopsis = string.Empty;
            ReleaseDate = null;
            Completed = false;
            ImageUrl = null;
            ErrorMessage = string.Empty;
        }

    }
}