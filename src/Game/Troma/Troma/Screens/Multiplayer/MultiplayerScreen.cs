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
        private string host;

        private FirstPersonView camera;
        private Player player;

        private float pauseAlpha;

        private string notifMsg;

        private Texture2D target;
        private SpriteFont Font;
        private Vector2 titleOrigin = new Vector2(0, 0);
        private Color c = new Color(170, 170, 170);

        private List<OtherPlayer> currentList;

        #endregion

        #region Initialization

        public MultiplayerScreen(Game game, string host)
            : base(game)
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            this.host = host;
            currentList = new List<OtherPlayer>();
        }

        public override void LoadContent()
        {
            base.LoadContent();

            client = new GameClient();
            client.ScoreChanged += ScoreChanged;
            client.EndedGame += EndedGame;
            client.Join(host);

            if (client.Connected)
            {
                #region HUD

                target = FileManager.Load<Texture2D>("Menus/target");
                Font = FileManager.Load<SpriteFont>("Fonts/HUD");
                notifMsg = "";

                #endregion

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

                game.ResetElapsedTime();
                client.Start();
            }
        }

        #endregion

        public override void Update(GameTime gameTime, bool hasFocus, bool isVisible)
        {
            base.Update(gameTime, hasFocus, isVisible);
            currentList.Clear();
            currentList.AddRange(client.Players);

            if (!client.Connected && client.Scoring == null && IsActive)
                QuitGame(ErrorType.ConnectFailed);
            else
            {
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

                foreach (OtherPlayer p in currentList)
                    p.Update(gameTime);
            }
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (!client.Connected)
                return;

            if (input.IsPressed(Keys.P) || input.IsPressed(Buttons.Start) ||
                input.IsPressed(Buttons.Back) || input.IsPressed(Keys.Escape))
            {
                ScreenManager.AddScreen(new InGameMenuMulti(game, this));
            }
            else
            {
                player.HandleInput(gameTime, input);

                if (player.Alive)
                {
                    if (!client.Alive)
                        player.Killed();
                    else
                    {
                        client.SetData(player.GetState(), player.GetInput());

                        if (player.HasShoot)
                            client.SendShoot();
                    }
                }
                else if (client.Alive)
                    player.Spawn(client.State);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (!client.Connected)
                return;

            GameServices.ResetGraphicsDeviceFor3D();
            Scene.Draw(gameTime, camera);
            player.Draw(gameTime, camera);

            foreach (OtherPlayer p in currentList)
                p.Draw(gameTime, camera);

#if DEBUG
            CollisionManager.Draw(gameTime, camera);
            DrawingAxes.Draw(gameTime, camera);
            XConsole.DrawHUD(gameTime);
#endif

            if (IsActive)
            {
                player.DrawHUD(gameTime);

                #region HUD

                int width = GameServices.GraphicsDevice.Viewport.Width;
                int height = GameServices.GraphicsDevice.Viewport.Height;

                string nbrClient = (client.Players.Count + 1).ToString();

                float textScale1 = 0.00080f * width;
                float textScale2 = 0.00086f * width;

                Vector2 Position1 = new Vector2(
                    1700 * width / 1920,
                    20 * height / 1080);

                Vector2 Position2 = new Vector2(
                    1698 * width / 1920,
                    18 * height / 1080);

                Vector2 Position3 = new Vector2(
                    70 * width / 1920,
                    20 * height / 1080);

                Rectangle targetImage = new Rectangle(
                    1770 * width / 1920,
                    20 * height / 1080,
                    65 * width / 1920,
                    78 * width / 1920);

                Vector2 notifPos = new Vector2(
                    900 * width / 1920,
                    20 * height / 1080);

                GameServices.SpriteBatch.Begin();

                GameServices.SpriteBatch.DrawString(Font, nbrClient, Position2,
                    Color.Black * 0.3f, 0, titleOrigin, textScale2, SpriteEffects.None, 0);

                GameServices.SpriteBatch.DrawString(Font, nbrClient, Position1, c, 0,
                    titleOrigin, textScale1, SpriteEffects.None, 0);

                GameServices.SpriteBatch.Draw(target, targetImage, Color.White * 0.8f);

                GameServices.SpriteBatch.DrawString(Font, String.Format("{0}/{1}", client.Score, client.MaxScore),
                    Position3, c, 0, titleOrigin, textScale1, SpriteEffects.None, 0);

                GameServices.SpriteBatch.DrawString(Font, notifMsg,
                    notifPos, Color.LimeGreen, 0, titleOrigin, textScale1, SpriteEffects.None, 0);

                GameServices.SpriteBatch.End();

                #endregion
            }

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);
                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }

        #region Event

        public void ScoreChanged(object o, EventArgs e)
        {
            notifMsg += "+100\n";
            TimerManager.Add(1500, CleanMessage);
        }

        public void CleanMessage(object o, EventArgs e)
        {
            notifMsg = "";
        }

        public void EndedGame(object o, EventArgs e)
        {
            //ScreenManager.AddScreen(new EndGameScreen(game, time, _initialNbTarget, player.MunitionUsed()));
        }

        #endregion

        public void QuitGame(ErrorType error)
        {
            client.Shutdown();
            Troma.KillServer();
            LoadingScreen.Load(game, ScreenManager, false, new MainMenu(game),
                new ConnectOrHost(game, error));
        }
    }
}
