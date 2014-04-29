using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Criterion
{
    /// <summary>
    /// 
    /// </summary>
    public class RelationshipTree
        : IRelationshipTree
    {
        private readonly bool isRoot;
        private readonly IRelationshipTree parent;
        private readonly string name;
        private readonly string alias;
        private readonly System.Type type;
        private readonly List<IRelationshipTree> relationships;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="alias"></param>
        /// <param name="type"></param>
        internal protected RelationshipTree(string alias, System.Type type)
        {
            this.isRoot = true;
            this.alias = alias;
            this.type = type;
            this.relationships = new List<IRelationshipTree>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <param name="alias"></param>
        /// <param name="type"></param>
        internal protected RelationshipTree(IRelationshipTree parent, string name, string alias, System.Type type)
        {
            this.isRoot = false;
            this.parent = parent;
            this.name = name;
            this.alias = alias;
            this.type = type;
            this.relationships = new List<IRelationshipTree>();
        }

        /// <summary>
        /// 
        /// </summary>
        public IRelationshipTree Parent
        {
            get { return parent; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Alias
        {
            get { return alias; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Type Type
        {
            get { return type; }
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<IRelationshipTree> Relationships
        {
            get { return relationships; }
        }

        /// <summary>
        /// 
        /// </summary>
        internal void AddRelationship(IRelationshipTree relationshipTree)
        {
            if (relationshipTree != null)
                this.relationships.Add(relationshipTree);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="alias"></param>
        /// <param name="type"></param>
        internal void AddRelationship(string name, string alias, System.Type type)
        {
            this.relationships.Add(new RelationshipTree(this, name, alias, type));
        }
    }
}
