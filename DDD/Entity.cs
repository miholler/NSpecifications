using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD
{
    public abstract class Entity<TIdentity> 
    {
        public abstract TIdentity Identity { get; }

        /// <summary>
        /// Two entities are equal if they reference the same memory address or
        /// if the other object is in the inheritance hierarchy of this instance 
        /// and have an identity with the same value.
        /// </summary>
        /// <param name="other">The other <see cref="System.Object" /> to compare with this instance.</param>
        public override bool Equals(object other)
        {
            var otherEntity = other as Entity<TIdentity>;
            return otherEntity != null
                && (!otherEntity.GetType().IsAssignableFrom(this.GetType())
                    || !this.GetType().IsAssignableFrom(otherEntity.GetType()))
                && this.Identity.Equals(otherEntity.Identity);
        }

        /// <summary>
        /// Two entities are equal if they are of the same type and have an identity with the same value.
        /// </summary>
        public static bool operator ==(Entity<TIdentity> entityOne, Entity<TIdentity> entityTwo)
        {
            if ((object)entityOne == null && (object)entityTwo != null)
                return false;
            return entityOne.Equals(entityTwo);
        }

        /// <summary>
        /// Two entities are different if only one of them is null, if they are of
        /// different types or if they have identities with different values.
        /// </summary>
        public static bool operator !=(Entity<TIdentity> entityOne, Entity<TIdentity> entityTwo)
        {
            return !(entityOne == entityTwo);
        }

        public override int GetHashCode()
        {
            return Identity.GetHashCode();
        }

    }
}
