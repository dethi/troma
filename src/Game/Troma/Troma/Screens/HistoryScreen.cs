using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine;

namespace Troma
{
    public class HistoryScreen : GameScreen
    {
        SpriteFont tromaFont;
        StringBuilder display;

        string message;
        private Rectangle bgRect;
        int i;
        double time2;

        public HistoryScreen(Game game)
            : base(game)
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
        }

        public static void Load(Game game)
        {
        }

        public override void LoadContent()
        {
            base.LoadContent();
            tromaFont = GameServices.Game.Content.Load<SpriteFont>("Fonts/history");

            bgRect = new Rectangle(0, 0,
                GameServices.GraphicsDevice.Viewport.Width,
                GameServices.GraphicsDevice.Viewport.Height);

            display = new StringBuilder();
            message = Resource.History;
            i = 0;
            time2 = 0;
        }

        public override void Update(GameTime gameTime, bool hasFocus, bool isVisible)
        {
            base.Update(gameTime, hasFocus, isVisible);
            double time = gameTime.TotalGameTime.TotalMilliseconds;
            if (i < message.Length & (time - time2) >= 50)
            {
                display.Append(message[i]);
                i++;
                time2 = time;
                SFXManager.Play("Typewriter");
            }
     
                
        }

        public override void Draw(GameTime gameTime)
        {
            float scale = 0.5f * GameServices.GraphicsDevice.Viewport.Width / 1920;
            Color color = Color.White * TransitionAlpha;

            // Center the text in the viewport.
            Vector2 viewportSize = new Vector2(
                GameServices.GraphicsDevice.Viewport.Width,
                GameServices.GraphicsDevice.Viewport.Height);
            Vector2 textSize = tromaFont.MeasureString(message) * scale;
            Vector2 textPosition = (viewportSize - textSize) / 2;

            GameServices.SpriteBatch.Begin();
            GameServices.SpriteBatch.DrawString(tromaFont, display.ToString(), textPosition,
                color, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
            GameServices.SpriteBatch.End();
        }
    }
}
