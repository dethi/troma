using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Arrow
{
    public class MenuPause : Menu
    {
        private SpriteBatch spriteBatch;

        Texture2D fond;
        private Rectangle rectangle = new Rectangle(0, 0, 2500, 2500);

        private Button boutonReprendre;
        private Button boutonQuitter;
        public delegate void Delegate();

        public MenuPause(Game game)
            : base(game)
        {
            this.game = game;
        }

        public override void Initialize()
        {
            this.spriteBatch = new SpriteBatch(this.game.GraphicsDevice);
            fond = game.Content.Load<Texture2D>("Textures/troma");

            Delegate quitterDelegate = new Delegate(Quitter);
            Delegate reprendreDelegate = new Delegate(Reprendre);

            boutonReprendre = (new Button(game, 100, 100, 128, 32, 
                "boutonReprendreOff", "boutonReprendre", reprendreDelegate, 1));
            boutonReprendre.Initialize();

            boutonQuitter = (new Button(game, 100, 150, 128, 32, 
                "boutonQuitterOff", "boutonQuitter", quitterDelegate, 1));
            boutonQuitter.Initialize();

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            boutonReprendre.Update(gameTime);
            boutonQuitter.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (DisplayMenu)
            {
                this.spriteBatch.Begin();
                this.spriteBatch.Draw(fond, rectangle, Color.White * 0.6f);
                this.spriteBatch.End();

                boutonReprendre.Draw(gameTime);
                boutonQuitter.Draw(gameTime);
            }
        }

        public void Quitter()
        {
            game.Exit();
        }
        public void Reprendre()
        {
            Mouse.SetPosition(
                game.GraphicsDevice.Viewport.Width / 2,
                game.GraphicsDevice.Viewport.Height / 2);

            DisplayMenu = !DisplayMenu;
        }
    }
}
