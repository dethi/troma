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
    public abstract class MenuScreen : GameScreen
    {
        public List<IEntry> MenuEntries { get; protected set; }
        public SpriteFont SpriteFont { get; protected set; }

        protected int selectedEntry;

        public MenuScreen(Game game)
            : base(game)
        {
            MenuEntries = new List<IEntry>();

            TransitionOnTime = TimeSpan.FromSeconds(1);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            selectedEntry = 0;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            SpriteFont = GameServices.Game.Content.Load<SpriteFont>("Fonts/28DaysLater");
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

            if (input.IsPressed(Keys.Left) || (input.IsPressed(Buttons.DPadLeft)))
            {
                SFXManager.Play("Button_selected");
                OnChangeChoice(selectedEntry, -1);
            }

            if (input.IsPressed(Keys.Right) || (input.IsPressed(Buttons.DPadRight)))
            {
                SFXManager.Play("Button_selected");
                OnChangeChoice(selectedEntry, 1);
            }
        }

        protected void OnSelectEntry(int entryIndex)
        {
            if (MenuEntries[entryIndex].Type == EntryType.Button)
                ((Button)MenuEntries[entryIndex]).OnSelectEntry();
        }

        protected void OnChangeChoice(int entryIndex, int choice)
        {
            if (choice != 0)
            {
                if (MenuEntries[entryIndex].Type == EntryType.Stepper)
                    ((Stepper)MenuEntries[entryIndex]).OnChangeValue(choice);
                else if (MenuEntries[entryIndex].Type == EntryType.Switch)
                    ((Switch)MenuEntries[entryIndex]).OnChangeValue((choice < 0));
            }
        }

        protected virtual void OnCancel()
        {
            ExitScreen();
        }
    }
}
