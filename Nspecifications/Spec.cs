namespace NSpecifications;

public class Spec<T> : ASpec<T>
{
    public static readonly Spec<T> Any = new(_ => true);
    public static readonly Spec<T> None = new(_ => false);

    public Spec(Expression<Func<T, bool>> predicate)
    {
        Predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
    }

    public override Expression<Func<T, bool>> Predicate { get; }
}

public static class Spec
{
    public static ASpec<T> Any<T>()
        => Spec<T>.Any;

    public static ASpec<T> None<T>()
        => Spec<T>.None;
}
