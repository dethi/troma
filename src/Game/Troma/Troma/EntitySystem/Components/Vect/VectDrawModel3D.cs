﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine;

namespace Troma
{
    public class VectDrawModel3D : DrawableEntityComponent
    {
        private Effect _effect;
        private Texture2D _texture;
        private Texture2D _normalMap;
        private bool hasNormalMap;

        public VectDrawModel3D(Entity aParent, Effect effect)
            : base(aParent)
        {
            Name = "VectDrawModel3D";
            _requiredComponents.Add("Model3D");
            _requiredComponents.Add("VectTransform");

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
            Func<int, Matrix> GetWorld = Entity.GetComponent<VectTransform>().GetWorld;
            int length = Entity.GetComponent<VectTransform>().Length;

            Model model = Entity.GetComponent<Model3D>().Model;

            _effect.Parameters["View"].SetValue(camera.View);
            _effect.Parameters["Projection"].SetValue(camera.Projection);

            if (hasNormalMap)
                _effect.Parameters["EyePosition"].SetValue(camera.Position);

            for (int i = 0; i < length; i++)
            {
                _effect.Parameters["World"].SetValue(GetWorld(i));

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
}
