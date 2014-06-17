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
    class GraphicsMenu : MenuScreen
    {
        private Switch cloudMenuEntry;
        private Switch displayMenuEntry;
        private Switch vsyncMenuEntry;
        private Switch multisamplingMenuEntry;
        private Button backMenuEntry;

        private Texture2D bg;
        private Texture2D bgTrans;
        private Texture2D arrow;

        private Rectangle bgRect;
        private Rectangle bgTransRect;
        private Rectangle arrowRect;

        public GraphicsMenu(Game game)
            : base(game)
        {
            Vector2 entryPos = new Vector2(143, 143);
            float space = 100;

            // Create menu entries.
            cloudMenuEntry = new Switch(Resource.labelCloud, 1, entryPos, Settings.DynamicClouds);
            entryPos.Y += space;
            displayMenuEntry = new Switch(Resource.labelFullScreen, 1, entryPos, Settings.FullScreen);
            entryPos.Y += space;
            vsyncMenuEntry = new Switch(Resource.labelVsync, 1, entryPos, Settings.Vsync);
            entryPos.Y += space;
            multisamplingMenuEntry = new Switch(Resource.labelMultisampling, 1, entryPos, Settings.Multisampling);
            entryPos.Y = 875;
            backMenuEntry = new Button(Resource.Back, 1, entryPos);

            // Hook up menu event handlers.
            cloudMenuEntry.ChangedValue += CloudChangedValue;
            displayMenuEntry.ChangedValue += DisplayChangedValue;
            vsyncMenuEntry.ChangedValue += VsyncChangedValue;
            multisamplingMenuEntry.ChangedValue += MultisamplingChangedValue;
            backMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(cloudMenuEntry);
            MenuEntries.Add(displayMenuEntry);
            MenuEntries.Add(vsyncMenuEntry);
            MenuEntries.Add(multisamplingMenuEntry);
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

            Vector2 columnsPos = new Vector2(800 * widthScale, 0);

            bgTransRect.X = (int)(123 * widthScale);
            bgTransRect.Y = (int)(123 * heightScale);
            bgTransRect.Width = (int)(columnsPos.X + 90 * scale);
            bgTransRect.Height = (int)((3 * 100) * heightScale + (SpriteFont.LineSpacing + 40) * scale);

            GameServices.SpriteBatch.Begin();

            GameServices.SpriteBatch.Draw(bg, bgRect, Color.White);
            GameServices.SpriteBatch.Draw(bgTrans, bgTransRect, Color.White * TransitionAlpha * 0.15f);

            // Draw each menu entry in turn.
            for (int i = 0; i < MenuEntries.Count; i++)
            {
                bool isSelected = IsActive && (i == selectedEntry);
                MenuEntries[i].ColumnsPos = columnsPos;
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

        private void CloudChangedValue(object sender, EventArgs e)
        {
            Settings.DynamicClouds = cloudMenuEntry.Activated;
        }

        private void DisplayChangedValue(object sender, EventArgs e)
        {
            Settings.FullScreen = displayMenuEntry.Activated;
        }

        private void VsyncChangedValue(object sender, EventArgs e)
        {
            Settings.Vsync = vsyncMenuEntry.Activated;
        }

        private void MultisamplingChangedValue(object sender, EventArgs e)
        {
            Settings.Multisampling = multisamplingMenuEntry.Activated;
        }

        protected override void OnCancel(object sender, EventArgs e)
        {
            Settings.Save();
            ExitScreen();
        }
    }
}
