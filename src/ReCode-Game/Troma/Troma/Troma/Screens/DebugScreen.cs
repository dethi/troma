using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GameEngine.Screen;

namespace Troma.Screens
{
    public class DebugScreen : GameScreen
    {
        #region Fields

        ContentManager content;
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;

        float memory;
        double fps;

        #endregion

        #region Initialization

        public DebugScreen(Game game)
            : base(game)
        {
            IsHUD = true;
            ScreenState = ScreenState.Active;
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            spriteBatch = ScreenManager.SpriteBatch;
            spriteFont = content.Load<SpriteFont>("Fonts/Debug");
        }

        public override void UnloadContent()
        {
            content.Unload();
        }

        #endregion

        /// <summary>
        /// Update used memory and current FPS
        /// </summary>
        public override void Update(GameTime gameTime, bool hasFocus, bool isVisible)
        {
            memory = GC.GetTotalMemory(false) / 1048576f;
            fps = 1000.0d / gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        public override void Draw(GameTime gameTime)
        {
            string debug = String.Format(
                "FPS: {0:F1}\n" +
                "Mem: {1:F2} Mo",
                fps, memory);

            Vector2 size = spriteFont.MeasureString(debug);
            Vector2 pos = new Vector2(5,
                ScreenManager.GraphicsDevice.Viewport.Height - size.Y - 5);

            spriteBatch.Begin();
            spriteBatch.DrawString(spriteFont, debug, pos, Color.Gold);
            spriteBatch.End();
        }
    }
}
