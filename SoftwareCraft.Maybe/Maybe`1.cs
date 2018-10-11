using System;
using System.Linq;

namespace SoftwareCraft.Maybe
{
	public static class Maybe
	{
		public static Maybe<T> Some<T>(T value) => new Some<T>(value);
		public static Maybe<T> None<T>() => new None<T>();
	}

	public abstract class Maybe<T>
	{
		protected T[] Items;

		public abstract void Match(Action<T> some, Action none);

		public abstract U Match<U>(Func<T, U> some, Func<U> none);

		public abstract Maybe<U> Map<U>(Func<T, U> some);

		public abstract Maybe<U> Bind<U>(Func<T, Maybe<U>> func);
	}

	public sealed class Some<T> : Maybe<T>, IEquatable<Some<T>>
	{
		internal Some(T value) => Items = new[]
		                                  {
			                                  value
		                                  };

		public bool Equals(Some<T> other) => Items[0].Equals(other.Items[0]);

		public override void Match(Action<T> some, Action none)
		{
			if (some == null) throw new ArgumentNullException(nameof(some));

			some(Items[0]);
		}

		public override U Match<U>(Func<T, U> some, Func<U> none)
		{
			if (some == null) throw new ArgumentNullException(nameof(some));

			return some(Items[0]);
		}

		public override Maybe<U> Map<U>(Func<T, U> func)
		{
			if (func == null) throw new ArgumentNullException(nameof(func));

			return new Some<U>(func(Items[0]));
		}

		public override Maybe<U> Bind<U>(Func<T, Maybe<U>> func)
		{
			if (func == null) throw new ArgumentNullException(nameof(func));

			return func(Items[0]);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			return obj is Some<T> other && Equals(other);
		}

		public override int GetHashCode() => Items[0].GetHashCode();

		public override string ToString() => Items[0].ToString();
	}

	public sealed class None<T> : Maybe<T>, IEquatable<None<T>>
	{
		public bool Equals(None<T> other) => true;

		public override void Match(Action<T> some, Action none)
		{
			if (none == null) throw new ArgumentNullException(nameof(none));

			none();
		}

		public override U Match<U>(Func<T, U> some, Func<U> none)
		{
			if (none == null) throw new ArgumentNullException(nameof(none));

			return none();
		}

		public override Maybe<U> Map<U>(Func<T, U> func)
		{
			if (func == null) throw new ArgumentNullException(nameof(func));

			return new None<U>();
		}

		public override Maybe<U> Bind<U>(Func<T, Maybe<U>> func)
		{
			if (func == null) throw new ArgumentNullException(nameof(func));

			return new None<U>();
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			return obj is None<T> other && Equals(other);
		}

		public override int GetHashCode() => 0;

		public override string ToString() => "";
	}
}