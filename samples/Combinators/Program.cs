using System;
using System.Linq.Expressions;
using Nessos.Expressions.Splicer;

namespace Combinators
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

			Console.WriteLine(Pow(3).Splice());
			
			
		}
	}
}
