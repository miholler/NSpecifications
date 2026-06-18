using static System.Linq.Expressions.Expression;

namespace NSpecifications.Internal;

internal static class PredicateBuilder
{
    public static Expression<Func<TTo, bool>> CastUp<TFrom, TTo>(Expression<Func<TFrom, bool>> predicate) where TTo : TFrom
    {
        ArgumentNullException.ThrowIfNull(predicate);

        var map = predicate.Parameters.ToDictionary(x => x, x => Parameter(typeof(TTo), x.Name));
        var body = ParameterReplacer.Replace(predicate.Body, map);
        return Lambda<Func<TTo, bool>>(body, map.Values);
    }

    public static Expression<Func<T, bool>> Not<T>(Expression<Func<T, bool>> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        return Lambda<Func<T, bool>>(Expression.Not(predicate.Body), predicate.Parameters);
    }

    public static Expression<Func<T, bool>> AndAlso<T>(Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return left.Merge(right, Expression.AndAlso);
    }

    public static Expression<Func<T, bool>> OrElse<T>(Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return left.Merge(right, Expression.OrElse);
    }

    private static Expression<TDelegate> Merge<TDelegate>(this Expression<TDelegate> left, Expression<TDelegate> right, Func<Expression, Expression, Expression> merger)
    {
        // Zip parameters (map from parameters of right to parameters of left)
        var map = left.Parameters
            .Zip(right.Parameters, (l, r) => (Left: l, Right: r))
            .ToDictionary(x => x.Right, x => x.Left);

        // Replace parameters in the right lambda expression with the parameters in the left
        var rightBody = ParameterReplacer.Replace(right.Body, map);

        // Create a merged lambda expression with parameters from the left expression
        return Lambda<TDelegate>(merger(left.Body, rightBody), left.Parameters);
    }
}
