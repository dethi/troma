using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using GameEngine;
using ClientServerExtension;

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

        private const float JUMP_SPEED = 40;

        private const float DT_SOUND_MOVE = 0.45f;
        private const float DT_SOUND_RUN = 0.3f;

        #endregion

        #region Fields

        public bool Alive;

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
        private SphereCollision collisionResult;

        #endregion

        #region Fields for Weapon

        public Entity _weaponActive;
        public WeaponInfo _weaponInfoActive;

        public Entity _weaponMain;
        public Entity _weaponSecond;

        public bool HasShoot;
        private bool _isReload;
        private bool _inSightPosition;

        private Vector3 bulletDir;
        private Ray bulletRay;
        private RayCollision bulletResult;

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
            Alive = false;

            _height = HEIGHT;
            _position = _initPosition;
            _rotation = _initRotation;

            newPos = _position;
            rotationBuffer = Vector3.Zero;

            _isCrouched = false;
            _touchGround = false;
            _collisionDetected = false;
            _collisionDetectedDown = false;

            _terrain = terrain;
            _weaponActive = weapon;

            if (_weaponActive != null)
                _weaponActive.Initialize();

            MoveTo(_position, _rotation);

#if DEBUG
            XConsole.AddDebug(Debug);
#endif
        }

        public void Initialize(ITerrain terrain, Entity weaponMain, Entity weaponSecond)
        {
            Alive = false;

            _height = HEIGHT;
            _position = _initPosition;
            _rotation = _initRotation;

            newPos = _position;
            rotationBuffer = Vector3.Zero;

            _isCrouched = false;
            _touchGround = false;
            _collisionDetected = false;
            _collisionDetectedDown = false;

            _terrain = terrain;

            MoveTo(_position, _rotation);

            _weaponMain = weaponMain;
            _weaponSecond = weaponSecond;

            if (_weaponMain != null)
            {
                _weaponMain.Initialize();
                _weaponMain.GetComponent<Weapon>().Arms.Initialize();
            }
            if (_weaponSecond != null)
            {
                _weaponSecond.Initialize();
                _weaponSecond.GetComponent<Weapon>().Arms.Initialize();
            }

            _weaponActive = _weaponMain;
            _weaponInfoActive = _weaponActive.GetComponent<Weapon>().Info;

#if DEBUG
            XConsole.AddDebug(Debug);
#endif
        }

        #endregion

        #region Update and Draw

        public void Update(GameTime gameTime)
        {
            GroundCollision();
            _weaponActive.GetComponent<Weapon>().Muzzle.Update();
        }

        public void HandleInput(GameTime gameTime, InputState input)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_weaponActive != null)
            {
                _weaponActive.GetComponent<Weapon>().UpdateRecoil(gameTime);
                WeaponAction(gameTime, input);
            }

            Move(dt, input);
            Crouch(dt, input);
            Jump(input);

            // Apply movement
            velocity += acceleration * dt * dt;
            move += velocity * dt;
            newPos = PreviewMove(move, rotate.Y);
            ApplyCollision();

            rotate.X -= _weaponActive.GetComponent<Weapon>().CameraYRecoil;
            rotate.Y += _weaponActive.GetComponent<Weapon>().CameraXRecoil;

            MoveTo(newPos, rotate);

            #region Weapon Update

            float time = (float)((gameTime.TotalGameTime.TotalMilliseconds / 2000) % 2 * Math.PI);
            int amplitude;
            float cosDisp, sinDisp;
            float xT, yT, zT;

            if (move != Vector3.Zero && !_weaponActive.GetComponent<Weapon>().SightPosition)
            {
                amplitude = 5;
                cosDisp = amplitude * (float)Math.Cos(time * amplitude) / 60;//cossinus displacement
                sinDisp = amplitude * (float)Math.Sin(time * amplitude) / 60;//sinus displacement
                yT = Math.Abs(sinDisp);
            }
            else
            {
                amplitude = 2;
                cosDisp = 0.0f;
                sinDisp = amplitude * (float)Math.Sin(time * amplitude) / 100;//sinus displacement
                yT = sinDisp * 0.2f;
            }

            xT = cosDisp / 2;
            zT = cosDisp / 3;

            _weaponActive.GetComponent<Transform>().Position = _view.Position +
                Vector3.Transform(new Vector3(xT, yT, zT), Matrix.CreateRotationY(rotate.Y)) +
                Vector3.Transform(_weaponActive.GetComponent<Weapon>().RecoilTranslation, Matrix.CreateRotationY(rotate.Y));

            _weaponActive.GetComponent<Transform>().Rotation = new Vector3(
                1.57f - _view.Rotation.X,
                3.14f + _view.Rotation.Y,
                _view.Rotation.Z);

            _weaponActive.Update(gameTime);
            _weaponActive.GetComponent<Weapon>().Arms.Update(gameTime);

            #endregion

            PlaySoundEffect(dt);
        }

        public void Draw(GameTime gameTime, ICamera camera)
        {
#if DEBUG
            BoundingSphereRenderer.Render(sphere, camera, Color.Fuchsia);
#endif

            if (_weaponActive != null)
                _weaponActive.Draw(gameTime, camera);
        }

        public void DrawHUD(GameTime gameTime)
        {
            if (_weaponActive != null)
                _weaponActive.DrawHUD(gameTime);
        }

        public string Debug(GameTime gameTime)
        {
            return String.Format(new System.Globalization.CultureInfo("en-GB"),
                "Pos: ({0:F1}; {1:F1}; {2:F1})\nCollision: {3}",
                _position.X, _position.Y, _position.Z, _collisionDetected);
        }

        #endregion

        #region Actions

        private void Move(float dt, InputState input)
        {
            _isMove = input.PlayerMove(out move);
            _isRun = (_isMove && !_isCrouched && input.PlayerRun());

            // Walk/Run
            if (_isMove)
            {
                move *= dt * WALK_SPEED;

                if (_isRun && !_weaponActive.GetComponent<Weapon>().SightPosition)
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

                sphere.Radius = (_height + 0.2f) / 2;
                sphere.Center = new Vector3(newPos.X, newPos.Y + sphere.Radius, newPos.Z);

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

                sphere.Center = new Vector3(newPos.X, newPos.Y + sphere.Radius, newPos.Z);

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
            if (input.PlayerChangeWeapon())
                ChangeWeapon();

            if (input.PlayerReload())
                _isReload = _weaponActive.GetComponent<Weapon>().Reload();

            if (input.PlayerSight())
                _inSightPosition = _weaponActive.GetComponent<Weapon>().ChangeSight();

            HasShoot = input.PlayerShoot(_weaponActive.GetComponent<Weapon>().Info.Automatic) &&
                _weaponActive.GetComponent<Weapon>().Shoot();

            if (HasShoot)
            {
                bulletDir = _view.LookAt - _view.Position;
                bulletDir.Normalize();

                bulletRay = new Ray(_view.Position, bulletDir);
                bulletResult = CollisionManager.IsCollision(bulletRay);

                if (bulletResult.IsCollide)
                    TargetManager.IsTargetAchieved(bulletResult.CollisionWith);
            }
        }

        /// <summary>
        /// Change active weapon
        /// </summary>
        private void ChangeWeapon()
        {
            if (_weaponSecond == null)
                return;

            _weaponActive.GetComponent<Weapon>().ChangeDown();
            TimerManager.Add(300, EventChangeWeapon);
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

        #endregion

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

        public int MunitionUsed()
        {
            return ((_weaponMain == null) ? 0 : _weaponMain.GetComponent<Weapon>().MunitionUsed) +
                ((_weaponSecond == null) ? 0 : _weaponSecond.GetComponent<Weapon>().MunitionUsed);
        }

        public void EventChangeWeapon(object o, EventArgs e)
        {
            if (_weaponActive == _weaponMain)
            {
                _weaponActive = _weaponSecond;
                _weaponInfoActive = _weaponSecond.GetComponent<Weapon>().Info;
            }
            else
            {
                _weaponActive = _weaponMain;
                _weaponInfoActive = _weaponMain.GetComponent<Weapon>().Info;
            }

            _weaponActive.GetComponent<Weapon>().ChangeUp();
        }

        public INPUT GetInput()
        {
            return new INPUT()
            {
                IsMove = _isMove,
                IsRun = _isRun,
                IsCrouch = _isCrouched,

                IsReload = _isReload,
                InSightPosition = _inSightPosition,
                Weapon = _weaponInfoActive.Type,
            };
        }

        public STATE GetState()
        {
            return new STATE()
            {
                Alive = this.Alive,
                Position = this.Position,
                Rotation = this.Rotation
            };
        }

        public void Spawn(STATE state)
        {
            Alive = true;
            Position = state.Position;
            Rotation = state.Rotation;
        }

        public void Killed()
        {
            Alive = false;
            // do it
        }

        #endregion
    }
}
