using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickGraph;

namespace GraphSharp.Controls
{

    public abstract class GraphAdapter
    {

        public virtual bool ContainsEdge(object source, object target)
        {
            throw new NotSupportedException();
        }

        public virtual bool TryGetEdge(object source, object target, out IEdge<object> edge)
        {
            throw new NotSupportedException();
        }

        public virtual bool TryGetEdges(object source, object target, out IEnumerable<IEdge<object>> edges)
        {
            throw new NotSupportedException();
        }

        public virtual bool IsOutEdgesEmpty(object v)
        {
            throw new NotSupportedException();
        }

        public virtual int OutDegree(object v)
        {
            throw new NotSupportedException();
        }

        public virtual IEdge<object> OutEdge(object v, int index)
        {
            throw new NotSupportedException();
        }

        public virtual IEnumerable<IEdge<object>> OutEdges(object v)
        {
            throw new NotSupportedException();
        }

        public virtual bool TryGetOutEdges(object v, out IEnumerable<IEdge<object>> edges)
        {
            throw new NotSupportedException();
        }

        public virtual bool AllowParallelEdges
        {
            get { throw new NotSupportedException(); }
        }

        public virtual bool IsDirected
        {
            get { throw new NotSupportedException(); }
        }

        public virtual bool ContainsVertex(object vertex)
        {
            throw new NotSupportedException();
        }

        public virtual bool IsVerticesEmpty
        {
            get { throw new NotSupportedException(); }
        }

        public virtual int VertexCount
        {
            get { throw new NotSupportedException(); }
        }

        public virtual IEnumerable<object> Vertices
        {
            get { throw new NotSupportedException(); }
        }

        public virtual bool ContainsEdge(IEdge<object> edge)
        {
            throw new NotSupportedException();
        }

        public virtual int EdgeCount
        {
            get { throw new NotSupportedException(); }
        }

        public virtual IEnumerable<IEdge<object>> Edges
        {
            get { throw new NotSupportedException(); }
        }

        public virtual bool IsEdgesEmpty
        {
            get { throw new NotSupportedException(); }
        }

        public virtual int Degree(object v)
        {
            throw new NotSupportedException();
        }

        public virtual int InDegree(object v)
        {
            throw new NotSupportedException();
        }

        public virtual IEdge<object> InEdge(object v, int index)
        {
            throw new NotSupportedException();
        }

        public virtual IEnumerable<IEdge<object>> InEdges(object v)
        {
            throw new NotSupportedException();
        }

        public virtual bool IsInEdgesEmpty(object v)
        {
            throw new NotSupportedException();
        }

        public virtual bool TryGetInEdges(object v, out IEnumerable<IEdge<object>> edges)
        {
            throw new NotSupportedException();
        }
    }

    public class GraphAdapter<TV, TE> : GraphAdapter
        where TE : IEdge<TV>
    {
        public readonly IBidirectionalGraph<TV, TE> graph;

        public override bool AllowParallelEdges
        {
            get { return graph.AllowParallelEdges; }
        }

        public override bool IsDirected
        {
            get { return graph.IsDirected; }
        }

        public override bool ContainsVertex(object vertex)
        {
            if (vertex is TV) return graph.ContainsVertex((TV)vertex);
            return false;
        }

        public override bool IsVerticesEmpty
        {
            get { return graph.IsVerticesEmpty; }
        }

        public override int VertexCount
        {
            get { return graph.VertexCount; }
        }

        public override IEnumerable<object> Vertices
        {
            get { return graph.Vertices.OfType<object>(); }
        }

        public override int EdgeCount
        {
            get { return graph.EdgeCount; }
        }

        public override bool IsEdgesEmpty
        {
            get { return graph.IsEdgesEmpty; }
        }
        public override bool IsInEdgesEmpty(object v)
        {
            if (v is TV) return graph.IsInEdgesEmpty((TV)v);
            throw new NotSupportedException();
        }
        public override bool IsOutEdgesEmpty(object v)
        {
            if (v is TV) return graph.IsOutEdgesEmpty((TV)v);
            throw new NotSupportedException();
        }
        public override int Degree(object v)
        {
            if (v is TV) return graph.Degree((TV)v);
            throw new NotSupportedException();
        }
        public override int InDegree(object v)
        {
            if (v is TV) return graph.InDegree((TV)v);
            throw new NotSupportedException();
        }
        public override int OutDegree(object v)
        {
            if (v is TV) return graph.OutDegree((TV)v);
            throw new NotSupportedException();
        }

        public override bool ContainsEdge(object source, object target)
        {
            if (source is TV && target is TV)
            {
                return graph.ContainsEdge((TV)source, (TV)target);
            }
            return false;
        }

        public override IEnumerable<IEdge<object>> Edges
        {
            get { return (IEnumerable<IEdge<object>>)graph.Edges; } // TODO: fix
        }

        public override bool ContainsEdge(IEdge<object> edge)
        {
            return this.ContainsEdge(edge.Source, edge.Target); // TODO: fix
            throw new NotSupportedException();
        }

        public override bool TryGetEdge(object source, object target, out IEdge<object> edge)
        {
            edge = null;
            if (source is TV && target is TV)
            {
                TE _edge;
                bool success = graph.TryGetEdge((TV)source, (TV)target, out _edge);
                edge = (IEdge<object>)_edge;  // TODO: fix
                return success;
            }
            return false;
        }

        public override IEdge<object> OutEdge(object v, int index)
        {
            if (v is TV) return (IEdge<object>)graph.OutEdge((TV)v, index);   // TODO: fix
            throw new NotSupportedException();
        }
        public override IEdge<object> InEdge(object v, int index)
        {
            if (v is TV) return (IEdge<object>)graph.InEdge((TV)v, index);   // TODO: fix
            throw new NotSupportedException();
        }

        public override IEnumerable<IEdge<object>> OutEdges(object v)
        {
            if (v is TV) return (IEnumerable<IEdge<object>>)graph.OutEdges((TV)v);   // TODO: fix
            throw new NotSupportedException();
        }
        public override IEnumerable<IEdge<object>> InEdges(object v)
        {
            if (v is TV) return (IEnumerable<IEdge<object>>)graph.InEdges((TV)v);   // TODO: fix
            throw new NotSupportedException();
        }
        public override bool TryGetEdges(object source, object target, out IEnumerable<IEdge<object>> edges)
        {
            edges = null;
            if (source is TV && target is TV)
            {
                IEnumerable<TE> _edges;
                bool success = graph.TryGetEdges((TV)source, (TV)target, out _edges);
                edges = (IEnumerable<IEdge<object>>)_edges;  // TODO: fix
                return success;
            }
            return false;
        }
        public override bool TryGetInEdges(object v, out IEnumerable<IEdge<object>> edges)
        {
            if (v is TV)
            {
                IEnumerable<TE> _edges;
                bool success = graph.TryGetInEdges((TV)v, out _edges);
                edges = (IEnumerable<IEdge<object>>)_edges; // TODO: fix
                return success;
            }
            throw new NotSupportedException();
        }
        public override bool TryGetOutEdges(object v, out IEnumerable<IEdge<object>> edges)
        {
            if (v is TV)
            {
                IEnumerable<TE> _edges;
                bool success = graph.TryGetOutEdges((TV)v, out _edges);
                edges = (IEnumerable<IEdge<object>>)_edges; // TODO: fix
                return success;
            }
            throw new NotSupportedException();
        }
    }
}
