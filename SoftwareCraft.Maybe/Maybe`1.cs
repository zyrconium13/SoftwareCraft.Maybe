using System;
using System.Linq;

namespace SoftwareCraft.Maybe
{
	public abstract class Maybe<T>
	{
		protected T[] Items;

		public abstract bool HasValue { get; }

		public virtual void Map(Action<T> some) { }

		public abstract void Map(Action<T> some, Action none);

		public abstract U Map<U>(Func<T, U> some, Func<U> none);

		public static Maybe<T> Wrap(Func<T> func)
		{
			if (func == null) throw new ArgumentNullException(nameof(func));

			var result = func();

			return result == null ?
				(Maybe<T>) new None<T>() :
				new Some<T>(result);
		}

		public Maybe<U> Bind<U>(Func<T, Maybe<U>> func)
			=> Map(func, () => new None<U>());

		public static Maybe<T> Some(T value) => new Some<T>(value);

		public static Maybe<T> None() => new None<T>();
	}

	public sealed class Some<T> : Maybe<T>
	{
		internal Some(T value) => Items = new[]
		                                  {
			                                  value
		                                  };

		public override bool HasValue => true;

		public T Value => Items[0];

		public override void Map(Action<T> some)
		{
			if (some == null) throw new ArgumentNullException(nameof(some));

			if (Items.Length != 0) some(Items[0]);
		}

		public override void Map(Action<T> some, Action none)
		{
			if (some == null) throw new ArgumentNullException(nameof(some));

			some(Items[0]);
		}

		public override U Map<U>(Func<T, U> some, Func<U> none)
		{
			if (some == null) throw new ArgumentNullException(nameof(some));

			return some(Items[0]);
		}
	}

	public sealed class None<T> : Maybe<T>
	{
		public override bool HasValue => false;

		public override void Map(Action<T> some, Action none)
		{
			if (none == null) throw new ArgumentNullException(nameof(none));

			none();
		}

		public override U Map<U>(Func<T, U> some, Func<U> none)
		{
			if (none == null) throw new ArgumentNullException(nameof(none));

			return none();
		}
	}
}