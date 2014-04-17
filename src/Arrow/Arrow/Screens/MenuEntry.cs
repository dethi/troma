using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arrow
{
    class MenuEntry
    {
        string text;
        float selectionFade;
        Vector2 position;
        Color nonselectedColor = new Color(150, 145, 140);

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public event EventHandler Selected;

        public MenuEntry(string text)
        {
            this.text = text;
        }

        public virtual void Update(MenuScreen screen, bool isSelected, GameTime gameTime)
        {
            float fadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 4;

            if (isSelected)
                selectionFade = 1;
            else
                selectionFade = 0;
        }
        
        public virtual void Draw(MenuScreen screen, bool isSelected, GameTime gameTime, Game game)
        {

            // Pulsate the size of the selected menu entry.            
            double time = gameTime.TotalGameTime.TotalSeconds;
            float pulsate = (float)Math.Sin(time * 6) + 1;
            
            float r = 64 * pulsate/700 ;
            float g = 56 * pulsate/700;
            float b = 48 * pulsate/700 ;
            Color selectedColor = new Color(r, g, b);

            Color color = isSelected ? selectedColor : nonselectedColor;
            

            float scale = ((0.75f + selectionFade * 0.5f + selectionFade * pulsate * 0.1f) * game.GraphicsDevice.Viewport.Width)/1980;
            // Modify the alpha to fade text out during transitions.
            color *= screen.TransitionAlpha;

            // Draw text, centered on the middle of each line.
            ScreenManager screenManager = screen.ScreenManager;
            SpriteBatch spriteBatch = screenManager.SpriteBatch;
            SpriteFont font = screen.Font;

            Vector2 origin = new Vector2(0, font.LineSpacing/2);

            spriteBatch.DrawString(font, text, position, color, -0.35f,
                                   origin, scale, SpriteEffects.None, 0);
        }

        protected internal virtual void OnSelectEntry()
        {
            if (Selected != null)
                Selected(this, new EventArgs()) ;
        }

        public virtual int GetHeight(MenuScreen screen)
        {
            return screen.Font.LineSpacing;
        }
        
        public virtual int GetWidth(MenuScreen screen)
        {
            return (int)screen.Font.MeasureString(Text).X;
        }
    }
}
