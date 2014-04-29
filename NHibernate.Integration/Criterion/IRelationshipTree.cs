using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Criterion
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRelationshipTree
    {
        /// <summary>
        /// 
        /// </summary>
        IRelationshipTree Parent { get; }

        /// <summary>
        /// 
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 
        /// </summary>
        string Alias { get; }

        /// <summary>
        /// 
        /// </summary>
        System.Type Type { get; }

        /// <summary>
        /// 
        /// </summary>
        IEnumerable<IRelationshipTree> Relationships { get; }
    }

    
}
