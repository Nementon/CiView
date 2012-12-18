using EndObjectExplorer.Model;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Collections.Generic;


namespace EndObjectExplorer.ViewModel
{
    public class GraphLayoutViewModel : BaseViewModel
    {
        #region Fields
        private IVertex _selectedVertex;
        private string _selectLayoutAlg;
        private CKGraphHost _graphHost;
        #endregion

        #region Properties
        
        public ObservableCollection<CKGraph> Graphs {
            get
            {
                return _graphHost.Graphs;
            }
         }

        public string SelectedLayoutAlg 
        {
            get
            {
                return _selectLayoutAlg;
            }
            set
            {
                _selectLayoutAlg = value;
                NotifyPropertyChanged("SelectedLayoutAlg");
            }
        }

        public List<string> LayoutAlgs
        {
            get;
            private set;
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
            RegisterLayoutAlgs();
        }

        #region Internal Helpers
        private void RegisterLayoutAlgs()
        {
            List<string> layoutAlgs = new List<string>();
            layoutAlgs.Add("Circular");
            layoutAlgs.Add("Tree");
            layoutAlgs.Add("FR");
            layoutAlgs.Add("BoundedFR");
            layoutAlgs.Add("KK");
            layoutAlgs.Add("ISOM");
            layoutAlgs.Add("LinLog");
            layoutAlgs.Add("EfficientSugiyama");
            layoutAlgs.Add("CompoundFDP");

            LayoutAlgs = layoutAlgs;
            SelectedLayoutAlg = "Tree";
        }
        #endregion
    }
}
