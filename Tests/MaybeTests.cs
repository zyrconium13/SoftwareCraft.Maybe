using System;
using System.Linq;
using SoftwareCraft.Maybe;
using Xunit;

namespace Tests
{
	public class MaybeTests
	{
		[Fact]
		public void Test()
		{
			var mi = Maybe.Some(13);
		}
	}
}