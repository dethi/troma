using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine
{
    public static class GameServices
    {
        #region Fields

        private static Game _game;
        private static GraphicsDevice _graphicsDevice;
        private static SpriteBatch _spriteBatch;

        /// <summary>
        /// The SpriteBatch used in drawing operations
        /// </summary>
        public static SpriteBatch SpriteBatch
        {
            get { return _spriteBatch; }
        }

        /// <summary>
        /// The Graphics device used  in the game
        /// </summary>
        public static GraphicsDevice GraphicsDevice
        {
            get { return _graphicsDevice; }
        }

        public static Game Game
        {
            get { return _game; }
        }

        #endregion

        #region Initialization

        public static void Initialize(Game game, GraphicsDevice graphicsDevice)
        {
            _game = game;
            _graphicsDevice = graphicsDevice;
            _spriteBatch = new SpriteBatch(_graphicsDevice);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Reset RasterizerState to the default value
        /// </summary>
        public static void DefaultChangeRasterizerState()
        {
            ChangeRasterizerState(CullMode.CullClockwiseFace, FillMode.Solid);
        }

        /// <summary>
        /// Allow to change the RasterizerState
        /// </summary>
        public static void ChangeRasterizerState(CullMode cm, FillMode fl)
        {
            _graphicsDevice.RasterizerState = new RasterizerState()
            {
                CullMode = cm,
                FillMode = fl
            };
        }

        /// <summary>
        /// Change the value of GraphicsDevice in order to display correctly 3D
        /// </summary>
        public static void ResetGraphicsDeviceFor3D()
        {
            _graphicsDevice.BlendState = BlendState.Opaque;
            _graphicsDevice.DepthStencilState = DepthStencilState.Default;
        }

        #endregion
    }
}
