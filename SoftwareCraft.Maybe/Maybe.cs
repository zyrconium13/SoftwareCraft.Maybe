using System;
using System.Linq;

namespace SoftwareCraft.Functional
{
	public static class Maybe
	{
		public static Maybe<T> Some<T>(T value)
		{
			return new Some<T>(value);
		}

		public static Maybe<T> None<T>()
		{
			return new None<T>();
		}
	}
}