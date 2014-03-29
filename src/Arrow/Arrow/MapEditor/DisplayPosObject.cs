using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arrow
{
    public class DisplayPosObject : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Game game;
        private EntityManager modelmanager;
        private int ieme_model;

        private SpriteBatch spriteBatch;
        private SpriteFont spriteFont;

        private string entity_name;
        private string entity_x;
        private string entity_y;
        private string entity_z;

        private Vector3 scale;
        private Quaternion rotation;
        private Vector3 translation;


        public DisplayPosObject(Game game)
            : base(game)
        {
            this.game = game;
        }
        
        public override void Initialize()
        {
            //this.modelmanager = new EntityManager(game);
            this.spriteBatch = new SpriteBatch(this.Game.GraphicsDevice);
            base.Initialize();
        }

        protected override void LoadContent()
        { 
            this.spriteFont = this.Game.Content.Load<SpriteFont>("Fonts/FPS");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            modelmanager.Entities.ElementAt(ieme_model).Value.position.Decompose(out scale, out rotation, out translation);

            this.entity_name = modelmanager.Entities.ElementAt(ieme_model).Key;
            this.entity_x = translation.X.ToString();
            this.entity_y = translation.Y.ToString();
            this.entity_z = translation.Z.ToString();

            base.Update(gameTime);
        }

        public void AssociateModel(EntityManager mod)
        {
            this.modelmanager = mod;
        }

        public void Upieme(int ieme)
        {
            this.ieme_model = ieme;
        }

        public override void Draw(GameTime gameTime)
        {
            string str = entity_name + "\n" + "X: " + entity_x + "\n" + "Y: " + entity_y + "\n" + "Z: " + entity_z;
            Vector2 size = this.spriteFont.MeasureString(str);

            this.spriteBatch.Begin();
            this.spriteBatch.DrawString(this.spriteFont, str, 
                new Vector2(this.GraphicsDevice.Viewport.Width - size.X - 5,this.GraphicsDevice.Viewport.Height - size.Y - 5), Color.Gold);
            this.spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
