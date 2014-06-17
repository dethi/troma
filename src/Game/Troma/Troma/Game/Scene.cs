﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine;

namespace Troma
{
    public enum Map
    {
        Town,
    }

    public class Scene
    {
        public static CloudManager CloudManager { get; private set; }
        public static ITerrain Terrain { get; private set; }

        public static void Initialize(Map m, ICamera camera)
        {
            EntityManager.Clear();
            CollisionManager.Clear();

            switch (m)
            {
                case Map.Town:
                    BuildTown(camera);
                    break;

                default:
                    break;
            }

            EntityManager.Initialize();
            CollisionManager.Initialize();
        }

        public static void Update(GameTime gameTime, bool dynamicClouds, bool isActive)
        {
            if (dynamicClouds)
                CloudManager.Update(gameTime);

            if (isActive)
            {
                EntityManager.Update(gameTime);
                CollisionManager.Update(gameTime);
            }
        }

        public static void Draw(GameTime gameTime, ICamera camera)
        {
            CloudManager.Draw(gameTime, camera);
            Terrain.Draw(camera);
            EntityManager.Draw(gameTime, camera);
        }

        private static void BuildTown(ICamera camera)
        {
            Effect terrainEffect = FileManager.Load<Effect>("Effects/Terrain");
            Texture2D terrainTexture = FileManager.Load<Texture2D>("Terrains/texture");
            Texture2D terrainHeighmap = FileManager.Load<Texture2D>("Terrains/heightmap");

            TerrainInfo terrainInfo = new TerrainInfo()
            {
                Position = Vector3.Zero,
                Size = new Size(513, 513),
                Depth = 0,
                Texture = terrainTexture,
                TextureScale = 32,
                Heighmap = terrainHeighmap
            };

            float y = 0;

            CloudManager = SceneRenderer.InitializeSky(SkyType.CloudField, terrainInfo, camera);
            Terrain = new HeightMap(GameServices.Game, terrainEffect, terrainInfo);

            Effect modelEffect = FileManager.Load<Effect>("Effects/GameObject");
            Effect modelWithNormal = FileManager.Load<Effect>("Effects/GameObjectWithNormal");
            modelWithNormal.Name = "GameObjectWithNormal";

            #region Rails

            List<Vector3> modelPos = new List<Vector3>();
            modelPos.Add(new Vector3(0, y, 200));
            modelPos.Add(new Vector3(120, y, 200));
            modelPos.Add(new Vector3(240, y, 200));
            modelPos.Add(new Vector3(360, y, 200));
            modelPos.Add(new Vector3(480, y, 200));
            modelPos.Add(new Vector3(0, y, 210));
            modelPos.Add(new Vector3(120, y, 210));
            modelPos.Add(new Vector3(240, y, 210));
            modelPos.Add(new Vector3(360, y, 210));
            modelPos.Add(new Vector3(480, y, 210));

            VectGameObject.BuildEntity(modelPos.ToArray(), "Town/rail", modelEffect);

            #endregion

            #region Wood barrier

            List<Vector3> barrierPos = new List<Vector3>();

            for (int i = 202; i < 430; i += 10)
            {
                barrierPos.Add(new Vector3(i, y, 192));
                barrierPos.Add(new Vector3(i, y, 222));
            }

            for (int j = 460; j < 512; j += 10)
                barrierPos.Add(new Vector3(j, y, 222));

            VectGameObject.BuildEntity(barrierPos.ToArray(), "Town/wood_barrier", modelEffect);

            #endregion

            GameObject.BuildEntity(new Vector3(0, y, 117), "Town/gare", modelEffect);
            GameObject.BuildEntity(new Vector3(0, y, 220), "Town/quai", modelEffect);
            GameObject.BuildEntity(new Vector3(460, y, 153), "Town/garde_passage_a_niveau", modelEffect);
            GameObject.BuildEntity(new Vector3(435, y, 192), "Town/barriere_train_droite", modelEffect);
            GameObject.BuildEntity(new Vector3(435, y, 222), "Town/barriere_train_gauche", modelEffect);
            GameObject.BuildEntity(new Vector3(300, y, 300), "Town/eglise", modelEffect);
            GameObject.BuildEntity(new Vector3(50, y, 290), "Town/cimetiere", modelEffect);
            GameObject.BuildEntity(new Vector3(380, y, 30), "Town/mairie", modelEffect);
            GameObject.BuildEntity(new Vector3(290, y, 55), "Town/fontaine", modelEffect);
        }
    }
}