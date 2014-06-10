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
    public abstract class MenuScreenOld : GameScreen
    {
        public List<MenuEntry> MenuEntries { get; protected set; }
        public SpriteFont Font { get; protected set; }

        protected readonly string menuTitle;
        protected int selectedEntry;

        public MenuScreenOld(Game game, string menuTitle)
            : base(game)
        {
            MenuEntries = new List<MenuEntry>();
            this.menuTitle = menuTitle;

            TransitionOnTime = TimeSpan.FromSeconds(1);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            selectedEntry = 0;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            Font = FileManager.Load<SpriteFont>("Fonts/Menu");
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
            bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            // Update each nested MenuEntry object.
            for (int i = 0; i < MenuEntries.Count; i++)
            {
                bool isSelected = IsActive && (i == selectedEntry);
                MenuEntries[i].Update(gameTime, this, isSelected);
            }

            UpdateMenuEntryLocations();
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            // Move to the previous menu entry?
            if (input.IsPressed(Keys.Down) || (input.IsPressed(Buttons.DPadDown)))
            {
                SFXManager.Play("Button_selected");
                selectedEntry++;

                if (selectedEntry >= MenuEntries.Count)
                    selectedEntry = 0;
            }

            // Move to the next menu entry?
            if (input.IsPressed(Keys.Up) || (input.IsPressed(Buttons.DPadUp)))
            {
                SFXManager.Play("Button_selected");
                selectedEntry--;

                if (selectedEntry < 0)
                    selectedEntry = MenuEntries.Count - 1;
            }

            // Select ?
            if (input.IsPressed(Keys.Enter) || (input.IsPressed(Buttons.A)))
            {
                SFXManager.Play("Button_entry");
                OnSelectEntry(selectedEntry);
            }

        }

        protected virtual void UpdateMenuEntryLocations()
        {
            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);
            int width = game.GraphicsDevice.Viewport.Width;
            float height = game.GraphicsDevice.Viewport.Height;

            Vector2 position = new Vector2(
                (100 * width) / 1920,
                (350 * height) / 1080);

            float totalTextSize = MenuEntries[0].FontSize * MenuEntries.Count;
            height -= (1.4f * position.Y) - totalTextSize;
            float spacing = height / MenuEntries.Count;

            // update each menu entry's location in turn
            for (int i = 0; i < MenuEntries.Count; i++)
            {
                if (ScreenState == ScreenState.TransitionOn)
                    position.X -= transitionOffset * 256;
                else
                    position.X += transitionOffset * 512;

                MenuEntries[i].Position = position;
                position.Y += spacing;
            }
        }

        protected void OnSelectEntry(int entryIndex)
        {
            MenuEntries[entryIndex].OnSelectEntry();
        }

        protected virtual void OnCancel()
        {
            ExitScreen();
        }
    }
}
