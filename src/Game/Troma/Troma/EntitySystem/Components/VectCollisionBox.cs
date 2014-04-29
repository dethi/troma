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

                Vector3 meshMin = new Vector3(float.MaxValue);
                Vector3 meshMax = new Vector3(float.MinValue);

                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    int stride = meshPart.VertexBuffer.VertexDeclaration.VertexStride;

                    VertexPositionNormalTexture[] vertexData =
                        new VertexPositionNormalTexture[meshPart.NumVertices];

                    meshPart.VertexBuffer.GetData(
                        meshPart.VertexOffset * stride, vertexData, 0,
                        meshPart.NumVertices, stride);

                    Vector3 vertPosition = new Vector3();

                    for (int i = 0; i < vertexData.Length; i++)
                    {
                        vertPosition = vertexData[i].Position;

                        meshMin = Vector3.Min(meshMin, vertPosition);
                        meshMax = Vector3.Max(meshMax, vertPosition);
                    }
                }

                for (int i = 0; i < length; i++)
                {
                    Vector3 _min = Vector3.Transform(meshMin, GetWorld(i));
                    Vector3 _max = Vector3.Transform(meshMax, GetWorld(i));

                    BoxList.Add(new BoundingBox(_min, _max));
                }
            }
        }
    }
}
