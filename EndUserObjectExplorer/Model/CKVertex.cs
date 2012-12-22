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

    public abstract class CKVertex : IVertex
    {
        #region Fields
        #endregion

        #region Properties
        public Boolean IsRunning { get; set; }
        public CKVertexType VertexType { get; protected set; }
        public CKGraph OwnerGraph { get; set; }
        #endregion

        public CKVertex() { }

        public virtual String Infos() { return ""; }
    }
}
