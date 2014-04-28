using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameEngine
{
    public static class RenderState
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

        public static Vector4 LightColor;
        public static float LightBrightness;

        public static Vector4 AmbientLightColor;
        public static float AmbientLightLevel;

        public static void Initialize()
        {
            LightDirection = new Vector3(-1f, 1f, -1f);
            LightColor = new Vector4(1, 1, 1, 1);
            LightBrightness = 0.8f;

            AmbientLightColor = new Vector4(0.98f, 0.92f, 0.24f, 1);
            AmbientLightLevel = 0.23f;
        }
    }
}
