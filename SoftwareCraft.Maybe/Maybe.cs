using System;
using System.Linq;

namespace SoftwareCraft.Functional;

using System.Collections.Generic;
using System.Threading.Tasks;

public static class Maybe
{
	public static Maybe<T> Some<T>(T value) => new Some<T>(value);

	public static Maybe<T> None<T>() => new None<T>();

	public static class Lifting
	{
		#region Lift2

		public static Maybe<Tuple<T1, T2>> Lift<T1, T2>(Maybe<T1> m1, Maybe<T2> m2)
		{
			return m1.SelectMany(v1 => m2.SelectMany(v2 => Maybe.Some(Tuple.Create(v1, v2))));
		}

		public static Maybe<Tuple<T1, T2>> LiftLazy<T1, T2>(Func<Maybe<T1>> f1, Func<Maybe<T2>> f2)
		{
			return f1().SelectMany(v1 => f2().SelectMany(v2 => Maybe.Some(Tuple.Create(v1, v2))));
		}

		public static async Task<Maybe<Tuple<T1, T2>>> LiftLazyAsync<T1, T2>(
			Func<Task<Maybe<T1>>> f1, Func<Task<Maybe<T2>>> f2)
		{
			return await (await f1())
				.SelectManyAsync(async v1 => (await f2())
									 .SelectMany(v2 => Maybe.Some(Tuple.Create(v1, v2))));
		}

		#endregion

		#region Lift3

		public static Maybe<Tuple<T1, T2, T3>> Lift<T1, T2, T3>(Maybe<T1> m1, Maybe<T2> m2, Maybe<T3> m3)
		{
			return m1.SelectMany(v1 => m2.SelectMany(v2 => m3.SelectMany(v3 => Maybe.Some(Tuple.Create(v1, v2, v3)))));
		}

		public static Maybe<Tuple<T1, T2, T3>> LiftLazy<T1, T2, T3>(Func<Maybe<T1>> f1, Func<Maybe<T2>> f2,
																	Func<Maybe<T3>> f3)
		{
			return f1().SelectMany(
				v1 => f2().SelectMany(v2 => f3().SelectMany(v3 => Maybe.Some(Tuple.Create(v1, v2, v3)))));
		}

		public static async Task<Maybe<Tuple<T1, T2, T3>>> LiftLazyAsync<T1, T2, T3>(
			Func<Task<Maybe<T1>>> f1, Func<Task<Maybe<T2>>> f2, Func<Task<Maybe<T3>>> f3)
		{
			return await (await f1())
				.SelectManyAsync(async v1 => await (await f2())
									 .SelectManyAsync(async v2 => (await f3())
														  .SelectMany(v3 => Maybe.Some(Tuple.Create(v1, v2, v3)))));
		}

		#endregion

		#region Lift4

		public static Maybe<Tuple<T1, T2, T3, T4>> Lift<T1, T2, T3, T4>(Maybe<T1> m1, Maybe<T2> m2, Maybe<T3> m3,
																		Maybe<T4> m4)
		{
			return m1.SelectMany(
				v1 => m2.SelectMany(
					v2 => m3.SelectMany(v3 => m4.SelectMany(v4 => Maybe.Some(Tuple.Create(v1, v2, v3, v4))))));
		}

		public static Maybe<Tuple<T1, T2, T3, T4>> LiftLazy<T1, T2, T3, T4>(Func<Maybe<T1>> f1, Func<Maybe<T2>> f2,
																			Func<Maybe<T3>> f3, Func<Maybe<T4>> f4)
		{
			return f1().SelectMany(
				v1 => f2().SelectMany(
					v2 => f3().SelectMany(
						v3 => f4().SelectMany(
							v4 => Maybe.Some(Tuple.Create(v1, v2, v3, v4))))));
		}

		public static async Task<Maybe<Tuple<T1, T2, T3, T4>>> LiftLazyAsync<T1, T2, T3, T4>(
			Func<Task<Maybe<T1>>> f1, Func<Task<Maybe<T2>>> f2, Func<Task<Maybe<T3>>> f3, Func<Task<Maybe<T4>>> f4)
		{
			return await (await f1())
				.SelectManyAsync(async v1 => await (await f2())
									 .SelectManyAsync(async v2 => await (await f3())
														  .SelectManyAsync(async v3 => (await f4())
																			   .SelectMany(v4 =>
																				   Maybe.Some(Tuple.Create(
																					   v1, v2, v3, v4))))));
		}

		#endregion

		#region Lift5

		public static Maybe<Tuple<T1, T2, T3, T4, T5>> Lift<T1, T2, T3, T4, T5>(Maybe<T1> m1, Maybe<T2> m2, Maybe<T3> m3,
																				Maybe<T4> m4, Maybe<T5> m5)
		{
			return m1.SelectMany(
				v1 => m2.SelectMany(
					v2 => m3.SelectMany(
						v3 => m4.SelectMany(
							v4 => m5.SelectMany(
								v5 => Maybe.Some(Tuple.Create(v1, v2, v3, v4, v5)))))));
		}

		public static Maybe<Tuple<T1, T2, T3, T4, T5>> LiftLazy<T1, T2, T3, T4, T5>(Func<Maybe<T1>> f1, Func<Maybe<T2>> f2,
			Func<Maybe<T3>> f3, Func<Maybe<T4>> f4, Func<Maybe<T5>> f5)
		{
			return f1().SelectMany(
				v1 => f2().SelectMany(
					v2 => f3().SelectMany(
						v3 => f4().SelectMany(
							v4 => f5().SelectMany(
								v5 => Maybe.Some(Tuple.Create(v1, v2, v3, v4, v5)))))));
		}

		public static async Task<Maybe<Tuple<T1, T2, T3, T4, T5>>> LiftLazyAsync<T1, T2, T3, T4, T5>(
			Func<Task<Maybe<T1>>> f1, Func<Task<Maybe<T2>>> f2, Func<Task<Maybe<T3>>> f3, Func<Task<Maybe<T4>>> f4,
			Func<Task<Maybe<T5>>> f5)
		{
			return await (await f1())
				.SelectManyAsync(async v1 => await (await f2())
									 .SelectManyAsync(async v2 => await (await f3())
														  .SelectManyAsync(async v3 => await (await f4())
																			   .SelectManyAsync(async v4 =>
																				   (await f5()).SelectMany(v5 =>
																					   Maybe.Some(Tuple.Create(
																						   v1, v2, v3, v4,
																						   v5)))))));
		}

		#endregion
	}
}