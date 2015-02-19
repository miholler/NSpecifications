using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDD
{

    public abstract class Specification<TEntity> : ISpecify<TEntity>
    {
        public abstract bool IsSatisfiedBy(TEntity entity);

        public static Specification<TEntity> operator &(Specification<TEntity> spec1, Specification<TEntity> spec2)
        {
            return (Specification<TEntity>)spec1.And(spec2);
        }

        public static Specification<TEntity> operator |(Specification<TEntity> spec1, Specification<TEntity> spec2)
        {
            return (Specification<TEntity>)spec1.Or(spec2);
        }

        public static Specification<TEntity> operator !(Specification<TEntity> spec1)
        {
            return (Specification<TEntity>)spec1.Not();
        }

    }

    internal class AndSpecification<TEntity> : Specification<TEntity>
    {
        protected ISpecify<TEntity> Spec1 { get; private set; }
        protected ISpecify<TEntity> Spec2 { get; private set; }

        internal AndSpecification(ISpecify<TEntity> spec1, ISpecify<TEntity> spec2)
        {
            if (spec1 == null)
                throw new ArgumentNullException("spec1");

            if (spec2 == null)
                throw new ArgumentNullException("spec2");

            Spec1 = spec1;
            Spec2 = spec2;
        }

        public override bool IsSatisfiedBy(TEntity candidate)
        {
            return Spec1.IsSatisfiedBy(candidate) && Spec2.IsSatisfiedBy(candidate);
        }
    }

    internal class OrSpecification<TEntity> : Specification<TEntity>
    {
        protected ISpecify<TEntity> Spec1 { get; private set; }
        protected ISpecify<TEntity> Spec2 { get; private set; }

        internal OrSpecification(ISpecify<TEntity> spec1, ISpecify<TEntity> spec2)
        {
            if (spec1 == null)
                throw new ArgumentNullException("spec1");

            if (spec2 == null)
                throw new ArgumentNullException("spec2");

            Spec1 = spec1;
            Spec2 = spec2;
        }

        public override bool IsSatisfiedBy(TEntity candidate)
        {
            return Spec1.IsSatisfiedBy(candidate) || Spec2.IsSatisfiedBy(candidate);
        }
    }

    internal class NotSpecification<TEntity> : Specification<TEntity>
    {
        protected ISpecify<TEntity> Wrapped { get; private set; }

        internal NotSpecification(ISpecify<TEntity> spec)
        {
            if (spec == null)
            {
                throw new ArgumentNullException("spec");
            }

            Wrapped = spec;
        }

        public override bool IsSatisfiedBy(TEntity candidate)
        {
            return !Wrapped.IsSatisfiedBy(candidate);
        }
    }

    internal class PagedEntitySpecification<TEntity> : Specification<TEntity> 
    {
        private long _index;

        public long PageSize { get; private set; }

        public long Page { get; private set; }

        internal PagedEntitySpecification(long pageSize, long page)
        {
            PageSize = pageSize;
            Page = page;
        }

        public override bool IsSatisfiedBy(TEntity candidate)
        {
            bool result = _index >= PageSize * Page && _index < (PageSize * (Page + 1)) - 1;
            _index++;
            return result;
        }
    }
}