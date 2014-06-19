using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameEngine;

namespace Troma
{
    class ConnectOrHost : MenuScreen
    {
        private Button connectMenuEntry;
        private Button hostMenyEntry;
        private Button backMenuEntry;

        private Texture2D bg;
        private Texture2D bgTrans;
        private Texture2D arrow;

        private Rectangle bgRect;
        private Rectangle bgTransRect;
        private Rectangle arrowRect;

        public ConnectOrHost(Game game)
            : base(game)
        {
            Vector2 entryPos = new Vector2(143, 435);
            float space = 220;

            // Create menu entries.
            connectMenuEntry = new Button(Resource.Connect, 1, entryPos);
            entryPos.Y += space;
            hostMenyEntry = new Button(Resource.Host, 1, entryPos);
            entryPos.Y += space;
            backMenuEntry = new Button(Resource.Back, 1, entryPos);

            // Hook up menu event handlers.
            connectMenuEntry.Selected += ConnectMenuEntrySelected;
            hostMenyEntry.Selected += HostMenuEntrySelected;
            backMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(connectMenuEntry);
            MenuEntries.Add(hostMenyEntry);
            MenuEntries.Add(backMenuEntry);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            bg = GameServices.Game.Content.Load<Texture2D>("Menus/background-blur");
            bgTrans = GameServices.Game.Content.Load<Texture2D>("Menus/translucide");
            arrow = GameServices.Game.Content.Load<Texture2D>("Menus/arrow");

            bgRect = new Rectangle(0, 0, 1920, 1080);
            bgTransRect = new Rectangle(0, 0, 550, 1080);
            arrowRect = new Rectangle(0, 0, 64, 64);
        }

        public override void Draw(GameTime gameTime)
        {
            int width = GameServices.GraphicsDevice.Viewport.Width;
            int height = GameServices.GraphicsDevice.Viewport.Height;

            float widthScale = (float)width / 1920;
            float heightScale = (float)height / 1080;
            float scale = (widthScale + heightScale) / 2;

            bgRect.Height = height;
            bgRect.Width = (int)(height * 1.778f);
            bgRect.X = -(bgRect.Width - width) / 2;

            bgTransRect.Height = height;
            bgTransRect.Width = (int)(500 * widthScale);

            GameServices.SpriteBatch.Begin();
            GameServices.SpriteBatch.Draw(bg, bgRect, Color.White * ((IsExiting) ? TransitionAlpha : 1));
            GameServices.SpriteBatch.Draw(bgTrans, bgTransRect, Color.White * TransitionAlpha * 0.15f);

            // Draw each menu entry in turn.
            for (int i = 0; i < MenuEntries.Count; i++)
            {
                bool isSelected = IsActive && (i == selectedEntry);
                MenuEntries[i].Draw(gameTime, this, isSelected);

                if (isSelected)
                {
                    arrowRect.Height = (int)(64 * ((widthScale + heightScale) / 2));
                    arrowRect.Width = arrowRect.Height;
                    arrowRect.X = (int)(MenuEntries[i].Position.X - 1.5f * arrowRect.Width);
                    arrowRect.Y = (int)MenuEntries[i].Position.Y;
                    GameServices.SpriteBatch.Draw(arrow, arrowRect, Color.White * TransitionAlpha);
                }
            }

            GameServices.SpriteBatch.End();
        }

        private void ConnectMenuEntrySelected(object sender, EventArgs e)
        {
            //
        }

        private void HostMenuEntrySelected(object sender, EventArgs e)
        {
            Troma.StartServer();
            SoundManager.Stop();
            LoadingScreen.Load(game, ScreenManager, true, new MultiplayerScreen(game, "localhost"));
        }


        protected override void OnCancel(object sender, EventArgs e)
        {
            ExitScreen();
        }
    }
}
