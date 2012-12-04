using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CK.Plugin;
using EndObjectExplorer.Model;

namespace EndUserObjectExplorer.Model
{
    public class PluginVertex : CKVertex
    {
        IPluginInfo _pluginInfo;

        public PluginVertex(IPluginInfo pluginInfo)
        {
            _pluginInfo = pluginInfo;
            Type = CKVertexType.Plugin;
            Name = _pluginInfo.PluginFullName;
        }
    }
}
