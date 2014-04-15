using Microsoft.Xna.Framework;

namespace GraphicsEngine.Camera
{
    interface ICamera
    {
        public Vector3 Position
        {
            get;
            set;
        }

        public Vector3 Rotation
        {
            get;
            set;
        }

        public Vector3 LookAt
        {
            get;
        }

        public Matrix Projection
        {
            get;
        }

        public Matrix View
        {
            get;
        }
    }
}
