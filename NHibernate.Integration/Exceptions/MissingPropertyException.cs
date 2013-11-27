using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class MissingPropertyException
        : MissingMetadataException
    {
        private readonly string propertyName;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="propertyName"></param>
        public MissingPropertyException(string message, string propertyName)
            : base(message)
        {
            this.propertyName = propertyName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="propertyName"></param>
        /// <param name="innerException"></param>
        public MissingPropertyException(string message, string propertyName, System.Exception innerException)
            : base(message, innerException)
        {
            this.propertyName = propertyName;
        }

        /// <summary>
        /// The missing property.
        /// </summary>
        public string PropertyName
        {
            get { return this.propertyName; }
        }
    }
}
