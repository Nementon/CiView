using EndObjectExplorer.Model;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Collections.Generic;


namespace EndObjectExplorer.ViewModel
{
    public class GraphLayoutViewModel : BaseViewModel
    {
        #region Fields
        private string _selectLayoutAlg;
        #endregion

        #region Properties
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
        public List<string> LayoutAlgs { get; private set; }
        public CKGraph Graph { get; private set; }
        #endregion

        public GraphLayoutViewModel(CKGraph graph)
        {
            Contract.Requires(graph != null);
            RegisterLayoutAlgs();
            Graph = graph;
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
            SelectedLayoutAlg = "FR";
        }
        #endregion
    }
}
