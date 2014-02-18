using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Arrow
{
    public class Button : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Game game;
        private SpriteBatch spriteBatch;

        private Rectangle bouton;
        private bool isOn = false;
        private bool isClick = false;

        private string nameTextureIsOff;
        private string nameTextureIsOn;
        private Texture2D textureIsOff;
        private Texture2D textureIsOn;
        
        private float transparence;
        
        public delegate void Delegate();
        Menu.Delegate boutonDelegate2;
        
        public Button(Game game, int x, int y, int width, int height, string nameTextureIsOff, string nameTextureIsOn, MenuPause.Delegate boutonDelegate, float transparence)
            : base(game)
        {
            this.game = game;
            this.bouton = new Rectangle(x, y, width, height);
            this.nameTextureIsOff = "Textures/" + nameTextureIsOff;
            this.nameTextureIsOn = "Textures/" + nameTextureIsOn;
            boutonDelegate2 = boutonDelegate;
            this.transparence = transparence;
        }

        public override void Initialize()
        {
            this.spriteBatch = new SpriteBatch(this.Game.GraphicsDevice);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            textureIsOff = game.Content.Load<Texture2D>(nameTextureIsOff);
            textureIsOn = game.Content.Load<Texture2D>(nameTextureIsOn);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();

            // Test si on est sur l'image
            if ((mouse.X >= bouton.Left) && (mouse.X <= bouton.Right) && (mouse.Y >= bouton.Top) &&
                (mouse.Y <= bouton.Bottom))
                isOn = true;
            else
                isOn = false;

            // Test si on clique sur l'image
            if (mouse.LeftButton == ButtonState.Pressed && (mouse.X >= bouton.Left) &&
                (mouse.X <= bouton.Right) && (mouse.Y >= bouton.Top) && (mouse.Y <= bouton.Bottom))
                isClick = true;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            this.spriteBatch.Begin();
            //Change l'image en fonction de la position du curseur
            if (isOn == false)
                this.spriteBatch.Draw(this.textureIsOn, bouton, Color.White * transparence);
            else
                this.spriteBatch.Draw(this.textureIsOff, bouton, Color.White * transparence);
            this.spriteBatch.End();

            //Charge une nouvelle image si on clique
            if (isClick == true)
            {
                boutonDelegate2();
                isClick = false;          
            }

            base.Draw(gameTime);
        }
    }
}
