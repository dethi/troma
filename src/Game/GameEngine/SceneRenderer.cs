using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameEngine
{
    public static class SceneRenderer
    {
        public static Color BackgroundColor = Color.Black;

        public static CloudManager InitializeSky(SkyType skyType, 
            TerrainInfo terrainInfo, ICamera camera)
        {
            LightInfo.Initialize();

            switch (skyType)
            {
                case SkyType.CloudField:
                    LightInfo.AmbientIntensity = 0.4f;
                    BackgroundColor = Color.DarkGray;
                    break;
                default:
                    BackgroundColor = Color.SkyBlue;
                    break;
            }

            return new CloudManager(skyType, terrainInfo, camera);
        }

        public static void InitializeMenu()
        {
            BackgroundColor = Color.Black;
        }
    }
}
