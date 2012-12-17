using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using CK.Core;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace CiView.SewerModel
{
    public class LineItem
    {
        #region Fields
        private double _nodeHeight;
        private bool _isCollapsed;
        private LineItem _nextError;
        #endregion

        #region Properties
        public BagItems Host { get; private set; }
        public LineItem Parent { get; internal set; }
        public LineItem Next { get; internal set; }
        public LineItem FirstChild { get; internal set; }
        public LineItem LastChild { get; internal set; }
        public LineItem Previous { get; internal set; }
        public int Depth { get; internal set; }
        public LogLevel LogType { get; private set; }
        public double ItemHeight { get; private set; }
        public String Content { get; private set; }
        public bool IsCollapsed 
        {
            get { return _isCollapsed; }
            set
            {
                if (value == _isCollapsed)
                    return;

                if (value)
                {
                   Collapse(true); 
                }
                else
                {
                  UnCollapse(true);
                }
            }
        }
        public double NodeHeight
        {
            get { return _nodeHeight; }
            internal set { _nodeHeight = value; }
        }
        public LineItem NextError {
            get { return _nextError; }
            internal set
            {
                _nextError = value;
                if (Parent != null)
                    Parent.NextError = value;
            }
        }
        public LineItem NextSiblingError { get; /*TODO*/ internal set; }
        public bool IsEllipseCollapsed { get; /*TODO*/ internal set; }
        #endregion

        #region Optionals Constructor

        public LineItem(String content, LogLevel logLevel, BagItems host)
            : this(content, logLevel, host, false) { }

        public LineItem(String content, LogLevel logLevel, BagItems host, bool isCollaspsed)
            : this(content, logLevel, host, false, false) { }
        #endregion

        public LineItem(String content, LogLevel logLevel, BagItems host, bool isCollaspsed, bool isEllipseCollapsed)
        {
            Contract.Requires(content != null);
            Contract.Requires(logLevel != null);
            Contract.Requires(host != null);
      
            Next = null;
            Previous = null;
            Host = host;
            LogType = logLevel;
            Content = content;
            ItemHeight = ComputeItemHeight(content);
            _nodeHeight = ItemHeight;

            // TODO : Maybe have some particulary behaviors if the item is collapsed or ellipsed
            IsCollapsed = isCollaspsed;
            IsEllipseCollapsed = isEllipseCollapsed;
        }

        public void InsertChildren(List<LineItem> children)
        {
            Contract.Requires(children != null, "List<LineItem> children must be not null");
            foreach (LineItem child in children)
            {
                InsertChild(child);
            }
        }

        public void InsertChild(LineItem child)
        {
            Contract.Requires(child != null, "LineItem child must be not null");
            child.Parent = this;
            child.Depth = Depth + 1;
            if (FirstChild == null)
            {
                FirstChild = child;
                LastChild = child;
            }
            else
            {
                child.Previous = LastChild;
                LastChild.Next = child;
                LastChild = child;
            }
            child.IncrementParentsNodeHeight(child.ItemHeight);

            if (child.LogType == LogLevel.Error)
            {
                child.NextError = child;
            }
            Host.OnChildInserted(this, EventArgs.Empty);
            
        }

        public void Delete()
        {
            Delete(true);
        }
       
        public override string ToString()
        {
            return Content;
        }

        #region Private/Internal Helpers
        
        private double ComputeItemHeight(String content)
        {
            return 1.0;
        }
       
        private void Delete(bool isRaiser)
        {
            
           if(FirstChild != null)
            {
                FirstChild.Delete(false);
            }
            while(Next!=null && !isRaiser)
            {
                Next.Delete(false);
            }

            if (isRaiser == false)
            {
                if (Previous != null)
                {
                    Previous.Next = null;
                    Previous = null;
                }
                if (Parent != null)
                {
                    Parent.FirstChild = null;
                    Parent.LastChild = null;
                    Parent = null;
                }

            }
            else
            {
                if (Parent != null)
                {
                    if(Parent.FirstChild==this)
                    {
                        Parent.FirstChild = Next;
                    }
                    if(Parent.LastChild == this)
                    {
                        Parent.LastChild = Previous;
                    }

         
                    Parent = null;
                }
                if (Next != null && Previous == null)
                {
                    Next.Previous = null;
                }
                if (Previous != null)
                {
                   
                   
                    Previous.Next = Next;
                    if(Next!=null)
                    {
                    Next.Previous = Previous;
                    }
                    Previous = null;
                    Next = null;
                }

                Host.OnItemDeleted(this,EventArgs.Empty);
               
            }
            Console.WriteLine("Delete {0}", this);
            NextSiblingError = null;
            NextError = null;
      

        }
    
        internal void Collapse(bool isRaiser)
        {
            if (FirstChild != null)
            {
                FirstChild.Collapse(false);
            }
            while (Next != null && !isRaiser)
            {
                Next.Collapse(false);
            }
            DecrementParentsNodeHeight(ItemHeight);
            NodeHeight = 0;
            _isCollapsed = true;

            if (isRaiser)
                Host.OnCollasped(this, EventArgs.Empty);
        }
     
        internal void UnCollapse(bool isRaiser)
        {
            if (FirstChild != null)
            {
                FirstChild.UnCollapse(false);
            }
            while (Next != null && !isRaiser)
            {
                Next.UnCollapse(false);
            }
            IncrementParentsNodeHeight(ItemHeight);
            NodeHeight += ItemHeight;
            _isCollapsed = false;

            if (isRaiser)
                Host.OnUnCollasped(this, EventArgs.Empty);
        }
  
        internal void IncrementParentsNodeHeight(double height)
        {
            if (Parent != null)
            {
                Parent.NodeHeight += height;
                Parent.IncrementParentsNodeHeight(height);
            }
        }

        internal void DecrementParentsNodeHeight(double height)
        {
            if (Parent != null)
            {
                Parent.NodeHeight -= height;
                Parent.DecrementParentsNodeHeight(height);
            }
        }

        #endregion
    }
}