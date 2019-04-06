using System;
using System.Collections.Generic;
using System.Text;

namespace Nessos.Sample
{
	public abstract class Reducer<T>
	{
		public abstract R Apply<R>(Func<R, T, R> f, R seed);
	}

    public static class Reducer
    {
        public static Reducer<T> Of<T>(this IEnumerable<T> source)
        {
            return null;
        }

        public static Reducer<S> Select<T, S>(this Reducer<T> source, Func<T, S> mapf)
        {
            return null;
        }

        public static Reducer<T> Where<T>(this Reducer<T> source, Func<T, bool> filterf)
        {
            return null;
        }

        public static R Aggregate<T, R>(this Reducer<T> source, R seed, Func<R, T, R> aggregatef)
        {
            return default(R);
        }

        public static int Sum(this Reducer<int> source)
        {
            return 0;
        }
    }
}
