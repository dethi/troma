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
        //public bool lightingEnabled;

        public FBX(Game game, string nameModel) : this(game, nameModel, Matrix.Identity) { }

        public FBX(Game game, string nameModel, Matrix position)
            : base(game)
        {
            this.game = game;
            this.nameModel = nameModel;
            this.position = position;

            textureEnabled = true;
            //lightingEnabled = true;

            model = game.Content.Load<Model>("Models/" + nameModel);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.TextureEnabled = textureEnabled;
                    //effect.LightingEnabled = lightingEnabled;
                    effect.EnableDefaultLighting();

                    effect.World = position;
                    effect.Projection = Camera.Instance.Projection;
                    effect.View = Camera.Instance.View;
                }
                mesh.Draw();
            }

            base.Draw(gameTime);
        }
    }
}