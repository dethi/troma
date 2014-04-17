using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arrow
{
    public partial class Game
    {
        /// <summary>
        /// Get the screen size of primary screen
        /// </summary>
        public Vector2 GetScreenSize()
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

        /// <summary>
        /// Reset RasterizerState to the default value
        /// </summary>
        public void DefaultChangeRasterizerState()
        {
            ChangeRasterizerState(CullMode.CullClockwiseFace, FillMode.Solid);
        }

        /// <summary>
        /// Allow to change the RasterizerState
        /// </summary>
        public void ChangeRasterizerState(CullMode cm, FillMode fl)
        {
            GraphicsDevice.RasterizerState = new RasterizerState()
            {
                CullMode = cm,
                FillMode = fl
            };
        }

        /// <summary>
        /// Change the value of GraphicsDevice in order to display correctly 3D
        /// </summary>
        public void ResetGraphicsDeviceFor3D()
        {
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
        }
    }
}
