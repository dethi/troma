﻿using System;
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
        private Vector3 _position;
        private Vector3 _rotation;
        private float _height;

        private Vector3 move;
        private Vector3 rotate;
        private Vector3 rotationBuffer;

        private Vector3 velocity;
        private Vector3 acceleration;

        private bool isCrouched;
        private bool touchGround;

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
        /// Y position of the head
        /// </summary>
        private float viewPosY
        {
            get { return _position.Y + _height; }
        }

        /// <summary>
        /// (X,Z) position
        /// </summary>
        private Vector2 pos2D
        {
            get { return new Vector2(Position.X, Position.Z); }
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

            move = Vector3.Zero;
            rotate = Vector3.Zero;
            rotationBuffer = Vector3.Zero;
            velocity = Vector3.Zero;
            acceleration = Vector3.Zero;

            isCrouched = false;
            touchGround = false;

            this.terrain = terrain;

            MoveTo(_position, _rotation);
        }

        #endregion

        public void Update(GameTime gameTime)
        {
            GroundCollision();
        }


        public void HandleInput(GameTime gameTime, InputState input)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Crouch(dt, input);
            Jump(dt, input);
            Move(dt, input);

            // Debug
            /*
            if (input.IsPressed(Keys.Z))
                _position.Y += 5;
            else if (input.IsPressed(Keys.X))
                _position.Y -= 5;
             */

            // Apply movement
            MoveTo(PreviewMove(move, rotate.Y), rotate);
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
            else if (input.PlayerCrouch())
                isCrouched = !isCrouched;
        }

        private void Jump(float dt, InputState input)
        {
            if (!touchGround)
                acceleration.Y = Physics.Gravity;
            else
            {
                acceleration.Y = 0;
                velocity.Y = 0;
            }

            if (touchGround && input.PlayerJump())
            {
                velocity.Y = JUMP_SPEED;
                touchGround = false;
                isCrouched = false;
            }

            velocity += acceleration * dt * dt;
            _position += velocity * dt;
        }

        private void GroundCollision()
        {
            if (terrain.IsOnTerrain(_position))
            {
                float y = terrain.GetY(_position);

                if (touchGround)
                    _position.Y = y;
                else
                    touchGround = (y - 2 <= _position.Y) && (_position.Y <= y + 0.1f);
            }
            else
                touchGround = false;
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
    }
}
