using System;
using System.Threading.Tasks;

namespace SoftwareCraft.Maybe
{
	/// <summary>
	/// Provides a way to represent data that may or may not contain a value. Use it instead of returning null.
	/// </summary>
	/// <typeparam name="T">The type of the contained value. Must be a reference type.</typeparam>
	public class Maybe<T>
		where T : class
	{
		private readonly Action DefaultNothingAction = () => { };

		private readonly T[] items;

		/// <summary>
		/// Creates an instance that does not contain a value.
		/// </summary>
		public Maybe()
		{
			items = new T[0];
		}

		/// <summary>
		/// Creates an instance that contains a value.
		/// </summary>
		/// <param name="value"></param>
		public Maybe(T value)
		{
			items = new[] { value };
		}

		/// <summary>
		/// Provides a safe way to retrieve the contained value.
		/// </summary>
		/// <param name="surrogate">This will be returned if the current instance does not contain a value.</param>
		public T ValueOrDefault(T surrogate)
		{
			return items.Length == 0 ? surrogate : items[0];
		}

		/// <summary>
		/// Provides a safe way to retrieve the contained value, allowing for lazy evaluation of the default value.
		/// </summary>
		/// <param name="surrogateFactory">The output of this <see cref="Func{TResult}"/> will be returned if the current instance does not contain a value.</param>
		public T ValueOrDefault(Func<T> surrogateFactory)
		{
			if (surrogateFactory == null) throw new ArgumentNullException(nameof(surrogateFactory));

			return items.Length == 0 ? surrogateFactory() : items[0];
		}

		/// <summary>
		/// Provides a safe way to retrieve the contained value, allowing for lazy asynchronous evaluation of the default value.
		/// </summary>
		/// <param name="surrogateFactory">The output of this <see cref="Func{TResult}"/> will be returned if the current instance does not contain a value.</param>
		public Task<T> ValueOrDefaultAsync(Func<T> surrogateFactory)
		{
			if (surrogateFactory == null) throw new ArgumentNullException(nameof(surrogateFactory));

			return items.Length == 0
				? Task.Run(() => surrogateFactory())
				: Task.FromResult(items[0]);
		}

		/// <summary>
		/// Allows specifying actions that will be called if the current instance contains a value or not.
		/// </summary>
		/// <param name="some">The action that will be called if the current instance contains a value.</param>
		public void Map(Action<T> some)
		{
			Map(some, DefaultNothingAction);
		}

		/// <summary>
		/// Allows specifying actions that will be called if the current instance contains a value or not.
		/// </summary>
		/// <param name="some">The action that will be called if the current instance contains a value.</param>
		/// <param name="nothing">The action that will be called if the current instance does not contain a value.</param>
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

		/// <summary>
		/// Facilitates wrapping an existing method that currently returns a single instance of an object, but may also return null.
		/// </summary>
		/// <param name="func">The function whose result must be converted to a <see cref="Maybe{T}"/>.</param>
		/// <returns></returns>
		public static Maybe<T> FromResult(Func<T> func)
		{
			if (func == null) throw new ArgumentNullException(nameof(func));

			var result = func();

			if (result == null) return new Maybe<T>();

			return new Maybe<T>(result);
		}

		/// <summary>
		/// Allows specifying actions that will be called if the current instance contains a value or not. The delegates will be executed asynchronously.
		/// </summary>
		/// <param name="some">The action that will be called if the current instance contains a value.</param>
		public Task MapAsync(Action<T> some)
		{
			return Task.Run(() => Map(some));
		}

		/// <summary>
		/// Allows specifying actions that will be called if the current instance contains a value or not. The delegates will be executed asynchronously.
		/// </summary>
		/// <param name="some">The action that will be called if the current instance contains a value.</param>
		/// <param name="nothing">The action that will be called if the current instance does not contain a value.</param>
		public Task MapAsync(Action<object> some, Action nothing)
		{
			return Task.Run(() => Map(some, nothing));
		}

		/// <summary>
		/// Facilitates wrapping an existing method that currently returns a single instance of an object, but may also return null. The method will be executed asynchronously.
		/// </summary>
		/// <param name="func">The function whose result must be converted to a <see cref="Maybe{T}"/>.</param>
		/// <returns></returns>
		public static Task<Maybe<T>> FromResultAsync(Func<T> func)
		{
			return Task.Run(() => FromResult(func));
		}
	}
}