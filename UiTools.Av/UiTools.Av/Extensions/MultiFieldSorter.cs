﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UiTools.Av.Models;

namespace UiTools.Av.Extensions;

public static class MultiFieldSorter
{
    private static Func<IQueryable<T>, IOrderedQueryable<T>> MakeSortingFunc<T>(IEnumerable<SortingCondition> conditions)
    {
        var parameter = Expression.Parameter(typeof(T), "p");
        var source = Expression.Parameter(typeof(IQueryable<T>), "source");
        Expression sortExpression = null;
        var sourceExpression = (Expression)source;

        foreach (var condition in conditions)
        {
            var propertyExpression = Expression.PropertyOrField(parameter, condition.FieldName);
            var keySelector = Expression.Lambda(propertyExpression, parameter);

            sortExpression = sortExpression == null
                //? Expression.Call(typeof(Queryable), "OrderBy", new[] { typeof(T), propertyExpression.Type }, sourceExpression, keySelector)
                ? condition.IsDescending
                    ? Expression.Call(typeof(Queryable), "OrderByDescending", [typeof(T), propertyExpression.Type], sourceExpression, keySelector)
                    : Expression.Call(typeof(Queryable), "OrderBy", [typeof(T), propertyExpression.Type], sourceExpression, keySelector)
                : condition.IsDescending
                    ? Expression.Call(typeof(Queryable), "ThenByDescending", [typeof(T), propertyExpression.Type], sortExpression, keySelector)
                    : Expression.Call(typeof(Queryable), "ThenBy", [typeof(T), propertyExpression.Type], sortExpression, keySelector);

            sourceExpression = sortExpression;
        }

        var orderFunc = Expression.Lambda<Func<IQueryable<T>, IOrderedQueryable<T>>>(sortExpression, source);

        return orderFunc.Compile();
    }

    public static IEnumerable<T> MultiSort<T>(this IEnumerable<T> @this, IEnumerable<SortingCondition> conditions)
    {
        if (!conditions.Any()) return @this;
        var sortedFunc = MakeSortingFunc<T>(conditions);
        return sortedFunc(@this.AsQueryable());
    }

}