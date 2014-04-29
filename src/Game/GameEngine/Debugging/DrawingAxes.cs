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
            new VertexPositionColor(new Vector3(0.005f, 0, 0), Color.Red),
            new VertexPositionColor(new Vector3(0, 0, 0), Color.Green),
            new VertexPositionColor(new Vector3(0, 0.005f, 0), Color.Green),
            new VertexPositionColor(new Vector3(0, 0, 0), Color.Blue),
            new VertexPositionColor(new Vector3(0, 0, 0.005f), Color.Blue)
        };

        private static BasicEffect _effect;
        private static Texture2D _pixel;

        public static void Initialize()
        {
            _effect = new BasicEffect(GameServices.GraphicsDevice);
            _effect.VertexColorEnabled = true;

            _pixel = new Texture2D(GameServices.GraphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });
        }

        /// <summary>
        /// Drawing 3D axes
        /// </summary>
        public static void Draw(GameTime gameTime, ICamera camera)
        {
            GameServices.SpriteBatch.Begin();
            GameServices.SpriteBatch.Draw(_pixel, new Rectangle(0, 0, 100, 100), Color.White * 0.6f);
            GameServices.SpriteBatch.End();

            GameServices.ResetGraphicsDeviceFor3D();

            Vector3 pos = GameServices.GraphicsDevice.Viewport.Unproject(
                new Vector3(50, 60, 0.4f), camera.Projection, camera.View, Matrix.Identity);

            _effect.World = Matrix.CreateTranslation(pos);
            _effect.View = camera.View;
            _effect.Projection = camera.Projection;

            foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GameServices.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                    PrimitiveType.LineList, lines, 0, 3);
            }
        }
    }
}
