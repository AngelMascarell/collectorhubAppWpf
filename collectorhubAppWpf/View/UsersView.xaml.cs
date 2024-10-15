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
    /// Lógica de interacción para UsersView.xaml
    /// </summary>
    public partial class UsersView : UserControl
    {
        public UsersView(InicioViewModel inicioViewModel)
        {
            InitializeComponent();
            DataContext = new UsersViewModel(inicioViewModel);
        }

        private void OnUserSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var viewModel = DataContext as UsersViewModel;
            if (viewModel != null)
            {
                if (datagridUsers.SelectedItem != null)
                {
                    viewModel.UserSelected = (UserModel)datagridUsers.SelectedItem;
                }
                else
                {
                    viewModel.UserSelected = null;
                }
            }
        }

    }
}
