using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ClientServerExtension
{
    [Serializable()]
    public class Box : ISerializable
    {
        private List<BoundingBox> _boundingBox;

        public BoundingBox[] BoudingBox
        {
            get { return _boundingBox.ToArray(); }
        }

        public Box()
        {
            _boundingBox = new List<BoundingBox>();
        }

        public Box(List<BoundingBox> list)
        {
            _boundingBox = new List<BoundingBox>();
            _boundingBox.AddRange(list);
        }

        public Box(SerializationInfo info, StreamingContext ctxt)
        {
            _boundingBox = new List<BoundingBox>(
                (BoundingBox[])info.GetValue("BoxList", typeof(BoundingBox[])));
        }

        public void Save(string filename)
        {
            Stream stream = File.Open(filename + ".bin", FileMode.Create);
            BinaryFormatter bFormatter = new BinaryFormatter();
            bFormatter.Serialize(stream, this);
            stream.Close();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("BoxList", _boundingBox.ToArray());
        }

        public void Generate(Model model)
        {
            Generate(model, Matrix.Identity);
        }

        public void Generate(Model model, Matrix world)
        {
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

                meshMin = Vector3.Transform(meshMin, world);
                meshMax = Vector3.Transform(meshMax, world);

                _boundingBox.Add(new BoundingBox(meshMin, meshMax));
            }
        }

        public BoundingBox[] ComputeWorldTranslation(Matrix world)
        {
            List<BoundingBox> newList = new List<BoundingBox>();
            Vector3 min;
            Vector3 max;

            foreach (BoundingBox box in _boundingBox)
            {
                min = Vector3.Transform(box.Min, world);
                max = Vector3.Transform(box.Max, world);

                newList.Add(new BoundingBox(min, max));
            }

            return newList.ToArray();
        }

        public void Clear()
        {
            _boundingBox.Clear();
        }
    }
}
