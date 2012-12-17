using EndObjectExplorer.Model;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;


namespace EndObjectExplorer.ViewModel
{
    public class GraphLayoutViewModel : BaseViewModel
    {
        #region Fields
        private ObservableCollection<CKGraph> _graphs;
        private IVertex _selectedVertex;
        private CKGraphHost _graphHost;
        #endregion

        #region Properties
        
        public ObservableCollection<CKGraph> Graphs {
            get
            {
                return _graphHost.Graphs;
            }
         }

        public CKGraph Graph
        {
            get
            {
                return _graphHost.Graph;
            }
        }

        /// <summary>
        /// Returns the <see cref="EndObjectExplorer.Model.Graph"/>Graph</see> that the user has curently selected.
        /// </summary>
        /// TODO Implement it !
        public IVertex SelectedVertex
        {
            get
            {
                return _selectedVertex;
            }
            set
            {
                _selectedVertex = value;
            }
        }
        #endregion

        public GraphLayoutViewModel(CKGraphHost host)
        {
            Contract.Requires(host != null);
            _graphHost = host;
        }

        #region Internal Helpers
        #endregion
    }
}
