using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CK.Plugin;
using QuickGraph;

namespace EndObjectExplorer.Model
{
    public class CKGraph : BidirectionalGraph<IVertex, CKEdge> //TODO UnBidirectionalGraph<CKVertex, IEdge<CKVertex>>
    {
        public CKGraph() : this(false) {}

        public CKGraph(bool allowParalleEdge)
            : base(allowParalleEdge)
        {
        }

        public List<IVertex> FindLinkedVertexOf(IVertex arg)
        {
            return FindLinkedVertexOf(arg, new List<IVertex>());
        }

        private List<IVertex> FindLinkedVertexOf(IVertex arg, List<IVertex> linkedVertices)
        {
            if (!IsOutEdgesEmpty(arg))
            {
               foreach(var edge in OutEdges(arg))
               {
                   if (!linkedVertices.Contains(edge.Target))
                   {
                       linkedVertices.Add(edge.Target);
                       FindLinkedVertexOf(edge.Target, linkedVertices);
                   }
               }
            }

            if (!IsInEdgesEmpty(arg))
            {
                foreach (var edge in InEdges(arg))
                {
                    if (!linkedVertices.Contains(edge.Source))
                    {
                        linkedVertices.Add(edge.Source);
                        FindLinkedVertexOf(edge.Source, linkedVertices);
                    }
                }
            }
            return linkedVertices;
        }

        protected override void OnVertexAdded(IVertex args)
        {
            base.OnVertexAdded(args);
            args.OwnerGraph = this;
        }
    }
}
