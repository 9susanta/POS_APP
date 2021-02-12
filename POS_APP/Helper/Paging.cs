using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_APP.Helper
{
    class Paging
    {
        public int PageIndex { get; set; }

		public List<T> SetPaging<T>(List<T> ListToPage, int RecordsPerPage)
		{
			int PageGroup = PageIndex * RecordsPerPage;

			List<T> PagedList = new List<T>();

			PagedList = ListToPage.Skip(PageGroup).Take(RecordsPerPage).ToList(); //This is where the Magic Happens. If you have a Specific sort or want to return ONLY a specific set of columns, add it to this LINQ Query.

			return PagedList;
		}
	}
}
