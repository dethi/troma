using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arrow
{
    class OptionsMenuScreen : MenuScreen
    {
        #region Fields

            MenuEntry volumeMenuEntry;
            MenuEntry languageMenuEntry;
            MenuEntry displayMenuEntry;
            MenuEntry keyboardMenuEntry;

            static string[] keyboard = { "QWERTY", "AZERTY" };
            static int currentKeyboard = 0;

            static string[] languages = { "Francais", "Anglais" };
            static int currentLanguage = 0;

            static bool display = true;

            static int volume = 10;

        #endregion

        #region Initialization

            public OptionsMenuScreen(Game game)
            : base("Options", game, "America")
        {
            // Create our menu entries.
            keyboardMenuEntry = new MenuEntry(string.Empty);
            languageMenuEntry = new MenuEntry(string.Empty);
            displayMenuEntry = new MenuEntry(string.Empty);
            volumeMenuEntry = new MenuEntry(string.Empty);

            SetMenuEntryText();

            MenuEntry back = new MenuEntry("Back");

            // Hook up menu event handlers.
            keyboardMenuEntry.Selected += KeyboardMenuEntrySelected;
            languageMenuEntry.Selected += LanguageMenuEntrySelected;
            displayMenuEntry.Selected += DisplayMenuEntrySelected;
            volumeMenuEntry.Selected += VolumeMenuEntrySelected;

            back.Selected += OnCancel;
            
            // Add entries to the menu.
            MenuEntries.Add(keyboardMenuEntry);
            MenuEntries.Add(languageMenuEntry);
            MenuEntries.Add(displayMenuEntry);
            MenuEntries.Add(volumeMenuEntry);
            MenuEntries.Add(back);
        }

            private void KeyboardMenuEntrySelected(object sender, EventArgs e)
            {
                currentKeyboard = (currentKeyboard + 1) % keyboard.Length;

                SetMenuEntryText();
            }

            private void LanguageMenuEntrySelected(object sender, EventArgs e)
            {
                currentLanguage = (currentLanguage + 1) % languages.Length;

                SetMenuEntryText();
            }

            private void DisplayMenuEntrySelected(object sender, EventArgs e)
            {
                display = !display;

                SetMenuEntryText();

                /*if(displayMenuEntry.Text == "Pleine ecran: " + "on")
                    Activate*/
            }

            private void VolumeMenuEntrySelected(object sender, EventArgs e)
            {
                volume++;

                SetMenuEntryText();
            }
                     
            void SetMenuEntryText()
            {
                keyboardMenuEntry.Text = "Clavier: " + keyboard[currentKeyboard];
                languageMenuEntry.Text = "Language: " + languages[currentLanguage];
                displayMenuEntry.Text = "Pleine ecran: " + (display ? "off" : "on");
                volumeMenuEntry.Text = "Volume: " + volume;
            }
        #endregion   
    }
}
