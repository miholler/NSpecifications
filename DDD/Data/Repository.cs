using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Data
{
    public abstract class Repository<TEntity> : IRepository<TEntity>
    {
    }

    public abstract class Repository<TEntity, TIdentity> : Repository<TEntity>
        where TEntity : Entity<TIdentity>
    {
    }

    public abstract class GenericRepository<TEntity, TIdentity> : Repository<TEntity, TIdentity>
        where TEntity : Entity<TIdentity>
    {
        public abstract void Add(TEntity entity);

        public abstract void Remove(TEntity entity);

        public abstract TEntity Get(TIdentity id);

        public abstract TEntity[] GetAll();

        protected abstract IEnumerable<TEntity> Search(ISpecification<TEntity> specification, object options, Func<object, TEntity> constructor);

        public abstract TEntity[] Count();

        public abstract TEntity[] Count(ISpecification<TEntity> specification);

    }
}
