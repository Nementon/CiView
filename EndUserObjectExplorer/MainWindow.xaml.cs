using System.Windows;
using EndObjectExplorer.Model;
using EndObjectExplorer.ViewModel;
using EndObjectExplorer.Control;
using CK.Plugin;


namespace EndObjectExplorer
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GraphLayoutViewModel _dataContext;
        private bool _isVisible = true;

        public MainWindow()
        {
            InitializeComponent();

            CKGraph graph = new CKGraph("C:|Users|jerome|Documents|CiviKey|ck-certified|Output|Debug");
            _dataContext = new GraphLayoutViewModel(graph);
            DataContext = _dataContext;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (ConnectionPanel.IsEnabled == true)
            {
                ConnectionPanel.IsEnabled = false;
                ConnectionPanel.Visibility = Visibility.Hidden;
                ArchitecturePanel.IsEnabled = true;
                ArchitecturePanel.Visibility = Visibility.Visible;
            }
            else if (ArchitecturePanel.IsEnabled == false)
            {
                ArchitecturePanel.IsEnabled = true;
                ArchitecturePanel.Visibility = Visibility.Visible;
            }

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (ArchitecturePanel.IsEnabled == true)
            {
                ArchitecturePanel.IsEnabled = false;
                ArchitecturePanel.Visibility = Visibility.Hidden;
                ConnectionPanel.IsEnabled = true;
                ConnectionPanel.Visibility = Visibility.Visible;
            }
            else if (ConnectionPanel.IsEnabled == false)
            {
                ConnectionPanel.IsEnabled = true;
                ConnectionPanel.Visibility = Visibility.Visible;
            }
        }

        
    }
}
