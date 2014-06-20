using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using GameEngine;

namespace Troma
{
    public class TitleIntroScreen : GameScreen
    {
        private Video _video;
        private VideoPlayer _player;

        private Texture2D _videoTexture;
        private Rectangle _screen;

        public TitleIntroScreen(Game game)
            : base(game)
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            _video = FileManager.Load<Video>("Videos/Title_Intro");
            _player = new VideoPlayer();

            int width = GameServices.GraphicsDevice.Viewport.Width;
            int height = GameServices.GraphicsDevice.Viewport.Height;
            int sizeHeight = (width * 9) / 16;

            _screen = new Rectangle(
                0,
                (height - sizeHeight) / 2,
                width,
                sizeHeight);

            _player.Play(_video);
        }

        public override void Update(GameTime gameTime, bool hasFocus, bool isVisible)
        {
            base.Update(gameTime, hasFocus, isVisible);

            if (_player.State == MediaState.Stopped)
            {
                foreach (GameScreen screen in ScreenManager.GetScreens())
                    screen.ExitScreen();

                ScreenManager.AddScreen(new MainMenu(game));
            }
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (input.IsPressed(Keys.Enter) || input.IsPressed(Buttons.A) ||
                input.IsPressed(Buttons.Back) || input.IsPressed(Keys.Escape))
            {
                _player.Stop();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (_player.State != MediaState.Stopped)
                _videoTexture = _player.GetTexture();

            if (_videoTexture != null)
            {
                GameServices.SpriteBatch.Begin();
                GameServices.SpriteBatch.Draw(_videoTexture, _screen, Color.White);
                GameServices.SpriteBatch.End();
            }
        }
    }
}
