using System;
using System.Collections;
using System.Linq;
using NHibernate.Reflection;

namespace NHibernate.Transform
{
	/// <summary>
	/// 
	/// </summary>
	public class PocoTransformerResult
		: IPocoTransformerResult
	{
		private readonly System.Type resultType;
		private readonly Func<System.Type, object, object> propertyResolver;
		private readonly Func<System.Type, IPocoAccessor> pocoAccessorResolver;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="resultType"></param>
		public PocoTransformerResult(System.Type resultType)
			:this(resultType, null, null)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="resultType"></param>
		/// <param name="propertyResolver"></param>
		/// <param name="pocoAccessorResolver"></param>
		public PocoTransformerResult(System.Type resultType, Func<System.Type, object, object> propertyResolver, Func<System.Type, IPocoAccessor> pocoAccessorResolver)
		{
			if (resultType == null)
				throw new ArgumentNullException("resultType", "The object result resultType cannot be null");

			if (resultType.IsPrimitive || resultType.IsInterface || resultType.IsAbstract)
				throw new ArgumentException("The object result resultType must be an concrete class, so you cannot use an interface/abstract class, neither an primitive resultType", "resultType");

			this.resultType = resultType;
			this.propertyResolver = propertyResolver;
			this.pocoAccessorResolver = pocoAccessorResolver;
		}

		/// <summary>
		/// 
		/// </summary>
		public System.Type ResultType
		{
			get { return this.resultType; }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="collection"></param>
		/// <returns></returns>
		public IList TransformList(IList collection)
		{
			return collection;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="tuple"></param>
		/// <param name="aliases"></param>
		/// <returns></returns>
		public object TransformTuple(object[] tuple, string[] aliases)
		{
			if (aliases == null)
				throw new ArgumentNullException("aliases");

			try
			{
				object instance = null;
				IPocoAccessor accessor = this.pocoAccessorResolver.Invoke(this.resultType);
				if (accessor != null)
				{
					instance = accessor.MakeInstance();
					for (int index = 0; index < aliases.Length; index++)
					{
						string alias = aliases[index];
						object propertyValue = tuple[index];

						IPropertyAction property = accessor.GetPropertyAction(alias, StringComparison.InvariantCultureIgnoreCase);
						if (property != null && propertyValue != null)
						{
							System.Type propType = propertyValue.GetType();
							if (propType.IsPrimitive || property.PropertyType.IsAssignableFrom(propType))
							{
								property.Set(instance, propertyValue);
							}
							else
							{
								object obj = this.propertyResolver.Invoke(property.PropertyType, propertyValue);
								property.Set(instance, obj);
							}

						}
					}
				}

				return instance;
			}
			catch (Exception ex)
			{
				throw new HibernateException(string.Format("Error on initializing the instance, class: {0}: ", resultType.FullName), ex);
			}
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
		public object TransformInstance(object instance)
		{
			if (instance == null) return null;

			int index = -1;
			
			var accessor = this.pocoAccessorResolver.Invoke(instance.GetType());
			if (accessor != null && accessor.PropertyActions.Any())
			{
				int count = accessor.PropertyActions.Count();
				object[] values = new object[count];
				string[] properties = new string[count];

				foreach (var property in accessor.PropertyActions)
				{
					index++;
					properties[index] = property.MemberName;
					values[index] = property.Get(instance);
				}
				return this.TransformTuple(values, properties);
			}

			return null;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return this.resultType.GetHashCode();
		}


	}
}