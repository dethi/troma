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
    public class FBX : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Game game;

        private string nameModel;
        private Model model;
        public Matrix position;

        public bool textureEnabled;
        public bool lightingEnabled;

        public FBX(Game game, string nameModel)
            : base(game)
        {
            this.game = game;
            this.nameModel = nameModel;
        }

        public override void Initialize()
        {
            this.textureEnabled = true;
            this.lightingEnabled = true;
            this.position = Matrix.Identity;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.model = game.Content.Load<Model>("Models/" + nameModel);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (ModelMesh mesh in this.model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = this.position;
                    effect.View = game.player.view;
                    effect.Projection = game.player.projection;

                    effect.TextureEnabled = this.textureEnabled;
                    effect.LightingEnabled = this.lightingEnabled;
                }

                mesh.Draw();
            }

            base.Draw(gameTime);
        }
    }
}
