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
    public class DisplayPosition : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Game game;
        private SpriteBatch spriteBatch;
        private SpriteFont spriteFont;

        public DisplayPosition(Game game)
            : base(game)
        {
            this.game = game;
        }

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

        public override void Draw(GameTime gameTime)
        {
            string str = String.Format("X: {0}\nY: {1}\nZ: {2}", game.camera.Position.X,
                game.camera.Position.Y, game.camera.Position.Z);
            Vector2 size = this.spriteFont.MeasureString(str);

            this.spriteBatch.Begin();
            this.spriteBatch.DrawString(this.spriteFont, str,
                new Vector2(5, 5), Color.Gold);
            this.spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
