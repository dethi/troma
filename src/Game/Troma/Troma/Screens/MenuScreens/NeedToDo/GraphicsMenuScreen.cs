using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine;


namespace Troma
{
    class GraphicsMenuScreen : MenuScreenOld
    {
        #region Fields
        
        private MenuEntry cloudMenuEntry;
        private MenuEntry displayMenuEntry;
        private MenuEntry vsyncMenuEntry;
        private MenuEntry multisamplingMenuEntry;
        private MenuEntry backMenuEntry;

        private int x1;
        private int x2;
        private int x3;

        private Texture2D background;
        private Texture2D balle_gauche;
        private Texture2D balle_droite;
        private Texture2D logo;

        #endregion

        #region Initialization

        public GraphicsMenuScreen(Game game)
            : base(game, Resource.Graphics)
        {
            cloudMenuEntry = new MenuEntry(string.Empty, 0.60f, 0, false);
            displayMenuEntry = new MenuEntry(string.Empty, 0.60f, 0, false);
            vsyncMenuEntry = new MenuEntry(string.Empty, 0.60f, 0, false);
            multisamplingMenuEntry = new MenuEntry(string.Empty, 0.60f, 0, false);
            backMenuEntry = new MenuEntry(string.Empty, 0.60f, 0, false);

            SetMenuEntryText();

            cloudMenuEntry.Selected += CloudMenuEntrySelected;
            displayMenuEntry.Selected += DisplayMenuEntrySelected;
            vsyncMenuEntry.Selected += VsyncMenuEntrySelected;
            multisamplingMenuEntry.Selected += MultisamplingMenuEntrySelected;
            backMenuEntry.Selected += OnCancel;

            MenuEntries.Add(cloudMenuEntry);
            MenuEntries.Add(displayMenuEntry);
            MenuEntries.Add(vsyncMenuEntry);
            MenuEntries.Add(multisamplingMenuEntry);
            MenuEntries.Add(backMenuEntry);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            x1 = 0;
            x2 = 0;
            x3 = 0;

            background = FileManager.Load<Texture2D>("Menus/Fond");
            balle_gauche = FileManager.Load<Texture2D>("Menus/balle-droite");
            balle_droite = FileManager.Load<Texture2D>("Menus/balle-gauche");
            logo = FileManager.Load<Texture2D>("Menus/eie");
        }

        #endregion

        public override void Draw(GameTime gameTime)
        {
            int width = game.GraphicsDevice.Viewport.Width;
            int height = game.GraphicsDevice.Viewport.Height;

            Color c = new Color(120, 110, 100) * TransitionAlpha;
            Rectangle posBackground = new Rectangle(0, 0, width, height);
            Rectangle posLogo = new Rectangle(width - 100, height - 100, 100, 100);

            x1 = (x1 < 2100) ? (x1 + 12) : 0;
            x2 = (x2 > 0) ? (x2 - 15) : 3500;
            x3 = (x3 > 0) ? (x3 - 22) : 2800;

            Rectangle rect1 = new Rectangle(
                x1 * width / 1920,
                height - (60 * width / 1920),
                60 * width / 1920,
                60 * width / 1920);
            Rectangle rect2 = new Rectangle(
                x2 * width / 1920,
                height - (80 * width / 1920),
                50 * width / 1920,
                50 * width / 1920);
            Rectangle rect3 = new Rectangle(
                x3 * width / 1920,
                height - (30 * width / 1920),
                40 * width / 1920,
                40 * width / 1920);

            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            Vector2 titlePosition = new Vector2(
                0.5f * width,
                0.0625f * width - 100 * transitionOffset);
            Vector2 titleOrigin = Font.MeasureString(menuTitle) / 2;
            Color titleColor = new Color(0, 0, 0) * TransitionAlpha;
            float titleScale = 0.00078125f * width;

            GameServices.SpriteBatch.Begin();

            GameServices.SpriteBatch.Draw(background, posBackground, Color.White * TransitionAlpha);
            GameServices.SpriteBatch.Draw(logo, posLogo, Color.White * TransitionAlpha * 0.4f);
            GameServices.SpriteBatch.Draw(balle_gauche, rect1, c);
            GameServices.SpriteBatch.Draw(balle_droite, rect2, c);
            GameServices.SpriteBatch.Draw(balle_droite, rect3, c);

            GameServices.SpriteBatch.DrawString(Font, menuTitle, titlePosition, titleColor, 0,
                titleOrigin, titleScale, SpriteEffects.None, 0);

            // Draw each menu entry in turn.
            for (int i = 0; i < MenuEntries.Count; i++)
            {
                bool isSelected = IsActive && (i == selectedEntry);
                MenuEntries[i].Draw(gameTime, this, isSelected);
            }

            GameServices.SpriteBatch.End();
        }

        private void CloudMenuEntrySelected(object sender, EventArgs e)
        {
            Settings.DynamicClouds = !Settings.DynamicClouds;
            SetMenuEntryText();
        }

        private void DisplayMenuEntrySelected(object sender, EventArgs e)
        {
            Settings.FullScreen = !Settings.FullScreen;
            SetMenuEntryText();
        }

        private void VsyncMenuEntrySelected(object sender, EventArgs e)
        {
            Settings.Vsync = !Settings.Vsync;
            SetMenuEntryText();
        }

        private void MultisamplingMenuEntrySelected(object sender, EventArgs e)
        {
            Settings.Multisampling = !Settings.Multisampling;
            SetMenuEntryText();
        }

        private void OnCancel(object sender, EventArgs e)
        {
            Settings.Save();
            OnCancel();
        }

        private void SetMenuEntryText()
        {
            cloudMenuEntry.Text = Resource.labelCloud + (Settings.DynamicClouds ? Resource.Yes : Resource.No);
            displayMenuEntry.Text = Resource.labelFullScreen + (Settings.FullScreen ? Resource.Yes : Resource.No);
            vsyncMenuEntry.Text = Resource.labelVsync + (Settings.Vsync ? Resource.Yes : Resource.No);
            multisamplingMenuEntry.Text = Resource.labelMultisampling + (Settings.Multisampling ? Resource.Yes : Resource.No);
            backMenuEntry.Text = Resource.Back;
        }
    }
}
