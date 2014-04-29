using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine;

namespace Troma
{
    public class SoloScreen : GameScreen
    {
        #region Fields

        private FirstPersonView camera;
        private Player player;
        private ITerrain terrain;

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

            EntityManager.Clear();
            CollisionManager.Clear();

            if (_mapName == "Farm")
            {
                float y = 15;

                camera = new FirstPersonView(game.GraphicsDevice.Viewport.AspectRatio);
                player = new Player(new Vector3(80, 20, 460), Vector3.Zero, camera);

                Effect terrainEffect = FileManager.Load<Effect>("Effects/Terrain");
                Texture2D terrainTexture = FileManager.Load<Texture2D>("Terrains/texture");
                Texture2D terrainHeighmap = FileManager.Load<Texture2D>("Terrains/heightmap");

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
                player.Initialize(terrain);

                Effect modelEffect = FileManager.Load<Effect>("Effects/GameObject");

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

            EntityManager.Initialize();
            CollisionManager.Initialize();

#if DEBUG
            DrawingAxes.Initialize();
#endif

            game.ResetElapsedTime();
        }

        #endregion

        public override void Update(GameTime gameTime, bool hasFocus, bool isVisible)
        {
            base.Update(gameTime, hasFocus, isVisible);
            player.Update(gameTime);
            EntityManager.Update(gameTime);
            CollisionManager.Update(gameTime);
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            player.HandleInput(gameTime, input);
        }

        public override void Draw(GameTime gameTime)
        {
            GameServices.ResetGraphicsDeviceFor3D();
            terrain.Draw(camera);
            EntityManager.Draw(gameTime, camera);

#if DEBUG
            CollisionManager.Draw(gameTime, camera);
            DrawingAxes.Draw(camera);
#endif

            EntityManager.DrawHUD(gameTime);
        }
    }
}
