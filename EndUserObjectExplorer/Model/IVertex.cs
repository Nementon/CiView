using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EndObjectExplorer.Model
{
    public interface IVertex
    {
         Boolean IsRunning { get; set; }
         CKVertexType Type { get;  }
         String Name { get; }
         String Infos();
    }
}
