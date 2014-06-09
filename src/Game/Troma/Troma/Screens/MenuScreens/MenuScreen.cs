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
        public List<Entry> MenuEntries { get; protected set; }
        public SpriteFont SpriteFont { get; protected set; }

        protected int selectedEntry;

        public MenuScreen(Game game)
            : base(game)
        {
            MenuEntries = new List<Entry>();

            TransitionOnTime = TimeSpan.FromSeconds(1);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            selectedEntry = 0;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            SpriteFont = FileManager.Load<SpriteFont>("Fonts/28DaysLater");
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
