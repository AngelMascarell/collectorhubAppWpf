using collectorhubAppWpf.Model;
using collectorhubAppWpf.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace collectorhubAppWpf.View
{
    /// <summary>
    /// Lógica de interacción para CreateMangaList.xaml
    /// </summary>
    public partial class CreateMangaListView : UserControl
    {


        public CreateMangaListView()
        {
            InitializeComponent();
            DataContext = new CreateMangaListViewModel();
        }

        /*private void AddMangaButton_Click(object sender, RoutedEventArgs e)
        {
            if (MangaListBox.SelectedItems.Count > 0)
            {
                foreach (MangaModel manga in MangaListBox.SelectedItems)
                {
                    _viewModel.AddMangaToList(manga);
                }
            }
        }

        private void RemoveMangaButton_Click(object sender, RoutedEventArgs e)
        {
            // Eliminar mangas seleccionados de la lista en el ViewModel
            var selectedMangas = MangaListBox.SelectedItems.Cast<MangaModel>().ToList();
            foreach (var manga in selectedMangas)
            {
                _viewModel.RemoveMangaFromList(manga);
            }
        }

        private void CreateListButton_Click(object sender, RoutedEventArgs e)
        {
            // Crear la lista de mangas en el ViewModel
            var name = NameTextBox.Text;
            var description = DescriptionTextBox.Text;

            if (!string.IsNullOrWhiteSpace(name) && _viewModel.MangasInList.Any())
            {
                _viewModel.CreateMangaList(name, description);
                MessageBox.Show("Lista de mangas creada exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                ClearFields();
            }
            else
            {
                MessageBox.Show("Por favor, ingrese un nombre válido y agregue al menos un manga.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ClearFields()
        {
            NameTextBox.Clear();
            DescriptionTextBox.Clear();
            MangaListBox.SelectedItems.Clear();
            _viewModel.ClearMangasInList();
        }
        */
    }
}
