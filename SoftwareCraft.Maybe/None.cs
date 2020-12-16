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

        public override Maybe<U> Select<U>(Func<T, U> some, Func<U> none)
        {
            if (some == null) throw new ArgumentNullException(nameof(some));
            if (none == null) throw new ArgumentNullException(nameof(none));

            return new Some<U>(none());
        }

        public override Maybe<U> Select<U>(Func<T, U> some)
        {
            if (some == null) throw new ArgumentNullException(nameof(some));

            return new None<U>();
        }

        public override async Task<Maybe<U>> SelectAsync<U>(Func<T, Task<U>> some, Func<Task<U>> none)
        {
            if (some == null) throw new ArgumentNullException(nameof(some));
            if (none == null) throw new ArgumentNullException(nameof(none));

            var value = await none();

            return new Some<U>(value);
        }

        public override Task<Maybe<U>> SelectAsync<U>(Func<T, Task<U>> some)
        {
            if (some == null) throw new ArgumentNullException(nameof(some));

            return Task.FromResult((Maybe<U>) new None<U>());
        }

        public override Maybe<U> SelectMany<U>(Func<T, Maybe<U>> some, Func<Maybe<U>> none)
        {
            if (some == null) throw new ArgumentNullException(nameof(some));
            if (none == null) throw new ArgumentNullException(nameof(none));

            return none();
        }

        public override Maybe<U> SelectMany<U>(Func<T, Maybe<U>> some)
        {
            if (some == null) throw new ArgumentNullException(nameof(some));

            return new None<U>();
        }

        public override Task<Maybe<U>> SelectManyAsync<U>(Func<T, Task<Maybe<U>>> some, Func<Task<Maybe<U>>> none)
        {
            if (some == null) throw new ArgumentNullException(nameof(some));
            if (none == null) throw new ArgumentNullException(nameof(none));

            return none();
        }

        public override Task<Maybe<U>> SelectManyAsync<U>(Func<T, Task<Maybe<U>>> some)
        {
            if (some == null) throw new ArgumentNullException(nameof(some));

            return Task.FromResult((Maybe<U>) new None<U>());
        }

        public override void Match(Action<T> some, Action none)
        {
            if (some == null) throw new ArgumentNullException(nameof(some));
            if (none == null) throw new ArgumentNullException(nameof(none));

            none();
        }

        public override Task MatchAsync(Func<T, Task> some, Func<Task> none)
        {
            if (some == null) throw new ArgumentNullException(nameof(some));
            if (none == null) throw new ArgumentNullException(nameof(none));

            return none();
        }

        public override TOut Match<TOut>(Func<T, TOut> some, Func<TOut> none)
        {
            if (some == null) throw new ArgumentNullException(nameof(some));
            if (none == null) throw new ArgumentNullException(nameof(none));

            return none();
        }

        public override Task<TOut> MatchAsync<TOut>(Func<T, Task<TOut>> some, Func<Task<TOut>> none)
        {
            if (some == null) throw new ArgumentNullException(nameof(some));
            if (none == null) throw new ArgumentNullException(nameof(none));

            return none();
        }

        public override string ToString() => "";
    }
}