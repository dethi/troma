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
    public sealed class Camera
    {
        #region Singleton
        private static volatile Camera _instance;
        private static object _syncRoot = new Object();

        public static Camera Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null)
                            _instance = new Camera();
                    }
                }

                return _instance;
            }
        }

        private Camera() { }
        #endregion

        #region Attributes

        private Vector3 cameraPosition;
        private Vector3 cameraRotation;
        private Vector3 cameraLookAt;

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

        public Vector3 LookAt
        {
            get { return cameraLookAt; }
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

        /// <summary>
        /// Create new camera
        /// </summary>
        public void New(Game game, Vector3 position, Vector3 rotation)
        {
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                game.GraphicsDevice.Viewport.AspectRatio, 0.05f, 1000.0f);

            // Initialise la position et la rotation de la camera
            MoveTo(position, rotation);
        }

        /// <summary>
        /// Change camera position and rotation
        /// </summary>
        /// <param name="pos">Position</param>
        /// <param name="rot">Rotation</param>
        private void MoveTo(Vector3 pos, Vector3 rot)
        {
            Position = pos;
            Rotation = rot;
        }

        /// <summary>
        /// Update look at vector
        /// </summary>
        private void UpdateLookAt()
        {
            Matrix rotationMatrix = Matrix.CreateRotationX(cameraRotation.X) *
                Matrix.CreateRotationY(cameraRotation.Y);
            Vector3 lookAtOffset = Vector3.Transform(Vector3.UnitZ, rotationMatrix);
            cameraLookAt = cameraPosition + lookAtOffset;
        }

        /// <summary>
        /// Calculates the new position of the camera
        /// </summary>
        /// <param name="amount">Direction and scale of the movement</param>
        public Vector3 PreviewMove(Vector3 amount)
        {
            Matrix rotate = Matrix.CreateRotationY(cameraRotation.Y);
            Vector3 movement = new Vector3(amount.X, amount.Y, amount.Z);
            movement = Vector3.Transform(movement, rotate);
            return cameraPosition + movement;
        }

        /// <summary>
        /// Move camera
        /// </summary>
        /// <param name="scale">Direction and scale of the movement</param>
        public void Move(Vector3 scale)
        {
            MoveTo(PreviewMove(scale), Rotation);
        }
    }
}
