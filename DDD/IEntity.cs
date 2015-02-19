using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD
{
    /// <summary>
    /// An entity is defined by it's identity, not by reference nor by it's other attributes.
    /// </summary>
    /// <typeparam name="TIdentity">The type of the identity.</typeparam>
    public interface IEntity<TIdentity> where TIdentity : IComparable
    {
        /// <summary>
        /// Gets the identity.
        /// </summary>
        /// <value>
        /// The identity.
        /// </value>
        TIdentity Identity { get; }
    }
}
