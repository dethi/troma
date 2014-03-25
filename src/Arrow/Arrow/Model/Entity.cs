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

        public string entityName { get; private set; }
        private Model model;
        public Matrix position;

        public bool lightingEnabled;

        #region Constructor

        public Entity(Game game, string entityName)
            : this(game, entityName, Vector2.Zero) { }

        public Entity(Game game, string entityName, Vector2 pos)
            : this(game, entityName, new Vector3(pos.X, game.map.GetHeight(pos.X, pos.Y), pos.Y)) { }

        public Entity(Game game, string entityName, Vector3 pos)
        {
            this.game = game;
            this.camera = Camera.Instance;

            this.entityName = entityName;
            this.position = Matrix.CreateTranslation(pos);

            this.lightingEnabled = true;

            model = game.Content.Load<Model>("Models/" + entityName);
        }

        #endregion

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