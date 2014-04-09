using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Criterion
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICriteriaBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        EntityMode Mode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matchMode"></param>
        void EnableLike(MatchMode matchMode);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        DetachedCriteria MakeCriteria(object instance);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="alias"></param>
        /// <returns></returns>
        DetachedCriteria MakeCriteria(object instance, string alias);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="instance"></param>
        /// <returns></returns>
        DetachedCriteria MakeCriteria<TEntity>(object instance) where TEntity : class;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="instance"></param>
        /// <param name="alias"></param>
        /// <returns></returns>
        DetachedCriteria MakeCriteria<TEntity>(object instance, string alias) where TEntity : class;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="persistentClass"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        DetachedCriteria MakeCriteria(System.Type persistentClass, object instance);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="persistentClass"></param>
        /// <param name="instance"></param>
        /// <param name="alias"></param>
        /// <returns></returns>
        DetachedCriteria MakeCriteria(System.Type persistentClass, object instance, string alias);
    }
}
