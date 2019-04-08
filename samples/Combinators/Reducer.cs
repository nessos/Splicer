using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Nessos.Expressions.Splicer;

namespace Nessos.Sample
{
	public abstract class Reducer<T>
	{
		public abstract Expression<Func<R>> Apply<R>(Expression<Func<R, T, R>> f, Expression<Func<R>> seed);
	}

    public class OfReducer<T> : Reducer<T>
    {
        private readonly Expression<Func<IEnumerable<T>>> source;
        public OfReducer(Expression<Func<IEnumerable<T>>> source)
        {
            this.source = source;
        }

        public override Expression<Func<R>> Apply<R>(Expression<Func<R, T, R>> f, Expression<Func<R>> seed)
        {
            return () => source.Invoke().Aggregate(seed.Invoke(), (acc, x) => f.Invoke(acc, x));
        }
    }

	public class MapReducer<T, S> : Reducer<S>
	{
		private readonly Reducer<T> source;
		private readonly Expression<Func<T, S>> mapf;
		public MapReducer(Reducer<T> source, Expression<Func<T, S>> mapf)
		{
			this.source = source;
			this.mapf = mapf;
		}

		public override Expression<Func<R>> Apply<R>(Expression<Func<R, S, R>> f, Expression<Func<R>> seed)
		{
			return source.Apply<R>((acc, x) => f.Invoke(acc, mapf.Invoke(x)), seed);
		}
	}

	public class FilterReducer<T> : Reducer<T>
	{
		private readonly Reducer<T> source;
		private readonly Expression<Func<T, bool>> filterf;
		public FilterReducer(Reducer<T> source, Expression<Func<T, bool>> filterf)
		{
			this.source = source;
			this.filterf = filterf;
		}

		public override Expression<Func<R>> Apply<R>(Expression<Func<R, T, R>> f, Expression<Func<R>> seed)
		{
			return source.Apply<R>((acc, x) => filterf.Invoke(x) ? f.Invoke(acc, x) : acc, seed);
		}
	}

	public static class Reducer
	{
		public static Reducer<T> Of<T>(this Expression<Func<IEnumerable<T>>> source) =>
			new OfReducer<T>(source);

		public static Reducer<S> Select<T, S>(this Reducer<T> source, Expression<Func<T, S>> mapf) =>
			new MapReducer<T, S>(source, mapf);

		public static Reducer<T> Where<T>(this Reducer<T> source, Expression<Func<T, bool>> filterf) =>
			new FilterReducer<T>(source, filterf);

		public static Expression<Func<R>> Aggregate<T, R>(this Reducer<T> source, Expression<Func<R>> seed, Expression<Func<R, T, R>> aggregatef) =>
			source.Apply<R>(aggregatef, seed);

		public static Expression<Func<int>> Sum(this Reducer<int> source) =>
			source.Aggregate(() => 0, (acc, x) => acc + x);
	}
}
