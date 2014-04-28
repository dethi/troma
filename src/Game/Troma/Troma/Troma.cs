using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GameEngine;

namespace Troma
{
    public partial class Troma : Microsoft.Xna.Framework.Game
    {
        #region Fields

        internal GraphicsDeviceManager graphics { get; private set; }
        ScreenManager screenManager;

        // Prelaod any assets using by UI rendering
        static readonly string[] preloadAssets =
        {
        };

        #endregion

        #region Initialization

        /// <summary>
        /// The main game constructor
        /// </summary>
        public Troma()
        {
            graphics = new GraphicsDeviceManager(this);
            //ActivateFullScreen();
            //DisableVsync();

            Content.RootDirectory = "Content";

            screenManager = new ScreenManager(this);
            Components.Add(screenManager);

            screenManager.AddScreen(new TestScreen(this));
            //screenManager.AddScreen(new MainMenuScreen(this));

#if DEBUG
            screenManager.AddScreen(new DebugScreen(this));
#endif
        }

        protected override void Initialize()
        {
            base.Initialize();
            screenManager.Initialize();
        }

        /// <summary>
        /// Load graphics content
        /// </summary>
        protected override void LoadContent()
        {
            foreach (string asset in preloadAssets)
                Content.Load<object>(asset);
        }

        #endregion

        #region Draw

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.SkyBlue);
            base.Draw(gameTime);
        }

        #endregion
    }
}
