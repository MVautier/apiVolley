using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace ApiColomiersVolley.BLL.Core.Models.Generic
{
    public class Sorting
    {
        public string Field { get; set; }
        public bool OrderAsc { get; set; }

        public IQueryable<T> Sort<T>(IQueryable<T> list)
        {
            if (string.IsNullOrEmpty(Field))
            {
                return list;
            }

            return list.OrderBy(string.Format("{0} {1}", Field, OrderAsc ? "ASC" : "DESC"));
        }

        public IQueryable<T> Sort<T>(IQueryable<T> list, string field)
        {
            if (string.IsNullOrEmpty(Field))
            {
                return list;
            }

            return list.OrderBy(string.Format("{0} {1}", field, OrderAsc ? "ASC" : "DESC"));
        }

        public IQueryable<T> ApplyExpressions<T, TKey>(IQueryable<T> source, Expression<Func<T, TKey>> sortBy, List<Expression<Func<T, TKey>>> thenBy = null)
        {
            IOrderedQueryable<T> result;

            result = OrderAsc ? source.OrderBy(sortBy) : source.OrderByDescending(sortBy);
            if (thenBy != null && thenBy.Any())
            {
                thenBy.ToList().ForEach(thenBy =>
                {
                    result = OrderAsc ? result.ThenBy(thenBy) : result.ThenByDescending(thenBy);
                });
            }
            return result;
        }
    }
}
