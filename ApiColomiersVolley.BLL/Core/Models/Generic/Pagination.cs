using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.Core.Models.Generic
{
    public class Pagination
    {
        public int Size { get; set; }
        public int Page { get; set; }

        public IQueryable<T> Paginate<T>(IQueryable<T> list)
        {
            return list.Skip(Page * Size).Take(Size);
        }
    }
}
