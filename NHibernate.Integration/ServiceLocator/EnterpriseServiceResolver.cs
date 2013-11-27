using System;
using NHibernate.Criterion;
using NHibernate.Resolvers;
using NHibernate.Transform;

namespace NHibernate.ServiceLocator
{
    /// <summary>
    /// 
    /// </summary>
    public class EnterpriseServiceResolver
        : IServiceResolver
    {
        private readonly IPocoResolver pocoResolver;
        private readonly ICriteriaBuilder criteriaBuilder;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pocoResolver"></param>
        /// <param name="criteriaBuilder"></param>
        public EnterpriseServiceResolver(IPocoResolver pocoResolver, ICriteriaBuilder criteriaBuilder)
        {
            if (pocoResolver == null)
                throw new ArgumentNullException("pocoResolver", "The pocoResolver instance cannot be null.");

            if (criteriaBuilder == null)
                throw new ArgumentNullException("criteriaBuilder", "The criteriaBuilder instance cannot be null.");

            this.pocoResolver = pocoResolver;
            this.criteriaBuilder = criteriaBuilder;
        }

        /// <summary>
        /// 
        /// </summary>
        public IPocoResolver PocoResolver { get { return pocoResolver; } }

        /// <summary>
        /// 
        /// </summary>
        public ICriteriaBuilder CriteriaBuilder { get { return criteriaBuilder; } }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TInput"></typeparam>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public TOutput TransformFrom<TInput, TOutput>(TInput model)
            where TInput : class
            where TOutput : class
        {
            return this.pocoResolver.TransformFrom<TInput, TOutput>(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public IResultTransformer GetTransformer<TEntity>()
            where TEntity : class
        {
            return this.pocoResolver.GetTransformer<TEntity>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public DetachedCriteria MakeCriteria<TEntity>(TEntity model)
            where TEntity : class
        {
            return this.CriteriaBuilder.MakeCriteria<TEntity>(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public DetachedCriteria MakeCriteria<TEntity>(int pageIndex, int pageSize)
            where TEntity : class
        {
            return DetachedCriteria.For<TEntity>()
                                    .SetFirstResult(pageIndex * pageSize)
                                    .SetMaxResults(pageSize);
        }
    }
}