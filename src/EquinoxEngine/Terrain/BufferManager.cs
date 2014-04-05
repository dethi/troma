﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EquinoxEngine.Terrain
{
    internal class BufferManager
    {
        int _active = 0;
        internal VertexBuffer VertexBuffer;
        IndexBuffer[] _IndexBuffers;
        GraphicsDevice _device;

        internal IndexBuffer IndexBuffer
        {
            get { return _IndexBuffers[_active]; }
        }

        internal BufferManager(VertexPositionNormalTexture[] vertices, GraphicsDevice device)
        {
            _device = device;

            VertexBuffer = new VertexBuffer(
                device, 
                VertexPositionNormalTexture.VertexDeclaration, 
                vertices.Length, 
                BufferUsage.WriteOnly);
            VertexBuffer.SetData(vertices);

            _IndexBuffers = new IndexBuffer[]
            {
                new IndexBuffer(_device, IndexElementSize.ThirtyTwoBits, 100000, BufferUsage.WriteOnly),
                new IndexBuffer(_device, IndexElementSize.ThirtyTwoBits, 100000, BufferUsage.WriteOnly)
            };
        }

        internal void UpdateIndexBuffer(int[] indices, int indexCount)
        {
            int inactive = (_active == 0) ? 1 : 0;
            _IndexBuffers[inactive].SetData(indices, 0, indexCount);
        }

        internal void SwapBuffer()
        {
            _active = (_active == 0) ? 1 : 0;
        }
    }
}
