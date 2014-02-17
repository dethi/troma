using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arrow
{
    public class MenuPause : Menu
    {
        private Game game;
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

        
        public void Quitter()
        {
            game.Exit();
        }
        public void Reprendre()
        {
            if (menuActivate)
            {
                menuActivate = false;
            }
            else
            {
                menuActivate = true;
            }
        }

        public override void Initialize()
        {
            this.spriteBatch = new SpriteBatch(this.Game.GraphicsDevice);
            fond = game.Content.Load<Texture2D>("Textures/troma");

            Delegate quitterDelegate = new Delegate(Quitter);
            Delegate reprendreDelegate = new Delegate(Reprendre);

            boutonReprendre = (new Button(game, 100, 100, 128, 32, "boutonReprendreOff", "boutonReprendre", reprendreDelegate, 1));
            boutonReprendre.Initialize();

            boutonQuitter = (new Button(game, 100, 150, 128, 32, "boutonQuitterOff", "boutonQuitter", quitterDelegate, 1));
            boutonQuitter.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            boutonReprendre.Update(gameTime);
            boutonQuitter.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (menuActivate)
            {
                this.spriteBatch.Begin();
                this.spriteBatch.Draw(fond, rectangle, Color.White * 0.6f);
                this.spriteBatch.End();

                boutonReprendre.Draw(gameTime);
                boutonQuitter.Draw(gameTime);
            }
            base.Draw(gameTime);
        }
    }
}
