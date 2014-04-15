using Microsoft.Xna.Framework;

namespace GraphicsEngine.Camera
{
    public class FirstPersonView : ICamera
    {
        #region Fields

        Vector3 cameraPosition;
        Vector3 cameraRotation;
        Vector3 cameraLookAt;

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

        public FirstPersonView(float aspectRatio)
            : this(aspectRatio, Vector3.Zero, Vector3.Zero) { }

        public FirstPersonView(float aspectRatio, Vector3 position, Vector3 rotation)
        {
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                aspectRatio, 0.05f, 1000.0f);

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
