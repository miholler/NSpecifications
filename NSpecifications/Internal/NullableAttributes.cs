namespace System.Diagnostics.CodeAnalysis;

/// <summary>
/// Specifies that when a method returns <see cref="ReturnValue"/>, the parameter will not be null 
/// even if the corresponding type allows it.
/// </summary>
/// <param name="returnValue">
/// The return value condition. If the method returns this value, the associated parameter will not be null.
/// </param>
[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
internal sealed class NotNullWhenAttribute(bool returnValue) : Attribute
{
    /// <summary>
    /// Gets the return value condition.
    /// </summary>
    public bool ReturnValue { get; } = returnValue;
}

/// <summary>
/// Specifies that the output will be non-null if the named parameter is non-null.
/// </summary>
/// <param name="parameterName">
/// The associated parameter name. The output will be non-null if the argument to the parameter specified is non-null.
/// </param>
[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue, AllowMultiple = true, Inherited = false)]
internal sealed class NotNullIfNotNullAttribute(string parameterName) : Attribute
{
    /// <summary>
    /// Gets the associated parameter name.
    /// </summary>
    public string ParameterName { get; } = parameterName;
}
