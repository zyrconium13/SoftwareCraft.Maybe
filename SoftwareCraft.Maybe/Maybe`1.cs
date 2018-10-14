using System;
using System.Linq;

namespace SoftwareCraft.Maybe
{
	public static class Maybe
	{
		public static Maybe<T> Some<T>(T value) => new Some<T>(value);
		public static Maybe<T> None<T>() => new None<T>();
	}

	public abstract class Maybe<T> : IEquatable<Maybe<T>>
	{
		protected readonly T[] Items;

		protected Maybe() => Items = new T[0];

		protected Maybe(T value) => Items = new[]
		                                    {
			                                    value
		                                    };

		public bool Equals(Maybe<T> other)
		{
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

		public abstract Maybe<U> SelectMany<U>(Func<T, Maybe<U>> some, Func<Maybe<U>> none);

		public abstract Maybe<U> SelectMany<U>(Func<T, Maybe<U>> some);

		public abstract U Match<U>(Func<T, U> some, Func<U> none);

		public abstract void Match(Action<T> some, Action none);

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != GetType()) return false;
			return Equals((Maybe<T>) obj);
		}

		public static bool operator ==(Maybe<T> left, Maybe<T> right) => Equals(left, right);

		public static bool operator !=(Maybe<T> left, Maybe<T> right) => !Equals(left, right);

		public override int GetHashCode()
			=> Items.Length == 0 ?
				0 :
				Items[0].GetHashCode();
	}

	public sealed class Some<T> : Maybe<T>
	{
		internal Some(T value)
			: base(value) { }

		public override Maybe<U> Select<U>(Func<T, U> some, Func<U> none)
		{
			if (some == null) throw new ArgumentNullException(nameof(some));

			return new Some<U>(some(Items[0]));
		}

		public override Maybe<U> Select<U>(Func<T, U> some)
		{
			if (some == null) throw new ArgumentNullException(nameof(some));

			return new Some<U>(some(Items[0]));
		}

		public override Maybe<U> SelectMany<U>(Func<T, Maybe<U>> some, Func<Maybe<U>> none)
		{
			if (some == null) throw new ArgumentNullException(nameof(some));

			return some(Items[0]);
		}

		public override Maybe<U> SelectMany<U>(Func<T, Maybe<U>> some)
		{
			if (some == null) throw new ArgumentNullException(nameof(some));

			return some(Items[0]);
		}

		public override U Match<U>(Func<T, U> some, Func<U> none)
		{
			if (some == null) throw new ArgumentNullException(nameof(some));

			return some(Items[0]);
		}

		public override void Match(Action<T> some, Action none)
		{
			if (some == null) throw new ArgumentNullException(nameof(some));

			some(Items[0]);
		}

		public override string ToString() => Items[0].ToString();
	}

	public sealed class None<T> : Maybe<T>
	{
		internal None() { }

		public override Maybe<U> Select<U>(Func<T, U> some, Func<U> none)
		{
			if (some == null) throw new ArgumentNullException(nameof(some));

			none();

			return new None<U>();
		}

		public override Maybe<U> Select<U>(Func<T, U> some) => new None<U>();

		public override Maybe<U> SelectMany<U>(Func<T, Maybe<U>> some)
		{
			if (some == null) throw new ArgumentNullException(nameof(some));

			return new None<U>();
		}

		public override Maybe<U> SelectMany<U>(Func<T, Maybe<U>> some, Func<Maybe<U>> none)
		{
			if (none == null) throw new ArgumentNullException(nameof(none));

			return none();
		}

		public override U Match<U>(Func<T, U> some, Func<U> none)
		{
			if (none == null) throw new ArgumentNullException(nameof(none));

			return none();
		}

		public override void Match(Action<T> some, Action none)
		{
			if (none == null) throw new ArgumentNullException(nameof(none));

			none();
		}

		public override string ToString() => "";
	}
}