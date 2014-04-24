using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Arrow
{
    public class GameplayScreen : GameScreen
    {
        #region Fields

        private ContentManager content;

        private Camera camera;
        private Player player;

        private MapManager maps;
        private Effect mapEffect;

        private Entity skydome;

        private EntityManager entities;
        private Texture2D cross;

        private string carte;

        private float pauseAlpha;

        #endregion

        #region Initialization

        public GameplayScreen(Game game)
            : base(game)
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            camera = Camera.Instance;
            player = new Player(game, new Vector3(800, 10, 600));
            carte = "Town";
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            camera.New(game, Vector3.Zero, Vector3.Zero);
            player.Position = new Vector3(800, 10, 600);

            cross = content.Load<Texture2D>("Cross");

            mapEffect = content.Load<Effect>("Effects/Terrain");
            maps = new MapManager(game,
                "Textures/" + carte + "/heightmap",
                "Textures/" + carte + "/texture",
                32f,
                513,
                513,
                20f,
                3,
                3);

            entities = new EntityManager(game, maps);

            if (carte == "Farm")
            {
                
                skydome = new Entity(game, "skydome", new Vector3(
                    256,
                    maps.GetHeight(256, 256).Value,
                    256));
                skydome.lightingEnabled = false;

                #region Models

                #region Wood barrier

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

                #endregion

                entities.AddEntity("Farm/house", new Vector2(0, 511));
                entities.AddEntity("Farm/barn", new Vector2(60, 511));
                entities.AddEntity("Farm/bandbags", new Vector2(150, 150));
                entities.AddEntity("Farm/table", new Vector2(58, 400));
                entities.AddEntity("Farm/barrel", new Vector2(58, 405));
                entities.AddEntity("Farm/truck_allemand", new Vector2(122, 206));
                entities.AddEntity("Farm/farm", new Vector2(0, 262));
                entities.AddEntity("Farm/shelter", new Vector2(0, 411));

                #endregion

                ScreenManager.Game.ResetElapsedTime();
            }
            else
            {
                entities.AddEntity("Town/gare", new Vector2(768, 685));
                entities.AddEntity("Town/quai", new Vector2(768, 813));
                entities.AddEntity("Town/eglise", new Vector2(1008, 445));
                entities.AddEntity("Town/mairie", new Vector2(1008, 565));
                entities.AddEntity("Town/poste", new Vector2(728, 525));
                entities.AddEntity("Town/immeuble", new Vector2(768, 405));
                entities.AddEntity("Town/hotel", new Vector2(728, 645));
                entities.AddEntity("Town/fontaine", new Vector2(848, 565));
                entities.AddEntity("Town/barriere_train_gauche", new Vector2(1120, 760));
                entities.AddEntity("Town/barriere_train_droite", new Vector2(1140, 800));
                entities.AddEntity("Town/garde_passage_a_niveau", new Vector2(1165, 840));

                entities.AddEntity("Town/immeuble2", new Vector2(728, 405));
                entities.AddEntity("Town/immeuble2", new Vector2(608, 405));

                entities.AddEntity("Town/immeuble3", new Vector2(848, 1000));
                entities.AddEntity("Town/immeuble3", new Vector2(968, 1000));

                entities.AddEntity("Town/maison", new Vector2(1160, 525));
                entities.AddEntity("Town/maison", new Vector2(1160, 405));
                entities.AddEntity("Town/maison", new Vector2(1160, 645));

                entities.AddEntity("Town/cimetiere", new Vector2(1100, 1100));

                Model batiment1X = content.Load<Model>("Models/Town/batiment2");
                entities.AddEntity(batiment1X, new Vector2(608, 525));
                //entities.AddEntity(batiment1X, new Vector2(728, 645));
                entities.AddEntity(batiment1X, new Vector2(608, 645));

                entities.AddEntity("Town/batiment3", new Vector2(1128, 405));

                #region tombe
                entities.AddEntity("Town/tombe", new Vector2(1110, 1110));
                #endregion

                #region rail
                Model railX = content.Load<Model>("Models/Town/rail");
                entities.AddEntity(railX, new Vector2(0, 775));
                entities.AddEntity(railX, new Vector2(120, 775));
                entities.AddEntity(railX, new Vector2(240, 775));
                entities.AddEntity(railX, new Vector2(360, 775));
                entities.AddEntity(railX, new Vector2(480, 775));
                entities.AddEntity(railX, new Vector2(600, 775));
                entities.AddEntity(railX, new Vector2(720, 775));
                entities.AddEntity(railX, new Vector2(840, 775));
                entities.AddEntity(railX, new Vector2(960, 775));
                entities.AddEntity(railX, new Vector2(1080, 775));
                entities.AddEntity(railX, new Vector2(1200, 775));
                entities.AddEntity(railX, new Vector2(1320, 775));
                entities.AddEntity(railX, new Vector2(1440, 775));
                entities.AddEntity(railX, new Vector2(1560, 775));
                entities.AddEntity(railX, new Vector2(1680, 775));

                entities.AddEntity(railX, new Vector2(0, 790));
                entities.AddEntity(railX, new Vector2(120, 790));
                entities.AddEntity(railX, new Vector2(240, 790));
                entities.AddEntity(railX, new Vector2(360, 790));
                entities.AddEntity(railX, new Vector2(480, 790));
                entities.AddEntity(railX, new Vector2(600, 790));
                entities.AddEntity(railX, new Vector2(720, 790));
                entities.AddEntity(railX, new Vector2(840, 790));
                entities.AddEntity(railX, new Vector2(960, 790));
                entities.AddEntity(railX, new Vector2(1080, 790));
                entities.AddEntity(railX, new Vector2(1200, 790));
                entities.AddEntity(railX, new Vector2(1320, 790));
                entities.AddEntity(railX, new Vector2(1440, 790));
                entities.AddEntity(railX, new Vector2(1560, 790));
                entities.AddEntity(railX, new Vector2(1680, 790));

                #endregion
            }
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
            //skydome.Draw();
            entities.Draw();
            player.Draw();

            //
            // Display the cross in the center of the screen
            //
            #region Cross

            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(
                cross,
                new Vector2((ScreenManager.GraphicsDevice.Viewport.Width / 2) - 8,
                    (ScreenManager.GraphicsDevice.Viewport.Height / 2) - 8),
                Color.White);
            ScreenManager.SpriteBatch.End();

            #endregion
        }
    }
}
