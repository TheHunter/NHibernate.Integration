using System;
using DynamicMapResolver;

namespace NHibernate.Transform
{
    /// <summary>
    /// 
    /// </summary>
    public class PocoTransformer
        : IPocoTransformer
    {
        private readonly System.Type typeToTransform;
        private readonly System.Type typeTransformer;
        private readonly ISourceMapper mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapper"></param>
        public PocoTransformer(ISourceMapper mapper)
        {
            if (mapper == null)
                throw new ArgumentNullException("mapper", "mapper object cannot be null.");

            this.typeToTransform = mapper.SourceType;
            this.typeTransformer = mapper.DestinationType;
            this.mapper = mapper;
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Type TypeToTransform
        {
            get { return this.typeToTransform; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Type TypeTransformer
        {
            get { return this.typeTransformer; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public object Transform(object source)
        {
            return mapper.Map(source);
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