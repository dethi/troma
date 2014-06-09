using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine;

namespace Troma
{
    class ScoreMenu : MenuScreen
    {
        private Entry backMenuEntry;

        private Texture2D bg;
        private Texture2D arrow;

        private Rectangle bgRect;
        private Rectangle arrowRect;

        public ScoreMenu(Game game)
            : base(game)
        {
            Vector2 entryPos = new Vector2(143, 875);

            // Create menu entries.
            backMenuEntry = new Entry(Resource.Back, 1, entryPos);

            // Hook up menu event handlers.
            backMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(backMenuEntry);

            SceneRenderer.InitializeMenu();
        }

        public override void LoadContent()
        {
            base.LoadContent();

            bg = FileManager.Load<Texture2D>("Menus/background-blur");
            arrow = FileManager.Load<Texture2D>("Menus/arrow");

            bgRect = new Rectangle(0, 0, 1920, 1080);
            arrowRect = new Rectangle(0, 0, 64, 64);
        }

        public override void Draw(GameTime gameTime)
        {
            int width = GameServices.GraphicsDevice.Viewport.Width;
            int height = GameServices.GraphicsDevice.Viewport.Height;

            float widthScale = (float)width / 1920;
            float heightScale = (float)height / 1080;

            bgRect.Height = height;
            bgRect.Width = (int)(height * 1.778f);
            bgRect.X = -(bgRect.Width - width) / 2;

            GameServices.SpriteBatch.Begin();

            GameServices.SpriteBatch.Draw(bg, bgRect, Color.White * TransitionAlpha);

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

        private void OnCancel(object sender, EventArgs e)
        {
            OnCancel();
        }
    }
}
