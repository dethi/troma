using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameEngine
{
    public interface ITerrain
    {
        TerrainInfo Info { get; }

        void Draw(ICamera camera);
        bool IsOnTerrain(Vector3 pos);
        float GetY(Vector3 pos);
    }
}
