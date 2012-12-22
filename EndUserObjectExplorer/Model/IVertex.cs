using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EndObjectExplorer.Model
{
    public interface IVertex
    {
         CKGraph OwnerGraph { get; set; }
         Boolean IsRunning { get; set; }
         CKVertexType VertexType { get;  }
         String Infos();
    }
}
