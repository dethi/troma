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
        private Rectangle rectangle;

        private double lastTime = 0;
        
        private Button boutonReprendre;
        private Button boutonQuitter;
        
        public MenuPause(Game game)
            : base(game)
        {
            this.game = game;
            rectangle = new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);
        }

        public override void Initialize()
        {
            this.spriteBatch = new SpriteBatch(this.game.GraphicsDevice);
            fond = game.Content.Load<Texture2D>("Textures/overlay");
            
            Menu.Delegate quitterDelegate = new Delegate(Quitter);
            Menu.Delegate reprendreDelegate = new Delegate(Reprendre);

            boutonReprendre = (new Button(game, (game.GraphicsDevice.Viewport.Width / 2) - 125, (game.GraphicsDevice.Viewport.Height / 2) - 50 - 70, 250, 100, 
                "boutonReprendreOff", "boutonReprendre", reprendreDelegate, 1));
            boutonReprendre.Initialize();

            boutonQuitter = (new Button(game, (game.GraphicsDevice.Viewport.Width / 2) - 125, (game.GraphicsDevice.Viewport.Height / 2) - 50 + 70, 250, 100, 
                "boutonQuitterOff", "boutonQuitter", quitterDelegate, 1));
            boutonQuitter.Initialize();

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            double currentTime = gameTime.TotalGameTime.TotalMilliseconds;

            //detecte l'activation du menu (touche P ou bouton START Xbox)
            if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.P))
            {
                if (lastTime + 400 <= currentTime)
                {
                    lastTime = currentTime;
                    DisplayMenu = !DisplayMenu;

                    Mouse.SetPosition(
                        game.GraphicsDevice.Viewport.Width / 2,
                        game.GraphicsDevice.Viewport.Height / 2);
                }
            }

            
            boutonReprendre.Update(gameTime);
            boutonQuitter.Update(gameTime);
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (DisplayMenu)
            {
                this.spriteBatch.Begin();
                this.spriteBatch.Draw(fond, rectangle, Color.White * 0.8f);
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
