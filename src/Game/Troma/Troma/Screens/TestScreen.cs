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
        private HeightMap terrain;

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

            camera = new FirstPersonView(game.GraphicsDevice.Viewport.AspectRatio);
            player = new Player(new Vector3(5, 15, 5), Vector3.Zero, camera);

            Effect terrainEffect = FileManager.Load<Effect>("Effects/Terrain");
            Texture2D terrainTexture = FileManager.Load<Texture2D>("Terrains/texture");
            Texture2D terrainHeighmap = FileManager.Load<Texture2D>("Terrains/heightmap");

            TerrainInfo terrainInfo = new TerrainInfo()
            {
                Position = Vector3.Zero,
                Size = new Size(512, 512),
                Depth = 8,
                Texture = terrainTexture,
                TextureScale = 512,
                Heighmap = terrainHeighmap
            };

            terrain = new HeightMap(game, terrainEffect, terrainInfo);
            player.Initialize(terrain);

            game.ResetElapsedTime();
        }

        #endregion

        public override void Update(GameTime gameTime, bool hasFocus, bool isVisible)
        {
            base.Update(gameTime, hasFocus, isVisible);
            player.Update(gameTime);
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            player.HandleInput(gameTime, input);
        }

        public override void Draw(GameTime gameTime)
        {
            GameServices.ResetGraphicsDeviceFor3D();
            terrain.Draw(camera);
        }
    }
}
