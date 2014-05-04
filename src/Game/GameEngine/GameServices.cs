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
        private static GraphicsDeviceManager _graphicsDeviceManager;
        private static SpriteBatch _spriteBatch;

        public static SpriteBatch SpriteBatch
        {
            get { return _spriteBatch; }
        }

        public static GraphicsDeviceManager GraphicsDeviceManager
        {
            get { return _graphicsDeviceManager; }
        }

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

        public static void Initialize(Game game, GraphicsDevice graphicsDevice, 
             GraphicsDeviceManager graphicsDeviceManager)
        {
            _game = game;
            _graphicsDevice = graphicsDevice;
            _graphicsDeviceManager = graphicsDeviceManager;
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

        /// <summary>
        /// Get the screen size of primary screen
        /// </summary>
        private static Vector2 GetScreenSize()
        {
            return new Vector2(
                GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width,
                GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
        }

        /// <summary>
        /// Activate full screen using the better resolution
        /// </summary>
        public static void ActivateFullScreen()
        {
            Vector2 primaryScreen = GetScreenSize();

            _graphicsDeviceManager.PreferredBackBufferWidth = (int)primaryScreen.X;
            _graphicsDeviceManager.PreferredBackBufferHeight = (int)primaryScreen.Y;
            _graphicsDeviceManager.IsFullScreen = true;

            _graphicsDeviceManager.ApplyChanges();
        }

        /// <summary>
        /// Deactivate full screen
        /// </summary>
        public static void DeactivateFullScreen()
        {
            Vector2 primaryScreen = GetScreenSize();

            _graphicsDeviceManager.PreferredBackBufferWidth = 800;
            _graphicsDeviceManager.PreferredBackBufferHeight = 480;
            _graphicsDeviceManager.IsFullScreen = false;

            _graphicsDeviceManager.ApplyChanges();
        }

        /// <summary>
        /// Disable V-Sync, allow more than 60 FPS
        /// </summary>
        public static void DisableVsync()
        {
            _game.IsFixedTimeStep = false;
            _graphicsDeviceManager.SynchronizeWithVerticalRetrace = false;

            _graphicsDeviceManager.ApplyChanges();
        }

        #endregion
    }
}
