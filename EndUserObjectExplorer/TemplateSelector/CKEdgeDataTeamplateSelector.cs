using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using EndObjectExplorer.Model;
using EndObjectExplorer.Control;
using QuickGraph;
using EndObjectExplorer.Model;
using GraphSharp.Controls;

namespace EndObjectExplorer.TemplateSelector
{
    class CKEdgeDataTeamplateSelector : DataTemplateSelector
    {
        public DataTemplate GeneralisationTemplate { get; set; }
        public DataTemplate ImplementationTemplate { get; set; }
        public DataTemplate ReferenceTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            Edge<IVertex> edge = item as Edge<IVertex>;

            Debug.Assert(edge != null); if (edge == null) return null;
            Debug.Assert(GeneralisationTemplate != null); 
            Debug.Assert(ImplementationTemplate != null); 
            Debug.Assert(ReferenceTemplate != null);

            if (edge is ServiceGeneralisationEdge)
                return GeneralisationTemplate;
            else if (edge is ServiceImplementationEdge)
                return ImplementationTemplate;
            else if (edge is ServiceReferenceEdge)
                return ReferenceTemplate;
            else
                throw new Exception("Unknow IEdge Type");
        }
    }
}
