using System;
using System.Linq;

namespace SoftwareCraft.Maybe
{
	/// <summary>
	///     Provides a way to represent data that may or may not contain a value. Use it instead of returning null.
	/// </summary>
	/// <typeparam name="T">The type of the contained value. Must be a reference type.</typeparam>
	public class Maybe<T>
	{
		private readonly T[] items;

		/// <summary>
		///     Creates an instance that does not contain a value.
		/// </summary>
		public Maybe() => items = new T[0];

		/// <summary>
		///     Creates an instance that contains a value.
		/// </summary>
		/// <param name="value"></param>
		public Maybe(T value) => items = new[]
		                                 {
			                                 value
		                                 };

		public bool HasValue => items.Length == 1;

		/// <summary>
		///     Allows specifying actions that will be called if the current instance contains a value or not.
		/// </summary>
		/// <param name="some">The action that will be called if the current instance contains a value.</param>
		public void Map(Action<T> some)
		{
			if (some == null) throw new ArgumentNullException(nameof(some));

			if (items.Length != 0) some(items[0]);
		}

		/// <summary>
		///     Allows specifying actions that will be called if the current instance contains a value or not.
		/// </summary>
		/// <param name="some">The action that will be called if the current instance contains a value.</param>
		/// <param name="nothing">The action that will be called if the current instance does not contain a value.</param>
		public void Map(Action<T> some, Action nothing)
		{
			if (some == null) throw new ArgumentNullException(nameof(some));
			if (nothing == null) throw new ArgumentNullException(nameof(nothing));

			if (items.Length == 0)
				nothing();
			else
				some(items[0]);
		}

		/// <summary>
		///     Allows specifying actions that will be called if the current instance contains a value or not.
		/// </summary>
		/// <param name="some">The action that will be called if the current instance contains a value.</param>
		/// <param name="nothing">The action that will be called if the current instance does not contain a value.</param>
		/// <returns>Returns the value provided by either of the delegate functions.</returns>
		public U Map<U>(Func<T, U> some, Func<U> nothing)
		{
			if (some == null) throw new ArgumentNullException(nameof(some));
			if (nothing == null) throw new ArgumentNullException(nameof(nothing));

			return items.Length == 0 ?
				nothing() :
				some(items[0]);
		}

		/// <summary>
		///     Facilitates wrapping an existing method that currently returns a single instance of an object, but may also return
		///     null.
		/// </summary>
		/// <param name="func">The function whose result must be converted to a <see cref="Maybe{T}" />.</param>
		/// <returns></returns>
		public static Maybe<T> Wrap(Func<T> func)
		{
			if (func == null) throw new ArgumentNullException(nameof(func));

			var result = func();

			return result == null ?
				new Maybe<T>() :
				new Maybe<T>(result);
		}

		public Maybe<U> Bind<U>(Func<T, Maybe<U>> func)
			=> Map(func, () => new Maybe<U>());
	}
}