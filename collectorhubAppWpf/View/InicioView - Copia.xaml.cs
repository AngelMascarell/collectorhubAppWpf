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
    /// Lógica de interacción para InicioView.xaml
    /// </summary>
    public partial class InicioView : Window
    {
        public InicioView()
        {
            InitializeComponent();

            SearchTextBox.Text = "Buscar manga...";
            SearchTextBox.Foreground = new SolidColorBrush(Colors.Gray);
        }

        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchTextBox.Text == "Buscar manga...")
            {
                SearchTextBox.Text = string.Empty; 
                SearchTextBox.Foreground = new SolidColorBrush(Colors.Black); 
            }
        }

        private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                SearchTextBox.Text = "Buscar manga..."; 
                SearchTextBox.Foreground = new SolidColorBrush(Colors.Gray); 
            }
        }


    }
}
