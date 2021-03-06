﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine;

namespace Troma
{
    public class DrawWeapon : DrawableEntityComponent
    {
        private Texture2D chargeur;
        private SpriteFont Font;

        private Texture2D _cross;

        Vector2 titleOrigin = new Vector2(0, 0);
        Color c = new Color(170, 170, 170);
        Color orange = new Color(215, 145, 96);
        Color red = new Color(207, 46, 49);
        Color color;

        public DrawWeapon(Entity aParent)
            : base(aParent)
        {
            Name = "DrawWeapon";
            _requiredComponents.Add("Weapon");
        }

        public override void Start()
        {
            base.Start();

            _cross = FileManager.Load<Texture2D>("cross");
            Font = FileManager.Load<SpriteFont>("Fonts/HUD");
            chargeur = FileManager.Load<Texture2D>("Menus/Chargeur");

            Font.Spacing = 4f;
        }

        public override void Draw(GameTime gameTime, ICamera camera)
        {
            Entity.GetComponent<Weapon>().Muzzle.DrawHUD( 
                Entity.GetComponent<Weapon>().SightPosition);

            GameServices.ResetGraphicsDeviceFor3D();

            Entity.GetComponent<Weapon>().Arms.Draw(gameTime, camera);
        }

        public override void DrawHUD(GameTime gameTime)
        {
            int width = GameServices.GraphicsDevice.Viewport.Width;
            int height = GameServices.GraphicsDevice.Viewport.Height;
            int size = (64 * width) / 1920;

            WeaponInfo weaponInfo = Entity.GetComponent<Weapon>().Info;

            string Text1 = weaponInfo.Munition.ToString();
            string Text2 = " / ";
            string Text3 = (weaponInfo.MunitionPerLoader * weaponInfo.Loader).ToString();

            float textScale1 = 0.00070f * width;
            float textScale2 = 0.00050f * width;
            float textScale3 = 0.00080f * width;
            float textScale4 = 0.00055f * width;
            float textScale5 = 0.00030f * width;

            Rectangle chargeurImage1 = new Rectangle(1850 * width / 1920, height - (110 * width / 1920), 20 * width / 1920, 20 * width / 1920);
            Rectangle chargeurImage2 = new Rectangle(1860 * width / 1920, height - (110 * width / 1920), 20 * width / 1920, 20 * width / 1920);
            Rectangle chargeurImage3 = new Rectangle(1870 * width / 1920, height - (110 * width / 1920), 20 * width / 1920, 20 * width / 1920);

            Rectangle chargeurImage4 = new Rectangle(1850 * width / 1920, height - (90 * width / 1920), 20 * width / 1920, 20 * width / 1920);
            Rectangle chargeurImage5 = new Rectangle(1860 * width / 1920, height - (90 * width / 1920), 20 * width / 1920, 20 * width / 1920);
            Rectangle chargeurImage6 = new Rectangle(1870 * width / 1920, height - (90 * width / 1920), 20 * width / 1920, 20 * width / 1920);

            Rectangle chargeurImage7 = new Rectangle(1850 * width / 1920, height - (70 * width / 1920), 20 * width / 1920, 20 * width / 1920);
            Rectangle chargeurImage8 = new Rectangle(1860 * width / 1920, height - (70 * width / 1920), 20 * width / 1920, 20 * width / 1920);
            Rectangle chargeurImage9 = new Rectangle(1870 * width / 1920, height - (70 * width / 1920), 20 * width / 1920, 20 * width / 1920);

            if (weaponInfo.Munition == 0)
            {
                color = red;
            }
            else if (weaponInfo.Munition < 3 && weaponInfo.Munition != 0)
            {
                color = orange;
            }
            else color = c;

            Vector2 Position1 = new Vector2(
            1680 * width / 1920,
            height - (120 * width / 1920));

            Vector2 Position3 = new Vector2(
            1678 * width / 1920,
            height - (125 * width / 1920));

            Vector2 Position2 = new Vector2(
            1722 * width / 1920,
            height - (105 * width / 1920));

            Vector2 Position5 = new Vector2(
            1720 * width / 1920,
            height - (108 * width / 1920));

            Vector2 Position4 = new Vector2(
            1770 * width / 1920,
            height - (105 * width / 1920));

            Vector2 Position6 = new Vector2(
            1768 * width / 1920,
            height - (108 * width / 1920));

            Vector2 Position7 = new Vector2(
            1680 * width / 1920,
            height - (150 * width / 1920));

            //affichage HUD arme + ombre
            GameServices.SpriteBatch.Begin();
            GameServices.SpriteBatch.DrawString(Font, Text1, Position3, Color.Black * 0.3f, 0, titleOrigin, textScale3, SpriteEffects.None, 0);
            GameServices.SpriteBatch.DrawString(Font, Text2, Position5, Color.Black * 0.2f, 0, titleOrigin, textScale4, SpriteEffects.None, 0);
            GameServices.SpriteBatch.DrawString(Font, Text3, Position6, Color.Black * 0.2f, 0, titleOrigin, textScale4, SpriteEffects.None, 0);
            GameServices.SpriteBatch.DrawString(Font, Text1, Position1, color, 0, titleOrigin, textScale1, SpriteEffects.None, 0);
            GameServices.SpriteBatch.DrawString(Font, Text2, Position2, c, 0, titleOrigin, textScale2, SpriteEffects.None, 0);
            GameServices.SpriteBatch.DrawString(Font, Text3, Position4, c, 0, titleOrigin, textScale2, SpriteEffects.None, 0);

            GameServices.SpriteBatch.DrawString(Font, Entity.GetComponent<Weapon>().Info.Name, Position7, c, 0, titleOrigin, textScale5, SpriteEffects.None, 0);

            //affichage chargeur
            if (weaponInfo.Loader > 0) GameServices.SpriteBatch.Draw(chargeur, chargeurImage1, Color.White * 0.8f);
            if (weaponInfo.Loader > 1) GameServices.SpriteBatch.Draw(chargeur, chargeurImage2, Color.White * 0.8f);
            if (weaponInfo.Loader > 2) GameServices.SpriteBatch.Draw(chargeur, chargeurImage3, Color.White * 0.8f);

            if (weaponInfo.Loader > 3) GameServices.SpriteBatch.Draw(chargeur, chargeurImage4, Color.White * 0.8f);
            if (weaponInfo.Loader > 4) GameServices.SpriteBatch.Draw(chargeur, chargeurImage5, Color.White * 0.8f);
            if (weaponInfo.Loader > 5) GameServices.SpriteBatch.Draw(chargeur, chargeurImage6, Color.White * 0.8f);

            if (weaponInfo.Loader > 6) GameServices.SpriteBatch.Draw(chargeur, chargeurImage7, Color.White * 0.8f);
            if (weaponInfo.Loader > 7) GameServices.SpriteBatch.Draw(chargeur, chargeurImage8, Color.White * 0.8f);
            if (weaponInfo.Loader > 8) GameServices.SpriteBatch.Draw(chargeur, chargeurImage9, Color.White * 0.8f);


            if (!Entity.GetComponent<Weapon>().SightPosition)
            {
                #region Cross

                Rectangle rect = new Rectangle(
                    (width - size) / 2,
                    (height - size) / 2,
                    size, size);

                GameServices.SpriteBatch.Draw(_cross, rect, Color.White);

                #endregion
            }

            GameServices.SpriteBatch.End();
        }
    }
}
