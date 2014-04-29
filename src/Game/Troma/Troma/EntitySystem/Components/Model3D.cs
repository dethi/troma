using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine;

namespace Troma
{
    public class Model3D : EntityComponent
    {
        public Model Model;
        public string ModelName;

        public Model3D(Entity aParent, string model)
            : base(aParent)
        {
            Name = "Model3D";
            _requiredComponents.Add("Transform");

            Model = FileManager.Load<Model>("Models/" + model);
            ModelName = model;

#if DEBUG
            foreach(ModelMesh mesh in Model.Meshes)
            {
                foreach(ModelMeshPart meshPart in mesh.MeshParts)
                    NbVects.Add(meshPart.NumVertices);
            }
#endif
        }
    }
}