using System;
using System.Diagnostics;
using System.Linq;
using SoftwareCraft.Maybe;

namespace Sandbox
{
	internal class Program1
	{
		private static void Main(string[] args)
		{
			var m1 = Maybe.None<int>();

			var m2 = Maybe.Some(Maybe.Some(Maybe.Some(42)));

			var msg1 = m1.SelectMany(i => Maybe.Some($"Your value is {i}"));
			var msg11 = m1.Select(i => $"Your value is {i}");

			m1.SelectMany(a => Maybe.Some(a + 10))
			  .SelectMany(b => Maybe.Some(b * 2.0))
			  .SelectMany(c => Maybe.Some(c - 7))
			  .Match(r => Console.WriteLine($"The result is {r:N}."), () => Console.WriteLine("No result."));

			var msg = m1.Select(i => i + 10, () => 0);

			m2.SelectMany(maybe => maybe.SelectMany(maybe1 => maybe1.SelectMany(i => Maybe.Some(i / 2))));

			Console.WriteLine(msg);

			//switch (msg1)
			//{
			//	case None<string> none:
			//		Console.WriteLine("No value.");
			//		break;
			//	case Some<string> some:
			//		Console.WriteLine(some.Value);
			//		break;
			//}

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