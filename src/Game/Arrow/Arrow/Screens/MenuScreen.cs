﻿using System;
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
        int x1 = 0;
        int x2 = 0;
        int x3 = 0;
        string background;

        MenuSong audio;
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

        public MenuScreen(string menuTitle, Game game, string background)
            : base(game)
        {
            this.menuTitle = menuTitle;
            TransitionOnTime = TimeSpan.FromSeconds(1);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            this.background = background;
    
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            font = content.Load<SpriteFont>("Fonts/Texture");
            audio = new MenuSong(game);
            audio.LoadContent();
            audio.SongPlayed = true;
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
            Texture2D fond = game.Content.Load<Texture2D>("Textures/" + background);
            Texture2D fond2 = game.Content.Load<Texture2D>("Textures/balle3");
            Texture2D fond3 = game.Content.Load<Texture2D>("Textures/balle4");
            Texture2D fond4 = game.Content.Load<Texture2D>("Textures/eie");

            Color c = new Color(120, 110, 100) * TransitionAlpha;

            int largeur = game.GraphicsDevice.Viewport.Width;
            int hauteur = game.GraphicsDevice.Viewport.Height;
            
            Rectangle rectangle = new Rectangle(0, 0, largeur, hauteur);
            spriteBatch.Draw(fond, rectangle, Color.White * TransitionAlpha);

            Rectangle logo = new Rectangle(largeur - 100, hauteur - 100, 100, 100);
            spriteBatch.Draw(fond4, logo, Color.White * TransitionAlpha * 0.4f );


            if (x1 < 2100){x1 += 12;}
            else { x1 = 0; }
            
            if (x2 > 0){x2 -= 15;}
            else { x2 = 3500; }
            
            if (x3 > 0){x3 -= 22;}
            else { x3 = 2800; }

            Rectangle rectangleBas1 = new Rectangle((0 + x1) * largeur / 1920,
                                                        hauteur - (60 * largeur/1920) ,
                                                        60 * largeur / 1920,
                                                            60 * largeur / 1920);
            Rectangle rectangleBas2 = new Rectangle((0 + x2) * largeur / 1920,
                                                       hauteur - (80 * largeur / 1920),
                                                       50 * largeur / 1920,
                                                       50 * largeur / 1920);
            Rectangle rectangleBas3 = new Rectangle((0 + x3) * largeur / 1920,
                                                       hauteur - (30 * largeur / 1920),
                                                       40 * largeur / 1920,
                                                       40 * largeur / 1920);
            spriteBatch.Draw(fond2, rectangleBas1, c);
            spriteBatch.Draw(fond3, rectangleBas2, c);
            spriteBatch.Draw(fond3, rectangleBas3, c);


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
            Color titleColor = new Color(0, 0, 0) * TransitionAlpha;
            
            double time = (gameTime.TotalGameTime.TotalSeconds)/3;
            float pulsate = (float)Math.Sin(time * 6) + 1;
            float titleScale = 1.5f * game.GraphicsDevice.Viewport.Width / 1920 + (0.03f*pulsate);

            titlePosition.Y -= transitionOffset * 100;

            spriteBatch.DrawString(font, menuTitle, titlePosition, titleColor, 0,
                                   titleOrigin, titleScale, SpriteEffects.None, 0);

            spriteBatch.End();
        }
        protected void OnCancel(object sender, EventArgs e)
        {
            OnCancel();
        }
    }
}
