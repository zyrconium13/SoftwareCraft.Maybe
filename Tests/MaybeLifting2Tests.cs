namespace Tests;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareCraft.Functional;

[TestClass]
public sealed class MaybeLifting2Tests
{
  #region Eager Lift

  [TestMethod]
  public void EagerLiftTwoSome()
  {
    var m1 = Maybe.Some(13);
    var m2 = Maybe.Some("hello");

    var tuple = Maybe.Lifting.Lift2(m1, m2);

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

    var tuple = Maybe.Lifting.Lift2(m1, m2);

    Assert.IsTrue(tuple.IsNone);
  }

  [TestMethod]
  public void EagerLiftSomeAndNone()
  {
    var m1 = Maybe.Some(13);
    var m2 = Maybe.None<string>();

    var tuple = Maybe.Lifting.Lift2(m1, m2);

    Assert.IsTrue(tuple.IsNone);
  }

  [TestMethod]
  public void EagerLiftTwoNone()
  {
    var m1 = Maybe.None<int>();
    var m2 = Maybe.None<string>();

    var tuple = Maybe.Lifting.Lift2(m1, m2);

    Assert.IsTrue(tuple.IsNone);
  }

  #endregion
}