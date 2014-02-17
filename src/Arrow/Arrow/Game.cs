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
    public class Game : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        //private SpriteBatch spriteBatch;
        //private ModelManager modelManager;

        private Player player;

        private HeightMap map;
        private Effect effect;

        public Game()
        {
            this.graphics = new GraphicsDeviceManager(this);
            //graphics.IsFullScreen = true;

            Content.RootDirectory = "Content";

            //modelManager = new ModelManager();

            /*
            // Disable V-Sync, allow more than 60 FPS
            this.IsFixedTimeStep = false;
            this.graphics.SynchronizeWithVerticalRetrace = false;
            this.graphics.ApplyChanges();
            */
        }

        protected override void Initialize()
        {
            player = new Player(this);

            Components.Add(new FPS(this));
            Components.Add(new DisplayPosition(this));

            //Components.Add(new Button(this, 10, 10 ,32 ,32, "textureIsOff", "textureIsOn"));
            Components.Add(new MenuPause(this));


            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            //this.spriteBatch = new SpriteBatch(GraphicsDevice);

            map = new HeightMap(GraphicsDevice,
                Content.Load<Texture2D>("Textures/heightmap"),
                Content.Load<Texture2D>("Textures/grass"),
                32f,
                128,
                128,
                8f);

            effect = Content.Load<Effect>("Effects/Terrain");

            SFXManager.AddSFX("Springfield", Content.Load<SoundEffect>("Sounds/Springfield"));
        }

        protected override void UnloadContent() { }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            //modelManager.Update(gameTime);

            //VARIABLE GLOBALE EN ATTENTE
            if (Menu.playerOff == false)
            {
                player.Update(gameTime, map);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //modelManager.Draw(gameTime);
            map.Draw(Camera.Instance, effect);

            base.Draw(gameTime);
        }
    }
}
