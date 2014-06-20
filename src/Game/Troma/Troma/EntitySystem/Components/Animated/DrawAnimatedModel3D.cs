using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameEngine;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Troma
{
    class DrawAnimatedModel3D : DrawableEntityComponent
    {
        public DrawAnimatedModel3D(Entity aParent)
            : base(aParent)
        {
            Name = "DrawAnimatedModel3D";
            _requiredComponents.Add("AnimatedModel3D");
        }

        public override void Start()
        {
            base.Start();
        }

        public override void Draw(GameTime gameTime, ICamera camera)
        {
            Matrix world = Entity.GetComponent<Transform>().World;
            Model model = Entity.GetComponent<AnimatedModel3D>().Model;
            Matrix[] bones = Entity.GetComponent<AnimatedModel3D>().animationPlayer.GetSkinTransforms();

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (SkinnedEffect effect in mesh.Effects)
                {
                    effect.SetBoneTransforms(bones);
                    effect.View = camera.View;
                    effect.Projection = camera.Projection;

                    effect.EnableDefaultLighting();
                    effect.SpecularColor = new Vector3(0.25f);
                    effect.SpecularPower = 16;
                }

                mesh.Draw();
            }
        }
    }
}
