using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using EndObjectExplorer.Behavior;
using EndObjectExplorer.Model;
using GraphSharp.Controls;

namespace EndUserObjectExplorer.Control
{
    public class CKVertexControl : VertexControl
    {
        public CKVertexControl()
        {
            RegisterBehaviors();
            IsSelected = false;
        }

        public bool IsSelected { get; set; }
        public new IVertex Vertex
        {
            get
            {
                return (IVertex)base.Vertex;
            }
            set
            {
                base.Vertex = value;
            }
        }

        private void RegisterBehaviors()
        {
            System.Windows.Interactivity.BehaviorCollection behaviors = System.Windows.Interactivity.Interaction.GetBehaviors(this);
            behaviors.Add(new CKVertexDragBehavior());
        }
    }
}
