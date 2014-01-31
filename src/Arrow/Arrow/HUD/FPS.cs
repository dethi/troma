using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Arrow
{
    public class FPS : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private SpriteFont spriteFont;
        private double fps = 0.0f;

        public FPS(Game game)
            : base(game)
        { }

        public override void Initialize()
        {
            this.spriteBatch = new SpriteBatch(this.Game.GraphicsDevice);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.spriteFont = this.Game.Content.Load<SpriteFont>("Fonts/FPS");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            this.fps = 1000.0d / gameTime.ElapsedGameTime.TotalMilliseconds;
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            string str = string.Format("{0:00.00} FPS", this.fps);
            Vector2 size = this.spriteFont.MeasureString(str);

            this.spriteBatch.Begin();
            this.spriteBatch.DrawString(this.spriteFont, str, 
                new Vector2(this.GraphicsDevice.Viewport.Width - size.X - 5, 5), Color.Gold);
            this.spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
