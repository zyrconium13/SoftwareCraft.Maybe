using System;
using System.Diagnostics;
using System.Linq;
using SoftwareCraft.Maybe;

namespace Sandbox
{
	internal class Person
	{
	}

	internal class Program1
	{
		private static void Main(string[] args)
		{
			var m1 = Maybe.None<int>();

			var m2 = Maybe.Some(Maybe.Some(Maybe.Some(42)));

			var msg1 = m1.SelectMany(i => Maybe.Some($"Your value is {i}"));
			var msg11 = m1.Select(i => $"Your value is {i}");

			m1.Select(a => a + 10)
				.Select(b => b * 2.0)
				.Select(c => c - 7)
				.Match(r => Console.WriteLine($"The result is {r:N}."), () => Console.WriteLine("No result."));

			var msg = m1.Select(i => i + 10, () => 0);

			m2.SelectMany(maybe => maybe.SelectMany(maybe1 => maybe1.SelectMany(i => Maybe.Some(i / 2))));

			Console.WriteLine(msg);

			var numbers = new[] {1, 2, 3, 4, 5};

			var maybes1 = numbers.Select(i => new[] {Maybe.Some(i - 1), Maybe.Some(i + 1)});

			var maybe2 = numbers.SelectMany(i => new[] {Maybe.Some(i - 1), Maybe.Some(i + 1)});

			var e1 = Maybe.Some(42);
			var e1s = e1.ToString();

			var e2 = Maybe.Some(42);
			var e2s = e2.ToString();

			var x = e1.SelectMany(i => e2.Select(i1 => i + i1), Maybe.None<int>);

			Debug.Assert(e1.Equals(e2));
			Debug.Assert(e1 == e2);
		}
	}
}