using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Cfg;
using NHibernate.Criterion;
using NHibernate.Dialect.Function;
using NHibernate.Hql.Ast.ANTLR;
using NHibernate.Linq;
using NUnit.Framework;
using TheHunter.Domain;

namespace NHibernate.Integration.Test
{
    public class Queries
        : NhTestBase
    {
        public Queries()
        {
            this.BeforeBuilding = delegate(Configuration configuration)
                {
                    configuration.AddSqlFunction("checksum", new ScalarArgsSqlFunction("checksum", "(", ")", NHibernateUtil.Int32));
                    configuration.AddSqlFunction("counter", new SqlAggregateFunction("count_big", true, NHibernateUtil.Int32));
                };
        }

        [Test]
        [Category("All")]
        public void Test1()
        {
            using (ISession session = SessionFactory.OpenSession())
            {
                var instance1 = session.Load<Salesman>(1L);
                var instance2 = session.Load<Agency>(1L);
                var instance3 = session.Query<TradeContract>().ToList();

                Assert.IsNotNull(instance1);
                Assert.IsNotNull(instance2);
                Assert.IsNotNull(instance3);
            }
        }

        [Test]
        [Category("Hql Queries")]
        public void Test2()
        {
            using (ISession session = SessionFactory.OpenSession())
            {
                var query = session.CreateQuery("from Salesman sal");

                var lista = query.List();
                Assert.IsNotNull(lista);

            }
        }

        [Test]
        [Category("Hql Queries")]
        public void Test3()
        {
            using (ISession session = SessionFactory.OpenSession())
            {
                var query = session.CreateQuery("from Salesman sal");

                var lista = query.List();
                Assert.IsNotNull(lista);

            }
        }

        [Test]
        [Category("Hql Queries")]
        public void Test4()
        {
            using (ISession session = SessionFactory.OpenSession())
            {
                var query = session.CreateQuery("select checksum(sal.Name, sal.Surname) as hashColumn from Salesman sal");

                var lista = query.List();
                Assert.IsNotNull(lista);
            }
        }
        
        /// <summary>
        /// throws QuerySyntaxException, this is a bug which Nhibernate team could fix on the future..
        /// </summary>
        [Test]
        [Category("Hql Queries")]
        [ExpectedException(typeof(QuerySyntaxException))]
        public void Test5()
        {
            using (ISession session = SessionFactory.OpenSession())
            {
                var query = session.CreateQuery("select count(checksum(sal.Name, sal.Surname)) as total from Salesman sal");

                var lista = query.List();
                Assert.IsNotNull(lista);
            }
        }

        /// <summary>
        /// With my custom function( counter() ) this test doesn't fail, but I don't understand Why this happens,
        /// because my custom function It's almost identical than nhibernate count() function.
        /// </summary>
        [Test]
        [Category("Hql Queries")]
        public void Test6()
        {
            using (ISession session = SessionFactory.OpenSession())
            {
                var query = session.CreateQuery("select counter(checksum(sal.Name, sal.Surname)) as total from Salesman sal");

                dynamic counter = query.UniqueResult();
                Assert.IsTrue(counter > 0);
            }
        }

        /// <summary>
        /// Using an aggregation function with the distinct keyword and an any kind of scalar function as parameter
        /// throws an exception... this is a bug which I hope it will be fixed.
        /// </summary>
        [Test]
        [Category("Hql Queries")]
        [ExpectedException(typeof(QuerySyntaxException))]
        public void Test7()
        {
            using (ISession session = SessionFactory.OpenSession())
            {
                var query = session.CreateQuery("select counter(distinct checksum(sal.Name, sal.Surname)) as total from Salesman sal");

                dynamic counter = query.UniqueResult();
                Assert.IsTrue(counter > 0);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Category("Hql Queries")]
        public void Test8()
        {
            using (ISession session = SessionFactory.OpenSession())
            {
                var query = session.CreateQuery("select counter(concat(sal.Name, sal.Surname)) as total from Salesman sal");

                dynamic counter = query.UniqueResult();
                Assert.IsTrue(counter > 0);
            }
        }

        [Test]
        [Category("Hql Queries")]
        public void Test9()
        {
            using (ISession session = SessionFactory.OpenSession())
            {
                var query = session.CreateQuery("select checksum(sal.Name) as chkName from Salesman sal");

                var list = query.List();
                Assert.IsTrue(list.Count > 0);
            }
        }

        [Test]
        [Category("Sql Queries")]
        public void TestSql1()
        {
            using (ISession session = SessionFactory.OpenSession())
            {
                var query = session.CreateSQLQuery("select checksum(sal.Name) hashColumn from Salesman sal");

                var lista = query.List();
                Assert.IsNotNull(lista);
            }
        }

        [Test]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void TestNh1()
        {
            ISession session = SessionFactory.OpenSession();
            using (session)
            {
                var result = session.CreateQuery("from Salesman")
                    .List<Salesman>();

                Assert.IsNotNull(result);
            }
            session.BeginTransaction();
        }

        [Test]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void TestNh2()
        {
            ISession session = SessionFactory.OpenSession();

            var tran = session.BeginTransaction();
            tran.Commit();
            tran.Commit();              /* throws ObjectDisposedException */
        }

        [Test]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void TestNh3()
        {
            ISession session = SessionFactory.OpenSession();

            var tran = session.BeginTransaction();
            tran.Commit();
            tran.Rollback();            /* throws ObjectDisposedException */
        }

        [Test]
        [ExpectedException(typeof(TransactionException))]
        public void TestNh4()
        {
            ISession session = SessionFactory.OpenSession();
            using (session)
            {
                
            }
            var tran = session.Transaction;
            tran.Commit();                          /* throws ObjectDisposedException */
        }

        [Test]
        public void EqualsSessions()
        {
            ISession session1 = SessionFactory.OpenSession();
            ISession session2 = SessionFactory.OpenSession();

            Assert.AreNotEqual(session1, session2);

            ISession session3 = SessionFactory.OpenSession();
            ISession session4 = session3.GetSession(EntityMode.Poco);
            Assert.AreNotEqual(session3, session4);

        }

        [Test]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void FailedTransactionAfterDispose()
        {
            ISession session1 = SessionFactory.OpenSession();
            var tran = session1.BeginTransaction();
            session1.Dispose();

            Assert.IsFalse(tran.IsActive);
            Assert.IsFalse(session1.IsOpen);
            tran.Rollback();                    /* throws ObjectDisposedException */
        }

        [Test]
        public void SelectTop1()
        {
            using (ISession session1 = SessionFactory.OpenSession())
            {
                var query = session1.CreateQuery("select sal from Salesman sal take 1");
                var result = query.UniqueResult();

                Assert.IsNotNull(result);
            }
        }

        [Test]
        public void SelectUnique()
        {
            using (ISession session1 = SessionFactory.OpenSession())
            {
                var criteria = DetachedCriteria.For<Salesman>().Add(Restrictions.IdEq(1L));
                var result = criteria.GetExecutableCriteria(session1).UniqueResult();
                Assert.IsNotNull(result);
            }

            using (ISession session1 = SessionFactory.OpenSession())
            {
                var criteria = QueryOver.Of<Salesman>().And(Restrictions.IdEq(1L));
                var result = criteria.GetExecutableQueryOver(session1)
                                     .UnderlyingCriteria
                                     .UniqueResult();
                Assert.IsNotNull(result);
            }

            using (ISession session1 = SessionFactory.OpenSession())
            {
                var query = session1.Query<Salesman>();
                var result = query.First(salesman => salesman.ID == 1);
                Assert.IsNotNull(result);
            }
        }

        [Test]
        [ExpectedException(typeof(NonUniqueResultException))]
        public void WrongSelectUnique1()
        {
            using (ISession session1 = SessionFactory.OpenSession())
            {
                var criteria = DetachedCriteria.For<Salesman>().Add(Restrictions.Eq("Name", "Manuel"));
                var result = criteria.GetExecutableCriteria(session1).UniqueResult();
                Assert.IsNotNull(result);
            }
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public void WrongSelectUnique2()
        {
            using (ISession session1 = SessionFactory.OpenSession())
            {
                var query = session1.Query<Salesman>();
                var res1 = query.Where(salesman => salesman.Name == "Manuel");

                var result = query.First(salesman => salesman.Name == "Manuel");

                if (result == null)
                    throw new Exception("Result cannot be null");

                if (res1.Count() > 1)
                    throw new Exception("Query result must throw an exception because the result is bigger than one instance.");
            }
        }

        //[Test]
        //[ExpectedException(typeof(NonUniqueResultException))]
        //public void WrongSelectUnique3()
        //{
        //    using (ISession session1 = SessionFactory.OpenSession())
        //    {
        //        var criteria = DetachedCriteria.For<Salesman>().Add(Restrictions.Eq("Name", "Silvio"));
        //        var result = criteria.GetExecutableCriteria(session1).UniqueResult();
        //        Assert.IsNotNull(result);
        //    }
        //}

        [Test]
        public void TestProjection1()
        {
            using (ISession session1 = SessionFactory.OpenSession())
            {
                var query = session1.Query<TradeContract>();
                var result = query.Where(contract => contract.Owner.Name != "Manuel")
                        .Select(contract => contract.Owner)
                        .Distinct()
                        .ToList();
                Assert.IsNotNull(result);
            }
        }

        [Test]
        public void TestProjection2()
        {
            using (ISession session1 = SessionFactory.OpenSession())
            {
                var query = session1.Query<TradeContract>();
                var result = query.Where(contract => contract.Owner.Name != "Manuel");
                Assert.IsNotNull(result);
            }
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestProjection3()
        {
            using (ISession session1 = SessionFactory.OpenSession())
            {
                var query = session1.Query<TradeContract>();
                var result = query.Select(contract => new { contract.Owner.Name, contract.Owner.Surname })
                                  .Distinct()
                                  .ToList()
                                  ;

                Assert.IsNotNull(result);
            }
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestProjection4()
        {
            using (ISession session1 = SessionFactory.OpenSession())
            {
                var query = session1.Query<TradeContract>();
                var result = query.Select(contract => new Tester  { Name=contract.Owner.Name, Surname=contract.Owner.Surname })
                                  .Distinct()
                                  .ToList()
                                  ;

                Assert.IsNotNull(result);
            }
        }

        [Test]
        public void TestLoad1()
        {
            using (ISession session1 = SessionFactory.OpenSession())
            {
                var ret = session1.Load<Salesman>(-1L, LockMode.None);
                Assert.IsNotNull(ret);
            }
        }

        [Test]
        public void TestLoad2()
        {
            using (ISession session1 = SessionFactory.OpenSession())
            {
                var ret = session1.Load<Salesman>(-1L);
                Assert.IsNotNull(ret);
            }
        }
    }

    class Tester
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}

