using System;
using Nessos.Expressions.Splicer;

namespace Combinators
{
	class Program
	{
		static void Main(string[] args)
		{
			var ors = 
				Expr.False<string>()
					.Or(x => x.Length > 10)
					.Or(x => x.ToUpper() == "Hello")
					.Splice();

			Console.WriteLine(ors);
			
		}
	}
}
