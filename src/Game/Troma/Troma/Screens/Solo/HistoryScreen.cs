using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameEngine;

namespace Troma
{
    public class HistoryScreen : GameScreen
    {
        private SpriteFont font;

        private Texture2D bg;
        private Rectangle bgRect;

        private StringBuilder msg;
        private string completeMsg;

        private int i;
        private double dt;
        private bool write;

        public HistoryScreen(Game game)
            : base(game)
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            font = GameServices.Game.Content.Load<SpriteFont>("Fonts/history");
            bg = GameServices.Game.Content.Load<Texture2D>("Menus/BgHistory");
            bgRect = new Rectangle(0, 0, 1920, 1080);

            msg = new StringBuilder();
            completeMsg = Resource.History;

            i = 0;
            dt = 0;
            write = true;
        }

        public override void Update(GameTime gameTime, bool hasFocus, bool isVisible)
        {
            base.Update(gameTime, hasFocus, isVisible);

            dt += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (write && i < completeMsg.Length && dt >= 50)
            {
                if (completeMsg[i] != ' ' && i % 3 == 0)
                    SFXManager.Play("Typewriter");
                else if (completeMsg[i] == '\n')
                {
                    SFXManager.Play("TypewriterPullback");
                    TimerManager.Add(1000, EventPullbackEnded);
                    write = false;
                }

                msg.Append(completeMsg[i]);
                i++;
                dt = 0;
            }
            else if (i == completeMsg.Length)
            {
                i++;
                TimerManager.Add(1500, OnCancel);
            }
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            base.HandleInput(gameTime, input);

            // Allows the screen to exit
            if (input.IsPressed(Keys.Enter) || input.IsPressed(Buttons.A) ||
                input.IsPressed(Buttons.Back) || input.IsPressed(Keys.Escape))
                OnCancel(null, null);
        }

        public override void Draw(GameTime gameTime)
        {
            int width = GameServices.GraphicsDevice.Viewport.Width;
            int height = GameServices.GraphicsDevice.Viewport.Height;

            bgRect.Height = height;
            bgRect.Width = (int)(height * 1.778f);
            bgRect.X = -(bgRect.Width - width) / 2;

            float scale = 0.8f * width / 1920;

            // Center the text in the viewport.
            Vector2 viewportSize = new Vector2(
                width,
                height);

            Vector2 textSize = font.MeasureString(completeMsg) * scale;
            Vector2 textPosition = (viewportSize - textSize) / 2;

            GameServices.SpriteBatch.Begin();
            GameServices.SpriteBatch.Draw(bg, bgRect, Color.White * TransitionAlpha);
            GameServices.SpriteBatch.DrawString(font, msg.ToString(), textPosition,
                Color.Black * TransitionAlpha, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
            GameServices.SpriteBatch.End();
        }

        public void EventPullbackEnded(object o, EventArgs e)
        {
            write = true;
        }

        public void OnCancel(object o, EventArgs e)
        {
            ExitScreen();
        }
    }
}
