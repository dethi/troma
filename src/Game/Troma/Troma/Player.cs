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

        private const float DT_SOUND_MOVE = 0.45f;
        private const float DT_SOUND_RUN = 0.3f;

        #endregion

        #region Fields

        private readonly Vector3 _initPosition;
        private readonly Vector3 _initRotation;
        private ITerrain _terrain;

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

        private bool _isCrouched;
        private bool _touchGround;
        private bool _isRun;
        private bool _isMove;

        #region Fields for collision

        private BoundingSphere sphere;
        private Vector3[] ptConstSphere;

        private Ray rayDown;
        private float? dstCollisionDown;
        private Ray dirRayX;
        private float dirX;
        private float? dstCollisionMoveX;
        private Ray dirRayZ;
        private float dirZ;
        private float? dstCollisionMoveZ;

        private bool _collisionDetected;
        private bool _collisionDetectedDown;
        private CollisionType collisionResult;

        #endregion

        #region Fields for Weapon

        private Entity _weapon;
        private Vector3 nearPoint;
        private Vector3 farPoint;
        private Vector3 bulletDir;
        private Ray bulletRay;
        private CollisionType bulletResult;

        #endregion

        #region Fields for move sound

        private float dt_elapsedTime = 0.5f;
        private float pan = 0.7f;

        #endregion

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

        public void Initialize(ITerrain terrain, Entity weapon)
        {
            _height = HEIGHT;
            _position = _initPosition;
            _rotation = _initRotation;

            newPos = _position;
            rotationBuffer = Vector3.Zero;
            ptConstSphere = new Vector3[2];

            _isCrouched = false;
            _touchGround = false;
            _collisionDetected = false;
            _collisionDetectedDown = false;

            _terrain = terrain;
            _weapon = weapon;

            if (_weapon != null)
                _weapon.Initialize();

            MoveTo(_position, _rotation);

#if DEBUG
            XConsole.AddDebug(Debug);
#endif
        }

        #endregion

        #region Update and Draw

        public void Update(GameTime gameTime)
        {
            GroundCollision();
        }

        public void HandleInput(GameTime gameTime, InputState input)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_weapon != null)
                WeaponAction(gameTime, input);

            Move(dt, input);
            Crouch(dt, input);
            Jump(input);

            // Apply movement
            velocity += acceleration * dt * dt;
            move += velocity * dt;
            newPos = PreviewMove(move, rotate.Y);

            ApplyCollision();
            MoveTo(newPos, rotate);

            PlaySoundEffect(dt);
        }

        public void Draw(GameTime gameTime, ICamera camera)
        {
#if DEBUG
            BoundingSphereRenderer.Render(sphere, camera, Color.Fuchsia);
#endif

            if (_weapon != null)
            {
                _weapon.GetComponent<Transform>().Position = _view.Position;
                _weapon.GetComponent<Transform>().Rotation = _view.Rotation;
                _weapon.Draw(gameTime, camera);
            }
        }

        public void DrawHUD(GameTime gameTime)
        {
            if (_weapon != null)
                _weapon.DrawHUD(gameTime);
        }

        public string Debug(GameTime gameTime)
        {
            return String.Format(new System.Globalization.CultureInfo("en-GB"),
                "Pos: ({0:F1}; {1:F1}; {2:F1})\nCollision: {3}",
                _position.X, _position.Y, _position.Z, _collisionDetected);
        }

        #endregion

        private void Move(float dt, InputState input)
        {
            _isMove = input.PlayerMove(out move);
            _isRun = (_isMove && !_isCrouched && input.PlayerRun());

            // Walk/Run
            if (_isMove)
            {
                move *= dt * WALK_SPEED;

                if (_isRun)
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

        private void Jump(InputState input)
        {
            if (!_touchGround && !_collisionDetectedDown)
                acceleration.Y = Physics.Gravity;
            else
            {
                acceleration.Y = 0;
                velocity.Y = 0;
            }

            if ((_touchGround || _collisionDetectedDown) && input.PlayerJump())
            {
                velocity.Y = JUMP_SPEED;
                _touchGround = false;
                _isCrouched = false;
            }
        }

        private void GroundCollision()
        {
            if (_terrain.IsOnTerrain(_position))
            {
                float y = _terrain.GetY(_position);

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
                _collisionDetected = collisionResult.IsCollide;

                // Response to the collision : go to the exact collision point
                if (_collisionDetected)
                {
                    if (move.X != 0)
                    {
                        dirX = (newPos.X - _position.X > 0) ? 1 : -1;

                        dirRayX = new Ray(sphere.Center, Vector3.Right * dirX);

                        foreach (BoundingBox box in collisionResult.CollisionWith)
                        {
                            dstCollisionMoveX = dirRayX.Intersects(box);

                            if (dstCollisionMoveX.HasValue)
                                newPos.X -= dirX * (sphere.Radius - dstCollisionMoveX.Value);
                        }
                    }

                    if (move.Z != 0)
                    {
                        dirZ = (newPos.Z - _position.Z > 0) ? 1 : -1;

                        dirRayZ = new Ray(sphere.Center, Vector3.Backward * dirZ);

                        foreach (BoundingBox box in collisionResult.CollisionWith)
                        {
                            dstCollisionMoveZ = dirRayZ.Intersects(box);

                            if (dstCollisionMoveZ.HasValue)
                                newPos.Z -= dirZ * (sphere.Radius - dstCollisionMoveZ.Value);
                        }
                    }
                }

                #endregion

                #region Y collision

                ptConstSphere[1].X = newPos.X;
                ptConstSphere[1].Z = newPos.Z;

                sphere = BoundingSphere.CreateFromPoints(ptConstSphere);
                collisionResult = CollisionManager.IsCollision(sphere);
                _collisionDetected = collisionResult.IsCollide;

                // Response to the collision : go to the exact collision point
                if (_collisionDetected)
                {
                    rayDown = new Ray(sphere.Center, Vector3.Down);

                    foreach (BoundingBox box in collisionResult.CollisionWith)
                    {
                        dstCollisionDown = rayDown.Intersects(box);

                        if (dstCollisionDown.HasValue)
                        {
                            newPos.Y += sphere.Radius - dstCollisionDown.Value;
                            _collisionDetectedDown = true;
                        }
                    }
                }
                else
                    _collisionDetectedDown = false;

                #endregion
            }
        }

        private void WeaponAction(GameTime gameTime, InputState input)
        {
            if (input.PlayerReload())
                _weapon.GetComponent<Weapon>().Reload();

            if (input.PlayerSight())
                _weapon.GetComponent<Weapon>().ChangeSight();

            if (input.PlayerShoot(_weapon.GetComponent<Weapon>().Info.Automatic))
            {
                if (_weapon.GetComponent<Weapon>().Shoot())
                {
                    nearPoint = GameServices.GraphicsDevice.Viewport.Unproject(
                        new Vector3(InputState.MouseOrigin.X, InputState.MouseOrigin.Y, 0), _view.Projection,
                        _view.View, Matrix.Identity);
                    farPoint = GameServices.GraphicsDevice.Viewport.Unproject(
                        new Vector3(InputState.MouseOrigin.X, InputState.MouseOrigin.Y, 1), _view.Projection,
                        _view.View, Matrix.Identity);

                    bulletDir = farPoint - nearPoint;
                    bulletDir.Normalize();

                    bulletRay = new Ray(_view.Position, bulletDir);
                    bulletResult = CollisionManager.IsCollision(bulletRay);

                    if (bulletResult.IsCollide)
                        TargetManager.IsTargetAchieved(bulletResult.CollisionWith[0]);
                }
            }
        }

        private void PlaySoundEffect(float dt)
        {
            dt_elapsedTime += dt;
            pan *= -1;

            if ((_touchGround || _collisionDetectedDown) && 
                ((_isRun && dt_elapsedTime >= DT_SOUND_RUN) ||
                (_isMove && dt_elapsedTime >= DT_SOUND_MOVE)))
            {
                SFXManager.Play("Move", 0.5f, pan);
                dt_elapsedTime = 0;
            }
        }

        /// <summary>
        /// Change position and rotation
        /// </summary>
        private void MoveTo(Vector3 pos, Vector3 rot)
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

        #region Public Methods

        public void Reset()
        {
            _height = HEIGHT;
            _position = _initPosition;
            _rotation = _initRotation;

            newPos = _position;
            _isCrouched = false;
            _touchGround = false;
            _collisionDetected = false;
            _collisionDetectedDown = false;

            MoveTo(_position, _rotation);
        }

        #endregion
    }
}
