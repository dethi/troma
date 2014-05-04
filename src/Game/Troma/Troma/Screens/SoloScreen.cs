using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameEngine;

namespace Troma
{
    public class SoloScreen : GameScreen
    {
        #region Fields

        private FirstPersonView camera;
        private Player player;
        private ITerrain terrain;
        private CloudManager cloudManager;

        private string _mapName;

        #endregion

        #region Initialization

        public SoloScreen(Game game, string mapName)
            : base(game)
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            _mapName = mapName;
        }

        public override void LoadContent()
        {
            base.LoadContent();

#if DEBUG
            XConsole.Initialize();
            DrawingAxes.Initialize();
            BoundingSphereRenderer.Initialize(30);
#endif

            EntityManager.Clear();
            CollisionManager.Clear();
            TargetManager.Clear();

            camera = new FirstPersonView(game.GraphicsDevice.Viewport.AspectRatio);

            if (_mapName == "Farm")
            {
                float y = 15;
                player = new Player(new Vector3(80, 20, 460), Vector3.Zero, camera);

                Effect terrainEffect = FileManager.Load<Effect>("Effects/Terrain");
                Texture2D terrainTexture = FileManager.Load<Texture2D>("Terrains/Farm/texture");
                Texture2D terrainHeighmap = FileManager.Load<Texture2D>("Terrains/Farm/heightmap");

                TerrainInfo terrainInfo = new TerrainInfo()
                {
                    Position = Vector3.Zero,
                    Size = new Size(513, 513),
                    Depth = y,
                    Texture = terrainTexture,
                    TextureScale = 512,
                    Heighmap = terrainHeighmap
                };

                terrain = new HeightMap(game, terrainEffect, terrainInfo);

                Effect modelEffect = FileManager.Load<Effect>("Effects/GameObject");

                WeaponInfo garandM1 = new WeaponInfo()
                {
                    MunitionPerLoader = 8,
                    Loader = 10,

                    Automatic = false,
                    ROF = 0.5f,

                    Model = "GarandM1"
                };

                player.Initialize(terrain, WeaponObject.BuildEntity(garandM1, modelEffect));

                #region Models

                #region Wood barrier
                /*
            Model woodBarrierX = content.Load<Model>("Models/Farm/wood_barrier_x");
            Model woodBarrierZ = content.Load<Model>("Models/Farm/wood_barrier_z");

            entities.AddEntity(woodBarrierZ, new Vector2(119, 101));
            entities.AddEntity(woodBarrierZ, new Vector2(119, 91));
            entities.AddEntity(woodBarrierZ, new Vector2(119, 81));
            entities.AddEntity(woodBarrierZ, new Vector2(119, 71));
            entities.AddEntity(woodBarrierZ, new Vector2(119, 61));
            entities.AddEntity(woodBarrierZ, new Vector2(119, 51));
            entities.AddEntity(woodBarrierZ, new Vector2(119, 41));
            entities.AddEntity(woodBarrierZ, new Vector2(119, 31));
            entities.AddEntity(woodBarrierZ, new Vector2(119, 21));
            entities.AddEntity(woodBarrierZ, new Vector2(119, 11));
            entities.AddEntity(woodBarrierZ, new Vector2(119, 1));
            entities.AddEntity(woodBarrierX, new Vector2(109, 1));
            entities.AddEntity(woodBarrierX, new Vector2(99, 1));
            entities.AddEntity(woodBarrierX, new Vector2(89, 1));
            entities.AddEntity(woodBarrierX, new Vector2(79, 1));
            entities.AddEntity(woodBarrierX, new Vector2(69, 1));
            entities.AddEntity(woodBarrierX, new Vector2(59, 1));
            entities.AddEntity(woodBarrierX, new Vector2(49, 1));
            entities.AddEntity(woodBarrierX, new Vector2(39, 1));
            entities.AddEntity(woodBarrierX, new Vector2(29, 1));
            entities.AddEntity(woodBarrierX, new Vector2(19, 1));
            entities.AddEntity(woodBarrierX, new Vector2(9, 1));
            entities.AddEntity(woodBarrierX, new Vector2(0, 1));

            #endregion

            #region Barbed barrier

            Model barbedBarrierZ = content.Load<Model>("Models/Farm/barbed_barrier_z");
            Model postBarrier = content.Load<Model>("Models/Farm/post_barrier");

            entities.AddEntity(barbedBarrierZ, new Vector2(160, 0));
            entities.AddEntity(barbedBarrierZ, new Vector2(160, 9));
            entities.AddEntity(barbedBarrierZ, new Vector2(160, 18));
            entities.AddEntity(barbedBarrierZ, new Vector2(160, 27));
            entities.AddEntity(barbedBarrierZ, new Vector2(160, 36));
            entities.AddEntity(barbedBarrierZ, new Vector2(160, 45));
            entities.AddEntity(barbedBarrierZ, new Vector2(160, 54));
            entities.AddEntity(barbedBarrierZ, new Vector2(160, 63));
            entities.AddEntity(barbedBarrierZ, new Vector2(160, 72));
            entities.AddEntity(barbedBarrierZ, new Vector2(160, 81));
            entities.AddEntity(barbedBarrierZ, new Vector2(160, 90));
            entities.AddEntity(barbedBarrierZ, new Vector2(160, 99));
            entities.AddEntity(barbedBarrierZ, new Vector2(160, 108));
            entities.AddEntity(barbedBarrierZ, new Vector2(160, 117));
            entities.AddEntity(barbedBarrierZ, new Vector2(160, 126));
            entities.AddEntity(barbedBarrierZ, new Vector2(160, 135));
            entities.AddEntity(barbedBarrierZ, new Vector2(160, 144));
            entities.AddEntity(barbedBarrierZ, new Vector2(160, 153));
            entities.AddEntity(barbedBarrierZ, new Vector2(160, 162));
            entities.AddEntity(barbedBarrierZ, new Vector2(160, 171));
            entities.AddEntity(barbedBarrierZ, new Vector2(160, 180));
            entities.AddEntity(barbedBarrierZ, new Vector2(160, 189));
            entities.AddEntity(barbedBarrierZ, new Vector2(160, 198));
            entities.AddEntity(barbedBarrierZ, new Vector2(160, 207));
            entities.AddEntity(barbedBarrierZ, new Vector2(160, 216));
            entities.AddEntity(barbedBarrierZ, new Vector2(160, 225));
            entities.AddEntity(barbedBarrierZ, new Vector2(160, 234));
            entities.AddEntity(barbedBarrierZ, new Vector2(160, 242));
            entities.AddEntity(barbedBarrierZ, new Vector2(160, 251));
            entities.AddEntity(barbedBarrierZ, new Vector2(160, 260));
            entities.AddEntity(barbedBarrierZ, new Vector2(160, 269));
            entities.AddEntity(barbedBarrierZ, new Vector2(160, 278));
            entities.AddEntity(barbedBarrierZ, new Vector2(160, 287));
            entities.AddEntity(barbedBarrierZ, new Vector2(160, 296));
            entities.AddEntity(barbedBarrierZ, new Vector2(160, 305));
            entities.AddEntity(postBarrier, new Vector2(160, 315));

            entities.AddEntity(barbedBarrierZ, new Vector2(160, 340));
            entities.AddEntity(barbedBarrierZ, new Vector2(160, 349));
            entities.AddEntity(barbedBarrierZ, new Vector2(160, 358));
            entities.AddEntity(barbedBarrierZ, new Vector2(160, 367));
            entities.AddEntity(barbedBarrierZ, new Vector2(160, 376));
            entities.AddEntity(barbedBarrierZ, new Vector2(160, 385));
            entities.AddEntity(barbedBarrierZ, new Vector2(160, 394));
            entities.AddEntity(barbedBarrierZ, new Vector2(160, 403));
            entities.AddEntity(barbedBarrierZ, new Vector2(160, 412));
            entities.AddEntity(barbedBarrierZ, new Vector2(160, 421));
            entities.AddEntity(barbedBarrierZ, new Vector2(160, 430));
            entities.AddEntity(barbedBarrierZ, new Vector2(160, 439));
            entities.AddEntity(barbedBarrierZ, new Vector2(160, 448));
            entities.AddEntity(barbedBarrierZ, new Vector2(160, 457));
            entities.AddEntity(barbedBarrierZ, new Vector2(160, 466));
            entities.AddEntity(barbedBarrierZ, new Vector2(160, 475));
            entities.AddEntity(barbedBarrierZ, new Vector2(160, 484));
            entities.AddEntity(barbedBarrierZ, new Vector2(160, 493));
            entities.AddEntity(barbedBarrierZ, new Vector2(160, 502));
            entities.AddEntity(postBarrier, new Vector2(160, 511));
            */
            #endregion

                GameObject.BuildEntity(new Vector3(0, y, 511), "Farm/house", modelEffect);
                GameObject.BuildEntity(new Vector3(60, y, 511), "Farm/barn", modelEffect);
                GameObject.BuildEntity(new Vector3(150, y, 150), "Farm/bandbags", modelEffect);
                GameObject.BuildEntity(new Vector3(58, y, 400), "Farm/table", modelEffect);
                GameObject.BuildEntity(new Vector3(58, y, 405), "Farm/barrel", modelEffect);
                GameObject.BuildEntity(new Vector3(122, y, 206), "Farm/truck_allemand", modelEffect);
                GameObject.BuildEntity(new Vector3(0, y, 262), "Farm/farm", modelEffect);
                GameObject.BuildEntity(new Vector3(0, y, 411), "Farm/shelter", modelEffect);

                #endregion
            }
            else
            {
                player = new Player(new Vector3(10, 15, 10), Vector3.Zero, camera);

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

                cloudManager = SceneRenderer.InitializeSky(SkyType.CloudField, terrainInfo, camera);
                terrain = new HeightMap(game, terrainEffect, terrainInfo);

                Effect modelEffect = FileManager.Load<Effect>("Effects/GameObject");
                Effect modelWithNormal = FileManager.Load<Effect>("Effects/GameObjectWithNormal");

                WeaponInfo garandM1 = new WeaponInfo()
                {
                    MunitionPerLoader = 8,
                    Loader = 10,

                    Automatic = false,
                    ROF = 0.5f,

                    Model = "Weapon/M1Garand",
                    Position = new Vector3(-0.7f, -0.3f, 0),
                    Rotation = new Vector3(0, 0, 0),
                    PositionSight = new Vector3(0, 0, -1.3f),
                    RotationSight = Vector3.Zero,

                    SFXEmpty = "GarandM1_empty",
                    SFXReload = "GarandM1_reload",
                    SFXShoot = "GarandM1_shoot"
                };

                player.Initialize(terrain, WeaponObject.BuildEntity(garandM1, modelEffect));

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

                #region Target

                List<Tuple<Vector3, float>> ciblePos = new List<Tuple<Vector3, float>>();
                ciblePos.Add(new Tuple<Vector3, float>(new Vector3(200, y, 300), 90));
                ciblePos.Add(new Tuple<Vector3, float>(new Vector3(125, y, 260), 60));
                ciblePos.Add(new Tuple<Vector3, float>(new Vector3(190, y + 4, 234), 60));
                ciblePos.Add(new Tuple<Vector3, float>(new Vector3(440, y, 185), 90));
                ciblePos.Add(new Tuple<Vector3, float>(new Vector3(437, y, 294), 60));
                ciblePos.Add(new Tuple<Vector3, float>(new Vector3(309, y, 398), 60));
                ciblePos.Add(new Tuple<Vector3, float>(new Vector3(453, y + 1, 212), 90));
                ciblePos.Add(new Tuple<Vector3, float>(new Vector3(370, y + 1, 203), 90));
                ciblePos.Add(new Tuple<Vector3, float>(new Vector3(334, y, 284), 60));
                ciblePos.Add(new Tuple<Vector3, float>(new Vector3(139, y + 4, 185), 90));
                ciblePos.Add(new Tuple<Vector3, float>(new Vector3(73, y + 4, 232), 50));
                ciblePos.Add(new Tuple<Vector3, float>(new Vector3(217, y, 378), 50));
                ciblePos.Add(new Tuple<Vector3, float>(new Vector3(91, y, 360), -30));
                ciblePos.Add(new Tuple<Vector3, float>(new Vector3(152, 0, 451), 0));

                foreach (Tuple<Vector3, float> data in ciblePos)
                    TargetObject.BuildEntity(data.Item1, data.Item2, modelEffect);

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

            EntityManager.Initialize();
            CollisionManager.Initialize();
            TargetManager.Initialize();

            game.ResetElapsedTime();
        }

        #endregion

        public override void Update(GameTime gameTime, bool hasFocus, bool isVisible)
        {
            base.Update(gameTime, hasFocus, isVisible);
            player.Update(gameTime);
            EntityManager.Update(gameTime);
            CollisionManager.Update(gameTime);
            cloudManager.Update(gameTime);

#if DEBUG
            XConsole.Update(gameTime);
#endif
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            player.HandleInput(gameTime, input);

#if DEBUG
            if (input.IsPressed(Keys.D1) || input.IsPressed(Buttons.Start))
                player.Reset();

            DebugConfig.HandleInput(gameTime, input);
#endif
        }

        public override void Draw(GameTime gameTime)
        {
            GameServices.ResetGraphicsDeviceFor3D();

            cloudManager.Draw(gameTime, camera);
            terrain.Draw(camera);
            EntityManager.Draw(gameTime, camera);
            player.Draw(gameTime, camera);

#if DEBUG
            CollisionManager.Draw(gameTime, camera);
            DrawingAxes.Draw(gameTime, camera);
            XConsole.DrawHUD(gameTime);
#endif

            EntityManager.DrawHUD(gameTime);
            player.DrawHUD(gameTime);
        }
    }
}
