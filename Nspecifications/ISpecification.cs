namespace NSpecifications
{
    public interface ISpecification
    {
    }

    public interface ISpecification<in TEntity> : ISpecification
    {
        bool IsSatisfiedBy(TEntity entity);
    }
}
