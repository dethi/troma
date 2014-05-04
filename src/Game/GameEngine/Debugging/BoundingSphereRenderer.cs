using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine
{
#if DEBUG
    /// <summary>
    /// Provides a set of methods for rendering BoundingSpheres.
    /// </summary>
    public static class BoundingSphereRenderer
    {
        private static VertexBuffer _vertBuffer;
        private static BasicEffect _effect;
        private static int _sphereResolution;

        /// <summary>
        /// Initializes
        /// </summary>
        public static void Initialize(int sphereResolution)
        {
            _sphereResolution = sphereResolution;

            _effect = new BasicEffect(GameServices.GraphicsDevice);
            _effect.LightingEnabled = false;
            _effect.VertexColorEnabled = false;

            VertexPositionColor[] verts = new VertexPositionColor[(sphereResolution + 1) * 3];

            int index = 0;
            float step = MathHelper.TwoPi / (float)sphereResolution;

            //create the loop on the XY plane first
            for (float a = 0; a <= MathHelper.TwoPi; a += step)
            {
                verts[index++] = new VertexPositionColor(
                    new Vector3((float)Math.Cos(a), (float)Math.Sin(a), 0f),
                    Color.White);
            }

            //next on the XZ plane
            for (float a = 0; a <= MathHelper.TwoPi; a += step)
            {
                verts[index++] = new VertexPositionColor(
                    new Vector3((float)Math.Cos(a), 0f, (float)Math.Sin(a)),
                    Color.White);
            }

            //finally on the YZ plane
            for (float a = 0; a <= MathHelper.TwoPi; a += step)
            {
                verts[index++] = new VertexPositionColor(
                    new Vector3(0f, (float)Math.Cos(a), (float)Math.Sin(a)),
                    Color.White);
            }

            _vertBuffer = new VertexBuffer(
                GameServices.GraphicsDevice, typeof(VertexPositionColor),
                verts.Length, BufferUsage.None);
            _vertBuffer.SetData(verts);
        }

        /// <summary>
        /// Renders a bounding sphere using different colors for each axis.
        /// </summary>
        /// <param name="sphere">The sphere to render.</param>
        /// <param name="xyColor">The color for the XY circle.</param>
        /// <param name="xzColor">The color for the XZ circle.</param>
        /// <param name="yzColor">The color for the YZ circle.</param>
        public static void Render(BoundingSphere sphere, ICamera camera,
            Color xyColor, Color xzColor, Color yzColor)
        {
            if (DebugConfig.DisplayBox)
            {
                if (_vertBuffer == null)
                    Initialize(30);

                GameServices.GraphicsDevice.SetVertexBuffer(_vertBuffer);

                _effect.World =
                    Matrix.CreateScale(sphere.Radius) *
                    Matrix.CreateTranslation(sphere.Center);
                _effect.View = camera.View;
                _effect.Projection = camera.Projection;

                foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
                {
                    _effect.DiffuseColor = xyColor.ToVector3();
                    pass.Apply();
                    GameServices.GraphicsDevice.DrawPrimitives(
                          PrimitiveType.LineStrip,
                          0,
                          _sphereResolution);

                    _effect.DiffuseColor = xzColor.ToVector3();
                    pass.Apply();
                    GameServices.GraphicsDevice.DrawPrimitives(
                          PrimitiveType.LineStrip,
                          _sphereResolution + 1,
                          _sphereResolution);

                    _effect.DiffuseColor = yzColor.ToVector3();
                    pass.Apply();
                    GameServices.GraphicsDevice.DrawPrimitives(
                          PrimitiveType.LineStrip,
                          (_sphereResolution + 1) * 2,
                          _sphereResolution);
                }
            }
        }

        public static void Render(BoundingSphere[] spheres, ICamera camera,
            Color xyColor, Color xzColor, Color yzColor)
        {
            foreach (BoundingSphere sphere in spheres)
                Render(sphere, camera, xyColor, xzColor, yzColor);
        }

        /// <summary>
        /// Renders a bounding sphere using a single color for all three axis.
        /// </summary>
        /// <param name="sphere">The sphere to render.</param>
        /// <param name="color">The color to use for rendering the circles.</param>
        public static void Render(BoundingSphere sphere, ICamera camera, Color color)
        {
            if (DebugConfig.DisplayBox)
            {
                if (_vertBuffer == null)
                    Initialize(30);

                GameServices.GraphicsDevice.SetVertexBuffer(_vertBuffer);

                _effect.World =
                      Matrix.CreateScale(sphere.Radius) *
                      Matrix.CreateTranslation(sphere.Center);
                _effect.View = camera.View;
                _effect.Projection = camera.Projection;
                _effect.DiffuseColor = color.ToVector3();

                foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
                {
                    pass.Apply();

                    GameServices.GraphicsDevice.DrawPrimitives(
                          PrimitiveType.LineStrip,
                          0,
                          _sphereResolution);

                    GameServices.GraphicsDevice.DrawPrimitives(
                          PrimitiveType.LineStrip,
                          _sphereResolution + 1,
                          _sphereResolution);

                    GameServices.GraphicsDevice.DrawPrimitives(
                          PrimitiveType.LineStrip,
                          (_sphereResolution + 1) * 2,
                          _sphereResolution);
                }
            }
        }

        public static void Render(BoundingSphere[] spheres, ICamera camera, Color color)
        {
            foreach (BoundingSphere sphere in spheres)
                Render(sphere, camera, color);
        }
    }
#endif
}
