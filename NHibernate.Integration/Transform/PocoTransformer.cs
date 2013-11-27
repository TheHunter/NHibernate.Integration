using System;

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
        private readonly Func<object, object> transformer;
        //private readonly ISourceMapper mapper;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="transformer"></param>
        /// <param name="typeToTransform"></param>
        /// <param name="typeTransformer"></param>
        public PocoTransformer(Func<object, object> transformer , System.Type typeToTransform, System.Type typeTransformer)
        {
            if (typeToTransform == null)
                throw new ArgumentNullException("typeToTransform", "TypeToResolve cannot be null.");

            if (typeTransformer == null)
                throw new ArgumentNullException("typeTransformer", "typeResolver cannot be null.");

            //if (mapper == null)
            //    throw new ArgumentNullException("mapper", "mapper object cannot be null.");

            //this.typeToTransform = mapper.SourceType;
            //this.typeTransformer = mapper.DestinationType;
            //this.mapper = mapper;

            if (transformer == null)
                throw new ArgumentNullException("transformer", "mapper object cannot be null.");

            this.typeToTransform = typeToTransform;
            this.typeTransformer = typeTransformer;
            this.transformer = transformer;
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
        /// <param name="value"></param>
        /// <returns></returns>
        public object Transform(object value)
        {
            //return mapper.Map(value);
            return this.transformer(value);
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