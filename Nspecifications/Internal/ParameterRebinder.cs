namespace NSpecifications.Internal;

public sealed class ParameterRebinder : ExpressionVisitor
{
    private readonly IReadOnlyDictionary<ParameterExpression, ParameterExpression> _map;

    public ParameterRebinder(IReadOnlyDictionary<ParameterExpression, ParameterExpression> map)
    {
        _map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
    }

    public static Expression Rebind(Expression expression, IReadOnlyDictionary<ParameterExpression, ParameterExpression> map)
        => new ParameterRebinder(map).Visit(expression);

    protected override Expression VisitParameter(ParameterExpression node)
    {
        if (_map.TryGetValue(node, out var replacement))
            node = replacement;

        return base.VisitParameter(node);
    }
}
