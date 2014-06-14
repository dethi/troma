using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine;

namespace Troma
{
    class ScoreScreen : MenuScreen
    {
        private Button restartMenuEntry;
        private Button backMenuEntry;

        private Texture2D bgTrans;
        private Texture2D arrow;

        private Rectangle bgTransRect;
        private Rectangle arrowRect;

        private SpriteFont digitalFont;

        private int score;
        private string precision;
        private string time;

        public ScoreScreen(Game game, TimeSpan elapsedTime, int nbTarget, int nbMunition)
            : base(game)
        {
            Vector2 entryPos = new Vector2(143, 435);
            float space = 220;

            // Create menu entries.
            restartMenuEntry = new Button(Resource.Restart, 1, entryPos);
            entryPos.Y += space;
            backMenuEntry = new Button(Resource.Exit, 1, entryPos);

            // Hook up menu event handlers.
            restartMenuEntry.Selected += RestartMenuEntrySelected;
            backMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(restartMenuEntry);
            MenuEntries.Add(backMenuEntry);

            score = ComputeScore(elapsedTime, nbTarget, nbMunition);
            precision = "Precision :   " + (100 * nbTarget / nbMunition) + " %";
            time = Resource.ElapsedTime + " :   " + elapsedTime.Minutes + ":" + elapsedTime.Seconds;

            IsHUD = true;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            bgTrans = GameServices.Game.Content.Load<Texture2D>("Menus/translucide");
            arrow = GameServices.Game.Content.Load<Texture2D>("Menus/arrow");

            digitalFont = GameServices.Game.Content.Load<SpriteFont>("Fonts/Digital");
            digitalFont.Spacing += 10f;

            bgTransRect = new Rectangle(0, 0, 550, 1080);
            arrowRect = new Rectangle(0, 0, 64, 64);
        }

        public override void Draw(GameTime gameTime)
        {
            int width = GameServices.GraphicsDevice.Viewport.Width;
            int height = GameServices.GraphicsDevice.Viewport.Height;

            float widthScale = (float)width / 1920;
            float heightScale = (float)height / 1080;
            float scale = (widthScale + heightScale) / 2;

            bgTransRect.Height = height;
            bgTransRect.Width = (int)(500 * widthScale);

            Vector2 pos = new Vector2(
                bgTransRect.Width + 150 * widthScale,
                300 * heightScale);

            GameServices.SpriteBatch.Begin();
            GameServices.SpriteBatch.Draw(bgTrans, bgTransRect, Color.White * TransitionAlpha * 0.15f);

            GameServices.SpriteBatch.DrawString(digitalFont, score.ToString(), pos, Color.Ivory * TransitionAlpha, 0,
                Vector2.Zero, scale, SpriteEffects.None, 0);

            pos.Y += 310 * heightScale;

            GameServices.SpriteBatch.DrawString(digitalFont, time, pos, Color.Ivory * TransitionAlpha, 0,
                Vector2.Zero, scale * 0.2f, SpriteEffects.None, 0);

            pos.Y += 55 * heightScale;

            GameServices.SpriteBatch.DrawString(digitalFont, precision, pos, Color.Ivory * TransitionAlpha, 0,
                Vector2.Zero, scale * 0.2f, SpriteEffects.None, 0);
            

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

        private void RestartMenuEntrySelected(object sender, EventArgs e)
        {
            LoadingScreen.Load(game, ScreenManager, true, new SoloScreen(game, ""));
        }

        private void OnCancel(object sender, EventArgs e)
        {
            LoadingScreen.Load(game, ScreenManager, false, new MainMenu(game));
        }

        private int ComputeScore(TimeSpan elapsedTime, int nbTarget, int nbMunition)
        {
            double t = elapsedTime.TotalSeconds;

            return (int)(37 * (100 * nbTarget + (1 / t) * 1000) / (nbMunition + 1));
        }
    }
}
