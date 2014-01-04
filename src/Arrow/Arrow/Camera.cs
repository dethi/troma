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
    public class Camera : Microsoft.Xna.Framework.GameComponent
    {
        #region Attributes
        private Vector3 cameraPosition;
        private Vector3 cameraRotation;
        private float cameraSpeed;
        private Vector3 cameraLookAt;
        private Vector3 mouseRotationBuffer;
        private MouseState currentMouseState;
        private Vector2 originMouse;
        #endregion

        #region Properties
        public Vector3 Position
        {
            get { return cameraPosition; }
            set
            {
                cameraPosition = value;
                UpdateLookAt();
            }
        }

        public Vector3 Rotation
        {
            get { return cameraRotation; }
            set
            {
                cameraRotation = value;
                UpdateLookAt();
            }
        }

        public Matrix Projection { get; private set; }

        public Matrix View
        {
            get
            {
                return Matrix.CreateLookAt(cameraPosition, cameraLookAt, Vector3.Up);
            }
        }
        #endregion

        public Camera(Game game) : this(game, Vector3.Zero, Vector3.Zero, 40f) { }

        public Camera(Game game, Vector3 position) : this(game, position, Vector3.Zero, 40f) { }

        public Camera(Game game, Vector3 position, Vector3 rotation, float speed)
            : base(game)
        {
            cameraSpeed = speed;

            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                Game.GraphicsDevice.Viewport.AspectRatio, 0.05f, 1000.0f);

            // Initialise la position et la rotation de la camera
            MoveTo(position, rotation);

            int centerX = game.GraphicsDevice.Viewport.Width / 2;
            int centerY = game.GraphicsDevice.Viewport.Height / 2;
            Mouse.SetPosition(centerX, centerY);
            originMouse = new Vector2(centerX, centerY);
        }

        public override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            currentMouseState = Mouse.GetState();

            //
            // Mouvement par le clavier
            //
            #region Clavier
            Vector3 moveVector = Vector3.Zero;

            if (Keyboard.GetState().IsKeyDown(Keys.W))
                moveVector.Z += 1;
            if (Keyboard.GetState().IsKeyDown(Keys.S))
                moveVector.Z += -1;
            if (Keyboard.GetState().IsKeyDown(Keys.A))
                moveVector.X += 1;
            if (Keyboard.GetState().IsKeyDown(Keys.D))
                moveVector.X += -1;

            if (moveVector != Vector3.Zero)
            {
                moveVector.Normalize();
                moveVector *= dt * cameraSpeed;
            }
            #endregion

            //
            // Mouvement par la souris
            //
            #region Souris
            if (currentMouseState.X != originMouse.X || currentMouseState.Y != originMouse.Y)
            {
                float deltaX = currentMouseState.X - originMouse.X;
                float deltaY = currentMouseState.Y - originMouse.Y;

                mouseRotationBuffer.X -= 0.05f * deltaX * dt;
                mouseRotationBuffer.Y -= 0.05f * deltaY * dt;

                if (mouseRotationBuffer.Y < MathHelper.ToRadians(-75.0f))
                {
                    mouseRotationBuffer.Y = mouseRotationBuffer.Y - (mouseRotationBuffer.Y -
                        MathHelper.ToRadians(-75.0f));
                }
                if (mouseRotationBuffer.Y > MathHelper.ToRadians(75.0f))
                {
                    mouseRotationBuffer.Y = mouseRotationBuffer.Y - (mouseRotationBuffer.Y -
                        MathHelper.ToRadians(75.0f));
                }

                Rotation = new Vector3(-MathHelper.Clamp(mouseRotationBuffer.Y,
                    MathHelper.ToRadians(-75.0f), MathHelper.ToRadians(75.0f)),
                    MathHelper.WrapAngle(mouseRotationBuffer.X), 0);

                Mouse.SetPosition((int)originMouse.X, (int)originMouse.Y);
            }
            #endregion

            // Effectue le mouvement (clavier + souris)
            Move(moveVector);

            base.Update(gameTime);
        }

        // Configure la position et la rotation de la camera
        private void MoveTo(Vector3 pos, Vector3 rot)
        {
            Position = pos;
            Rotation = rot;
        }

        // Met a jour le vecteur cameraLookAt
        private void UpdateLookAt()
        {
            Matrix rotationMatrix = Matrix.CreateRotationX(cameraRotation.X) *
                Matrix.CreateRotationY(cameraRotation.Y);
            Vector3 lookAtOffset = Vector3.Transform(Vector3.UnitZ, rotationMatrix);
            cameraLookAt = cameraPosition + lookAtOffset;
        }

        // Simule le mouvement (permet d'obtenir la nouvelle position de la camera)
        private Vector3 PreviewMove(Vector3 amount)
        {
            Matrix rotate = Matrix.CreateRotationY(cameraRotation.Y);
            Vector3 movement = new Vector3(amount.X, amount.Y, amount.Z);
            movement = Vector3.Transform(movement, rotate);
            return cameraPosition + movement;
        }

        // Deplace la camera
        private void Move(Vector3 scale)
        {
            MoveTo(PreviewMove(scale), Rotation);
        }
    }
}
