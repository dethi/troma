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
        private SpriteBatch spriteBatch;
        //private ModelManager modelManager;

        public Camera camera { get; private set; }

        //private SquareMap map;
        //private BasicEffect effect;

        private HeightMap terrain;
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
            camera = new Camera(this, new Vector3(0, 6.8f, 0));

            //effect = new BasicEffect(GraphicsDevice);
            //map = new SquareMap(GraphicsDevice);

            Components.Add(new FPS(this));
            Components.Add(new DisplayPosition(this));

            //Components.Add(new Button(this, 10, 10 ,32 ,32, "textureIsOff", "textureIsOn"));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            this.spriteBatch = new SpriteBatch(GraphicsDevice);

            terrain = new HeightMap(GraphicsDevice,
                Content.Load<Texture2D>("Textures/heightmap"),
                Content.Load<Texture2D>("Textures/grass"),
                32f,
                128,
                128,
                3f);

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
            //camera.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            //modelManager.Draw(gameTime);
            //map.Draw(camera, effect);
            terrain.Draw(camera, effect);

            base.Draw(gameTime);
        }
    }
}
