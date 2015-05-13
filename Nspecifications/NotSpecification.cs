using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NSpecifications
{
    internal class NotSpecification<TEntity> : Specification<TEntity>
    {
        private readonly Specification<TEntity> _wrapped;

        protected Specification<TEntity> Wrapped
        {
            get { return _wrapped; }
        }

        internal NotSpecification(Specification<TEntity> spec)
        {
            if (spec == null)
            {
                throw new ArgumentNullException("spec");
            }

            _wrapped = spec;
        }

        public new bool IsSatisfiedBy(TEntity candidate)
        {
            return !Wrapped.IsSatisfiedBy(candidate);
        }

        public override Expression<Func<TEntity, bool>> Expression
        {
            get { return Wrapped.Expression.Not(); }
        }
    }

}
