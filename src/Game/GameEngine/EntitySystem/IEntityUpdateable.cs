using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameEngine
{
    public interface IEntityUpdateable
    {
        /// <summary>
        /// Gets whether the component should be updated.
        /// </summary>
        bool Enabled { get; }

        /// <summary>
        /// Invoked when the Enabled property changes.
        /// </summary>
        event EventHandler<EventArgs> EnabledChanged;

        /// <summary>
        /// Updates the component.
        /// </summary>
        void Update(GameTime gameTime);
    }
}
