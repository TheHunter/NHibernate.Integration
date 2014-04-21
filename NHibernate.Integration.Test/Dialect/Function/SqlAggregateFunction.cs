using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;

namespace NHibernate.Dialect.Function
{
    /// <summary>
    /// 
    /// </summary>
    public class SqlAggregateFunction
        : ISQLFunction, IFunctionGrammar
    {
        private IType returnType = null;
		private readonly string name;
        private readonly bool acceptAsterisk;

		/// <summary>
		/// Initializes a new instance of the StandardSQLFunction class.
		/// </summary>
		/// <param name="name">SQL function name.</param>
		/// <param name="acceptAsterisk">Whether the function accepts an asterisk (*) in place of arguments</param>
		public SqlAggregateFunction(string name, bool acceptAsterisk)
		{
			this.name = name;
			this.acceptAsterisk = acceptAsterisk;
		}

		/// <summary>
		/// Initializes a new instance of the StandardSQLFunction class.
		/// </summary>
		/// <param name="name">SQL function name.</param>
		/// <param name="acceptAsterisk">True if accept asterisk like argument</param>
		/// <param name="typeValue">Return type for the fuction.</param>
        public SqlAggregateFunction(string name, bool acceptAsterisk, IType typeValue)
			: this(name, acceptAsterisk)
		{
			returnType = typeValue;
		}

		#region ISQLFunction Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnType"></param>
        /// <param name="mapping"></param>
        /// <returns></returns>
		public virtual IType ReturnType(IType columnType, IMapping mapping)
		{
			return returnType ?? columnType;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
		public SqlString Render(IList args, ISessionFactoryImplementor factory)
		{
			//ANSI-SQL92 definition
			//<general set function> ::=
			//<set function type> <leftparen> [ <setquantifier> ] <value expression> <right paren>
			//<set function type> : := AVG | MAX | MIN | SUM | COUNT
			//<setquantifier> ::= DISTINCT | ALL

			if (args.Count < 1 || args.Count > 2)
			{
				throw new QueryException(string.Format("Aggregate {0}(): Not enough parameters (attended from 1 to 2).", name));
			}
			else if ("*".Equals(args[args.Count - 1]) && !acceptAsterisk)
			{
				throw new QueryException(string.Format("Aggregate {0}(): invalid argument '*'.", name));
			}
			SqlStringBuilder cmd = new SqlStringBuilder();
			cmd.Add(name)
				.Add("(");
			if (args.Count > 1)
			{
				object firstArg = args[0];
				if (!StringHelper.EqualsCaseInsensitive("distinct", firstArg.ToString()) &&
				    !StringHelper.EqualsCaseInsensitive("all", firstArg.ToString()))
				{
					throw new QueryException(string.Format("Aggregate {0}(): token unknow {1}.", name, firstArg));
				}
				cmd.AddObject(firstArg).Add(" ");
			}
			cmd.AddObject(args[args.Count - 1])
				.Add(")");
			return cmd.ToSqlString();
		}

		#endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		public override string ToString()
		{
			return name;
		}


		#region IFunctionGrammar Members

		bool IFunctionGrammar.IsSeparator(string token)
		{
			return false;
		}

		bool IFunctionGrammar.IsKnownArgument(string token)
		{
			return "distinct".Equals(token.ToLowerInvariant()) ||
				"all".Equals(token.ToLowerInvariant());
		}

		#endregion
    }
}
