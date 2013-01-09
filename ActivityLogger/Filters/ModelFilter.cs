using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CK.Core;
using ActivityLogger.SewerModel;

namespace ActivityLogger.Filters
{
   public class ModelFilter
    {
        #region Properties
        public BagItems Host {  get;  set; }
        public int NumChildren { get; set; }
        #endregion
      
        public ModelFilter(int numChildren,BagItems host)
        {
            NumChildren = numChildren;
            Host = host;
        }
        
        public void DeleteBranches()
        {
            if(NumChildren<Host.ChildrenNumber)
            {
                int j = Host.ChildrenNumber - NumChildren;
                for (int i=0;i<j;i++)
                {
                    Host.FirstChild.Delete();
                }
            }
        }

    }
}
