using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Text;

namespace MyTools
{
    

    public class SortingCondition
    {
        public string FieldName { get; set; }
        public bool IsDescending { get; set; }
    }


    public static class MultiFieldSorter
    {
        public static Func<IQueryable<T>, IOrderedQueryable<T>> MakeSortingFunc<T>(IEnumerable<SortingCondition> conditions)
        {
            var parameter = Expression.Parameter(typeof(T), "p");
            var source = Expression.Parameter(typeof(IQueryable<T>), "source");
            Expression? sortExpression = null;
            var sourceExpression = (Expression)source;

            foreach (var condition in conditions)
            {
                var propertyExpression = Expression.PropertyOrField(parameter, condition.FieldName);
                var keySelector = Expression.Lambda(propertyExpression, parameter);

                sortExpression = sortExpression == null
                    ? Expression.Call(typeof(Queryable), "OrderBy", new[] { typeof(T), propertyExpression.Type }, sourceExpression, keySelector)
                    : condition.IsDescending
                        ? Expression.Call(typeof(Queryable), "ThenByDescending", new[] { typeof(T), propertyExpression.Type }, sortExpression, keySelector)
                        : Expression.Call(typeof(Queryable), "ThenBy", new[] { typeof(T), propertyExpression.Type }, sortExpression, keySelector);

                sourceExpression = sortExpression;
            }

            var orderFunc = Expression.Lambda<Func<IQueryable<T>, IOrderedQueryable<T>>>(sortExpression, source);

            return orderFunc.Compile();
        }

        public static IEnumerable<T> Sort<T>(this IEnumerable<T> @this, IEnumerable<SortingCondition> conditions)
        {
            if (!conditions.Any()) return @this;
            var sortedFunc = MakeSortingFunc<T>(conditions);
            return sortedFunc(@this.AsQueryable());
        }

    }
}
