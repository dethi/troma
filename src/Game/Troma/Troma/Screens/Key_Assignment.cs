using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine;

namespace Troma
{
    class Key_Assignment : MenuScreen
    {
        #region Fields

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

        public Key_Assignment(Game game)
            : base(game, Resource.SummonsKeyboard)
        {
            backMenuEntry = new MenuEntry(string.Empty, 0.60f, 0, false);

            SetMenuEntryText();

            backMenuEntry.Selected += OnCancel;
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

        public override void DrawKeyboard(GameTime gameTime)
        {

            int width = GameServices.GraphicsDevice.Viewport.Width;
            int height = GameServices.GraphicsDevice.Viewport.Height;

            string Text1 = Resource.Up;
            string Text2 = Resource.Bottom;
            string Text3 = Resource.Left;
            string Text4 = Resource.Right;
            string Text5 = Resource.Run;
            string Text6 = Resource.Aimfor;
            string Text7 = Resource.Reload;
            string Text8 = Resource.Jump;
            string Text9 = Resource.Crouch;
            string Text10 = Resource.Menu_Paused;

            float textScale = 0.70f * width;

            Vector2 titleOrigin = new Vector2(0, 0);

            Vector2 Position1 = new Vector2(
            1680 * width / 1920,
            height - (120 * width / 1920));
            Vector2 Position3 = new Vector2(
            1678 * width / 1920,
            height - (125 * width / 1920));

            Vector2 Position2 = new Vector2(
            1730 * width / 1920,
            height - (105 * width / 1920));
            Vector2 Position5 = new Vector2(
            1728 * width / 1920,
            height - (108 * width / 1920));

            Vector2 Position4 = new Vector2(
            1770 * width / 1920,
            height - (105 * width / 1920));
            Vector2 Position6 = new Vector2(
            1768 * width / 1920,
            height - (108 * width / 1920));

            Vector2 Position7 = new Vector2(
            1680 * width / 1920,
            height - (150 * width / 1920));

            Vector2 Position8 = new Vector2(
            1710 * width / 1920,
            height - (150 * width / 1920));

            Vector2 Position9 = new Vector2(
            1710 * width / 1920,
            height - (150 * width / 1920));

            Vector2 Position10 = new Vector2(
            1710 * width / 1920,
            height - (150 * width / 1920));

            GameServices.SpriteBatch.Begin();
            GameServices.SpriteBatch.DrawString(Font, Text1, Position1, Color.Black * 0.3f, 0, titleOrigin, textScale, SpriteEffects.None, 0);
            GameServices.SpriteBatch.DrawString(Font, Text2, Position2, Color.Black * 0.3f, 0, titleOrigin, textScale, SpriteEffects.None, 0);
            GameServices.SpriteBatch.DrawString(Font, Text3, Position3, Color.Black * 0.3f, 0, titleOrigin, textScale, SpriteEffects.None, 0);
            GameServices.SpriteBatch.DrawString(Font, Text4, Position4, Color.Black * 0.3f, 0, titleOrigin, textScale, SpriteEffects.None, 0);
            GameServices.SpriteBatch.DrawString(Font, Text5, Position5, Color.Black * 0.3f, 0, titleOrigin, textScale, SpriteEffects.None, 0);
            GameServices.SpriteBatch.DrawString(Font, Text6, Position6, Color.Black * 0.3f, 0, titleOrigin, textScale, SpriteEffects.None, 0);
            GameServices.SpriteBatch.DrawString(Font, Text7, Position7, Color.Black * 0.3f, 0, titleOrigin, textScale, SpriteEffects.None, 0);
            GameServices.SpriteBatch.DrawString(Font, Text8, Position8, Color.Black * 0.3f, 0, titleOrigin, textScale, SpriteEffects.None, 0);
            GameServices.SpriteBatch.DrawString(Font, Text9, Position9, Color.Black * 0.3f, 0, titleOrigin, textScale, SpriteEffects.None, 0);
            GameServices.SpriteBatch.DrawString(Font, Text10, Position10, Color.Black * 0.3f, 0, titleOrigin, textScale, SpriteEffects.None, 0);
            GameServices.SpriteBatch.End();
        }

        private void OnCancel(object sender, EventArgs e)
        {
            OnCancel();
        }

        private void SetMenuEntryText()
        {
            backMenuEntry.Text = Resource.Back;
        }
    }
}
