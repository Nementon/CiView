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
using QuickGraph;
using EndObjectExplorer.Model;
using EndObjectExplorer.ViewModel;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace EndObjectExplorer
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GraphLayoutViewModel _dataContext;

        public MainWindow()
        {
            InitializeComponent();

            CKGraphHost host = new CKGraphHost("N:|Clouds|SkyDrive|Dev|C#|Civikey|Certified|Output|Debug");
            _dataContext = new GraphLayoutViewModel(host);
            DataContext = _dataContext;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
          //  _dataContext.Graphs.Add(new FuuCKGraph());
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //if (_dataContext.Graphs.Count != 0)
             //   _dataContext.Graphs.RemoveAt(0);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //if (_dataContext.Graphs.Count == 1)
            //{
             //   CKGraph lastGraph = _dataContext.Graphs.ElementAt(0);
              
              //  CKVertex v0 = lastGraph.Vertices.ElementAt(0);
               // CKVertex v1 = new CKVertex("No Way");

                //lastGraph.AddVertex(v1);
                //lastGraph.AddEdge(new CKEdge(v0, v1));
            //}
        }
    }
}
