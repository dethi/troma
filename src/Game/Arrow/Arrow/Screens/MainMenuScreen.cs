using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arrow
{
    class MainMenuScreen : MenuScreen
    {
        public MainMenuScreen(Game game)
            : base("Troma", game, "America")
        {
            // Create our menu entries.
            MenuEntry playGameMenuEntry = new MenuEntry("Jouer");
            MenuEntry optionsMenuEntry = new MenuEntry("Options");
            MenuEntry exitMenuEntry = new MenuEntry("Quitter");

            // Hook up menu event handlers.
            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(playGameMenuEntry);
            MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }

        void PlayGameMenuEntrySelected(object sender, EventArgs e)
        {
            LoadingScreen.Load(game, ScreenManager, true,
                               new GameplayScreen(game));
        }

        void OptionsMenuEntrySelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen(game));
        }

        protected override void OnCancel()
        {
            game.Exit();
        }

        protected void OnCancel(object sender, EventArgs e)
        {
            OnCancel();
        }

    }
}
