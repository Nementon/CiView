using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CK.Plugin;
using EndUserObjectExplorer.Model;
using QuickGraph;

namespace EndObjectExplorer.Model
{
    public class CKGraph : BidirectionalGraph<IVertex, CKEdge> //TODO UnBidirectionalGraph<CKVertex, IEdge<CKVertex>>
    {
        public CKGraph() { }
    }
}
