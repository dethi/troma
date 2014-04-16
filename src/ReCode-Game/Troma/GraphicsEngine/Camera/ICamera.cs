using Microsoft.Xna.Framework;

namespace GraphicsEngine.Camera
{
    public interface ICamera
    {
        Vector3 Position
        {
            get;
            set;
        }

        Vector3 Rotation
        {
            get;
            set;
        }

        Vector3 LookAt
        {
            get;
        }

        Matrix Projection
        {
            get;
        }

        Matrix View
        {
            get;
        }
    }
}
