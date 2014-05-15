using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine;

namespace Troma
{
    public class InGameMenuScreen : MenuScreen
    {
        public InGameMenuScreen(Game game)
            : base(game, "Pause")
        {
            MenuEntry resumeMenuEntry = new MenuEntry(Resource.Resume);
            MenuEntry optionMenu = new MenuEntry("Options");
            MenuEntry backMenuEntry = new MenuEntry(Resource.Exit);

            resumeMenuEntry.Selected += ResumeMenuEntrySelected;
            optionMenu.Selected += OptionsMenuEntrySelected;
            backMenuEntry.Selected += OnCancel;

            MenuEntries.Add(resumeMenuEntry);
            MenuEntries.Add(optionMenu);
            MenuEntries.Add(backMenuEntry);

            IsHUD = true;
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

            GameServices.SpriteBatch.Begin();

            GameServices.SpriteBatch.DrawString(Font, menuTitle, titlePosition, Color.White, 0,
                titleOrigin, titleScale, SpriteEffects.None, 0);

            // Draw each menu entry in turn.
            for (int i = 0; i < MenuEntries.Count; i++)
            {
                bool isSelected = IsActive && (i == selectedEntry);
                MenuEntries[i].Draw(gameTime, this, isSelected);
            }

            GameServices.SpriteBatch.End();
        }

        private void ResumeMenuEntrySelected(object sender, EventArgs e)
        {
            ExitScreen();
        }

        private void OptionsMenuEntrySelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen(game));
        }

        private void OnCancel(object sender, EventArgs e)
        {
            LoadingScreen.Load(game, ScreenManager, false, new MainMenuScreen(game));
        }
    }
}
