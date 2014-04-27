using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine
{
    public static class DrawingAxes
    {
        private static VertexPositionColor[] lines = new VertexPositionColor[6]
        {
            new VertexPositionColor(new Vector3(0, 0, 0), Color.Red),
            new VertexPositionColor(new Vector3(0.1f, 0, 0), Color.Red),
            new VertexPositionColor(new Vector3(0, 0, 0), Color.Green),
            new VertexPositionColor(new Vector3(0, 0.1f, 0), Color.Green),
            new VertexPositionColor(new Vector3(0, 0, 0), Color.Blue),
            new VertexPositionColor(new Vector3(0, 0, 0.1f), Color.Blue)
        };

        /// <summary>
        /// Drawing 3D axes
        /// </summary>
        public static void Draw(ICamera camera)
        {
            BasicEffect effect = new BasicEffect(GameServices.GraphicsDevice);

            effect.World = Matrix.Identity * Matrix.CreateTranslation(camera.LookAt);
            effect.View = camera.View;
            effect.Projection = camera.Projection;
            effect.VertexColorEnabled = true;

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GameServices.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                    PrimitiveType.LineList, lines, 0, 3);
            }
        }
    }
}
