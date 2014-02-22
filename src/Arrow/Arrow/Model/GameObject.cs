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

        public string modelName { get; private set; }
        private Model model;
        public Matrix position;

        #region Constructor

        public GameObject(Game game, string modelName) 
            : this(game, modelName, Vector2.Zero) { }

        public GameObject(Game game, string modelName, Vector2 pos)
            : this(game, modelName, new Vector3(pos.X, game.map.GetHeight(pos.X, pos.Y), pos.Y)) { }

        public GameObject(Game game, string modelName, Vector3 pos)
        {
            this.game = game;
            this.camera = Camera.Instance;

            this.modelName = modelName;
            this.position = Matrix.CreateTranslation(pos);

            model = game.Content.Load<Model>("Models/" + modelName);
        }

        #endregion

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