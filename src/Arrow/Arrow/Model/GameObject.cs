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
    public class GameObject
    {
        private Game game;
        private Camera camera;

        private string nameModel;
        private Model model;
        public Matrix position;

        public GameObject(Game game, string nameModel, Vector3 pos)
        {
            this.game = game;
            this.camera = Camera.Instance;

            this.nameModel = nameModel;

            // Modifie la matrice identity et affecte la position (x,y,z);
            this.position = Matrix.CreateTranslation(pos);

            model = game.Content.Load<Model>("Models/" + nameModel);
        }

        public void Draw(GameTime gameTime)
        {
            game.ResetGraphicsDeviceFor3D();

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    effect.World = position;
                    effect.Projection = camera.Projection;
                    effect.View = camera.View;
                }

                mesh.Draw();
            }
        }                
    }
}