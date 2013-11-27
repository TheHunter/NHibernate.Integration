using System;
using System.Collections.Generic;

namespace NHibernate.Reflection
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPocoAccessor
    {
        /// <summary>
        /// 
        /// </summary>
        System.Type PocoType { get; }

        /// <summary>
        /// 
        /// </summary>
        IEnumerable<IPropertyAction> PropertyActions { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        IPropertyAction GetPropertyAction(string propertyName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="comparisonType"></param>
        /// <returns></returns>
        IPropertyAction GetPropertyAction(string propertyName, StringComparison comparisonType);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        object MakeInstance();

    }
}
