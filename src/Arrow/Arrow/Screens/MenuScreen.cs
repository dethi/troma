using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Arrow
{
    class MenuScreen : GameScreen
    {
        List<MenuEntry> menuEntries = new List<MenuEntry>();
        int selectedEntry = 0;
        string menuTitle;
        int x = 0;
        int y = 0;
        int z = 0;

        private ContentManager content;
        private SpriteFont font;
        public SpriteFont Font
        {
            get { return font; }
        }

        protected IList<MenuEntry> MenuEntries
        {
            get { return menuEntries; }
        }

        public MenuScreen(string menuTitle, Game game)
            : base(game)
        {
            this.menuTitle = menuTitle;

            TransitionOnTime = TimeSpan.FromSeconds(1);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            font = content.Load<SpriteFont>("Fonts/Texture");
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            // Move to the previous menu entry?
            if (input.IsPressed(Keys.Down) || (input.IsPressed(Buttons.DPadDown)))
            {
                selectedEntry++;

                if (selectedEntry >= menuEntries.Count)
                    selectedEntry = 0;
            }

            // Move to the next menu entry?
            if (input.IsPressed(Keys.Up) || (input.IsPressed(Buttons.DPadUp)))
            {
                selectedEntry--;

                if (selectedEntry < 0)
                    selectedEntry = menuEntries.Count - 1;
            }

            if (input.IsPressed(Keys.Enter) || (input.IsPressed(Buttons.A)))
            {
                OnSelectEntry(selectedEntry);
            }

        }

        protected virtual void OnSelectEntry(int entryIndex)
        {
            menuEntries[entryIndex].OnSelectEntry();
        }

        protected virtual void OnCancel()
        {
            ExitScreen();
        }

        protected virtual void UpdateMenuEntryLocations()
        {
            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            // start at Y = 175; each X value is generated per entry
            Vector2 position = new Vector2(0f, (400f * game.GraphicsDevice.Viewport.Width) / 1920);

            // update each menu entry's location in turn
            for (int i = 0; i < menuEntries.Count; i++)
            {
                MenuEntry menuEntry = menuEntries[i];

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

        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            // Update each nested MenuEntry object.
            for (int i = 0; i < menuEntries.Count; i++)
            {
                bool isSelected = IsActive && (i == selectedEntry);

                menuEntries[i].Update(this, isSelected, gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            // make sure our entries are in the right place before we draw them
            UpdateMenuEntryLocations();

            GraphicsDevice graphics = ScreenManager.GraphicsDevice;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            // fond d ecran
            Texture2D fond = game.Content.Load<Texture2D>("Textures/America");
            Texture2D fond2 = game.Content.Load<Texture2D>("Textures/white");

            Color c = new Color(20, 15, 10) * TransitionAlpha;

            Vector2 origin = new Vector2(0, 0);
            Rectangle rectangle = new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width, game.GraphicsDevice.Viewport.Height);
            spriteBatch.Draw(fond, rectangle, rectangle, Color.White * TransitionAlpha, 0, origin, SpriteEffects.None, 0);



            if (x < 1900)
            {
                x += 20;
            }
            else { x = 0; }
            if (y > 0)
            {
                y -= 10;
            }
            else { y = 2100; }
            if (z > 0)
            {
                z -= 25;
            }
            else { z = 2500; }


            Rectangle rectangleBas1 = new Rectangle(0 + x, 1050, 150, 8);
            Rectangle rectangleBas2 = new Rectangle(0 + y, 1060, 170, 6);
            Rectangle rectangleBas3 = new Rectangle(0 + z, 1040, 130, 4);
            spriteBatch.Draw(fond2, rectangleBas1, c);
            spriteBatch.Draw(fond2, rectangleBas2, c);
            spriteBatch.Draw(fond2, rectangleBas3, c);


            // Draw each menu entry in turn.
            for (int i = 0; i < menuEntries.Count; i++)
            {
                MenuEntry menuEntry = menuEntries[i];

                bool isSelected = IsActive && (i == selectedEntry);

                menuEntry.Draw(this, isSelected, gameTime, game);
            }

            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            // Draw the menu title centered on the screen
            Vector2 titlePosition = new Vector2(graphics.Viewport.Width / 2, 120 * game.GraphicsDevice.Viewport.Width / 1920);
            Vector2 titleOrigin = font.MeasureString(menuTitle) / 2;
            Color titleColor = new Color(100, 100, 100) * TransitionAlpha;
            float titleScale = 2f * game.GraphicsDevice.Viewport.Width / 1920;

            titlePosition.Y -= transitionOffset * 100;

            spriteBatch.DrawString(font, menuTitle, titlePosition, titleColor, 0,
                                   titleOrigin, titleScale, SpriteEffects.None, 0);

            spriteBatch.End();
        }
    }
}
