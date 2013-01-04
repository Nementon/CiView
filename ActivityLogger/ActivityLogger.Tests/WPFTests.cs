using System;
using NUnit.Framework;
using ActivityLogger.SewerModel;
using ActivityLogger.Sink;
using CK.Core;
using TestWPF;

namespace ActivityLogger.Tests
{

    [TestFixture]
    public class WPFTests
    {
        IDefaultActivityLogger logger = DefaultActivityLogger.Create();
        BagItems bag = new BagItems();

        public void ActivityLoggerSinkRegistred()
        {
            ActivityLoggerLineItemSink log = new ActivityLoggerLineItemSink(bag);
            logger.Register(log);
        }

        [Test]
        public void PaginatedObservableCollectionTest()
        {
            using (logger.OpenGroup(LogLevel.Trace, () => "EndMainGroup", "MainGroup"))
            {
                logger.Trace("First");
                logger.Trace("Second");
                logger.Trace("Third");
                logger.Info("last");
                using (logger.OpenGroup(LogLevel.Info, () => "EndInfoGroup", "InfoGroup"))
                {
                    logger.Info("Second");
                    logger.Trace("Fourth");
                    using (logger.OpenGroup(LogLevel.Warn, () => "EndWarnGroup", "WarnGroup"))
                    {
                        logger.Info("Warn!");
                    }
                }

            PaginatedObservableCollection<String> obs = new PaginatedObservableCollection<String>(10);
            obs.Add(bag.FirstChild.ToString());
            obs.Add(bag.FirstChild.Next.ToString());


            }

            

        }
    }
}
