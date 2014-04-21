using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Properties;
using NHibernate.Reflection;
using NHibernate.Transform;

namespace NHibernate.Resolvers
{
	/// <summary>
	/// 
	/// </summary>
	public class PocoResolver
		: IPocoResolver
	{
		private readonly Func<object, System.Type, object> transformer;
		private readonly HashSet<IPocoAccessor> pocoAccessors;
		private readonly IPropertyAccessor propertyAccessor;
		private readonly HashSet<IPocoTransformerResult> pocoTransformers;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="transformer"></param>
		public PocoResolver(Func<object, System.Type, object> transformer)
		{
			if (transformer == null)
				throw new ArgumentNullException("transformer", "The delegate which serves for transforming instances cannot be null.");

			this.transformer = transformer;
			this.pocoTransformers = new HashSet<IPocoTransformerResult>();
			this.pocoAccessors = new HashSet<IPocoAccessor>();
			this.propertyAccessor =
				new ChainedPropertyAccessor(new[]
												{
													PropertyAccessorFactory.GetPropertyAccessor(null),
													PropertyAccessorFactory.GetPropertyAccessor("field")
												});
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TSource"></typeparam>
		/// <typeparam name="TOutput"></typeparam>
		/// <param name="source"></param>
		/// <returns></returns>
		public TOutput TransformFrom<TSource, TOutput>(TSource source)
			where TSource : class
			where TOutput : class
		{
			if (source == null)
				return null;

			return this.transformer.Invoke(source, typeof(TOutput)) as TOutput;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TOutput"></typeparam>
		/// <returns></returns>
		public IPocoTransformerResult GetTransformer<TOutput>()
			where TOutput : class
		{
			return this.GetTransformer(typeof(TOutput));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public IPocoTransformerResult GetTransformer(System.Type type)
		{
			IPocoTransformerResult transformer = pocoTransformers.FirstOrDefault(n => n.ResultType == type);
			if (transformer == null)
			{
				transformer = new PocoTransformerResult(type, this.TryToTransform, this.GetPocoAccessor);
				pocoTransformers.Add(transformer);
			}

			return transformer;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <param name="instance"></param>
		/// <returns></returns>
		public object TryToTransform(System.Type type, object instance)
		{
            if (type == null || instance == null)
                return null;
			
			return this.transformer.Invoke(instance, type);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public IPocoAccessor GetPocoAccessor(System.Type type)
		{
			IPocoAccessor accessor = this.pocoAccessors.FirstOrDefault(n => n.PocoType == type);
			if (accessor == null)
			{
				accessor = new PocoAccessor(this.propertyAccessor, type);
				this.pocoAccessors.Add(accessor);
			}

			return accessor;
		}

	}

	
}