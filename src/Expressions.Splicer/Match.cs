using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Nessos.Expressions.Splicer
{
	public static class MatchExtensions
	{

		internal class MatchVisitor<R> : ExpressionVisitor
		{
			private readonly Func<Expression, Func<Expression, R>, R> f;
			public R Result { private set; get; }

			internal MatchVisitor(Func<Expression, Func<Expression, R>, R> f)
			{
				this.f = f;
			}

			public override Expression Visit(Expression node)
			{
				//Console.WriteLine($"Visit {node}");
				Result = f(node, expr =>
				{
					//Console.WriteLine($"k {expr}");
					var _expr = base.Visit(expr);
					if (typeof(R) == typeof(Expression))
						Result = (R)(object)_expr;
					return this.Result;
				});
				if (Result is Expression expr)
				{
					return expr;
				}
				return node;
			}
		}

		public static R Match<R>(this Expression expr, Func<Expression, Func<Expression, R>, R> f)
		{
			var visitor = new MatchVisitor<R>(f);
			var _ = visitor.Visit(expr);
			return visitor.Result;
		}

	}
}
