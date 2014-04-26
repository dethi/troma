﻿using System;
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

        private float selectionFade;
        private readonly Color notSelectedColor;

        public event EventHandler Selected;

        public MenuEntry(string text)
        {
            this.Text = text;
            notSelectedColor = new Color(150, 145, 140);
        }

        public virtual void Update(GameTime gameTime, MenuScreen screen, bool isSelected)
        {
            selectionFade = (isSelected) ? 1 : 0;
        }

        public virtual void Draw(GameTime gameTime, MenuScreen screen, bool isSelected)
        {
            // Pulsate the size of the selected menu entry.            
            float pulsate = (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 6) + 1;

            Color selectedColor = new Color(
                64 * pulsate / 700, 
                56 * pulsate / 700, 
                48 * pulsate / 700);

            Color color = (isSelected) ? selectedColor : notSelectedColor;
            color *= screen.TransitionAlpha;

            float scale = ((0.75f + selectionFade * 0.5f + 
                selectionFade * pulsate * 0.1f) * 
                GameServices.GraphicsDevice.Viewport.Width) / 1980;
            Vector2 origin = new Vector2(0, screen.Font.LineSpacing / 2);

            GameServices.SpriteBatch.DrawString(screen.Font, Text, Position, 
                color, -0.35f, origin, scale, SpriteEffects.None, 0);
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
