using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine;

namespace Troma
{
    public class LoadingScreen : GameScreen
    {
        bool loadingIsSlow;
        bool otherScreensAreGone;
        GameScreen[] screensToLoad;
        SpriteFont spriteFont;

        private Texture2D bg;
        private Rectangle bgRect;

        private LoadingScreen(Game game, ScreenManager screenManager, bool loadingIsSlow,
            GameScreen[] screensToLoad)
            : base(game)
        {
            this.loadingIsSlow = loadingIsSlow;
            this.screensToLoad = screensToLoad;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
        }

        public static void Load(Game game, ScreenManager screenManager, bool loadingIsSlow,
            params GameScreen[] screensToLoad)
        {
            // Tell all the current screens to transition off.
            foreach (GameScreen screen in screenManager.GetScreens())
                screen.ExitScreen();

            // Create and activate the loading screen.
            LoadingScreen loadingScreen = new LoadingScreen(game,
                screenManager, loadingIsSlow, screensToLoad);

            screenManager.AddScreen(loadingScreen);
        }

        public override void LoadContent()
        {
            base.LoadContent();
            spriteFont = FileManager.Load<SpriteFont>("Fonts/Loading");
            bg = GameServices.Game.Content.Load<Texture2D>("blank");

            bgRect = new Rectangle(0, 0,
                GameServices.GraphicsDevice.Viewport.Width,
                GameServices.GraphicsDevice.Viewport.Height);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
            bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if ((ScreenState == ScreenState.Active) &&
                (ScreenManager.GetScreens().Length == 1))
            {
                otherScreensAreGone = true;
            }

            if (otherScreensAreGone)
            {
                ScreenManager.RemoveScreen(this);

                foreach (GameScreen screen in screensToLoad)
                {
                    if (screen != null)
                        ScreenManager.AddScreen(screen);
                }

                ScreenManager.Game.ResetElapsedTime();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (loadingIsSlow)
            {
                string message = Resource.Loading;
                float scale = 1.1f * GameServices.GraphicsDevice.Viewport.Width / 1920;
                Color color = Color.White * TransitionAlpha;

                // Center the text in the viewport.
                Vector2 viewportSize = new Vector2(
                    GameServices.GraphicsDevice.Viewport.Width,
                    GameServices.GraphicsDevice.Viewport.Height);
                Vector2 textSize = spriteFont.MeasureString(message) * scale;
                Vector2 textPosition = (viewportSize - textSize) / 2;

                GameServices.SpriteBatch.Begin();
                GameServices.SpriteBatch.Draw(bg, bgRect, Color.Black);
                GameServices.SpriteBatch.DrawString(spriteFont, message, textPosition,
                    color, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
                GameServices.SpriteBatch.End();
            }
        }
    }
}
