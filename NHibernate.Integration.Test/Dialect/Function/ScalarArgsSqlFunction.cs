using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.Type;

namespace NHibernate.Dialect.Function
{
    /// <summary>
    /// 
    /// </summary>
    public class ScalarArgsSqlFunction
         : ISQLFunction
    {
        private readonly string nameFunction;
        private readonly string begin;
        private readonly string end;
        private const string sep = ",";
        private readonly IType returnType;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameFunction"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        public ScalarArgsSqlFunction(string nameFunction, string begin, string end)
            :this(nameFunction, begin, end, null)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameFunction"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <param name="returnType"></param>
        public ScalarArgsSqlFunction(string nameFunction, string begin, string end, IType returnType)
        {
            this.nameFunction = nameFunction;
            this.begin = begin;
            this.end = end;
            this.returnType = returnType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnType"></param>
        /// <param name="mapping"></param>
        /// <returns></returns>
        public IType ReturnType(IType columnType, IMapping mapping)
        {
            return this.returnType ?? columnType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        public SqlString Render(IList args, ISessionFactoryImplementor factory)
        {
            SqlStringBuilder buffer = new SqlStringBuilder();
            buffer.Add(this.nameFunction);
            buffer.Add(begin);

            for (int i = 0; i < args.Count; i++)
            {
                buffer.AddObject(args[i]);
                if (i < args.Count - 1)
                    buffer.Add(sep);
            }
            buffer.Add(end);
            return buffer.ToSqlString();
        }

        /// <summary>
        /// 
        /// </summary>
        public bool HasArguments
        {
            get { return true; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool HasParenthesesIfNoArguments
        {
            get { return true; }            
        }
    }
}
