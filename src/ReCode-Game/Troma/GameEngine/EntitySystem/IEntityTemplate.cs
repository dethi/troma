using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngine
{
    /// <summary>Interface IEntityTemplate.</summary>
    public interface IEntityTemplate
    {
        /// <summary>Builds the entity.</summary>
        /// <param name="entity">The entity.</param>
        /// <param name="args">The args.</param>
        /// <returns>The build entity.</returns>
        Entity BuildEntity(Entity entity, params object[] args);
    }
}
