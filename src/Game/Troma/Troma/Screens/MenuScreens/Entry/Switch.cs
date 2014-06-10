using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine;

namespace Troma
{
    public class Switch : IEntry
    {
        private Vector2 _position;
        private Vector2 _colmunsPos;

        private readonly Vector2 originPos;
        private readonly Color color;
        private readonly Color selectedColor;

        public event EventHandler ChangedValue;

        public string Text { get; set; }
        public float Scale { get; set; }
        public EntryType Type { get; private set; }
        public bool Activated { get; private set; }

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

        public Switch(string text, float scale, Vector2 pos, bool activated)
        {
            Type = EntryType.Switch;

            Text = text;
            Scale = scale;
            originPos = pos;

            color = new Color(112, 97, 63);
            selectedColor = Color.White;

            Activated = activated;
        }

        public void Draw(GameTime gameTime, MenuScreen screen, bool isSelected)
        {
            Color colorYes = (Activated) ? selectedColor : color;
            Color colorNo = (Activated) ? color : selectedColor;
            colorYes *= screen.TransitionAlpha;
            colorNo *= screen.TransitionAlpha;

            float widthScale = (float)GameServices.GraphicsDevice.Viewport.Width / 1920;
            float heightScale = (float)GameServices.GraphicsDevice.Viewport.Height / 1080;
            float midScale = (widthScale + heightScale) / 2;

            _position.X = originPos.X * widthScale;
            _position.Y = originPos.Y * heightScale;
            _colmunsPos.Y = _position.Y;

            GameServices.SpriteBatch.DrawString(screen.SpriteFont, Text, Position, Color.Black * screen.TransitionAlpha, 
                0, Vector2.Zero, Scale * midScale, SpriteEffects.None, 0);

            GameServices.SpriteBatch.DrawString(screen.SpriteFont, Resource.Yes, _colmunsPos, colorYes,
                0, Vector2.Zero, Scale * midScale, SpriteEffects.None, 0);

            _colmunsPos.X += (screen.SpriteFont.MeasureString(Resource.Yes).X + 30) * Scale * midScale;

            GameServices.SpriteBatch.DrawString(screen.SpriteFont, Resource.No, _colmunsPos, colorNo,
                0, Vector2.Zero, Scale * midScale, SpriteEffects.None, 0);
        }

        public void OnChangeValue(bool p)
        {
            Activated = p;

            if (ChangedValue != null)
                ChangedValue(this, new EventArgs());
        }
    }
}
