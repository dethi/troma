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
            BoundingSphereRenderer.Initialize(30);
#endif

            EntityManager.Clear();
            CollisionManager.Clear();

            camera = new FirstPersonView(game.GraphicsDevice.Viewport.AspectRatio);
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
                TextureScale = 512,
                Heighmap = terrainHeighmap
            };

            float y = 0;

            terrain = new HeightMap(game, terrainEffect, terrainInfo);
            player.Initialize(terrain);

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

            Effect modelEffect = FileManager.Load<Effect>("Effects/GameObject");
            VectGameObject.BuildEntity(modelPos.ToArray(), "rail", modelEffect);
            GameObject.BuildEntity(new Vector3(0, y, 117), "gare", modelEffect.Clone());
            GameObject.BuildEntity(new Vector3(0, y, 220), "quai", modelEffect.Clone());
            GameObject.BuildEntity(new Vector3(460, y, 153), "garde_passage_a_niveau", modelEffect.Clone());
            GameObject.BuildEntity(new Vector3(435, y, 192), "barriere_train_droite", modelEffect.Clone());
            GameObject.BuildEntity(new Vector3(435, y, 222), "barriere_train_gauche", modelEffect.Clone());
            GameObject.BuildEntity(new Vector3(300, y, 300), "eglise", modelEffect.Clone());

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

            if (input.IsPressed(Keys.D1) || input.IsPressed(Buttons.Start))
                player.Reset();
        }

        public override void Draw(GameTime gameTime)
        {
            GameServices.ResetGraphicsDeviceFor3D();
            terrain.Draw(camera);
            EntityManager.Draw(gameTime, camera);
            player.Draw(gameTime, camera);

#if DEBUG
            CollisionManager.Draw(gameTime, camera);
            DrawingAxes.Draw(gameTime, camera);
            XConsole.DrawHUD(gameTime);
#endif

            EntityManager.DrawHUD(gameTime);
        }
    }
}
