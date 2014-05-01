using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Criterion
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICriteriaCompiled
    {
        /// <summary>
        /// 
        /// </summary>
        string RootAlias { get; }

        /// <summary>
        /// 
        /// </summary>
        System.Type RootType { get; }

        /// <summary>
        /// 
        /// </summary>
        MatchMode MatchMode { get; }

        /// <summary>
        /// 
        /// </summary>
        EntityMode EntityMode { get; }

        /// <summary>
        /// 
        /// </summary>
        IEnumerable<ICriterion> Restrictions { get; }

        /// <summary>
        /// 
        /// </summary>
        object Instance { get; }

        /// <summary>
        /// 
        /// </summary>
        DetachedCriteria Criteria { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        IRelationshipTree FindRelationshipProperty(string property);
    }
}
