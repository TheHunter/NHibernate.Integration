using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using NHibernate.Linq;
using NUnit.Framework;
using TheHunter.Domain;
using Configuration = NHibernate.Cfg.Configuration;

namespace NHibernate.Integration.Test
{
    [TestFixture]
    public class NhTestBase
    {
        private ISessionFactory sessionFactory;
        private static string rootPath;

        [TestFixtureSetUp]
        protected void OnStartUp()
        {
            rootPath = Path.GetDirectoryName(
                Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory));

            XmlTextReader configReader = new XmlTextReader(new MemoryStream(Properties.Resources.Configuration));
            Configuration cfg = new Configuration();
            cfg.Configure(configReader);
            cfg.AddDirectory(new DirectoryInfo(Path.Combine(rootPath, "Hbm")));


            string strConnection = string.Format(ConfigurationManager.ConnectionStrings["dbc2"].ConnectionString, rootPath);
            cfg.SetProperty("connection.connection_string", strConnection);

            if (BeforeBuilding != null)
                this.BeforeBuilding.Invoke(cfg);
            
            sessionFactory = cfg.BuildSessionFactory();
        }

        /// <summary>
        /// 
        /// </summary>
        protected Action<Configuration> BeforeBuilding { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected ISessionFactory SessionFactory
        {
            get { return sessionFactory; }
        }

    }
}
