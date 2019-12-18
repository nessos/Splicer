namespace Nessos.Sample
{
	using System;
	using System.Linq.Expressions;

	using Kritikos.Expressions.Deconstructor;
	using Nessos.Expressions.Splicer;

	public static class Optimizer
	{
		public static Expression Optimize(this Expression expr)
		{
			if (expr == null)
			{
				throw new ArgumentNullException(nameof(expr));
			}

			var optimized =
				expr.Match<Expression>((e, k) => e switch
				{
					#region Simple Math

					BinaryExpression(ExpressionType.Add, ConstantExpression(_, 0), var r) => k(r),
					BinaryExpression(ExpressionType.Add, var l, ConstantExpression(_, 0)) => k(l),
					BinaryExpression(ExpressionType.Multiply, ConstantExpression(_, 1), var r) => k(r),
					BinaryExpression(ExpressionType.Multiply, var l, ConstantExpression(_, 1)) => k(l),
					BinaryExpression(ExpressionType.Divide, var r, ConstantExpression(_, 1)) => k(r),

					#endregion Simple Math

					#region Bool

					BinaryExpression(ExpressionType.And, ConstantExpression(_, true), var r) => k(r),
					BinaryExpression(ExpressionType.And, ConstantExpression(_, false), _) => Expression.Constant(false),
					BinaryExpression(ExpressionType.And, var l, ConstantExpression(_, true)) => k(l),
					BinaryExpression(ExpressionType.And, _, ConstantExpression(_, false)) => Expression.Constant(false),

					BinaryExpression(ExpressionType.Or, ConstantExpression(_, true), _) => Expression.Constant(true),
					BinaryExpression(ExpressionType.Or, ConstantExpression(_, false), var r) => k(r),
					BinaryExpression(ExpressionType.Or, _, ConstantExpression(_, true)) => Expression.Constant(true),
					BinaryExpression(ExpressionType.Or, var l, ConstantExpression(_, false)) => k(l),

					#endregion Bool

					_ => k(e),
				});

			return expr.ToString() == optimized.ToString()
				? expr
				: expr.Optimize();
		}
	}
}
