using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Troma
{
    public partial class Troma
    {
        /// <summary>
        /// Get the screen size of primary screen
        /// </summary>
        private Vector2 GetScreenSize()
        {
            return new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width,
                GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
        }

        /// <summary>
        /// Activate full screen using the better resolution
        /// </summary>
        private void ActivateFullScreen()
        {
            Vector2 primaryScreen = GetScreenSize();

            graphics.PreferredBackBufferWidth = (int)primaryScreen.X;
            graphics.PreferredBackBufferHeight = (int)primaryScreen.Y;
            graphics.IsFullScreen = true;

            graphics.ApplyChanges();
        }

        /// <summary>
        /// Disable V-Sync, allow more than 60 FPS
        /// </summary>
        private void DisableVsync()
        {
            IsFixedTimeStep = false;
            graphics.SynchronizeWithVerticalRetrace = false;

            graphics.ApplyChanges();
        }
    }
}
