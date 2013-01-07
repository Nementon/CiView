using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ActivityLogger.SewerModel;
using ActivityLogger.Sink;
using CK.Core;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ModelView.ViewModel
{
    public class ActivityLoggerViewModel : BaseViewModel
    {
        #region Properties
        public ObservableCollection<string> _logsList;
        public ObservableCollection<string> LogsList { get { return _logsList; } }
        public PaginatedObservableCollection<LineItem> obs = new PaginatedObservableCollection<LineItem>(10);
        public ObservableCollection<string> _logsListInfoBox;
        public ObservableCollection<string> LogsListInfoBox { get { return _logsListInfoBox; } }
        private bool _logLevelInfoIsChecked = false;
        public bool _logLevelTaceIsChecked = true;
        public bool _logLevelWarnIsChecked = false;
        public bool _logLevelErrorIsChecked = true;
        public bool _logLevelFatalIsChecked = false;
        public bool LogLevelInfoIsChecked { get { return _logLevelInfoIsChecked; } set { _logLevelInfoIsChecked = value; } }
        public bool LogLevelTaceIsChecked { get { return _logLevelTaceIsChecked; } set { _logLevelTaceIsChecked = value; } }
        public bool LogLevelWarnIsChecked { get { return _logLevelWarnIsChecked; } set { _logLevelWarnIsChecked = value; } }
        public bool LogLevelErrorIsChecked { get { return _logLevelErrorIsChecked; } set { _logLevelErrorIsChecked = value; } }
        public bool LogLevelFatalIsChecked { get { return _logLevelFatalIsChecked; } set { _logLevelFatalIsChecked = value; } }
        private ICommand reDisplayCommand;
        //private ICommand checkLogLevelsCommand;
        #endregion

       

        public ActivityLoggerViewModel(BagItems bag)
        {
            _logsList = new ObservableCollection<string>();
            _logsListInfoBox = new ObservableCollection<string>();
            IDefaultActivityLogger logger = DefaultActivityLogger.Create();

            ActivityLoggerLineItemSink log = new ActivityLoggerLineItemSink(bag);


            bag.ChildInserted += addLogToObs;
            logger.Register(log);

            using (logger.OpenGroup(LogLevel.Trace, () => "EndMainGroup", "MainGroup (trace)"))
            {
                logger.Info("last (info)");
                using (logger.OpenGroup(LogLevel.Info, () => "EndInfoGroup", "InfoGroup (info)"))
                {
                    logger.Info("Second (info)");
                    logger.Trace("Fourth (trace)");
                    using (logger.OpenGroup(LogLevel.Warn, () => "EndWarnGroup", "WarnGroup"))
                    {
                        logger.Warn("Warn! (Warn)");
                       
                    }
                    logger.Error("Erreur (Erreur)");
                }
                logger.Fatal("Fatal (Fatal)");
            }
        }

        private void addLogToObs(LineItem sender, EventArgs e)
        {

            obs.Add(sender);
            addLogToLogsList(sender);
            addInfoLogToInfoBox(sender);
        }

        private void addLogToLogsList(LineItem sender)
        {
            checkCollapseFilter(sender);
            if (sender.IsCollapsed)
            {

                if ((sender.Previous != null) && !sender.Previous.IsCollapsed)
                {
                    LogsList.Add("[...]");
                }
            }
            else
            {
                LogsList.Add(formatLog(sender));
            }
        }

        private void checkCollapseFilter(LineItem sender)
        {
            if (LogLevelTaceIsChecked && (sender.LogType.Equals(LogLevel.Trace))||
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

        public void collapseAllLogs()
        {
            LogsList.Clear();
            foreach (LineItem i in obs)
            {
                i.IsCollapsed = true;
                addLogToLogsList(i);
            }
        }

        public void uncollapseAllLogs()
        {
            LogsList.Clear();
            foreach (LineItem i in obs)
            {
                i.IsCollapsed = false;
                addLogToLogsList(i);
            }
        }
        
        public void addInfoLogToInfoBox(LineItem sender)
        {
            
            LogsListInfoBox.Add("Content : " + sender.Content);
            LogsListInfoBox.Add("Deph : " + sender.Depth);
            LogsListInfoBox.Add("logLevel : " + sender.LogType.ToString());
            if (sender.Parent != null)
            {
                LogsListInfoBox.Add("Parent : " + sender.Parent.Content);
            }
            else
            {
                LogsListInfoBox.Add("Parent : null");
            }
            //LogsListInfoBox.Add("Children : " + sender.FirstChild.Content);
        
        }

        public ICommand ReDisplayCommand
        {
            get
            {
                if (reDisplayCommand == null)
                    reDisplayCommand = new RelayCommand(() => ReDisplay(), () => CanReDisplay());

                return reDisplayCommand;
            }
        }

        private void ReDisplay()
        {
            LogsList.Clear();
            foreach (LineItem l in obs)
            {
                addLogToLogsList(l);
                addInfoLogToInfoBox(l);
            }
        }

        private bool CanReDisplay()
        {
            return true;
        }

        //public ICommand CheckLogLevelsCommand
        //{
        //    get
        //    {
        //        if (checkLogLevelsCommand == null)
        //            checkLogLevelsCommand = new RelayCommand(() => CheckLogLevels(), () => CanCheckLogLevels());

        //        return reDisplayCommand;
        //    }
        //}
        //private void CheckLogLevels()
        //{
        //    LogLevelInfoIsChecked = true;
        //}

        //private bool CanCheckLogLevels()
        //{
        //    return true;
        //}


        //public ICommand CheckDisplayFilterCommand
        //{
        //    get
        //    {
        //        if (_checkDisplayFilterCommand == null)
        //            _checkDisplayFilterCommand = new RelayCommand(() => AddPerson(), () => this.CanAddPerson());

        //        return this.addCommand;
        //    }
        //}

        //private bool CanExecuteSaveCommand()
        //{
        //    return true;
        //}

        //private void CreateCheckDisplayFilterCommand()
        //{
        //    // CheckDisplayFilterCommand = new RelayCommand()
        //}

        //public void CheckDisplayFilterExecute()
        //{
        //    LogLevelInfoIsChecked = true;
        //}   

    }
}
