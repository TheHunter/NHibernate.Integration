using System;
using System.Collections.Generic;

namespace NHibernate.Reflection
{
    /// <summary>
    /// 
    /// </summary>
    public static class HelperType
    {
        /// <summary>
        /// 
        /// </summary>
        private static readonly Dictionary<System.Type, object> defaultValues = new Dictionary<System.Type, object>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object GetDefaultValue(this System.Type type)
        {
            if (type.IsValueType)
            {
                object ret;
                if (!defaultValues.ContainsKey(type))
                {
                    ret = Activator.CreateInstance(type);
                    defaultValues.Add(type, ret);
                }
                else
                {
                    ret = defaultValues[type];
                }
                return ret;
            }
            return null;
        }

    }
}
