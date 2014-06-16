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

        private TimeSpan time;
        private int _initialNbTarget;

        private float pauseAlpha;

        private Texture2D target;
        private Texture2D clock;
        private SpriteFont Font;
        private Vector2 titleOrigin = new Vector2(0, 0);
        private Color c = new Color(170, 170, 170);

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
            XConsole.Reset();
            XConsole.Initialize();
            DrawingAxes.Initialize();
            BoundingSphereRenderer.Initialize(30);
#endif

            EntityManager.Clear();
            CollisionManager.Clear();
            TargetManager.Clear();

            camera = new FirstPersonView(game.GraphicsDevice.Viewport.AspectRatio);
            player = new Player(new Vector3(10, 0, 10), Vector3.Zero, camera);

            Effect terrainEffect = FileManager.Load<Effect>("Effects/Terrain");
            Texture2D terrainTexture = FileManager.Load<Texture2D>("Terrains/texture");
            Texture2D terrainHeighmap = FileManager.Load<Texture2D>("Terrains/heightmap");

            target = FileManager.Load<Texture2D>("Menus/target");
            clock = FileManager.Load<Texture2D>("Menus/Clock");
            Font = FileManager.Load<SpriteFont>("Fonts/HUD");

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
            modelWithNormal.Name = "GameObjectWithNormal";

            player.Initialize(terrain, WeaponObject.BuildEntity(Constants.GarandM1), WeaponObject.BuildEntity(Constants.ColtM1911));

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

            EntityManager.Initialize();
            CollisionManager.Initialize();
            TargetManager.Initialize();

            _initialNbTarget = TargetManager.Count;

            time = new TimeSpan();
            game.ResetElapsedTime();
        }

        #endregion

        public override void Update(GameTime gameTime, bool hasFocus, bool isVisible)
        {
            base.Update(gameTime, hasFocus, isVisible);

            if (Settings.DynamicClouds || ScreenState == ScreenState.TransitionOn)
                cloudManager.Update(gameTime);

            if (!IsActive)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
            {
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

                if (TargetManager.Count == 0)
                    ScreenManager.AddScreen(new EndGameScreen(game, time, _initialNbTarget, player.MunitionUsed()));
                else
                {
                    player.Update(gameTime);
                    EntityManager.Update(gameTime);
                    CollisionManager.Update(gameTime);

#if DEBUG
                    XConsole.Update(gameTime);
#endif
                }
            }
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (input.IsPressed(Keys.P) || input.IsPressed(Buttons.Start))
                ScreenManager.AddScreen(new InGameMenu(game));
            else
            {
                time += gameTime.ElapsedGameTime;
                player.HandleInput(gameTime, input);

#if DEBUG
                if (input.IsPressed(Keys.D1) || input.IsPressed(Buttons.B))
                    player.Reset();

                DebugConfig.HandleInput(gameTime, input);
#endif
            }
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
            if (IsActive)
            {
                EntityManager.DrawHUD(gameTime);
                player.DrawHUD(gameTime);

                if (TargetManager.Count > 0)
                {
                    int width = GameServices.GraphicsDevice.Viewport.Width;
                    int height = GameServices.GraphicsDevice.Viewport.Height;

                    string timeTotal = String.Format("{0:D2}:{1:D2}", time.Minutes, time.Seconds);
                    string nbreTarget = TargetManager.Count.ToString();

                    float textScale1 = 0.00080f * width;
                    float textScale2 = 0.00086f * width;

                    Vector2 Position1 = new Vector2(
                    1700 * width / 1920,
                    20 * height / 1080);

                    Vector2 Position2 = new Vector2(
                    1698 * width / 1920,
                    18 * height / 1080);

                    Vector2 Position3 = new Vector2(
                    170 * width / 1920,
                    20 * height / 1080);

                    Rectangle targetImage = new Rectangle(
                        1770 * width / 1920,
                        20 * height / 1080,
                        65 * width / 1920,
                        78 * width / 1920);

                    Rectangle clockImage = new Rectangle(
                        70 * width / 1920,
                        20 * height / 1080,
                        80 * width / 1920,
                        80 * width / 1920);

                    GameServices.SpriteBatch.Begin();

                    GameServices.SpriteBatch.DrawString(Font, nbreTarget, Position2, Color.Black * 0.3f, 0, titleOrigin, textScale2, SpriteEffects.None, 0);
                    GameServices.SpriteBatch.DrawString(Font, nbreTarget, Position1, c, 0, titleOrigin, textScale1, SpriteEffects.None, 0);
                    GameServices.SpriteBatch.Draw(target, targetImage, Color.White * 0.8f);

                    GameServices.SpriteBatch.DrawString(Font, timeTotal, Position3, c, 0, titleOrigin, textScale1, SpriteEffects.None, 0);
                    GameServices.SpriteBatch.Draw(clock, clockImage, Color.White * 0.8f);

                    GameServices.SpriteBatch.End();
                }
            }

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);
                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }
    }
}
