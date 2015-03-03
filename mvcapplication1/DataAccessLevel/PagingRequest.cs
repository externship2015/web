using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace MvcApplication1.DataAccessLevel
{
    public class PagingRequest
    {
        public int PageNumber { get; set; }    
        public int PageSize { get; set; }
        public string SortIndex { get; set; }
        public string SortOrder { get; set; }
        public PagingResponse<TTarget> GetResponse<TSource, TTarget>(IQueryable<TSource> items, Func<TSource, TTarget> selector)
        {
            var totalItems = items.Count();
            //if (String.IsNullOrEmpty(SortIndex) == false)
            //{
            //    Expression<Func<DateTime, int>> x = d => d.Date.TimeOfDay.Days;
            //    //apply sort
            //    items = SortOrder == "asc" ? items.OrderBy(SortIndex) : items.OrderByDescending(SortIndex);
            //}
            return new PagingResponse<TTarget>
            {
                Total = (int)Math.Ceiling((double)totalItems / PageSize),
                Page = PageNumber,
                Records = totalItems,
                Rows = items.Skip((PageNumber - 1) * PageSize).Take(PageSize).AsEnumerable().Select(selector).ToArray(),       
            };
         }
 
        public PagingResponse<TTarget> GetResponse<TSource, TTarget>(IEnumerable<TSource> items, Func<TSource, TTarget> selector)
        {
            return GetResponse(items.AsQueryable(), selector);
        }
    }
    
    public class PagingResponse<T>
    {
        public int Total { get; set; }
        public int Page { get; set; }
        public int Records { get; set; }
        public T[] Rows { get; set; }
    }

    
}
