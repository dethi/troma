using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Arrow
{
    public partial class Game : Microsoft.Xna.Framework.Game
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
        public Game()
        {
            this.graphics = new GraphicsDeviceManager(this);
            ActivateFullScreen();
            DisableVsync();

            Content.RootDirectory = "Content";

            screenManager = new ScreenManager(this);
            Components.Add(screenManager);

            screenManager.AddScreen(new GameplayScreen(this));

            #if !BUILD

            screenManager.AddScreen(new DebugScreen(this));

            #endif
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
            //graphics.GraphicsDevice.Clear(Color.DarkOliveGreen);
            graphics.GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);
        }

        #endregion
    }
}
