using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NSpecifications
{
    internal class AndSpecification<TEntity> : Specification<TEntity>
    {
        protected Specification<TEntity> Spec1 { get; private set; }

        protected Specification<TEntity> Spec2 { get; private set; }

        internal AndSpecification(Specification<TEntity> spec1, Specification<TEntity> spec2)
        {
            if (spec1 == null)
                throw new ArgumentNullException("spec1");

            if (spec2 == null)
                throw new ArgumentNullException("spec2");

            Spec1 = spec1;
            Spec2 = spec2;
        }

        public override Expression<Func<TEntity, bool>> Expression
        {
            get { return Spec1.Expression.And(Spec2.Expression); }
        }

        public override string ToString()
        {
            return "({" + Spec1.ToString() + "} And {" + Spec2.ToString() + "})";
        }
    }
}
