using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Criterion;
using NHibernate.Extra;
using NUnit.Framework;
using TheHunter.Domain;

namespace NHibernate.Integration.Test
{
    [TestFixture]
    public class NhExtensions
        : NhTestBase
    {
        [Test]
        public void TestAddCriterions()
        {
            IList<ICriterion> par = new List<ICriterion>();
            par.Add(Restrictions.Eq("Name", "Pipo"));
            par.Add(Restrictions.Eq("ID", 1000L));
            DetachedCriteria criteria = DetachedCriteria.For<Salesman>()
                .Add(par)
                ;
            
            using (ISession session = SessionFactory.OpenSession())
            {
                var res = criteria.GetExecutableCriteria(session).List<Salesman>();
                Assert.AreEqual(res.Count, 0);
            }
        }
    }
}
