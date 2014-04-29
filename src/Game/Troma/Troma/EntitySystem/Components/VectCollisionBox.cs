using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine;

namespace Troma
{
    public class VectCollisionBox : EntityComponent
    {
        public List<BoundingBox> BoxList { get; private set; }

        public VectCollisionBox(Entity aParent)
            : base(aParent)
        {
            Name = "VectCollisionBox";
            _requiredComponents.Add("Model3D");
            _requiredComponents.Add("VectTransform");

            BoxList = new List<BoundingBox>();
        }

        public override void Start()
        {
            base.Start();
            GenerateBoundingBox();
            CollisionManager.AddBox(BoxList);
        }

        private void GenerateBoundingBox()
        {
            Func<int, Matrix> GetWorld = Entity.GetComponent<VectTransform>().GetWorld;
            int length = Entity.GetComponent<VectTransform>().Length;

            Model model = Entity.GetComponent<Model3D>().Model;
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

                for (int i = 0; i < length; i++)
                {
                    Vector3 _min = Vector3.Transform(min, GetWorld(i));
                    Vector3 _max = Vector3.Transform(max, GetWorld(i));

                    BoxList.Add(new BoundingBox(_min, _max));
                }
            }
        }
    }
}
