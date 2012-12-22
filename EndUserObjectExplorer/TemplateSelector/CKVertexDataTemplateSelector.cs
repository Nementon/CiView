using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using EndObjectExplorer.Model;
using EndUserObjectExplorer.Control;

namespace EndUserObjectExplorer.TemplateSelector
{
    public class CKVertexDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ServiceTemplate { get; set; }
        public DataTemplate PluginTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            IVertex vertex = item as IVertex;
            Debug.Assert(vertex != null); if (vertex == null) return null;
            Debug.Assert(ServiceTemplate != null); Debug.Assert(PluginTemplate != null);

            if (vertex is ServiceVertex)
                return ServiceTemplate;
            else if (vertex is PluginVertex)
                return PluginTemplate;
            else
                throw new Exception("Unknow IVertex Type");
        }
    }
}
