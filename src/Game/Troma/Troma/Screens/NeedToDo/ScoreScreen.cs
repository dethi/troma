using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine;

namespace Troma
{
    public class ScoreScreen : MenuScreenOld
    {
        private string _score;

        public ScoreScreen(Game game, TimeSpan time)
            : base(game, Resource.Victory)
        {
            MenuEntry backMenuEntry = new MenuEntry(Resource.Back, 0.75f, 0, false);
            backMenuEntry.Selected += OnCancel;
            MenuEntries.Add(backMenuEntry);

            IsHUD = true;

            _score = String.Format("{0} min {1} sec\n",
                time.Minutes,
                time.Seconds);
        }

        public override void Draw(GameTime gameTime)
        {
            int width = game.GraphicsDevice.Viewport.Width;
            int height = game.GraphicsDevice.Viewport.Height;
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            Vector2 titlePosition = new Vector2(
                0.5f * width,
                0.0625f * width - 100 * transitionOffset);
            Vector2 titleOrigin = Font.MeasureString(menuTitle) / 2;
            Color titleColor = new Color(0, 0, 0) * TransitionAlpha;
            float titleScale = 0.00078125f * width;

            float scoreScale = 0.00078125f * width;
            Vector2 scorePosition = new Vector2(
                (width - Font.MeasureString(_score).X * scoreScale) / 2,
                (300 * height) / 1080);

            GameServices.SpriteBatch.Begin();

            GameServices.SpriteBatch.DrawString(Font, menuTitle, titlePosition, Color.White, 0,
                titleOrigin, titleScale, SpriteEffects.None, 0);
            GameServices.SpriteBatch.DrawString(Font, _score, scorePosition, Color.White, 0,
                Vector2.Zero, scoreScale, SpriteEffects.None, 0);
            MenuEntries[0].Draw(gameTime, this, true);

            GameServices.SpriteBatch.End();
        }

        protected override void UpdateMenuEntryLocations()
        {
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);
            int width = game.GraphicsDevice.Viewport.Width;
            int height = game.GraphicsDevice.Viewport.Height;

            int textWidth = MenuEntries[0].GetWidth(this);

            float scale = ((MenuEntries[0].FontSize + 0.5f) * width) / 1980;
            MenuEntries[0].Position = new Vector2(
                (width - textWidth * scale) / 2,
                (800 * height) / 1080);

            if (ScreenState == ScreenState.TransitionOn)
                MenuEntries[0].Position.X -= transitionOffset * 256;
            else
                MenuEntries[0].Position.X += transitionOffset * 512;
        }

        private void OnCancel(object sender, EventArgs e)
        {
            LoadingScreen.Load(game, ScreenManager, false, new MainMenu(game));
        }
    }
}
