using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EquinoxEngine.Terrain
{
    public class QuadTree
    {
        #region Fields

        private QuadNode _rootNode;
        private TreeVertexCollection _vertices;
        private BufferManager _buffers;
        private Vector3 _position;
        private int _topNodeSize;

        private QuadNode _activeNode;

        private Vector3 _cameraPosition;
        private Vector3 _lastCameraPosition;

        public int[] Indices;

        public Matrix View;
        public Matrix Projection;

        public BasicEffect Effect;
        public GraphicsDevice Device;

        public int MinimumDepth;
        public bool Cull;

        public int TopNodeSize
        {
            get { return _topNodeSize; }
        }

        public QuadNode RootNode
        {
            get { return _rootNode; }
        }

        public TreeVertexCollection Vertices
        {
            get { return _vertices; }
        }

        public Vector3 CameraPosition
        {
            get { return _cameraPosition; }
            set { _cameraPosition = value; }
        }

        public BoundingFrustum ViewFrustrum { get; private set; }
        public int IndexCount { get; private set; }

        #endregion

        public QuadTree(Vector3 position, Texture2D heightmap, Matrix viewMatrix,
            Matrix projectionMatrix, GraphicsDevice device, int scale)
        {
            Device = device;
            _position = position;
            _topNodeSize = heightmap.Width - 1;

            _vertices = new TreeVertexCollection(position, heightmap, scale);
            _buffers = new BufferManager(_vertices.Vertices, device);
            _rootNode = new QuadNode(NodeType.FullNode, _topNodeSize, 1, null, this, 0);

            View = viewMatrix;
            Projection = projectionMatrix;
            ViewFrustrum = new BoundingFrustum(viewMatrix * projectionMatrix);

            #region BasicEffect

            Effect = new BasicEffect(device);
            Effect.EnableDefaultLighting();
            Effect.FogEnabled = true;
            Effect.FogStart = 300f;
            Effect.FogEnd = 1000f;
            Effect.FogColor = Color.Black.ToVector3();
            Effect.TextureEnabled = true;
            Effect.Texture = new Texture2D(device, 100, 100);
            Effect.Projection = projectionMatrix;
            Effect.View = viewMatrix;
            Effect.World = Matrix.Identity;

            #endregion

            ViewFrustrum = new BoundingFrustum(viewMatrix * projectionMatrix);

            //Construct an array large enough to hold all of the indices we'll need.
            Indices = new int[((heightmap.Width + 1) * (heightmap.Height + 1)) * 3];
        }

        public void Update(GameTime gameTime)
        {
            if (_cameraPosition != _lastCameraPosition)
            {
                Effect.View = View;
                Effect.Projection = Projection;
                ViewFrustrum.Matrix = View * Projection;

                _lastCameraPosition = _cameraPosition;
                IndexCount = 0;

                _rootNode.Merge();
                _rootNode.EnforceMinimumDepth();
                _activeNode = _rootNode.DeepestNodeWithPoint(CameraPosition);

                if (_activeNode != null)
                    _activeNode.Split();

                _rootNode.SetActiveVertices();

                _buffers.UpdateIndexBuffer(Indices, IndexCount);
                _buffers.SwapBuffer();
            }
        }

        public void Draw(GameTime gameTime)
        {
            Device.SetVertexBuffer(_buffers.VertexBuffer);
            Device.Indices = _buffers.IndexBuffer;

            foreach (EffectPass pass in Effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                Device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 
                    _vertices.Vertices.Length, 0, IndexCount / 3);
            }
        }

        internal void UpdateBuffer(int vIndex)
        {
            Indices[IndexCount] = vIndex;
            IndexCount++;
        }
    }
}
