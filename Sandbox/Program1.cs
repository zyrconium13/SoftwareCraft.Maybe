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
			var m1 = Maybe.Some(13);

			var m2 = Maybe.Some(Maybe.Some(Maybe.Some(42)));

			var msg1 = m1.Map(i => i * 2);

			m1.Bind(i => Maybe.Some($"Your value is {i}"))
			  .Match(Console.WriteLine, () => { });

			var msg = m1.Bind(a => Maybe.Some(a + 10))
			            .Bind(b => Maybe.Some(b * 2))
			            .Bind(c => Maybe.Some(c - 7))
			            .Match(r => $"The result is {r}.", () => "No result.");

			Console.WriteLine(msg);

			var e1 = Maybe.Some(42);
			var e1s = e1.ToString();

			var e2 = Maybe.Some(42);
			var e2s = e2.ToString();

			Debug.Assert(e1.Equals(e2));
		}
	}
}