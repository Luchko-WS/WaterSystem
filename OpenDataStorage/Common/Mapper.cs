using System;

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
    }
}