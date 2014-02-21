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

        public FBX(Game game, string nameModel, int x, int y, int z) : this(game, nameModel, Matrix.Identity, x ,y, z) { }

        /*Constructeur FBX positionne le model "nameModel" a la position (x,y,z)*/
        public FBX(Game game, string nameModel, Matrix position, int x, int y, int z)
            : base(game)
        {
            this.game = game;
            this.nameModel = nameModel;
            this.position = position * Matrix.CreateTranslation(x, y, z); // Modifie la matrice identity et affecte la position (x,y,z);

            textureEnabled = true;
            //lightingEnabled = true;

            model = game.Content.Load<Model>("Models/" + nameModel);
        }

        public override void Draw(GameTime gameTime)
        {
            game.ResetGraphicsDeviceFor3D();

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