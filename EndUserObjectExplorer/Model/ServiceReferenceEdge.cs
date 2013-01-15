using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CK.Plugin;
using EndObjectExplorer.Model;
using QuickGraph;

namespace EndObjectExplorer.Model
{
    public class ServiceReferenceEdge : Edge<IVertex>
    {
        #region Fields
        RunningRequirement _requirement;
        #endregion

        #region Properties
        public RunningRequirement Requirement
        {
            get { return _requirement; }
        }
        #endregion

        public ServiceReferenceEdge(IVertex source, IVertex target, RunningRequirement rqmnt)
            : base(source, target)
        {
            _requirement = rqmnt;
        }
    }
}
