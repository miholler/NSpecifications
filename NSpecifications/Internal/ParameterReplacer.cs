namespace NSpecifications.Internal;

internal sealed class ParameterReplacer(IReadOnlyDictionary<ParameterExpression, ParameterExpression> map) : ExpressionVisitor
{
    private readonly IReadOnlyDictionary<ParameterExpression, ParameterExpression> _map = map ?? throw new ArgumentNullException(nameof(map));

    public static Expression Replace(Expression expression, IReadOnlyDictionary<ParameterExpression, ParameterExpression> map)
        => new ParameterReplacer(map).Visit(expression);

    protected override Expression VisitParameter(ParameterExpression node)
        => _map.TryGetValue(node, out var replacement)
            ? replacement
            : base.VisitParameter(node);
}
