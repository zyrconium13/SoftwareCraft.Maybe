using System;
using SoftwareCraft.Maybe;
using Xunit;

namespace Tests
{
	public class MaybeTests
	{
		[Fact]
		public void Empty_maybe_returns_provided_default()
		{
			var expected = new object();

			var sut = new Maybe<object>();

			var actual = sut.ValueOrDefault(expected);

			Assert.Same(expected, actual);
		}

		[Fact]
		public void Maybe_returns_contained_value()
		{
			var expected = new object();
			var another = new object();

			var sut = new Maybe<object>(expected);

			var actual = sut.ValueOrDefault(another);

			Assert.Same(expected, actual);
		}

		[Fact]
		public void Maybe_calls_SOME_callback()
		{
			var sut = new Maybe<object>(new object());

			var someCallbackCalled = false;

			sut.Map((i) => someCallbackCalled = true);

			Assert.True(someCallbackCalled);
		}

		[Fact]
		public void Maybe_calls_SOME_callback_overload()
		{
			var sut = new Maybe<object>(new object());

			var someCallbackCalled = false;
			var nothingCallbackCalled = false;

			sut.Map((i) => someCallbackCalled = true, () => nothingCallbackCalled = true);

			Assert.True(someCallbackCalled);
			Assert.False(nothingCallbackCalled);
		}

		[Fact]
		public void Maybe_calls_NOTHING_callback()
		{
			var sut = new Maybe<object>();

			var someCallbackCalled = false;
			var nothingCallbackCalled = false;

			sut.Map((i) => someCallbackCalled = true, () => nothingCallbackCalled = true);

			Assert.False(someCallbackCalled);
			Assert.True(nothingCallbackCalled);
		}

		[Fact]
		public void Null_SOME_callback_throws_exception()
		{
			var sut = new Maybe<object>(new object());

			Assert.Throws<ArgumentNullException>(() => sut.Map(null));
			Assert.Throws<ArgumentNullException>(() => sut.Map(null, () => { }));
		}

		[Fact]
		public void Null_NOTHING_callback_throws_exception()
		{
			var sut = new Maybe<object>(new object());

			Assert.Throws<ArgumentNullException>(() => sut.Map((i) => { }, null));
		}

		[Fact]
		public void Wrap_method_output_into_maybe()
		{
			var expected = new object();
			var other = new object();

			Func<object> func = () => expected;

			var actual = Maybe<object>.FromResult(func);

			Assert.Same(expected, actual.ValueOrDefault(other));
		}

		[Fact]
		public void Wrap_method_null_output_into_maybe()
		{
			var expected = new object();

			Func<object> func = () => null;

			var actual = Maybe<object>.FromResult(func);

			Assert.Same(expected, actual.ValueOrDefault(expected));
		}

		[Fact]
		public void Null_wrapped_method_throws_exception()
		{
			Assert.Throws<ArgumentNullException>(() => Maybe<object>.FromResult(null));
		}
	}
}