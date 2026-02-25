using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using SoftwareCraft.Functional;

// BenchmarkRunner.Run<EagerLiftingBenchmarks>();
// BenchmarkRunner.Run<LazyLiftingBenchmarks>();
BenchmarkRunner.Run<LazyLiftingAsyncBenchmarks>();

[MemoryDiagnoser]
public class EagerLiftingBenchmarks
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

[MemoryDiagnoser]
public class LazyLiftingBenchmarks
{
  private readonly Func<Maybe<int>>    f1 = () => Maybe.Some(13);
  private readonly Func<Maybe<string>> f2 = () => Maybe.Some("hello");

  private readonly Func<Maybe<int>>    m3 = () => Maybe.None<int>();
  private readonly Func<Maybe<string>> m4 = () => Maybe.None<string>();

  [Benchmark]
  public Maybe<Tuple<int, string>> LazyLift2A() => Maybe.Lifting.LiftLazy(f1, f2);

  [Benchmark]
  public Maybe<Tuple<int, string>> LazyLift2B() => Maybe.Lifting.LiftLazy(f1, m4);

  [Benchmark]
  public Maybe<Tuple<int, string>> LazyLift2C() => Maybe.Lifting.LiftLazy(m3, f2);

  [Benchmark]
  public Maybe<Tuple<int, string>> LazyLift2D() => Maybe.Lifting.LiftLazy(m3, m4);
}

[MemoryDiagnoser]
public class LazyLiftingAsyncBenchmarks
{
  private readonly Func<Task<Maybe<int>>> f1 = async () =>
                                               {
                                                 await Task.Delay(1);

                                                 return Maybe.Some(13);
                                               };

  private readonly Func<Task<Maybe<string>>> f2 = async () =>
                                                  {
                                                    await Task.Delay(1);

                                                    return Maybe.Some("hello");
                                                  };

  private readonly Func<Task<Maybe<int>>> m3 = async () =>
                                               {
                                                 await Task.Delay(1);

                                                 return Maybe.None<int>();
                                               };

  private readonly Func<Task<Maybe<string>>> m4 = async () =>
                                                  {
                                                    await Task.Delay(1);

                                                    return Maybe.None<string>();
                                                  };

  [Benchmark]
  public Task<Maybe<Tuple<int, string>>> LazyLift2A() => Maybe.Lifting.LiftLazyAsync(f1, f2);

  [Benchmark]
  public Task<Maybe<Tuple<int, string>>> LazyLift2B() => Maybe.Lifting.LiftLazyAsync(f1, m4);

  [Benchmark]
  public Task<Maybe<Tuple<int, string>>> LazyLift2C() => Maybe.Lifting.LiftLazyAsync(m3, f2);

  [Benchmark]
  public Task<Maybe<Tuple<int, string>>> LazyLift2D() => Maybe.Lifting.LiftLazyAsync(m3, m4);
}