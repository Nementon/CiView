using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ActivityLogger.SewerModel;
using ActivityLogger.Sink;
using NUnit.Framework;
using CK.Core;

namespace ActivityLogger.Tests
{
    [TestFixture]
    class ActivityLoggerLineItemSinkTests
    {
        IDefaultActivityLogger logger = DefaultActivityLogger.Create();
        BagItems bag = new BagItems();

        [Test]
        public void ActivityLoggerSinkRegistred()
        {
            ActivityLoggerLineItemSink log = new ActivityLoggerLineItemSink(bag);
            logger.Register(log);

            Assert.That(logger.RegisteredSinks.Count, Is.EqualTo(1));
        }

        [Test]
        public void OpenGroupTest()
        {
            using (logger.OpenGroup(LogLevel.Trace, () => "EndMainGroup", "MainGroup"))
            {
                Console.WriteLine(bag.FirstChild.Content);
                Assert.That(bag.FirstChild.Content == "MainGroup");
                Assert.That(bag.FirstChild.Depth == 0);
                Assert.That(bag.FirstChild.LogType == LogLevel.Trace);


                logger.Trace("First");

                Console.WriteLine(bag.FirstChild.FirstChild.Content);
                Assert.That(bag.FirstChild.FirstChild.Content == "First");
                Assert.That(bag.FirstChild.FirstChild.Depth == 1);
                Assert.That(bag.FirstChild.FirstChild.LogType == LogLevel.Trace);

                logger.Trace("Second");

                Console.WriteLine(bag.FirstChild.FirstChild.Next.Content);
                Assert.That(bag.FirstChild.FirstChild.Next.Content == "Second");
                Assert.That(bag.FirstChild.FirstChild.Next.Depth == 1);
                Assert.That(bag.FirstChild.FirstChild.Next.LogType == LogLevel.Trace);

                logger.Trace("Third");

                Console.WriteLine(bag.FirstChild.FirstChild.Next.Next.Content);
                Assert.That(bag.FirstChild.FirstChild.Next.Next.Content == "Third");
                Assert.That(bag.FirstChild.FirstChild.Next.Next.Depth == 1);
                Assert.That(bag.FirstChild.FirstChild.Next.Next.LogType == LogLevel.Trace);

                logger.Info("last");

                Console.WriteLine(bag.FirstChild.FirstChild.Next.Next.Next.Content);
                Assert.That(bag.FirstChild.FirstChild.Next.Next.Next.Content == "last");
                Assert.That(bag.FirstChild.FirstChild.Next.Next.Next.Depth == 1);
                Assert.That(bag.FirstChild.FirstChild.Next.Next.Next.LogType == LogLevel.Info);

                    using (logger.OpenGroup(LogLevel.Info, () => "EndInfoGroup", "InfoGroup"))
                    {
                        Console.WriteLine(bag.FirstChild.LastChild.Content);
                        Assert.That(bag.FirstChild.LastChild.Content == "InfoGroup");
                        Assert.That(bag.FirstChild.LastChild.Depth == 1);
                        Assert.That(bag.FirstChild.LastChild.LogType == LogLevel.Info);

                        logger.Info("Second");

                        Console.WriteLine(bag.FirstChild.LastChild.FirstChild.Content);
                        Assert.That(bag.FirstChild.LastChild.FirstChild.Content == "Second");
                        Assert.That(bag.FirstChild.LastChild.FirstChild.Depth == 2);
                        Assert.That(bag.FirstChild.LastChild.FirstChild.LogType == LogLevel.Info);

                        logger.Trace("Fourth");

                        Console.WriteLine(bag.FirstChild.LastChild.FirstChild.Next.Content);
                        Assert.That(bag.FirstChild.LastChild.FirstChild.Next.Content == "Fourth");
                        Assert.That(bag.FirstChild.LastChild.FirstChild.Next.Depth == 2);
                        Assert.That(bag.FirstChild.LastChild.FirstChild.Next.LogType == LogLevel.Trace);


                        using (logger.OpenGroup(LogLevel.Warn, () => "EndWarnGroup", "WarnGroup"))
                       {
                           Console.WriteLine(bag.FirstChild.LastChild.LastChild.Content);
                           Assert.That(bag.FirstChild.LastChild.LastChild.Content == "WarnGroup");
                           Assert.That(bag.FirstChild.LastChild.LastChild.Depth == 2);
                           Assert.That(bag.FirstChild.LastChild.LastChild.LogType == LogLevel.Warn);

                           logger.Info("Warn!");

                           Console.WriteLine(bag.FirstChild.LastChild.LastChild.FirstChild.Content);
                           Assert.That(bag.FirstChild.LastChild.LastChild.FirstChild.Content == "Warn!");
                           Assert.That(bag.FirstChild.LastChild.LastChild.FirstChild.Depth == 3);
                           Assert.That(bag.FirstChild.LastChild.LastChild.FirstChild.LogType == LogLevel.Info);
                       }
                   }
            }
        }
    }
}
