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

        internal abstract T Value { get; }

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

        public abstract Task MatchAsync(Func<T, Task> some, Func<Task> none);

        public abstract TOut Match<TOut>(Func<T, TOut> some, Func<TOut> none);

        public abstract Task<TOut> MatchAsync<TOut>(Func<T, Task<TOut>> some, Func<Task<TOut>> none);

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

        public static Maybe<Tuple<T1, T2>> LiftLazy<T1, T2>(
            Func<Maybe<T1>> f1,
            Func<Maybe<T2>> f2)
        {
            return f1().SelectMany(
                t1 => f2().SelectMany(
                    t2 => Maybe.Some(new Tuple<T1, T2>(t1, t2))));
        }

        public static async Task<Maybe<Tuple<T1, T2>>> LiftLazyAsync<T1, T2>(
            Func<Task<Maybe<T1>>> f1,
            Func<Task<Maybe<T2>>> f2)
        {
            return await (await f1()).SelectManyAsync(
                async t1 => (await f2()).Select(
                    t2 => new Tuple<T1, T2>(t1, t2)));
        }

        public static Maybe<Tuple<T1, T2, T3>> LiftLazy<T1, T2, T3>(
            Func<Maybe<T1>> f1,
            Func<Maybe<T2>> f2,
            Func<Maybe<T3>> f3)
        {
            return f1().SelectMany(
                t1 => f2().SelectMany(
                    t2 => f3().SelectMany(
                        t3 => Maybe.Some(new Tuple<T1, T2, T3>(t1, t2, t3)))));
        }

        public static async Task<Maybe<Tuple<T1, T2, T3>>> LiftLazyAsync<T1, T2, T3>(
            Func<Task<Maybe<T1>>> f1,
            Func<Task<Maybe<T2>>> f2,
            Func<Task<Maybe<T3>>> f3)
        {
            return await (await f1()).SelectManyAsync(
                async t1 => await (await f2()).SelectManyAsync(
                    async t2 => (await f3()).Select(
                        t3 => new Tuple<T1, T2, T3>(t1, t2, t3))));
        }

        public static Maybe<Tuple<T1, T2, T3, T4>> LiftLazy<T1, T2, T3, T4>(
            Func<Maybe<T1>> f1,
            Func<Maybe<T2>> f2,
            Func<Maybe<T3>> f3,
            Func<Maybe<T4>> f4)
        {
            return f1().SelectMany(
                t1 => f2().SelectMany(
                    t2 => f3().SelectMany(
                        t3 => f4().SelectMany(
                            t4 => Maybe.Some(new Tuple<T1, T2, T3, T4>(t1, t2, t3, t4))))));
        }

        public static async Task<Maybe<Tuple<T1, T2, T3, T4>>> LiftLazyAsync<T1, T2, T3, T4>(
            Func<Task<Maybe<T1>>> f1,
            Func<Task<Maybe<T2>>> f2,
            Func<Task<Maybe<T3>>> f3,
            Func<Task<Maybe<T4>>> f4)
        {
            return await (await f1()).SelectManyAsync(
                async t1 => await (await f2()).SelectManyAsync(
                    async t2 => await (await f3()).SelectManyAsync(
                        async t3 => (await f4()).Select(
                            t4 => new Tuple<T1, T2, T3, T4>(t1, t2, t3, t4)))));
        }

        public static Maybe<Tuple<T1, T2, T3, T4, T5>> LiftLazy<T1, T2, T3, T4, T5>(
            Func<Maybe<T1>> f1,
            Func<Maybe<T2>> f2,
            Func<Maybe<T3>> f3,
            Func<Maybe<T4>> f4,
            Func<Maybe<T5>> f5)
        {
            return f1().SelectMany(
                t1 => f2().SelectMany(
                    t2 => f3().SelectMany(
                        t3 => f4().SelectMany(
                            t4 => f5().SelectMany(
                                t5 => Maybe.Some(new Tuple<T1, T2, T3, T4, T5>(t1, t2, t3, t4, t5)))))));
        }

        public static async Task<Maybe<Tuple<T1, T2, T3, T4, T5>>> LiftLazyAsync<T1, T2, T3, T4, T5>(
            Func<Task<Maybe<T1>>> f1,
            Func<Task<Maybe<T2>>> f2,
            Func<Task<Maybe<T3>>> f3,
            Func<Task<Maybe<T4>>> f4,
            Func<Task<Maybe<T5>>> f5)
        {
            return await (await f1()).SelectManyAsync(
                async t1 => await (await f2()).SelectManyAsync(
                    async t2 => await (await f3()).SelectManyAsync(
                        async t3 => await (await f4()).SelectManyAsync(
                            async t4 => (await f5()).Select(
                                t5 => new Tuple<T1, T2, T3, T4, T5>(t1, t2, t3, t4, t5))))));
        }
    }
}