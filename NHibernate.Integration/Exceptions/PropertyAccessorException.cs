using System;

namespace NHibernate.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class PropertyAccessorException
        : Exception

    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public PropertyAccessorException(string message)
            : base(message)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public PropertyAccessorException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}