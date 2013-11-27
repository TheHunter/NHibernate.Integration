namespace NHibernate.Reflection
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPropertyAction
    {
        /// <summary>
        /// 
        /// </summary>
        string MemberName { get; }

        /// <summary>
        /// 
        /// </summary>
        System.Type PropertyType { get; }

        /// <summary>
        /// 
        /// </summary>
        AccessType AccessType { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="value"></param>
        void Set(object instance, object value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        object Get(object instance);
    }
}
