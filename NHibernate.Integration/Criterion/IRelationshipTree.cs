using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Criterion
{
    /// <summary>
    /// Contains information about property relationship present into domain class.
    /// </summary>
    public interface IRelationshipTree
    {
        /// <summary>
        /// Indicates the relationship root of the calling instance.
        /// </summary>
        IRelationshipTree Parent { get; }

        /// <summary>
        /// Indicates the property name on which there's an relationship.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The alias to assign into property relationship.
        /// </summary>
        string Alias { get; }

        /// <summary>
        /// The property type.
        /// </summary>
        System.Type Type { get; }

        /// <summary>
        /// Returns the full path of this relationship.
        /// </summary>
        /// <returns></returns>
        string GetPath();

        /// <summary>
        /// Returna all relationships of the calling instance.
        /// </summary>
        IEnumerable<IRelationshipTree> Relationships { get; }

        /// <summary>
        /// Appends a new relationship of the calling instance.
        /// </summary>
        /// <param name="name">property name</param>
        /// <param name="alias">alias of this relationship</param>
        /// <param name="type">property type.</param>
        /// <returns></returns>
        IRelationshipTree AddRelationship(string name, string alias, System.Type type);
    }

    
}
