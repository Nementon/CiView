using System;
using CiView.SewerModel;
using CK.Core;
using NUnit.Framework;

namespace ActivityLogger.Tests
{
    [TestFixture]
    public class SewerModelTest
    {
        private BagItems CreateMockBag()
        {
            BagItems bag = new BagItems();
            LineItem root = new LineItem("Root", LogLevel.Trace, bag);
            LineItem rootChild = new LineItem("FirstRootChild", LogLevel.Trace, bag);
            LineItem rootChildChild = new LineItem("RootChildFirstChild", LogLevel.Error, bag);
            return bag;
        }

        [Test]
        public void InsertBagItemsTest()
        {
            BagItems bag = new BagItems();
            LineItem root = new LineItem("Root", LogLevel.Trace, bag);
            LineItem rootChild = new LineItem("rootChild", LogLevel.Trace, bag);
            LineItem rootChildChild = new LineItem("RootChildFirstChild", LogLevel.Error, bag);

            bag.ChildInserted += (LineItem item, EventArgs e) => Console.WriteLine(item);

            bag.InsterRootItem(root);
            bag.InsterRootItem(rootChild);
            bag.InsterRootItem(rootChildChild);

            Assert.That(bag.FirstChild == root);
            Assert.That(bag.FirstChild.Next == rootChild);
            Assert.That(bag.LastChild == rootChildChild);
        
            Assert.That(rootChildChild.Previous == rootChild );
            Assert.That(rootChild.Previous == root);
            Assert.That(root.Previous == null);

        }

        [Test]
        public void InsertItemsTest()
        {
            BagItems bag = new BagItems();
            LineItem root = new LineItem("Root", LogLevel.Trace, bag);
            LineItem rootChild = new LineItem("rootChild", LogLevel.Trace, bag);
            LineItem rootChildChild = new LineItem("RootChildFirstChild", LogLevel.Error, bag);

            bag.ChildInserted += (LineItem item, EventArgs e) => Console.WriteLine(item);

            bag.InsterRootItem(root);
            root.InsertChild(rootChild);
            rootChild.InsertChild(rootChildChild);

            Assert.That(bag.FirstChild == root);
            Assert.That(bag.LastChild == root);
            Assert.That(root.FirstChild == rootChild);
            Assert.That(rootChild.FirstChild == rootChildChild);

            Assert.That(rootChildChild.Parent == rootChild);
            Assert.That(rootChild.Parent == root);
            Assert.That(root.Parent == null);

        }

        [Test]
        public void DeleteTest()
        {
            //Family
            BagItems bag = new BagItems();
            LineItem root = new LineItem("Root", LogLevel.Trace, bag);
            LineItem rootChild = new LineItem("rootChild", LogLevel.Trace, bag);
            LineItem rootChildChild = new LineItem("RootChildFirstChild", LogLevel.Error, bag);
            
            //Alice Family
            LineItem rootChildAlice = new LineItem("Alice", LogLevel.Trace, bag);
            LineItem rootChildAliceChildChild = new LineItem("Fils d'Alice child child", LogLevel.Trace, bag);
            LineItem rootChildAliceChild1 = new LineItem("Fille d'Alice 1", LogLevel.Trace, bag);
            LineItem rootChildAliceChild2 = new LineItem("Fils d'Alice 2", LogLevel.Trace, bag);
            LineItem rootChildAliceChild3 = new LineItem("Fille2 d'Alice 3", LogLevel.Trace, bag);
            //Bob Family
            LineItem rootChildBob = new LineItem("Bob", LogLevel.Trace, bag);

            bag.InsterRootItem(root);
            root.InsertChild(rootChild);
            rootChild.InsertChild(rootChildChild);
            root.InsertChild(rootChildAlice);
            root.InsertChild(rootChildBob);
            //Alice Familly
            rootChildAlice.InsertChild(rootChildAliceChild1);
            rootChildAlice.InsertChild(rootChildAliceChild2);
            rootChildAlice.InsertChild(rootChildAliceChild3);
            rootChildAliceChild2.InsertChild(rootChildAliceChildChild);

            #region oldies
            //  bag.ItemDeleted += (LineItem sender, EventArgs e) => Assert.That(sender == rootChildAlice);
           // root.Delete();
           /** rootChildAlice.Delete();
            IsDeleted(rootChildAliceChild1);
            IsDeleted(rootChildAliceChild2);
            IsDeleted(rootChildAliceChild3);
            IsDeleted(rootChildAliceChildChild);
       
         Assert.That(rootChild.Next == rootChildBob);
         Assert.That(rootChildBob.Previous == rootChild);
           IsDeleted(root);
            IsDeleted(rootChild);
            IsDeleted(rootChildChild);
            IsDeleted(rootChildAlice);
            IsDeleted(rootChildBob);
          */
#endregion
            Console.WriteLine(root.FirstChild);
            rootChild.Delete();
            Console.WriteLine(root.FirstChild);
            Console.WriteLine(root.LastChild);
            Assert.That(root.FirstChild==rootChildAlice);
            Assert.That(rootChildAlice.Previous==null);
            Assert.That(rootChildBob== root.LastChild);

           
        }

        public void IsDeleted(LineItem item)
        {
            Assert.That(item.Parent == null);
            Assert.That(item.Previous == null);
            Assert.That(item.Next == null);
            Assert.That(item.NextSiblingError == null);
            Assert.That(item.NextError == null);
            Assert.That(item.LastChild == null);
            Assert.That(item.FirstChild == null);
        }

        [Test]
        public void CollaspeTest()
        {
            //Family
            BagItems bag = new BagItems();
            LineItem root = new LineItem("Root", LogLevel.Trace, bag);
            LineItem rootChild = new LineItem("rootChild", LogLevel.Trace, bag);
            LineItem rootChildChild = new LineItem("RootChildFirstChild", LogLevel.Error, bag);

            bag.InsterRootItem(root);
            root.InsertChild(rootChild);
            rootChild.InsertChild(rootChildChild);

            bag.ItemCollapsed += (LineItem sender, EventArgs e) => Assert.That(sender == root);
            root.IsCollapsed = true;

            Assert.IsTrue(bag.Height == 0);
            Assert.IsTrue(root.NodeHeight == 0);
            Assert.IsTrue(root.IsCollapsed);
            Assert.IsTrue(rootChild.NodeHeight == 0);
            Assert.IsTrue(rootChild.IsCollapsed);
            Assert.IsTrue(rootChildChild.NodeHeight == 0);
            Assert.IsTrue(rootChildChild.IsCollapsed);
        }
 

        [Test]
        public void UnCollaspeTest()
        {
            //Family
            BagItems bag = new BagItems();
            LineItem root = new LineItem("Root", LogLevel.Trace, bag);
            LineItem rootChild = new LineItem("rootChild", LogLevel.Trace, bag);
            LineItem rootChildChild = new LineItem("RootChildFirstChild", LogLevel.Error, bag);

            bag.InsterRootItem(root);
            root.InsertChild(rootChild);
            rootChild.InsertChild(rootChildChild);
            double rootFirstChildNodeHight = root.FirstChild.ItemHeight;
            double rootNodeHight = root.ItemHeight;
            bag.ItemUnCollapsed += (LineItem sender, EventArgs e) => Assert.That(sender == root);
            root.IsCollapsed = true;
            root.IsCollapsed = false;

            Assert.That(root.NodeHeight == rootFirstChildNodeHight + rootNodeHight, String.Format("{0} {1} {2}", root.NodeHeight, rootFirstChildNodeHight, rootNodeHight));

        }
 
        [Test]
        public void TestHeight()
        {
            BagItems bag = new BagItems();
            LineItem root = new LineItem("Root", LogLevel.Trace, bag);
            LineItem rootChild = new LineItem("rootChild", LogLevel.Trace, bag);
            LineItem rootChildChild = new LineItem("RootChildFirstChild", LogLevel.Error, bag);
           
            bag.InsterRootItem(root);
            bag.InsterRootItem(rootChild);
            bag.InsterRootItem(rootChildChild);

            Assert.IsTrue((bag.Height == 2), String.Format("Bag height is {0}", bag.Height));

            bag.FirstChild.IsCollapsed = true;
            Assert.IsTrue((bag.Height == 0), String.Format("Bag height is {0}", bag.Height));

            bag.FirstChild.IsCollapsed = false;
            Assert.IsTrue((bag.Height == 2), String.Format("Bag height is {0}", bag.Height));
     }
    }
}
