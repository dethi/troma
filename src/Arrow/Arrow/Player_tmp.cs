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
    // Controls of the player
    //
    public class Player_tmp : Microsoft.Xna.Framework.GameComponent
    {
        private Game game;

        public Matrix view { get; private set; }
        public Matrix projection { get; private set; }
        public Matrix world { get; private set; }

        private Vector3 cameraPosition = new Vector3(0f, 2f, 4.0f);
        private Vector3 cameraTarget = new Vector3(0, 2f, 0);
        private Vector3 cameraUp = Vector3.Up;

        public Player_tmp(Game game)
            : base(game)
        {
            this.game = game;
        }

        public override void Initialize()
        {
            this.view = Matrix.CreateLookAt(this.cameraPosition, cameraTarget, cameraUp);
            this.projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                Game.GraphicsDevice.Viewport.AspectRatio, 0.01f, 10000.0f);
            this.world = Matrix.Identity;

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            // Fire
            if (GamePad.GetState(PlayerIndex.One).Triggers.Right == 1)
                GamePad.SetVibration(PlayerIndex.One, 0.7f, 0.7f);
            else if (GamePad.GetState(PlayerIndex.One).Triggers.Right < 0.7)
                GamePad.SetVibration(PlayerIndex.One, 0, 0);

            // Control camera
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

            base.Update(gameTime);
        }
    }
}
