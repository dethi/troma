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
    public class MenuScreen : GameScreen
    {
        List<MenuEntry> _menuEntries;

        readonly string menuTitle;
        int selectedEntry;

        int x1;
        int x2;
        int x3;

        Texture2D background;
        Texture2D balle_gauche;
        Texture2D balle_droite;
        Texture2D logo;

        private SpriteFont _font;
        public SpriteFont Font
        {
            get { return _font; }
        }

        protected IList<MenuEntry> MenuEntries
        {
            get { return _menuEntries; }
        }

        public MenuScreen(string menuTitle, Game game)
            : base(game)
        {
            _menuEntries = new List<MenuEntry>();
            this.menuTitle = menuTitle;

            TransitionOnTime = TimeSpan.FromSeconds(1);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            selectedEntry = 0;
            x1 = 0;
            x2 = 0;
            x3 = 0;

            _font = FileManager.Load<SpriteFont>("Fonts/Menu");
            background = FileManager.Load<Texture2D>("Menus/Fond");
            balle_gauche = FileManager.Load<Texture2D>("Menus/balle-droite");
            balle_droite = FileManager.Load<Texture2D>("Menus/balle-gauche");
            logo = FileManager.Load<Texture2D>("Menus/eie");
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
            bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            // Update each nested MenuEntry object.
            for (int i = 0; i < _menuEntries.Count; i++)
            {
                bool isSelected = IsActive && (i == selectedEntry);
                _menuEntries[i].Update(this, isSelected, gameTime);
            }
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            // Move to the previous menu entry?
            if (input.IsPressed(Keys.Down) || (input.IsPressed(Buttons.DPadDown)))
            {
                selectedEntry++;

                if (selectedEntry >= _menuEntries.Count)
                    selectedEntry = 0;
            }

            // Move to the next menu entry?
            if (input.IsPressed(Keys.Up) || (input.IsPressed(Buttons.DPadUp)))
            {
                selectedEntry--;

                if (selectedEntry < 0)
                    selectedEntry = _menuEntries.Count - 1;
            }

            // Select ?
            if (input.IsPressed(Keys.Enter) || (input.IsPressed(Buttons.A)))
            {
                OnSelectEntry(selectedEntry);
            }

        }

        public override void Draw(GameTime gameTime)
        {
            UpdateMenuEntryLocations();

            GraphicsDevice graphics = GameServices.GraphicsDevice;
            SpriteBatch spriteBatch = GameServices.SpriteBatch;

            spriteBatch.Begin();

            Color c = new Color(120, 110, 100) * TransitionAlpha;

            int largeur = game.GraphicsDevice.Viewport.Width;
            int hauteur = game.GraphicsDevice.Viewport.Height;

            Rectangle posBackground = new Rectangle(0, 0, largeur, hauteur);
            spriteBatch.Draw(background, posBackground, Color.White * TransitionAlpha);

            Rectangle posLogo = new Rectangle(largeur - 100, hauteur - 100, 100, 100);
            spriteBatch.Draw(logo, posLogo, Color.White * TransitionAlpha * 0.4f);


            if (x1 < 2100)
                x1 += 12;
            else
                x1 = 0;

            if (x2 > 0)
                x2 -= 15;
            else
                x2 = 3500;

            if (x3 > 0)
                x3 -= 22;
            else
                x3 = 2800;

            Rectangle rectangleBas1 = new Rectangle(
                (0 + x1) * largeur / 1920,
                hauteur - (60 * largeur / 1920),
                60 * largeur / 1920,
                60 * largeur / 1920);
            Rectangle rectangleBas2 = new Rectangle(
                (0 + x2) * largeur / 1920,
                hauteur - (80 * largeur / 1920),
                50 * largeur / 1920,
                50 * largeur / 1920);
            Rectangle rectangleBas3 = new Rectangle(
                (0 + x3) * largeur / 1920,
                hauteur - (30 * largeur / 1920),
                40 * largeur / 1920,
                40 * largeur / 1920);

            spriteBatch.Draw(balle_gauche, rectangleBas1, c);
            spriteBatch.Draw(balle_droite, rectangleBas2, c);
            spriteBatch.Draw(balle_droite, rectangleBas3, c);


            // Draw each menu entry in turn.
            for (int i = 0; i < _menuEntries.Count; i++)
            {
                MenuEntry menuEntry = _menuEntries[i];
                bool isSelected = IsActive && (i == selectedEntry);
                menuEntry.Draw(this, isSelected, gameTime);
            }

            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            // Draw the menu title centered on the screen
            Vector2 titlePosition = new Vector2(graphics.Viewport.Width / 2, 120 * game.GraphicsDevice.Viewport.Width / 1920);
            Vector2 titleOrigin = _font.MeasureString(menuTitle) / 2;
            Color titleColor = new Color(0, 0, 0) * TransitionAlpha;

            double time = (gameTime.TotalGameTime.TotalSeconds) / 3;
            float pulsate = (float)Math.Sin(time * 6) + 1;
            float titleScale = 1.5f * game.GraphicsDevice.Viewport.Width / 1920 + (0.03f * pulsate);

            titlePosition.Y -= transitionOffset * 100;

            spriteBatch.DrawString(_font, menuTitle, titlePosition, titleColor, 0,
                                   titleOrigin, titleScale, SpriteEffects.None, 0);

            spriteBatch.End();
        }

        protected virtual void UpdateMenuEntryLocations()
        {
            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            Vector2 position = new Vector2(0f, (400f * game.GraphicsDevice.Viewport.Width) / 1920);

            // update each menu entry's location in turn
            for (int i = 0; i < _menuEntries.Count; i++)
            {
                MenuEntry menuEntry = _menuEntries[i];

                // each entry is to be centered horizontally
                position.X = 50;

                if (ScreenState == ScreenState.TransitionOn)
                    position.X -= transitionOffset * 256;
                else
                    position.X += transitionOffset * 512;

                // set the entry's position
                menuEntry.Position = position;

                // move down for the next entry the size of this entry
                position.Y += (menuEntry.GetHeight(this) + (200 * game.GraphicsDevice.Viewport.Width)) / 1920;
            }
        }

        protected virtual void OnSelectEntry(int entryIndex)
        {
            _menuEntries[entryIndex].OnSelectEntry();
        }

        protected virtual void OnCancel()
        {
            ExitScreen();
        }
    }
}
