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

			var msg1 = m1.Bind(i => Maybe.Some($"Your value is {i}"));

			var msg = m1.Bind(a => Maybe.Some(a + 10))
			            .Bind(b => Maybe.Some(b * 2.0))
			            .Bind(c => Maybe.Some(c - 7))
			            .Map(r => $"The result is {r:N}.", () => "No result.");
			Console.WriteLine(msg);

			switch (msg1)
			{
				case None<string> none:
					Console.WriteLine("No value.");
					break;
				case Some<string> some:
					Console.WriteLine(some.Value);
					break;
			}

			var e1 = Maybe.Some(42);
			var e1s = e1.ToString();

			var e2 = Maybe.Some(42);
			var e2s = e2.ToString();

			Debug.Assert(e1.Equals(e2));
		}
	}
}