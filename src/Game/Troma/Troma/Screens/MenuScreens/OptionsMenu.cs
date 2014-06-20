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
    class OptionsMenu : MenuScreen
    {
        private Stepper keyboardMenuEntry;
        private Stepper languageMenuEntry;
        private Numerous volumeMenuEntry;
        private Button graphicsMenuEntry;
        private Button keysMenuEntry;
        private Button backMenuEntry;

        private Texture2D bg;
        private Texture2D bgTrans;
        private Texture2D arrow;

        private Rectangle bgRect;
        private Rectangle bgTransRect;
        private Rectangle arrowRect;

        public OptionsMenu(Game game)
            : base(game)
        {
            Vector2 entryPos = new Vector2(143, 143);
            float space = 100;

            // Create menu entries.
            keyboardMenuEntry = new Stepper(String.Empty, 1, entryPos, 2, 
                ((Settings.Keyboard == "AZERTY") ? 0 : 1));
            entryPos.Y += space;
            languageMenuEntry = new Stepper(String.Empty, 1, entryPos, 2,
                ((Settings.Language == "Francais") ? 0 : 1));
            entryPos.Y += space;
            volumeMenuEntry = new Numerous(String.Empty, 1, entryPos, 0, 200, 10, (int)(Settings.MusicVolume * 100f));
            entryPos.Y += space;
            graphicsMenuEntry = new Button(String.Empty, 1, entryPos);
            entryPos.Y += space;
            keysMenuEntry = new Button(String.Empty, 1, entryPos);
            entryPos.Y = 875;
            backMenuEntry = new Button(Resource.Back, 1, entryPos);

            // Hook up menu event handlers.
            keyboardMenuEntry.ChangedValue += KeyboardChangedValue;
            languageMenuEntry.ChangedValue += LanguageChangedValue;
            volumeMenuEntry.ChangedValue += VolumeChangedValue;
            graphicsMenuEntry.Selected += GraphicsEntrySelected;
            keysMenuEntry.Selected += KeysEntrySelected;
            backMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(keyboardMenuEntry);
            MenuEntries.Add(languageMenuEntry);
            MenuEntries.Add(volumeMenuEntry);
            MenuEntries.Add(graphicsMenuEntry);
            MenuEntries.Add(keysMenuEntry);
            MenuEntries.Add(backMenuEntry);

            SetMenuEntryText();
        }

        public override void LoadContent()
        {
            base.LoadContent();

            bg = GameServices.Game.Content.Load<Texture2D>("Menus/background-blur");
            bgTrans = GameServices.Game.Content.Load<Texture2D>("Menus/translucide");
            arrow = GameServices.Game.Content.Load<Texture2D>("Menus/arrow");

            bgRect = new Rectangle(0, 0, 1920, 1080);
            bgTransRect = new Rectangle(0, 0, 0, 0);
            arrowRect = new Rectangle(0, 0, 64, 64);
        }

        public override void Draw(GameTime gameTime)
        {
            int width = GameServices.GraphicsDevice.Viewport.Width;
            int height = GameServices.GraphicsDevice.Viewport.Height;

            float widthScale = (float)width / 1920;
            float heightScale = (float)height / 1080;
            float scale = (widthScale + heightScale) / 2;

            bgRect.Height = height;
            bgRect.Width = (int)(height * 1.778f);
            bgRect.X = -(bgRect.Width - width) / 2;

            Vector2 columnsPos = new Vector2(800 * widthScale, 0);

            bgTransRect.X = (int)(123 * widthScale);
            bgTransRect.Y = (int)(123 * heightScale);
            bgTransRect.Width = (int)(columnsPos.X + 270 * scale);
            bgTransRect.Height = (int)((4 * 100) * heightScale + (SpriteFont.LineSpacing + 40) * scale);

            GameServices.SpriteBatch.Begin();

            GameServices.SpriteBatch.Draw(bg, bgRect, Color.White * ((IsExiting) ? TransitionAlpha : 1));
            GameServices.SpriteBatch.Draw(bgTrans, bgTransRect, Color.White * TransitionAlpha * 0.15f);

            // Draw each menu entry in turn.
            for (int i = 0; i < MenuEntries.Count; i++)
            {
                bool isSelected = IsActive && (i == selectedEntry);
                MenuEntries[i].ColumnsPos = columnsPos;
                MenuEntries[i].Draw(gameTime, this, isSelected);

                if (isSelected)
                {
                    arrowRect.Height = (int)(64 * ((widthScale + heightScale) / 2));
                    arrowRect.Width = arrowRect.Height;
                    arrowRect.X = (int)(MenuEntries[i].Position.X - 1.5f * arrowRect.Width);
                    arrowRect.Y = (int)MenuEntries[i].Position.Y;
                    GameServices.SpriteBatch.Draw(arrow, arrowRect, Color.White * TransitionAlpha);
                }
            }

            GameServices.SpriteBatch.End();
        }

        private void KeyboardChangedValue(object sender, EventArgs e)
        {
            Settings.Keyboard = keyboardMenuEntry.Choices[keyboardMenuEntry.SelectedChoice];
        }

        private void LanguageChangedValue(object sender, EventArgs e)
        {
            Settings.Language = languageMenuEntry.Choices[languageMenuEntry.SelectedChoice];
            SetMenuEntryText();
        }

        private void VolumeChangedValue(object sender, EventArgs e)
        {
            Settings.MusicVolume = (float)volumeMenuEntry.Value / 100f;
        }

        private void GraphicsEntrySelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new GraphicsMenu(game));
        }

        private void KeysEntrySelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new HelpMenu(game));
        }

        protected override void OnCancel(object sender, EventArgs e)
        {
            Settings.Save();
            ExitScreen();
        }

        private void SetMenuEntryText()
        {
            keyboardMenuEntry.Text = Resource.labelKeyboard;
            keyboardMenuEntry.Choices[0] = "AZERTY";
            keyboardMenuEntry.Choices[1] = "QWERTY";

            languageMenuEntry.Text = Resource.labelLanguage;
            languageMenuEntry.Choices[0] = "Francais";
            languageMenuEntry.Choices[1] = "English";

            volumeMenuEntry.Text = "Volume";
            graphicsMenuEntry.Text = Resource.Graphics;
            keysMenuEntry.Text = Resource.SummonsKeyboard;
            backMenuEntry.Text = Resource.Back;
        }
    }
}
