using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Arrow
{
    public class DebugScreen : GameScreen
    {
        #region Fields

        private ContentManager content;
        private SpriteBatch spriteBatch;
        private SpriteFont spriteFont;

        private float memory;
        private double fps;
        private Camera camera;

        #endregion

        #region Initialization

        public DebugScreen(Game game)
            : base(game)
        {
            IsHUD = true;
            ScreenState = ScreenState.Active;
            camera = Camera.Instance;
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            spriteBatch = ScreenManager.SpriteBatch;
            spriteFont = content.Load<SpriteFont>("Fonts/Debug");
        }

        public override void UnloadContent()
        {
            content.Unload();
        }

        #endregion

        public override void Update(GameTime gameTime, bool hasFocus, bool isVisible)
        {
            memory = GC.GetTotalMemory(false) / 1048576f;
            fps = 1000.0d / gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        public override void Draw(GameTime gameTime)
        {
            string debug = String.Format(
                "X: {0:F2}\n" +
                "Y: {1:F2}\n" +
                "Z: {2:F2}\n" +
                "FPS: {3:F1}\n" +
                "Mem: {4:F2} Mo",
                camera.Position.X, camera.Position.Y, camera.Position.Z, fps, memory);

            Vector2 size = spriteFont.MeasureString(debug);
            Vector2 pos = new Vector2(5,
                ScreenManager.GraphicsDevice.Viewport.Height - size.Y - 5);

            spriteBatch.Begin();
            spriteBatch.DrawString(spriteFont, debug, pos, Color.Gold);
            spriteBatch.End();
        }
    }
}
