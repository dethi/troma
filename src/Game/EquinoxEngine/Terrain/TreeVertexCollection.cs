using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EquinoxEngine.Terrain
{
    public class TreeVertexCollection
    {
        public VertexPositionNormalTexture[] Vertices;

        Vector3 _position;
        int _topSize;
        int _halfSize;
        int _vertexCount;
        int _scale;

        public VertexPositionNormalTexture this[int index]
        {
            get { return Vertices[index]; }
            set { Vertices[index] = value; }
        }

        public TreeVertexCollection(Vector3 pos, Texture2D heightmap, int scale)
        {
            _scale = scale;
            _topSize = heightmap.Width - 1;
            _halfSize = _topSize / 2;
            _position = pos;
            _vertexCount = heightmap.Width * heightmap.Width;

            Vertices = new VertexPositionNormalTexture[_vertexCount];
            BuildVertices(heightmap);
            CalculateAllNormals();
        }

        private void BuildVertices(Texture2D heightmap)
        {
            Color[] heightmapColor = new Color[_vertexCount];
            heightmap.GetData(heightmapColor);

            float x = _position.X;
            float y = _position.Y;
            float z = _position.Z;
            float max_X = x + _topSize;

            for (int i = 0; i < _vertexCount; i++)
            {
                if (x > max_X)
                {
                    x = _position.X;
                    z++;
                }

                y = _position.Y + (heightmapColor[i].R / 5.0f);

                var vert = new VertexPositionNormalTexture(
                    new Vector3(x * _scale, y * _scale, z * _scale), 
                    Vector3.Zero, 
                    Vector2.Zero);
                vert.TextureCoordinate = new Vector2(
                    (vert.Position.X - _position.X) / _topSize,
                    (vert.Position.Z - _position.Z) / _topSize);
                Vertices[i] = vert;

                x++;
            }
        }

        private void CalculateAllNormals()
        {
            if (_vertexCount < 9)
                return;

            int i = _topSize + 2;
            int j = 0;
            int k = i + _topSize;

            for (int n = 0; i <= (_vertexCount - _topSize) - 2; i += 2, n++, j += 2, k += 2)
            {
                if (n == _halfSize)
                {
                    n = 0;
                    i += _topSize + 2;
                    j += _topSize + 2;
                    k += _topSize + 2;
                }

                SetNormals(i, j, j+1);
                SetNormals(i, j + 1, j + 2);
                SetNormals(i, j + 2, i + 1);
                SetNormals(i, i + 1, k + 2);
                SetNormals(i, k + 2, k + 1);
                SetNormals(i, k, i - 1);
                SetNormals(i, i - 1, j);
            }
        }

        private void SetNormals(int idx1, int idx2, int idx3)
        {
            if (idx3 >= Vertices.Length)
                idx3 = Vertices.Length - 1;

            Vector3 normal = Vector3.Cross(
                Vertices[idx2].Position - Vertices[idx1].Position,
                Vertices[idx1].Position - Vertices[idx3].Position);
            normal.Normalize();

            Vertices[idx1].Normal += normal;
            Vertices[idx2].Normal += normal;
            Vertices[idx3].Normal += normal;
        }
    }
}
