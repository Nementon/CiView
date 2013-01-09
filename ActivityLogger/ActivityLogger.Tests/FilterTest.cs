using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ActivityLogger.SewerModel;
using ActivityLogger.Filters;
using ActivityLogger.Sink;
using NUnit.Framework;
using CK.Core;

namespace ActivityLogger.Tests
{
    [TestFixture]
    public class FilterTest
    {
        [Test]
        public void DeleteBranches()
        {
            BagItems bag = new BagItems();
            LineItem root = new LineItem("Root", LogLevel.Trace, bag);
            LineItem rootChild = new LineItem("rootChild", LogLevel.Trace, bag);
            LineItem rootChildChild = new LineItem("RootChildFirstChild", LogLevel.Error, bag);

            bag.InsterRootItem(root);
            bag.InsterRootItem(rootChild);
            bag.InsterRootItem(rootChildChild);

            ModelFilter mf = new ModelFilter(1, bag);
            mf.DeleteBranches();
            Console.WriteLine(bag.FirstChild.ToString());

        }
    }
}
