using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine;

namespace Troma
{
    public class Numerous : IEntry
    {
        private Vector2 _position;
        private Vector2 _colmunsPos;

        private readonly Vector2 originPos;
        private readonly Color color;
        private readonly Color selectedColor;
        private Texture2D plus;
        private Texture2D minus;
        private Rectangle rect;

        private readonly int _min;
        private readonly int _max;
        private readonly int _step;
        public int Value;

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

        public Numerous(string text, float scale, Vector2 pos, int min, int max, 
            int step, int value)
        {
            Type = EntryType.Numerous;

            Text = text;
            Scale = scale;
            originPos = pos;

            color = color = new Color(112, 97, 63);
            selectedColor = Color.White;
            plus = GameServices.Game.Content.Load<Texture2D>("Menus/plus");
            minus = GameServices.Game.Content.Load<Texture2D>("Menus/minus");
            rect = new Rectangle(0, 0, 60, 60);

            _min = min;
            _max = max;
            _step = step;
            Value = value;
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

            string value = (Value < 100) ? " " + Value.ToString() : Value.ToString();

            GameServices.SpriteBatch.DrawString(screen.SpriteFont, Text, Position, Color.Black * screen.TransitionAlpha, 
                0, Vector2.Zero, Scale * (widthScale + heightScale) / 2, SpriteEffects.None, 0);

            if (Value > _min)
                GameServices.SpriteBatch.Draw(minus, rect, Color.White * screen.TransitionAlpha);

            rect.X += (int)(90 * midScale);

            GameServices.SpriteBatch.DrawString(screen.SpriteFont, value, new Vector2(rect.X, rect.Y), c, 
                0, Vector2.Zero, Scale * midScale, SpriteEffects.None, 0);

            rect.X += (int)(90 * midScale);

            if (Value < _max)
                GameServices.SpriteBatch.Draw(plus, rect, Color.White * screen.TransitionAlpha);
        }

        public void OnChangeValue(int choice)
        {
            Value += choice * _step;
            Value = Math.Max(Value, _min);
            Value = Math.Min(Value, _max);

            if (ChangedValue != null)
                ChangedValue(this, new EventArgs());
        }
    }
}
