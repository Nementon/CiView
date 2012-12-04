using System.Collections.Generic;
using System.Linq;
using GraphSharp.Algorithms.Highlight;
using QuickGraph;
using System.Windows;

namespace GraphSharp.Controls
{
    public partial class GraphLayout<TVertex, TEdge, TGraph> : IHighlightController<TVertex, TEdge, TGraph>
        where TVertex : class
        where TEdge : IEdge<TVertex>
        where TGraph : class, IBidirectionalGraph<TVertex, TEdge>
    {
        #region IHighlightController<TVertex,TEdge,TGraph> Members
        private readonly IDictionary<TVertex, object> highlightedVertices = new Dictionary<TVertex, object>();
        private readonly IDictionary<TVertex, object> semiHighlightedVertices = new Dictionary<TVertex, object>();
        private readonly IDictionary<TEdge, object> highlightedEdges = new Dictionary<TEdge, object>();
        private readonly IDictionary<TEdge, object> semiHighlightedEdges = new Dictionary<TEdge, object>();

        public IEnumerable<TVertex> HighlightedVertices
        {
            get { return highlightedVertices.Keys.ToArray(); }
        }

        public IEnumerable<TVertex> SemiHighlightedVertices
        {
            get { return semiHighlightedVertices.Keys.ToArray(); }
        }

        public IEnumerable<TEdge> HighlightedEdges
        {
            get { return highlightedEdges.Keys.ToArray(); }
        }

        public IEnumerable<TEdge> SemiHighlightedEdges
        {
            get { return semiHighlightedEdges.Keys.ToArray(); }
        }

        public bool IsHighlightedVertex(TVertex vertex)
        {
            return highlightedVertices.ContainsKey(vertex);
        }

        public bool IsHighlightedVertex(TVertex vertex, out object highlightInfo)
        {
            return highlightedVertices.TryGetValue(vertex, out highlightInfo);
        }

        public bool IsSemiHighlightedVertex(TVertex vertex)
        {
            return semiHighlightedVertices.ContainsKey(vertex);
        }

        public bool IsSemiHighlightedVertex(TVertex vertex, out object semiHighlightInfo)
        {
            return semiHighlightedVertices.TryGetValue(vertex, out semiHighlightInfo);
        }

        public bool IsHighlightedEdge(TEdge edge)
        {
            return highlightedEdges.ContainsKey(edge);
        }

        public bool IsHighlightedEdge(TEdge edge, out object highlightInfo)
        {
            return highlightedEdges.TryGetValue(edge, out highlightInfo);
        }

        public bool IsSemiHighlightedEdge(TEdge edge)
        {
            return semiHighlightedEdges.ContainsKey(edge);
        }

        public bool IsSemiHighlightedEdge(TEdge edge, out object semiHighlightInfo)
        {
            return semiHighlightedEdges.TryGetValue(edge, out semiHighlightInfo);
        }

        public void HighlightVertex(TVertex vertex, object highlightInfo)
        {
            highlightedVertices[vertex] = highlightInfo;
            VertexControl vc;
            if (_vertexControls.TryGetValue(vertex, out vc))
            {
                GraphElementBehavior.SetIsHighlighted(vc, true);
                GraphElementBehavior.SetHighlightInfo(vc, highlightInfo);
            }
        }

        public void SemiHighlightVertex(TVertex vertex, object semiHighlightInfo)
        {
            semiHighlightedVertices[vertex] = semiHighlightInfo;
            VertexControl vc;
            if (_vertexControls.TryGetValue(vertex, out vc))
            {
                GraphElementBehavior.SetIsSemiHighlighted(vc, true);
                GraphElementBehavior.SetSemiHighlightInfo(vc, semiHighlightInfo);
            }
        }

        public void HighlightEdge(TEdge edge, object highlightInfo)
        {
            highlightedEdges[edge] = highlightInfo;
            EdgeControl ec;
            if (_edgeControls.TryGetValue(edge, out ec))
            {
                GraphElementBehavior.SetIsHighlighted(ec, true);
                GraphElementBehavior.SetHighlightInfo(ec, highlightInfo);
            }
        }

        public void SemiHighlightEdge(TEdge edge, object semiHighlightInfo)
        {
            semiHighlightedEdges[edge] = semiHighlightInfo;
            EdgeControl ec;
            if (_edgeControls.TryGetValue(edge, out ec))
            {
                GraphElementBehavior.SetIsSemiHighlighted(ec, true);
                GraphElementBehavior.SetSemiHighlightInfo(ec, semiHighlightInfo);
            }
        }

        public void RemoveHighlightFromVertex(TVertex vertex)
        {
            highlightedVertices.Remove(vertex);
            VertexControl vc;
            if (_vertexControls.TryGetValue(vertex, out vc))
            {
                GraphElementBehavior.SetIsHighlighted(vc, false);
                GraphElementBehavior.SetHighlightInfo(vc, null);
            }
        }

        public void RemoveSemiHighlightFromVertex(TVertex vertex)
        {
            semiHighlightedVertices.Remove(vertex);
            VertexControl vc;
            if (_vertexControls.TryGetValue(vertex, out vc))
            {
                GraphElementBehavior.SetIsSemiHighlighted(vc, false);
                GraphElementBehavior.SetSemiHighlightInfo(vc, null);
            }
        }

        public void RemoveHighlightFromEdge(TEdge edge)
        {
            highlightedEdges.Remove(edge);
            EdgeControl ec;
            if (_edgeControls.TryGetValue(edge, out ec))
            {
                GraphElementBehavior.SetIsHighlighted(ec, false);
                GraphElementBehavior.SetHighlightInfo(ec, null);
            }
        }

        public void RemoveSemiHighlightFromEdge(TEdge edge)
        {
            semiHighlightedEdges.Remove(edge);
            EdgeControl ec;
            if (_edgeControls.TryGetValue(edge, out ec))
            {
                GraphElementBehavior.SetIsSemiHighlighted(ec, false);
                GraphElementBehavior.SetSemiHighlightInfo(ec, null);
            }
        }

        #endregion

        private void SetHighlightProperties(TVertex vertex, VertexControl presenter)
        {
            object highlightInfo;
            if (IsHighlightedVertex(vertex, out highlightInfo))
            {
                GraphElementBehavior.SetIsHighlighted(presenter, true);
                GraphElementBehavior.SetHighlightInfo(presenter, highlightInfo);
            }

            object semiHighlightInfo;
            if (IsSemiHighlightedVertex(vertex, out semiHighlightInfo))
            {
                GraphElementBehavior.SetIsSemiHighlighted(presenter, true);
                GraphElementBehavior.SetSemiHighlightInfo(presenter, semiHighlightInfo);
            }
        }

        private void SetHighlightProperties(TEdge edge, EdgeControl edgeControl)
        {
            object highlightInfo;
            if (IsHighlightedEdge(edge, out highlightInfo))
            {
                GraphElementBehavior.SetIsHighlighted(edgeControl, true);
                GraphElementBehavior.SetHighlightInfo(edgeControl, highlightInfo);
            }

            object semiHighlightInfo;
            if (IsSemiHighlightedEdge(edge, out semiHighlightInfo))
            {
                GraphElementBehavior.SetIsSemiHighlighted(edgeControl, true);
                GraphElementBehavior.SetSemiHighlightInfo(edgeControl, semiHighlightInfo);
            }
        }

        public IHighlightAlgorithm<TVertex, TEdge, TGraph> HighlightAlgorithm
        {
            get { return (IHighlightAlgorithm<TVertex, TEdge, TGraph>)GetValue(HighlightAlgorithmProperty); }
            protected set { SetValue(HighlightAlgorithmPropertyKey, value); }
        }

        public IHighlightAlgorithmFactory<TVertex, TEdge, TGraph> HighlightAlgorithmFactory
        {
            get { return (IHighlightAlgorithmFactory<TVertex, TEdge, TGraph>)GetValue(HighlightAlgorithmFactoryProperty); }
            set { SetValue(HighlightAlgorithmFactoryProperty, value); }
        }

        public string HighlightAlgorithmType
        {
            get { return (string)GetValue(HighlightAlgorithmTypeProperty); }
            set { SetValue(HighlightAlgorithmTypeProperty, value); }
        }
        public static readonly DependencyProperty HighlightAlgorithmFactoryProperty =
            DependencyProperty.Register("HighlightAlgorithmFactory",
                                typeof(IHighlightAlgorithmFactory<TVertex, TEdge, TGraph>),
                                typeof(GraphLayout<TVertex, TEdge, TGraph>),
                                new PropertyMetadata(
                                    new StandardHighlightAlgorithmFactory<TVertex, TEdge, TGraph>(),
                                    HighlightAlgorithmFactory_PropertyChanged, HighlightAlgorithmFactory_Coerce));


        protected static readonly DependencyPropertyKey HighlightAlgorithmPropertyKey =
            DependencyProperty.RegisterReadOnly("HighlightAlgorithm",
                                                typeof(IHighlightAlgorithm<TVertex, TEdge, TGraph>),
                                                typeof(GraphLayout<TVertex, TEdge, TGraph>),
                                                new UIPropertyMetadata(null, HighlightAlgorithm_PropertyChanged));
        public static readonly DependencyProperty HighlightAlgorithmProperty = HighlightAlgorithmPropertyKey.DependencyProperty;

        public static readonly DependencyProperty HighlightAlgorithmTypeProperty =
            DependencyProperty.Register("HighlightAlgorithmType", typeof(string),
                                        typeof(GraphLayout<TVertex, TEdge, TGraph>),
                                        new PropertyMetadata(string.Empty, HighlightAlgorithmType_PropertyChanged,
                                                             HighlightAlgorithmType_Coerce));

        public static readonly DependencyProperty HighlightParametersProperty =
            DependencyProperty.Register("HighlightParameters", typeof(IHighlightParameters),
                                        typeof(GraphLayout<TVertex, TEdge, TGraph>),
                                        new PropertyMetadata(null, null, HighlightParameters_Coerce));

        public void HighlightTriggerEventHandler(object sender, HighlightTriggeredEventArgs args)
        {
            OnHighlightTriggered(args, this);
        }

        private static void OnHighlightTriggered(HighlightTriggeredEventArgs args, GraphLayout<TVertex, TEdge, TGraph> gl)
        {
            if (gl.Graph == null || gl.HighlightAlgorithm == null)
                return;

            if (args.OriginalSource is VertexControl)
            {
                var vc = (VertexControl)args.OriginalSource;
                var vertex = vc.Vertex as TVertex;
                if (vertex == null || !gl.Graph.ContainsVertex(vertex))
                    return;

                if (args.IsPositiveTrigger) gl.HighlightAlgorithm.OnVertexHighlighting(vertex);
                else gl.HighlightAlgorithm.OnVertexHighlightRemoving(vertex);
            }
            else if (args.OriginalSource is EdgeControl)
            {
                var ec = (EdgeControl)args.OriginalSource;
                var edge = default(TEdge);
                try
                {
                    edge = (TEdge)ec.Edge;
                }
                catch
                {
                }
                if (Equals(edge, default(TEdge)) || !gl.Graph.ContainsEdge(edge)) return;

                if (args.IsPositiveTrigger) gl.HighlightAlgorithm.OnEdgeHighlighting(edge);
                else gl.HighlightAlgorithm.OnEdgeHighlightRemoving(edge);
            }
        }

        protected static object HighlightAlgorithmFactory_Coerce(DependencyObject obj, object newValue)
        {
            var gl = (GraphLayout<TVertex, TEdge, TGraph>)obj;
            return newValue ?? gl.HighlightAlgorithmFactory;
        }

        private static void HighlightAlgorithmFactory_PropertyChanged(DependencyObject obj,
                                                                      DependencyPropertyChangedEventArgs e)
        {
            var gl = (GraphLayout<TVertex, TEdge, TGraph>)obj;

            var highlightMethod = gl.HighlightAlgorithmType;
            gl.HighlightAlgorithmType = null;
            gl.HighlightAlgorithmType = highlightMethod;
        }

        private static object HighlightAlgorithmType_Coerce(DependencyObject obj, object newValue)
        {
            var gl = (GraphLayout<TVertex, TEdge, TGraph>)obj;

            if (!gl.HighlightAlgorithmFactory.IsValidMode(newValue as string))
                return null;

            return newValue;
        }

        protected static void HighlightAlgorithmType_PropertyChanged(DependencyObject obj,
                                                                     DependencyPropertyChangedEventArgs e)
        {
            var gl = (GraphLayout<TVertex, TEdge, TGraph>)obj;

            string newAlgoType = e.NewValue == null ? string.Empty : e.NewValue.ToString();

            //regenerate algorithm, parameters
            var parameters = gl.HighlightAlgorithmFactory.CreateParameters(newAlgoType, gl.HighlightParameters);
            gl.HighlightAlgorithm = gl.HighlightAlgorithmFactory.CreateAlgorithm(newAlgoType,
                                                                                 gl.CreateHighlightContext(), gl,
                                                                                 parameters);
        }

        private static void HighlightAlgorithm_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var algo = e.NewValue as IHighlightAlgorithm<TVertex, TEdge, TGraph>;
            if (algo != null)
                algo.ResetHighlight();
        }

        private static object HighlightParameters_Coerce(DependencyObject obj, object newValue)
        {
            var gl = (GraphLayout<TVertex, TEdge, TGraph>)obj;

            if (gl.HighlightAlgorithm != null)
            {
                gl.HighlightAlgorithm.TrySetParameters(newValue as IHighlightParameters);
                return gl.HighlightAlgorithm.Parameters;
            }
            return null;
        }

        public IHighlightParameters HighlightParameters
        {
            get { return (IHighlightParameters)GetValue(HighlightParametersProperty); }
            set { SetValue(HighlightParametersProperty, value); }
        }

        protected virtual IHighlightContext<TVertex, TEdge, TGraph> CreateHighlightContext()
        {
            return new HighlightContext<TVertex, TEdge, TGraph>(Graph);
        }

        public GraphLayout()
        {
            AddHandler(GraphElementBehavior.HighlightTriggeredEvent, new HighlightTriggerEventHandler(HighlightTriggerEventHandler));
        }

    }
}