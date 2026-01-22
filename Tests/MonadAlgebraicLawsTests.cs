namespace Tests;

using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareCraft.Functional;

[TestClass]
public class MonadAlgebraicLawsTests
{
  // Monad functions
  // map :: (a -> b) -> (M a -> M b) == Select
  // unit :: a -> M a == Ctor (aka. return)
  // join :: M (M a) -> M a == Join
  // bind :: M a -> (a -> M b) -> M b == SelectMany

  // Functor :: Law1 & Law2 (has map)
  // Premonad :: Functor + Law3 (has unit)
  // Monad :: Premonad + Law4 & Law5 & Law6 & Law7 (has join)

  [TestMethod]
  // map id == id
  public void Law1()
  {
    var m = Maybe.Some(13);

    Assert.AreEqual(
      m.Select( // map
        Functions.Id // id
      ),
      m.Id() // id
    );
  }

  [TestMethod]
  // map f . map g == map (f . g)
  public void Law2()
  {
    Func<int, double>    g = i => i;
    Func<double, string> f = d => d.ToString(CultureInfo.InvariantCulture);

    var m = Maybe.Some(13);

    Assert.AreEqual(
      m.Select(g) // map g
       .Select(f), // map f
      m.Select(x => // map
                 f(g(x)) // f . g
      )
    );
  }

  [TestMethod]
  // unit . f == map f . unit
  public void Law3()
  {
    Func<int, string> f = i => i.ToString();

    var x = 13;

    Assert.AreEqual(
      Maybe.Some( // unit
        f(x) // f x
      ),
      Maybe.Some(x) // unit
           .Select(f) // map f ma
    );
  }

  [TestMethod]
  // join . map (map f) == map f . join
  public void Law4()
  {
    Func<int, string> f = i => i.ToString();

    // simulate partial function application: map f :: (M a -> M b)
    Func<Maybe<int>, Maybe<string>> g = ma => ma.Select(f);

    var mma = Maybe.Some(Maybe.Some(13));

    Assert.AreEqual(
      mma.Select(g) // map (map f); map g
         .Join(), // join
      mma.Join() // join
         .Select(f) // map f mma
    );
  }

  [TestMethod]
  // join . unit == id
  public void Law5()
  {
    var ma = Maybe.Some(13);

    Assert.AreEqual(
      Maybe.Some(ma) // unit
           .Join(), // join
      ma.Id() // id
    );
  }

  [TestMethod]
  // join . map unit == id
  public void Law6()
  {
    var ma = Maybe.Some(13);

    Assert.AreEqual(
      ma.Select(i => // map
                  Maybe.Some(i)) // unit
        .Join(), // join
      ma.Id() // id
    );
  }

  [TestMethod]
  // join . map join == join . join
  public void Law7()
  {
    var mmma = Maybe.Some(Maybe.Some(Maybe.Some(13)));

    Assert.AreEqual(
      mmma.Select(mma => // map
                    mma.Join()) // join
          .Join(), // join
      mmma
       .Join() // join
       .Join() // join
    );
  }

  [TestMethod]
  // join == bind id
  public void Law8()
  {
    var mma = Maybe.Some(Maybe.Some(13));

    Assert.AreEqual(
      mma.Join(), // join
      mma.SelectMany( // bind
        Functions.Id // id
      )
    );
  }
}