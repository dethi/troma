using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GraphicsEngine.Terrain
{
    public struct TerrainInfo
    {
        public Vector3 Position;
        public Size Size;
        public float Depth;

        public Texture2D Texture;
        public float TextureScale;
        public Texture2D Heighmap;
    }
}
