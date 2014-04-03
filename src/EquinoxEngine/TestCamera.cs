using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EquinoxEngine
{
    public class TestCamera
    {
        //Move speed for camera
        private float _moveScale = 4f;

        private Vector3 _position;
        private Vector3 _target;
        private Matrix _view;
        private Matrix _projection;
        private GraphicsDevice _device;

        public Matrix View { get { return _view; } }
        public Matrix Projection { get { return _projection; } }
        public Vector3 Position { get { return _position; } }


        public TestCamera(Vector3 position, Vector3 target, GraphicsDevice device, float farDistance)
        {
            _device = device;
            _position = position;
            _target = target;

            UpdateCamera();

            _projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, _device.Viewport.AspectRatio,
                                    1.0f, farDistance);
        }

        public void Move(Vector3 translation)
        {
            _position.X += translation.X * _moveScale;
            _position.Y += translation.Y * _moveScale;
            _position.Z += translation.Z * _moveScale;

            //Move the target with the camera as well
            _target.X += translation.X * _moveScale;
            _target.Y += translation.Y * _moveScale;
            _target.Z += translation.Z * _moveScale;

            UpdateCamera();

        }

        public void Pitch(Vector3 translation)
        {
            //Move the target with the camera as well
            _target.X += translation.X * _moveScale;
            _target.Y += translation.Y * _moveScale;
            _target.Z += translation.Z * _moveScale;

            UpdateCamera();
        }

        private void UpdateCamera()
        {
            _view = Matrix.CreateLookAt(_position, _target, Vector3.Up);
        }
    }
}