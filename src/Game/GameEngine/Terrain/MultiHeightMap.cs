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
        int squareOf;
        #endregion Fields

        public TerrainInfo Info
        {
            get { return new TerrainInfo(); }
        }

        public MultiHeightMap(Game game, Effect effect, TerrainInfo terrainInfo, string mapName, int squareOf)
        {
            this.squareOf = squareOf;
            heightTab = new HeightMap[squareOf, squareOf];
            for (int x = 0; x < squareOf; x++)
            {
                for (int y = 0; y < squareOf; y++)
                {
                    TerrainInfo info = new TerrainInfo(new Vector3(x * terrainInfo.Size.Width, 0,
                                                                   y * terrainInfo.Size.Height),
                                                                   terrainInfo.Size, terrainInfo.Depth,
                                                                   FileManager.Load<Texture2D>("Terrains/" + mapName +"_texture" + x + y),
                                                                   terrainInfo.TextureScale,
                                                                   FileManager.Load<Texture2D>("Terrains/" + mapName + "_heightmap" + x + y));
                    heightTab[x, y] = new HeightMap(game, effect, info);
                }
            }
        }


        public float GetY(Vector3 pos)
        {
            int mapX = (int)pos.X % squareOf;
            int mapY = (int)pos.Z % squareOf;

            return heightTab[mapX, mapY].GetY(pos);
        }

        public bool IsOnTerrain(Vector3 pos)
        {
            int mapX = (int)pos.X % squareOf;
            int mapY = (int)pos.Z % squareOf;

            return heightTab[mapX, mapY].IsOnTerrain(pos);
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
