﻿using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Exceptions;
using NHibernate.Metadata;
using NHibernate.SqlCommand;

namespace NHibernate.Criterion
{
    /// <summary>
    /// 
    /// </summary>
    public class CriteriaBuilder
        : ICriteriaBuilder
    {
        private readonly Func<System.Type, IPersistentClassInfo> metadataProvider;
        private MatchMode likeMode;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="metadataProvider"></param>
        public CriteriaBuilder(Func<System.Type, IPersistentClassInfo> metadataProvider)
        {
            if (metadataProvider == null)
                throw new CriteriaBuilderException("The metadataProvider instance cannot be null.");

            this.metadataProvider = metadataProvider;
            this.Mode = EntityMode.Poco;
            this.likeMode = MatchMode.Exact;
        }

        /// <summary>
        /// 
        /// </summary>
        public EntityMode Mode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matchMode"></param>
        public void EnableLike(MatchMode matchMode)
        {
            this.likeMode = matchMode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public DetachedCriteria MakeCriteria(object instance)
        {
            if (instance == null)
                return null;

            return BuildCriteria(instance.GetType(), instance);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="instance"></param>
        /// <returns></returns>
        public DetachedCriteria MakeCriteria<TEntity>(object instance)
            where TEntity : class
        {
            return MakeCriteria(typeof(TEntity), instance);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="persistentClass"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        public DetachedCriteria MakeCriteria(System.Type persistentClass, object instance)
        {
            if (instance == null)
                return null;

            return BuildCriteria(persistentClass, instance);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="persistentClass"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        private DetachedCriteria BuildCriteria(System.Type persistentClass, object instance)
        {
            System.Type typeClass = persistentClass;
            IPersistentClassInfo metadataInfo = metadataProvider.Invoke(typeClass);

            if (metadataInfo == null)
                throw new Exception("non metadata info.");

            string alias = typeClass.Name.ToLower();
            DetachedCriteria root = DetachedCriteria.For(typeClass, alias);

            object idValue = metadataInfo.HasIdentifierProperty ? metadataInfo.GetIdentifier(instance, this.Mode) : null;
            //object idValue = null;

            if (idValue != null)
            {
                root.Add(Restrictions.IdEq(idValue));
            }
            else
            {
                var propertiesToExclude = new List<IPropertyInfo>(metadataInfo.Properties.Where(n => n.IsAssociationType))
                    {
                        metadataInfo.Identifier
                    };

                var propertiesAssociation = new List<IPropertyInfo>(metadataInfo.Properties.Where(n => n.IsAssociationType && !n.IsCollectionType));

                Example ex = Example.Create(instance)
                                    .ExcludeNulls()
                                    .EnableLike(this.likeMode)
                                    ;

                propertiesToExclude.ForEach(n => ex.ExcludeProperty(n.Name));
                root.Add(ex);

                foreach (var propertyInfo in propertiesAssociation)
                {
                    AddCriterion(root, alias, propertyInfo,
                                 metadataInfo.GetPropertyValue(instance, propertyInfo.Name, this.Mode));
                }

            }
            return root;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="root"></param>
        /// <param name="rootAlias"></param>
        /// <param name="property"></param>
        /// <param name="instance"></param>
        private void AddCriterion(DetachedCriteria root, string rootAlias, IPropertyInfo property, object instance)
        {
            if (instance == null)
                return;

            System.Type typeClass = instance.GetType();
            IPersistentClassInfo metadataInfo = metadataProvider.Invoke(typeClass);

            if (metadataInfo == null)
                throw new Exception("non metadata info.");

            object idValue = metadataInfo.HasIdentifierProperty ? metadataInfo.GetIdentifier(instance, this.Mode) : null;

            ICriterion criterion = idValue != null
                                    ? Restrictions.IdEq(idValue)
                                    : Example.Create(instance)
                                             .ExcludeNulls()
                                             .EnableLike(this.likeMode)
                                             ;

            root.CreateAlias(string.Format("{0}.{1}", rootAlias, property.Name), typeClass.Name.ToLower(), JoinType.InnerJoin, criterion);

        }
    }
}
