using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GameEngine;
using GameEngine.Camera;
using GameEngine.Input;

namespace Troma
{
    class Player
    {
        #region Constants

        private const float HEIGHT = 7;
        private const float CROUCH_HEIGHT = 4.8f;
        private const float CROUCH_SPEED = 2f;

        private const float WALK_SPEED = 40f;
        private const float COEF_RUN_SPEED = 1.7f;

        private const float JUMP_SPEED = 32f;

        #endregion

        #region Fields

        private Vector3 _initPosition;
        private Vector3 _initRotation;

        private ICamera _view;
        private Vector3 _position;
        private Vector3 _rotation;
        private float _height;

        private Vector3 move;
        private Vector3 rotate;
        private Vector3 rotationBuffer;
        private Vector3 velocity;
        private bool isCrouched;
        private bool hasJumping;

        public Vector3 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                _view.Position = new Vector3(_position.X, _viewPosY, _position.Z);
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

        private float _viewPosY
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

        public void Initialize()
        {
            _height = HEIGHT;
            _position = _initPosition;
            _rotation = _initRotation;

            move = Vector3.Zero;
            rotate = Vector3.Zero;
            rotationBuffer = Vector3.Zero;
            velocity = Vector3.Zero;

            isCrouched = false;
            hasJumping = false;
        }

        #endregion

        public void Update(GameTime gameTime)
        {
        }


        public void HandleInput(GameTime gameTime, InputState input)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Crouch(dt, input);
            Jump(dt, input);
            Move(dt, input);
        }

        public void Draw()
        {
        }

        private void Move(float dt, InputState input)
        {
            // Walk/Run
            if (input.PlayerMove(out move))
            {
                move *= dt * WALK_SPEED;

                if (!isCrouched && input.PlayerRun())
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

            // Apply movement
            MoveTo(PreviewMove(move, rotate.Y), rotate);
        }

        private void Crouch(float dt, InputState input)
        {
            if ((!isCrouched && _height < HEIGHT) || 
                (isCrouched && _height > CROUCH_HEIGHT))
            {
                float moveAxisY = CROUCH_SPEED * dt;

                if (isCrouched)
                    _height = Math.Max(CROUCH_HEIGHT, _height - moveAxisY);
                else
                    _height = Math.Min(HEIGHT, _height + moveAxisY);
            }
            else
                isCrouched = input.PlayerCrouch();
        }

        private void Jump(float dt, InputState input)
        {
            if (!hasJumping && !isCrouched)
            {
                hasJumping = input.PlayerJump();

                if (hasJumping)
                    velocity.Y += JUMP_SPEED;
            }

            // Apply gravity
            velocity.Y -= Physics.Gravity * dt;
        }

        /// <summary>
        /// Change position and rotation
        /// </summary>
        public void MoveTo(Vector3 pos, Vector3 rot)
        {
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





        // Vieux code
        //
        //
        //
        //
        /*
        public void Update(GameTime gameTime, MapManager map)
        {
            MapCollision(map);
        }

        public void HandleInput(GameTime gameTime, InputState input, MapManager map)
        {
            Jump(input, map);
        }

        private void MapCollision(MapManager map)
        {
            float? mapHeight = map.GetHeight(Position.X, Position.Z);

            if (!jumped && mapHeight.HasValue && Position.Y != mapHeight)
                Position = new Vector3(Position.X, mapHeight.Value, Position.Z);
        }
         */
    }
}
