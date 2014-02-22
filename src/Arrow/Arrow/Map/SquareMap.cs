using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arrow
{
    public class SquareMap
    {
        public const int V_SIZE = 4;
        public const int MAP_WIDTH = 20 * V_SIZE;
        public const int MAP_HEIGHT = 20 * V_SIZE;

        GraphicsDevice device;
        VertexBuffer floorBuffer;
        Color[] floorColors = new Color[2] { Color.White, Color.Gray };

        public SquareMap(GraphicsDevice device)
        {
            this.device = device;
            BuildFloorBuffer();
        }

        public void Draw(Camera camera, BasicEffect effect)
        {
            effect.VertexColorEnabled = true;
            effect.World = Matrix.Identity;
            effect.View = camera.View;
            effect.Projection = camera.Projection;

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.SetVertexBuffer(floorBuffer);
                device.DrawPrimitives(PrimitiveType.TriangleList, 0, floorBuffer.VertexCount / 3);
            }
        }

        private void BuildFloorBuffer()
        {
            List<VertexPositionColor> vertexList = new List<VertexPositionColor>();
            int counter = 0;

            for (int x = 0; x < MAP_WIDTH; x += V_SIZE)
            {
                counter++;
                for (int z = 0; z < MAP_HEIGHT; z += V_SIZE)
                {
                    counter++;
                    foreach (VertexPositionColor vertex in FloorTile(x, z, floorColors[counter % 2]))
                        vertexList.Add(vertex);
                }
            }

            floorBuffer = new VertexBuffer(device, VertexPositionColor.VertexDeclaration,
                vertexList.Count, BufferUsage.WriteOnly);

            floorBuffer.SetData<VertexPositionColor>(vertexList.ToArray());
        }

        private List<VertexPositionColor> FloorTile(int xOffset, int zOffset, Color tileColor)
        {
            List<VertexPositionColor> vList = new List<VertexPositionColor>();

            vList.Add(new VertexPositionColor(new Vector3(xOffset, 0, zOffset), tileColor));
            vList.Add(new VertexPositionColor(new Vector3(V_SIZE + xOffset, 0, zOffset), tileColor));
            vList.Add(new VertexPositionColor(new Vector3(xOffset, 0, V_SIZE + zOffset), tileColor));

            vList.Add(new VertexPositionColor(new Vector3(V_SIZE + xOffset, 0, zOffset), tileColor));
            vList.Add(new VertexPositionColor(new Vector3(V_SIZE + xOffset, 0, V_SIZE + zOffset), tileColor));
            vList.Add(new VertexPositionColor(new Vector3(xOffset, 0, V_SIZE + zOffset), tileColor));

            return vList;
        }
    }
}
