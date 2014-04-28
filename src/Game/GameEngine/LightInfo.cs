using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameEngine
{
    public static class LightInfo
    {
        private static Vector3 _lightDirection;

        public static Vector3 LightDirection
        {
            get { return _lightDirection; }
            set
            {
                _lightDirection = value;
                _lightDirection.Normalize();
            }
        }

        public static Vector4 DiffuseColor;
        public static float DiffuseIntensity;

        public static Vector4 AmbientColor;
        public static float AmbientIntensity;

        public static void Initialize()
        {
            LightDirection = new Vector3(0, 1, -1);
            DiffuseColor = Color.White.ToVector4();
            DiffuseIntensity = 1;

            AmbientColor = Color.LightYellow.ToVector4();
            AmbientIntensity = 0.18f;
        }
    }
}
