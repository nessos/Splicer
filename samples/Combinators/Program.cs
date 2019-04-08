using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Nessos.Expressions.Splicer;


namespace Nessos.Sample
{
	class Program
	{
		
		static Expression<Func<int, int>> Pow(int n)
		{
			if (n <= 0)
				return _ => 1;
			else
			{
				var f = Pow(n - 1);
				return x => x * f.Invoke(x);
			}
		}

		public void OrExample()
		{
			var ors =
				Expr.False<string>()
					.Or(x => x.Length > 10)
					.Or(x => x.ToUpper() == "Hello")
					.Splice();
			Console.WriteLine(ors);
		}

		static void Main(string[] args)
		{
			Func<Expression<Func<IEnumerable<int>>>, Expression<Func<int>>> f =
				source => source.Of()
								.Where(x => x % 2 == 0)
								.Select(x => 2 * x)
								.Sum();
			
			var _f = f.Compile();
			var arr = Enumerable.Range(1, 100).ToArray();
			Console.WriteLine(_f(arr));
		}
	}
}
