using System;
using System.Linq.Expressions;
using System.Reflection;

namespace OpenDataStorage.Helpers
{
    public static class ReflectionHelper
    {
        public static string GetPropName<TSource, TProp>(Expression<Func<TSource, TProp>> property)
        {
            Type type = typeof(TSource);
            MemberExpression member = property.Body as MemberExpression;
            if (member == null)
            {
                throw new ArgumentException($"Expression '{property.ToString()}' refers to a method, not a property.");
            }
            PropertyInfo propertyInfo = member.Member as PropertyInfo;
            if (propertyInfo == null)
            {
                throw new ArgumentException($"Expression '{property.ToString()}' refers to a field, not a property.");
            }
            if (type != propertyInfo.ReflectedType && !type.IsSubclassOf(propertyInfo.ReflectedType))
            {
                throw new ArgumentException($"Expression '{property.ToString()}' refers to a property that is not from type {type}.");
            }
            return propertyInfo.Name;
        }
    }
}