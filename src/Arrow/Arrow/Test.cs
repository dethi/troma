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
    //
    // Include all your test here instead in class Game
    // This class have the same structure as Game (Properties and Methods)
    //
    class Test
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private ContentManager Content;

        // TODO: Add your properties here
        #region Properties
        // Here
        #endregion

        // Don't modify this methods
        public Test(GraphicsDeviceManager graphics, ContentManager Content)
        {
            this.Content = Content;
            this.graphics = graphics;
        }

        public void Initialize()
        {
            // TODO: Add your initialization logic here
        }

        public void LoadContent(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;

            // TODO: use this.Content to load your game content here
        }

        public void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
        }

        public void Draw(GameTime gameTime)
        {
            // TODO: Add your drawing code here
        }
    }
}
