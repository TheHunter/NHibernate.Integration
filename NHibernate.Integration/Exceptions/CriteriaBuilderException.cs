using System;

namespace NHibernate.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class CriteriaBuilderException
        : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public CriteriaBuilderException(string message)
            : base(message)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public CriteriaBuilderException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
