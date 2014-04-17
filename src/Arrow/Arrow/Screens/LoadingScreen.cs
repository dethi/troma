using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Arrow
{
    class LoadingScreen : GameScreen
    {
        bool loadingIsSlow;
        bool otherScreensAreGone;
        GameScreen[] screensToLoad;
        Game game;
        private ContentManager content;
        private SpriteFont spriteFont;

        private LoadingScreen(ScreenManager screenManager, bool loadingIsSlow,
                              GameScreen[] screensToLoad, Game game)
            :base(game)
        {
            this.loadingIsSlow = loadingIsSlow;
            this.screensToLoad = screensToLoad;
            this.game = game;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
        }

        public static void Load(Game game,ScreenManager screenManager, bool loadingIsSlow,
                                params GameScreen[] screensToLoad)
        {
            // Tell all the current screens to transition off.
            foreach (GameScreen screen in screenManager.GetScreens())
                screen.ExitScreen();

            // Create and activate the loading screen.
            LoadingScreen loadingScreen = new LoadingScreen(screenManager,
                                                            loadingIsSlow,
                                                            screensToLoad, 
                                                            game);

            screenManager.AddScreen(loadingScreen);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            // If all the previous screens have finished transitioning
            // off, it is time to actually perform the load.
            if (otherScreensAreGone)
            {
                ScreenManager.RemoveScreen(this);

                foreach (GameScreen screen in screensToLoad)
                {
                    if (screen != null)
                    {
                        ScreenManager.AddScreen(screen);
                    }
                }

                // Once the load has finished, we use ResetElapsedTime to tell
                // the  game timing mechanism that we have just finished a very
                // long frame, and that it should not try to catch up.
                ScreenManager.Game.ResetElapsedTime();
            }
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            spriteFont = content.Load<SpriteFont>("Fonts/Texture");
        }
        
        public override void Draw(GameTime gameTime)
        {
            // If we are the only active screen, that means all the previous screens
            // must have finished transitioning off. We check for this in the Draw
            // method, rather than in Update, because it isn't enough just for the
            // screens to be gone: in order for the transition to look good we must
            // have actually drawn a frame without them before we perform the load.
            if ((ScreenState == ScreenState.Active) &&
                (ScreenManager.GetScreens().Length == 1))
            {
                otherScreensAreGone = true;
            }

            // The gameplay screen takes a while to load, so we display a loading
            // message while that is going on, but the menus load very quickly, and
            // it would look silly if we flashed this up for just a fraction of a
            // second while returning from the game to the menus. This parameter
            // tells us how long the loading is going to take, so we know whether
            // to bother drawing the message.
            if (loadingIsSlow)
            {
                SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

                const string message = "Chargement";

                // Center the text in the viewport.
                float scale = 1.1f * game.GraphicsDevice.Viewport.Width / 1920;
                Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
                Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
                Vector2 textSize = spriteFont.MeasureString(message) * scale;
                Vector2 textPosition = (viewportSize - textSize) / 2;
                
                Vector2 origin = new Vector2(0, 0);

                Color color = Color.White * TransitionAlpha;

                // Draw the text.
                spriteBatch.Begin();
                spriteBatch.DrawString(spriteFont, message, textPosition, color, 0, origin, scale, SpriteEffects.None, 0);
                spriteBatch.End();
            }
        }
    }
}
