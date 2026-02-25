namespace Tests;

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftwareCraft.Functional;

[TestClass]
internal sealed class MaybeAsyncTests
{
  [TestMethod]
  public async Task Test1()
  {
    var meanMaybe = Mean([1, 2, 3, 4]);

    var messageA = await meanMaybe
                        .SelectAsync(MultiplyAndRoundAsync)
                        .SelectManyAsync(DescribeAsync)
                        .MatchAsync(async desc =>
                                    {
                                      await Task.Delay(1);

                                      return $"This is the description: {desc}";
                                    }
                                  , () => Task.FromResult("There was no description."));

    var messageB = await meanMaybe
                        .Select(MultiplyAndRound)
                        .SelectManyAsync(DescribeAsync)
                        .MatchAsync(async desc =>
                                    {
                                      await Task.Delay(1);

                                      return $"This is the description: {desc}";
                                    }
                                  , () => Task.FromResult("There was no description."));;

    var messageC = await meanMaybe
                        .SelectAsync(MultiplyAndRoundAsync)
                        .SelectMany(Describe)
                        .MatchAsync(async desc =>
                                    {
                                      await Task.Delay(1);

                                      return $"This is the description: {desc}";
                                    }
                                  , () => Task.FromResult("There was no description."));;

    var messageD = meanMaybe
                  .Select(MultiplyAndRound)
                  .SelectMany(Describe)
                  .MatchAsync(async desc =>
                              {
                                await Task.Delay(1);

                                return $"This is the description: {desc}";
                              }
                            , () => Task.FromResult("There was no description."));;
  }

  private static Maybe<double> Mean(int[] numbers) =>
    numbers.Length == 0
      ? Maybe.None<double>()
      : numbers.Average().AsMaybe();

  private static int MultiplyAndRound(double mean) => (int)Math.Round(mean * 10);

  private static async Task<int> MultiplyAndRoundAsync(double mean)
  {
    await Task.Delay(1);

    return (int)Math.Round(mean * 10);
  }

  private static Maybe<string> Describe(int number) =>
    number % 2 == 0
      ? $"{number} is a nice even number".AsMaybe()
      : Maybe.None<string>();

  private static async Task<Maybe<string>> DescribeAsync(int number)
  {
    await Task.Delay(1);

    return number % 2 == 0
             ? $"{number} is a nice even number".AsMaybe()
             : Maybe.None<string>();
  }
}