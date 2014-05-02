using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameEngine
{
    public class ScreenManager : DrawableGameComponent
    {
        #region Fields

        private List<GameScreen> screens;
        private List<GameScreen> screensToUpdate;
        private InputState input;
        private ContentManager content;

        private bool isInitialized;

        #endregion

        #region Initialization

        public ScreenManager(Game game)
            : base(game)
        {
            screens = new List<GameScreen>();
            screensToUpdate = new List<GameScreen>();
            input = new InputState();

            isInitialized = false;
        }

        public override void Initialize()
        {
            base.Initialize();

            input.MouseOrigin = new Vector2(
                GraphicsDevice.Viewport.Width / 2,
                GraphicsDevice.Viewport.Height / 2);

            isInitialized = true;
        }

        protected override void LoadContent()
        {
            content = Game.Content;

            GameServices.Initialize(Game, GraphicsDevice);

            foreach (GameScreen s in screens)
                s.LoadContent();

            FileManager.Content = content;
        }

        protected override void UnloadContent()
        {
            foreach (GameScreen s in screens)
                s.UnloadContent();
        }

        #endregion

        public override void Draw(GameTime gameTime)
        {
            foreach (GameScreen s in screens)
            {
                if (s.ScreenState != ScreenState.Hidden)
                    s.Draw(gameTime);
            }
        }

        public override void Update(GameTime gameTime)
        {
            input.Update();

            // Allows the game to exit
            if (input.IsDown(Buttons.Back) || input.IsDown(Keys.Escape))
                Game.Exit();

            screensToUpdate.Clear();

            foreach (GameScreen s in screens)
                screensToUpdate.Add(s);

            bool hasFocus = Game.IsActive;
            bool isVisible = true;

            while (screensToUpdate.Count > 0)
            {
                GameScreen s = screensToUpdate[screensToUpdate.Count - 1];
                screensToUpdate.RemoveAt(screensToUpdate.Count - 1);

                s.Update(gameTime, hasFocus, isVisible);

                if (s.ScreenState == ScreenState.TransitionOn ||
                    s.ScreenState == ScreenState.Active)
                {
                    if (hasFocus && !s.IsHUD)
                    {
                        s.HandleInput(gameTime, input);
                        hasFocus = false;
                    }

                    if (!s.IsHUD)
                        isVisible = false;
                }
            }
        }

        /// <summary>
        /// Add a screen
        /// </summary>
        public void AddScreen(GameScreen s)
        {
            s.ScreenManager = this;
            s.IsExiting = false;

            if (isInitialized)
                s.LoadContent();

            screens.Add(s);
            FileManager.Content = content;
        }

        /// <summary>
        /// Remove a screen
        /// </summary>
        public void RemoveScreen(GameScreen s)
        {
            if (isInitialized)
                s.UnloadContent();

            screens.Remove(s);
            screensToUpdate.Remove(s);
        }

        /// <summary>
        /// Return an array that contains all screen
        /// </summary>
        public GameScreen[] GetScreens()
        {
            return screens.ToArray();
        }
    }
}
