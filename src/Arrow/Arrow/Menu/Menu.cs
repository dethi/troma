using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Arrow
{
    public class Menu
    {
        private Game game;
        private double lastTime = 0;

        public bool DisplayMenu { get; protected set; }

        public Menu(Game game)
        {
            this.game = game;
        }

        public virtual void Initialize()
        {
            this.game.IsMouseVisible = true;
        }

        public virtual void Update(GameTime gameTime)
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

            //rend la souris visible durant l'activation du menu
            if (DisplayMenu)
                this.game.IsMouseVisible = true;
            else
                this.game.IsMouseVisible = false;
        }

        public virtual void Draw(GameTime gameTime) { }
    }
}
