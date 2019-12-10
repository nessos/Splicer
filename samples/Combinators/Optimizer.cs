using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Nessos.Expressions.Splicer;
using Kritikos.Expressions.Deconstructor;

namespace Nessos.Sample
{
	public static class Optimizer
	{
		public static Expression Optimize(this Expression expr)
		{
			var _expr = 
				expr.Match<Expression>((e, k) => e switch
					{
						BinaryExpression (ExpressionType.Add, ConstantExpression (_, 0), var r)  => k(r),
						BinaryExpression(ExpressionType.Add, var l, ConstantExpression(_, 0)) => k(l),
						BinaryExpression(ExpressionType.Multiply, ConstantExpression(_, 1), var r) => k(r),
						BinaryExpression(ExpressionType.Multiply, var l, ConstantExpression(_, 1)) => k(l),
						_ => k(e),
					});

			if (expr.ToString() == _expr.ToString())
				return expr;
			else return _expr.Optimize();
		}
	}
}
