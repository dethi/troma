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
        public static bool playerOff = false;
        
        private Game game;
        
        public bool menuActivate = false;
        private double lastTime = 0;

        public Menu(Game game)
        {
            this.game = game;
        }

        public virtual void Initialize()
        {
            this.game.IsMouseVisible = true;
        }

        public bool PlayerTrue()
        {
            return menuActivate;
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
                    menuActivate = !menuActivate;
                }
            }

            //rend la souris visible durant l'activation du menu
            if (menuActivate)
            {
                this.game.IsMouseVisible = true;
                playerOff = true;
            }
            else
            {
                this.game.IsMouseVisible = false;
                playerOff = false;
            }
        }

        public virtual void Draw(GameTime gameTime) { }
    }
}
