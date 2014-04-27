using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Cfg;
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
        [ExpectedException(typeof(ObjectDisposedException))]
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
    }
}
