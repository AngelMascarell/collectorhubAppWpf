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
        private String _username;
        private string _password;
        private string _email;
        private DateTime _registerDate;
        private DateTime _birthdate;

        private List<MangaModel> _mangas;

        private string _selectedRole;

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

        public string SelectedRole
        {
            get { return _selectedRole; }
            set
            {
                if (_selectedRole != value)
                {
                    _selectedRole = value;
                    OnPropertyChanged(nameof(SelectedRole));
                }
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
            NavigateToEditUserCommand = new RelayCommand(param => NavigateToUpdateUser(inicioViewModel));

            InicioViewModel = inicioViewModel;
            UserSelected = null;
        }

        private void NavigateToUpdateUser(InicioViewModel inicioViewModel)
        {
            if (UserSelected != null)
            {
                MessageBox.Show($"Usuario seleccionado: {UserSelected.Username}");
                inicioViewModel.UserSelected = UserSelected;
                CurrentView = new UpdateUserView(UserSelected);
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un usuario para editar.");
            }
        }



        private void NavigateToAddUser()
        {
            CurrentView = new CreateUserView(_inicioViewModel);
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
            get { return _userSelected; }
            set
            {
                if (_userSelected != value)
                {
                    _userSelected = value;
                    InicioViewModel.UserSelected = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CanSelectUser));
                }
            }
        }

        public bool CanSelectUser => UserSelected != null;

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

        public string Username
        {
            get => _username;
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged(nameof(Username));
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

                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Properties.Settings.Default.AccessToken);

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
                                     : new List<MangaModel>()
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
            string apiUrl = "http://localhost:8080/user/getUsersFilter";

            var filterRequest = new
            {
                username = Username,
                email = Email,
                registerDate = RegisterDate != default(DateTime) ? (DateTime?)RegisterDate : null,
                birthdate = Birthdate != default(DateTime) ? (DateTime?)Birthdate : null,
                role = SelectedRole
            };

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Properties.Settings.Default.AccessToken);

                    var jsonContent = JsonConvert.SerializeObject(filterRequest);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        Users = JsonConvert.DeserializeObject<ObservableCollection<UserModel>>(jsonResponse);

                        _currentPageIndex = 0;

                        OnPropertyChanged(nameof(Users));
                        OnPropertyChanged(nameof(CurrentPageUsers));
                        OnPropertyChanged(nameof(CurrentPage));
                    }
                    else
                    {
                        MessageBox.Show("Error al obtener los usuarios. Código de respuesta: " + response.StatusCode);
                    }
                }
                catch (HttpRequestException ex)
                {
                    MessageBox.Show("Error al realizar la solicitud HTTP: " + ex.Message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }



    }
}
