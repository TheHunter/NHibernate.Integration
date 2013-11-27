using System.Reflection;
using NHibernate.Exceptions;
using NHibernate.Properties;

namespace NHibernate.Reflection
{
    /// <summary>
    /// 
    /// </summary>
    public class PropertyAction
        : IPropertyAction
    {
        private readonly PropertyInfo property;
        private readonly IGetter getter;
        private readonly ISetter setter;
        private readonly AccessType accessType;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="property"></param>
        /// <param name="getter"></param>
        /// <param name="setter"></param>
        public PropertyAction(PropertyInfo property, IGetter getter, ISetter setter)
        {
            if (property == null)
                throw new PropertyAccessorException("The propertyInfo for the current PropertyAction cannot be null.");

            if (getter == null && setter == null)
                throw new PropertyAccessorException(string.Format("The current PropertyAction doesn't have no accessor, property name: {0} - declaring type: {1}", property.Name, property.DeclaringType == null ? string.Empty : property.DeclaringType.FullName));

            if (getter != null && setter != null)
                this.accessType = AccessType.ReadWrite;
            else
                this.accessType = getter != null ? AccessType.Read : AccessType.Write;


            this.property = property;
            this.getter = getter;
            this.setter = setter;
        }

        /// <summary>
        /// 
        /// </summary>
        public string MemberName
        {
            get { return this.property.Name; }
                
        }
        
        /// <summary>
        /// 
        /// </summary>
        public System.Type PropertyType
        {
            get { return this.property.PropertyType; }
        }

        /// <summary>
        /// 
        /// </summary>
        public AccessType AccessType
        {
            get { return this.accessType; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="value"></param>
        public void Set(object instance, object value)
        {
            if (setter == null)
                return;

            setter.Set(instance, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public object Get(object instance)
        {
            return getter == null ? null : getter.Get(instance);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is PropertyAction)
                return obj.GetHashCode() == this.GetHashCode();

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (this.property.GetHashCode() - this.property.Name.GetHashCode());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("Propertyname: {0}, TypeToResolve: {1}", this.property.Name, this.property.PropertyType.FullName);
        }
    }
}