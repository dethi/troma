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
    //
    // Include all your test here instead in class Game
    // This class have the same structure as Game (Properties and Methods)
    //
    class Test
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private ContentManager Content;
        GraphicsDevice GraphicsDevice;

        // TODO: Add your properties here
        #region Properties
        // Here
        Model model;

        Matrix view;
        Matrix projection;
        Matrix world;

        Vector3 cameraPosition = new Vector3(0f, 2f, 4.0f);
        Vector3 cameraTarget = new Vector3(0, 2f, 0);
        Vector3 cameraUp = Vector3.Up;
        #endregion

        private void CreateCamera()
        {
            this.view = Matrix.CreateLookAt(this.cameraPosition, cameraTarget, cameraUp);
            this.projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, this.GraphicsDevice.Viewport.AspectRatio, 0.01f, 10000.0f);
            this.world = Matrix.Identity;
        }

        // Don't modify this methods
        public Test(GraphicsDevice GraphicsDevice, GraphicsDeviceManager graphics, ContentManager Content)
        {
            this.GraphicsDevice = GraphicsDevice;
            this.Content = Content;
            this.graphics = graphics;
        }

        public void Initialize()
        {
            // TODO: Add your initialization logic here
            CreateCamera();
        }

        public void LoadContent(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;

            // TODO: use this.Content to load your game content here
            this.model = Content.Load<Model>("map");
        }

        public void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here

            // Fire
            if (GamePad.GetState(PlayerIndex.One).Triggers.Right == 1)
                GamePad.SetVibration(PlayerIndex.One, 0.7f, 0.7f);
            else if (GamePad.GetState(PlayerIndex.One).Triggers.Right < 0.7)
                GamePad.SetVibration(PlayerIndex.One, 0, 0);
            
            // Camera
            // TODO: correct axies
            if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X != 0)
            {
                this.world *= Matrix.CreateRotationZ(MathHelper.ToRadians(-1.1f * 
                    GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X));
            }
            if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Y != 0)
            {
                this.cameraTarget.Y += 0.1f * GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Y;
                this.view = Matrix.CreateLookAt(this.cameraPosition, this.cameraTarget, this.cameraUp);
            }
        }

        public void Draw(GameTime gameTime)
        {
            // TODO: Add your drawing code here
            foreach (ModelMesh mesh in this.model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = this.world * Matrix.CreateRotationX(MathHelper.ToRadians(90));
                    effect.View = this.view;
                    effect.Projection = this.projection;

                    effect.TextureEnabled = true;
                    effect.LightingEnabled = true;
                }

                mesh.Draw();
            }
        }
    }
}
