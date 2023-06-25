using CRMAPI.Controllers;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data;
using System.Linq.Expressions;

namespace CRMAPI.Utilities
{
    public class EndPointDataFilter
    {
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
