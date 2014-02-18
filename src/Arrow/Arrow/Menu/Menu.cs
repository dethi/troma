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
        protected Game game;
        protected MenuSong audio;
        protected SpriteBatch spriteBatch;

        public delegate void Delegate();

        public bool DisplayMenu { get; protected set; }

        public Menu(Game game)
        {
            this.game = game;
            DisplayMenu = true;

            this.spriteBatch = new SpriteBatch(this.game.GraphicsDevice);
        }

        public virtual void Initialize()
        {
            this.game.IsMouseVisible = true;
            audio = new MenuSong(game);
        }

        public virtual void LoadContent()
        {
            audio.LoadContent();
        }

        public virtual void Update(GameTime gameTime)
        {         
            //rend la souris visible durant l'activation du menu
            if (DisplayMenu)
            {
                this.game.IsMouseVisible = true;
                audio.SongPlayed = true;
            }
            else
            {
                this.game.IsMouseVisible = false;
                audio.SongPlayed = false;
            }
        }

        public virtual void Draw(GameTime gameTime) { }
    }
}
