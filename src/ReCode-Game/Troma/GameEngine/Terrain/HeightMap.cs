﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using GameEngine.Camera;

namespace GameEngine.Terrain
{
    public class Heightmap
    {
        #region Fields

        Game game;
        Effect effect;
        TerrainInfo terrainInfo;

        VertexBuffer vertexBuffer;
        IndexBuffer indexBuffer;

        float[,] depths;

        #endregion

        /// <summary>
        /// Build a terrain
        /// </summary>
        public Heightmap(Game game, Effect effect, TerrainInfo terrainInfo)
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
            Vector3 lightDirection = new Vector3(-1f, 1f, -1f);
            lightDirection.Normalize();

            effect.CurrentTechnique = effect.Techniques["Technique1"];

            effect.Parameters["terrainTexture"].SetValue(terrainInfo.Texture);
            effect.Parameters["World"].SetValue(Matrix.Identity);
            effect.Parameters["View"].SetValue(camera.View);
            effect.Parameters["Projection"].SetValue(camera.Projection);

            effect.Parameters["lightDirection"].SetValue(lightDirection);
            effect.Parameters["lightColor"].SetValue(new Vector4(1, 1, 1, 1));
            effect.Parameters["lightBrightness"].SetValue(0.8f);

            effect.Parameters["ambientLightLevel"].SetValue(0.15f);
            effect.Parameters["ambientLightColor"].SetValue(new Vector4(0.98f, 0.92f, 0.24f, 1f));

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                game.GraphicsDevice.SetVertexBuffer(vertexBuffer);
                game.GraphicsDevice.Indices = indexBuffer;
                game.GraphicsDevice.DrawIndexedPrimitives(
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
            depths = new float[terrainInfo.Size.Width, terrainInfo.Size.Height];

            Color[] heightmapData = new Color[heightmap.Width * heightmap.Height];
            heightmap.GetData(heightmapData);

            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                    depths[x, z] = (float)(heightmapData[x * width + z].R / 255f) 
                        * depth + terrainInfo.Position.Y;
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

            indexBuffer = new IndexBuffer(game.GraphicsDevice, IndexElementSize.ThirtyTwoBits,
                indices.Length, BufferUsage.None);
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

            for (int x = 0; x < vertices.Length; x++)
                vertices[x].Normal = Vector3.Zero;

            int triangleCount = indices.Length / 3;

            for (int x = 0; x < triangleCount; x++)
            {
                int v1 = indices[x * 3];
                int v2 = indices[(x * 3) + 1];
                int v3 = indices[(x * 3) + 2];

                Vector3 firstSide = vertices[v2].Position - vertices[v1].Position;
                Vector3 secondSide = vertices[v1].Position - vertices[v3].Position;
                Vector3 triangleNormal = Vector3.Cross(firstSide, secondSide);
                triangleNormal.Normalize();

                vertices[v1].Normal += triangleNormal;
                vertices[v2].Normal += triangleNormal;
                vertices[v3].Normal += triangleNormal;
            }

            for (int x = 0; x < vertices.Length; x++)
                vertices[x].Normal.Normalize();

            vertexBuffer.SetData(vertices);
        }

        /// <summary>
        /// Search the Y position of a terrain point
        /// </summary>
        public float? GetY(float x, float z)
        {
            int xmin = (int)Math.Floor(x);
            int xmax = xmin + 1;
            int zmin = (int)Math.Floor(z);
            int zmax = zmin + 1;

            if ((xmin < 0) || (xmax > depths.GetUpperBound(0)))
                throw new ArgumentOutOfRangeException(String.Format("X: [ {0} ; {1} ]", xmin, xmax));
            else if ((zmin < 0) || (zmax > depths.GetUpperBound(1)))
                throw new ArgumentOutOfRangeException(String.Format("Z: [ {0} ; {1} ]", zmin, zmax));
            else
            {
                Vector3 p1 = new Vector3(xmin, depths[xmin, zmax], zmax);
                Vector3 p2 = new Vector3(xmax, depths[xmax, zmin], zmin);
                Vector3 p3;

                if ((x - xmin) + (z - zmin) <= 1)
                    p3 = new Vector3(xmin, depths[xmin, zmin], zmin);
                else
                    p3 = new Vector3(xmax, depths[xmax, zmax], zmax);

                Plane plane = new Plane(p1, p2, p3);
                Ray ray = new Ray(new Vector3(x, 0, z), Vector3.Up);
                float? height = ray.Intersects(plane);

                return height;
            }
        }
    }
}
