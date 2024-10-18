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
    /// Lógica de interacción para UpdateUserView.xaml
    /// </summary>
    public partial class UpdateUserView : UserControl
    {

        public UpdateUserView(UserModel selectedUser)
        {
            InitializeComponent();
            DataContext = new UpdateUserViewModel(selectedUser);
        }
    }
}
