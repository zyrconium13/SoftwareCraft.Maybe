using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftwareCraft.Functional
{
    public sealed class None<T> : Maybe<T>
    {
        internal None()
        {
        }

        public override bool IsSome => false;

        public override bool IsNone => true;
        internal override T Value => throw new InvalidOperationException("Trying to call Value on a None.");

        public override Maybe<TU> Select<TU>(Func<T, TU> some, Func<TU> none)
        {
            if (some == null) throw new ArgumentNullException(nameof(some));
            if (none == null) throw new ArgumentNullException(nameof(none));

            return new Some<TU>(none());
        }

        public override Maybe<TU> Select<TU>(Func<T, TU> some)
        {
            if (some == null) throw new ArgumentNullException(nameof(some));

            return new None<TU>();
        }

        public override async Task<Maybe<TU>> SelectAsync<TU>(Func<T, Task<TU>> some, Func<Task<TU>> none)
        {
            if (some == null) throw new ArgumentNullException(nameof(some));
            if (none == null) throw new ArgumentNullException(nameof(none));

            var value = await none();

            return new Some<TU>(value);
        }

        public override Task<Maybe<TU>> SelectAsync<TU>(Func<T, Task<TU>> some)
        {
            if (some == null) throw new ArgumentNullException(nameof(some));

            return Task.FromResult((Maybe<TU>) new None<TU>());
        }

        public override Maybe<TU> SelectMany<TU>(Func<T, Maybe<TU>> some, Func<Maybe<TU>> none)
        {
            if (some == null) throw new ArgumentNullException(nameof(some));
            if (none == null) throw new ArgumentNullException(nameof(none));

            return none();
        }

        public override Maybe<TU> SelectMany<TU>(Func<T, Maybe<TU>> some)
        {
            if (some == null) throw new ArgumentNullException(nameof(some));

            return new None<TU>();
        }

        public override Task<Maybe<TU>> SelectManyAsync<TU>(Func<T, Task<Maybe<TU>>> some, Func<Task<Maybe<TU>>> none)
        {
            if (some == null) throw new ArgumentNullException(nameof(some));
            if (none == null) throw new ArgumentNullException(nameof(none));

            return none();
        }

        public override Task<Maybe<TU>> SelectManyAsync<TU>(Func<T, Task<Maybe<TU>>> some)
        {
            if (some == null) throw new ArgumentNullException(nameof(some));

            return Task.FromResult((Maybe<TU>) new None<TU>());
        }

        public override void Match(Action<T> some, Action none)
        {
            if (some == null) throw new ArgumentNullException(nameof(some));
            if (none == null) throw new ArgumentNullException(nameof(none));

            none();
        }

        public override TOut Match<TOut>(Func<T, TOut> some, Func<TOut> none) => none();

        public override string ToString() => "";
    }
}