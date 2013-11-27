namespace NHibernate.Transform
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPocoTransformer
    {
        /// <summary>
        /// 
        /// </summary>
        System.Type TypeToTransform { get; }

        /// <summary>
        /// 
        /// </summary>
        System.Type TypeTransformer { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        object Transform(object value);
    }
}
