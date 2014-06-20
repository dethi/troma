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
    class PopUpScreen : MenuScreen
    {
        private Button OkMenuEntry;

        private Texture2D bgTrans;
        private Texture2D arrow;

        private Rectangle bgTransRect;
        private Rectangle arrowRect;

        private string msg;
        private Vector2 msgPos;

        public PopUpScreen(Game game, string message)
            : base(game)
        {
            Vector2 entryPos = new Vector2(143, 435);

            // Create menu entries.
            OkMenuEntry = new Button("Ok", 1, entryPos);

            // Hook up menu event handlers.
            OkMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(OkMenuEntry);

            IsHUD = true;
            msg = message;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            bgTrans = GameServices.Game.Content.Load<Texture2D>("Menus/translucide");
            arrow = GameServices.Game.Content.Load<Texture2D>("Menus/arrow");

            bgTransRect = new Rectangle(0, 0, 0, 0);
            arrowRect = new Rectangle(0, 0, 64, 64);

            int width = GameServices.GraphicsDevice.Viewport.Width;
            int height = GameServices.GraphicsDevice.Viewport.Height;

            float widthScale = (float)width / 1920;
            float heightScale = (float)height / 1080;
            float scale = (widthScale + heightScale) / 2;

            Vector2 textSize = SpriteFont.MeasureString(msg) * scale;
            msgPos = new Vector2(
                980 * widthScale - (textSize.X / 2),
                560 * widthScale - (textSize.Y / 2));

            bgTransRect.X = (int)(940 * widthScale - (textSize.X / 2));
            bgTransRect.Y = (int)(520 * widthScale - (textSize.Y / 2));
            bgTransRect.Width = (int)textSize.X + 80;
            bgTransRect.Height = (int)(textSize.Y + 80 + 100 * scale);

            OkMenuEntry.originPos = new Vector2(
                960,
                (msgPos.Y + textSize.Y) / heightScale + 60);
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 0.45f);

            int width = GameServices.GraphicsDevice.Viewport.Width;
            int height = GameServices.GraphicsDevice.Viewport.Height;

            float widthScale = (float)width / 1920;
            float heightScale = (float)height / 1080;
            float scale = (widthScale + heightScale) / 2;

            GameServices.SpriteBatch.Begin();
            GameServices.SpriteBatch.Draw(bgTrans, bgTransRect, Color.White * TransitionAlpha * 0.3f);
            GameServices.SpriteBatch.DrawString(SpriteFont, msg, msgPos, Color.Black * TransitionAlpha, 0,
                Vector2.Zero, scale, SpriteEffects.None, 0);

            // Draw each menu entry in turn.
            for (int i = 0; i < MenuEntries.Count; i++)
            {
                bool isSelected = IsActive && (i == selectedEntry);
                MenuEntries[i].Draw(gameTime, this, isSelected);

                if (isSelected)
                {
                    arrowRect.Height = (int)(64 * ((widthScale + heightScale) / 2));
                    arrowRect.Width = arrowRect.Height;
                    arrowRect.X = (int)(MenuEntries[i].Position.X - 1.5f * arrowRect.Width);
                    arrowRect.Y = (int)MenuEntries[i].Position.Y;
                    GameServices.SpriteBatch.Draw(arrow, arrowRect, Color.White * TransitionAlpha);
                }
            }

            GameServices.SpriteBatch.End();
        }

        protected override void OnCancel(object sender, EventArgs e)
        {
            ExitScreen();
        }
    }
}
