using System;
using System.Linq;
using SoftwareCraft.Maybe;
using Xunit;

namespace Tests
{
	public class MaybeTests
	{
		[Fact]
		public void Empty_maybe_returns_factory_provided_value()
		{
			var expected = new object();

			var sut = new Maybe<object>();

			object DefaultFactory() => expected;

			var actual = sut.ValueOrDefault(DefaultFactory);

			Assert.Same(expected, actual);
		}

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
		public void Null_default_factory_throws_exception()
		{
			var sut = new Maybe<object>();

			Assert.Throws<ArgumentNullException>(() => sut.ValueOrDefault(null));
		}
	}

	public class MaybeMapTests
	{
		[Fact]
		public void Maybe_calls_NOTHING_callback()
		{
			var sut = new Maybe<object>();

			var someCallbackCalled = false;
			var nothingCallbackCalled = false;

			sut.Map(i => someCallbackCalled = true, () => nothingCallbackCalled = true);

			Assert.False(someCallbackCalled);
			Assert.True(nothingCallbackCalled);
		}

		[Fact]
		public void Maybe_calls_SOME_callback()
		{
			var sut = new Maybe<object>(new object());

			var someCallbackCalled = false;

			sut.Map(i => someCallbackCalled = true);

			Assert.True(someCallbackCalled);
		}

		[Fact]
		public void Maybe_calls_SOME_callback_overload()
		{
			var sut = new Maybe<object>(new object());

			var someCallbackCalled = false;
			var nothingCallbackCalled = false;

			sut.Map(i => someCallbackCalled = true, () => nothingCallbackCalled = true);

			Assert.True(someCallbackCalled);
			Assert.False(nothingCallbackCalled);
		}

		[Fact]
		public void Null_NOTHING_callback_throws_exception()
		{
			var sut = new Maybe<object>(new object());

			Assert.Throws<ArgumentNullException>(() => sut.Map(i => { }, null));
		}

		[Fact]
		public void Null_SOME_callback_throws_exception()
		{
			var sut = new Maybe<object>(new object());

			Assert.Throws<ArgumentNullException>(() => sut.Map(null));
			Assert.Throws<ArgumentNullException>(() => sut.Map(null, () => { }));
		}
	}

	public class MaybeReturnMapTests
	{
		[Fact]
		public void NOTHING_returns_result()
		{
			var expectedObject = new object();

			var sut = new Maybe<object>();

			var actualObject = sut.Map(x => null, () => expectedObject);

			Assert.Equal(expectedObject, actualObject);
		}

		[Fact]
		public void Null_NOTHING_callback_throws_exception()
		{
			var sut = new Maybe<object>(new object());

			Assert.Throws<ArgumentNullException>(() => sut.Map(x => new object(), null));
		}

		[Fact]
		public void Null_SOME_callback_throws_exception()
		{
			var sut = new Maybe<object>(new object());

			Assert.Throws<ArgumentNullException>(() => sut.Map(null, () => new object()));
		}

		[Fact]
		public void SOME_returns_result()
		{
			var expectedObject = new object();

			var sut = new Maybe<object>(expectedObject);

			var actualObject = sut.Map(x => x, () => null);

			Assert.Equal(expectedObject, actualObject);
		}
	}

	public class MaybeFromResultTests
	{
		[Fact]
		public void Null_wrapped_method_throws_exception()
		{
			Assert.Throws<ArgumentNullException>(() => Maybe<object>.FromResult(null));
		}

		[Fact]
		public void Wrap_method_null_output_into_maybe()
		{
			var expected = new object();

			object Func() => null;

			var actual = Maybe<object>.FromResult(Func);

			Assert.Same(expected, actual.ValueOrDefault(expected));
		}

		[Fact]
		public void Wrap_method_output_into_maybe()
		{
			var expected = new object();
			var other = new object();

			object Func() => expected;

			var actual = Maybe<object>.FromResult(Func);

			Assert.Same(expected, actual.ValueOrDefault(other));
		}
	}
}