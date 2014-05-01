using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Exceptions;
using NHibernate.Extra;
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
        private readonly Func<System.Type, IClassMetadata> metadataProvider;
        private readonly IDictionary<System.Type, IPersistentClassInfo> persistentClassInfo; 
        private MatchMode likeMode;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="metadataProvider"></param>
        public CriteriaBuilder(Func<System.Type, IClassMetadata> metadataProvider)
        {
            if (metadataProvider == null)
                throw new CriteriaBuilderException("The metadataProvider instance cannot be null.");

            this.metadataProvider = metadataProvider;
            this.Mode = EntityMode.Poco;
            this.likeMode = MatchMode.Exact;

            this.persistentClassInfo = new Dictionary<System.Type, IPersistentClassInfo>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="classType"></param>
        /// <returns></returns>
        private IPersistentClassInfo GetClassInfo(System.Type classType)
        {
            IPersistentClassInfo classInfo = this.persistentClassInfo.ContainsKey(classType) ? this.persistentClassInfo[classType] : null;

            if (classInfo == null)
            {
                classInfo = new PersistentClassInfo(metadataProvider.Invoke(classType));
                this.persistentClassInfo.Add(classType, classInfo);
            }
            return classInfo;
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
        public ICriteriaCompiled MakeCriteria(object instance)
        {
            if (instance == null)
                return null;

            return MakeCriteria(instance.GetType(), instance);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="alias"></param>
        /// <returns></returns>
        public ICriteriaCompiled MakeCriteria(object instance, string alias)
        {
            if (instance == null)
                return null;

            return MakeCriteria(instance.GetType(), instance, alias);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="instance"></param>
        /// <returns></returns>
        public ICriteriaCompiled MakeCriteria<TEntity>(TEntity instance)
            where TEntity : class
        {
            return MakeCriteria(typeof(TEntity), instance);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="instance"></param>
        /// <param name="alias"></param>
        /// <returns></returns>
        public ICriteriaCompiled MakeCriteria<TEntity>(TEntity instance, string alias)
            where TEntity : class
        {
            return MakeCriteria(typeof(TEntity), instance, alias);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="persistentClass"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        public ICriteriaCompiled MakeCriteria(System.Type persistentClass, object instance)
        {
            if (persistentClass == null)
                return null;

            return MakeCriteria(persistentClass, instance, persistentClass.Name.ToCamelCase());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="persistentClass"></param>
        /// <param name="instance"></param>
        /// <param name="alias"></param>
        /// <returns></returns>
        public ICriteriaCompiled MakeCriteria(System.Type persistentClass, object instance, string alias)
        {
            if (instance == null || persistentClass == null || alias == null || alias.Trim().Equals(string.Empty))
                return null;

            return BuildCriteria(persistentClass, instance, alias);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="persistentClass"></param>
        /// <param name="instance"></param>
        /// <param name="alias"></param>
        /// <returns></returns>
        private ICriteriaCompiled BuildCriteria(System.Type persistentClass, object instance, string alias)
        {
            return new CriteriaCompiled(alias, persistentClass, this.likeMode, this.Mode, instance, this.GetClassInfo);
        }

    }
}
