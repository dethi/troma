using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Arrow
{
    public partial class Player
    {
        #region Attributes

        private Game game;

        private float height;

        private Vector3 rotationBuffer;

        private Vector3 velocity;
        private bool jumped;

        #endregion

        #region Properties

        public Camera cam { get; private set; }

        public Vector3 Position
        {
            get
            {
                return new Vector3(cam.Position.X, cam.Position.Y - height, cam.Position.Z);
            }
            set
            {
                cam.Position = new Vector3(value.X, value.Y + height, value.Z);
            }
        }

        public Vector3 Rotation
        {
            get { return cam.Rotation; }
            set { cam.Position = value; }
        }

        #endregion

        #region Constructor

        public Player(Game game)
            : this(game, Vector3.Zero) { }

        public Player(Game game, Vector3 pos)
            : this(game, pos, Vector3.Zero) { }

        public Player(Game game, Vector3 pos, Vector3 rot)
        {
            this.game = game;

            this.height = HEIGHT;

            this.cam = Camera.Instance;
            Position = pos;
            rotationBuffer = Vector3.Zero;

            velocity = new Vector3(0, 1, 0);
            jumped = false;
        }

        #endregion

        public void Update(GameTime gameTime, MapManager map)
        {
            Shoot(gameTime);
            MapCollision(map);
        }

        public void HandleInput(GameTime gameTime, InputState input, MapManager map)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Crouch(input);
            CameraOrientation(input, dt);
            Walk(input, dt);
            Jump(input, map);
        }

        public void Draw()
        {
        }

        /// <summary>
        /// Move player
        /// </summary>
        /// <param name="dtSeconds">Total seconds elapsed since last update</param>
        private void Walk(InputState input, float dtSeconds)
        {
            Vector3 moveVector = Vector3.Zero;

            if (input.PlayerMove(out moveVector))
            {
                moveVector *= dtSeconds * WALK_SPEED;

                if (input.PlayerRun())
                {
                    if (moveVector.Z > 0)
                        moveVector.Z *= COEF_RUN_SPEED;
                }
            }

            // Effectue le mouvement
            cam.Move(moveVector);

        }

        /// <summary>
        /// Change camera orientation
        /// </summary>
        /// <param name="dtSeconds">Total seconds elapsed since last update</param>
        private void CameraOrientation(InputState input, float dtSeconds)
        {
            if (input.PlayerRotate(ref rotationBuffer, dtSeconds))
            {
                if (rotationBuffer.Y < MathHelper.ToRadians(-75.0f))
                    rotationBuffer.Y -= (rotationBuffer.Y - MathHelper.ToRadians(-75.0f));
                if (rotationBuffer.Y > MathHelper.ToRadians(75.0f))
                    rotationBuffer.Y -= (rotationBuffer.Y - MathHelper.ToRadians(75.0f));

                cam.Rotation = new Vector3(-MathHelper.Clamp(rotationBuffer.Y,
                    MathHelper.ToRadians(-75.0f), MathHelper.ToRadians(75.0f)),
                    MathHelper.WrapAngle(rotationBuffer.X), 0);
            }
        }

        /// <summary>
        /// Prevents map collision
        /// </summary>
        private void MapCollision(MapManager map)
        {
            float? mapHeight = map.GetHeight(Position.X, Position.Z);

            if (!jumped && mapHeight.HasValue && Position.Y != mapHeight)
                Position = new Vector3(Position.X, mapHeight.Value, Position.Z);
        }

        /// <summary>
        /// Shoot
        /// </summary>              
        private void Shoot(GameTime gameTime)
        {
        }

        /// <summary>
        /// Crouch
        /// </summary>
        private void Crouch(InputState input)
        {
            float currentHeight = height;

            if (input.PlayerCrouch())
                currentHeight = CROUCH_HEIGHT;
            else
                currentHeight = HEIGHT;

            if (height != currentHeight)
            {
                Vector3 pos = Position;
                height = currentHeight;
                Position = pos;
            }
        }

        /// <summary>
        /// Jump
        /// </summary>
        private void Jump(InputState input, MapManager map)
        {
            if (!jumped)
            {
                velocity.Y = 1;
                jumped = input.PlayerJump();
            }
            else
            {
                Position += velocity;
                velocity.Y -= 0.05f;

                if (Position.Y <= map.GetHeight(Position.X, Position.Z))
                    jumped = false;
            }
        }
    }
}