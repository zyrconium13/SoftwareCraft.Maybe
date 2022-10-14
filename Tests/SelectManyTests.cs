using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareCraft.Functional;

namespace Tests;

[TestClass]
public class SelectManyTests
{
    [TestMethod]
    public void Some_Plus_Some_Equals_Some()
    {
        var m1 = Maybe.Some(13);
        var m2 = Maybe.Some(42);

        var x = from a in m1
            from b in m2
            select a + b;
        Assert.AreEqual(x, Maybe.Some(13 + 42));
    }
    
    [TestMethod]
    public void Some_Plus_None_Equals_None()
    {
        var m1 = Maybe.Some(13);
        var m2 = Maybe.None<int>();

        var x = from a in m1
            from b in m2
            select a + b;
        Assert.AreEqual(x, Maybe.None<int>());
    }
    
    [TestMethod]
    public void None_Plus_Some_Equals_None()
    {
        var m1 = Maybe.None<int>();
        var m2 = Maybe.Some(42);

        var x = from a in m1
            from b in m2
            select a + b;
        Assert.AreEqual(x, Maybe.None<int>());
    }
    
    [TestMethod]
    public void None_Plus_None_Equals_None()
    {
        var m1 = Maybe.None<int>();
        var m2 = Maybe.None<int>();

        var x = from a in m1
            from b in m2
            select a + b;
        Assert.AreEqual(x, Maybe.None<int>());
    }
}