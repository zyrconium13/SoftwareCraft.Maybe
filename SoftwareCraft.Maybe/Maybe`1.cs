using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareCraft.Functional
{
    public abstract class Maybe<T> : IEquatable<Maybe<T>>
    {
        protected readonly T[] Items;

        protected Maybe() => Items = new T[0];

        protected Maybe(T value)
        {
            if (null == value) throw new ArgumentNullException();

            Items = new[]
            {
                value
            };
        }

        public abstract bool IsSome { get; }

        public abstract bool IsNone { get; }

        internal T Value
        {
            get
            {
                if (IsSome) return Items[0];

                throw new InvalidOperationException("Trying to call Value on a None.");
            }
        }

        public bool Equals(Maybe<T> other)
        {
            if (null == other) return false;

            switch (Items.Length)
            {
                case 1 when other.Items.Length == 0: // Some equals None?
                case 0 when other.Items.Length == 1: // None equals Some?
                    return false;
                case 0 when other.Items.Length == 0: // None equals None?
                    return true;
                default:
                    return Items[0].Equals(other.Items[0]); // Some equals Some?
            }
        }

        public abstract Maybe<U> Select<U>(Func<T, U> some, Func<U> none);

        public abstract Maybe<U> Select<U>(Func<T, U> some);

        public abstract Task<Maybe<U>> SelectAsync<U>(Func<T, Task<U>> some, Func<Task<U>> none);
        
        public abstract Task<Maybe<U>> SelectAsync<U>(Func<T, Task<U>> some);

        public abstract Maybe<U> SelectMany<U>(Func<T, Maybe<U>> some, Func<Maybe<U>> none);

        public abstract Maybe<U> SelectMany<U>(Func<T, Maybe<U>> some);

        public abstract Task<Maybe<U>> SelectManyAsync<U>(Func<T, Task<Maybe<U>>> some, Func<Task<Maybe<U>>> none);

        public abstract Task<Maybe<U>> SelectManyAsync<U>(Func<T, Task<Maybe<U>>> some);

        public abstract void Match(Action<T> some, Action none);

        public abstract TOut Match<TOut>(Func<T, TOut> some, Func<TOut> none);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Maybe<T>) obj);
        }

        public static bool operator ==(Maybe<T> left, Maybe<T> right) => Equals(left, right);

        public static bool operator !=(Maybe<T> left, Maybe<T> right) => !Equals(left, right);

        public override int GetHashCode() => Items.Length == 0 ? 0 : Items[0].GetHashCode();
    }

    public static class MaybeExtensions
    {
        public static Maybe<T1> Apply<T, T1>(this Maybe<Func<T, T1>> m, Maybe<T> other)
            => m.Match(f => other.Match(o => Maybe.Some(f(o)), Maybe.None<T1>), Maybe.None<T1>);

        public static Maybe<T1> Apply<T, T1>(this Maybe<Func<T, T1>> m, T other)
            => m.Match(f => Maybe.Some(f(other)), Maybe.None<T1>);

        public static bool AllSome<T>(params Maybe<T>[] maybes)
            => maybes.All(x => x.IsSome);

        public static bool AnySome<T>(params Maybe<T>[] maybes)
            => maybes.Any(x => x.IsSome);

        public static bool AllNone<T>(params Maybe<T>[] maybes)
            => maybes.All(x => x.IsNone);

        public static bool AnyNone<T>(params Maybe<T>[] maybes)
            => maybes.Any(x => x.IsNone);

        public static Maybe<Tuple<T1, T2>> Lift<T1, T2>(Maybe<T1> m1, Maybe<T2> m2)
        {
            if (m1.IsSome && m2.IsSome)
                return Maybe.Some(new Tuple<T1, T2>(m1.Value, m2.Value));

            return Maybe.None<Tuple<T1, T2>>();
        }

        public static Maybe<Tuple<T1, T2, T3>> Lift<T1, T2, T3>(Maybe<T1> m1, Maybe<T2> m2, Maybe<T3> m3)
        {
            if (m1.IsSome && m2.IsSome && m3.IsSome)
                return Maybe.Some(new Tuple<T1, T2, T3>(m1.Value, m2.Value, m3.Value));

            return Maybe.None<Tuple<T1, T2, T3>>();
        }

        public static Maybe<Tuple<T1, T2, T3, T4>> Lift<T1, T2, T3, T4>(Maybe<T1> m1, Maybe<T2> m2, Maybe<T3> m3,
            Maybe<T4> m4)
        {
            if (m1.IsSome && m2.IsSome && m3.IsSome && m4.IsSome)
                return Maybe.Some(new Tuple<T1, T2, T3, T4>(m1.Value, m2.Value, m3.Value, m4.Value));

            return Maybe.None<Tuple<T1, T2, T3, T4>>();
        }

        public static Maybe<Tuple<T1, T2, T3, T4, T5>> Lift<T1, T2, T3, T4, T5>(Maybe<T1> m1, Maybe<T2> m2,
            Maybe<T3> m3,
            Maybe<T4> m4, Maybe<T5> m5)
        {
            if (m1.IsSome && m2.IsSome && m3.IsSome && m4.IsSome && m5.IsSome)
                return Maybe.Some(new Tuple<T1, T2, T3, T4, T5>(m1.Value, m2.Value, m3.Value, m4.Value, m5.Value));

            return Maybe.None<Tuple<T1, T2, T3, T4, T5>>();
        }
    }
}