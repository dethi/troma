using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arrow
{
    public class MenuStart : Menu
    {
        public bool GameStart { get; private set; }

        private Rectangle rectangle;
        Texture2D fond;

        private Button boutonJouer;

        public MenuStart(Game game)
            : base(game)
        {
            rectangle = new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);
        }

        public override void Initialize()
        {
            GameStart = false;

            fond = game.Content.Load<Texture2D>("Textures/debarquement");

            Delegate jouerDelegate = new Delegate(Jouer);

            boutonJouer = (new Button(game, (game.GraphicsDevice.Viewport.Width / 2) - 125, (game.GraphicsDevice.Viewport.Height / 2) - 50 - 100, 250, 100,
                "boutonJouerOff", "boutonJouer", jouerDelegate, 1));
            boutonJouer.Initialize();

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (DisplayMenu)
                boutonJouer.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (DisplayMenu)
            {
                this.spriteBatch.Begin();
                this.spriteBatch.Draw(fond, rectangle, Color.White * 1f);
                this.spriteBatch.End();

                boutonJouer.Draw(gameTime);
            }
        }

        public void Jouer()
        {
            DisplayMenu = false;
            this.game.IsMouseVisible = false;
            audio.SongPlayed = false;
            GameStart = true;
        }

    }
}
