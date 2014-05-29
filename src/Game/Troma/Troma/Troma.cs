using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
            Content.RootDirectory = "Content";

            screenManager = new ScreenManager(this);
            Components.Add(screenManager);
        }

        protected override void Initialize()
        {
            base.Initialize();

            GameServices.Initialize(this, GraphicsDevice, graphics);
            TimerManager.Initialize();
            SoundManager.Initialize();
            Settings.Initialize();
            SoundManager.SetVolume(Settings.MusicVolume);
            SFXManager.SetVolume(Settings.MusicVolume);

            //screenManager.AddScreen(new SoloScreen(this, ""));
            //screenManager.AddScreen(new TestScreen(this));
            //screenManager.AddScreen(new MainMenuScreen(this));
            //screenManager.AddScreen(new ScoreScreen(this, null));
            screenManager.AddScreen(new PegiScreen(this));
        }

        /// <summary>
        /// Load graphics content
        /// </summary>
        protected override void LoadContent()
        {
            foreach (string asset in preloadAssets)
                Content.Load<object>(asset);

            SFXManager.Add("Button_entry", Content.Load<SoundEffect>("Sounds/Button_entry"));
            SFXManager.Add("Button_selected", Content.Load<SoundEffect>("Sounds/Button_selected"));
            SFXManager.Add("GarandM1_empty", Content.Load<SoundEffect>("Sounds/GarandM1_empty"));
            SFXManager.Add("GarandM1_reload", Content.Load<SoundEffect>("Sounds/GarandM1_reload"));
            SFXManager.Add("GarandM1_shoot", Content.Load<SoundEffect>("Sounds/GarandM1_shoot"));
            SFXManager.Add("Move", Content.Load<SoundEffect>("Sounds/Move"));
        }

        #endregion

        #region Update and Draw

        protected override void Update(GameTime gameTime)
        {
            TimerManager.Update(gameTime);
            SoundManager.Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(SceneRenderer.BackgroundColor);
            base.Draw(gameTime);
        }

        #endregion
    }
}
