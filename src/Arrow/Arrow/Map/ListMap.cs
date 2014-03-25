using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Arrow
{
    public class ListMap
    {
        private HeightMap[,] listmap;
        private int HeightMapW;
        private int HeightMapH;
        private int HeightMapN;

        public ListMap(Game game, Texture2D heightMap, Texture2D terrainTexture,
            float textureScale, int terrainWidth, int terrainHeight, float heightScale, int HeightMapN)
        {
            this.HeightMapW = terrainWidth;
            this.HeightMapH = terrainHeight;
            this.HeightMapN = HeightMapN;

            listmap = new HeightMap[HeightMapW, HeightMapH];

            for (int x = 0; x < HeightMapN; x++)
            {
                for (int y = 0; y < HeightMapN; y++)
                {
                    listmap[x, y] = new HeightMap(game, heightMap, terrainTexture, textureScale, terrainWidth, terrainHeight, heightScale, (x * HeightMapW), (y * HeightMapH));
                }   
            }
        }

        public void Draw(Effect effect, Vector3 playerPos)
        {
            int posOnMapX = (int)playerPos.X / HeightMapW;
            int posOnMapY = (int)playerPos.Z / HeightMapH;

            for (int x = posOnMapX - 1; x < posOnMapX + 1; x++)
            {
                for (int y = posOnMapY - 1; y < posOnMapY + 1; y++)
                {
                    if (x >= 0 && x <= HeightMapW)
                    {
                        if (y >= 0 && y <= HeightMapH)
                        {
                            listmap[x, y].Draw(effect);
                        }
                    }
                }
            }
        }

        public HeightMap GetHeightMap(int x, int y)
        {
            return listmap[x, y];
        }

        public Vector2 PosToMap(Vector3 pos)
        {
            Vector2 posOnMapXY = new Vector2((int)pos.X / HeightMapW, (int)pos.Z / HeightMapH);
            
            return posOnMapXY;
        }

    }
}
