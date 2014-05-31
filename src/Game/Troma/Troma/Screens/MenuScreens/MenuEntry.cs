using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine;

namespace Troma
{
    public class MenuEntry
    {
        public string Text;
        public Vector2 Position;
        public float FontSize;

        private float _rotation;
        private bool _isPulsate;

        private float selectionFade;
        private readonly Color notSelectedColor;
        private readonly Color selectedColor;

        private float pulsate;

        public event EventHandler Selected;

        public MenuEntry(string text)
            : this(text, 0.75f, -0.35f, true)
        { }

        public MenuEntry(string text, float fontSize, float rot, bool isPulsate)
        {
            this.Text = text;
            _rotation = rot;
            _isPulsate = isPulsate;
            FontSize = fontSize;

            notSelectedColor = new Color(150, 145, 140);
            selectedColor = Color.Black;
        }

        public void Update(GameTime gameTime, MenuScreen screen, bool isSelected)
        {
            selectionFade = (isSelected) ? 1 : 0;
        }

        public virtual void Draw(GameTime gameTime, MenuScreen screen, bool isSelected)
        {
            // Pulsate the size of the selected menu entry.
            if (_isPulsate)
                pulsate = (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 6) + 1;
            else
                pulsate = 0;

            Color color = (isSelected) ? selectedColor : notSelectedColor;
            color *= screen.TransitionAlpha;

            float scale = ((FontSize + selectionFade * 0.5f + 
                selectionFade * pulsate * 0.1f) * 
                GameServices.GraphicsDevice.Viewport.Width) / 1980;

            Vector2 origin = new Vector2(0, screen.Font.LineSpacing / 2);

            GameServices.SpriteBatch.DrawString(screen.Font, Text, Position, color, _rotation, 
                origin, scale, SpriteEffects.None, 0);
        }

        protected internal void OnSelectEntry()
        {
            if (Selected != null)
                Selected(this, new EventArgs());
        }

        public int GetHeight(MenuScreen screen)
        {
            return screen.Font.LineSpacing;
        }

        public int GetWidth(MenuScreen screen)
        {
            return (int)screen.Font.MeasureString(Text).X;
        }
    }
}
