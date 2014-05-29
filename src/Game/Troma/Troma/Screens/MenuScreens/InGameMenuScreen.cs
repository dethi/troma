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
        private MenuEntry resumeMenuEntry;
        private MenuEntry optionMenuEntry;
        private MenuEntry backMenuEntry;

        public InGameMenuScreen(Game game)
            : base(game, "Pause")
        {
            resumeMenuEntry = new MenuEntry(string.Empty,0.60f,0,false);
            optionMenuEntry = new MenuEntry(string.Empty, 0.60f, 0, false);
            backMenuEntry = new MenuEntry(string.Empty, 0.60f, 0, false);

            SetMenuEntryText();

            resumeMenuEntry.Selected += ResumeMenuEntrySelected;
            optionMenuEntry.Selected += OptionsMenuEntrySelected;
            backMenuEntry.Selected += OnCancel;

            MenuEntries.Add(resumeMenuEntry);
            MenuEntries.Add(optionMenuEntry);
            MenuEntries.Add(backMenuEntry);

            IsHUD = true;
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            SetMenuEntryText();
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
            SetMenuEntryText();
        }

        private void OptionsMenuEntrySelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen(game));
            SetMenuEntryText();
        }

        private void OnCancel(object sender, EventArgs e)
        {
            LoadingScreen.Load(game, ScreenManager, false, new MainMenuScreen(game));
            SetMenuEntryText();
        }
        private void SetMenuEntryText()
        {
            resumeMenuEntry.Text = Resource.Resume;
            optionMenuEntry.Text = "Options";
            backMenuEntry.Text = Resource.Back;
        }
    }
}
