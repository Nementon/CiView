using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using EndObjectExplorer.Model;
using EndUserObjectExplorer.Behavior;
using GraphSharp;
using GraphSharp.Algorithms.Layout;
using GraphSharp.Algorithms.Layout.Simple.Tree;
using GraphSharp.Controls;

namespace EndUserObjectExplorer.Control
{
    public class CKGraphLayout : GraphLayout<IVertex, CKEdge, CKGraph>
    {

        public CKGraphLayout() 
        {
            OverlapRemovalAlgorithmType = "FSA";
            //SimpleTreeLayoutParameters param = new SimpleTreeLayoutParameters();
            //param.Direction = LayoutDirection.LeftToRight;
            //LayoutParameters = param;
            //LayoutAlgorithmType = "Tree";
            
           System.Windows.Interactivity.BehaviorCollection behaviors = System.Windows.Interactivity.Interaction.GetBehaviors(this);
           behaviors.Add(new CKGraphHighLightBehavior());
            
        }

        protected override void CreateVertexControl(IVertex vertex)
        {
            //base.CreateVertexControl(vertex);
            VertexControl presenter;
            var compoundGraph = Graph as ICompoundGraph<IVertex, CKEdge>;

            // Create the Control of the vertex
            presenter = new CKVertexControl
            {
                Vertex = vertex
            };

            //var presenter = _vertexPool.GetObject();
            //presenter.Vertex = vertex;
            _vertexControls[vertex] = presenter;
            presenter.RootCanvas = this;

            if (IsCompoundMode && compoundGraph != null && compoundGraph.IsChildVertex(vertex))
            {
                var parent = compoundGraph.GetParent(vertex);
                var parentControl = GetOrCreateVertexControl(parent) as CompoundVertexControl;

                Debug.Assert(parentControl != null);

                parentControl.Vertices.Add(presenter);
            }
            else
            {
                //add the presenter to the GraphLayout
                Children.Add(presenter);
            }

            presenter.ApplyTemplate();
            VisualStateManager.GoToState(presenter, "BeforeAdded", false);
            VisualStateManager.GoToState(presenter, "AfterAdded", true);

            //Measuring & Arrange
            presenter.InvalidateMeasure();
            //SetHighlightProperties(vertex, presenter);
        }
    }
}
