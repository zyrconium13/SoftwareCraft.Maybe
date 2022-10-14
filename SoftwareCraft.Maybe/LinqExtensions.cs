using System;

namespace SoftwareCraft.Functional;

public static class LinqExtensions
{
    public static Maybe<TV> SelectMany<T, TU, TV>(
        this Maybe<T> first,
        Func<T, Maybe<TU>> selector,
        Func<T, TU, TV> projector) => first.SelectMany(
            some1 => selector(some1)
                .SelectMany(
                    some2 => Maybe.Some(projector(some1, some2)),
                    Maybe.None<TV>),
            Maybe.None<TV>);
}