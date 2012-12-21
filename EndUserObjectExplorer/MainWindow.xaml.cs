using System.Windows;
using EndObjectExplorer.Model;
using EndObjectExplorer.ViewModel;
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

            CKGraph graph = new CKGraph("N:|Clouds|SkyDrive|Dev|C#|Civikey|Certified|Output|Debug");
            _dataContext = new GraphLayoutViewModel(graph);
            DataContext = _dataContext;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (_isVisible)
            {
                _dataContext.Graph.DetachEdgesByRunningRequirement(RunningRequirement.OptionalTryStart);
                _isVisible = false;
            }
            else
            {
                _dataContext.Graph.AttachEdgesByRunningRequirement(RunningRequirement.OptionalTryStart);
                _isVisible = true;
            }
        }
    }
}
