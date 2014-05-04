using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine
{
    public struct BillboardVertex : IVertexType
    {
        public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration
        (
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(sizeof(float) * 3, VertexElementFormat.Vector4, VertexElementUsage.TextureCoordinate, 0)
        );

        public Vector3 Position;
        public Vector4 TexCoord;

        public BillboardVertex(Vector3 position, Vector2 texCoord, Vector2 offset)
        {
            Position = position;

            // Coordinates for the texture map.
            TexCoord.X = texCoord.X;
            TexCoord.Y = texCoord.Y;

            // The 2D offset vector.
            TexCoord.Z = offset.X;
            TexCoord.W = offset.Y;
        }

        VertexDeclaration IVertexType.VertexDeclaration
        {
            get { return VertexDeclaration; }
        }
    }

    public class Billboard
    {
        public VertexBuffer vertexBuffer;
        private IndexBuffer indexBuffer;

        public Billboard(GraphicsDevice graphicsDevice, Vector3 position, float width, float height)
        {
            Create(graphicsDevice, position, width, height);
        }

        public void Create(GraphicsDevice graphicsDevice, Vector3 position, float width, float height)
        {
            BillboardVertex[] vertices = new BillboardVertex[4];

            vertices[0].Position = position;
            vertices[0].TexCoord = new Vector4(0.0f, 0.0f, -1.0f, 1.0f);

            vertices[1].Position = position;
            vertices[1].TexCoord = new Vector4(1.0f, 0.0f, 1.0f, 1.0f);

            vertices[2].Position = position;
            vertices[2].TexCoord = new Vector4(0.0f, 1.0f, -1.0f, -1.0f);

            vertices[3].Position = position;
            vertices[3].TexCoord = new Vector4(1.0f, 1.0f, 1.0f, -1.0f);


            vertexBuffer = new VertexBuffer(graphicsDevice, typeof(BillboardVertex), vertices.Length, BufferUsage.WriteOnly);
            vertexBuffer.SetData(vertices);

            short[] indices =
                {
                    (short)0, (short)1, (short)2,
                    (short)2, (short)1, (short)3
                };

            indexBuffer = new IndexBuffer(graphicsDevice, IndexElementSize.SixteenBits, indices.Length, BufferUsage.WriteOnly);
            indexBuffer.SetData(indices);
        }

        public void Draw(GraphicsDevice graphicsDevice, Effect effect)
        {
            if (graphicsDevice != null && effect != null)
            {
                graphicsDevice.SetVertexBuffer(vertexBuffer);
                graphicsDevice.Indices = indexBuffer;

                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vertexBuffer.VertexCount, 0, 2);
                }

                graphicsDevice.Indices = null;
                graphicsDevice.SetVertexBuffer(null);
            }
        }
    }
}