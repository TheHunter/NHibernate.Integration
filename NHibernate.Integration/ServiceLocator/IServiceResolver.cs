using NHibernate.Criterion;
using NHibernate.Resolvers;
using NHibernate.Transform;

namespace NHibernate.ServiceLocator
{
    /// <summary>
    /// 
    /// </summary>
    public interface IServiceResolver
    {
        /// <summary>
        /// 
        /// </summary>
        IPocoResolver PocoResolver { get; }

        /// <summary>
        /// 
        /// </summary>
        ICriteriaBuilder CriteriaBuilder { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        TOutput TransformFrom<TInput, TOutput>(TInput model)
            where TInput : class
            where TOutput : class;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        IResultTransformer GetTransformer<TEntity>()
            where TEntity : class;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        DetachedCriteria MakeCriteria<TEntity>(TEntity model)
            where TEntity : class;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        DetachedCriteria MakeCriteria<TEntity>(int pageIndex, int pageSize)
            where TEntity : class;
    }
}
