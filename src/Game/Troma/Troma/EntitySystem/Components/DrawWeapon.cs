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
        private Effect _effect;
        private Texture2D _texture;
        private Texture2D _normalMap;
        private bool hasNormalMap;

        private Texture2D chargeur;
        private SpriteFont Font;

        private Texture2D _cross;

        public DrawWeapon(Entity aParent, Effect effect)
            : base(aParent)
        {
            Name = "DrawWeapon";
            _requiredComponents.Add("Model3D");
            _requiredComponents.Add("Weapon");

            _effect = effect;
            hasNormalMap = (effect.Name == "GameObjectWithNormal");
        }

        public override void Start()
        {
            base.Start();

            string model = Entity.GetComponent<Model3D>().ModelName;
            _texture = FileManager.Load<Texture2D>("Models/" + model + "_Texture");
            _cross = FileManager.Load<Texture2D>("cross");

            if (hasNormalMap)
                _normalMap = FileManager.Load<Texture2D>("Models/" + model + "_Normal");

            _effect.CurrentTechnique = _effect.Techniques["Technique1"];

            _effect.Parameters["ColorMap"].SetValue(_texture);

            if (hasNormalMap)
                _effect.Parameters["NormalMap"].SetValue(_normalMap);

            _effect.Parameters["AmbientColor"].SetValue(LightInfo.AmbientColor);
            _effect.Parameters["AmbientIntensity"].SetValue(LightInfo.AmbientIntensity);

            _effect.Parameters["LightDirection"].SetValue(LightInfo.LightDirection);
            _effect.Parameters["DiffuseColor"].SetValue(LightInfo.DiffuseColor);
            _effect.Parameters["DiffuseIntensity"].SetValue(LightInfo.DiffuseIntensity);

            _effect.Parameters["SpecularColor"].SetValue(LightInfo.DiffuseColor);
        }

        public override void Draw(GameTime gameTime, ICamera camera)
        {
            Matrix world = Entity.GetComponent<Transform>().World;
            Model model = Entity.GetComponent<Model3D>().Model;
            WeaponInfo weaponInfo = Entity.GetComponent<Weapon>().Info;

            if (Entity.GetComponent<Weapon>().SightPosition)
            {
                _effect.Parameters["World"].SetValue(weaponInfo.MatrixRotationSight *
                    weaponInfo.MatrixPositionSight * world);
            }
            else
            {
                _effect.Parameters["World"].SetValue(weaponInfo.MatrixRotation *
                    weaponInfo.MatrixPosition * world);
            }

            _effect.Parameters["View"].SetValue(camera.View);
            _effect.Parameters["Projection"].SetValue(camera.Projection);

            if (hasNormalMap)
                _effect.Parameters["EyePosition"].SetValue(camera.Position);

            foreach (ModelMesh mesh in model.Meshes)
            {
                if (mesh.Name != "muzzle_flash")
                {
                    ModelMeshPart meshPart = mesh.MeshParts[0];

                    foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
                    {
                        pass.Apply();

                        GameServices.GraphicsDevice.SetVertexBuffer(meshPart.VertexBuffer, meshPart.VertexOffset);
                        GameServices.GraphicsDevice.Indices = meshPart.IndexBuffer;
                        GameServices.GraphicsDevice.DrawIndexedPrimitives(
                            PrimitiveType.TriangleList, 0, 0, meshPart.NumVertices,
                            meshPart.StartIndex, meshPart.PrimitiveCount);
                    }
                }
            }
        }

        public override void DrawHUD(GameTime gameTime)
        {
            int width = GameServices.GraphicsDevice.Viewport.Width;
            int height = GameServices.GraphicsDevice.Viewport.Height;
            int size = (64 * width) / 1920;

            chargeur = FileManager.Load<Texture2D>("Models/Weapon/Chargeur");
            WeaponInfo weaponInfo = Entity.GetComponent<Weapon>().Info;
            Font = FileManager.Load<SpriteFont>("Fonts/Menu");

            string Text1 = "" + weaponInfo.Munition + " / " + (weaponInfo.MunitionPerLoader * weaponInfo.Loader);
            float textScale1 = 0.00035f * width;
            Rectangle chargeurImage = new Rectangle(1850 * width / 1920, height - (120 * width / 1920), 20, 50);

            Vector2 titleOrigin = new Vector2(0, 0);
            Color c = new Color(170, 170, 170);
            Vector2 Position1 = new Vector2(
            1650 * width / 1920,
            height - (120 * width / 1920));

            GameServices.SpriteBatch.Begin();
            GameServices.SpriteBatch.DrawString(Font, Text1, Position1, c, 0, titleOrigin, textScale1, SpriteEffects.None, 0);
            GameServices.SpriteBatch.Draw(chargeur, chargeurImage, Color.White);
            
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
