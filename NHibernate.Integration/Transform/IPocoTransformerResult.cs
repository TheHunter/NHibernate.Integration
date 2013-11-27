namespace NHibernate.Transform
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPocoTransformerResult
        : IResultTransformer
    {
        /// <summary>
        /// 
        /// </summary>
        System.Type ResultType { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        object TransformInstance(object instance);
    }
}
