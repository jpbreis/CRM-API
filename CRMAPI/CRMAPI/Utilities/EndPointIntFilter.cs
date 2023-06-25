using System.Linq.Expressions;

namespace CRMAPI.Utilities
{
    public static class EndPointIntFilter
    {
        public static Expression<Func<T, bool>> CreateIntPropertyFilter<T>(string propertyName, int[] propertyValues)
        {
            var parameter = Expression.Parameter(typeof(T), "entity");
            var property = Expression.Property(parameter, propertyName);
            var nullExpression = Expression.Constant(null, typeof(int?));

            var nullableValues = propertyValues.Select(value => (int?)value).ToArray();

            var containsMethod = typeof(Enumerable).GetMethods()
                .Where(m => m.Name == "Contains" && m.GetParameters().Length == 2)
                .Single()
                .MakeGenericMethod(typeof(int?));
            var containsExpression = Expression.Call(containsMethod, Expression.Constant(nullableValues), property);

            var isNullExpression = Expression.Equal(property, nullExpression);

            var combinedExpression = Expression.OrElse(isNullExpression, containsExpression);

            var lambda = Expression.Lambda<Func<T, bool>>(combinedExpression, parameter);

            return lambda;
        }
    }
}
