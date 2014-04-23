using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.EntitySystem
{
    public interface IEntityDrawable
    {
        /// <summary>
        /// Gets whether the component should be drawn.
        /// </summary>
        bool Visible { get; }

        /// <summary>
        /// Invoked when the Visible property changes.
        /// </summary>
        event EventHandler<EventArgs> VisibleChanged;

        /// <summary>
        /// Draws the component.
        /// </summary>
        void Draw(SpriteBatch spriteBatch);

        void DrawHUD(SpriteBatch spriteBatch);
    }
}
