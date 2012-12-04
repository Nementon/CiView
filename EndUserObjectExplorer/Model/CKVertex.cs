using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CK.Plugin;

namespace EndObjectExplorer.Model
{
    public enum CKVertexType
    {
       Service = 0,
       Plugin = 2
    }

    public abstract class CKVertex : IVertex //<TInfo> where TInfo : IDiscoveredInfo
    {

        #region Properties
        public Boolean IsRunning { get; set; }
        public CKGraph Owner { get; private set; }
        public CKVertexType Type { get; protected set; }
        public String Name { get; protected set; }
        #endregion

        public CKVertex() { }

        public virtual String Infos() { return ""; }
    }
}
