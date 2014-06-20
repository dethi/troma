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
    class EndGameMulti : MenuScreen
    {
        private Button backMenuEntry;

        private SpriteFont font;

        private Texture2D bg;
        private Texture2D bgTrans;
        private Texture2D arrow;

        private Rectangle bgRect;
        private Rectangle bgTransRect;
        private Rectangle arrowRect;

        private String login;
        private String score;
        private Vector2 loginPos;
        private Vector2 scorePos;

        private Tuple<int, string, int>[] scoring;

        public EndGameMulti(Game game, Tuple<int, string, int>[] scoring)
            : base(game)
        {
            Vector2 entryPos = new Vector2(143, 875);

            // Create menu entries.
            backMenuEntry = new Button(Resource.Back, 1, entryPos);

            // Hook up menu event handlers.
            backMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(backMenuEntry);

            this.scoring = scoring;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            font = GameServices.Game.Content.Load<SpriteFont>("Fonts/Digital");
            font.Spacing = 5f;

            bg = GameServices.Game.Content.Load<Texture2D>("Menus/background-blur");
            bgTrans = GameServices.Game.Content.Load<Texture2D>("Menus/translucide");
            arrow = GameServices.Game.Content.Load<Texture2D>("Menus/arrow");

            bgRect = new Rectangle(0, 0, 1920, 1080);
            bgTransRect = new Rectangle(0, 0, 0, 0);
            arrowRect = new Rectangle(0, 0, 64, 64);

            Dictionary<KeyActions, Keys> kb = InputConfiguration.GetKeybord();

            StringBuilder tmp = new StringBuilder();

            foreach (Tuple<int, string, int> e in scoring)
                tmp.AppendLine(e.Item2);

            login = tmp.ToString();

            tmp.Clear();

            foreach (Tuple<int, string, int> e in scoring)
                tmp.AppendLine(e.Item3.ToString());

            score = tmp.ToString();
        }

        public override void Draw(GameTime gameTime)
        {
            int width = GameServices.GraphicsDevice.Viewport.Width;
            int height = GameServices.GraphicsDevice.Viewport.Height;

            float widthScale = (float)width / 1920;
            float heightScale = (float)height / 1080;
            float scale = 0.9f * (widthScale + heightScale) / 2;

            bgRect.Height = height;
            bgRect.Width = (int)(height * 1.778f);
            bgRect.X = -(bgRect.Width - width) / 2;

            loginPos = new Vector2(
                143 * widthScale,
                143 * heightScale);

            scorePos = new Vector2(
                loginPos.X + font.MeasureString(login).X * scale * 0.35f + 200 * widthScale,
                143 * heightScale);

            float lineSpacing = font.LineSpacing * scale * 0.35f;
            int nbLine = login.Split('\n').Length - 1;

            bgTransRect.X = (int)(133 * widthScale);
            bgTransRect.Height = (int)(lineSpacing);
            bgTransRect.Width = (int)(scorePos.X - loginPos.X + (font.MeasureString(score).X + 40) * scale * 0.35f);

            GameServices.SpriteBatch.Begin();

            GameServices.SpriteBatch.Draw(bg, bgRect, Color.White);

            for (float i = 0; i < nbLine; i += 2)
            {
                bgTransRect.Y = (int)(loginPos.Y - 1 + i * lineSpacing);
                GameServices.SpriteBatch.Draw(bgTrans, bgTransRect, Color.White * TransitionAlpha * 0.15f);
            }

            GameServices.SpriteBatch.DrawString(font, login, loginPos, Color.Black * TransitionAlpha, 0,
                Vector2.Zero, scale * 0.35f, SpriteEffects.None, 0);
            GameServices.SpriteBatch.DrawString(font, score, scorePos, Color.Ivory * TransitionAlpha, 0,
                Vector2.Zero, scale * 0.35f, SpriteEffects.None, 0);

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
