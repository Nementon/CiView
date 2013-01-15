using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Interactivity;
using EndObjectExplorer.Model;
using EndObjectExplorer.Control;
using GraphSharp.Controls;

namespace EndObjectExplorer.Behavior
{
    public class CKVertexSubGraphDragBehavior : Behavior<CKVertexControl>
    {
        private List<CKVertexControl> _linkedVertexControl = new List<CKVertexControl>();
        private Point _mouseLastPositionOnCanvas;
        GraphCanvas _graphCanvas;


        protected override void OnAttached()
        {
            AssociatedObject.MouseLeftButtonDown += (sender, e) =>
            {
                _graphCanvas = AssociatedObject.RootCanvas;
                _mouseLastPositionOnCanvas = e.GetPosition(_graphCanvas);
                _linkedVertexControl = FindLinkedVertexControls();
                AssociatedObject.CaptureMouse();
            };

            AssociatedObject.MouseLeftButtonUp += (sender, e) =>
            {
                AssociatedObject.ReleaseMouseCapture();
            };

            AssociatedObject.MouseMove += (sender, e) =>
            {
                if (AssociatedObject.IsMouseCaptured)
                {
                    Point mouseCurrentPositionOnCanvas = e.GetPosition(_graphCanvas);
                    if (_mouseLastPositionOnCanvas != mouseCurrentPositionOnCanvas)
                    {
                        Vector diff = _mouseLastPositionOnCanvas - mouseCurrentPositionOnCanvas;
                        foreach (var vc in _linkedVertexControl)
                        {
                            GraphCanvas.SetX(vc, GraphCanvas.GetX(vc) - diff.X);
                            GraphCanvas.SetY(vc, GraphCanvas.GetY(vc) - diff.Y);
                        }
                        _mouseLastPositionOnCanvas = mouseCurrentPositionOnCanvas;
                    }
                }
            };
        }

        List<CKVertexControl> FindLinkedVertexControls()
        {
            var vertex = AssociatedObject.Vertex as CKVertex;
            Debug.Assert(vertex != null);

            List<IVertex> linkedVertices = vertex.OwnerGraph.FindLinkedVertexOf(vertex);
            List<CKVertexControl> linkedVerticesControl = new List<CKVertexControl>();
            linkedVerticesControl.Add(AssociatedObject);

            foreach (UIElement element in _graphCanvas.Children)
            {
                if (element is CKVertexControl)
                {
                    CKVertexControl ckV = (CKVertexControl)element;
                    if (linkedVertices.Contains(ckV.Vertex) && !linkedVerticesControl.Contains(ckV))
                    {
                        linkedVerticesControl.Add(ckV);
                    }
                }
            }
            return linkedVerticesControl;
        }
    }
}
