using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameEngine.Camera
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
