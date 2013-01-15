using System;
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
    public class CKVertexDragBehavior : Behavior<CKVertexControl>
    {
        private Point _mouseLastPositionOnCanvas;
        private GraphCanvas _graphCanvas;

        protected override void OnAttached()
        {
            AssociatedObject.MouseLeftButtonDown += (sender, e) =>
            {
                _graphCanvas = AssociatedObject.RootCanvas;
                _mouseLastPositionOnCanvas = e.GetPosition(_graphCanvas);
                AssociatedObject.CaptureMouse();
                e.Handled = true;
            };

            AssociatedObject.MouseLeftButtonUp += (sender, e) =>
            {
                AssociatedObject.ReleaseMouseCapture();
                e.Handled = true;
            };

            AssociatedObject.MouseMove += (sender, e) =>
            {
                Point mouseCurrentPositionOnCanvas = e.GetPosition(_graphCanvas);
                if (AssociatedObject.IsMouseCaptured)
                {
                    if (_mouseLastPositionOnCanvas != mouseCurrentPositionOnCanvas)
                    {
                        Vector diff = _mouseLastPositionOnCanvas - mouseCurrentPositionOnCanvas;

                        GraphCanvas.SetX(AssociatedObject, GraphCanvas.GetX(AssociatedObject) - diff.X);
                        GraphCanvas.SetY(AssociatedObject, GraphCanvas.GetY(AssociatedObject) - diff.Y);

                        _mouseLastPositionOnCanvas = mouseCurrentPositionOnCanvas;
                        e.Handled = true;
                    }
                }
            };
        }
    }
}
