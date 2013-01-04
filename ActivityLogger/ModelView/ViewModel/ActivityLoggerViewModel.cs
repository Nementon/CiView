using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ActivityLogger.SewerModel;
using ActivityLogger.Sink;
using CK.Core;
using System.Collections.ObjectModel;

namespace ModelView.ViewModel
{
    public class ActivityLoggerViewModel
    {
        public ObservableCollection<string> _lb;
        public ObservableCollection<string> Lb { get { return _lb; } }
        public PaginatedObservableCollection<LineItem> obs = new PaginatedObservableCollection<LineItem>(10);
        public ObservableCollection<string> _ib;
        public ObservableCollection<string> Ib {get{return _ib;}}
        public bool LogLevelInfoIsChecked = true;
        public bool LogLevelTaceIsChecked = true;
        public bool LogLevelWarnIsChecked = true;  
        public bool LogLevelErrorIsChecked = false;        
        public bool LogLevelFatalIsChecked = false;

        public ActivityLoggerViewModel(BagItems bag)
        {
            _lb = new ObservableCollection<string>();
            IDefaultActivityLogger logger = DefaultActivityLogger.Create();
            //BagItems bag = new BagItems();

            ActivityLoggerLineItemSink log = new ActivityLoggerLineItemSink(bag);


            bag.ChildInserted += addLogToObs;
            logger.Register(log);

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
                    logger.Error("Erreur");
                }
                logger.Warn("warntest");
            }
        }

        private void addLogToObs(LineItem sender, EventArgs e)
        {
            obs.Add(sender);
            addLogToLb(sender);            
        }

        private void addLogToLb(LineItem sender)
        {
            CheckCollapseFilter(sender);
            if (sender.IsCollapsed)
            {

                if ((sender.Previous != null) && !sender.Previous.IsCollapsed)
                {
                    Lb.Add("[...]");
                }
            }
            else
            {
                Lb.Add(formatLog(sender));
            }
        }

        private void CheckCollapseFilter(LineItem sender)
        {
            if ((LogLevelTaceIsChecked && (sender.LogType.Equals(LogLevel.Trace)))||
                LogLevelInfoIsChecked && (sender.LogType.Equals(LogLevel.Info)) ||
                LogLevelErrorIsChecked && (sender.LogType.Equals(LogLevel.Error))||
                LogLevelWarnIsChecked && (sender.LogType.Equals(LogLevel.Warn))||
                LogLevelFatalIsChecked && (sender.LogType.Equals(LogLevel.Fatal)))
            {
                sender.IsCollapsed = false;
            }
            else
            {
                sender.IsCollapsed = true;
            }
        }

        private string formatLog(LineItem sender)
        {
            String log = "";
            int depth = sender.Depth; 

            while (depth != 0)
            {
                log = log + "   ";
                depth--;
            }
            if (sender.IsCollapsed)
            {
                log = log + "+ ";
            }
            log = log + sender.Content;

            return log;
        }

        public void changeCollapse(LineItem sender)
        {
            if (sender.IsCollapsed)
            {
                sender.IsCollapsed = false;
            }
            else
            {
                sender.IsCollapsed = true;
            }
        }

        public void CollapseAllLogs()
        {
            Lb.Clear();
            foreach (LineItem i in obs)
            {
                i.IsCollapsed = true;
                addLogToLb(i);
            }
        }

        public void UncollapseAllLogs()
        {
            Lb.Clear();
            foreach (LineItem i in obs)
            {
                i.IsCollapsed = false;
                addLogToLb(i);
            }
        }
        
        public void addInfoLogToInfoBox(LineItem sender)
        {
            Ib.Add("Content : " + sender.Content);
            Ib.Add("Deph : " + sender.Depth);
            Ib.Add("Parent : " + sender.Parent.Content);
            Ib.Add("Children : " + sender.FirstChild.Content);
        }

    }
}
