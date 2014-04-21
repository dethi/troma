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

        /// <summary>
        /// Manage a set of terrain
        /// </summary>
        /// <param name="heightMapT">Heighmap texture</param>
        /// <param name="terrainT">Terrain texture</param>
        /// <param name="textureScale">Size of terrain texture</param>
        /// <param name="terrainWidth">Terrain width (X)</param>
        /// <param name="terrainHeight">Terrain height (Z)</param>
        /// <param name="heightScale">Max terrain depth (Y)</param>
        /// <param name="nbMapX">Number of map (X)</param>
        /// <param name="nbMapZ">Number of map (Z)</param>
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

        /// <summary>
        /// Return the instance of the map that contains the (X,Z) position
        /// </summary>
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

        /// <summary>
        /// Search the Y position of a terrain point
        /// </summary>
        public float? GetHeight(float x, float z)
        {
            return GetMap(x, z).GetHeight(
                (x % terrainWidth), 
                (z % terrainHeight));
        }
    }
}