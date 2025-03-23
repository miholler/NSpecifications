using static System.Linq.Expressions.Expression;

namespace NSpecifications.Internal;

internal static class PredicateBuilder
{
    public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> predicate)
        => Lambda<Func<T, bool>>(Expression.Not(predicate.Body), predicate.Parameters);

    public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        => left.Merge(right, Expression.AndAlso);

    public static Expression<Func<T, bool>> OrElse<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        => left.Merge(right, Expression.OrElse);

    private static Expression<TDelegate> Merge<TDelegate>(this Expression<TDelegate> left, Expression<TDelegate> right, Func<Expression, Expression, Expression> merger)
    {
        // Zip parameters (map from parameters of right to parameters of left)
        var map = left.Parameters
            .Zip(right.Parameters, (l, r) => new
            {
                Left = l,
                Right = r
            })
            .ToDictionary(x => x.Right, x => x.Left);

        // Replace parameters in the right lambda expression with the parameters in the left
        var rightBody = ParameterReplacer.Replace(right.Body, map);

        // Create a merged lambda expression with parameters from the left expression
        return Lambda<TDelegate>(merger(left.Body, rightBody), left.Parameters);
    }
}

internal static class PredicateBuilder<T>
{
    public static Expression<Func<TDerived, bool>> CastUp<TDerived>(Expression<Func<T, bool>> predicate) where TDerived : T
    {
        var map = predicate.Parameters.ToDictionary(x => x, x => Parameter(typeof(TDerived), x.Name));
        var body = ParameterReplacer.Replace(predicate.Body, map);
        return Lambda<Func<TDerived, bool>>(body, map.Values);
    }
}
