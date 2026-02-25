namespace Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareCraft.Functional;

[TestClass]
public sealed class MaybeLifting2Tests
{
  #region Lazy Lift

  [TestMethod]
  public void LazyLiftTwoSome()
  {
    var f1 = () => Maybe.Some(13);
    var f2 = () => Maybe.Some("hello");

    var tuple = Maybe.Lifting.LiftLazy(f1, f2);

    tuple.Match(
      t =>
      {
        Assert.AreEqual(13,      t.Item1);
        Assert.AreEqual("hello", t.Item2);
      }
    , () => Assert.Fail("This was not supposed to fail!"));
  }

  [TestMethod]
  public void LazyLiftNoneAndSome()
  {
    var f1 = () => Maybe.None<int>();
    var f2 = () => Maybe.Some("hello");

    var tuple = Maybe.Lifting.LiftLazy(f1, f2);

    Assert.IsTrue(tuple.IsNone);
  }

  [TestMethod]
  public void LazyLiftSomeAndNone()
  {
    var f1 = () => Maybe.Some(13);
    var f2 = () => Maybe.None<string>();

    var tuple = Maybe.Lifting.LiftLazy(f1, f2);

    Assert.IsTrue(tuple.IsNone);
  }

  [TestMethod]
  public void LazyLiftTwoNone()
  {
    var f1 = () => Maybe.None<int>();
    var f2 = () => Maybe.None<string>();

    var tuple = Maybe.Lifting.LiftLazy(f1, f2);

    Assert.IsTrue(tuple.IsNone);
  }

  #endregion

  #region Eager Lift

  [TestMethod]
  public void EagerLiftTwoSome()
  {
    var m1 = Maybe.Some(13);
    var m2 = Maybe.Some("hello");

    var tuple = Maybe.Lifting.Lift(m1, m2);

    tuple.Match(
      t =>
      {
        Assert.AreEqual(13,      t.Item1);
        Assert.AreEqual("hello", t.Item2);
      }
    , () => Assert.Fail("This was not supposed to fail!"));
  }

  [TestMethod]
  public void EagerLiftNoneAndSome()
  {
    var m1 = Maybe.None<int>();
    var m2 = Maybe.Some("hello");

    var tuple = Maybe.Lifting.Lift(m1, m2);

    Assert.IsTrue(tuple.IsNone);
  }

  [TestMethod]
  public void EagerLiftSomeAndNone()
  {
    var m1 = Maybe.Some(13);
    var m2 = Maybe.None<string>();

    var tuple = Maybe.Lifting.Lift(m1, m2);

    Assert.IsTrue(tuple.IsNone);
  }

  [TestMethod]
  public void EagerLiftTwoNone()
  {
    var m1 = Maybe.None<int>();
    var m2 = Maybe.None<string>();

    var tuple = Maybe.Lifting.Lift(m1, m2);

    Assert.IsTrue(tuple.IsNone);
  }

  #endregion
}