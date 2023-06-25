using CRMAPI.Models;
using System.Linq.Expressions;

namespace CRMAPI.Utilities
{
    public class EndPointFilter
    {
        public static Expression<Func<T, bool>> PassProp<T>(string stringName = "", string stringValue = "", int[]? intName = null, int[]? intValue = null, bool valid = false)
        {
            if (stringName != "" && stringValue != "")
            {
                var stringFilter = CreateStringPropertyFilter<T>(stringName, stringValue);
                return stringFilter;
            }

            if (intName != null && intName.Length > 0)
            {
                var idFilter = CreateIntPropertyFilter<T>(stringName, intValue);
                return idFilter;
            }

            return null;
        }

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

        public static Expression<Func<T, bool>> CreateDataFilter<T>(DateTime? startDate, DateTime? endDate, string dataColumn)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            Expression? filterExpression = null;

            if (startDate != null && DateTime.TryParse(startDate.ToString(), out DateTime parsedDateStart))
            {
                var dataStartProperty = Expression.Property(parameter, dataColumn);
                Expression? dataProp;

                if (dataStartProperty.Type == typeof(DateTime?))
                {
                    var nullableDateStartProperty = Expression.Property(dataStartProperty, "Value");
                    var isDateStartNull = Expression.Property(dataStartProperty, "HasValue");

                    dataProp = Expression.Condition(isDateStartNull,
                        Expression.Property(nullableDateStartProperty, "Date"),
                        Expression.Constant(DateTime.MinValue, typeof(DateTime)));
                }
                else
                    dataProp = Expression.Property(dataStartProperty, "Date");

                var dateStartValue = Expression.Constant(parsedDateStart.Date, typeof(DateTime));
                var dateStartCheck = Expression.Equal(dataProp, dateStartValue);
                filterExpression = dateStartCheck;

                if (endDate != null && DateTime.TryParse(endDate.ToString(), out DateTime parsedDateEnd))
                {
                    var dataStartValue = Expression.Constant(parsedDateStart.Date, typeof(DateTime));
                    var dataEndValue = Expression.Constant(parsedDateEnd.Date.AddDays(1), typeof(DateTime));
                    var dateStartBetweenCheck = Expression.AndAlso(
                        Expression.GreaterThanOrEqual(dataProp, dataStartValue),
                        Expression.LessThan(dataProp, dataEndValue));
                    filterExpression = Expression.OrElse(filterExpression, dateStartBetweenCheck);
                }
            }

            if (filterExpression != null)
            {
                var lambda = Expression.Lambda<Func<T, bool>>(filterExpression, parameter);
                return lambda;
            }

            return null;
        }
    }
}
