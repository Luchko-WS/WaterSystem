using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;

namespace OpenDataStorage.Common
{
    public static class Mapper
    {
        public static TRes CreateInstanceAndMapProperties<TRes>(object sourceObj)
        {
            TRes resObj = (TRes)Activator.CreateInstance(typeof(TRes));
            var resPropertiesInfo = typeof(TRes).GetProperties();
            var sourceType = sourceObj.GetType();
            foreach (var resObjProp in resPropertiesInfo)
            {
                var sourcePropInfo = sourceType.GetProperty(resObjProp.Name);
                if (sourcePropInfo != null) resObjProp.SetValue(resObj, sourcePropInfo.GetValue(sourceObj, null));
            }
            return resObj;
        }

        public static void MapProperties(object sourceObj, object targerObject, Func<PropertyInfo, bool> filter = null)
        {
            var targetType = targerObject.GetType();
            var resPropertiesInfo = targetType.GetProperties();
            var sourceType = sourceObj.GetType();
            foreach (var resObjProp in resPropertiesInfo)
            {
                var sourcePropInfo = sourceType.GetProperty(resObjProp.Name);
                if (sourcePropInfo != null)
                {
                    if (filter == null || (filter != null && filter.Invoke(sourcePropInfo)))
                    {
                        var value = sourcePropInfo.GetValue(sourceObj, null);
                        resObjProp.SetValue(targerObject, value);
                    }
                }
            }
        }

        public static IEnumerable<SqlParameter> GetSqlParametersForObject(object sourceObj, Func<PropertyInfo, bool> filter = null)
        {
            var sqlParameters = new List<SqlParameter>();
            var sourceType = sourceObj.GetType();
            var resPropertiesInfo = sourceType.GetProperties();
            foreach (var prop in resPropertiesInfo)
            {
                var sourcePropInfo = sourceType.GetProperty(prop.Name);
                if (filter == null || (filter != null && filter.Invoke(sourcePropInfo)))
                {
                    var value = sourcePropInfo.GetValue(sourceObj, null);
                    sqlParameters.Add(new SqlParameter { ParameterName = prop.Name, Value = value });
                }
            }
            return sqlParameters;
        }
    }
}