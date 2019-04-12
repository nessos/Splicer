using System;
using System.Collections.Generic;
using System.Diagnostics;
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

			Func<IEnumerable<int>, int> g = 
				source =>
				source
					.Where(x => x % 2 == 0)
					.Where(x => x % 2 == 0)
					.Where(x => x % 2 == 0)
					.Select(x => x * 2)
					.Select(x => x * 2)
					.Select(x => x * 2)
					.Aggregate(0, (acc, x) => acc + x);

			Func<Expression<Func<IEnumerable<int>>>, Expression<Func<int>>> f =
				source => source.Of()
								.Where(x => x % 2 == 0)
								.Where(x => x % 2 == 0)
								.Where(x => x % 2 == 0)
								.Select(x => x * 2)
								.Select(x => x * 2)
								.Select(x => x * 2)
								.Aggregate(() => 0, (acc, x) => acc + x);
			
			var _f = f.Compile();
			var arr = Enumerable.Range(1, 100000000).ToArray();

			var watch = Stopwatch.StartNew();

			var x0 = _f(arr);

			Console.WriteLine(x0);
			Console.WriteLine(watch.Elapsed);
			watch = Stopwatch.StartNew();

			var x1 = g(arr);

			Console.WriteLine(x1);
			Console.WriteLine(watch.Elapsed);
		}
	}
}
