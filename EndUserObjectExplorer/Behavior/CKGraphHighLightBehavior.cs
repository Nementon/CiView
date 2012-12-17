using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;
using EndUserObjectExplorer.Control;
using GraphSharp.Controls;

namespace EndUserObjectExplorer.Behavior
{
    public class CKGraphHighLightBehavior : Behavior<CKGraphLayout>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MouseLeave += new System.Windows.Input.MouseEventHandler(AssociatedObject_MouseLeave);
            AssociatedObject.MouseMove += new System.Windows.Input.MouseEventHandler(AssociatedObject_MouseMove);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.MouseLeave -= new System.Windows.Input.MouseEventHandler(AssociatedObject_MouseLeave);
            AssociatedObject.MouseMove -= new System.Windows.Input.MouseEventHandler(AssociatedObject_MouseMove);
        }

        object last_source = null;
        void AssociatedObject_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.Source != last_source)
            {
                last_source = e.Source;

                if (e.Source is EdgeControl) HighlightEdge(e.Source as EdgeControl);
                else if (e.Source is VertexControl) HighlightVertex(e.Source as VertexControl);
                else ClearHighlights();
            }
        }

        void AssociatedObject_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ClearHighlights();
            last_source = null;
        }

        private void ClearHighlights()
        {
            foreach (var uie in AssociatedObject.Children) VisualStateManager.GoToState((FrameworkElement)uie, "HighlightNormal", true);
        }

        private void HighlightEdge(EdgeControl ec)
        {
            ClearHighlights();
            VisualStateManager.GoToState(ec, "Highlighted", true);
            VisualStateManager.GoToState(ec.Source, "HighlightedSource", true);
            VisualStateManager.GoToState(ec.Target, "HighlightedTarget", true);
        }

        private void HighlightVertex(VertexControl vc)
        {
            ClearHighlights();
            foreach (var child in AssociatedObject.Children)
            {
                var ec = child as EdgeControl;
                if (ec != null)
                {
                    if (ec.Source == vc)
                    {
                        VisualStateManager.GoToState(ec, "HighlightedTarget", true);
                        VisualStateManager.GoToState(ec.Target, "HighlightedTarget", true);
                    }
                    else if (ec.Target == vc)
                    {
                        VisualStateManager.GoToState(ec, "HighlightedSource", true);
                        VisualStateManager.GoToState(ec.Source, "HighlightedSource", true);
                    }
                }
            }
            VisualStateManager.GoToState(vc, "Highlighted", true);
        }
    }
}
