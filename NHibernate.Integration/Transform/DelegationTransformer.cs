using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Transform
{
    /// <summary>
    /// 
    /// </summary>
    public class DelegationTransformer<TSource, TDestination>
        : IPocoTransformer
    {
        private readonly System.Type typeToTransform;
        private readonly System.Type typeTransformer;
        private readonly Func<TSource, TDestination> converter;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="converter"></param>
        public DelegationTransformer(Func<TSource, TDestination> converter)
        {
            this.typeToTransform = typeof(TSource);
            this.typeTransformer = typeof(TDestination);
            this.converter = converter;
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Type TypeToTransform { get { return this.typeToTransform; } }

        /// <summary>
        /// 
        /// </summary>
        public System.Type TypeTransformer { get { return this.typeTransformer; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public object Transform(object value)
        {
            if (value is TSource)
                return this.converter((TSource)value);

            return null;
        }
    }
}
