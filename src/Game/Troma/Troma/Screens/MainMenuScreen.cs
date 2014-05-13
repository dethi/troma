using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine;

namespace Troma
{
    public class MainMenuScreen : MenuScreen
    {
        private int x1;
        private int x2;
        private int x3;
        private int x4;

        private Texture2D background;
        private Texture2D balle_gauche;
        private Texture2D balle_droite;
        private Texture2D logo;

        private string Text1 = "Suivez-nous sur les reseaux sociaux !";
        private string Text2 = "twitter.com/Emagine_Studio";
        private string Text3 = "facebook.com/EmagineStudio2018";
        private string Text4 = "Troma.eu";


        public MainMenuScreen(Game game)
            : base(game, "Troma")
        {
            // Create menu entries.
            MenuEntry soloMenuEntry = new MenuEntry("Solo");
            MenuEntry multiMenuEntry = new MenuEntry("Multijoueur");
            MenuEntry optionsMenuEntry = new MenuEntry("Options");
            MenuEntry exitMenuEntry = new MenuEntry("Quitter");

            // Hook up menu event handlers.
            soloMenuEntry.Selected += SoloMenuEntrySelected;
            multiMenuEntry.Selected += MultiMenuEntrySelected;
            optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(soloMenuEntry);
            MenuEntries.Add(multiMenuEntry);
            MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(exitMenuEntry);

            SceneRenderer.InitializeMenu();
        }

        public override void LoadContent()
        {
            base.LoadContent();

            x1 = 0;
            x2 = 0;
            x3 = 0;
            x4 = 0;

            background = FileManager.Load<Texture2D>("Menus/Fond");
            balle_gauche = FileManager.Load<Texture2D>("Menus/balle-droite");
            balle_droite = FileManager.Load<Texture2D>("Menus/balle-gauche");
            logo = FileManager.Load<Texture2D>("Menus/eie");

            SoundManager.Play("Menu");
        }

        public override void Draw(GameTime gameTime)
        {
            int width = game.GraphicsDevice.Viewport.Width;
            int height = game.GraphicsDevice.Viewport.Height;

            Color c = new Color(120, 110, 100) * TransitionAlpha;
            Rectangle posBackground = new Rectangle(0, 0, width, height);
            Rectangle posLogo = new Rectangle(width - 100, height - 100, 100, 100);

            x1 = (x1 < 2100) ? (x1 + 1) : -500;
            x2 = (x2 > 0) ? (x2 - 2) : 3900;
            x3 = (x3 > 0) ? (x3 - 2) : 2900;
            x4 = (x4 < 2300) ? (x4 + 2) : -700;

            /*
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
             */

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
            float textScale1 = 0.00015f * width;
            float textScale2 = 0.00013f * width;
            float textScale3 = 0.00011f * width;

            Vector2 Position1 = new Vector2(
                x1 * width / 1920,
                height - (57 * width / 1920));
            
            Vector2 Position2 = new Vector2(
                x2 * width / 1920,
                height - (75 * width / 1920));

            Vector2 Position3 = new Vector2(
                x3 * width / 1920,
                height - (20 * width / 1920));

            Vector2 Position4 = new Vector2(
                x4 * width / 1920,
                height - (38 * width / 1920));


            GameServices.SpriteBatch.Begin();

            GameServices.SpriteBatch.Draw(background, posBackground, Color.White * TransitionAlpha);
            
            //GameServices.SpriteBatch.Draw(balle_gauche, rect1, c);
            //GameServices.SpriteBatch.Draw(balle_droite, rect2, c);
            //GameServices.SpriteBatch.Draw(balle_droite, rect3, c);

            GameServices.SpriteBatch.DrawString(Font, menuTitle, titlePosition, titleColor, 0,
                titleOrigin, titleScale, SpriteEffects.None, 0);

            GameServices.SpriteBatch.DrawString(Font, Text1, Position1, c, 0, titleOrigin, textScale1, SpriteEffects.None, 0);
            GameServices.SpriteBatch.DrawString(Font, Text2, Position2, c, 0, titleOrigin, textScale2, SpriteEffects.None, 0);
            GameServices.SpriteBatch.DrawString(Font, Text3, Position3, c, 0, titleOrigin, textScale2, SpriteEffects.None, 0);
            GameServices.SpriteBatch.DrawString(Font, Text4, Position4, c, 0, titleOrigin, textScale3, SpriteEffects.None, 0);

            GameServices.SpriteBatch.Draw(logo, posLogo, Color.White * TransitionAlpha * 0.6f);

            // Draw each menu entry in turn.
            for (int i = 0; i < MenuEntries.Count; i++)
            {
                bool isSelected = IsActive && (i == selectedEntry);
                MenuEntries[i].Draw(gameTime, this, isSelected);
            }

            GameServices.SpriteBatch.End();
        }

        private void SoloMenuEntrySelected(object sender, EventArgs e)
        {
            SoundManager.Stop();
            LoadingScreen.Load(game, this.ScreenManager, true, new SoloScreen(game, ""));
        }

        private void MultiMenuEntrySelected(object sender, EventArgs e)
        {
            //SoundManager.Stop();
        }

        private void OptionsMenuEntrySelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen(game));
        }

        protected override void OnCancel()
        {
            game.Exit();
        }

        private void OnCancel(object sender, EventArgs e)
        {
            SoundManager.Stop();
            OnCancel();
        }
    }
}
