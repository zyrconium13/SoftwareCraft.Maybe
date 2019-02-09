using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareCraft.Functional;
using Tests.Properties;

namespace Tests
{
	[TestClass]
	public class MaybeReferenceTypesTests
	{
		[TestMethod]
		public void NullThrowsException()
		{
			Assert.ThrowsException<ArgumentNullException>(() => Maybe.Some((SampleReferenceType) null));
		}

		[TestMethod]
		public void SelectDelegatesCannotBeNull()
		{
			Assert.ThrowsException<ArgumentNullException>(() =>
				Maybe.Some(new SampleReferenceType()).Select<double>(null));
			Assert.ThrowsException<ArgumentNullException>(() => Maybe.Some(new SampleReferenceType()).Select(null, () =>
			{
				Assert.Fail(Resources.Maybe_ThisShouldNotBeCalled);
				return 0.0;
			}));
			Assert.ThrowsException<ArgumentNullException>(() =>
				Maybe.Some(new SampleReferenceType()).Select(i =>
				{
					Assert.Fail(Resources.Maybe_ThisShouldNotBeCalled);
					return 0.0;
				}, null));

			Assert.ThrowsException<ArgumentNullException>(() => Maybe.None<SampleReferenceType>().Select<double>(null));
			Assert.ThrowsException<ArgumentNullException>(() => Maybe.None<SampleReferenceType>().Select(null, () =>
			{
				Assert.Fail(Resources.Maybe_ThisShouldNotBeCalled);
				return 0.0;
			}));
			Assert.ThrowsException<ArgumentNullException>(() =>
				Maybe.None<SampleReferenceType>().Select(i =>
				{
					Assert.Fail(Resources.Maybe_ThisShouldNotBeCalled);
					return 0.0;
				}, null));
		}

		[TestMethod]
		public void ReactToTheExistenceOfAValue()
		{
			// Arrange
			var maybe = Maybe.Some(new SampleReferenceType());

			// Act
			var expectedType = new AnotherReferenceType();
			var result = maybe.Select(x => expectedType);

			// Assert
			result.Match(actualType => Assert.AreSame(expectedType, actualType),
				() => Assert.Fail(Resources.Maybe_ShouldHaveAValue));
		}

		[TestMethod]
		public void ReactToTheExistenceOfAValue_2()
		{
			// Arrange
			var maybe = Maybe.Some(new SampleReferenceType());

			// Act
			var expectedType = new AnotherReferenceType();
			var result = maybe.Select(x => expectedType,
				() =>
				{
					Assert.Fail(Resources.Maybe_ThisShouldNotBeCalled);
					return new AnotherReferenceType();
				});

			// Assert
			result.Match(actualType => Assert.AreSame(expectedType, actualType),
				() => Assert.Fail(Resources.Maybe_ShouldHaveAValue));
		}

		[TestMethod]
		public void ReactToTheLackOfAValue()
		{
			// Arrange
			var maybe = Maybe.None<SampleReferenceType>();

			// Act
			var typeA = new AnotherReferenceType();
			var typeB = new AnotherReferenceType();
			var result = maybe.Select(x => typeA, () => typeB);

			// Assert
			result.Match(type => Assert.AreSame(typeB, type), () => Assert.Fail(Resources.Maybe_ShouldHaveAValue));
		}

		[TestMethod]
		public void MatchDelegatesCannotBeNull()
		{
			Assert.ThrowsException<ArgumentNullException>(() =>
				Maybe.Some(new SampleReferenceType())
					.Match(null, () => Assert.Fail(Resources.Maybe_ThisShouldNotBeCalled)));
			Assert.ThrowsException<ArgumentNullException>(() =>
				Maybe.Some(new SampleReferenceType())
					.Match(i => Assert.Fail(Resources.Maybe_ThisShouldNotBeCalled), null));

			Assert.ThrowsException<ArgumentNullException>(() =>
				Maybe.None<SampleReferenceType>()
					.Match(null, () => Assert.Fail(Resources.Maybe_ThisShouldNotBeCalled)));
			Assert.ThrowsException<ArgumentNullException>(() =>
				Maybe.None<SampleReferenceType>().Match(i => Assert.Fail(Resources.Maybe_ThisShouldNotBeCalled), null));
		}

		[TestMethod]
		public void MatchOnSomePassesContainedValue()
		{
			// Arrange
			var expectedType = new SampleReferenceType();

			// Act
			var maybe = Maybe.Some(expectedType);

			// Act
			maybe.Match(actualType => Assert.AreSame(expectedType, actualType),
				() => Assert.Fail(Resources.Maybe_ShouldHaveAValue));
		}

		[TestMethod]
		public void MatchOnNoneOnlyCallsDelegate()
		{
			// Arrange
			var maybe = Maybe.None<SampleReferenceType>();

			// Act & Assert
			maybe.Match(x => Assert.Fail(Resources.Maybe_ShouldNotHaveAValue), () => Assert.IsTrue(true));
		}

		[TestMethod]
		public void X()
		{
			var maybe = Maybe.Some(new SampleReferenceType());
			Assert.IsInstanceOfType(maybe, typeof(Maybe<SampleReferenceType>));

			var result = maybe.SelectMany(i => Maybe.None<AnotherReferenceType>());

			Assert.IsInstanceOfType(result, typeof(Maybe<AnotherReferenceType>));
		}

		[TestMethod]
		public void W()
		{
			var maybe = Maybe.Some(new SampleReferenceType());
			Assert.IsInstanceOfType(maybe, typeof(Maybe<SampleReferenceType>));

			var result = maybe.SelectMany(i => Maybe.None<AnotherReferenceType>(), () =>
			{
				Assert.Fail(Resources.Maybe_ThisShouldNotBeCalled);
				return Maybe.Some(new AnotherReferenceType());
			});

			Assert.IsInstanceOfType(result, typeof(Maybe<AnotherReferenceType>));
		}

		[TestMethod]
		public void Y()
		{
			var maybe = Maybe.None<SampleReferenceType>();
			Assert.IsInstanceOfType(maybe, typeof(Maybe<SampleReferenceType>));

			var result = maybe.SelectMany(i =>
			{
				Assert.Fail(Resources.Maybe_ThisShouldNotBeCalled);
				return Maybe.None<AnotherReferenceType>();
			}, () => Maybe.Some(new AnotherReferenceType()));

			Assert.IsInstanceOfType(result, typeof(Maybe<AnotherReferenceType>));
		}

		[TestMethod]
		public void Z()
		{
			var maybe = Maybe.None<SampleReferenceType>();
			Assert.IsInstanceOfType(maybe, typeof(Maybe<SampleReferenceType>));

			var result = maybe.SelectMany(i =>
			{
				Assert.Fail(Resources.Maybe_ThisShouldNotBeCalled);
				return Maybe.Some(new SampleReferenceType());
			});

			Assert.IsInstanceOfType(result, typeof(Maybe<SampleReferenceType>));
		}

		[TestMethod]
		public void EqualityTests()
		{
			var noneA = Maybe.None<SampleReferenceType>();
			var noneB = Maybe.None<SampleReferenceType>();
			var someA = Maybe.Some(new SampleReferenceType());
			var someB = Maybe.Some(new SampleReferenceType());
			var someC = Maybe.Some(new AnotherReferenceType());

			Assert.AreEqual(noneA, noneB);
			Assert.AreNotEqual(noneA, someA);
			Assert.AreNotEqual(someA, noneA);
			Assert.AreNotEqual(someA, someC);
			Assert.AreNotEqual(someB, someC);
			Assert.AreNotSame(null, someA);
			Assert.AreNotSame(null, noneA);
			Assert.AreNotSame(someA, someB);
			Assert.AreSame(someA, someA);
			Assert.AreSame(noneA, noneA);

			Assert.IsFalse(someA == null);
			Assert.IsFalse(noneA == null);

			Assert.AreNotEqual(someA.GetHashCode(), someB.GetHashCode());
			Assert.AreNotEqual(someA.GetHashCode(), someC.GetHashCode());
			Assert.AreEqual(noneA.GetHashCode(), noneB.GetHashCode());
		}

		[TestMethod]
		public void StringRepresentation()
		{
			var some = Maybe.Some(new SampleReferenceType());

			Assert.AreEqual(typeof(SampleReferenceType).FullName, some.ToString());

			var none = Maybe.None<SampleReferenceType>();

			Assert.AreEqual(string.Empty, none.ToString());
		}
	}

	[TestClass]
	public class MaybeValueTypesTests
	{
		[TestMethod]
		public void NullThrowsException()
		{
			Assert.ThrowsException<ArgumentNullException>(() => Maybe.Some<int?>(null));
		}

		[TestMethod]
		public void SelectDelegatesCannotBeNull()
		{
			Assert.ThrowsException<ArgumentNullException>(() => Maybe.Some(13).Select<double>(null));
			Assert.ThrowsException<ArgumentNullException>(() => Maybe.Some(13).Select(null, () =>
			{
				Assert.Fail(Resources.Maybe_ThisShouldNotBeCalled);
				return 0.0;
			}));
			Assert.ThrowsException<ArgumentNullException>(() =>
				Maybe.Some(13).Select(i =>
				{
					Assert.Fail(Resources.Maybe_ThisShouldNotBeCalled);
					return 0.0;
				}, null));

			Assert.ThrowsException<ArgumentNullException>(() => Maybe.None<int>().Select<double>(null));
			Assert.ThrowsException<ArgumentNullException>(() => Maybe.None<int>().Select(null, () =>
			{
				Assert.Fail(Resources.Maybe_ThisShouldNotBeCalled);
				return 0.0;
			}));
			Assert.ThrowsException<ArgumentNullException>(() =>
				Maybe.None<int>().Select(i =>
				{
					Assert.Fail(Resources.Maybe_ThisShouldNotBeCalled);
					return 0.0;
				}, null));
		}

		[TestMethod]
		public void ReactToTheExistenceOfAValue()
		{
			// Arrange
			var maybe = Maybe.Some(13);

			// Act
			var expectedValue = 18.0;
			var result = maybe.Select(x => expectedValue);

			// Assert
			result.Match(actualValue => Assert.AreEqual(expectedValue, actualValue),
				() => Assert.Fail(Resources.Maybe_ShouldHaveAValue));
		}

		[TestMethod]
		public void ReactToTheExistenceOfAValue_2()
		{
			// Arrange
			var maybe = Maybe.Some(13);

			// Act
			var expectedValue = 18.0;
			var result = maybe.Select(x => expectedValue, () =>
			{
				Assert.Fail(Resources.Maybe_ThisShouldNotBeCalled);
				return 0.0;
			});

			// Assert
			result.Match(actualValue => Assert.AreEqual(expectedValue, actualValue),
				() => Assert.Fail(Resources.Maybe_ShouldHaveAValue));
		}

		[TestMethod]
		public void ReactToTheLackOfAValue()
		{
			// Arrange
			var maybe = Maybe.None<int>();

			// Act
			var result = maybe.Select(x =>
			{
				Assert.Fail(Resources.Maybe_ThisShouldNotBeCalled);
				return 13;
			}, () => 14);

			// Assert
			result.Match(i => Assert.AreEqual(14, i), () => Assert.Fail(Resources.Maybe_ShouldHaveAValue));
		}

		[TestMethod]
		public void MatchDelegatesCannotBeNull()
		{
			Assert.ThrowsException<ArgumentNullException>(() =>
				Maybe.Some(13).Match(null, () => Assert.Fail(Resources.Maybe_ThisShouldNotBeCalled)));
			Assert.ThrowsException<ArgumentNullException>(() =>
				Maybe.Some(13).Match(i => Assert.Fail(Resources.Maybe_ThisShouldNotBeCalled), null));

			Assert.ThrowsException<ArgumentNullException>(() =>
				Maybe.None<int>().Match(null, () => Assert.Fail(Resources.Maybe_ThisShouldNotBeCalled)));
			Assert.ThrowsException<ArgumentNullException>(() =>
				Maybe.None<int>().Match(i => Assert.Fail(Resources.Maybe_ThisShouldNotBeCalled), null));
		}

		[TestMethod]
		public void MatchOnSomePassesContainedValue()
		{
			// Arrange
			var expectedValue = 13;

			var maybe = Maybe.Some(expectedValue);

			// Act & Assert
			maybe.Match(actualValue => Assert.AreEqual(expectedValue, actualValue),
				() => Assert.Fail(Resources.Maybe_ShouldHaveAValue));
		}

		[TestMethod]
		public void MatchOnNoneOnlyCallsDelegate()
		{
			// Arrange
			var maybe = Maybe.None<int>();

			// Act & Assert
			maybe.Match(x => Assert.Fail(Resources.Maybe_ShouldNotHaveAValue), () => Assert.IsTrue(true));
		}

		[TestMethod]
		public void SelectManyDelegatesCannotBeNull()
		{
			Assert.ThrowsException<ArgumentNullException>(() => Maybe.Some(13).SelectMany<double>(null));
			Assert.ThrowsException<ArgumentNullException>(() => Maybe.Some(13).SelectMany(null, () =>
			{
				Assert.Fail(Resources.Maybe_ThisShouldNotBeCalled);
				return Maybe.None<double>();
			}));
			Assert.ThrowsException<ArgumentNullException>(() =>
				Maybe.Some(13).SelectMany(i =>
				{
					Assert.Fail(Resources.Maybe_ThisShouldNotBeCalled);
					return Maybe.None<double>();
				}, null));

			Assert.ThrowsException<ArgumentNullException>(() => Maybe.None<int>().SelectMany<double>(null));
			Assert.ThrowsException<ArgumentNullException>(() => Maybe.None<int>().SelectMany(null, () =>
			{
				Assert.Fail(Resources.Maybe_ThisShouldNotBeCalled);
				return Maybe.None<double>();
			}));
			Assert.ThrowsException<ArgumentNullException>(() =>
				Maybe.None<int>().SelectMany(i =>
				{
					Assert.Fail(Resources.Maybe_ThisShouldNotBeCalled);
					return Maybe.None<double>();
				}, null));
		}

		[TestMethod]
		public void X()
		{
			var maybe = Maybe.Some(13);
			Assert.IsInstanceOfType(maybe, typeof(Maybe<int>));

			var result = maybe.SelectMany(i => Maybe.None<double>());

			Assert.IsInstanceOfType(result, typeof(Maybe<double>));
		}

		[TestMethod]
		public void W()
		{
			var maybe = Maybe.Some(13);
			Assert.IsInstanceOfType(maybe, typeof(Maybe<int>));

			var result = maybe.SelectMany(i => Maybe.None<double>(), () =>
			{
				Assert.Fail(Resources.Maybe_ThisShouldNotBeCalled);
				return Maybe.Some(0.0);
			});

			Assert.IsInstanceOfType(result, typeof(Maybe<double>));
		}

		[TestMethod]
		public void Y()
		{
			var maybe = Maybe.None<int>();
			Assert.IsInstanceOfType(maybe, typeof(Maybe<int>));

			var result = maybe.SelectMany(i =>
			{
				Assert.Fail(Resources.Maybe_ThisShouldNotBeCalled);
				return Maybe.None<double>();
			}, () => Maybe.Some(13.0));

			Assert.IsInstanceOfType(result, typeof(Maybe<double>));
		}

		[TestMethod]
		public void Z()
		{
			var maybe = Maybe.None<int>();
			Assert.IsInstanceOfType(maybe, typeof(Maybe<int>));

			var result = maybe.SelectMany(i =>
			{
				Assert.Fail(Resources.Maybe_ThisShouldNotBeCalled);
				return Maybe.Some(13.0);
			});

			Assert.IsInstanceOfType(result, typeof(Maybe<double>));
		}

		[TestMethod]
		public void EqualityTests()
		{
			var noneA = Maybe.None<int>();
			var noneB = Maybe.None<int>();
			var someA = Maybe.Some(13);
			var someB = Maybe.Some(13);
			var someC = Maybe.Some(14);

			Assert.AreEqual(noneA, noneB);
			Assert.AreNotEqual(noneA, someA);
			Assert.AreNotEqual(someA, noneA);
			Assert.AreEqual(someA, someB);
			Assert.AreNotEqual(someA, someC);
			Assert.AreNotEqual(someB, someC);
			Assert.AreNotSame(null, someA);
			Assert.AreNotSame(null, noneA);
			Assert.AreNotSame(someA, someB);
			Assert.AreSame(someA, someA);
			Assert.AreSame(noneA, noneA);

			Assert.IsTrue(someA == someB);
			Assert.IsTrue(someA != someC);

			Assert.IsFalse(someA == null);
			Assert.IsFalse(noneA == null);

			Assert.AreEqual(someA.GetHashCode(), someB.GetHashCode());
			Assert.AreNotEqual(someA.GetHashCode(), someC.GetHashCode());
			Assert.AreEqual(noneA.GetHashCode(), noneB.GetHashCode());
		}

		[TestMethod]
		public void StringRepresentation()
		{
			var some = Maybe.Some(13);

			Assert.AreEqual("13", some.ToString());

			var none = Maybe.None<int>();

			Assert.AreEqual(string.Empty, none.ToString());
		}
	}

	internal class SampleReferenceType { }

	internal class AnotherReferenceType { }
}