using System;
using System.Linq;

namespace SoftwareCraft.Functional
{
	public sealed class None<T> : Maybe<T>
	{
		internal None() { }

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

		public override Maybe<U> SelectMany<U>(Func<T, Maybe<U>> some)
		{
			if (some == null) throw new ArgumentNullException(nameof(some));

			return new None<U>();
		}

		public override Maybe<U> SelectMany<U>(Func<T, Maybe<U>> some, Func<Maybe<U>> none)
		{
			if (some == null) throw new ArgumentNullException(nameof(some));
			if (none == null) throw new ArgumentNullException(nameof(none));

			return none();
		}

		public override void Match(Action<T> some, Action none)
		{
			if (some == null) throw new ArgumentNullException(nameof(some));
			if (none == null) throw new ArgumentNullException(nameof(none));

			none();
		}

		public override string ToString() => "";
	}
}