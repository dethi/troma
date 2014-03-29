using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Arrow
{
    class GameplayScreen : GameScreen
    {
        #region Fields

        private ContentManager content;
        private SpriteBatch spriteBatch;

        private Camera camera;
        private Player player;

        private MapManager maps;
        private Effect mapEffect;

        private Entity skydome;

        private EntityManager entities;
        private Texture2D cross;

        private float pauseAlpha;

        #endregion

        #region Initialization

        public GameplayScreen(Game game)
            : base(game)
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            camera = Camera.Instance;
            player = new Player(game, new Vector3(80, 10, 460));
        }

        /// <summary>
        /// Load graphics content for the game
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            camera.New(game, Vector3.Zero, Vector3.Zero);
            player.Position = new Vector3(80, 10, 460);

            spriteBatch = new SpriteBatch(ScreenManager.GraphicsDevice);
            cross = content.Load<Texture2D>("Cross");

            mapEffect = content.Load<Effect>("Effects/Terrain");
            maps = new MapManager(game,
                "Textures/Farm/heightmap",
                "Textures/Farm/texture",
                513f,
                513,
                513,
                20f,
                3,
                3);

            entities = new EntityManager(game, maps);

            skydome = new Entity(game, "skydome", new Vector3(
                256, 
                maps.GetHeight(256, 256).Value, 
                256));
            skydome.lightingEnabled = false;

            #region Models

            #region wood_barrier

            entities.AddEntity("shelter", new Vector2(0, 411));
            entities.AddEntity("wood_barrier/wood_barrier_0", new Vector2(119, 101));
            entities.AddEntity("wood_barrier/wood_barrier_1", new Vector2(119, 91));
            entities.AddEntity("wood_barrier/wood_barrier_2", new Vector2(119, 81));
            entities.AddEntity("wood_barrier/wood_barrier_3", new Vector2(119, 71));
            entities.AddEntity("wood_barrier/wood_barrier_4", new Vector2(119, 61));
            entities.AddEntity("wood_barrier/wood_barrier_5", new Vector2(119, 51));
            entities.AddEntity("wood_barrier/wood_barrier_6", new Vector2(119, 41));
            entities.AddEntity("wood_barrier/wood_barrier_7", new Vector2(119, 31));
            entities.AddEntity("wood_barrier/wood_barrier_8", new Vector2(119, 21));
            entities.AddEntity("wood_barrier/wood_barrier_9", new Vector2(119, 11));
            entities.AddEntity("wood_barrier/wood_barrier_10", new Vector2(119, 1));
            entities.AddEntity("wood_barrier/wood_barrier_11", new Vector2(109, 1));
            entities.AddEntity("wood_barrier/wood_barrier_12", new Vector2(99, 1));
            entities.AddEntity("wood_barrier/wood_barrier_13", new Vector2(89, 1));
            entities.AddEntity("wood_barrier/wood_barrier_14", new Vector2(79, 1));
            entities.AddEntity("wood_barrier/wood_barrier_15", new Vector2(69, 1));
            entities.AddEntity("wood_barrier/wood_barrier_16", new Vector2(59, 1));
            entities.AddEntity("wood_barrier/wood_barrier_17", new Vector2(49, 1));
            entities.AddEntity("wood_barrier/wood_barrier_18", new Vector2(39, 1));
            entities.AddEntity("wood_barrier/wood_barrier_19", new Vector2(29, 1));
            entities.AddEntity("wood_barrier/wood_barrier_20", new Vector2(19, 1));
            entities.AddEntity("wood_barrier/wood_barrier_21", new Vector2(9, 1));
            entities.AddEntity("wood_barrier/wood_barrier_22", new Vector2(0, 1));

            #endregion

            #region barbed_barrier

            entities.AddEntity("barbed_barrier/barbed_barrier_0", new Vector2(160, 0));
            entities.AddEntity("barbed_barrier/barbed_barrier_1", new Vector2(160, 9));
            entities.AddEntity("barbed_barrier/barbed_barrier_2", new Vector2(160, 18));
            entities.AddEntity("barbed_barrier/barbed_barrier_3", new Vector2(160, 27));
            entities.AddEntity("barbed_barrier/barbed_barrier_4", new Vector2(160, 36));
            entities.AddEntity("barbed_barrier/barbed_barrier_5", new Vector2(160, 45));
            entities.AddEntity("barbed_barrier/barbed_barrier_6", new Vector2(160, 54));
            entities.AddEntity("barbed_barrier/barbed_barrier_7", new Vector2(160, 63));
            entities.AddEntity("barbed_barrier/barbed_barrier_8", new Vector2(160, 72));
            entities.AddEntity("barbed_barrier/barbed_barrier_9", new Vector2(160, 81));
            entities.AddEntity("barbed_barrier/barbed_barrier_10", new Vector2(160, 90));
            entities.AddEntity("barbed_barrier/barbed_barrier_11", new Vector2(160, 99));
            entities.AddEntity("barbed_barrier/barbed_barrier_12", new Vector2(160, 108));
            entities.AddEntity("barbed_barrier/barbed_barrier_13", new Vector2(160, 117));
            entities.AddEntity("barbed_barrier/barbed_barrier_14", new Vector2(160, 126));
            entities.AddEntity("barbed_barrier/barbed_barrier_15", new Vector2(160, 135));
            entities.AddEntity("barbed_barrier/barbed_barrier_16", new Vector2(160, 144));
            entities.AddEntity("barbed_barrier/barbed_barrier_17", new Vector2(160, 153));
            entities.AddEntity("barbed_barrier/barbed_barrier_18", new Vector2(160, 162));
            entities.AddEntity("barbed_barrier/barbed_barrier_19", new Vector2(160, 171));
            entities.AddEntity("barbed_barrier/barbed_barrier_20", new Vector2(160, 180));
            entities.AddEntity("barbed_barrier/barbed_barrier_21", new Vector2(160, 189));
            entities.AddEntity("barbed_barrier/barbed_barrier_22", new Vector2(160, 198));
            entities.AddEntity("barbed_barrier/barbed_barrier_23", new Vector2(160, 207));
            entities.AddEntity("barbed_barrier/barbed_barrier_24", new Vector2(160, 216));
            entities.AddEntity("barbed_barrier/barbed_barrier_25", new Vector2(160, 225));
            entities.AddEntity("barbed_barrier/barbed_barrier_26", new Vector2(160, 234));
            entities.AddEntity("barbed_barrier/barbed_barrier_27", new Vector2(160, 242));
            entities.AddEntity("barbed_barrier/barbed_barrier_28", new Vector2(160, 251));
            entities.AddEntity("barbed_barrier/barbed_barrier_29", new Vector2(160, 260));
            entities.AddEntity("barbed_barrier/barbed_barrier_30", new Vector2(160, 269));
            entities.AddEntity("barbed_barrier/barbed_barrier_31", new Vector2(160, 278));
            entities.AddEntity("barbed_barrier/barbed_barrier_32", new Vector2(160, 287));
            entities.AddEntity("barbed_barrier/barbed_barrier_33", new Vector2(160, 296));
            entities.AddEntity("barbed_barrier/barbed_barrier_34", new Vector2(160, 305));
            entities.AddEntity("barbed_barrier/post_0", new Vector2(160, 315));

            entities.AddEntity("barbed_barrier/post_1", new Vector2(160, 340));
            entities.AddEntity("barbed_barrier/barbed_barrier_35", new Vector2(160, 349));
            entities.AddEntity("barbed_barrier/barbed_barrier_36", new Vector2(160, 358));
            entities.AddEntity("barbed_barrier/barbed_barrier_37", new Vector2(160, 367));
            entities.AddEntity("barbed_barrier/barbed_barrier_38", new Vector2(160, 376));
            entities.AddEntity("barbed_barrier/barbed_barrier_39", new Vector2(160, 385));
            entities.AddEntity("barbed_barrier/barbed_barrier_40", new Vector2(160, 394));
            entities.AddEntity("barbed_barrier/barbed_barrier_41", new Vector2(160, 403));
            entities.AddEntity("barbed_barrier/barbed_barrier_42", new Vector2(160, 412));
            entities.AddEntity("barbed_barrier/barbed_barrier_43", new Vector2(160, 421));
            entities.AddEntity("barbed_barrier/barbed_barrier_44", new Vector2(160, 430));
            entities.AddEntity("barbed_barrier/barbed_barrier_45", new Vector2(160, 439));
            entities.AddEntity("barbed_barrier/barbed_barrier_46", new Vector2(160, 448));
            entities.AddEntity("barbed_barrier/barbed_barrier_47", new Vector2(160, 457));
            entities.AddEntity("barbed_barrier/barbed_barrier_48", new Vector2(160, 466));
            entities.AddEntity("barbed_barrier/barbed_barrier_49", new Vector2(160, 475));
            entities.AddEntity("barbed_barrier/barbed_barrier_50", new Vector2(160, 484));
            entities.AddEntity("barbed_barrier/barbed_barrier_51", new Vector2(160, 493));
            entities.AddEntity("barbed_barrier/barbed_barrier_52", new Vector2(160, 502));
            entities.AddEntity("barbed_barrier/barbed_barrier_53", new Vector2(160, 511));

            #endregion

            entities.AddEntity("house", new Vector2(0, 511));
            entities.AddEntity("barn", new Vector2(60, 511));
            entities.AddEntity("bandbags", new Vector2(150, 150));
            entities.AddEntity("table", new Vector2(58, 400));
            entities.AddEntity("barrel", new Vector2(58, 405));
            entities.AddEntity("truck_allemand", new Vector2(122, 206));
            entities.AddEntity("farm", new Vector2(0, 262));

            #endregion

            ScreenManager.Game.ResetElapsedTime();
        }

        /// <summary>
        /// Unload graphics content used by the game
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }

        #endregion

        public override void Update(GameTime gameTime, bool hasFocus, bool isVisible)
        {
            base.Update(gameTime, hasFocus, isVisible);

            if (!isVisible)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

            if (IsActive)
                player.Update(gameTime, maps);
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            player.HandleInput(gameTime, input, maps);
        }

        public override void Draw(GameTime gameTime)
        {
            maps.Draw(mapEffect);
            skydome.Draw();
            entities.Draw();
            player.Draw();

            //
            // Display the cross in the center of the screen
            //
            #region Cross

            spriteBatch.Begin();
            spriteBatch.Draw(
                cross,
                new Vector2((ScreenManager.GraphicsDevice.Viewport.Width / 2) - 8,
                    (ScreenManager.GraphicsDevice.Viewport.Height / 2) - 8),
                Color.White);
            spriteBatch.End();

            #endregion
        }
    }
}
