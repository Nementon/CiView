using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CK.Plugin;
using CK.Plugin.Discoverer;
using CK.Plugin.Hosting;
using EndObjectExplorer.Model;
using QuickGraph;

namespace EndObjectExplorer.Model
{
    public class CKGraph : BidirectionalGraph<IVertex, Edge<IVertex>>
    {
        #region Fields
        private List<ServiceReferenceEdge> _servicesReferencesEdges;
        private Dictionary<RunningRequirement, ServiceReferenceEdge> _requirementServiceReferencesDic;
        private Dictionary<IServiceInfo, ServiceVertex> _servicesVerticesDic;
        private Dictionary<IPluginInfo, PluginVertex> _pluginsVerticesDic;
        #endregion

        public CKGraph(string ckAssemblieFolderPath) : this(ckAssemblieFolderPath, false) { }

        public CKGraph(string ckAssemblieFolderPath, bool allowParalleEdge) 
            : base(allowParalleEdge) 
        {
            _servicesReferencesEdges = new List<ServiceReferenceEdge>();
            _requirementServiceReferencesDic = new Dictionary<RunningRequirement, ServiceReferenceEdge>();
            _servicesVerticesDic = new Dictionary<IServiceInfo, ServiceVertex>();
            _pluginsVerticesDic = new Dictionary<IPluginInfo, PluginVertex>();

            InitializeGraph(ckAssemblieFolderPath);
        }

        #region Public API
        public List<IVertex> FindLinkedVertexOf(IVertex arg)
        {
            return FindLinkedVertexOf(arg, new List<IVertex>());
        }

        public void AttachEdgesByRunningRequirement(RunningRequirement requirement)
        {
            foreach (var edge in _servicesReferencesEdges)
            {
                if (edge.Requirement == requirement)
                {
                    AddEdge(edge);
                }
            }
        }

        public void DetachEdgesByRunningRequirement(RunningRequirement requirement)
        {
            foreach (var edge in _servicesReferencesEdges)
            {
                if (edge.Requirement == requirement)
                {
                    RemoveEdge(edge);
                }
            }
        }
        #endregion

        #region Private API
        private void InitializeGraph(string ckAssemblieFolderPath)
        {
            PluginDiscoverer discoverer = GetFooBarPluginDiscoverer(ckAssemblieFolderPath);

            InitializeServicesVertices(discoverer.AllServices);
            InitializePluginsVertices(discoverer.AllPlugins);

            BuildServicesLinks();
            BuildPluginsLinks();
        }

        private void InitializePluginsVertices(CK.Core.IReadOnlyCollection<IPluginInfo> allPlugins)
        {
            foreach (var plugin in allPlugins)
            {
                var pluginVertex = new PluginVertex(plugin);
                _pluginsVerticesDic.Add(plugin, pluginVertex);
                AddVertex(pluginVertex);
            }
        }

        private void InitializeServicesVertices(CK.Core.IReadOnlyCollection<IServiceInfo> allServices)
        {
            foreach (var service in allServices)
            {
                var serviceVertex = new ServiceVertex(service);
                _servicesVerticesDic.Add(service, serviceVertex);
                AddVertex(serviceVertex);
            }
        }

        private void BuildServicesLinks()
        {
            foreach (var service in _servicesVerticesDic.Keys)
            {
                AddServiceGeneralizationEdgeFor(service);
                AddServiceImplementationEdgeFor(service);
            }
        }

        private void AddServiceGeneralizationEdgeFor(IServiceInfo service)
        {
            if (service.Generalization != null)
            {
                AddEdge(
                    new ServiceGeneralisationEdge(
                        _servicesVerticesDic[service.Generalization],
                        _servicesVerticesDic[service]
                        )
                    );
            }
        }

        private void AddServiceImplementationEdgeFor(IServiceInfo service)
        {
            foreach (var plugin in service.Implementations)
            {
                AddEdge(
                    new ServiceImplementationEdge(
                        _pluginsVerticesDic[plugin],
                        _servicesVerticesDic[service]
                        )
                    );
            }
        }

        private void BuildPluginsLinks()
        {
            foreach (var plugin in _pluginsVerticesDic.Keys)
            {
                AddPluginServiceReferencesEdgesFor(plugin);
            }
        }

        private void AddPluginServiceReferencesEdgesFor(IPluginInfo plugin)
        {
            foreach (var referencedService in plugin.ServiceReferences)
            {
                var edge = new ServiceReferenceEdge(
                     _pluginsVerticesDic[plugin],
                     _servicesVerticesDic[referencedService.Reference],
                     referencedService.Requirements
                     );
                AddEdge(edge);
                _servicesReferencesEdges.Add(edge);
            }
        }

        private List<IVertex> FindLinkedVertexOf(IVertex arg, List<IVertex> linkedVertices)
        {
            if (!IsOutEdgesEmpty(arg))
            {
                foreach (var edge in OutEdges(arg))
                {
                    if (!linkedVertices.Contains(edge.Target))
                    {
                        linkedVertices.Add(edge.Target);
                        FindLinkedVertexOf(edge.Target, linkedVertices);
                    }
                }
            }

            if (!IsInEdgesEmpty(arg))
            {
                foreach (var edge in InEdges(arg))
                {
                    if (!linkedVertices.Contains(edge.Source))
                    {
                        linkedVertices.Add(edge.Source);
                        FindLinkedVertexOf(edge.Source, linkedVertices);
                    }
                }
            }
            return linkedVertices;
        }
        #endregion

        protected override void OnVertexAdded(IVertex args)
        {
            base.OnVertexAdded(args);
            args.OwnerGraph = this;
        }

        #region Internal Helper
        PluginDiscoverer GetFooBarPluginDiscoverer(String ckAssemblieFolderPath)
        {
            PluginDiscoverer discoverer = new PluginDiscoverer();
            bool discoverDone = false;

            discoverer.DiscoverDone += (object sender, DiscoverDoneEventArgs e) => discoverDone = true;

            ckAssemblieFolderPath = ckAssemblieFolderPath.Replace('|', System.IO.Path.DirectorySeparatorChar);
            discoverer.Discover(new DirectoryInfo(ckAssemblieFolderPath), true);

            Contract.Assert(discoverDone);
            return discoverer;
        }
        #endregion
    }
}
