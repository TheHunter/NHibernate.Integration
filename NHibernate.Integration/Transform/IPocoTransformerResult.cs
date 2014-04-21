using System;

namespace NHibernate.Transform
{
    /// <summary>
    /// A custom class for transforming input instance into specific object type.
    /// </summary>
    public interface IPocoTransformerResult
        : IResultTransformer
    {
        /// <summary>
        /// The result type of input instance.
        /// </summary>
        System.Type ResultType { get; }

    }
}
