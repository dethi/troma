using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameEngine
{
    public static class LightInfo
    {
        public static Vector3 LightPosition;
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

        public static Matrix LightViewMatrix
        {
            get 
            { 
                return Matrix.CreateLookAt(LightPosition, 
                    LightPosition * LightDirection, Vector3.Up); 
            }
        }

        public static Matrix LightProjectionMatrix
        {
            get
            {
                return Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver2, 1f, 5f, 100f);
            }
        }

        public static Matrix LightViewProjectionMatrix
        {
            get { return LightViewMatrix * LightProjectionMatrix; }
        }

        public static Vector4 DiffuseColor;
        public static float DiffuseIntensity;

        public static Vector4 AmbientColor;
        public static float AmbientIntensity;

        public static void Initialize()
        {
            LightPosition = new Vector3(250, 50, 250);
            LightDirection = new Vector3(2, 1, -2);
            DiffuseColor = Color.White.ToVector4();
            DiffuseIntensity = 0.18f;

            AmbientColor = Color.LightYellow.ToVector4();
            AmbientIntensity = 1f;
        }
    }
}
