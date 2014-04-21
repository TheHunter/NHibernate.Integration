using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Linq;
using NUnit.Framework;
using TheHunter.Domain;

namespace NHibernate.Integration.Test
{
    public class Queries
        : NhTestBase
    {
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
                //var query = session.CreateQuery("select checksum(sal.Name) hashColumn from Salesman sal");
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

        [Test]
        [Category("Hql Queries")]
        public void Test5()
        {
            using (ISession session = SessionFactory.OpenSession())
            {
                var query = session.CreateQuery("select count(checksum(sal.Name, sal.Surname)) as total from Salesman sal");

                var lista = query.List();
                Assert.IsNotNull(lista);
            }
        }

        [Test]
        [Category("Hql Queries")]
        public void Test6()
        {
            using (ISession session = SessionFactory.OpenSession())
            {
                //var query = session.CreateQuery("select counter(checksum(sal.Name, sal.Surname)) as total from Salesman sal");
                var query = session.CreateQuery("select counter(concat(sal.Name, sal.Surname)) as total from Salesman sal");

                dynamic counter = query.UniqueResult();
                Assert.IsTrue(counter > 0);
            }
        }

        [Test]
        [Category("Hql Queries")]
        public void Test7()
        {
            using (ISession session = SessionFactory.OpenSession())
            {
                var query = session.CreateQuery("select counter(distinct checksum(sal.Name, sal.Surname)) as total from Salesman sal");

                dynamic counter = query.UniqueResult();
                Assert.IsTrue(counter > 0);
            }
        }


        [Test]
        [Category("Hql Queries")]
        public void Test8()
        {
            using (ISession session = SessionFactory.OpenSession())
            {
                var query = session.CreateQuery("select counter(checksum(sal.Name)) as total from Salesman sal");

                dynamic counter = query.UniqueResult();
                Assert.IsTrue(counter > 0);
            }
        }

        /// <summary>
        /// The Test8 is equals to this one, but the unique difference is to return type.
        /// The original error is thrown by HqlParser.cs file, see the method HqlParser.selectFrom_return selectFrom() on calling input.LA(1);
        /// </summary>
        [Test]
        [Category("Hql Queries")]
        public void Test9()
        {
            using (ISession session = SessionFactory.OpenSession())
            {
                var query = session.CreateQuery("select count(checksum(sal.Name)) as total from Salesman sal");

                dynamic counter = query.UniqueResult();
                Assert.IsTrue(counter > 0);
            }
        }

        [Test]
        [Category("Hql Queries")]
        public void Test10()
        {
            using (ISession session = SessionFactory.OpenSession())
            {
                var query = session.CreateQuery("select checksum(sal.Name) as chkName from Salesman sal");

                var list = query.List();
                Assert.IsTrue(list.Count > 0);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        [Category("Hql Queries")]
        public void Test_Bug()
        {
            using (ISession session = SessionFactory.OpenSession())
            {
                var query = session.CreateQuery("select count(sal.Name + sal.Surname) as chkName from Salesman sal");

                long count = query.UniqueResult<long>();
                Assert.Greater(count, 0);
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
    }
}
