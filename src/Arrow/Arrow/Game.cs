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
        private Effect effect;

        private FBX house; // Déclaration des différents modeles fbx
        private FBX barn;
        private FBX wood_barrier;
        private FBX barbed_barrier;
        private FBX bandbags;
        private FBX antitank;
        private FBX table;
        private ModelManager mapobject; // Déclaration du ModelManager qui contiendra tous les models

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

            map = new HeightMap(this,
                Content.Load<Texture2D>("Textures/essai"), //heigth map a majorité blanche pour la hauteur du sol (désolé je ne vois pas comment dire ca autrement :p)
                Content.Load<Texture2D>("Textures/grass"),
                32f,
                513,
                513,
                10f);

            effect = Content.Load<Effect>("Effects/Terrain");

            house = new FBX(this, "house", 20, 10, 20); // Instanciation du model dans le jeu en cours au coordonnées en parametres
            barn = new FBX(this, "barn", 50, 10, 200);
            wood_barrier = new FBX(this, "wood_barrier", 20, 10, 50);
            barbed_barrier = new FBX(this, "barbed_barrier", 20, 10, 100);
            bandbags = new FBX(this, "bandbags", 50, 10, 50);
            antitank = new FBX(this, "antitank", 50, 10, 128);
            table = new FBX(this, "table", 100, 10, 128);
            mapobject = new ModelManager(); // Instanciation du ModelManager
            mapobject.AddModel(house); // Ajout du model au ModelManager
            mapobject.AddModel(barn);
            mapobject.AddModel(wood_barrier);
            mapobject.AddModel(barbed_barrier);
            mapobject.AddModel(bandbags);
            mapobject.AddModel(antitank);
            mapobject.AddModel(table);

            SFXManager.AddSFX("Springfield", Content.Load<SoundEffect>("Sounds/Springfield"));
            SFXManager.AddSFX("Walk", Content.Load<SoundEffect>("Sounds/Walk"));
            SFXManager.AddSFX("Run", Content.Load<SoundEffect>("Sounds/Run"));

            menuPause.LoadContent();
            menuStart.LoadContent();
        }

        protected override void UnloadContent() { }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            //modelManager.Update(gameTime);

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
                mapobject.Draw(gameTime);
                map.Draw(effect);

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
