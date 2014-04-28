using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GameEngine
{
    public class HeightMap : ITerrain
    {
        #region Fields

        private Game game;
        private Effect effect;
        private TerrainInfo terrainInfo;

        private VertexBuffer vertexBuffer;
        private IndexBuffer indexBuffer;

        private float[,] depths;

        #endregion

        /// <summary>
        /// Build a terrain
        /// </summary>
        public HeightMap(Game game, Effect effect, TerrainInfo terrainInfo)
        {
            this.game = game;
            this.effect = effect;
            this.terrainInfo = terrainInfo;

            ReadHeightMap(
                terrainInfo.Heighmap,
                terrainInfo.Size.Width,
                terrainInfo.Size.Height,
                terrainInfo.Depth);
            BuildVertexBuffer(terrainInfo.Size.Width, terrainInfo.Size.Height);
            BuildIndexBuffer(terrainInfo.Size.Width, terrainInfo.Size.Height);
            CalculateNormals();
        }

        public void Draw(ICamera camera)
        {
            effect.CurrentTechnique = effect.Techniques["Technique1"];

            effect.Parameters["terrainTexture"].SetValue(terrainInfo.Texture);
            effect.Parameters["World"].SetValue(Matrix.Identity);
            effect.Parameters["View"].SetValue(camera.View);
            effect.Parameters["Projection"].SetValue(camera.Projection);

            effect.Parameters["lightDirection"].SetValue(RenderState.LightDirection);
            effect.Parameters["lightColor"].SetValue(RenderState.LightColor);
            effect.Parameters["lightBrightness"].SetValue(RenderState.LightBrightness);

            effect.Parameters["ambientLightLevel"].SetValue(RenderState.AmbientLightLevel);
            effect.Parameters["ambientLightColor"].SetValue(RenderState.AmbientLightColor);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GameServices.GraphicsDevice.SetVertexBuffer(vertexBuffer);
                GameServices.GraphicsDevice.Indices = indexBuffer;
                GameServices.GraphicsDevice.DrawIndexedPrimitives(
                    PrimitiveType.TriangleList, 0, 0, vertexBuffer.VertexCount,
                    0, indexBuffer.IndexCount / 3);
            }
        }

        /// <summary>
        /// Build matrice using the heighmap image
        /// </summary>
        /// <param name="heightmap">Heighmap image</param>
        /// <param name="width">Terrain width</param>
        /// <param name="height">Terrain height</param>
        /// <param name="depth">Max depth of the terrain</param>
        private void ReadHeightMap(Texture2D heightmap, int width, int height, float depth)
        {
            depths = new float[width, height];

            Color[] heightmapData = new Color[heightmap.Width * heightmap.Height];
            heightmap.GetData(heightmapData);

            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    depths[x, z] = (float)(heightmapData[x + z * width].R / 255f)
                        * depth + terrainInfo.Position.Y;
                }
            }
        }

        /// <summary>
        /// Build vertex buffer
        /// </summary>
        /// <param name="width">Terrain width</param>
        /// <param name="height">Terrain height</param>
        private void BuildVertexBuffer(int width, int height)
        {
            VertexPositionNormalTexture[] vertices = new VertexPositionNormalTexture[width * height];

            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    vertices[x + z * width].Position = new Vector3(
                        terrainInfo.Position.X + x,
                        depths[x, z],
                        terrainInfo.Position.Z + z);

                    vertices[x + z * width].TextureCoordinate = new Vector2(
                        (float)x / terrainInfo.TextureScale,
                        (float)z / terrainInfo.TextureScale);
                }
            }

            vertexBuffer = new VertexBuffer(game.GraphicsDevice, typeof(VertexPositionNormalTexture),
                vertices.Length, BufferUsage.None);
            vertexBuffer.SetData(vertices);
        }

        /// <summary>
        /// Build index buffer
        /// </summary>
        /// <param name="width">Terrain width</param>
        /// <param name="height">Terrain height</param>
        private void BuildIndexBuffer(int width, int height)
        {
            int indexCount = (width - 1) * (height - 1) * 6;
            int[] indices = new int[indexCount];
            int counter = 0;

            for (int z = 0; z < height - 1; z++)
            {
                for (int x = 0; x < width - 1; x++)
                {
                    int upperLeft = x + (z * width);
                    int upperRight = upperLeft + 1;
                    int lowerLeft = upperLeft + width;
                    int lowerRight = upperLeft + width + 1;

                    indices[counter++] = upperLeft;
                    indices[counter++] = lowerRight;
                    indices[counter++] = lowerLeft;
                    indices[counter++] = upperLeft;
                    indices[counter++] = upperRight;
                    indices[counter++] = lowerRight;
                }
            }

            indexBuffer = new IndexBuffer(game.GraphicsDevice, 
                IndexElementSize.ThirtyTwoBits, indices.Length, BufferUsage.None);
            indexBuffer.SetData(indices);
        }

        /// <summary>
        /// Calculates normal of each vertices
        /// </summary>
        private void CalculateNormals()
        {
            VertexPositionNormalTexture[] vertices =
                new VertexPositionNormalTexture[vertexBuffer.VertexCount];
            int[] indices = new int[indexBuffer.IndexCount];

            vertexBuffer.GetData(vertices);
            indexBuffer.GetData(indices);

            for (int i = 0; i < vertices.Length; i++)
                vertices[i].Normal = Vector3.Zero;

            int triangleCount = indices.Length / 3;

            for (int i = 0; i < triangleCount; i++)
            {
                int v1 = indices[i * 3];
                int v2 = indices[(i * 3) + 1];
                int v3 = indices[(i * 3) + 2];

                Vector3 firstSide = vertices[v2].Position - vertices[v1].Position;
                Vector3 secondSide = vertices[v1].Position - vertices[v3].Position;
                Vector3 triangleNormal = Vector3.Cross(firstSide, secondSide);
                triangleNormal.Normalize();

                vertices[v1].Normal += triangleNormal;
                vertices[v2].Normal += triangleNormal;
                vertices[v3].Normal += triangleNormal;
            }

            for (int i = 0; i < vertices.Length; i++)
                vertices[i].Normal.Normalize();

            vertexBuffer.SetData(vertices);
        }

        /// <summary>
        /// Check if the position is on the terrain
        /// </summary>
        public bool IsOnTerrain(Vector3 pos)
        {
            Vector3 positionOnHeightmap = pos - terrainInfo.Position;

            return (0 < positionOnHeightmap.X &&
                positionOnHeightmap.X < terrainInfo.Size.Width - 1 &&
                0 < positionOnHeightmap.Z &&
                positionOnHeightmap.Z < terrainInfo.Size.Height - 1);
        }

        /// <summary>
        /// Search the Y position of a terrain point.
        /// Throw an IndexOutOfRangeException if position isn't on the terrain.
        /// </summary>
        public float GetY(Vector3 pos)
        {
            int xmin = (int)Math.Floor(pos.X);
            int xmax = xmin + 1;
            int zmin = (int)Math.Floor(pos.Z);
            int zmax = zmin + 1;

            Vector3 p1 = new Vector3(xmin, depths[xmin, zmax], zmax);
            Vector3 p2 = new Vector3(xmax, depths[xmax, zmin], zmin);
            Vector3 p3;

            if ((pos.X - xmin) + (pos.Z - zmin) <= 1)
                p3 = new Vector3(xmin, depths[xmin, zmin], zmin);
            else
                p3 = new Vector3(xmax, depths[xmax, zmax], zmax);

            Plane plane = new Plane(p1, p2, p3);
            Ray ray = new Ray(new Vector3(pos.X, 0, pos.Z), Vector3.Up);
            float? height = ray.Intersects(plane);

            return height.Value;
        }
    }
}
