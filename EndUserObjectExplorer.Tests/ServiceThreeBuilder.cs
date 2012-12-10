using System;
using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;
using QuickGraph;
using EndObjectExplorer.Model;

namespace EndUserObjectExplorer.Tests
{
    public class MockIServiceInfo
    {
        private MockIServiceInfo _generalisation = null;
        public MockIServiceInfo Generalisation { get { return _generalisation; } set { _generalisation = value; } }
        public String Name { get; set; }

        public MockIServiceInfo(String name) : this(name, null) { }
        public MockIServiceInfo(String name, MockIServiceInfo generalisation)
        {
            Name = name;
            _generalisation = generalisation;
        }

        public override String ToString()
        {
            return Name;
        }
    }

    [TestFixture]
    public class ServiceThreeBuilder
    {
        [Test]
        public void buildMockServicesThree()
        {
            /*
             *                      zero
             *              unDroite__|__unGauche 
             *            _____|______   ______|______
             *           |            | |             |
             *       deuxDroite  deuxG troisD       troisG
             * 
             * */
            List<MockIServiceInfo> allServices = new List<MockIServiceInfo>();
            MockIServiceInfo zero = new MockIServiceInfo("zero");
            MockIServiceInfo unDroite = new MockIServiceInfo("unDroite", zero);
            MockIServiceInfo deuxDroite = new MockIServiceInfo("deuxDroite", unDroite);
            MockIServiceInfo deuxGauche = new MockIServiceInfo("deuxGauche", unDroite);
            MockIServiceInfo unGauche = new MockIServiceInfo("unGauche", zero);
            MockIServiceInfo troisDroite = new MockIServiceInfo("troisDroite", unGauche);
            MockIServiceInfo troisGauche = new MockIServiceInfo("troisGauche", unGauche);

            allServices.Add(unDroite);
            allServices.Add(deuxGauche);
            allServices.Add(troisDroite);
            allServices.Add(troisGauche);
            allServices.Add(unGauche);
            allServices.Add(deuxDroite);
            allServices.Add(zero);

            var mockServicesThree = new AdjacencyGraph<MockIServiceInfo, TaggedEdge<MockIServiceInfo, string>>();

            mockServicesThree.VertexAdded += mockServiceThree_VertexAdded;
            mockServicesThree.VertexRemoved += mockServiceThree_VertexRemoved;
            mockServicesThree.EdgeAdded += mockServiceThree_EdgeAdded;
            mockServicesThree.EdgeRemoved += mockServiceThree_EdgeRemoved;

            foreach(MockIServiceInfo service in allServices)
            {
                if (service.Generalisation != null)
                {
                    mockServicesThree.AddVerticesAndEdge(
                        new TaggedEdge<MockIServiceInfo, String>(service, service.Generalisation, service.Name)
                        );
                }
            }

            TaggedEdge<MockIServiceInfo, string> tEdge;
            Assert.IsTrue(mockServicesThree.TryGetEdge(unDroite, zero, out tEdge));
            Assert.IsFalse(mockServicesThree.TryGetEdge(zero, unDroite, out tEdge));
            Assert.IsTrue(mockServicesThree.TryGetEdge(unGauche, zero, out tEdge));
            Assert.IsFalse(mockServicesThree.TryGetEdge(zero, unGauche, out tEdge));
            Assert.IsTrue(mockServicesThree.TryGetEdge(deuxDroite, unDroite, out tEdge));
            Assert.IsFalse(mockServicesThree.TryGetEdge(unDroite, deuxDroite, out tEdge));
            Assert.IsTrue(mockServicesThree.TryGetEdge(deuxGauche, unDroite, out tEdge));
            Assert.IsFalse(mockServicesThree.TryGetEdge(unDroite, deuxGauche, out tEdge));
            Assert.IsTrue(mockServicesThree.TryGetEdge(troisDroite, unGauche, out tEdge));
            Assert.IsFalse(mockServicesThree.TryGetEdge(unGauche, troisDroite, out tEdge));
            Assert.IsTrue(mockServicesThree.TryGetEdge(troisGauche, unGauche, out tEdge));
            Assert.IsFalse(mockServicesThree.TryGetEdge(unGauche, troisGauche, out tEdge));
        }

        private void mockServiceThree_VertexAdded(MockIServiceInfo vertex)
        {
            Console.WriteLine("Vertex {0} added !", vertex.Name);
        }

        private void mockServiceThree_VertexRemoved(MockIServiceInfo vertex)
        {
            Console.WriteLine("Vertex {0} removed !", vertex.Name);
        }

        private void mockServiceThree_EdgeAdded(TaggedEdge<MockIServiceInfo, string> e)
        {
            Console.WriteLine("Edge from {0} added to {1}", e.Source.Name, e.Target.Name);
        }

        void mockServiceThree_EdgeRemoved(TaggedEdge<MockIServiceInfo, string> e)
        {
            Console.WriteLine("Edge from {0} removed to {1}", e.Source.Name, e.Target.Name);
        }
    }
}
