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
        private MapObjPos mapobjpos;

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

            player = new Player(this, new Vector3(128, 10, 128));

            Components.Add(new FPS(this));
            Components.Add(new DisplayPosition(this));
            Components.Add(new MemoryUse(this));

            mapObject = new ModelManager(this);
            mapobjpos = new MapObjPos();

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
                Content.Load<Texture2D>("Textures/essai1"),
                Content.Load<Texture2D>("Textures/grass"),
                32f,
                513,
                513,
                0f);

            mapEffect = Content.Load<Effect>("Effects/Terrain");

            #endregion

            #region Models

            mapObject.AddModel("house", new Vector2(20, 20));
            mapObject.AddModel("barn", new Vector2(50, 200));
            mapObject.AddModel("wood_barrier", new Vector2(20, 50));
            mapObject.AddModel("barbed_barrier", new Vector2(20, 100));
            mapObject.AddModel("bandbags", new Vector2(150, 150));
            mapObject.AddModel("antitank", new Vector2(50, 128));
            mapObject.AddModel("table", new Vector2(100, 128));
            mapObject.AddModel("barrel", new Vector2(50, 50));
            mapObject.AddModel("soldier", new Vector2(250, 250));
            mapObject.AddModel("cible_homme", new Vector2(300, 300));

            #endregion

            #region Sound

            SFXManager.AddSFX("Springfield", Content.Load<SoundEffect>("Sounds/Springfield"));
            SFXManager.AddSFX("Walk", Content.Load<SoundEffect>("Sounds/Walk"));
            SFXManager.AddSFX("Run", Content.Load<SoundEffect>("Sounds/Run"));
            SFXManager.AddSFX("Reload", Content.Load<SoundEffect>("Sounds/Reload"));
            SFXManager.AddSFX("Empty_Gun", Content.Load<SoundEffect>("Sounds/Empty_Gun"));

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
            
            mapObject.MoveModel(new Vector4(mapobjpos.Change_x_z(mapObject.Models.ElementAt(1).Value.position.Translation), 1)); // appel de la méthode movemodel

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
