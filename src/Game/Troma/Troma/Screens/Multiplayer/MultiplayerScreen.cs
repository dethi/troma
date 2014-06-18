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
    public class MultiplayerScreen : GameScreen
    {
        #region Fields

        private GameClient client;

        private FirstPersonView camera;
        private Player player;

        private TimeSpan time;

        private float pauseAlpha;

        #endregion

        #region Initialization

        public MultiplayerScreen(Game game, string host)
            : base(game)
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            client = new GameClient(host);
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

            camera = new FirstPersonView(GameServices.GraphicsDevice.Viewport.AspectRatio);
            player = new Player(client.State.Position, client.State.Rotation, camera);

            Scene.Initialize(client.Terrain, camera);

            player.Initialize(Scene.Terrain,
                WeaponObject.BuildEntity(Constants.GarandM1),
                WeaponObject.BuildEntity(Constants.ColtM1911));

            System.Threading.Thread.Sleep(500);

            time = new TimeSpan();
            game.ResetElapsedTime();

            client.Start();
        }

        #endregion

        public override void Update(GameTime gameTime, bool hasFocus, bool isVisible)
        {
            base.Update(gameTime, hasFocus, isVisible);

            Scene.Update(gameTime,
                (Settings.DynamicClouds || ScreenState == ScreenState.TransitionOn),
                IsActive);

            if (!IsActive)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
            {
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);
                player.Update(gameTime);
            }
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (input.IsPressed(Keys.P) || input.IsPressed(Buttons.Start) ||
                input.IsPressed(Buttons.Back) || input.IsPressed(Keys.Escape))
            {
                ScreenManager.AddScreen(new InGameMenu(game));
            }
            else
            {
                time += gameTime.ElapsedGameTime;
                player.HandleInput(gameTime, input);

                if (player.Alive)
                {
                    if (!client.Alive)
                        player.Killed();
                    else
                    {
                        if (player.HasShoot)
                            client.SendShoot();

                        client.SetData(player.GetState(), player.GetInput());
                    }
                }
                else if (client.Alive)
                    player.Spawn(client.State);
            }

            foreach (OtherPlayer p in client.Players)
                p.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {


            GameServices.ResetGraphicsDeviceFor3D();
            Scene.Draw(gameTime, camera);
            player.Draw(gameTime, camera);

            foreach (OtherPlayer p in client.Players)
                p.Draw(gameTime, camera);

#if DEBUG
            CollisionManager.Draw(gameTime, camera);
            DrawingAxes.Draw(gameTime, camera);
            XConsole.DrawHUD(gameTime);
#endif

            if (IsActive)
            {
                player.DrawHUD(gameTime);
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
