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
    class ConnectPopUp : MenuScreen
    {
        private Color orange = new Color(215, 145, 96);
        private Button VaidateMenuEntry;

        private Texture2D bgTrans;
        private Texture2D arrow;

        private Rectangle bgTransRect;
        private Rectangle arrowRect;

        private string msg;
        private string ip;

        private Vector2 msgPos;
        private Vector2 ipPos;

        public ConnectPopUp(Game game)
            : base(game)
        {
            Vector2 entryPos = new Vector2(143, 435);

            // Create menu entries.
            VaidateMenuEntry = new Button(Resource.Validate, 1, entryPos);
            msg = Resource.IPAddress;
            ip = "";

            // Hook up menu event handlers.
            VaidateMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(VaidateMenuEntry);

            IsHUD = true;

            EventInput.Initialize(GameServices.Game.Window);
            EventInput.CharEntered += new CharEnteredHandler(EventInput_CharEntered);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            bgTrans = GameServices.Game.Content.Load<Texture2D>("Menus/translucide");
            arrow = GameServices.Game.Content.Load<Texture2D>("Menus/arrow");

            bgTransRect = new Rectangle(0, 0, 0, 0);
            arrowRect = new Rectangle(0, 0, 64, 64);

            int width = GameServices.GraphicsDevice.Viewport.Width;
            int height = GameServices.GraphicsDevice.Viewport.Height;

            float widthScale = (float)width / 1920;
            float heightScale = (float)height / 1080;
            float scale = (widthScale + heightScale) / 2;

            Vector2 textSize = SpriteFont.MeasureString(msg) * scale;
            msgPos = new Vector2(
                980 * widthScale - (textSize.X / 2),
                460 * widthScale - (textSize.Y / 2));

            ipPos = new Vector2(
                msgPos.X,
                msgPos.Y + textSize.Y + 10);

            bgTransRect.X = (int)(940 * widthScale - (textSize.X / 2));
            bgTransRect.Y = (int)(420 * widthScale - (textSize.Y / 2));
            bgTransRect.Width = (int)textSize.X + 80;
            bgTransRect.Height = (int)(textSize.Y + 80 + 150 * scale);

            VaidateMenuEntry.originPos = new Vector2(
                940,
                (msgPos.Y + textSize.Y) / heightScale + 100);
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 0.45f);

            int width = GameServices.GraphicsDevice.Viewport.Width;
            int height = GameServices.GraphicsDevice.Viewport.Height;

            float widthScale = (float)width / 1920;
            float heightScale = (float)height / 1080;
            float scale = (widthScale + heightScale) / 2;

            GameServices.SpriteBatch.Begin();
            GameServices.SpriteBatch.Draw(bgTrans, bgTransRect, Color.White * TransitionAlpha * 0.3f);
            GameServices.SpriteBatch.DrawString(SpriteFont, msg, msgPos, Color.Black * TransitionAlpha, 0,
                Vector2.Zero, scale, SpriteEffects.None, 0);
            GameServices.SpriteBatch.DrawString(SpriteFont, ip, ipPos, orange * TransitionAlpha, 0,
                Vector2.Zero, scale, SpriteEffects.None, 0);

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

        protected override void OnCancel(object sender, EventArgs e)
        {
            if (ip != "")
            {
                EventInput.CharEntered -= new CharEnteredHandler(EventInput_CharEntered);
                SoundManager.Stop();
                LoadingScreen.Load(game, ScreenManager, true, new MultiplayerScreen(game, ip));
            }
        }

        private void EventInput_CharEntered(object sender, CharacterEventArgs e)
        {

            if (char.IsControl(e.Character))
            {
                switch (e.Character)
                {
                    case '\b':
                        if (ip.Length > 0)
                            ip = ip.Substring(0, ip.Length - 1);
                        break;
                    case '\r':
                        //enter
                        break;
                }
            }
            else if (ip.Length < 16 && ('a' <= e.Character && e.Character <= 'z') ||
                ('A' <= e.Character && e.Character <= 'Z') ||
                ('0' <= e.Character && e.Character <= '9') || e.Character == '.')
            {
                ip += e.Character;
            }
        }
    }
}
