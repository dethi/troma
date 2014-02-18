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
    public class MemoryUse : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Game game;
        private SpriteBatch spriteBatch;
        private SpriteFont spriteFont;
        private float memory;


        public MemoryUse(Game game)
            : base(game)
        {
            this.game = game;
        }

        public override void Initialize()
        {
            this.spriteBatch = new SpriteBatch(game.GraphicsDevice);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.spriteFont = this.Game.Content.Load<SpriteFont>("Fonts/FPS");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            memory = GC.GetTotalMemory(false) / 1048576f;
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            string str = String.Format("{0:F3} Mo used", memory);
            Vector2 size = this.spriteFont.MeasureString(str);

            this.spriteBatch.Begin();
            this.spriteBatch.DrawString(this.spriteFont, str,
                new Vector2(5, GraphicsDevice.Viewport.Height - size.Y - 5), Color.Gold);
            this.spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
