using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CiView.SewerModel
{
    public class BagItems
    {
        #region Properties
        public LineItem FirstChild { get; internal set; }
        public LineItem LastChild { get; internal set; }
        public double Height 
        {
            get
            {
                double h = 0.0;
                LineItem child = FirstChild;
                while(child.Next != null)
                {
                    h += child.NodeHeight;
                    child = child.Next;
                }
                return h;
            }
        }
        #endregion
         

        public void InsertRootItems(List<LineItem> items)
        {
            foreach(LineItem child in items)
            {
                InsterRootItem(child);
            }
        }

        public void InsterRootItem(LineItem item)
        {
            item.Parent = null;
            item.Depth = 0;
            if (FirstChild == null)
            {
               FirstChild = item;
               LastChild = item;
            }
            else
            {
                LastChild.Next = item;
                item.Previous = LastChild;
                LastChild = item;
            }
            OnChildInserted(item, EventArgs.Empty);
        }

        #region Events
        public delegate void CollaspedEventHandler(LineItem sender, EventArgs e);
        public event CollaspedEventHandler ItemCollapsed;
        internal virtual void OnCollasped(LineItem sender, EventArgs e)
        {
            if (ItemCollapsed != null)
                ItemCollapsed(sender, e);
        }

        public delegate void UnCollaspedEventHandler(LineItem sender, EventArgs e);
        public event UnCollaspedEventHandler ItemUnCollapsed;
        internal virtual void OnUnCollasped(LineItem sender, EventArgs e)
        {
            if (ItemUnCollapsed != null)
                ItemUnCollapsed(sender, e);
        }
        public delegate void ItemDeletedEventHandler(LineItem sender, EventArgs e);
        public event ItemDeletedEventHandler ItemDeleted;
        internal virtual void OnItemDeleted(LineItem sender, EventArgs e)
        {
            if (ItemDeleted != null)
                ItemDeleted(sender, e);
        }
        public delegate void ChildInsertedEventHandler(LineItem sender, EventArgs e);
        public event ChildInsertedEventHandler ChildInserted;
        internal virtual void OnChildInserted(LineItem sender, EventArgs e)
        {
            if (ChildInserted != null)
                ChildInserted(sender, e);
        }

        #endregion
    }
}
