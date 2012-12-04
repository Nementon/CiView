using System;
using EndObjectExplorer.Model; 
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GraphSharp.Controls;
using System.Collections.ObjectModel;
using System.Windows;
using System.Diagnostics.Contracts;


namespace EndObjectExplorer.ViewModel
{
    public class CKGraphLayout : GraphLayout<IVertex, CKEdge, CKGraph> 
    { 

        #region Fields
        private String _rootServiceNames;
        #endregion

        #region Properties
        public String RootServiceName 
        {
            get 
            {
                // TOOD
                // return Graph.Vertices.First().Name;
                return "Salut !";
            }
        }
        #endregion 

        public CKGraphLayout() 
        {
            OverlapRemovalAlgorithmType = "FSA";
            LayoutAlgorithmType = "Tree";
        }
    }

    public class GraphLayoutViewModel : BaseViewModel
    {
        #region Fields
        private ObservableCollection<CKGraph> _graphs;
        private CKGraph _selectedGraph;
        private CKGraphHost _graphHost;
        #endregion

        #region Properties
        
        public ObservableCollection<CKGraph> Graphs {
            get
            {
                return _graphHost.Graphs;
            }
         }

        /// <summary>
        /// Returns the <see cref="EndObjectExplorer.Model.Graph"/>Graph</see> that the user has curently selected.
        /// </summary>
        /// TODO Implement it !
        public CKGraph SelectedGraph
        {
            get
            {
                return null;
            }
            set
            {
                _selectedGraph = value;
            }
        }
        #endregion

        public GraphLayoutViewModel(CKGraphHost host)
        {
            Contract.Requires(host != null);
            _graphHost = host;
        }

        #region Internal Helpers
        int SelectedGraphIndex()
        {
            Contract.Assert((_selectedGraph != null), "Why current graph is equal to null ??");
            Contract.Ensures(Contract.Result<int>() >= 0);
            Contract.Ensures(Contract.Result<int>() < Graphs.Count);

            return Graphs.IndexOf(_selectedGraph);    
        }
        #endregion
    }
}
