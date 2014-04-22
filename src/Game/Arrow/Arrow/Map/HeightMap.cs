﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arrow
{
    public class HeightMap
    {
        #region Attributes

        private Game game;
        private Camera camera;

        private VertexBuffer vertexBuffer;
        private IndexBuffer indexBuffer;

        private Texture2D terrainTexture;
        private Texture2D heightMapTexture;

        private float textureScale;
        private float[,] heights;

        private int offsetX;
        private int offsetZ;

        #endregion

        /// <summary>
        /// Build a terrain
        /// </summary>
        /// <param name="heightMapT">Heighmap texture</param>
        /// <param name="terrainT">Terrain texture</param>
        /// <param name="textureScale">Size of terrain texture</param>
        /// <param name="terrainWidth">Terrain width (X)</param>
        /// <param name="terrainHeight">Terrain height (Z)</param>
        /// <param name="heightScale">Max terrain depth (Y)</param>
        /// <param name="offsetX">X first position</param>
        /// <param name="offsetZ">Z first position</param>
        public HeightMap(Game game, string heightMapT, string terrainT,
            float textureScale, int terrainWidth, int terrainHeight, 
            float heightScale, int offsetX, int offsetZ)
        {
            this.game = game;
            this.terrainTexture = game.Content.Load<Texture2D>(terrainT);
            this.textureScale = textureScale;
            this.heightMapTexture = game.Content.Load<Texture2D>(heightMapT);

            this.offsetX = offsetX * (terrainWidth - 1);
            this.offsetZ = offsetZ * (terrainHeight - 1);

            camera = Camera.Instance;

            ReadHeightMap(heightMapTexture, terrainWidth, terrainHeight, heightScale);
            BuildVertexBuffer(terrainWidth, terrainHeight);
            BuildIndexBuffer(terrainWidth, terrainHeight);
            CalculateNormals();
        }

        public void Draw(Effect effect)
        {
            game.ResetGraphicsDeviceFor3D();

            Vector3 lightDirection = new Vector3(-1f, 1f, -1f);
            lightDirection.Normalize();

            effect.CurrentTechnique = effect.Techniques["Technique1"];

            effect.Parameters["terrainTexture1"].SetValue(terrainTexture);
            effect.Parameters["World"].SetValue(Matrix.Identity);
            effect.Parameters["View"].SetValue(camera.View);
            effect.Parameters["Projection"].SetValue(camera.Projection);

            effect.Parameters["lightDirection"].SetValue(lightDirection);
            effect.Parameters["lightColor"].SetValue(new Vector4(1, 1, 1, 1));
            effect.Parameters["lightBrightness"].SetValue(0.8f);

            effect.Parameters["ambientLightLevel"].SetValue(0.15f);
            //effect.Parameters["ambientLightColor"].SetValue(new Vector4(0.98f, 0.92f, 0.24f, 1f));

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                game.GraphicsDevice.SetVertexBuffer(vertexBuffer);
                game.GraphicsDevice.Indices = indexBuffer;
                game.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vertexBuffer.VertexCount,
                    0, indexBuffer.IndexCount / 3);
            }
        }

        /// <summary>
        /// Build matrice using the heighmap image
        /// </summary>
        /// <param name="heightMap">Heighmap image</param>
        /// <param name="heightScale">Max depth (Y) of the terrain</param>
        private void ReadHeightMap(Texture2D heightMap, int terrainWidth, int terrainHeight,
            float heightScale)
        {
            heights = new float[terrainWidth, terrainHeight];

            Color[] heightMapData = new Color[heightMap.Width * heightMap.Height];
            heightMap.GetData(heightMapData);

            for (int x = 0; x < terrainWidth; x++)
            {
                for (int z = 0; z < terrainHeight; z++)
                {
                    byte height = heightMapData[x + z * terrainWidth].R;
                    heights[x, z] = (float)(height / 255f) * heightScale;
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
                    vertices[x + z * width].Position = new Vector3(x + offsetX, heights[x, z], z + offsetZ);
                    vertices[x + z * width].TextureCoordinate = new Vector2(
                        (float)x / textureScale, (float)z / textureScale);
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
        public float? GetHeight(float x, float z)
        {
            int xmin = (int)Math.Floor(x);
            int xmax = xmin + 1;
            int zmin = (int)Math.Floor(z);
            int zmax = zmin + 1;

            if ((xmin < 0) || (zmin < 0) || (xmax > heights.GetUpperBound(0)) ||
                (zmax > heights.GetUpperBound(1)))
                return null;
            else
            {
                Vector3 p1 = new Vector3(xmin, heights[xmin, zmax], zmax);
                Vector3 p2 = new Vector3(xmax, heights[xmax, zmin], zmin);
                Vector3 p3;

                if ((x - xmin) + (z - zmin) <= 1)
                    p3 = new Vector3(xmin, heights[xmin, zmin], zmin);
                else
                    p3 = new Vector3(xmax, heights[xmax, zmax], zmax);

                Plane plane = new Plane(p1, p2, p3);
                Ray ray = new Ray(new Vector3(x, 0, z), Vector3.Up);
                float? height = ray.Intersects(plane);

                return height;
            }
        }
    }
}