using System;
using System.Threading.Tasks;
using SoftwareCraft.Maybe;
using Xunit;

namespace Tests
{
	public class MaybeMapAsyncTests
	{
		[Fact]
		public async Task Maybe_calls_SOME_callback_async()
		{
			var someCallbackCalled = false;

			var sut = new Maybe<object>(new object());

			await sut.MapAsync((o) => someCallbackCalled = true).ConfigureAwait(false);

			Assert.True(someCallbackCalled);
		}

		[Fact]
		public async Task Maybe_calls_SOME_callback_async_overload()
		{
			var someCallbackCalled = false;

			var sut = new Maybe<object>(new object());

			await sut.MapAsync(
				(o) => someCallbackCalled = true,
				() => { }).ConfigureAwait(false);

			Assert.True(someCallbackCalled);
		}

		[Fact]
		public async Task Maybe_calls_NOTHING_callback_async()
		{
			var nothingCallbackCalled = false;

			var sut = new Maybe<object>();

			await sut.MapAsync(
				(o) => { },
				() => nothingCallbackCalled = true).ConfigureAwait(false);

			Assert.True(nothingCallbackCalled);
		}

		[Fact]
		public async Task Null_SOME_callback_throws_exception_async()
		{
			var sut = new Maybe<object>(new object());

			await Assert.ThrowsAsync<ArgumentNullException>(() => sut.MapAsync(null)).ConfigureAwait(false);
		}

		[Fact]
		public async Task Null_NOTHING_callback_throws_exception_async()
		{
			var sut = new Maybe<object>();

			await Assert.ThrowsAsync<ArgumentNullException>(() => sut.MapAsync((o) => { }, null)).ConfigureAwait(false);
		}
	}

	public class MaybeFromResultAsyncTests
	{
		[Fact]
		public async Task Wrap_method_output_into_maybe_async()
		{
			var expected = new object();

			Func<object> func = () => expected;

			var actual = await Maybe<object>.FromResultAsync(func).ConfigureAwait(false);

			Assert.Same(expected, actual.ValueOrDefault(new object()));
		}

		[Fact]
		public async Task Wrap_method_null_output_into_maybe_async()
		{
			var expected = new object();

			Func<object> func = () => null;

			var actual = await Maybe<object>.FromResultAsync(func).ConfigureAwait(false);

			Assert.Same(expected, actual.ValueOrDefault(expected));
		}

		[Fact]
		public async Task Handling_maybe_of_task_of_object()
		{
			var expected = new object();

			Func<Task<object>> func = () => Task.FromResult(expected);

			var actual = await Maybe<Task<object>>.FromResultAsync(func).ConfigureAwait(false);

			Assert.Same(expected, await actual.ValueOrDefault(Task.FromResult(new object())).ConfigureAwait(false));
		}
	}
}