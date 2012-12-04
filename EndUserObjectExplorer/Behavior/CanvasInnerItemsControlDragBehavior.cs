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
        private Point relativePositionToCanvasImediateChild;

        protected override void OnAttached()
        {
            base.OnAttached();
            Canvas ownerCanvas = FindParentCanvas();
            UIElement canvasImediateChild = FindCanvasImediateChild();

            AssociatedObject.MouseLeftButtonDown += (sender, e) =>
            {
                relativePositionToCanvasImediateChild = e.GetPosition(canvasImediateChild);
                AssociatedObject.CaptureMouse();
            };

            AssociatedObject.MouseLeftButtonUp += (sender, e) =>
            {
                AssociatedObject.ReleaseMouseCapture();
            };

            AssociatedObject.MouseMove += (sender, e) =>
            {
                Point relativePositionToCanvas = e.GetPosition(ownerCanvas);
                Vector updatePosition = relativePositionToCanvas - relativePositionToCanvasImediateChild;
    
                if (AssociatedObject.IsMouseCaptured)
                {
                    if (updatePosition.X >= 0 && updatePosition.X <= ownerCanvas.Width)
                        Canvas.SetLeft(canvasImediateChild, updatePosition.X);
                    if (updatePosition.Y >= 0 && updatePosition.Y <= ownerCanvas.Height)
                        Canvas.SetTop(canvasImediateChild, updatePosition.Y);
                }
            };
        }

        private Canvas FindParentCanvas()
        {
            DependencyObject parentCanvas;
            parentCanvas = VisualTreeHelper.GetParent(AssociatedObject);
            while (parentCanvas.GetType() != typeof(Canvas))
            {
                parentCanvas = VisualTreeHelper.GetParent(parentCanvas);
            }
            Debug.Assert(parentCanvas is Canvas);
            return (Canvas)parentCanvas;
        }

        private UIElement FindCanvasImediateChild()
        {
            DependencyObject child = AssociatedObject;
            DependencyObject parent = VisualTreeHelper.GetParent(AssociatedObject);
            while (parent.GetType() != typeof(Canvas))
            {
                child = parent;
                parent = VisualTreeHelper.GetParent(parent);
            }
            Debug.Assert(child is UIElement);
            return (UIElement)child;
        }
    }
}
