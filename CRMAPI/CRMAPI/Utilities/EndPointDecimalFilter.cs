using System.Linq.Expressions;

namespace CRMAPI.Utilities
{
    public static class EndPointDecimalFilter
    {
        public static Expression<Func<T, bool>> CreateDoublePropertyFilter<T>(string propertyName, decimal[] propertyValues)
        {
            var parameter = Expression.Parameter(typeof(T), "entity");
            var property = Expression.Property(parameter, propertyName);
            var nullExpression = Expression.Constant(null, typeof(decimal?));

            var nullableValues = propertyValues.Select(value => (decimal?)value).ToArray();

            var containsMethod = typeof(Enumerable).GetMethods()
                .Where(m => m.Name == "Contains" && m.GetParameters().Length == 2)
                .Single()
                .MakeGenericMethod(typeof(decimal?));
            var containsExpression = Expression.Call(containsMethod, Expression.Constant(nullableValues), property);

            var isNullExpression = Expression.Equal(property, nullExpression);

            var combinedExpression = Expression.OrElse(isNullExpression, containsExpression);

            var lambda = Expression.Lambda<Func<T, bool>>(combinedExpression, parameter);

            return lambda;
        }
    }
}
