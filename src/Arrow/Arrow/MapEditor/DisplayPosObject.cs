using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Arrow
{
    public class DisplayPosObject : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Game game;
        private ModelManager modelmanager;
        private int ieme_model;
        private SpriteBatch spriteBatch;
        private SpriteFont spriteFont;
        private string fbx_name;
        private string fbx_x;
        private string fbx_y;
        private string fbx_z;
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
            this.modelmanager = new ModelManager(game);
            this.spriteBatch = new SpriteBatch(this.Game.GraphicsDevice);
            base.Initialize();
        }

        protected override void LoadContent()
        { 
            this.spriteFont = this.Game.Content.Load<SpriteFont>("Fonts/FPS");
            base.LoadContent();
        }

        public void AssociateModel(ModelManager mod)
        {
            this.modelmanager = mod;
        }

        public override void Update(GameTime gameTime)
        {
            modelmanager.Models.ElementAt(ieme_model).Value.position.Decompose(out scale, out rotation, out translation);
            this.fbx_name = modelmanager.Models.ElementAt(ieme_model).Key;
            this.fbx_x = translation.X.ToString();
            this.fbx_y = translation.Y.ToString();
            this.fbx_z = translation.Z.ToString();
            base.Update(gameTime);
        }

        public void Upieme(int ieme)
        {
            this.ieme_model = ieme;
        }

        public override void Draw(GameTime gameTime)
        {
            string str = fbx_name + "\n" + "X: " + fbx_x + "\n" + "Y: " + fbx_y + "\n" + "Z: " + fbx_z;
            Vector2 size = this.spriteFont.MeasureString(str);

            this.spriteBatch.Begin();
            this.spriteBatch.DrawString(this.spriteFont, str, 
                new Vector2(this.GraphicsDevice.Viewport.Width - size.X - 5,this.GraphicsDevice.Viewport.Height - size.Y - 5), Color.Gold);
            this.spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
