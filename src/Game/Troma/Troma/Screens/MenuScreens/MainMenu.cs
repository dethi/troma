using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine;

namespace Troma
{
    public class MainMenu : MenuScreen
    {
        private Button soloMenuEntry;
        private Button multiMenuEntry;
        private Button scoreMenuEntry;
        private Button optionsMenuEntry;
        private Button exitMenuEntry;

        private SpriteFont tromaFont;
        private Vector2 tromaPos;
        private Color tromaColor;

        private Texture2D bg;
        private Texture2D bgTrans;
        private Texture2D arrow;

        private Rectangle bgRect;
        private Rectangle bgTransRect;
        private Rectangle arrowRect;

        public MainMenu(Game game)
            : base(game)
        {
            Vector2 entryPos = new Vector2(143, 435);
            float space = 110;

            // Create menu entries.
            soloMenuEntry = new Button(string.Empty, 1, entryPos);
            entryPos.Y += space;
            multiMenuEntry = new Button(string.Empty, 1, entryPos);
            entryPos.Y += space;
            scoreMenuEntry = new Button(string.Empty, 1, entryPos);
            entryPos.Y += space;
            optionsMenuEntry = new Button(string.Empty, 1, entryPos);
            entryPos.Y += space;
            exitMenuEntry = new Button(string.Empty, 1, entryPos);

            // Hook up menu event handlers.
            soloMenuEntry.Selected += SoloMenuEntrySelected;
            multiMenuEntry.Selected += MultiMenuEntrySelected;
            scoreMenuEntry.Selected += ScoreMenuEntrySelected;
            optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(soloMenuEntry);
            MenuEntries.Add(multiMenuEntry);
            MenuEntries.Add(scoreMenuEntry);
            MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(exitMenuEntry);

            SceneRenderer.InitializeMenu();
        }

        public override void LoadContent()
        {
            base.LoadContent();

            tromaFont = GameServices.Game.Content.Load<SpriteFont>("Fonts/LuckyTypewriter");
            tromaColor = new Color(112, 97, 63);

            bg = GameServices.Game.Content.Load<Texture2D>("Menus/background");
            bgTrans = GameServices.Game.Content.Load<Texture2D>("Menus/translucide");
            arrow = GameServices.Game.Content.Load<Texture2D>("Menus/arrow");

            bgRect = new Rectangle(0, 0, 1920, 1080);
            bgTransRect = new Rectangle(0, 0, 550, 1080);
            arrowRect = new Rectangle(0, 0, 64, 64);

            SoundManager.Play("Menu");
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

            bgRect.Height = height;
            bgRect.Width = (int)(height * 1.778f);
            bgRect.X = -(bgRect.Width - width) / 2;

            bgTransRect.Height = height;
            bgTransRect.Width = (int)(500 * widthScale);

            tromaPos = new Vector2(
                1380 * widthScale,
                50 * heightScale);

            GameServices.SpriteBatch.Begin();

            GameServices.SpriteBatch.Draw(bg, bgRect, Color.White * TransitionAlpha);
            GameServices.SpriteBatch.Draw(bgTrans, bgTransRect, Color.White * TransitionAlpha * 0.15f);
            GameServices.SpriteBatch.DrawString(tromaFont, "troma", tromaPos, tromaColor * TransitionAlpha, 0,
                Vector2.Zero, (widthScale + heightScale) / 2, SpriteEffects.None, 0);

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

        private void SoloMenuEntrySelected(object sender, EventArgs e)
        {
            SoundManager.Stop();
            LoadingScreen.Load(game, ScreenManager, true, new SoloScreen(game, Map.Town));
        }

        private void MultiMenuEntrySelected(object sender, EventArgs e)
        {
            //SoundManager.Stop();
        }

        private void ScoreMenuEntrySelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new ScoreMenu(game));
        }

        private void OptionsMenuEntrySelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenu(game));
        }

        protected override void OnCancel(object sender, EventArgs e)
        {
            SoundManager.Stop();
            game.Exit();
        }

        private void SetMenuEntryText()
        {
            soloMenuEntry.Text = "Solo";
            multiMenuEntry.Text = Resource.Multiplayer;
            scoreMenuEntry.Text = "Score";
            optionsMenuEntry.Text = "Options";
            exitMenuEntry.Text = Resource.Exit;
        }
    }
}
