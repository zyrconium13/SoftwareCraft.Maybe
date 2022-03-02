using System;
using System.Collections.Generic;
using System.Linq;

namespace SoftwareCraft.Functional;

using System.Threading.Tasks;

public abstract class Maybe<T> : IEquatable<Maybe<T>>
{
	protected readonly T[] Items;

	protected Maybe() => Items = Array.Empty<T>();

	protected Maybe(T value)
	{
		if (null == value) throw new ArgumentNullException();

		Items = new[]
		{
			value
		};
	}

	public abstract bool IsSome { get; }

	public abstract bool IsNone { get; }

	internal abstract T Value { get; }

	public bool Equals(Maybe<T> other)
	{
		if (null == other) return false;

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

	public abstract Task<Maybe<U>> SelectAsync<U>(Func<T, Task<U>> some, Func<Task<U>> none);

	public abstract Task<Maybe<U>> SelectAsync<U>(Func<T, Task<U>> some);

	public abstract Maybe<U> SelectMany<U>(Func<T, Maybe<U>> some, Func<Maybe<U>> none);

	public abstract Maybe<U> SelectMany<U>(Func<T, Maybe<U>> some);

	public abstract Task<Maybe<U>> SelectManyAsync<U>(Func<T, Task<Maybe<U>>> some, Func<Task<Maybe<U>>> none);

	public abstract Task<Maybe<U>> SelectManyAsync<U>(Func<T, Task<Maybe<U>>> some);

	public abstract Maybe<T> SelectConditional(Predicate<T> predicate);

	public abstract void Match(Action<T> some, Action none);

	public abstract void Match(Action<T> some);

	public abstract Task MatchAsync(Func<T, Task> some, Func<Task> none);

	public abstract TOut Match<TOut>(Func<T, TOut> some, Func<TOut> none);

	public abstract Task<TOut> MatchAsync<TOut>(Func<T, Task<TOut>> some, Func<Task<TOut>> none);

	public override bool Equals(object obj)
	{
		if (ReferenceEquals(null, obj)) return false;
		if (ReferenceEquals(this, obj)) return true;
		if (obj.GetType() != GetType()) return false;
		return Equals((Maybe<T>)obj);
	}

	public static bool operator ==(Maybe<T> left, Maybe<T> right) => Equals(left, right);

	public static bool operator !=(Maybe<T> left, Maybe<T> right) => !Equals(left, right);

	public override int GetHashCode() => Items.Length == 0 ? 0 : Items[0].GetHashCode();
}

public static class MaybeExtensions
{
	public static Maybe<T> AsMaybe<T>(this T @this)
		=> new Some<T>(@this);

	public static Maybe<T1> Apply<T, T1>(this Maybe<Func<T, T1>> m, Maybe<T> other)
		=> m.Match(f => other.Match(o => Maybe.Some(f(o)), Maybe.None<T1>), Maybe.None<T1>);

	public static Maybe<T1> Apply<T, T1>(this Maybe<Func<T, T1>> m, T other)
		=> m.Match(f => Maybe.Some(f(other)), Maybe.None<T1>);

	public static bool AllSome<T>(params Maybe<T>[] maybes)
		=> maybes.All(x => x.IsSome);

	public static bool AnySome<T>(params Maybe<T>[] maybes)
		=> maybes.Any(x => x.IsSome);

	public static bool AllNone<T>(params Maybe<T>[] maybes)
		=> maybes.All(x => x.IsNone);

	public static bool AnyNone<T>(params Maybe<T>[] maybes)
		=> maybes.Any(x => x.IsNone);
}