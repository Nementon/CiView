using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;
using EndObjectExplorer.ViewModel;
using GraphSharp.Controls;

namespace EndObjectExplorer.Behavior
{
    /// <summary>
    /// Add a draggable behavior to UIELement contains in an ItemsControl who use a Canvas as ItemsPanelTemplate
    /// </summary>
    public class CanvasInnerItemsControlDragBehavior : Behavior<UIElement>
    {
        private Point _relativePositionToCanvasImediateChild;
        private Canvas _canvas;
        private UIElement _canvasImediateChild;

        protected override void OnAttached()
        {
            base.OnAttached();
            FindCanvasAndCanvasImediateChild();

            AssociatedObject.MouseLeftButtonDown += (sender, e) =>
            {
                _relativePositionToCanvasImediateChild = e.GetPosition(_canvasImediateChild);
                AssociatedObject.CaptureMouse();
            };

            AssociatedObject.MouseLeftButtonUp += (sender, e) =>
            {
                AssociatedObject.ReleaseMouseCapture();
            };

            AssociatedObject.MouseMove += (sender, e) =>
            {
                Point relativePositionToCanvas = e.GetPosition(_canvas);
                Vector updatePosition = relativePositionToCanvas - _relativePositionToCanvasImediateChild;
    
                if (AssociatedObject.IsMouseCaptured)
                {
                    if (updatePosition.X >= 0 && updatePosition.X <= _canvas.Width)
                        Canvas.SetLeft(_canvasImediateChild, updatePosition.X);
                    if (updatePosition.Y >= 0 && updatePosition.Y <= _canvas.Height)
                        Canvas.SetTop(_canvasImediateChild, updatePosition.Y);
                }
            };
        }

        private void FindCanvasAndCanvasImediateChild()
        {
            DependencyObject child = AssociatedObject;
            DependencyObject parent = VisualTreeHelper.GetParent(AssociatedObject);
            while (parent.GetType() != typeof(Canvas))
            {
                child = parent;
                parent = VisualTreeHelper.GetParent(parent);
            }
            Debug.Assert(child is UIElement);
            Debug.Assert(parent is Canvas);
            _canvas = (Canvas)parent;
            _canvasImediateChild = (UIElement)child;
        }
    }
}
