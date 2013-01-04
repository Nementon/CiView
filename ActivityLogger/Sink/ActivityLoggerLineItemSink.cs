using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CK.Core;
using ActivityLogger.SewerModel;

namespace ActivityLogger.Sink
{
    public class ActivityLoggerLineItemSink : IActivityLoggerSink
    {
        public LineItem currentLineItem;
        private BagItems bag;

        public ActivityLoggerLineItemSink(BagItems bagItem)
        {
            this.bag = bagItem;
            currentLineItem = null;
        }

        void IActivityLoggerSink.OnEnterLevel(LogLevel level, string text)
        {
            LineItem lineItem = new LineItem(text, level, bag);
            currentLineItem.InsertChild(lineItem);
        }

        void IActivityLoggerSink.OnContinueOnSameLevel(LogLevel level, string text)
        {
            LineItem lineItem = new LineItem(text, level, bag, true);
            currentLineItem.InsertChild(lineItem);
        }

        void IActivityLoggerSink.OnLeaveLevel(LogLevel level)
        {
            //LineItem lineItem = new LineItem("leavelvl", level, bag);

        }

        void IActivityLoggerSink.OnGroupOpen(IActivityLogGroup g)
        {
            LineItem lineItem = new LineItem(g.GroupText, g.GroupLevel, bag);
            if (currentLineItem == null)
            {
                bag.InsterRootItem(lineItem);
            }
            else
            {
                currentLineItem.InsertChild(lineItem);
            }
            currentLineItem = lineItem;
        }

        void IActivityLoggerSink.OnGroupClose(IActivityLogGroup g, IReadOnlyList<ActivityLogGroupConclusion> conclusions)
        {
            currentLineItem = currentLineItem.Parent;
        }



    }
}
