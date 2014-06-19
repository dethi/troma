using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine;

namespace Troma
{
    public class Button : IEntry
    {
        private Vector2 _position;

        public Vector2 originPos;
        private readonly Color color;
        private readonly Color selectedColor;

        public event EventHandler Selected;

        public string Text { get; set; }
        public float Scale { get; set; }
        public Vector2 ColumnsPos { get; set; }
        public EntryType Type { get; private set; }

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public Button(string text, float scale, Vector2 pos)
        {
            Type = EntryType.Button;

            Text = text;
            Scale = scale;
            originPos = pos;

            color = Color.Black;
            selectedColor = Color.White;
        }

        public void Draw(GameTime gameTime, MenuScreen screen, bool isSelected)
        {
            Color c = (isSelected) ? selectedColor : color;
            c *= screen.TransitionAlpha;

            float widthScale = (float)GameServices.GraphicsDevice.Viewport.Width / 1920;
            float heightScale = (float)GameServices.GraphicsDevice.Viewport.Height / 1080;

            _position.X = originPos.X * widthScale;
            _position.Y = originPos.Y * heightScale;

            GameServices.SpriteBatch.DrawString(screen.SpriteFont, Text, Position, c, 0,
                Vector2.Zero, Scale * (widthScale + heightScale) / 2, SpriteEffects.None, 0);
        }

        public void OnSelectEntry()
        {
            if (Selected != null)
                Selected(this, new EventArgs());
        }
    }
}
