using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine
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
        void Draw(GameTime gameTime, ICamera camera);

        /// <summary>
        /// Draw HUD
        /// </summary>
        void DrawHUD(GameTime gameTime);
    }
}
