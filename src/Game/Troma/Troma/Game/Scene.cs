using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine;
using ClientServerExtension;

namespace Troma
{
    public enum Map
    {
        Town,
        Cracovie,
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

                case Map.Cracovie :
                    BuildCracovie(camera);
                    break;

                default:
                    break;
            }

            EntityManager.Initialize();
            LoadBox(m);
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

        private static void BuildCracovie(ICamera camera)
        {
            Effect terrainEffect = FileManager.Load<Effect>("Effects/Terrain");
            Texture2D terrainTexture = null;
            Texture2D terrainHeighmap = null;
            Effect modelEffect = FileManager.Load<Effect>("Effects/GameObject");


            TerrainInfo terrainInfo = new TerrainInfo()
            {
                Position = Vector3.Zero,
                Size = new Size(5*256, 5*256),
                Depth = 20,
                Texture = terrainTexture,
                TextureScale = 32,
                Heighmap = terrainHeighmap
            };

            CloudManager = SceneRenderer.InitializeSky(SkyType.CloudField, terrainInfo, camera);
            Terrain = new MultiHeightMap(GameServices.Game, terrainEffect, terrainInfo, "cracovie", 5);


            GameObject.BuildEntity(new Vector3(132, 0, 276), "Town/immeuble4", modelEffect);
            GameObject.BuildEntity(new Vector3(132, 0, 556), "Town/immeuble4", modelEffect);
            GameObject.BuildEntity(new Vector3(132, 0, 436), "Town/immeuble3", modelEffect);

            GameObject.BuildEntity(new Vector3(262, 0, 836), "Town/immeuble3", modelEffect);

            GameObject.BuildEntity(new Vector3(292, 0, 276), "Town/immeuble2", modelEffect);
            GameObject.BuildEntity(new Vector3(292, 0, 436), "Town/poste", modelEffect);
            GameObject.BuildEntity(new Vector3(292, 0, 556), "Town/hotel", modelEffect);

            GameObject.BuildEntity(new Vector3(412, 0, 276), "Town/immeuble", modelEffect);
            GameObject.BuildEntity(new Vector3(412, 0, 676), "Town/gare", modelEffect);
            GameObject.BuildEntity(new Vector3(412, 0, 778), "Town/quai", modelEffect);
            GameObject.BuildEntity(new Vector3(412, 0, 836), "Town/maison2", modelEffect);

            GameObject.BuildEntity(new Vector3(492, 0, 516), "Town/fontaine", modelEffect);

            GameObject.BuildEntity(new Vector3(532, 0, 836), "Town/maison2", modelEffect);

            GameObject.BuildEntity(new Vector3(652, 0, 276), "Town/immeuble2", modelEffect);
            GameObject.BuildEntity(new Vector3(652, 0, 436), "Town/eglise", modelEffect);
            GameObject.BuildEntity(new Vector3(652, 0, 556), "Town/mairie", modelEffect);
            
            GameObject.BuildEntity(new Vector3(732, 0, 676), "Town/garde_passage_a_niveau", modelEffect);
            
            
            GameObject.BuildEntity(new Vector3(772, 0, 276), "Town/immeuble4", modelEffect);
            GameObject.BuildEntity(new Vector3(800, 0, 436), "Town/cimetiere", modelEffect);


            //GameObject.BuildEntity(new Vector3(460, 0, 153), "Town/garde_passage_a_niveau", modelEffect);
            //GameObject.BuildEntity(new Vector3(435, 0, 192), "Town/barriere_train_droite", modelEffect);
            //GameObject.BuildEntity(new Vector3(435, 0, 222), "Town/barriere_train_gauche", modelEffect);

             #region Rails

            List<Vector3> modelPos = new List<Vector3>();
            modelPos.Add(new Vector3(0, 0, 758));
            modelPos.Add(new Vector3(120, 0, 758));
            modelPos.Add(new Vector3(240, 0, 758));
            modelPos.Add(new Vector3(360, 0, 758));
            modelPos.Add(new Vector3(480, 0, 758));
            modelPos.Add(new Vector3(600, 0, 758));
            modelPos.Add(new Vector3(720, 0, 758));
            modelPos.Add(new Vector3(840, 0, 758));
            modelPos.Add(new Vector3(960, 0, 758));
            modelPos.Add(new Vector3(0, 0, 768));
            modelPos.Add(new Vector3(120, 0, 768));
            modelPos.Add(new Vector3(240, 0, 768));
            modelPos.Add(new Vector3(360, 0, 768));
            modelPos.Add(new Vector3(480, 0, 768));
            modelPos.Add(new Vector3(600, 0, 768));
            modelPos.Add(new Vector3(720, 0, 768));
            modelPos.Add(new Vector3(840, 0, 768));
            modelPos.Add(new Vector3(960, 0, 768));

            VectGameObject.BuildEntity(modelPos.ToArray(), "Town/rail", modelEffect);

            #endregion
        }

        private static void LoadBox(Map m)
        {
            Box worldBox;

            try
            {
                Stream stream;
                BinaryFormatter bFormatter = new BinaryFormatter();

                if (m == Map.Town)
                    stream = File.Open("Content/Box/TownBox.bin", FileMode.Open);
                else if (m == Map.Cracovie)
                    stream = File.Open("Content/Box/CracovieBox.bin", FileMode.Open);
                else
                    throw new FileNotFoundException();

                worldBox = (Box)bFormatter.Deserialize(stream);
                stream.Close();
            }
            catch
            {
                worldBox = new Box();

                List<Entity> entitiesWithBox = new List<Entity>();
                entitiesWithBox.AddRange(EntityManager.EntitiesWith<CollisionBox>());

                foreach (Entity e in entitiesWithBox)
                {
                    worldBox.Generate(
                        e.GetComponent<Model3D>().Model,
                        e.GetComponent<Transform>().World);
                }

                if (m == Map.Town)
                    worldBox.Save("Content/Box/TownBox");
                //else if (m == Map.Cracovie)
                    //worldBox.Save("Content/Box/CracovieBox");
            }

            CollisionManager.AddBox(worldBox.BoudingBox);
        }
    }
}
