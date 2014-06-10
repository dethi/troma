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
        /// Enable/Disable full screen
        /// </summary>
        public static void FullScreen(bool enable)
        {
            Vector2 primaryScreen = GetScreenSize();
            int div = (enable) ? 1 : 2;

            _graphicsDeviceManager.PreferredBackBufferWidth = (int)(primaryScreen.X / div);
            _graphicsDeviceManager.PreferredBackBufferHeight = (int)(primaryScreen.Y / div);
            _graphicsDeviceManager.IsFullScreen = enable;

#if DEBUG
            if (!enable) // Force 4/3 ratio
            {
                 _graphicsDeviceManager.PreferredBackBufferWidth = 1024;
                _graphicsDeviceManager.PreferredBackBufferHeight = 728;
            }
#endif

            _graphicsDeviceManager.ApplyChanges();

            InputState.MouseOrigin = new Vector2(
                _graphicsDevice.Viewport.Width / 2,
                _graphicsDevice.Viewport.Height / 2);
        }

        /// <summary>
        /// Enable/Disable V-Sync
        /// </summary>
        public static void Vsync(bool enable)
        {
            _game.IsFixedTimeStep = enable;
            _graphicsDeviceManager.SynchronizeWithVerticalRetrace = enable;

            _graphicsDeviceManager.ApplyChanges();
        }

        /// <summary>
        /// Enable/Disable Multisampling
        /// </summary>
        public static void Multisampling(bool enable)
        {
            _graphicsDeviceManager.PreferMultiSampling = enable;
            _graphicsDeviceManager.ApplyChanges();
        }

        #endregion
    }
}
