using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Arrow
{
    public class TestScreen : GameScreen
    {
        #region Fields

        private Camera camera;
        private BackgroundWorker _terrainWorker;

        #endregion

        #region Initialization

        public TestScreen(Game game)
            : base(game)
        {
            camera = Camera.Instance;
        }

        public override void LoadContent()
        {
            camera.New(game, Vector3.Zero, Vector3.Zero);
            ScreenManager.Game.ResetElapsedTime();
        }

        /// <summary>
        /// Unload graphics content used by the game
        /// </summary>
        public override void UnloadContent()
        {
        }

        #endregion

        public override void Update(GameTime gameTime, bool hasFocus, bool isVisible)
        {
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
        }

        public override void Draw(GameTime gameTime)
        {
        }
    }
}
