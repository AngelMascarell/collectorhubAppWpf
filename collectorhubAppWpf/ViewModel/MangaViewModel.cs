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

namespace collectorhubAppWpf.ViewModel
{
    public class MangaViewModel : ViewModelBase
    {
        private readonly HttpClient _httpClient;
        private MangaModel _mangaModel;
        private bool _isMangaTitleUnique = true;

        public MangaViewModel()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:8080/");
            _mangaModel = new MangaModel();
        }

        public MangaViewModel(MangaModel mangaModel)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:8080/");
            _mangaModel = mangaModel;
        }

        // Properties
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

                    // Verificar si el título es único
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

        private string _errorMessage;

        // Propiedad para el mensaje de error o aviso
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
            // Si el título está vacío, retorna false (título no existe)
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

                // Intentar crear el manga en el servidor
                //_httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Properties.Settings.Default.AccessToken);

                var response = await _httpClient.PostAsync("manga", content);

                if (response.IsSuccessStatusCode)
                {
                    // Mostrar mensaje de éxito si el manga fue creado correctamente
                    MessageBox.Show("Manga creado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    // Si el servidor retorna un conflicto (409), significa que ya existe un manga con el mismo título
                    MessageBox.Show("Ya existe un manga con ese título. Intente con otro título.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    // Otros errores
                    MessageBox.Show("Ocurrió un error al crear el manga.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                // Manejar las excepciones generales y mostrar un mensaje de error
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

        // Example Command
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

        // Método que valida si se puede guardar y establece el mensaje de error correspondiente
        private bool CanSave()
        {
            // Reiniciar el mensaje de error
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
    }
}