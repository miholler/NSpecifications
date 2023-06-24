using NSpecifications.Internal;
using static System.Linq.Expressions.Expression;

namespace NSpecifications;

public static class PredicateBuilder
{
    public static Expression<Func<T, bool>> Any<T>()
        => _ => true;

    public static Expression<Func<T, bool>> None<T>()
        => _ => false;

    public static Expression<Func<T, bool>> Create<T>(Expression<Func<T, bool>> predicate)
        => predicate;

    public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> predicate)
        => Lambda<Func<T, bool>>(Expression.Not(predicate.Body), predicate.Parameters);

    public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        => left.Merge(right, AndAlso);

    public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        => left.Merge(right, OrElse);

    private static Expression<TDelegate> Merge<TDelegate>(this Expression<TDelegate> left, Expression<TDelegate> right, Func<Expression, Expression, Expression> merger)
    {
        // zip parameters (map from parameters of right to parameters of left)
        var map = left.Parameters
            .Zip(right.Parameters, (l, r) => new
            {
                Left = l,
                Right = r
            })
            .ToDictionary(x => x.Right, x => x.Left);

        // replace parameters in the right lambda expression with the parameters in the left
        var rightBody = ParameterRebinder.Rebind(right.Body, map);

        // create a merged lambda expression with parameters from the left expression
        return Lambda<TDelegate>(merger(left.Body, rightBody), left.Parameters);
    }
}
