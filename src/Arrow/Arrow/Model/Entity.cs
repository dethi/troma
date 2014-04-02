using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arrow
{
    public class Entity
    {
        private Game game;
        private Camera camera;

        private Model model;
        public Matrix position;
        public bool lightingEnabled;

        public Entity(Game game, string entityName, Vector3 pos)
        {
            this.game = game;
            camera = Camera.Instance;
            position = Matrix.CreateTranslation(pos);
            lightingEnabled = true;

            model = game.Content.Load<Model>("Models/" + entityName);
        }

        public Entity(Game game, Model entityModel, Vector3 pos)
        {
            this.game = game;
            camera = Camera.Instance;
            model = entityModel;
            position = Matrix.CreateTranslation(pos);
            lightingEnabled = true;
        }

        public void Draw()
        {
            game.ResetGraphicsDeviceFor3D();

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.LightingEnabled = lightingEnabled;

                    effect.World = position;
                    effect.Projection = camera.Projection;
                    effect.View = camera.View;
                }

                mesh.Draw();
            }
        }                
    }
}