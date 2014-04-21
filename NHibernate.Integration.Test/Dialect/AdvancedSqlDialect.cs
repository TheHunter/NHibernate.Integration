using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Dialect.Function;

namespace NHibernate.Dialect
{
    /// <summary>
    /// 
    /// </summary>
    public class AdvancedSqlDialect
        : MsSql2005Dialect
    {
        /// <summary>
        /// 
        /// </summary>
        public AdvancedSqlDialect()
        {
            AddNewFunctions();
        }

        private void AddNewFunctions()
        {
            //RegisterFunction("checksum", new VarArgsSQLFunction(NHibernateUtil.Int32, "checksum(", ",", ")"));
            RegisterFunction("checksum", new ScalarArgsSqlFunction("checksum", "(", ")", NHibernateUtil.Int32));
            //RegisterFunction("counter", new SqlAggregateFunction("count_big", true, NHibernateUtil.Int64));
            RegisterFunction("counter", new SqlAggregateFunction("count_big", true, NHibernateUtil.Int32));

            //RegisterFunction("counter", new ClassicAggregateFunction("count_big", true));
        }
    }
}
