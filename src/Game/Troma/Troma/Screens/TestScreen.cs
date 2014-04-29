using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine;

namespace Troma
{
    public class TestScreen : GameScreen
    {
        #region Fields

        private FirstPersonView camera;
        private Player player;
        private ITerrain terrain;

        #endregion

        #region Initialization

        public TestScreen(Game game)
            : base(game)
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void LoadContent()
        {
            base.LoadContent();

#if DEBUG
            XConsole.Initialize();
            DrawingAxes.Initialize();
#endif

            EntityManager.Clear();
            CollisionManager.Clear();

            camera = new FirstPersonView(game.GraphicsDevice.Viewport.AspectRatio);
            player = new Player(new Vector3(5, 15, 5), Vector3.Zero, camera);

            Effect terrainEffect = FileManager.Load<Effect>("Effects/Terrain");
            Texture2D terrainTexture = FileManager.Load<Texture2D>("Terrains/texture");
            Texture2D terrainHeighmap = FileManager.Load<Texture2D>("Terrains/heightmap");

            TerrainInfo terrainInfo = new TerrainInfo()
            {
                Position = Vector3.Zero,
                Size = new Size(513, 513),
                Depth = 8,
                Texture = terrainTexture,
                TextureScale = 512,
                Heighmap = terrainHeighmap
            };

            terrain = new HeightMap(game, terrainEffect, terrainInfo);
            player.Initialize(terrain);

            List<Vector3> modelPos = new List<Vector3>();
            modelPos.Add(new Vector3(100, 8, 100));
            modelPos.Add(new Vector3(100, 8, 80));
            modelPos.Add(new Vector3(100, 8, 60));
            modelPos.Add(new Vector3(100, 8, 40));
            modelPos.Add(new Vector3(100, 8, 20));

            Effect modelEffect = FileManager.Load<Effect>("Effects/GameObject");
            GameObject.BuildEntity(modelPos[0], "hotel", modelEffect);

            EntityManager.Initialize();
            CollisionManager.Initialize();

            game.ResetElapsedTime();
        }

        #endregion

        public override void Update(GameTime gameTime, bool hasFocus, bool isVisible)
        {
            base.Update(gameTime, hasFocus, isVisible);
            player.Update(gameTime);
            EntityManager.Update(gameTime);
            CollisionManager.Update(gameTime);

#if DEBUG
            XConsole.Update(gameTime);
#endif
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
            DrawingAxes.Draw(gameTime, camera);
            XConsole.DrawHUD(gameTime);
#endif

            EntityManager.DrawHUD(gameTime);
        }
    }
}
