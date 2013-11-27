using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NHibernate.Exceptions;
using NHibernate.Properties;

namespace NHibernate.Reflection
{
    /// <summary>
    /// 
    /// </summary>
    public class PocoAccessor
        : IPocoAccessor
    {
        private readonly System.Type pocoType;
        private readonly ConstructorInfo constructor;
        private readonly IEnumerable<IPropertyAction> propertyActions;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyAccessor"></param>
        /// <param name="pocoType"></param>
        public PocoAccessor(IPropertyAccessor propertyAccessor, System.Type pocoType)
        {
            this.pocoType = pocoType;

            BindingFlags flags = BindingFlags.CreateInstance | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty;
            IEnumerable<PropertyInfo> properties = pocoType.GetProperties(flags);

            List<IPropertyAction> customAccessors = new List<IPropertyAction>();
            
            foreach (var property in properties)
            {
                try
                {
                    #region getter..
                    IGetter getter = null;
                    try
                    {
                        getter = propertyAccessor.GetGetter(pocoType, property.Name);
                    }
                    catch
                    {
                    }
                    #endregion

                    #region setter..
                    ISetter setter = null;
                    try
                    {
                        setter = propertyAccessor.GetSetter(pocoType, property.Name);
                    }
                    catch
                    {
                    }
                    #endregion

                    if (getter != null || setter != null)
                        customAccessors.Add(new PropertyAction(property, getter, setter));
                }
                catch
                {
                    
                }
            }

            if (customAccessors.Count == 0)
                throw new MissingAccessorsException(string.Format("The resultType object doens't exposes non private members to map, resultType: {0}", pocoType.Name));

            this.propertyActions = new HashSet<IPropertyAction>(customAccessors);
            this.constructor = pocoType.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, System.Type.EmptyTypes, null);
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Type PocoType { get { return this.pocoType; } }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<IPropertyAction> PropertyActions { get { return this.propertyActions; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public IPropertyAction GetPropertyAction(string propertyName)
        {
            return this.propertyActions.FirstOrDefault(n => n.MemberName.Equals(propertyName));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="comparisonType"></param>
        /// <returns></returns>
        public IPropertyAction GetPropertyAction(string propertyName, StringComparison comparisonType)
        {
            return this.propertyActions.FirstOrDefault(n => n.MemberName.Equals(propertyName, comparisonType));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public object MakeInstance()
        {
            return pocoType.IsClass
                       ? constructor.Invoke(null)
                       : Activator.CreateInstance(this.pocoType, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.pocoType.GetHashCode();
        }
    }
}