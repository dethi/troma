using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine;

namespace Troma
{
    public class Entry
    {
        public string Text;
        public Vector2 Position;
        public float Scale;

        private readonly Vector2 originPos;
        private readonly Color color;
        private readonly Color selectedColor;

        public event EventHandler Selected;

        public Entry(string text, float scale, Vector2 pos)
        {
            Text = text;
            Scale = scale;
            originPos = pos;

            color = Color.Black;
            selectedColor = Color.White;
        }

        public virtual void Draw(GameTime gameTime, MenuScreen screen, bool isSelected)
        {
            Color c = (isSelected) ? selectedColor : color;
            c *= screen.TransitionAlpha;

            float widthScale = (float)GameServices.GraphicsDevice.Viewport.Width / 1920;
            float heightScale = (float)GameServices.GraphicsDevice.Viewport.Height / 1080;

            Position = originPos * heightScale * Scale;

            GameServices.SpriteBatch.DrawString(screen.SpriteFont, Text, Position, c, 0,
                Vector2.Zero, Scale * (widthScale + heightScale) / 2, SpriteEffects.None, 0);
        }

        protected internal void OnSelectEntry()
        {
            if (Selected != null)
                Selected(this, new EventArgs());
        }

        public int GetHeight(MenuScreen screen)
        {
            return screen.SpriteFont.LineSpacing;
        }

        public int GetWidth(MenuScreen screen)
        {
            return (int)screen.SpriteFont.MeasureString(Text).X;
        }
    }
}
