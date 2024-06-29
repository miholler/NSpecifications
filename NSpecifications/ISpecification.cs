namespace NSpecifications;

/// <summary>
/// Defines methods to determine whether an
/// object meets a defined set of criteria.
/// </summary>
public interface ISpecification
{
    /// <summary>
    /// Determines whether the specification is satisfied by a specified value.
    /// </summary>
    /// <param name="candidate">The value to check.</param>
    /// <returns>
    /// <see langword="true"/> if the specification is satisfied by the specified value;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    bool IsSatisfiedBy(object? candidate);
}

/// <summary>
/// Defines methods to determine whether an
/// object meets a defined set of criteria.
/// </summary>
/// <typeparam name="T">The type of the candidate object.</typeparam>
public interface ISpecification<T>
{
    /// <summary>
    /// Determines whether the specification is satisfied by a specified value.
    /// </summary>
    /// <param name="candidate">The value to check.</param>
    /// <returns>
    /// <see langword="true"/> if the specification is satisfied by the specified value;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    bool IsSatisfiedBy(T candidate);
}

/// <summary>
/// Represents a specification that has a conditional AND operator.
/// </summary>
/// <typeparam name="T">The type of the candidate object.</typeparam>
public interface IAndSpecification<T> : ISpecification<T>
{
    /// <summary>
    /// Gets the left operand of the conditional AND operation.
    /// </summary>
    ISpecification<T> Left { get; }

    /// <summary>
    /// Gets the right operand of the conditional AND operation.
    /// </summary>
    ISpecification<T> Right { get; }
}

/// <summary>
/// Represents a specification that has a conditional OR operator.
/// </summary>
/// <typeparam name="T">The type of the candidate object.</typeparam>
public interface IOrSpecification<T> : ISpecification<T>
{
    /// <summary>
    /// Gets the left operand of the conditional OR operation.
    /// </summary>
    ISpecification<T> Left { get; }

    /// <summary>
    /// Gets the right operand of the conditional OR operation.
    /// </summary>
    ISpecification<T> Right { get; }
}

/// <summary>
/// Represents a specification that has a logical negation operator.
/// </summary>
/// <typeparam name="T">The type of the candidate object.</typeparam>
public interface INotSpecification<T> : ISpecification<T>
{
    /// <summary>
    ///  Gets the operand of the negation operation.
    /// </summary>
    ISpecification<T> Operand { get; }
}
