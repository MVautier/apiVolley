using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiColomiersVolley.BLL.Core.Models.Generic
{
    public class PagedList<T> where T : class
    {
        public int Count { get; private set; }

        public List<T> Datas { get; private set; }

        public PagedList(List<T> datas, int count)
        {
            Count = count;
            Datas = datas;
        }
    }
}
