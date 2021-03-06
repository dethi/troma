﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine;

namespace Troma
{
    public class DrawModel3D : DrawableEntityComponent
    {
        private Effect _effect;
        private Texture2D _texture;
        private Texture2D _normalMap;
        private bool hasNormalMap;

        public DrawModel3D(Entity aParent, Effect effect)
            : base(aParent)
        {
            Name = "DrawModel3D";
            _requiredComponents.Add("Model3D");

            _effect = effect;
            hasNormalMap = (effect.Name == "GameObjectWithNormal");
        }

        public override void Start()
        {
            base.Start();

            string model = Entity.GetComponent<Model3D>().ModelName;
            _texture = FileManager.Load<Texture2D>("Models/" + model + "_Texture");

            if (hasNormalMap)
                _normalMap = FileManager.Load<Texture2D>("Models/" + model + "_Normal");

            _effect.CurrentTechnique = _effect.Techniques["Technique1"];

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

            _effect.Parameters["World"].SetValue(world);
            _effect.Parameters["View"].SetValue(camera.View);
            _effect.Parameters["Projection"].SetValue(camera.Projection);

            _effect.Parameters["ColorMap"].SetValue(_texture);

            if (hasNormalMap)
            {
                _effect.Parameters["NormalMap"].SetValue(_normalMap);
                _effect.Parameters["EyePosition"].SetValue(camera.Position);
            }

            foreach (ModelMesh mesh in model.Meshes)
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
}
