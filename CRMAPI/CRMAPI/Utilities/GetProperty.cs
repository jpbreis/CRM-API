using System.Reflection;

namespace CRMAPI.Utilities
{
    public static class GetProperty
    {
        public static string? GetPropertyName<T>(string propertyName)
        {
            Type type = typeof(T);
            PropertyInfo? propertyInfo = type.GetProperty(propertyName);

            if (propertyInfo != null)
                return propertyInfo.Name;

            return null;
        }
    }
}
