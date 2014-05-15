using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Criterion;

namespace NHibernate.Extra
{
    /// <summary>
    /// 
    /// </summary>
    public static class NhExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="orders"></param>
        /// <returns></returns>
        public static DetachedCriteria AddOrder(this DetachedCriteria criteria, IEnumerable<Order> orders)
        {
            if (orders != null)
            {
                foreach (var order in orders)
                {
                    criteria.AddOrder(order);
                }
            }
            return criteria;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="criterions"></param>
        /// <returns></returns>
        public static DetachedCriteria Add(this DetachedCriteria criteria, IEnumerable<ICriterion> criterions)
        {
            if (criterions != null)
            {
                foreach (var current in criterions)
                {
                    criteria.Add(current);
                }
            }
            return criteria;
        }
    }
}
