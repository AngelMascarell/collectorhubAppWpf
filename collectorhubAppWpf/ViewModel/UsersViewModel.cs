using collectorhubAppWpf.ViewModel;
using collectorhubAppWpf.Model;
using MVVM.Commands;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using System.Windows.Controls;
using collectorhubAppWpf.View;
using collectorhubAppWpf.Stores;

namespace collectorhubAppWpf.ViewModel
{
    public class UsersViewModel : ViewModelBase
    {
        private readonly HttpClient _httpClient;
        private ObservableCollection<UserModel> _users;
        private UserModel _userSelected;

        private readonly NavigationStore _navigationStore;


        private int _currentPageIndex = 0;
        private int _itemsPerPage = 15;

        private long _guid;
        private String _userName;
        private string _password;
        private string _email;
        private DateTime _registerDate;
        private DateTime _birthdate;

        private List<MangaModel> _mangas;

        public InicioViewModel InicioViewModel { get; set; }


        private Visibility _isSearchPanelVisible = Visibility.Visible;
        public Visibility IsSearchPanelVisible
        {
            get { return _isSearchPanelVisible; }
            set
            {
                _isSearchPanelVisible = value;
                OnPropertyChanged(nameof(IsSearchPanelVisible));
            }
        }

        private Visibility _isUserListPanelVisible = Visibility.Visible;
        public Visibility IsUserListPanelVisible
        {
            get { return _isUserListPanelVisible; }
            set
            {
                _isUserListPanelVisible = value;
                OnPropertyChanged(nameof(IsUserListPanelVisible));
            }
        }


        private UserControl _currentView;
        public UserControl CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));

                IsSearchPanelVisible = _currentView == null ? Visibility.Visible : Visibility.Collapsed;
                IsUserListPanelVisible = _currentView == null ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public ICommand NavigateToCreateUserCommand { get; }
        public ICommand NavigateToEditUserCommand { get; }

        private readonly InicioViewModel _inicioViewModel;

        public UsersViewModel(InicioViewModel inicioViewModel)
        {
            _httpClient = new HttpClient();
            LoadUsersAsync();
            SearchCommand = new RelayCommand(param => ExecuteSearchCommand());
            NavigateToCreateUserCommand = new RelayCommand(param => NavigateToAddUser());
            NavigateToEditUserCommand = new RelayCommand(param => NavigateToUpdateUser());

            InicioViewModel = inicioViewModel;
            UserSelected = null;
        }

        private void NavigateToUpdateUser()
        {
            if (UserSelected != null)
            {
                MessageBox.Show($"Usuario seleccionado: {UserSelected.Username}"); // Mensaje de depuración
                _inicioViewModel.UserSelected = UserSelected;
                CurrentView = new UpdateUserView(UserSelected);
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un usuario para editar.");
            }
        }



        private void NavigateToAddUser()
        {
            CurrentView = new CreateUserView();
        }

        public ObservableCollection<UserModel> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged(nameof(Users));
                OnPropertyChanged(nameof(CurrentPageUsers));
            }
        }

        public UserModel UserSelected
        {
            get => _userSelected;
            set
            {
                _userSelected = value;
                OnPropertyChanged(nameof(UserSelected));

                InicioViewModel.UserSelected = value; // Actualiza el UserSelected en InicioViewModel
            }
        }

        public List<MangaModel> Mangas
        {
            get => _mangas;
            set
            {
                if (_mangas != value)
                {
                    _mangas = value ?? new List<MangaModel>();
                    OnPropertyChanged(nameof(Mangas));
                }
            }
        }

        public long Id
        {
            get => _guid;
            set
            {
                if (_guid != value)
                {
                    Id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        public string UserName
        {
            get => _userName;
            set
            {
                if (_userName != value)
                {
                    _userName = value;
                    OnPropertyChanged(nameof(UserName));
                }
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }

        public DateTime Birthdate
        {
            get => _birthdate;
            set
            {
                if (_birthdate != value)
                {
                    _birthdate = value;
                    OnPropertyChanged(nameof(Birthdate));
                }
            }
        }

        public DateTime RegisterDate
        {
            get => _registerDate;
            set
            {
                if (_registerDate != value)
                {
                    _registerDate = value;
                    OnPropertyChanged(nameof(RegisterDate));
                }
            }
        }

        public ICommand SearchCommand { get; }


        public ObservableCollection<UserModel> CurrentPageUsers
        {
            get
            {
                if (Users == null)
                {
                    return new ObservableCollection<UserModel>();
                }
                else
                {
                    int startIndex = _currentPageIndex * _itemsPerPage;
                    return new ObservableCollection<UserModel>(Users.Skip(startIndex).Take(_itemsPerPage));
                }
            }
        }

        public int CurrentPage => _currentPageIndex + 1;

        public ICommand NextPageCommand => new RelayCommand(NextPage, CanGoToNextPage);
        public ICommand PreviousPageCommand => new RelayCommand(PreviousPage, CanGoToPreviousPage);

        private bool CanGoToNextPage(object parameter) => Users != null && (_currentPageIndex + 1) * _itemsPerPage < Users.Count;
        private bool CanGoToPreviousPage(object parameter) => Users != null && _currentPageIndex > 0;

        private void NextPage(object parameter)
        {
            if (CanGoToNextPage(null))
            {
                _currentPageIndex++;
                OnPropertyChanged(nameof(CurrentPageUsers));
                OnPropertyChanged(nameof(CurrentPage));
            }
        }

        private void PreviousPage(object parameter)
        {
            if (CanGoToPreviousPage(null))
            {
                _currentPageIndex--;
                OnPropertyChanged(nameof(CurrentPageUsers));
                OnPropertyChanged(nameof(CurrentPage));
            }
        }


        private async Task LoadUsersAsync()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = "http://localhost:8080/user/all";
                    string jsonResponse = await client.GetStringAsync(apiUrl);

                    var jsonObject = JObject.Parse(jsonResponse);

                    var userList = jsonObject["userResponseList"];

                    Users = new ObservableCollection<UserModel>(
                        userList.Select(user => new UserModel
                        {
                            Id = (long)user["userId"],
                            Username = (string)user["username"],
                            Email = (string)user["email"],
                            Birthdate = (DateTime)user["birthdate"],
                            RegisterDate = (DateTime)user["registerDate"],
                            Mangas = user["mangas"] != null && user["mangas"].HasValues
                                     ? ((JArray)user["mangas"]).Select(manga => new MangaModel
                                     {
                                         Id = (long)manga["id"], 
                                         Title = (string)manga["title"],
                                         Author = (string)manga["author"],
                                         GenreId = (long)manga["genreId"],
                                         Chapters = (int)manga["chapters"],
                                         Completed = (bool)manga["completed"]
                                     }).ToList()
                                     : new List<MangaModel>()  // Inicializa como lista vacía si no hay mangas
                        }).ToList());
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



        private async Task ExecuteSearchCommand()
        {
            string apiUrl = "https://render-intermodular.onrender.com/user/getUsersFilter";
            StringBuilder apiUrlBuilder = new StringBuilder(apiUrl);
            apiUrlBuilder.Append("?");

            if (!string.IsNullOrEmpty(UserName))
            {
                apiUrlBuilder.Append($"name={UserName}&");
            }

            if (!string.IsNullOrEmpty(Email))
            {
                apiUrlBuilder.Append($"email={Email}&");
            }

            if (RegisterDate != default)
            {
                apiUrlBuilder.Append($"registerDate={RegisterDate:yyyy-MM-dd}&");
            }

            if (Birthdate != default)
            {
                apiUrlBuilder.Append($"birthdate={Birthdate:yyyy-MM-dd}&");
            }

            // Eliminar el último '&' si existe
            string apiUrlWithFilter = apiUrlBuilder.ToString().TrimEnd('&');

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Añadir el token de autenticación
                    //client.DefaultRequestHeaders.Add("auth-token", Properties.Default.AccessToken);

                    // Realizar la solicitud HTTP
                    string jsonResponse = await client.GetStringAsync(apiUrlWithFilter);

                    // Deserializar la respuesta
                    Users = JsonConvert.DeserializeObject<ObservableCollection<UserModel>>(jsonResponse);

                    // Reiniciar la paginación
                    _currentPageIndex = 0;

                    // Notificar cambios
                    OnPropertyChanged(nameof(Users));
                    OnPropertyChanged(nameof(CurrentPageUsers));
                    OnPropertyChanged(nameof(CurrentPage));
                }
                catch (HttpRequestException ex)
                {
                    MessageBox.Show("No existe ningún usuario con las características proporcionadas.");
                    // Optional: Puedes mostrar detalles adicionales si lo deseas
                    // MessageBox.Show($"Error al realizar la solicitud HTTP: {ex.Message}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

    }
}
