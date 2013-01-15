using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EndObjectExplorer.Model;
using QuickGraph;

namespace EndObjectExplorer.Model
{
    public class ServiceGeneralisationEdge : Edge<IVertex>
    {
        public ServiceGeneralisationEdge(IVertex source, IVertex target)
            : base(source, target)
        { }
    }
}
