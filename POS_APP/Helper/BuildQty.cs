using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_APP.Helper
{
    public class BuildQty
    {
        public List<CountItem> getBuildQty()
        {
            try
            {
                List<CountItem> lstCount = new List<CountItem>();
             
                for(int i=1;i<=12;i++)
                {
                    lstCount.Add(new CountItem { ItemNo=i });
                }
                return lstCount;
            }
            catch (Exception ex)
            {
              
            }
            return null;
        }
    }
    public class CountItem
    {
        public int ItemNo { get; set; }
    }
}
