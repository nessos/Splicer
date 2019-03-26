namespace Nessos.Expressions.Splicer
{
	using System;
	using System.Linq.Expressions;

	public static class SplicerHelpers
	{
		/// <summary>
		/// Comparer for nullable DateTime objects that translates properly via LINQ to Entities
		/// </summary>
		/// <returns>LINQ to Entities compatible comparer</returns>
		public static Expression<Func<DateTime?, DateTime?, int>> NullableDateTimeCompareTo()
			=> (src0, src1)
				=> src0 > src1
					? 1
					: src0 == src1
						? 0
						: -1;
	}
}
