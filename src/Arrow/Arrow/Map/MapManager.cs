using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Arrow
{
    public class MapManager
    {
        private HeightMap[,] maps;
        private int nbMapX;
        private int nbMapZ;
        private int terrainWidth;
        private int terrainHeight;

        public MapManager(Game game, string heightMapT, string terrainT,
            float textureScale, int terrainWidth, int terrainHeight, 
            float heightScale, int nbMapX, int nbMapZ)
        {
            this.nbMapX = nbMapX;
            this.nbMapZ = nbMapZ;
            this.terrainWidth = terrainWidth;
            this.terrainHeight = terrainHeight;
            this.maps = new HeightMap[nbMapX, nbMapZ];

            for (int x = 0; x < nbMapX; x++)
            {
                for (int z = 0; z < nbMapZ; z++)
                {
                    maps[x, z] = new HeightMap(game,
                        heightMapT + "(" + x + "," + z + ")",
                        terrainT + "(" + x + "," + z + ")", 
                        textureScale, 
                        terrainWidth, 
                        terrainHeight, 
                        heightScale, 
                        x, 
                        z);
                }
            }
        }

        public void Draw(Effect effect)
        {
            for (int x = 0; x < nbMapX; x++)
            {
                for (int z = 0; z < nbMapZ; z++)
                    maps[x, z].Draw(effect);
            }
        }

        public HeightMap GetMap(float x, float z)
        {
            int map_x = (int)x / terrainWidth;
            int map_z = (int)z / terrainHeight;

            if (map_x >= nbMapX)
                map_x = nbMapX - 1;
            else if (map_x < 0)
                map_x = 0;

            if (map_z >= nbMapZ)
                map_z = nbMapZ - 1;
            else if (z < 0)
                map_z = 0;

            return maps[map_x, map_z];
        }

        public float? GetHeight(float x, float z)
        {
            return GetMap(x, z).GetHeight(
                (x % terrainWidth), 
                (z % terrainHeight));
        }
    }
}