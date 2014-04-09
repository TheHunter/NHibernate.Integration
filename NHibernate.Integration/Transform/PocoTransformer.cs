using System;
//using DynamicMapResolver;

namespace NHibernate.Transform
{
    
    //public class PocoTransformer
    //    : IPocoTransformer
    //{
    //    private readonly System.Type typeToTransform;
    //    private readonly System.Type typeTransformer;
    //    private readonly ISourceMapper mapper;

        
    //    public PocoTransformer(ISourceMapper mapper)
    //    {
    //        if (mapper == null)
    //            throw new ArgumentNullException("mapper", "mapper object cannot be null.");

    //        this.typeToTransform = mapper.SourceType;
    //        this.typeTransformer = mapper.DestinationType;
    //        this.mapper = mapper;
    //    }

        
    //    public System.Type TypeToTransform
    //    {
    //        get { return this.typeToTransform; }
    //    }

        
    //    public System.Type TypeTransformer
    //    {
    //        get { return this.typeTransformer; }
    //    }

        
    //    public object Transform(object source)
    //    {
    //        return mapper.Map(source);
    //    }

        
    //    public override bool Equals(object obj)
    //    {
    //        if (obj == null)
    //            return false;

    //        if (obj is PocoTransformer)
    //            return this.GetHashCode() == obj.GetHashCode();

    //        return false;
    //    }

        
    //    public override int GetHashCode()
    //    {
    //        return 7 * (this.typeToTransform.GetHashCode() - this.typeTransformer.GetHashCode());
    //    }

        
    //    public override string ToString()
    //    {
    //        return string.Format("Type to resolve: {0}, Type resolver: {1}", this.TypeToTransform.FullName, this.TypeTransformer.FullName);
    //    }
    //}
}