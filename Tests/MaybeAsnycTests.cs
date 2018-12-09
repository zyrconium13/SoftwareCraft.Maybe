﻿using System;
using System.Linq;
using System.Threading.Tasks;
using SoftwareCraft.Maybe;
using Xunit;

namespace Tests
{
	public class MaybeAsyncTests
	{
		[Fact]
		public async Task Empty_maybe_returns_factory_default_value_async()
		{
			var expected = new object();

			var sut = new Maybe<object>();

			object DefaultFactory() => expected;

			var actual = await sut.ValueOrDefaultAsync(DefaultFactory);

			Assert.Same(expected, actual);
		}

		[Fact]
		public async Task Null_default_value_factory_throws_exception_async()
		{
			var sut = new Maybe<object>();

			await Assert.ThrowsAsync<ArgumentNullException>(() => sut.ValueOrDefaultAsync(null)).ConfigureAwait(false);
		}
	}

	public class MaybeMapAsyncTests
	{
		[Fact]
		public async Task Maybe_calls_NOTHING_callback_async()
		{
			var nothingCallbackCalled = false;

			var sut = new Maybe<object>();

			await sut.MapAsync(o => { }, () => nothingCallbackCalled = true).ConfigureAwait(false);

			Assert.True(nothingCallbackCalled);
		}

		[Fact]
		public async Task Maybe_calls_SOME_callback_async()
		{
			var someCallbackCalled = false;

			var sut = new Maybe<object>(new object());

			await sut.MapAsync(o => someCallbackCalled = true).ConfigureAwait(false);

			Assert.True(someCallbackCalled);
		}

		[Fact]
		public async Task Maybe_calls_SOME_callback_async_overload()
		{
			var someCallbackCalled = false;

			var sut = new Maybe<object>(new object());

			await sut.MapAsync(o => someCallbackCalled = true, () => { }).ConfigureAwait(false);

			Assert.True(someCallbackCalled);
		}

		[Fact]
		public async Task Null_NOTHING_callback_throws_exception_async()
		{
			var sut = new Maybe<object>();

			await Assert.ThrowsAsync<ArgumentNullException>(() => sut.MapAsync(o => { }, null)).ConfigureAwait(false);
		}

		[Fact]
		public async Task Null_SOME_callback_throws_exception_async()
		{
			var sut = new Maybe<object>(new object());

			await Assert.ThrowsAsync<ArgumentNullException>(() => sut.MapAsync(null)).ConfigureAwait(false);
		}
	}

	public class MaybeReturnMapAsyncTests
	{
		[Fact]
		public async Task NOTHING_returns_result()
		{
			var expectedObject = new object();

			var sut = new Maybe<object>();

			var actualObject = await sut.MapAsync(x => null, () => expectedObject);

			Assert.Equal(expectedObject, actualObject);
		}

		[Fact]
		public async Task Null_NOTHING_callback_throws_exception()
		{
			var sut = new Maybe<object>(new object());

			await Assert.ThrowsAsync<ArgumentNullException>(() => sut.MapAsync(x => Task.FromResult(new object()), null));
		}

		[Fact]
		public async Task Null_SOME_callback_throws_exception()
		{
			var sut = new Maybe<object>(new object());

			await Assert.ThrowsAsync<ArgumentNullException>(() => sut.MapAsync(null, () => new object()));
		}

		[Fact]
		public async Task SOME_returns_result()
		{
			var expectedObject = new object();

			var sut = new Maybe<object>(expectedObject);

			var actualObject = await sut.MapAsync(x => x, () => null);

			Assert.Equal(expectedObject, actualObject);
		}
	}

	public class MaybeFromResultAsyncTests
	{
		[Fact]
		public async Task Handling_maybe_of_task_of_object()
		{
			var expected = new object();

			Task<object> Func() => Task.FromResult(expected);

			var actual = await Maybe<Task<object>>.FromResultAsync(Func).ConfigureAwait(false);

			Assert.Same(expected, await actual.ValueOrDefault(Task.FromResult(new object())).ConfigureAwait(false));
		}

		[Fact]
		public async Task Wrap_method_null_output_into_maybe_async()
		{
			var expected = new object();

			object Func() => null;

			var actual = await Maybe<object>.FromResultAsync(Func).ConfigureAwait(false);

			Assert.Same(expected, actual.ValueOrDefault(expected));
		}

		[Fact]
		public async Task Wrap_method_output_into_maybe_async()
		{
			var expected = new object();

			object Func() => expected;

			var actual = await Maybe<object>.FromResultAsync(Func).ConfigureAwait(false);

			Assert.Same(expected, actual.ValueOrDefault(new object()));
		}
	}
}