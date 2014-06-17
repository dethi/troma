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
        private Button backMenuEntry;

        private Texture2D bg;
        private Texture2D bgTrans;
        private Texture2D arrow;

        private Rectangle bgRect;
        private Rectangle bgTransRect;
        private Rectangle arrowRect;

        private SpriteFont digitalFont;

        private string label;
        private string value;

        private Vector2 labelPos;
        private Vector2 valuePos;

        public ScoreMenu(Game game)
            : base(game)
        {
            Vector2 entryPos = new Vector2(143, 875);

            // Create menu entries.
            backMenuEntry = new Button(Resource.Back, 1, entryPos);

            // Hook up menu event handlers.
            backMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(backMenuEntry);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            bg = GameServices.Game.Content.Load<Texture2D>("Menus/background-blur");
            bgTrans = GameServices.Game.Content.Load<Texture2D>("Menus/translucide");
            arrow = GameServices.Game.Content.Load<Texture2D>("Menus/arrow");

            digitalFont = GameServices.Game.Content.Load<SpriteFont>("Fonts/Digital");
            digitalFont.Spacing = 10f;

            bgRect = new Rectangle(0, 0, 1920, 1080);
            bgTransRect = new Rectangle(0, 0, 0, 0);
            arrowRect = new Rectangle(0, 0, 64, 64);

            Score score = ScoreManager.Load();

            StringBuilder tmp = new StringBuilder();
            tmp.AppendLine(Resource.HighScore);
            tmp.Append("\n\n\n\n\n\n");
            tmp.AppendLine(Resource.AveragePrecision);
            tmp.AppendLine(Resource.TotalGameTime);
            label = tmp.ToString();

            tmp.Clear();

            for (int i = 0; i < score.HighScore.Length; i++)
                tmp.AppendLine(score.HighScore[i].ToString());
            tmp.Append("\n\n");
            tmp.AppendLine(score.AveragePrecision + " %");
            TimeSpan t = TimeSpan.FromSeconds(score.TotalElapsedTime);
            tmp.AppendLine(String.Format("{0:D2}:{1:D2}:{2:D2}", t.Hours, t.Minutes,t.Seconds));
            value = tmp.ToString();
        }

        public override void Draw(GameTime gameTime)
        {
            int width = GameServices.GraphicsDevice.Viewport.Width;
            int height = GameServices.GraphicsDevice.Viewport.Height;

            float widthScale = (float)width / 1920;
            float heightScale = (float)height / 1080;
            float scale = (widthScale + heightScale) / 2;

            bgRect.Height = height;
            bgRect.Width = (int)(height * 1.778f);
            bgRect.X = -(bgRect.Width - width) / 2;

            labelPos = new Vector2(
                143 * widthScale,
                143 * heightScale);

            valuePos = new Vector2(
                labelPos.X + digitalFont.MeasureString(label).X * scale * 0.25f + 150 * widthScale,
                143 * heightScale);

            float lineSpacing = digitalFont.LineSpacing * scale * 0.25f;
            int nbLine = value.Split('\n').Length - 1;

            bgTransRect.X = (int)(123 * widthScale);
            bgTransRect.Y = (int)(123 * heightScale);
            bgTransRect.Width = (int)(valuePos.X + 130 * scale);
            bgTransRect.Height = (int)((lineSpacing + 5 * scale) * nbLine);

            GameServices.SpriteBatch.Begin();

            GameServices.SpriteBatch.Draw(bg, bgRect, Color.White * TransitionAlpha);
            GameServices.SpriteBatch.Draw(bgTrans, bgTransRect, Color.White * TransitionAlpha * 0.15f);

            GameServices.SpriteBatch.DrawString(digitalFont, label, labelPos, Color.Black * TransitionAlpha, 0,
                Vector2.Zero, scale * 0.25f, SpriteEffects.None, 0);

            GameServices.SpriteBatch.DrawString(digitalFont, value, valuePos, Color.Ivory * TransitionAlpha, 0,
                Vector2.Zero, scale * 0.25f, SpriteEffects.None, 0);

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
