using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CK.Context;
using CK.Plugin.Config;
using CK.Plugin.Hosting;
using CK.Core;
using CK.Plugin;
using CK.Plugin.Discoverer;
using System.IO;
using System.Collections.ObjectModel;
using System.Security;
using System.Runtime.InteropServices;
using System.Diagnostics;
using QuickGraph;

namespace EndObjectExplorer.Model
{

    [SuppressUnmanagedCodeSecurity]
    public static class ConsoleManager
    {
        private const string Kernel32_DllName = "kernel32.dll";

        [DllImport(Kernel32_DllName)]
        private static extern bool AllocConsole();

        [DllImport(Kernel32_DllName)]
        private static extern bool FreeConsole();

        [DllImport(Kernel32_DllName)]
        private static extern IntPtr GetConsoleWindow();

        [DllImport(Kernel32_DllName)]
        private static extern int GetConsoleOutputCP();

        public static bool HasConsole
        {
            get { return GetConsoleWindow() != IntPtr.Zero; }
        }

        /// <summary>
        /// Creates a new console instance if the process is not attached to a console already.
        /// </summary>
        public static void Show()
        {
            //#if DEBUG
            if (!HasConsole)
            {
                AllocConsole();
                InvalidateOutAndError();
            }
            //#endif
        }

        /// <summary>
        /// If the process has a console attached to it, it will be detached and no longer visible. Writing to the System.Console is still possible, but no output will be shown.
        /// </summary>
        public static void Hide()
        {
            //#if DEBUG
            if (HasConsole)
            {
                SetOutAndErrorNull();
                FreeConsole();
            }
            //#endif
        }

        public static void Toggle()
        {
            if (HasConsole)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }

        static void InvalidateOutAndError()
        {
            Type type = typeof(System.Console);

            System.Reflection.FieldInfo _out = type.GetField("_out",
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            System.Reflection.FieldInfo _error = type.GetField("_error",
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            System.Reflection.MethodInfo _InitializeStdOutError = type.GetMethod("InitializeStdOutError",
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            Debug.Assert(_out != null);
            Debug.Assert(_error != null);

            Debug.Assert(_InitializeStdOutError != null);

            _out.SetValue(null, null);
            _error.SetValue(null, null);

            _InitializeStdOutError.Invoke(null, new object[] { true });
        }

        static void SetOutAndErrorNull()
        {
            Console.SetOut(TextWriter.Null);
            Console.SetError(TextWriter.Null);
        }
    }

    #region Debug TMP
    public class ILogService { }
    #endregion

    public class CKGraphHost
    {

        #region Fields
        private PluginRunner _pluginRunner;
        private List<CKGraph> _graphs;
        public Dictionary<IServiceInfo, ServiceVertex> _serviceVerticesDic;
        #endregion

        #region Properties
        public PluginRunner PluginRunner { get { return _pluginRunner; } }
        public ObservableCollection<CKGraph> Graphs { get; private set; }
        public IContext Context { get; private set; }
        public IPluginConfigAccessor Config { get; private set; }
        public ILogService LogService { get; private set; }
        public CKGraph Graph { get; private set; }
        #endregion

        public CKGraphHost(String ckAssemblieFolder /*IContext context, IPluginConfigAccessor config, ILogService logService*/)
        {
            //TODO
            //Contract.Ensures(context != null);
            //Contract.Ensures(congig != null);
            //Contract.Ensures(LogService != null);
            //
            //Context = context;
            //Config = config;
            //LogService = logService;
            //_pluginRunner = Context.GetService<PluginRunner>(true);

            Graphs = new ObservableCollection<CKGraph>();
            Graph = new CKGraph(
                BuildServicesThree(ckAssemblieFolder)
                );  
        }

        private BidirectionAdapterGraph<IVertex, CKEdge> BuildServicesThree(String ckAssemblieFolder)
        {
            
            PluginDiscoverer discoverer = GetFooBarPluginDiscoverer(ckAssemblieFolder);
            Dictionary<IServiceInfo, ServiceVertex> servicesVerticesDic = new Dictionary<IServiceInfo, ServiceVertex>();
            var serviceThree = new AdjacencyGraph<IVertex, CKEdge>();

            foreach (var service in discoverer.AllServices)
            {
                servicesVerticesDic.Add(service, new ServiceVertex(service));
            }

            foreach (var service in servicesVerticesDic.Keys)
            {
                if (service.Generalization != null)
                {
                    serviceThree.AddVerticesAndEdge(
                           new CKEdge(
                               servicesVerticesDic[service], 
                               servicesVerticesDic[service.Generalization]
                            )
                        );
                }
                else
                {
                    serviceThree.AddVertex(servicesVerticesDic[service]);
                }
            }

           return new BidirectionAdapterGraph<IVertex, CKEdge> (serviceThree);
        }

        #region Internal Helper
        public PluginDiscoverer GetFooBarPluginDiscoverer(String ckAssemblieFolder)
        {
            PluginDiscoverer discoverer = new PluginDiscoverer();
            bool discoverDone = false;

            discoverer.DiscoverDone += (object sender, DiscoverDoneEventArgs e) => discoverDone = true;

            ckAssemblieFolder = ckAssemblieFolder.Replace('|', System.IO.Path.DirectorySeparatorChar);
            discoverer.Discover(new DirectoryInfo(ckAssemblieFolder), true);

            Contract.Assert(discoverDone);
            return discoverer;
        }
        #endregion
    }
}
