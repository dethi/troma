using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine
{
    public struct TerrainInfo
    {
        public Vector3 Position;
        public Size Size;
        public float Depth;

        public Texture2D Texture;
        public float TextureScale;
        public Texture2D Heighmap;

        public TerrainInfo(Vector3 Position, Size Size, float Depth, 
            Texture2D Texture, float TextureScale, Texture2D Heighmap)
        {
            this.Position = Position;
            this.Size = Size;
            this.Depth = Depth;

            this.Texture = Texture;
            this.TextureScale = TextureScale;
            this.Heighmap = Heighmap;
        }
    }
}
