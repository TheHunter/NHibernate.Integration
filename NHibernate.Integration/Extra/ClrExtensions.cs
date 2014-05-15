using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Extra
{
    /// <summary>
    /// 
    /// </summary>
    public static class ClrExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToCamelCase(this string str)
        {
            if (str == null || str.Trim().Equals(string.Empty))
                return string.Empty;

            return str.Substring(0, 1).ToLower() + str.Substring(1);
        }

    }
}
