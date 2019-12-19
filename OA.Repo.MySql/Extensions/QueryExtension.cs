using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace OA.Repo.MySql.Extensions
{
    public static class QueryExtension
    {
        public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> q, string SortField, string sort)
        {
            bool Ascending = sort.ToUpper().Equals("ASC");
            var param = Expression.Parameter(typeof(T), "p");
            var prop = Expression.Property(param, SortField);
            var exp = Expression.Lambda(prop, param);
            string method = Ascending ? "OrderBy" : "OrderByDescending";
            Type[] types = new Type[] { q.ElementType, exp.Body.Type };
            var mce = Expression.Call(typeof(Queryable), method, types, q.Expression, exp);
            return q.Provider.CreateQuery<T>(mce);
        }
    }
}
