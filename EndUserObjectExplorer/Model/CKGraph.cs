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
        
        public CKGraph(BidirectionAdapterGraph<IVertex, CKEdge> servicesGraph)
        {
            ConvertToBidirectionalAndInitializeGraph(servicesGraph);
        }

        protected virtual void ConvertToBidirectionalAndInitializeGraph(BidirectionAdapterGraph<IVertex, CKEdge> servivesGraph)
        {
            foreach (var vertex in servivesGraph.Vertices)
            {
                //TODO To improve : @see why i can't use BidirectionAdapterGraph<ServiceVertex, CKEdge>
                if (vertex is ServiceVertex)
                {
                    AddVertex(vertex);
                    foreach (var plugin in ((ServiceVertex)vertex).Service.Implementations)
                    {
                        PluginVertex pVertex = new PluginVertex(plugin);
                        AddVertex(pVertex);
                        // Real Implementation
                        // AddEdge(new CKEdge(pVertex, vertex));

                        // UnReal Implementation
                        // Currently used to make the view more "user friendly" until the tree-layout algorithm of is up to date.
                        AddEdge(new CKEdge(vertex, pVertex));
                    }
                }
            }

            foreach (var edge in servivesGraph.Edges)
            {
                AddEdge(edge);
            }
        }
    }
}
