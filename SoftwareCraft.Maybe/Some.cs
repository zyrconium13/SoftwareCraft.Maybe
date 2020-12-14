using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareCraft.Functional
{
    public sealed class Some<T> : Maybe<T>
    {
        internal Some(T value)
            : base(value)
        {
        }

        public override bool IsSome => true;

        public override bool IsNone => false;

        public override Maybe<U> Select<U>(Func<T, U> some, Func<U> none)
        {
            if (some == null) throw new ArgumentNullException(nameof(some));
            if (none == null) throw new ArgumentNullException(nameof(none));

            return Select(some);
        }

        public override Maybe<U> Select<U>(Func<T, U> some)
        {
            if (some == null) throw new ArgumentNullException(nameof(some));

            return new Some<U>(some(Items[0]));
        }

        public override async Task<Maybe<U>> SelectAsync<U>(Func<T, Task<U>> some, Func<Task<U>> none)
        {
            if (some == null) throw new ArgumentNullException(nameof(some));
            if (none == null) throw new ArgumentNullException(nameof(none));

            return await SelectAsync(some);
        }

        public override async Task<Maybe<U>> SelectAsync<U>(Func<T, Task<U>> some)
        {
            if (some == null) throw new ArgumentNullException(nameof(some));

            var someValue = await some(Items[0]);

            return new Some<U>(someValue);
        }

        public override Maybe<U> SelectMany<U>(Func<T, Maybe<U>> some, Func<Maybe<U>> none)
        {
            if (some == null) throw new ArgumentNullException(nameof(some));
            if (none == null) throw new ArgumentNullException(nameof(none));

            return some(Items[0]);
        }

        public override Maybe<U> SelectMany<U>(Func<T, Maybe<U>> some)
        {
            if (some == null) throw new ArgumentNullException(nameof(some));

            return some(Items[0]);
        }

        public override async Task<Maybe<U>> SelectManyAsync<U>(Func<T, Task<Maybe<U>>> some, Func<Task<Maybe<U>>> none)
        {
            if (some == null) throw new ArgumentNullException(nameof(some));
            if (none == null) throw new ArgumentNullException(nameof(none));

            return await some(Items[0]);
        }

        public override async Task<Maybe<U>> SelectManyAsync<U>(Func<T, Task<Maybe<U>>> some)
        {
            if (some == null) throw new ArgumentNullException(nameof(some));

            return await some(Items[0]);
        }

        public override void Match(Action<T> some, Action none)
        {
            if (some == null) throw new ArgumentNullException(nameof(some));
            if (none == null) throw new ArgumentNullException(nameof(none));

            some(Items[0]);
        }

        public override TOut Match<TOut>(Func<T, TOut> some, Func<TOut> none) => some(Items[0]);

        public override string ToString() => Items[0].ToString();
    }
}