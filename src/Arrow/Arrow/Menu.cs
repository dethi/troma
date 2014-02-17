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
    public class Menu : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public static bool playerOff = false;
        
        private Game game;
        
        public bool menuActivate = false;
        private double lastTime = 0;

        public Menu(Game game)
            : base(game)
        {
            this.game = game;
        }

        public override void Initialize()
        {
            this.game.IsMouseVisible = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }


        public bool PlayerTrue()
        {
            if (menuActivate)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        public override void Update(GameTime gameTime)
        {
            //rend la souris visible durant l activation du menu
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
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            double currentTime = gameTime.TotalGameTime.TotalMilliseconds;

            //detecte l activation du menu (Touche P)
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.P))
            {
                if (lastTime + 400 <= currentTime)
                {
                    lastTime = currentTime;

                    if (menuActivate)
                    {
                        menuActivate = false;
                    }
                    else
                    {
                        menuActivate = true;
                    }
                }
            }
        }
    }
}
