using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using SoftwareCraft.Functional;

var summary = BenchmarkRunner.Run<LiftingBenchmarks>();

[MemoryDiagnoser]
public class LiftingBenchmarks
{
  private readonly Maybe<int>    m1 = Maybe.Some(13);
  private readonly Maybe<string> m2 = Maybe.Some("hello");

  private readonly Maybe<int>    m3 = Maybe.None<int>();
  private readonly Maybe<string> m4 = Maybe.None<string>();

  [Benchmark]
  public Maybe<Tuple<int, string>> EagerLift2A() => Maybe.Lifting.Lift(m1, m2);

  [Benchmark]
  public Maybe<Tuple<int, string>> EagerLift2B() => Maybe.Lifting.Lift(m1, m4);

  [Benchmark]
  public Maybe<Tuple<int, string>> EagerLift2C() => Maybe.Lifting.Lift(m3, m2);

  [Benchmark]
  public Maybe<Tuple<int, string>> EagerLift2D() => Maybe.Lifting.Lift(m3, m4);
}