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

        public HeightMap map { get; private set; }
        private Effect mapEffect;

        private ModelManager mapObject;
        private GameObject skydome;

        #if EDITOR_MODE

        private MapObjPos mapObjectPos;
        private int minNbObject;
        private int maxNbObject;
        private DisplayPosObject hudPosObject;
        
        #endif

        private MenuPause menuPause;
        private MenuStart menuStart;

        public Game()
        {
            this.graphics = new GraphicsDeviceManager(this);
            ActivateFullScreen();
            //DisableVsync();

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Camera camera = Camera.Instance;
            camera.New(this, Vector3.Zero, Vector3.Zero);

            player = new Player(this, new Vector3(80, 10, 460));

            #if !BUILD

            Components.Add(new FPS(this));
            Components.Add(new DisplayPosition(this));
            Components.Add(new MemoryUse(this));

            #endif

            mapObject = new ModelManager(this);

            #if EDITOR_MODE

            hudPosObject = new DisplayPosObject(this);
            Components.Add(hudPosObject);
            
            mapObjectPos = new MapObjPos();

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

            skydome = new GameObject(this, "skydome", new Vector2(256, 256));
            skydome.lightingEnabled = false;

            #endregion

            #region Models

            #region wood_barrier

            mapObject.AddModel("shelter", new Vector2(0, 411));
            mapObject.AddModel("wood_barrier/wood_barrier_0", new Vector2(119, 101));
            mapObject.AddModel("wood_barrier/wood_barrier_1", new Vector2(119, 91));
            mapObject.AddModel("wood_barrier/wood_barrier_2", new Vector2(119, 81));
            mapObject.AddModel("wood_barrier/wood_barrier_3", new Vector2(119, 71));
            mapObject.AddModel("wood_barrier/wood_barrier_4", new Vector2(119, 61));
            mapObject.AddModel("wood_barrier/wood_barrier_5", new Vector2(119, 51));
            mapObject.AddModel("wood_barrier/wood_barrier_6", new Vector2(119, 41));
            mapObject.AddModel("wood_barrier/wood_barrier_7", new Vector2(119, 31));
            mapObject.AddModel("wood_barrier/wood_barrier_8", new Vector2(119, 21));
            mapObject.AddModel("wood_barrier/wood_barrier_9", new Vector2(119, 11));
            mapObject.AddModel("wood_barrier/wood_barrier_10", new Vector2(119, 1));
            mapObject.AddModel("wood_barrier/wood_barrier_11", new Vector2(109, 1));
            mapObject.AddModel("wood_barrier/wood_barrier_12", new Vector2(99, 1));
            mapObject.AddModel("wood_barrier/wood_barrier_13", new Vector2(89, 1));
            mapObject.AddModel("wood_barrier/wood_barrier_14", new Vector2(79, 1));
            mapObject.AddModel("wood_barrier/wood_barrier_15", new Vector2(69, 1));
            mapObject.AddModel("wood_barrier/wood_barrier_16", new Vector2(59, 1));
            mapObject.AddModel("wood_barrier/wood_barrier_17", new Vector2(49, 1));
            mapObject.AddModel("wood_barrier/wood_barrier_18", new Vector2(39, 1));
            mapObject.AddModel("wood_barrier/wood_barrier_19", new Vector2(29, 1));
            mapObject.AddModel("wood_barrier/wood_barrier_20", new Vector2(19, 1));
            mapObject.AddModel("wood_barrier/wood_barrier_21", new Vector2(9, 1));
            mapObject.AddModel("wood_barrier/wood_barrier_22", new Vector2(0, 1));

            #endregion

            #region barbed_barrier

            mapObject.AddModel("barbed_barrier/barbed_barrier_0", new Vector2(160, 0));
            mapObject.AddModel("barbed_barrier/barbed_barrier_1", new Vector2(160, 9));
            mapObject.AddModel("barbed_barrier/barbed_barrier_2", new Vector2(160, 18));
            mapObject.AddModel("barbed_barrier/barbed_barrier_3", new Vector2(160, 27));
            mapObject.AddModel("barbed_barrier/barbed_barrier_4", new Vector2(160, 36));
            mapObject.AddModel("barbed_barrier/barbed_barrier_5", new Vector2(160, 45));
            mapObject.AddModel("barbed_barrier/barbed_barrier_6", new Vector2(160, 54));
            mapObject.AddModel("barbed_barrier/barbed_barrier_7", new Vector2(160, 63));
            mapObject.AddModel("barbed_barrier/barbed_barrier_8", new Vector2(160, 72));
            mapObject.AddModel("barbed_barrier/barbed_barrier_9", new Vector2(160, 81));
            mapObject.AddModel("barbed_barrier/barbed_barrier_10", new Vector2(160, 90));
            mapObject.AddModel("barbed_barrier/barbed_barrier_11", new Vector2(160, 99));
            mapObject.AddModel("barbed_barrier/barbed_barrier_12", new Vector2(160, 108));
            mapObject.AddModel("barbed_barrier/barbed_barrier_13", new Vector2(160, 117));
            mapObject.AddModel("barbed_barrier/barbed_barrier_14", new Vector2(160, 126));
            mapObject.AddModel("barbed_barrier/barbed_barrier_15", new Vector2(160, 135));
            mapObject.AddModel("barbed_barrier/barbed_barrier_16", new Vector2(160, 144));
            mapObject.AddModel("barbed_barrier/barbed_barrier_17", new Vector2(160, 153));
            mapObject.AddModel("barbed_barrier/barbed_barrier_18", new Vector2(160, 162));
            mapObject.AddModel("barbed_barrier/barbed_barrier_19", new Vector2(160, 171));
            mapObject.AddModel("barbed_barrier/barbed_barrier_20", new Vector2(160, 180));
            mapObject.AddModel("barbed_barrier/barbed_barrier_21", new Vector2(160, 189));
            mapObject.AddModel("barbed_barrier/barbed_barrier_22", new Vector2(160, 198));
            mapObject.AddModel("barbed_barrier/barbed_barrier_23", new Vector2(160, 207));
            mapObject.AddModel("barbed_barrier/barbed_barrier_24", new Vector2(160, 216));
            mapObject.AddModel("barbed_barrier/barbed_barrier_25", new Vector2(160, 225));
            mapObject.AddModel("barbed_barrier/barbed_barrier_26", new Vector2(160, 234));
            mapObject.AddModel("barbed_barrier/barbed_barrier_27", new Vector2(160, 242));
            mapObject.AddModel("barbed_barrier/barbed_barrier_28", new Vector2(160, 251));
            mapObject.AddModel("barbed_barrier/barbed_barrier_29", new Vector2(160, 260));
            mapObject.AddModel("barbed_barrier/barbed_barrier_30", new Vector2(160, 269));
            mapObject.AddModel("barbed_barrier/barbed_barrier_31", new Vector2(160, 278));
            mapObject.AddModel("barbed_barrier/barbed_barrier_32", new Vector2(160, 287));
            mapObject.AddModel("barbed_barrier/barbed_barrier_33", new Vector2(160, 296));
            mapObject.AddModel("barbed_barrier/barbed_barrier_34", new Vector2(160, 305));
            mapObject.AddModel("barbed_barrier/post_0", new Vector2(160, 315));

            mapObject.AddModel("barbed_barrier/post_1", new Vector2(160, 340));
            mapObject.AddModel("barbed_barrier/barbed_barrier_35", new Vector2(160, 349));
            mapObject.AddModel("barbed_barrier/barbed_barrier_36", new Vector2(160, 358));
            mapObject.AddModel("barbed_barrier/barbed_barrier_37", new Vector2(160, 367));
            mapObject.AddModel("barbed_barrier/barbed_barrier_38", new Vector2(160, 376)); 
            mapObject.AddModel("barbed_barrier/barbed_barrier_39", new Vector2(160, 385));
            mapObject.AddModel("barbed_barrier/barbed_barrier_40", new Vector2(160, 394));
            mapObject.AddModel("barbed_barrier/barbed_barrier_41", new Vector2(160, 403));
            mapObject.AddModel("barbed_barrier/barbed_barrier_42", new Vector2(160, 412));
            mapObject.AddModel("barbed_barrier/barbed_barrier_43", new Vector2(160, 421));
            mapObject.AddModel("barbed_barrier/barbed_barrier_44", new Vector2(160, 430));
            mapObject.AddModel("barbed_barrier/barbed_barrier_45", new Vector2(160, 439));
            mapObject.AddModel("barbed_barrier/barbed_barrier_46", new Vector2(160, 448));
            mapObject.AddModel("barbed_barrier/barbed_barrier_47", new Vector2(160, 457));
            mapObject.AddModel("barbed_barrier/barbed_barrier_48", new Vector2(160, 466));
            mapObject.AddModel("barbed_barrier/barbed_barrier_49", new Vector2(160, 475));
            mapObject.AddModel("barbed_barrier/barbed_barrier_50", new Vector2(160, 484));
            mapObject.AddModel("barbed_barrier/barbed_barrier_51", new Vector2(160, 493));
            mapObject.AddModel("barbed_barrier/barbed_barrier_52", new Vector2(160, 502));
            mapObject.AddModel("barbed_barrier/barbed_barrier_53", new Vector2(160, 511));

            #endregion

            mapObject.AddModel("house", new Vector2(0, 511));
            mapObject.AddModel("barn", new Vector2(60, 511));
            mapObject.AddModel("bandbags", new Vector2(150, 150));
            //mapObject.AddModel("antitank", new Vector2(50, 128));
            mapObject.AddModel("table", new Vector2(58, 400));
            mapObject.AddModel("barrel", new Vector2(58, 405));
            //mapObject.AddModel("soldier", new Vector2(250, 250));
            //mapObject.AddModel("cible_homme", new Vector2(300, 300));
            mapObject.AddModel("truck_allemand", new Vector2(122, 206));
            //mapObject.AddModel("truck_allemand_casse", new Vector2(40, 355));
            mapObject.AddModel("farm", new Vector2(0, 262));

            #endregion

            #if EDITOR_MODE

            minNbObject = 0;
            maxNbObject = mapObject.Models.Count - 1;
            hudPosObject.AssociateModel(mapObject);

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
            
            #if EDITOR_MODE

            mapObjectPos.Change_i(ref minNbObject, maxNbObject);
            mapObject.MoveModel(new Vector4(
                mapObjectPos.Change_x_z(mapObject.Models.ElementAt(minNbObject).Value.position.Translation),
                minNbObject));
            hudPosObject.Upieme(minNbObject);

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
                mapObject.Draw();
                player.Draw();

                //
                // Display the cross in the center of the screen
                //
                #region Cross

                spriteBatch.Begin();
                spriteBatch.Draw(cross,
                    new Vector2((GraphicsDevice.Viewport.Width / 2) - 8, (GraphicsDevice.Viewport.Height / 2) - 8),
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
