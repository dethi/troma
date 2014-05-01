using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using GameEngine;

namespace Troma
{
    class Player
    {
        #region Constants

        private const float HEIGHT = 7;
        private const float CROUCH_HEIGHT = 4.8f;
        private const float CROUCH_SPEED = 15;

        private const float WALK_SPEED = 40f;
        private const float COEF_RUN_SPEED = 1.7f;
        private const float RUN_SPEED = WALK_SPEED * COEF_RUN_SPEED;

        private const float JUMP_SPEED = 60;

        #endregion

        #region Fields

        private Vector3 _initPosition;
        private Vector3 _initRotation;

        private ICamera _view;
        private float _height;
        private Vector3 _position;
        private Vector3 _rotation;

        private Vector3 newPos;
        private Vector3 _prevPosition;

        private Vector3 move;
        private Vector3 rotate;
        private Vector3 rotationBuffer;

        private Vector3 velocity;
        private Vector3 acceleration;

        private BoundingSphere sphere;
        private Vector3[] ptConstSphere;

        private Ray rayDown;
        private float? dstCollisionDown;

        private bool collisionDetected;
        private bool collisionDetectedDown;
        private CollisionType collisionResult;

        private bool _isCrouched;
        private bool _touchGround;

        private ITerrain terrain;

        public Vector3 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                _view.Position = new Vector3(_position.X, viewPosY, _position.Z);
            }
        }

        public Vector3 Rotation
        {
            get { return _rotation; }
            set
            {
                _rotation = value;
                _view.Rotation = _rotation;
            }
        }

        /// <summary>
        /// (X,Z) position
        /// </summary>
        public Vector2 Pos2D
        {
            get { return new Vector2(Position.X, Position.Z); }
        }

        /// <summary>
        /// Y position of the head
        /// </summary>
        private float viewPosY
        {
            get { return _position.Y + _height; }
        }

        #endregion

        #region Initialization

        public Player(Vector3 pos, Vector3 rot, ICamera view)
        {
            _initPosition = pos;
            _initRotation = rot;
            _view = view;
        }

        public void Initialize(ITerrain terrain)
        {
            _height = HEIGHT;
            _position = _initPosition;
            _rotation = _initRotation;

            newPos = _position;
            rotationBuffer = Vector3.Zero;
            ptConstSphere = new Vector3[2];

            _isCrouched = false;
            _touchGround = false;
            collisionDetected = false;
            collisionDetectedDown = false;

            this.terrain = terrain;

            MoveTo(_position, _rotation);

#if DEBUG
            XConsole.AddDebug(Debug);
#endif
        }

        #endregion

        public void Update(GameTime gameTime)
        {
            GroundCollision();
        }


        public void HandleInput(GameTime gameTime, InputState input)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Move(dt, input);
            Crouch(dt, input);
            Jump(dt, input);

            // Apply movement
            velocity += acceleration * dt * dt;
            move += velocity * dt;
            newPos = PreviewMove(move, rotate.Y);

            ApplyCollision();
            MoveTo(newPos, rotate);
        }

        public void Draw(GameTime gameTime, ICamera camera)
        {
#if DEBUG
            BoundingSphereRenderer.Render(sphere, camera, Color.Fuchsia);
#endif
        }

        public string Debug(GameTime gameTime)
        {
            return String.Format(new System.Globalization.CultureInfo("en-GB"),
                "Pos: ({0:F1}; {1:F1}; {2:F1})\nCollision: {3}",
                _position.X, _position.Y, _position.Z, collisionDetected);
        }

        private void Move(float dt, InputState input)
        {
            // Walk/Run
            if (input.PlayerMove(out move))
            {
                move *= dt * WALK_SPEED;

                if (!_isCrouched && input.PlayerRun())
                {
                    if (move.Z > 0)
                        move.Z *= COEF_RUN_SPEED;
                }
            }

            // Rotate
            if (input.PlayerRotate(ref rotationBuffer, dt))
            {
                if (rotationBuffer.Y < MathHelper.ToRadians(-75.0f))
                    rotationBuffer.Y -= (rotationBuffer.Y - MathHelper.ToRadians(-75.0f));
                if (rotationBuffer.Y > MathHelper.ToRadians(75.0f))
                    rotationBuffer.Y -= (rotationBuffer.Y - MathHelper.ToRadians(75.0f));

                rotate = new Vector3(
                    -MathHelper.Clamp(rotationBuffer.Y, MathHelper.ToRadians(-75.0f), MathHelper.ToRadians(75.0f)),
                    MathHelper.WrapAngle(rotationBuffer.X),
                    0);
            }
        }

        private void Crouch(float dt, InputState input)
        {
            if ((!_isCrouched && _height < HEIGHT) ||
                (_isCrouched && _height > CROUCH_HEIGHT))
            {
                float moveAxisY = CROUCH_SPEED * dt;

                if (_isCrouched)
                    _height = Math.Max(CROUCH_HEIGHT, _height - moveAxisY);
                else
                    _height = Math.Min(HEIGHT, _height + moveAxisY);
            }
            else if (input.PlayerCrouch())
                _isCrouched = !_isCrouched;
        }

        private void Jump(float dt, InputState input)
        {
            if (!_touchGround && !collisionDetectedDown)
                acceleration.Y = Physics.Gravity;
            else
            {
                acceleration.Y = 0;
                velocity.Y = 0;
            }

            if ((_touchGround || collisionDetectedDown) && input.PlayerJump())
            {
                velocity.Y = JUMP_SPEED;
                _touchGround = false;
                _isCrouched = false;
            }
        }

        private void GroundCollision()
        {
            if (terrain.IsOnTerrain(_position))
            {
                float y = terrain.GetY(_position);

                if (_touchGround)
                    _position.Y = y;
                else
                    _touchGround = (y - 2 <= _position.Y) && (_position.Y <= y + 0.1f);
            }
            else
                _touchGround = false;
        }

        private void ApplyCollision()
        {
            if (move != Vector3.Zero)
            {
                #region X,Z collisions

                ptConstSphere[0] = newPos;
                ptConstSphere[1].X = newPos.X;
                ptConstSphere[1].Y = newPos.Y + _height + 0.2f;
                ptConstSphere[1].Z = newPos.Z;

                sphere = BoundingSphere.CreateFromPoints(ptConstSphere);
                collisionResult = CollisionManager.IsCollision(sphere);
                collisionDetected = collisionResult.IsCollide;

                // Response to the collision : go to the exact collision point
                if (collisionDetected)
                {
                    if (move.X != 0)
                    {
                        float dirX = (newPos.X - _position.X > 0) ? 1 : -1;

                        Ray dirRayX = new Ray(sphere.Center, Vector3.Right * dirX);
                        float? dstCollisionMoveX = dirRayX.Intersects(collisionResult.CollisionWith);

                        if (dstCollisionMoveX.HasValue)
                            newPos.X -= dirX * (sphere.Radius - dstCollisionMoveX.Value);
                    }

                    if (move.Z != 0)
                    {
                        float dirZ = (newPos.Z - _position.Z > 0) ? 1 : -1;

                        Ray dirRayZ = new Ray(sphere.Center, Vector3.Backward * dirZ);
                        float? dstCollisionMoveZ = dirRayZ.Intersects(collisionResult.CollisionWith);
                        
                        if (dstCollisionMoveZ.HasValue)
                            newPos.Z -= dirZ * (sphere.Radius - dstCollisionMoveZ.Value);
                    }
                }

                #endregion

                #region Y collision

                ptConstSphere[1].X = newPos.X;
                ptConstSphere[1].Z = newPos.Z;

                sphere = BoundingSphere.CreateFromPoints(ptConstSphere);
                collisionResult = CollisionManager.IsCollision(sphere);
                collisionDetected = collisionResult.IsCollide;
                
                // Response to the collision : go to the exact collision point
                if (collisionDetected)
                {
                    rayDown = new Ray(sphere.Center, Vector3.Down);
                    dstCollisionDown = rayDown.Intersects(collisionResult.CollisionWith);

                    if (dstCollisionDown.HasValue)
                    {
                        newPos.Y += sphere.Radius - dstCollisionDown.Value;
                        collisionDetectedDown = true;
                    }
                }
                else
                    collisionDetectedDown = false;

                #endregion
            }
        }

        /// <summary>
        /// Change position and rotation
        /// </summary>
        public void MoveTo(Vector3 pos, Vector3 rot)
        {
            if (pos != _position)
                _prevPosition = _position;

            Position = pos;
            Rotation = rot;
        }

        /// <summary>
        /// Calculates the new position
        /// </summary>
        /// <param name="movement">Direction and scale of the movement</param>
        /// <param name="angleAxisY">Y angle</param>
        private Vector3 PreviewMove(Vector3 movement, float angleAxisY)
        {
            movement = Vector3.Transform(
                movement,
                Matrix.CreateRotationY(angleAxisY));

            return _position + movement;
        }

        public void Reset()
        {
            _height = HEIGHT;
            _position = _initPosition;
            _rotation = _initRotation;

            newPos = _position;
            _isCrouched = false;
            _touchGround = false;
            collisionDetected = false;
            collisionDetectedDown = false;

            MoveTo(_position, _rotation);
        }
    }
}
