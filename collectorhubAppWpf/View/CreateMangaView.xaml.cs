using Intermodular2DAMGrupoCInterfaces.ViewModels;
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
using System.Windows.Shapes;

namespace collectorhubAppWpf.View
{
    /// <summary>
    /// Lógica de interacción para CreateMangaView.xaml
    /// </summary>
    public partial class CreateMangaView : Window
    {
        public CreateMangaView()
        {
            InitializeComponent();
            this.DataContext = new MangaViewModel();
        }
    }
}
