﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameEngine;

namespace Troma
{
    class HelpMenu : MenuScreen
    {
        private Button backMenuEntry;

        private Texture2D bg;
        private Texture2D bgTrans;
        private Texture2D arrow;

        private Rectangle bgRect;
        private Rectangle bgTransRect;
        private Rectangle arrowRect;

        private String keyLabel;
        private String key;
        private Vector2 keyLabelPos;
        private Vector2 keyPos;

        public HelpMenu(Game game)
            : base(game)
        {
            Vector2 entryPos = new Vector2(143, 875);

            // Create menu entries.
            backMenuEntry = new Button(Resource.Back, 1, entryPos);

            // Hook up menu event handlers.
            backMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(backMenuEntry);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            bg = GameServices.Game.Content.Load<Texture2D>("Menus/background-blur");
            bgTrans = GameServices.Game.Content.Load<Texture2D>("Menus/translucide");
            arrow = GameServices.Game.Content.Load<Texture2D>("Menus/arrow");

            bgRect = new Rectangle(0, 0, 1920, 1080);
            bgTransRect = new Rectangle(0, 0, 0, 0);
            arrowRect = new Rectangle(0, 0, 64, 64);

            Dictionary<KeyActions, Keys> kb = InputConfiguration.GetKeybord();

            StringBuilder tmp = new StringBuilder();
            tmp.AppendLine(Resource.labelUp);
            tmp.AppendLine(Resource.labelBottom);
            tmp.AppendLine(Resource.labelLeft);
            tmp.AppendLine(Resource.labelRight);
            tmp.AppendLine(Resource.labelRun);
            tmp.AppendLine(Resource.labelJump);
            tmp.AppendLine(Resource.labelCrouch);
            tmp.AppendLine(Resource.labelReload);
            tmp.AppendLine(Resource.labelWeapon);
            tmp.AppendLine(Resource.labelShoot);
            tmp.AppendLine(Resource.labelAimfor);
            tmp.AppendLine(Resource.labelPause);
            keyLabel = tmp.ToString();

            tmp.Clear();

            tmp.AppendLine(kb[KeyActions.Up].ToString());
            tmp.AppendLine(kb[KeyActions.Bottom].ToString());
            tmp.AppendLine(kb[KeyActions.Left].ToString());
            tmp.AppendLine(kb[KeyActions.Right].ToString());
            tmp.AppendLine(Resource.LeftShift);
            tmp.AppendLine(Resource.Space);
            tmp.AppendLine(kb[KeyActions.Crouch].ToString());
            tmp.AppendLine(kb[KeyActions.Reload].ToString());
            tmp.AppendLine(Resource.Molette);
            tmp.AppendLine(Resource.LeftMouse);
            tmp.AppendLine(Resource.RightMouse);
            tmp.AppendLine(Keys.P.ToString());
            key = tmp.ToString();
        }

        public override void Draw(GameTime gameTime)
        {
            int width = GameServices.GraphicsDevice.Viewport.Width;
            int height = GameServices.GraphicsDevice.Viewport.Height;

            float widthScale = (float)width / 1920;
            float heightScale = (float)height / 1080;
            float scale =0.9f* (widthScale + heightScale) / 2;

            bgRect.Height = height;
            bgRect.Width = (int)(height * 1.778f);
            bgRect.X = -(bgRect.Width - width) / 2;

            keyLabelPos = new Vector2(
                143 * widthScale,
                143 * heightScale);

            keyPos = new Vector2(
                keyLabelPos.X + SpriteFont.MeasureString(keyLabel).X * scale + 200 * widthScale,
                143 * heightScale);

            float lineSpacing = SpriteFont.LineSpacing * scale;
            int nbLine = keyLabel.Split('\n').Length - 1;

            bgTransRect.X = (int)(133 * widthScale);
            bgTransRect.Height = (int)(lineSpacing);
            bgTransRect.Width = (int)(keyPos.X - keyLabelPos.X + (SpriteFont.MeasureString(key).X + 40) * scale);

            GameServices.SpriteBatch.Begin();

            GameServices.SpriteBatch.Draw(bg, bgRect, Color.White);

            for (float i = 0; i < nbLine; i += 2)
            {
                bgTransRect.Y = (int)(keyLabelPos.Y - 2 + i * lineSpacing);
                GameServices.SpriteBatch.Draw(bgTrans, bgTransRect, Color.White * TransitionAlpha * 0.3f);
            }

            GameServices.SpriteBatch.DrawString(SpriteFont, keyLabel, keyLabelPos, Color.Black * TransitionAlpha, 0,
                Vector2.Zero, scale, SpriteEffects.None, 0);
            GameServices.SpriteBatch.DrawString(SpriteFont, key, keyPos, Color.Ivory * TransitionAlpha, 0,
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
            ExitScreen();
        }
    }
}
