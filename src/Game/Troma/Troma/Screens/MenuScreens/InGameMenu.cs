using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine;

namespace Troma
{
    class InGameMenu : MenuScreen
    {
        private Button resumeMenuEntry;
        private Button restartMenyEntry;
        private Button optionMenuEntry;
        private Button backMenuEntry;

        private Texture2D bgTrans;
        private Texture2D arrow;

        private Rectangle bgTransRect;
        private Rectangle arrowRect;

        public InGameMenu(Game game)
            : base(game)
        {
            Vector2 entryPos = new Vector2(143, 435);
            float space = 146;

            // Create menu entries.
            resumeMenuEntry = new Button(String.Empty, 1, entryPos);
            entryPos.Y += space;
            restartMenyEntry = new Button(String.Empty, 1, entryPos);
            entryPos.Y += space;
            optionMenuEntry = new Button(String.Empty, 1, entryPos);
            entryPos.Y += space;
            backMenuEntry = new Button(String.Empty, 1, entryPos);

            // Hook up menu event handlers.
            resumeMenuEntry.Selected += ResumeMenuEntrySelected;
            restartMenyEntry.Selected += RestartMenuEntrySelected;
            optionMenuEntry.Selected += OptionsMenuEntrySelected;
            backMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(resumeMenuEntry);
            MenuEntries.Add(restartMenyEntry);
            MenuEntries.Add(optionMenuEntry);
            MenuEntries.Add(backMenuEntry);

            IsHUD = true;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            bgTrans = GameServices.Game.Content.Load<Texture2D>("Menus/translucide");
            arrow = GameServices.Game.Content.Load<Texture2D>("Menus/arrow");

            bgTransRect = new Rectangle(0, 0, 550, 1080);
            arrowRect = new Rectangle(0, 0, 64, 64);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            SetMenuEntryText();
        }

        public override void Draw(GameTime gameTime)
        {
            int width = GameServices.GraphicsDevice.Viewport.Width;
            int height = GameServices.GraphicsDevice.Viewport.Height;

            float widthScale = (float)width / 1920;
            float heightScale = (float)height / 1080;

            bgTransRect.Height = height;
            bgTransRect.Width = (int)(500 * widthScale);

            GameServices.SpriteBatch.Begin();
            GameServices.SpriteBatch.Draw(bgTrans, bgTransRect, Color.White * TransitionAlpha * 0.15f);

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

        private void ResumeMenuEntrySelected(object sender, EventArgs e)
        {
            InputState.MouseResetPos();
            ExitScreen();
        }

        private void RestartMenuEntrySelected(object sender, EventArgs e)
        {
            LoadingScreen.Load(game, ScreenManager, true, new SoloScreen(game, ""));
        }

        private void OptionsMenuEntrySelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenu(game));
        }

        private void OnCancel(object sender, EventArgs e)
        {
            LoadingScreen.Load(game, ScreenManager, false, new MainMenu(game));
        }

        private void SetMenuEntryText()
        {
            resumeMenuEntry.Text = Resource.Resume;
            restartMenyEntry.Text = Resource.Restart;
            optionMenuEntry.Text = "Options";
            backMenuEntry.Text = Resource.Exit;
        }
    }
}
