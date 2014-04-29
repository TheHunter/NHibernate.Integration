using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Exceptions;
using NHibernate.Metadata;
using NHibernate.SqlCommand;
using Remotion.Linq.Utilities;

namespace NHibernate.Criterion
{
    /// <summary>
    /// 
    /// </summary>
    public class CriteriaCompiled
        : ICriteriaCompiled
    {
        private readonly string rootAlias;
        private readonly System.Type rootType;
        private readonly MatchMode matchMode;
        private readonly EntityMode entityMode;
        private readonly List<ICriterion> restrictions;
        private readonly object instance;
        private readonly DetachedCriteria criteria;
        private readonly Func<System.Type, IPersistentClassInfo> classInfoProvider;
        private readonly IRelationshipTree relationshipTree;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rootType"></param>
        /// <param name="matchMode"></param>
        /// <param name="entityMode"></param>
        /// <param name="instance"></param>
        /// <param name="classInfoProvider"></param>
        internal CriteriaCompiled(System.Type rootType, MatchMode matchMode, EntityMode entityMode,
                                object instance, Func<System.Type, IPersistentClassInfo> classInfoProvider)
            :this(rootType.Name.Lower(), rootType, matchMode, entityMode, instance, classInfoProvider)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rootAlias"></param>
        /// <param name="rootType"></param>
        /// <param name="matchMode"></param>
        /// <param name="entityMode"></param>
        /// <param name="instance"></param>
        /// <param name="classInfoProvider"></param>
        public CriteriaCompiled(string rootAlias, System.Type rootType, MatchMode matchMode, EntityMode entityMode,
                                object instance, Func<System.Type, IPersistentClassInfo> classInfoProvider)
        {
            
            if (rootAlias == null || rootAlias.Trim().Equals(string.Empty))
                throw new ArgumentException("The alias for making detached criteria cannot be empty or null", "rootAlias");

            if (rootType == null)
                throw new ArgumentNullException("rootType", "The type which can be used for maikng detached criteria cannot be null.");

            if (instance == null)
                throw new ArgumentNullException("instance", "The instance for building detached criteria cannot be null.");

            if (!rootType.IsInstanceOfType(instance))
                throw new ArgumentTypeException("The given instance for building detached criteria is not suitable for the given criteria type.", "instance", rootType, instance.GetType());


            this.rootAlias = rootAlias;
            this.rootType = rootType;
            this.matchMode = matchMode ?? MatchMode.Exact;
            this.entityMode = entityMode;
            this.instance = instance;
            this.classInfoProvider = classInfoProvider;

            this.criteria = DetachedCriteria.For(rootType, rootAlias);
            this.restrictions = new List<ICriterion>();
            this.Init();

            //this.relationshipTree = new RelationshipTree(null, rootAlias, rootType);
        }

        /// <summary>
        /// 
        /// </summary>
        private void Init()
        {
            IPersistentClassInfo metadataInfo = this.classInfoProvider.Invoke(this.rootType);
            if (metadataInfo == null)
                throw new MissingMetadataException(string.Format("No metadata info for type of <{0}>", rootType.FullName));

            object idValue = metadataInfo.HasIdentifierProperty ? metadataInfo.GetIdentifier(instance, this.entityMode) : null;

            if (idValue != null)
            {
                var criterion = Criterion.Restrictions.IdEq(idValue);
                this.criteria.Add(criterion);
                this.restrictions.Add(criterion);
            }
            else
            {
                var propertiesrelationship = new List<IPropertyInfo>(metadataInfo.Properties.Where(n => n.IsAssociationType && !n.IsCollectionType));

                Example criterion = Example.Create(instance)
                                    .ExcludeNulls()
                                    .EnableLike(this.MatchMode)
                                    ;

                if (metadataInfo.Identifier != null)
                    criterion.ExcludeProperty(metadataInfo.Identifier.Name);

                this.criteria.Add(criterion);
                this.restrictions.Add(criterion);
                

                foreach (var propertyInfo in propertiesrelationship)
                {
                    this.AddCriterion(propertyInfo.Name,
                                 metadataInfo.GetPropertyValue(instance, propertyInfo.Name, this.entityMode));
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string RootAlias
        {
            get { return rootAlias; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Type RootType
        {
            get { return rootType; }
        }

        /// <summary>
        /// 
        /// </summary>
        public MatchMode MatchMode
        {
            get { return matchMode; }
        }

        /// <summary>
        /// 
        /// </summary>
        public EntityMode EntityMode
        {
            get { return this.entityMode; }
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<ICriterion> Restrictions
        {
            get { return restrictions; }
        }

        /// <summary>
        /// 
        /// </summary>
        public object Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// 
        /// </summary>
        public DetachedCriteria Criteria
        {
            get { return criteria; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        private void AddCriterion(string propertyName, object propertyValue)
        {
            if (propertyValue == null)
                return;

            System.Type typeClass = propertyValue.GetType();
            IPersistentClassInfo metadataInfo = this.classInfoProvider.Invoke(typeClass);

            if (metadataInfo == null)
                throw new MissingMetadataException(string.Format("No metadata info for type of <{0}>", typeClass.FullName));

            object idValue = metadataInfo.HasIdentifierProperty
                                 ? metadataInfo.GetIdentifier(propertyValue, this.entityMode)
                                 : null;

            ICriterion criterion = null;
            if (idValue != null)
            {
                criterion = Criterion.Restrictions.IdEq(idValue);
            }
            else
            {
                Example ex = Example.Create(propertyValue)
                                   .ExcludeNulls()
                                   .EnableLike(this.matchMode);

                if (metadataInfo.Identifier != null)
                    ex.ExcludeProperty(metadataInfo.Identifier.Name);

                criterion = ex;
            }

            this.restrictions.Add(criterion);
            this.criteria.CreateAlias(string.Format("{0}.{1}", this.rootAlias, propertyName), typeClass.Name.ToLower(), JoinType.InnerJoin, criterion);
        }
    }
}
