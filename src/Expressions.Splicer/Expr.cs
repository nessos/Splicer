using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Nessos.Expressions.Splicer
{
	public static class Expr
	{
		public static Expression<Func<T, bool>> True<T>()
		{
			return _ => true;
		}

		public static Expression<Func<T, bool>> False<T>()
		{
			return _ => false;
		}

		public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
		{
			return x => left.Invoke(x) || right.Invoke(x);
		}

		public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
		{
			return x => left.Invoke(x) && right.Invoke(x);
		}

		public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expr)
		{
			return x => !expr.Invoke(x);
		}

	}
}
