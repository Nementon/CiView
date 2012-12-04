using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CK.Plugin;

namespace EndObjectExplorer.Model
{
    public class ServiceVertex : CKVertex
    {
        IServiceInfo _serviceInfo;
        public IServiceInfo Service { get { return _serviceInfo; } }


        public ServiceVertex(IServiceInfo serviceInfo)
        {
            _serviceInfo = serviceInfo;
            Type = CKVertexType.Service;
            Name = _serviceInfo.ServiceFullName;
        }
    }
}
