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

        public override Maybe<TU> Select<TU>(Func<T, TU> some, Func<TU> none)
        {
            if (some == null) throw new ArgumentNullException(nameof(some));
            if (none == null) throw new ArgumentNullException(nameof(none));

            return Select(some);
        }

        public override Maybe<TU> Select<TU>(Func<T, TU> some)
        {
            if (some == null) throw new ArgumentNullException(nameof(some));

            return new Some<TU>(some(Items[0]));
        }

        public override Task<Maybe<TU>> SelectAsync<TU>(Func<T, Task<TU>> some, Func<Task<TU>> none)
        {
            if (some == null) throw new ArgumentNullException(nameof(some));
            if (none == null) throw new ArgumentNullException(nameof(none));

            return SelectAsync(some);
        }

        public override async Task<Maybe<TU>> SelectAsync<TU>(Func<T, Task<TU>> some)
        {
            if (some == null) throw new ArgumentNullException(nameof(some));

            var someValue = await some(Items[0]);

            return new Some<TU>(someValue);
        }

        public override Maybe<TU> SelectMany<TU>(Func<T, Maybe<TU>> some, Func<Maybe<TU>> none)
        {
            if (some == null) throw new ArgumentNullException(nameof(some));
            if (none == null) throw new ArgumentNullException(nameof(none));

            return some(Items[0]);
        }

        public override Maybe<TU> SelectMany<TU>(Func<T, Maybe<TU>> some)
        {
            if (some == null) throw new ArgumentNullException(nameof(some));

            return some(Items[0]);
        }

        public override Task<Maybe<TU>> SelectManyAsync<TU>(Func<T, Task<Maybe<TU>>> some, Func<Task<Maybe<TU>>> none)
        {
            if (some == null) throw new ArgumentNullException(nameof(some));
            if (none == null) throw new ArgumentNullException(nameof(none));

            return SelectManyAsync(some);
        }

        public override Task<Maybe<TU>> SelectManyAsync<TU>(Func<T, Task<Maybe<TU>>> some)
        {
            if (some == null) throw new ArgumentNullException(nameof(some));

            return some(Items[0]);
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