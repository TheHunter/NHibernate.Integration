using System;

namespace NHibernate.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class MissingAccessorsException
        : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public MissingAccessorsException(string message)
            : base(message)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public MissingAccessorsException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}