using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using GameEngine;
using ClientServerExtension;

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
            "blank",
            "Menus/arrow",
            "Menus/background",
            "Menus/background-blur",
            "Menus/translucide",
            "Menus/BgHistory",
            "Menus/plus",
            "Menus/minus",
            "Menus/go-left",
            "Menus/go-right",
            "Fonts/28DaysLater",
            "Fonts/LuckyTypewriter",
            "Fonts/History",
            "Fonts/Digital"
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

            //Process p = Process.Start("GameServer");
            //p.Kill();
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

            //screenManager.AddScreen(new SoloScreen(this, Map.Cracovie));
            screenManager.AddScreen(new PegiScreen(this));
            //screenManager.AddScreen(new ScoreMenu(this));
            //screenManager.AddScreen(new HistoryScreen(this));
            //screenManager.AddScreen(new MultiplayerScreen(this, "192.168.1.7"));
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
            SFXManager.Add("M1911_empty", Content.Load<SoundEffect>("Sounds/M1911_empty"));
            SFXManager.Add("M1911_reload", Content.Load<SoundEffect>("Sounds/M1911_reload"));
            SFXManager.Add("M1911_shoot", Content.Load<SoundEffect>("Sounds/M1911_shoot"));
            SFXManager.Add("Move", Content.Load<SoundEffect>("Sounds/Move"));
            SFXManager.Add("TargetImpact", Content.Load<SoundEffect>("Sounds/TargetImpact"));
            SFXManager.Add("Typewriter", Content.Load<SoundEffect>("Sounds/Typewriter"));
            SFXManager.Add("TypewriterPullback", Content.Load<SoundEffect>("Sounds/TypewriterPullback"));

            // Generate a box file
            /*
            Model generateBox = Content.Load<Model>("Models/cible");
            Box box = new Box();
            box.Generate(generateBox);
            box.Save("PlayerBox");
            */
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
