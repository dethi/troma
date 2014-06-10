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
        private Entry resumeMenuEntry;
        private Entry optionMenuEntry;
        private Entry backMenuEntry;

        private Texture2D bg;
        private Texture2D bgTrans;
        private Texture2D arrow;

        private Rectangle bgTransRect;
        private Rectangle arrowRect;

        public InGameMenu(Game game)
            : base(game)
        {
            Vector2 entryPos = new Vector2(143, 435);
            float space = 220;

            // Create menu entries.
            resumeMenuEntry = new Entry(String.Empty, 1, entryPos);
            entryPos.Y += space;
            optionMenuEntry = new Entry(String.Empty, 1, entryPos);
            entryPos.Y += space;
            backMenuEntry = new Entry(String.Empty, 1, entryPos);

            // Hook up menu event handlers.
            resumeMenuEntry.Selected += ResumeMenuEntrySelected;
            optionMenuEntry.Selected += OptionsMenuEntrySelected;
            backMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(resumeMenuEntry);
            MenuEntries.Add(optionMenuEntry);
            MenuEntries.Add(backMenuEntry);

            IsHUD = true;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            bg = FileManager.Load<Texture2D>("Menus/background-blur");
            bgTrans = FileManager.Load<Texture2D>("Menus/translucide");
            arrow = FileManager.Load<Texture2D>("Menus/arrow");

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

        private void OptionsMenuEntrySelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen(game));
        }

        private void OnCancel(object sender, EventArgs e)
        {
            LoadingScreen.Load(game, ScreenManager, false, new MainMenu(game));
        }

        private void SetMenuEntryText()
        {
            resumeMenuEntry.Text = Resource.Resume;
            optionMenuEntry.Text = "Options";
            backMenuEntry.Text = Resource.Exit;
        }
    }
}
