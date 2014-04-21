using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Criterion;
using NUnit.Framework;
using TheHunter.Domain;

namespace NHibernate.Integration.Test
{
    /// <summary>
    /// 
    /// </summary>
    public class CriteriaBuilderTest
        :NhTestBase
    {
        [Test]
        public void TestNewInstance()
        {
            ICriteriaBuilder criteriaBuilder = new CriteriaBuilder(SessionFactory.GetClassMetadata);
            var criteria1 = criteriaBuilder.MakeCriteria(null);
            Assert.IsNull(criteria1);

            var criteria2 = criteriaBuilder.MakeCriteria((object)null, (string)null);
            Assert.IsNull(criteria2);

            var criteria3 = criteriaBuilder.MakeCriteria<Salesman>((object)null);
            Assert.IsNull(criteria3);

            var criteria4 = criteriaBuilder.MakeCriteria<Salesman>((object)null, null);
            Assert.IsNull(criteria4);

            

        }

        [Test]
        public void TestCriteriaCompiled()
        {
            ICriteriaBuilder criteriaBuilder = new CriteriaBuilder(SessionFactory.GetClassMetadata);
            Salesman salesman = new Salesman(1);

            DetachedCriteria criteria = criteriaBuilder.MakeCriteria<Salesman>(salesman);

            using (ISession session = SessionFactory.OpenSession())
            {
                var lista = criteria.GetExecutableCriteria(session).List();
                Assert.IsTrue(lista.Count == 1);
            }
        }
    }
}
