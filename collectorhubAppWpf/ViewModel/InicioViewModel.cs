using collectorhubAppWpf.Model;
using System.Net.Http;
using System.Windows.Input;
using System.Windows;
using MVVM.Commands;
using collectorhubAppWpf.View;
using System.Windows.Controls;
using collectorhubAppWpf.Stores;

namespace collectorhubAppWpf.ViewModel
{
    public class InicioViewModel : ViewModelBase
    {
        private readonly HttpClient _httpClient;
        private UserControl _currentView;
        public UserControl CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        // Commands para cambiar las vistas
        public ICommand ShowCreateGenreViewCommand { get; set; }
        public ICommand ShowAddMangaViewCommand { get; set; }
        public ICommand ShowCreateMangaListViewCommand { get; set; }
        public ICommand ShowReviewsViewCommand { get; set; }
        public ICommand ShowManageUsersViewCommand { get; set; }
        public ICommand ShowStatisticsViewCommand { get; set; }
        public ICommand ShowWelcomeViewCommand { get; set; }

        public ICommand NavigateToCreateUserCommand { get; set; }
        public ICommand NavigateToEditUserCommand { get; set; }

        private UserModel _userSelected;
        public UserModel UserSelected
        {
            get { return _userSelected; }
            set
            {
                if (_userSelected != value)
                {
                    _userSelected = value;
                    OnPropertyChanged(nameof(UserSelected));
                }
            }
        }

        public InicioViewModel(NavigationStore navigationStore)
        {
            CurrentView = new WelcomeView();

            // Inicializamos los comandos
            ShowCreateGenreViewCommand = new RelayCommand(param => ShowCreateGenreView());
            ShowAddMangaViewCommand = new RelayCommand(param => ShowAddMangaView());
            ShowCreateMangaListViewCommand = new RelayCommand(param => ShowCreateMangaListView());
            ShowReviewsViewCommand = new RelayCommand(param => ShowReviewsView());
            ShowManageUsersViewCommand = new RelayCommand(param => ShowManageUsersView());
            ShowStatisticsViewCommand = new RelayCommand(param => ShowStatisticsView());
            ShowWelcomeViewCommand = new RelayCommand(param => ShowWelcomeView());
            NavigateToCreateUserCommand = new RelayCommand(param => OpenCreateUserView());
            NavigateToEditUserCommand = new RelayCommand(param => OpenUpdateUserView());
        }

        private void OpenUpdateUserView()
        {
            if (UserSelected != null)
            {
                CurrentView = new UpdateUserView(UserSelected);
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un usuario para editar.");
            }
        }

        private void ShowCreateGenreView()
        {
            CurrentView = new CreateGenreView();
        }

        private void ShowWelcomeView()
        {
            CurrentView = new WelcomeView();
        }

        private void ShowAddMangaView()
        {
            CurrentView = new CreateMangaView();
        }

        private void OpenCreateUserView()
        {
            CurrentView = new CreateUserView(this);
        }

        private void ShowCreateMangaListView()
        {
            CurrentView = new CreateMangaListView();
        }

        private void ShowReviewsView()
        {
            CurrentView = new ReviewsView();
        }

        private void ShowManageUsersView()
        {
            //UserSelected = null;
            CurrentView = new UsersView(this);
        }

        private void ShowStatisticsView()
        {
            CurrentView = new StatsView();
        }

    }
}