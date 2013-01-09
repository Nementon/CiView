using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ActivityLogger.SewerModel;
using ActivityLogger.Sink;
using CK.Core;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Caliburn.Micro;
using System.ComponentModel;
using System.Windows;
using BinaryLogRecorder;

namespace ModelView.ViewModel
{
    public class ActivityLoggerViewModel : BaseViewModel
    {
        #region Fields
        private ObservableCollection<LineItem> _logs;
        private PaginatedObservableCollection<LineItem> _obs;
        private ObservableCollection<string> _logsListInfoBox;
        private bool _logLevelInfoIsChecked = true;
        private bool _logLevelTaceIsChecked = true;
        private bool _logLevelWarnIsChecked = true;
        private bool _logLevelErrorIsChecked = true;
        private bool _logLevelFatalIsChecked = true;

        #endregion

        #region Properties
        
        public ObservableCollection<LineItem> Logs { get { return _logs; } }
        public PaginatedObservableCollection<LineItem> Obs { get { return _obs; } }
        public ObservableCollection<string> LogsListInfoBox { get { return _logsListInfoBox; } }
        public bool LogLevelInfoIsChecked { get { return _logLevelInfoIsChecked; } set { _logLevelInfoIsChecked = value; ReDisplay(); } }
        public bool LogLevelTaceIsChecked { get { return _logLevelTaceIsChecked; } set { _logLevelTaceIsChecked = value; ReDisplay(); } }
        public bool LogLevelWarnIsChecked { get { return _logLevelWarnIsChecked; } set { _logLevelWarnIsChecked = value; ReDisplay(); } }
        public bool LogLevelErrorIsChecked { get { return _logLevelErrorIsChecked; } set { _logLevelErrorIsChecked = value; ReDisplay(); } }
        public bool LogLevelFatalIsChecked { get { return _logLevelFatalIsChecked; } set { _logLevelFatalIsChecked = value; ReDisplay(); } }
        public LineItem SelectedLog { get; set; }
        private ICommand toggleCollapseCommand;
        private ICommand displayInfoCommand;
        //private ICommand checkLogLevelsCommand;

        #endregion

       

        public ActivityLoggerViewModel(BagItems bag)
        {
            _logs = new ObservableCollection<LineItem>();
            _obs = new PaginatedObservableCollection<LineItem>(10);
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
            checkCollapseFilter(sender);
            Obs.Add(sender);
           // addLogToLogsList(sender);
           //addInfoLogToInfoBox(sender);
        }

        //private void addLogToLogsList(LineItem sender)
        //{
        //    checkCollapseFilter(sender);
        //    //if (sender.IsCollapsed)
        //    //{

        //    //    if ((sender.Previous != null) && !sender.Previous.IsCollapsed)
        //    //    {
        //    //        Logs.Add("[...]");
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    Logs.Add(formatLog(sender));
        //    //}
        //}

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

        //private string formatLog(LineItem sender)
        //{
        //    String log = "";
        //    int depth = sender.Depth; 

        //    while (depth != 0)
        //    {
        //        log = log + "   ";
        //        depth--;
        //    }
        //   log = log + sender.Content;

        //    return log;
        //}

        public void ToggleCollapse(LineItem sender)
        {
            if (sender.IsCollapsed)
            {
                sender.IsCollapsed = false;
            }
            else
            {
                sender.IsCollapsed = true;
            }
            ReDisplay();
        }

        public void collapseAllLogs()
        {
            Logs.Clear();
            foreach (LineItem i in Obs)
            {
                i.IsCollapsed = true;
                //addLogToLogsList(i);
            }
        }

        public void uncollapseAllLogs()
        {
            Logs.Clear();
            foreach (LineItem i in Obs)
            {
                i.IsCollapsed = false;
                //addLogToLogsList(i);
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
            if (sender.FirstChild != null)
            {
                LogsListInfoBox.Add("Children : " + sender.FirstChild.Content);
            }
            else
            {
                LogsListInfoBox.Add("Children : null");
            }
            if (sender.Next != null)
            {
                LogsListInfoBox.Add("Next : " + sender.Next.Content);
            }
            else
            {
                LogsListInfoBox.Add("Next : null");
            }
        
        }

        public ICommand ToggleCollapseCommand
        {
            get
            {
            
                if (toggleCollapseCommand == null)
                    toggleCollapseCommand = new RelayCommand(() => ToggleCollapse(SelectedLog), () => true);

                return toggleCollapseCommand;
            }
        }

        public ICommand DisplayInfoCommand
        {
            get
            {
                if (displayInfoCommand == null)
                    displayInfoCommand = new RelayCommand(() => DisplayInfo(SelectedLog), () => true);

                return displayInfoCommand;
            }
        }

        public void DisplayInfo(LineItem sender)
        {
            LogsListInfoBox.Clear();
            addInfoLogToInfoBox(sender);
        }
        
        private void ReDisplay()
        {
            foreach (LineItem l in Obs)
            {
                checkCollapseFilter(l);
            }
            Obs.ForceRaiseChanged();
           
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
