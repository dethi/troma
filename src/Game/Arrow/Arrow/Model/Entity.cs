using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SkinnedModel;

namespace Arrow
{
    public class Entity
    {
        private Game game;
        private Camera camera;

        private Model model;
        public Matrix position;
        public bool lightingEnabled;

        // field for animated model
        public SkinningData skinningData;
        public AnimationClip clip;
        public string animationName;

        public Entity(Game game, string entityName, Vector3 pos)
        {
            this.game = game;
            camera = Camera.Instance;
            position = Matrix.CreateTranslation(pos);
            lightingEnabled = true;

            model = game.Content.Load<Model>("Models/" + entityName);
        }


        // Constructor for animated model
        public Entity(Game game, string entityName, Vector3 pos, string animationName)
        {
            this.game = game;
            camera = Camera.Instance;
            position = Matrix.CreateTranslation(pos);
            lightingEnabled = true;

            model = game.Content.Load<Model>("Models/" + entityName);
            skinningData = model.Tag as SkinningData;
            this.animationName = animationName;

            if (skinningData == null)
            {
                throw new InvalidOperationException("This model does not contain a SkinningData tag.");
            }

            clip = skinningData.AnimationClips[animationName];
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

        public void DrawAnimation()
        {
            Matrix[] bones = game.animationPlayer.GetSkinTransforms();

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (SkinnedEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.SetBoneTransforms(bones);

                    effect.World = position;
                    effect.View = camera.View;
                    effect.Projection = camera.Projection;
                }

                mesh.Draw();
            }
        }
    }
}