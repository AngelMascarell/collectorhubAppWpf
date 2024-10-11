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
using collectorhubAppWpf.View;
using System.Windows.Controls;

namespace Intermodular2DAMGrupoCInterfaces.ViewModels
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

        public InicioViewModel()
        {
            // Inicialmente mostramos el mensaje de bienvenida o una vista por defecto
            CurrentView = new WelcomeView();

            // Inicializamos los comandos
            ShowCreateGenreViewCommand = new RelayCommand(param => ShowCreateGenreView());
            ShowAddMangaViewCommand = new RelayCommand(param => ShowAddMangaView());
            //ShowCreateMangaListViewCommand = new RelayCommand(param => ShowCreateMangaListView());
            //ShowReviewsViewCommand = new RelayCommand(param => ShowReviewsView());
            //ShowManageUsersViewCommand = new RelayCommand(param => ShowManageUsersView());
            //ShowStatisticsViewCommand = new RelayCommand(param => ShowStatisticsView());
            ShowWelcomeViewCommand = new RelayCommand(param => ShowWelcomeView());
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

        private void ShowCreateMangaListView()
        {
            //CurrentView = new CreateMangaListView();
        }

        private void ShowReviewsView()
        {
            //CurrentView = new ReviewsView();
        }

        private void ShowManageUsersView()
        {
            //CurrentView = new ManageUsersView();
        }

        private void ShowStatisticsView()
        {
            //CurrentView = new StatisticsView();
        }

    }
}