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

            var criteria3 = criteriaBuilder.MakeCriteria<Salesman>(null);
            Assert.IsNull(criteria3);

            var criteria4 = criteriaBuilder.MakeCriteria<Salesman>(null, null);
            Assert.IsNull(criteria4);
            
        }

        [Test]
        public void TestCriteriaCompiled()
        {
            ICriteriaBuilder criteriaBuilder = new CriteriaBuilder(SessionFactory.GetClassMetadata);
            Salesman instance = new Salesman(1);

            ICriteriaCompiled criteriaCompiled = null;

            criteriaCompiled = criteriaBuilder.MakeCriteria(instance as object);
            Assert.AreEqual(criteriaCompiled.Alias, "salesman");
            Assert.AreEqual(criteriaCompiled.Restrictions.Count(), 1);
            using (ISession session = SessionFactory.OpenSession())
            {
                var lista = criteriaCompiled.Criteria.GetExecutableCriteria(session).List();
                Assert.IsTrue(lista.Count == 1);
            }

            criteriaCompiled = criteriaBuilder.MakeCriteria(instance as object, "sal");
            Assert.AreEqual(criteriaCompiled.Alias, "sal");
            Assert.AreEqual(criteriaCompiled.Restrictions.Count(), 1);
            using (ISession session = SessionFactory.OpenSession())
            {
                var lista = criteriaCompiled.Criteria.GetExecutableCriteria(session).List();
                Assert.IsTrue(lista.Count == 1);
            }

            criteriaCompiled = criteriaBuilder.MakeCriteria(instance);
            Assert.AreEqual(criteriaCompiled.Alias, "salesman");
            Assert.AreEqual(criteriaCompiled.Restrictions.Count(), 1);
            using (ISession session = SessionFactory.OpenSession())
            {
                var lista = criteriaCompiled.Criteria.GetExecutableCriteria(session).List();
                Assert.IsTrue(lista.Count == 1);
            }

            criteriaCompiled = criteriaBuilder.MakeCriteria(instance, "sal");
            Assert.AreEqual(criteriaCompiled.Alias, "sal");
            Assert.AreEqual(criteriaCompiled.Restrictions.Count(), 1);
            using (ISession session = SessionFactory.OpenSession())
            {
                var lista = criteriaCompiled.Criteria.GetExecutableCriteria(session).List();
                Assert.IsTrue(lista.Count == 1);
            }

            criteriaCompiled = criteriaBuilder.MakeCriteria(typeof(Salesman), instance);
            Assert.AreEqual(criteriaCompiled.Alias, "salesman");
            Assert.AreEqual(criteriaCompiled.Restrictions.Count(), 1);
            using (ISession session = SessionFactory.OpenSession())
            {
                var lista = criteriaCompiled.Criteria.GetExecutableCriteria(session).List();
                Assert.IsTrue(lista.Count == 1);
            }

            criteriaCompiled = criteriaBuilder.MakeCriteria(typeof(Salesman), instance, "sal");
            Assert.AreEqual(criteriaCompiled.Alias, "sal");
            Assert.AreEqual(criteriaCompiled.Restrictions.Count(), 1);
            using (ISession session = SessionFactory.OpenSession())
            {
                var lista = criteriaCompiled.Criteria.GetExecutableCriteria(session).List();
                Assert.IsTrue(lista.Count == 1);
            }
        }

        [Test]
        public void TestCriteriaCompiled2()
        {
            ICriteriaCompiled criteriaCompiled = null;
            ICriteriaBuilder criteriaBuilder = new CriteriaBuilder(SessionFactory.GetClassMetadata);
            CarContract instance = new CarContract
                {
                    Owner = new Salesman(1)
                };

            criteriaCompiled = criteriaBuilder.MakeCriteria(instance);
            Assert.AreEqual(criteriaCompiled.Alias, "carcontract");
            Assert.AreEqual(criteriaCompiled.Restrictions.Count(), 1);
            using (ISession session = SessionFactory.OpenSession())
            {
                var lista = criteriaCompiled.Criteria.GetExecutableCriteria(session).List();
                Assert.AreEqual(lista.Count, 4);
            }

            criteriaCompiled = criteriaBuilder.MakeCriteria(typeof(CarContract), instance);
            Assert.AreEqual(criteriaCompiled.Alias, "carcontract");
            Assert.AreEqual(criteriaCompiled.Restrictions.Count(), 1);
            using (ISession session = SessionFactory.OpenSession())
            {
                var lista = criteriaCompiled.Criteria.GetExecutableCriteria(session).List();
                Assert.AreEqual(lista.Count, 4);
            }

            criteriaCompiled = criteriaBuilder.MakeCriteria<TradeContract>(instance);
            Assert.AreEqual(criteriaCompiled.Alias, "tradecontract");
            Assert.AreEqual(criteriaCompiled.Restrictions.Count(), 1);
            using (ISession session = SessionFactory.OpenSession())
            {
                var lista = criteriaCompiled.Criteria.GetExecutableCriteria(session).List();
                Assert.AreEqual(lista.Count, 10);
            }
        }
    }
}
