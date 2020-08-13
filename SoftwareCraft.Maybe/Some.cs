using System;

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

		public override void Match(Action<T> some, Action none)
		{
			if (some == null) throw new ArgumentNullException(nameof(some));
			if (none == null) throw new ArgumentNullException(nameof(none));

			some(Items[0]);
		}

		public override TOut Match<TOut>(Func<T, TOut> some, Func<TOut> none)
		{
			return some(Items[0]);
		}

		public override string ToString()
		{
			return Items[0].ToString();
		}
	}
}