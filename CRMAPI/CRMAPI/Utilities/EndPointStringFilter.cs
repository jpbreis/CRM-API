using System.Linq.Expressions;
using System;

namespace CRMAPI.Utilities
{
    public static class EndPointStringFilter
    {
        public static Expression<Func<T, bool>> CreateStringPropertyFilter<T>(string propertyName, string propertyValue)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            Expression filterExpression = null;

            var property = Expression.Property(parameter, propertyName);
            var value = Expression.Constant(propertyValue, typeof(string));
            var check = Expression.Call(property, "Contains", null, value);
            filterExpression = filterExpression == null ? check : Expression.AndAlso(filterExpression, check);

            if (filterExpression != null)
            {
                var lambda = Expression.Lambda<Func<T, bool>>(filterExpression, parameter);
                return lambda;
            }

            return null;
        }
    }
}
