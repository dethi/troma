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
        private GraphicsDeviceManager graphics;

        private SpriteBatch spriteBatch;
        private Texture2D cross;

        //private ModelManager modelManager;

        private Player player;

        private HeightMap map;
        private Effect effect;

        private MenuPause menuPause;

        public Game()
        {
            this.graphics = new GraphicsDeviceManager(this);
            //ActivateFullScreen();
            //DisableVsync();

            Content.RootDirectory = "Content";

            //modelManager = new ModelManager();
        }

        protected override void Initialize()
        {
            player = new Player(this, new Vector3(128, 10, 128));

            Components.Add(new FPS(this));
            Components.Add(new DisplayPosition(this));

            //Components.Add(new Button(this, 10, 10 ,32 ,32, "textureIsOff", "textureIsOn"));
            menuPause = new MenuPause(this);
            menuPause.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            cross = Content.Load<Texture2D>("Cross");

            map = new HeightMap(GraphicsDevice,
                Content.Load<Texture2D>("Textures/heightmap"),
                Content.Load<Texture2D>("Textures/grass"),
                32f,
                513,
                513,
                10f);

            effect = Content.Load<Effect>("Effects/Terrain");

            SFXManager.AddSFX("Springfield", Content.Load<SoundEffect>("Sounds/Springfield"));
            SFXManager.AddSFX("Bruit de pas2", Content.Load<SoundEffect>("Sounds/Bruit de pas2"));

            menuPause.LoadContent();
        }

        protected override void UnloadContent() { }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            //modelManager.Update(gameTime);

            if (!menuPause.DisplayMenu)
                player.Update(gameTime, map);

            menuPause.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //modelManager.Draw(gameTime);
            map.Draw(Camera.Instance, effect);

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

            base.Draw(gameTime);
        }
    }
}
