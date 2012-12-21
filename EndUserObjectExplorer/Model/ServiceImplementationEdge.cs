using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EndObjectExplorer.Model;
using QuickGraph;

namespace EndUserObjectExplorer.Model
{
    public class ServiceImplementationEdge : Edge<IVertex>
    {
        public ServiceImplementationEdge(IVertex source, IVertex target)
            : base(source, target)
        {
        }
    }
}
