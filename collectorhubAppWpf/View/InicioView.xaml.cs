using collectorhubAppWpf.Stores;
using collectorhubAppWpf.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace collectorhubAppWpf.View
{
    /// <summary>
    /// Lógica de interacción para InicioView.xaml
    /// </summary>
    public partial class InicioView : UserControl
    {

        private NavigationStore navigationStore;
        public InicioView()
        {
            InitializeComponent();
            DataContext = new InicioViewModel(navigationStore);
        }

    }
}
