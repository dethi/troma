using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine;

namespace Troma
{
    public class DrawCollisionBox : DrawableEntityComponent
    {
        private short[] bBoxIndices = 
        {
            0, 1, 1, 2, 2, 3, 3, 0,
            4, 5, 5, 6, 6, 7, 7, 4,
            0, 4, 1, 5, 2, 6, 3, 7
        };

        private BasicEffect _effect;

        public DrawCollisionBox(Entity aParent)
            : base(aParent)
        {
            Name = "DrawCollisionBox";
            _requiredComponents.Add("CollisionBox");
        }

        public override void Initialize()
        {
            _effect = new BasicEffect(GameServices.GraphicsDevice);
            _effect.World = Matrix.Identity;
            _effect.VertexColorEnabled = true;
        }

        public override void Draw(GameTime gameTime, ICamera camera)
        {
            _effect.View = camera.View;
            _effect.Projection = camera.Projection;

            List<BoundingBox> boxList = Entity.GetComponent<CollisionBox>().BoxList;

            foreach (BoundingBox box in boxList)
            {
                Vector3[] corners = box.GetCorners();
                VertexPositionColor[] primitiveList = new VertexPositionColor[corners.Length];

                // Assign the 8 box vertices
                for (int i = 0; i < corners.Length; i++)
                {
                    primitiveList[i] = new VertexPositionColor(corners[i], Color.Red);
                }

                // Draw the box with a LineList
                foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    GameServices.GraphicsDevice.DrawUserIndexedPrimitives(
                        PrimitiveType.LineList, primitiveList, 0, 8,
                        bBoxIndices, 0, 12);
                }
            }
        }
    }
}
