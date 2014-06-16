using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine
{
    public class MuzzleFlash
    {
        public bool Show;

        private Texture2D[] flash;
        private Vector2 offset;
        private Rectangle rect;
        private int frame;
        private int startFrame = 2;

        public MuzzleFlash(Texture2D[] f, Vector2 muzzleOffset)
        {
            flash = f;
            frame = startFrame;
            offset = muzzleOffset;
            rect = new Rectangle();
            Show = false;
        }

        public void Activate()
        {
            Show = true;
            frame = startFrame;
        }

        public void Update()
        {
            if (Show)
            {
                frame++;

                if (frame == flash.Length - 1)
                {
                    frame = startFrame;
                    Show = false;
                }
            }
        }

        public void DrawHUD(bool sight)
        {
            if (Show)
            {
                int width = GameServices.GraphicsDevice.Viewport.Width;
                int height = GameServices.GraphicsDevice.Viewport.Height;

                float widthScale = (float)width / 1920;
                float heightScale = (float)height / 1080;

                rect.Width = (int)(flash[0].Width * widthScale);
                rect.Height = (int)(flash[0].Height * heightScale);
                rect.X = (int)((width - rect.Width) / 2);
                rect.Y = (int)((height - rect.Height) / 2);

                if (!sight)
                {
                    rect.X += (int)(offset.X * widthScale);
                    rect.Y += (int)(offset.Y * heightScale);
                }

                GameServices.SpriteBatch.Begin();
                GameServices.SpriteBatch.Draw(flash[frame], rect, Color.White);
                GameServices.SpriteBatch.End();
            }
        }
    }
}
