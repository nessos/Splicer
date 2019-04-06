using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nessos.Sample
{
	public abstract class Reducer<T>
	{
		public abstract R Apply<R>(Func<R, T, R> f, R seed);
	}

    public class OfReducer<T> : Reducer<T>
    {
        private readonly IEnumerable<T> source;
        public OfReducer(IEnumerable<T> source)
        {
            this.source = source;
        }

        public override R Apply<R>(Func<R, T, R> f, R seed)
        {
            return source.Aggregate(seed, f);
        }
    }

    public class MapReducer<T, S> : Reducer<S>
    {
        private readonly Reducer<T> source;
        private readonly Func<T, S> mapf;
        public MapReducer(Reducer<T> source, Func<T, S> mapf)
        {
            this.source = source;
            this.mapf = mapf;
        }

        public override R Apply<R>(Func<R, S, R> f, R seed)
        {
            return source.Apply<R>((acc, x) => f(acc, mapf(x)), seed);
        }
    }

    public class FilterReducer<T> : Reducer<T>
    {
        private readonly Reducer<T> source;
        private readonly Func<T, bool> filterf;
        public FilterReducer(Reducer<T> source, Func<T, bool> filterf)
        {
            this.source = source;
            this.filterf = filterf;
        }

        public override R Apply<R>(Func<R, T, R> f, R seed)
        {
            return source.Apply<R>((acc, x) => filterf(x) ? f(acc, x) : acc, seed);
        }
    }

    public static class Reducer
    {
        public static Reducer<T> Of<T>(this IEnumerable<T> source) => 
            new OfReducer<T>(source);


        public static Reducer<S> Select<T, S>(this Reducer<T> source, Func<T, S> mapf) =>
            new MapReducer<T, S>(source, mapf);

        public static Reducer<T> Where<T>(this Reducer<T> source, Func<T, bool> filterf) =>
            new FilterReducer<T>(source, filterf);

        public static R Aggregate<T, R>(this Reducer<T> source, R seed, Func<R, T, R> aggregatef) =>
            source.Apply<R>(aggregatef, seed);

        public static int Sum(this Reducer<int> source) =>
            source.Aggregate(0, (acc, x) => acc + x);
    }
}
