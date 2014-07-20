using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD
{

    /// <summary>
    /// An entity, as explained in Eric Evans' book.
    /// </summary>
    public interface IEntity<TEntity>
    {
        /// <summary>
        /// Compares this entity with another entity of the same type.
        /// </summary>
        /// <param name="other">The other entity.</param>
        /// <returns>true if the identities are the same, regardles of other attributes.</returns>
        /// <remarks>Entities compare by identity, not by attributes.</remarks>
        bool SameIdentityAs(TEntity other);
        /// <remarks>Entities compare by identity, not by attributes.</remarks>

    }
}
