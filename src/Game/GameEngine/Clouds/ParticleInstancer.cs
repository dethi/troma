using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine
{
    public class ParticleInstancer
    {
        #region Fields

        private Effect _effect;
        private Texture2D _texture;

        private Vector3 _position;
        private Vector3 _scale;
        private Quaternion _rotation;
        private Vector2 _size;

        public Matrix World
        {
            get
            {
                return Matrix.CreateScale(_scale) * 
                    Matrix.CreateFromQuaternion(_rotation) * 
                    Matrix.CreateTranslation(_position);
            }
        }

        public Dictionary<int, ParticleInstance> Instances;
        public Dictionary<ParticleInstance, Matrix> InstancesTransformMatrices;

        private Color _color;

        private BlendState _blendState;
        private DepthStencilState _depthStencilState;

        private IndexBuffer _indexBuffer;
        private VertexBuffer _vertexBuffer;
        private DynamicVertexBuffer _dynVertexBuffer;

        private static VertexDeclaration InstanceVertexDeclaration = new VertexDeclaration(
            new VertexElement(0, VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 0),
            new VertexElement(16, VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 1),
            new VertexElement(32, VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 2),
            new VertexElement(48, VertexElementFormat.Vector4, VertexElementUsage.BlendWeight, 3));

        #endregion

        #region Initialization

        public ParticleInstancer(Texture2D texture, Effect effect)
        {
            _effect = effect;
            _texture = texture;

            _position = Vector3.Zero;
            _scale = Vector3.One;
            _rotation = Quaternion.Identity;
            _size = new Vector2(0.5f, 0.5f);

            Instances = new Dictionary<int, ParticleInstance>();
            InstancesTransformMatrices = new Dictionary<ParticleInstance, Matrix>();

            _color = Color.White;
            _blendState = BlendState.NonPremultiplied;
            _depthStencilState = DepthStencilState.None;

            Initialize();
        }

        private void Initialize()
        {
            _effect.CurrentTechnique = _effect.Techniques["BasicTextureH"];
            _effect.Parameters["lightColor"].SetValue(_color.ToVector4());
            _effect.Parameters["partTexture"].SetValue(_texture);


            VertexPositionTexture[] vertices = new VertexPositionTexture[4]
            {
                new VertexPositionTexture(Vector3.Zero, new Vector2(1, 1)),
                new VertexPositionTexture(Vector3.Zero, new Vector2(0, 1)),
                new VertexPositionTexture(Vector3.Zero, new Vector2(0, 0)),
                new VertexPositionTexture(Vector3.Zero, new Vector2(1, 0))
            };

            int[] index = new int[6]
            {
                0, 1, 2, 2, 3, 0
            };

            _vertexBuffer = new VertexBuffer(GameServices.GraphicsDevice,
                typeof(VertexPositionTexture), vertices.Length, BufferUsage.WriteOnly);
            _vertexBuffer.SetData(vertices);

            _indexBuffer = new IndexBuffer(GameServices.GraphicsDevice, 
                IndexElementSize.ThirtyTwoBits, index.Length, BufferUsage.WriteOnly);
            _indexBuffer.SetData(index);
        }

        #endregion

        public void Draw(GameTime gameTime, ICamera camera)
        {
            if (InstancesTransformMatrices.Count == 0)
                return;

            // If we have more instances than room in our vertex buffer, 
            // grow it to the neccessary size.
            if ((_dynVertexBuffer == null) ||
                (InstancesTransformMatrices.Count != _dynVertexBuffer.VertexCount))
            {
                CalcVertexBuffer();
            }

            _effect.Parameters["World"].SetValue(World);
            _effect.Parameters["EyePosition"].SetValue(camera.Position);
            _effect.Parameters["vp"].SetValue(camera.ViewProjection);

            GameServices.GraphicsDevice.SetVertexBuffers(
                        new VertexBufferBinding(_vertexBuffer, 0, 0),
                        new VertexBufferBinding(_dynVertexBuffer, 0, 1));

            GameServices.GraphicsDevice.BlendState = _blendState;
            GameServices.GraphicsDevice.DepthStencilState = _depthStencilState;

            _effect.CurrentTechnique.Passes[0].Apply();

            GameServices.GraphicsDevice.Indices = _indexBuffer;
            GameServices.GraphicsDevice.DrawInstancedPrimitives(PrimitiveType.TriangleList, 0, 0,
                _vertexBuffer.VertexCount, 0, 2, InstancesTransformMatrices.Count);

            GameServices.ResetGraphicsDeviceFor3D();
        }

        #region Public Methods

        public void CalcVertexBuffer()
        {
            if (_dynVertexBuffer != null)
                _dynVertexBuffer.Dispose();

            _dynVertexBuffer = new DynamicVertexBuffer(GameServices.GraphicsDevice, 
                InstanceVertexDeclaration, InstancesTransformMatrices.Count, 
                BufferUsage.WriteOnly);

            // Transfer the latest instance transform matrices into the instanceVertexBuffer.
            _dynVertexBuffer.SetData(InstancesTransformMatrices.Values.ToArray(), 0, 
                InstancesTransformMatrices.Count, SetDataOptions.Discard);
        }

        /// <summary>
        /// Translate object
        /// </summary>
        public void TranslateOO(Vector3 distance)
        {
            _position += Vector3.Transform(distance, _rotation);
        }

        public void TranslateAA(Vector3 distance)
        {
            _position += Vector3.Transform(distance, Quaternion.Identity);
        }

        /// <summary>
        /// Rotate object
        /// </summary>
        public void Rotate(Vector3 axis, float angle)
        {
            axis = Vector3.Transform(axis, _rotation);
            _rotation = Quaternion.Normalize(_rotation *
                Quaternion.CreateFromAxisAngle(axis, angle));
        }

        #endregion
    }
}
