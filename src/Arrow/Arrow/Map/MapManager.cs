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
        private HeightMap[,] mapTab;
        private int nbMapX;
        private int nbMapY;
        private int terrainWidth;
        private int terrainHeight;

        public MapManager(Game game, string heightMapT, string terrainT,
            float textureScale, int terrainWidth, int terrainHeight, float heightScale, int nbMapX, int nbMapY)
        {
            this.nbMapX = nbMapX;
            this.nbMapY = nbMapY;
            this.terrainWidth = terrainWidth;
            this.terrainHeight = terrainHeight;
            this.mapTab = new HeightMap[nbMapX, nbMapY];

            for (int x = 0; x < nbMapX; x++)
            {
                for (int y = 0; y < nbMapY; y++)
                {
                    mapTab[x, y] = new HeightMap(game, heightMapT + x, terrainT + y, textureScale, terrainWidth, terrainHeight, heightScale, x, y);
                }
            }
        }

        public HeightMap GetMap(Vector3 pos)
        {
            int x = (int)pos.X / terrainWidth;
            int y = (int)pos.Z / terrainHeight;

            if (x > nbMapX - 1)
                x = nbMapX - 1;
            if (y > nbMapY - 1)
                y = nbMapY - 1;
            if (x < 0)
                x = 0;
            if (y < 0)
                y = 0;

            return mapTab[x, y];
        }

        public void Draw(Effect effect)
        {
            for (int x = 0; x < nbMapX; x++)
            {
                for (int y = 0; y < nbMapY; y++)
                {
                    mapTab[x, y].Draw(effect);
                }
            }
        }
    }
}