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

        private Player player;

        private HeightMap map;
        private Effect mapEffect;

        private ModelManager mapObject;
        private GameObject house;
        private GameObject barn;
        private GameObject wood_barrier;
        private GameObject barbed_barrier;
        private GameObject bandbags;
        private GameObject antitank;
        private GameObject table;

        private MenuPause menuPause;
        private MenuStart menuStart;

        public Game()
        {
            this.graphics = new GraphicsDeviceManager(this);
           // ActivateFullScreen();
            //DisableVsync();

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Camera camera = Camera.Instance;
            camera.New(this, Vector3.Zero, Vector3.Zero);

            player = new Player(this, new Vector3(128, 10, 128));

            Components.Add(new FPS(this));
            Components.Add(new DisplayPosition(this));
            Components.Add(new MemoryUse(this));

            mapObject = new ModelManager();

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
                Content.Load<Texture2D>("Textures/essai"),
                Content.Load<Texture2D>("Textures/grass"),
                32f,
                513,
                513,
                10f);

            mapEffect = Content.Load<Effect>("Effects/Terrain");

            #endregion

            #region Models

            house = new GameObject(this, "house", new Vector3(20, 10, 20));
            barn = new GameObject(this, "barn", new Vector3(50, 10, 200));
            wood_barrier = new GameObject(this, "wood_barrier", new Vector3(20, 10, 50));
            barbed_barrier = new GameObject(this, "barbed_barrier", new Vector3(20, 10, 100));
            bandbags = new GameObject(this, "bandbags", new Vector3(50, 10, 50));
            antitank = new GameObject(this, "antitank", new Vector3(50, 10, 128));
            table = new GameObject(this, "table", new Vector3(100, 10, 128));

            mapObject.AddModel(house);
            mapObject.AddModel(barn);
            mapObject.AddModel(wood_barrier);
            mapObject.AddModel(barbed_barrier);
            mapObject.AddModel(bandbags);
            mapObject.AddModel(antitank);
            mapObject.AddModel(table);

            #endregion

            #region Sound

            SFXManager.AddSFX("Springfield", Content.Load<SoundEffect>("Sounds/Springfield"));
            SFXManager.AddSFX("Walk", Content.Load<SoundEffect>("Sounds/Walk"));
            SFXManager.AddSFX("Run", Content.Load<SoundEffect>("Sounds/Run"));
            SFXManager.AddSFX("Reload", Content.Load<SoundEffect>("Sounds/Reload"));
            SFXManager.AddSFX("Empty Gun", Content.Load<SoundEffect>("Sounds/Empty Gun"));

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
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (menuStart.GameStart)
            {
                map.Draw(mapEffect);
                mapObject.Draw(gameTime);

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
