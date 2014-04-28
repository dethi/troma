using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine;

namespace Troma
{
    public class CollisionBox : EntityComponent
    {
        public List<BoundingBox> BoxList;

        public CollisionBox(Entity aParent)
            : base(aParent)
        {
            Name = "CollisionBox";
            _requiredComponents.Add("Model3D");

            BoxList = new List<BoundingBox>();
        }

        public override void Start()
        {
            base.Start();
            GenerateBoundingBox();
        }

        private void GenerateBoundingBox()
        {
            Model model = Entity.GetComponent<Model3D>().Model;
            Matrix world = Entity.GetComponent<Transform>().World;

            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                Matrix meshTransform = transforms[mesh.ParentBone.Index];

                Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
                Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    int vertexStride = meshPart.VertexBuffer.VertexDeclaration.VertexStride;
                    int vertexBufferSize = meshPart.NumVertices * vertexStride;

                    float[] vertexData = new float[vertexBufferSize / sizeof(float)];
                    meshPart.VertexBuffer.GetData<float>(vertexData);

                    for (int i = 0; i < vertexBufferSize / sizeof(float); i += vertexStride / sizeof(float))
                    {
                        Vector3 transformedPosition = Vector3.Transform(
                            new Vector3(vertexData[i], vertexData[i + 1], vertexData[i + 2]), 
                            meshTransform);

                        min = Vector3.Min(min, transformedPosition);
                        max = Vector3.Max(max, transformedPosition);
                    }
                }

                min = Vector3.Transform(min, world);
                max = Vector3.Transform(max, world);

                BoxList.Add(new BoundingBox(min, max));
            }
        }
    }
}
