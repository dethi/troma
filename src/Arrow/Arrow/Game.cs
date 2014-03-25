using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Arrow
{
    public partial class Game : Microsoft.Xna.Framework.Game
    {
        public GraphicsDeviceManager graphics;

        private SpriteBatch spriteBatch;
        private Texture2D cross;

        public Player player { get; private set; }
        private InputState input;

        public HeightMap map { get; private set; }
        private Effect mapEffect;

        private EntityManager entities;
        private Entity skydome;

        #if EDITOR_MODE

        private EntityPos entityPos;
        private int currentEntity;
        private int maxEntity;
        private DisplayPosObject HUDPosObj;
        
        #endif

        private MenuPause menuPause;
        private MenuStart menuStart;

        public Game()
        {
            this.graphics = new GraphicsDeviceManager(this);
            //ActivateFullScreen();
            //DisableVsync();

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Camera camera = Camera.Instance;
            camera.New(this, Vector3.Zero, Vector3.Zero);

            input = new InputState(new Vector2(GraphicsDevice.Viewport.Width / 2, 
                GraphicsDevice.Viewport.Height / 2));

            player = new Player(this, input, new Vector3(80, 10, 460));

            #if !BUILD

            Components.Add(new FPS(this));
            Components.Add(new DisplayPosition(this));
            Components.Add(new MemoryUse(this));

            #endif

            entities = new EntityManager(this);

            #if EDITOR_MODE

            HUDPosObj = new DisplayPosObject(this);
            Components.Add(HUDPosObj);
            
            entityPos = new EntityPos(input);

            #endif

            menuPause = new MenuPause(this);
            menuPause.Initialize();

            menuStart = new MenuStart(this);
            menuStart.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            cross = Content.Load<Texture2D>("Cross");
            
            #region Map

            map = new HeightMap(this,
                Content.Load<Texture2D>("Textures/Ferme/heightmap"),
                Content.Load<Texture2D>("Textures/Ferme/texture"),
                513f,
                513,
                513,
                20f);

            mapEffect = Content.Load<Effect>("Effects/Terrain");

            #endregion

            #region Skydome

            skydome = new Entity(this, "skydome", new Vector2(256, 256));
            skydome.lightingEnabled = false;

            #endregion

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
            //mapObject.AddModel("antitank", new Vector2(50, 128));
            entities.AddEntity("table", new Vector2(58, 400));
            entities.AddEntity("barrel", new Vector2(58, 405));
            //mapObject.AddModel("soldier", new Vector2(250, 250));
            //mapObject.AddModel("cible_homme", new Vector2(300, 300));
            entities.AddEntity("truck_allemand", new Vector2(122, 206));
            //mapObject.AddModel("truck_allemand_casse", new Vector2(40, 355));
            entities.AddEntity("farm", new Vector2(0, 262));

            #endregion

            #if EDITOR_MODE

            currentEntity = 0;
            maxEntity = entities.Entities.Count - 1;
            HUDPosObj.AssociateModel(entities);

            #endif

            #region Sound

            SFXManager.AddSFX("Walk", Content.Load<SoundEffect>("Sounds/Walk"));
            SFXManager.AddSFX("Run", Content.Load<SoundEffect>("Sounds/Run"));

            #endregion

            #region Menu

            menuPause.LoadContent();
            menuStart.LoadContent();

            #endregion
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            input.Update();
            
            #if EDITOR_MODE

            entityPos.Change_i(ref currentEntity, maxEntity);
            entities.MoveModel(new Vector4(
                entityPos.Change_x_z(entities.Entities.ElementAt(currentEntity).Value.position.Translation),
                currentEntity));
            HUDPosObj.Upieme(currentEntity);

            #endif

            if (menuStart.GameStart)
            {
                if (!menuPause.DisplayMenu)
                    player.Update(gameTime, map);
                menuPause.Update(gameTime);
            }
            else
                menuStart.Update(gameTime);
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkOliveGreen);

            if (menuStart.GameStart)
            {
                map.Draw(mapEffect);
                skydome.Draw();
                entities.Draw();
                player.Draw();

                //
                // Display the cross in the center of the screen
                //
                #region Cross

                spriteBatch.Begin();
                spriteBatch.Draw(cross,
                    new Vector2((GraphicsDevice.Viewport.Width / 2) - 8, 
                        (GraphicsDevice.Viewport.Height / 2) - 8),
                    Color.White);
                spriteBatch.End();

                #endregion

                menuPause.Draw(gameTime);
            }
            else
                menuStart.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
