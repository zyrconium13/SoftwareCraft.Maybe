using System;
using System.Linq;

namespace SoftwareCraft.Maybe
{
	public struct Unit { }

	// ReSharper disable once InconsistentNaming
	public class IO<T>
	{
		private readonly Func<T> func;

		public IO(Func<T> func) => this.func = func;

		public IO<U> Bind<U>(Func<T, IO<U>> lift) => new IO<U>(() => lift(func()).Run());

		public T Run() => func();
	}
}