using System;

namespace SoftwareCraft.Maybe
{
	public class Maybe<T>
		where T : class
	{
		private readonly Action DefaultNothingAction = () => { };

		private readonly T[] items;

		public Maybe()
		{
			items = new T[0];
		}

		public Maybe(T value)
		{
			items = new[] { value };
		}

		public T ValueOrDefault(T surrogate)
		{
			return items.Length == 0 ? surrogate : items[0];
		}

		public void Map(Action<T> some)
		{
			Map(some, DefaultNothingAction);
		}

		public void Map(Action<T> some, Action nothing)
		{
			if (some == null) throw new ArgumentNullException(nameof(some));
			if (nothing == null) throw new ArgumentNullException(nameof(nothing));

			if (items.Length == 0)
			{
				nothing();
				return;
			}

			some(items[0]);
		}

		public static Maybe<T> FromResult(Func<T> func)
		{
			if (func == null) throw new ArgumentNullException(nameof(func));

			var result = func();

			if (result == null) return new Maybe<T>();

			return new Maybe<T>(result);
		}
	}
}