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
        : IPocoTransformer<TSource, TDestination>
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
            if (converter == null)
                throw new ArgumentNullException("converter", string.Format("GenericTransformer of <{0}, {1}> cannot be null.", typeof(TSource).Name, typeof(TDestination).Name));

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
        /// <param name="source"></param>
        /// <returns></returns>
        public TDestination Transform(TSource source)
        {
            return this.converter.Invoke(source);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public object Transform(object source)
        {
            if (source is TSource)
                return this.converter((TSource)source);

            return null;
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

            if (obj is PocoTransformer)
                return this.GetHashCode() == obj.GetHashCode();

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return 7 * (this.typeToTransform.GetHashCode() - this.typeTransformer.GetHashCode());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("Type to resolve: {0}, Type resolver: {1}", this.TypeToTransform.FullName, this.TypeTransformer.FullName);
        }
    }
}
