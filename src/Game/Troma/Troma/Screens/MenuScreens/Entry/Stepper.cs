using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine;

namespace Troma
{
    public class Stepper : IEntry
    {
        public string[] Choices;
        public int SelectedChoice;

        private Vector2 _position;
        private Vector2 _colmunsPos;

        private readonly Vector2 originPos;
        private readonly Color color;
        private readonly Color selectedColor;
        private Texture2D goLeft;
        private Texture2D goRight;
        private Rectangle rect;

        public event EventHandler ChangedValue;

        public string Text { get; set; }
        public float Scale { get; set; }
        public EntryType Type { get; private set; }

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public Vector2 ColumnsPos
        {
            get { return _colmunsPos; }
            set { _colmunsPos = value; }
        }

        public Stepper(string text, float scale, Vector2 pos, int nbChoice, int selected)
        {
            Type = EntryType.Stepper;

            Text = text;
            Scale = scale;
            originPos = pos;

            color = new Color(112, 97, 63);
            selectedColor = Color.White;
            goLeft = GameServices.Game.Content.Load<Texture2D>("Menus/go-left");
            goRight = GameServices.Game.Content.Load<Texture2D>("Menus/go-right");
            rect = new Rectangle(0, 0, 60, 60);

            Choices = new String[nbChoice];
            SelectedChoice = selected;
        }

        public void Draw(GameTime gameTime, MenuScreen screen, bool isSelected)
        {
            Color c = (isSelected) ? selectedColor : color;
            c *= screen.TransitionAlpha;

            float widthScale = (float)GameServices.GraphicsDevice.Viewport.Width / 1920;
            float heightScale = (float)GameServices.GraphicsDevice.Viewport.Height / 1080;
            float midScale = (widthScale + heightScale) / 2;

            _position.X = originPos.X * widthScale;
            _position.Y = originPos.Y * heightScale;
            _colmunsPos.Y = _position.Y;

            rect.Height = (int)(64 * midScale);
            rect.Width = rect.Height;
            rect.X = (int)_colmunsPos.X;
            rect.Y = (int)_position.Y;

            StringBuilder tmp = new StringBuilder();
            foreach (string s in Choices)
                tmp.AppendLine(s);
            float space = screen.SpriteFont.MeasureString(tmp.ToString()).X * midScale * Scale;

            GameServices.SpriteBatch.DrawString(screen.SpriteFont, Text, Position, Color.Black * screen.TransitionAlpha,
                0, Vector2.Zero, Scale * (widthScale + heightScale) / 2, SpriteEffects.None, 0);

            if (SelectedChoice > 0)
                GameServices.SpriteBatch.Draw(goLeft, rect, Color.White * screen.TransitionAlpha);

            rect.X += (int)(90 * midScale);

            GameServices.SpriteBatch.DrawString(screen.SpriteFont, Choices[SelectedChoice],
                new Vector2(rect.X, rect.Y), c, 0, Vector2.Zero, Scale * midScale,
                SpriteEffects.None, 0);

            rect.X += (int)(10 + space);

            if (SelectedChoice < Choices.Length - 1)
                GameServices.SpriteBatch.Draw(goRight, rect, Color.White * screen.TransitionAlpha);
        }

        public void OnChangeValue(int choice)
        {
            SelectedChoice += choice;
            SelectedChoice = Math.Max(SelectedChoice, 0);
            SelectedChoice = Math.Min(SelectedChoice, Choices.Length - 1);

            if (ChangedValue != null)
                ChangedValue(this, new EventArgs());
        }
    }
}
