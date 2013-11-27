using NHibernate.Reflection;
using NHibernate.Transform;

namespace NHibernate.Resolvers
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPocoResolver
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        TOutput TransformFrom<TSource, TOutput>(TSource source)
            where TSource : class
            where TOutput : class;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IPocoTransformerResult GetTransformer(System.Type type);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <returns></returns>
        IPocoTransformerResult GetTransformer<TData>()
            where TData : class;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        object TryToTransform(System.Type type, object instance);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IPocoAccessor GetPocoAccessor(System.Type type);
    }
}
