namespace NSpecifications.Internal;

/// <summary>
/// Represents a rebinder of parameters in expression trees.
/// </summary>
public sealed class ParameterRebinder : ExpressionVisitor
{
    private readonly IReadOnlyDictionary<ParameterExpression, ParameterExpression> _map;

    /// <summary>
    /// Initializes a new instance of the <see cref="ParameterRebinder"/> class.
    /// </summary>
    /// <param name="map">The map that describes which parameter is replaced by which.</param>
    public ParameterRebinder(IReadOnlyDictionary<ParameterExpression, ParameterExpression> map)
    {
        _map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
    }

    /// <summary>
    /// Rebinds all parameters of a specified expression using a specified parameter map.
    /// </summary>
    /// <param name="expression">The expression of which all parameters are to be rebound.</param>
    /// <param name="map">The map that describes which parameter is replaced by which.</param>
    /// <returns>An <see cref="Expression"/> whose parameters have been rebound according to the specified parameter map.</returns>
    public static Expression Rebind(Expression expression, IReadOnlyDictionary<ParameterExpression, ParameterExpression> map)
        => new ParameterRebinder(map).Visit(expression);

    /// <inheritdoc/>
    protected override Expression VisitParameter(ParameterExpression node)
    {
        if (_map.TryGetValue(node, out var replacement))
            node = replacement;

        return base.VisitParameter(node);
    }
}
