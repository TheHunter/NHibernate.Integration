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
        private RelationshipTree(IRelationshipTree parent, string name, string alias, System.Type type)
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
        /// <returns></returns>
        public string GetPath()
        {
            string part1 = this.Parent == null ? string.Empty : (this.Parent.Alias + ".");
            string part2 = this.Name ?? string.Empty;

            if (string.Empty == part1 || string.Empty == part2)
                return string.Empty;

            return string.Format("{0}{1}", part1, part2);
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
        /// <param name="name"></param>
        /// <param name="alias"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public IRelationshipTree AddRelationship(string name, string alias, System.Type type)
        {
            IRelationshipTree relationship = new RelationshipTree(this, name, alias, type);
            this.relationships.Add(relationship);
            return relationship;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("Root: {0}, Name: {1}, Alias: {2}", this.Parent == null ? "null" : this.Parent.Alias, this.Name ?? "null", this.Alias);
        }
    }
}
