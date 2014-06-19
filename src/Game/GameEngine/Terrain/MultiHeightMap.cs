using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine
{
    public class MultiHeightMap : ITerrain
    {
        #region Fields
        HeightMap[,] heightTab;
        TerrainInfo terrainInfo;
        int squareOf;
        Size oneMap;
        #endregion Fields

        public TerrainInfo Info
        {
            get { return terrainInfo; }
        }

        public MultiHeightMap(Game game, Effect effect, TerrainInfo terrainInfo, string mapName, int squareOf)
        {
            this.squareOf = squareOf;
            this.terrainInfo = terrainInfo;
            heightTab = new HeightMap[squareOf, squareOf];

            oneMap = new Size(
                terrainInfo.Size.Width / squareOf,
                terrainInfo.Size.Height / squareOf);

            for (int x = 0; x < squareOf; x++)
            {
                for (int y = 0; y < squareOf; y++)
                {
                    TerrainInfo info = new TerrainInfo(
                        new Vector3(x * (oneMap.Width - 1), 0, y * (oneMap.Height - 1)),
                        oneMap,
                        terrainInfo.Depth,
                        FileManager.Load<Texture2D>(String.Format("Terrains/{0}_texture{1}{2}", mapName, x, y)),
                        terrainInfo.TextureScale,
                        FileManager.Load<Texture2D>(String.Format("Terrains/{0}_heightmap{1}{2}", mapName, x, y)));

                    heightTab[x, y] = new HeightMap(game, effect.Clone(), info);
                }
            }
        }


        public float GetY(Vector3 pos)
        {
            int mapX = (int)(pos.X / (oneMap.Width - 1));
            int mapY = (int)(pos.Z / (oneMap.Height - 1));

            return heightTab[mapX, mapY].GetY(pos);
        }

        public bool IsOnTerrain(Vector3 pos)
        {
            int mapX = (int)(pos.X / (oneMap.Width - 1));
            int mapY = (int)(pos.Z / (oneMap.Height - 1));

            return ((mapX < squareOf && mapY < squareOf) &&
                heightTab[mapX, mapY].IsOnTerrain(pos));
        }

        public void Draw(ICamera camera)
        {
            for (int x = 0; x < squareOf; x++)
            {
                for (int y = 0; y < squareOf; y++)
                {
                    heightTab[x, y].Draw(camera);
                }
            }
        }
    }
}
