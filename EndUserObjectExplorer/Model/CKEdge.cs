using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CK.Plugin;
using QuickGraph;

namespace EndObjectExplorer.Model
{
    public class CKEdge : Edge<IVertex>
    {
        #region Fields
        RunningRequirement _requirements;
        #endregion

        #region Properties
        public RunningRequirement Requirements
        {
            get { return _requirements; }
            set { _requirements = value; }
        }
        #endregion

        public CKEdge(IVertex source, IVertex target) : base(source, target)
        {
        }
    }
}
