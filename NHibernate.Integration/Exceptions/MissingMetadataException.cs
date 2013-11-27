using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Exceptions
{
    /// <summary>
    /// Rappresents an generic error when business DAOs execute any kind of query on data store.
    /// </summary>
    public class MissingMetadataException
        : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public MissingMetadataException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public MissingMetadataException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
