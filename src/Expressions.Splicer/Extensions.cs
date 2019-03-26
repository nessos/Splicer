namespace Nessos.Expressions.Splicer
{
	using System;
	using System.Linq;
	using System.Linq.Expressions;

	public static class Extensions
	{
		private static readonly Splicer Splicer = new Splicer();

		public static void Deconstruct(
			this MethodCallExpression m,
			out string name,
			out Expression func,
			out Expression[] args)
		{
			name = m.Method.Name;
			if (name == "Invoke")
			{
				var thunk = Expression.Lambda<Func<object>>(m.Arguments[0], new ParameterExpression[0]);
				func = thunk.Compile().DynamicInvoke() as Expression;
			}
			else
			{
				func = m.Arguments[0];
			}

			args = Enumerable.Range(1, m.Arguments.Count - 1).Select(i => m.Arguments[i]).ToArray();
		}

		public static TResult Invoke<TSource, TResult>(this Expression<Func<TSource, TResult>> f, TSource src)
			=> throw new Exception("Stump call");

		public static TResult Invoke<TSource0, TSource1, TResult>(
			this Expression<Func<TSource0, TSource1, TResult>> f,
			TSource0 src0,
			TSource1 src1)
			=> throw new Exception("Stump call");

		public static Expression<Func<TSource, TResult>> Splice<TSource, TResult>(
			this Expression<Func<TSource, TResult>> lambda)
			=> Splicer.Visit(lambda) as Expression<Func<TSource, TResult>>;
	}
}
