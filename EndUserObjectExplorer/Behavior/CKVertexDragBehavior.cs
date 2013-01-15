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
        private bool _isCtrlDown = false;

        protected override void OnAttached()
        {
            AssociatedObject.MouseLeftButtonDown += (sender, e) =>
            {
                _graphCanvas = AssociatedObject.RootCanvas;
                _mouseLastPositionOnCanvas = e.GetPosition(_graphCanvas);
                AssociatedObject.CaptureMouse();
            };

            AssociatedObject.MouseLeftButtonUp += (sender, e) =>
            {
                AssociatedObject.ReleaseMouseCapture();
                e.Handled = true;
            };

            AssociatedObject.MouseMove += (sender, e) =>
            {
                if (AssociatedObject.IsMouseCaptured && _isCtrlDown)
                {
                    Point mouseCurrentPositionOnCanvas = e.GetPosition(_graphCanvas);
                    if (_mouseLastPositionOnCanvas != mouseCurrentPositionOnCanvas)
                    {
                        Vector diff = _mouseLastPositionOnCanvas - mouseCurrentPositionOnCanvas;
                        
                        GraphCanvas.SetX(AssociatedObject, GraphCanvas.GetX(AssociatedObject) - diff.X);
                        GraphCanvas.SetY(AssociatedObject, GraphCanvas.GetY(AssociatedObject) - diff.Y);
                        
                        _mouseLastPositionOnCanvas = mouseCurrentPositionOnCanvas;
                    }
                }
            };
        }
    }
}
