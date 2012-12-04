using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CK.Plugin;
using EndUserObjectExplorer.Model;
using QuickGraph;

namespace EndObjectExplorer.Model
{
    public class Service
    {
        IServiceInfo Self { get; set; }
        List<IPluginInfo> ImplementationPlugin { get; set; }
        public Service(IServiceInfo s)
        {
            Self = s;
            ImplementationPlugin = new List<IPluginInfo>();
        }
    }

    public class CKGraph : BidirectionalGraph<IVertex, CKEdge> //TODO UnBidirectionalGraph<CKVertex, IEdge<CKVertex>>
    {
        #region Fields
        CKGraphHost _host;
        private IServiceInfo _serviceInfo;
        private Dictionary<ServiceVertex, List<PluginVertex>> _three;
        #endregion

        #region Properties
        public CKGraphHost Host { get { return _host; } }
        public String RootServiceName { get; protected set; }
        public Dictionary<ServiceVertex, List<PluginVertex>> Three { get { return _three; } }
        public List<Service> T { get; set; }
        
        #endregion

        public CKGraph()
        {
            InitialiseGraph();
            T = new List<Service>();
            
        }

        public CKGraph(IServiceInfo serviceInfo, CKGraphHost host)
        {
            _host = host;
            _serviceInfo = serviceInfo;
            InitialiseGraph();
        }

        private CKGraph(CKGraphHost host)
        {
            InitialiseGraph();
            _host = host;
        }

        protected virtual void InitialiseGraph()
        {

            if (_serviceInfo != null)
            {
                RootServiceName = _serviceInfo.ServiceFullName;
                buildThree(_serviceInfo, 0);
            }
            else
            {
              /*  CKVertex[] vertices = new CKVertex[5];
                for (int i = 0; i < 5; ++i)
                {
                    //vertices[i] = new CKVertex("Salut");
                    AddVertex(vertices[i]);
                }

                AddEdge(new CKEdge(vertices[0], vertices[1]));
                AddEdge(new CKEdge(vertices[0], vertices[2]));
                AddEdge(new CKEdge(vertices[2], vertices[3]));
                AddEdge(new CKEdge(vertices[3], vertices[4]));
                RootServiceName = "Unknowed";
            
               */ }
        }

        // TODO Use Dictionary<CVertex(Service), CKVertex(Plugin)>
        private IVertex buildThree(IServiceInfo service, int deph)
        {
           /* ServiceVertex generalizationService = null;
            ServiceVertex currentService = new ServiceVertex(this, service);

            if (service.Generalization != null)
            {
                generalizationService = buildThree(service.Generalization, ++deph);
            }

            if (generalizationService != null)
            {
                AddVertex(generalizationService);
                AddVertex(currentService);
    
                AddEdge(new CKEdge(currentService, generalizationService));
            }
            else
            {
                AddVertex(currentService);
            }

            // Finnaly add plugin implementation
            foreach (IPluginInfo implementation in _serviceInfo.Implementations)
            {
                CKVertex pluginImplementation = new CKVertex(String.Format("Plugin : {0}", implementation.PluginFullName));
                AddVertex(pluginImplementation);
                AddEdge(new CKEdge(currentService, pluginImplementation));
            }*/

            return new ServiceVertex(service); // currentService;
        }
    }
}
