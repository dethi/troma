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
        private InputState input;

        private float height;

        private MouseState currentMouseState;
        private Vector2 originMouse;
        private Vector3 rotationBuffer;

        private Vector3 velocity;
        private bool jumped;

        private Weapon weapon;

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

        public Player(Game game, InputState input)
            : this(game, input, Vector3.Zero) { }

        public Player(Game game, InputState input, Vector3 pos)
            : this(game, input, pos, Vector3.Zero) { }

        public Player(Game game, InputState input, Vector3 pos, Vector3 rot)
        {
            this.game = game;
            this.input = input;

            this.height = HEIGHT;

            this.cam = Camera.Instance;
            Position = pos;

            int centerX = game.GraphicsDevice.Viewport.Width / 2;
            int centerY = game.GraphicsDevice.Viewport.Height / 2;
            originMouse = new Vector2(centerX, centerY);
            Mouse.SetPosition(centerX, centerY);

            velocity = new Vector3(0, 1, 0);
            jumped = false;

            weapon = new GarandM1(game);
        }

        #endregion

        public void Update(GameTime gameTime, HeightMap map)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Shoot(gameTime);
            Crouch();
            CameraOrientation(dt);
            Walk(dt);
            MapCollision(map);
            Jump(map);
        }

        public void Draw()
        {
            weapon.Draw();
        }

        /// <summary>
        /// Move player
        /// </summary>
        /// <param name="dtSeconds">Total seconds elapsed since last update</param>
        private void Walk(float dtSeconds)
        {
            Vector3 moveVector = Vector3.Zero;

            if (input.PlayerMove(out moveVector))
            {
                moveVector *= dtSeconds * WALK_SPEED;

                if (input.IsDown(KeyActions.Run) || input.IsDown(Buttons.LeftStick))
                {
                    if (moveVector.Z > 0)
                        moveVector.Z *= COEF_RUN_SPEED;
                    //SFXManager.Play("Run");
                }
                /*else
                    SFXManager.Play("Walk");*/
            }

            // Effectue le mouvement
            cam.Move(moveVector);

        }

        /// <summary>
        /// Change camera orientation
        /// </summary>
        /// <param name="dtSeconds">Total seconds elapsed since last update</param>
        private void CameraOrientation(float dtSeconds)
        {
            if (GamePad.GetState(PlayerIndex.One).IsConnected)
            {
                #region ThumbStickRight

                GamePadState gps = GamePad.GetState(PlayerIndex.One);

                if (gps.ThumbSticks.Right.X != 0 || gps.ThumbSticks.Right.Y != 0)
                {
                    rotationBuffer.X -= 1.5f * gps.ThumbSticks.Right.X * dtSeconds;
                    rotationBuffer.Y += 1.5f * gps.ThumbSticks.Right.Y * dtSeconds;

                    if (rotationBuffer.Y < MathHelper.ToRadians(-75.0f))
                    {
                        rotationBuffer.Y = rotationBuffer.Y - (rotationBuffer.Y -
                            MathHelper.ToRadians(-75.0f));
                    }
                    if (rotationBuffer.Y > MathHelper.ToRadians(75.0f))
                    {
                        rotationBuffer.Y = rotationBuffer.Y - (rotationBuffer.Y -
                            MathHelper.ToRadians(75.0f));
                    }

                    cam.Rotation = new Vector3(-MathHelper.Clamp(rotationBuffer.Y,
                        MathHelper.ToRadians(-75.0f), MathHelper.ToRadians(75.0f)),
                        MathHelper.WrapAngle(rotationBuffer.X), 0);
                }

                #endregion
            }
            else
            {
                #region Mouse

                currentMouseState = Mouse.GetState();

                if (currentMouseState.X != originMouse.X || currentMouseState.Y != originMouse.Y)
                {
                    float deltaX = currentMouseState.X - originMouse.X;
                    float deltaY = currentMouseState.Y - originMouse.Y;

                    rotationBuffer.X -= 0.05f * deltaX * dtSeconds;
                    rotationBuffer.Y -= 0.05f * deltaY * dtSeconds;

                    if (rotationBuffer.Y < MathHelper.ToRadians(-75.0f))
                    {
                        rotationBuffer.Y = rotationBuffer.Y - (rotationBuffer.Y -
                            MathHelper.ToRadians(-75.0f));
                    }
                    if (rotationBuffer.Y > MathHelper.ToRadians(75.0f))
                    {
                        rotationBuffer.Y = rotationBuffer.Y - (rotationBuffer.Y -
                            MathHelper.ToRadians(75.0f));
                    }

                    cam.Rotation = new Vector3(-MathHelper.Clamp(rotationBuffer.Y,
                        MathHelper.ToRadians(-75.0f), MathHelper.ToRadians(75.0f)),
                        MathHelper.WrapAngle(rotationBuffer.X), 0);

                    Mouse.SetPosition((int)originMouse.X, (int)originMouse.Y);
                }

                #endregion
            }
        }

        /// <summary>
        /// Prevents map collision
        /// </summary>
        private void MapCollision(HeightMap map)
        {
            float mapHeight = map.GetHeight(Position.X, Position.Z);

            if (!jumped && Position.Y != mapHeight)
                Position = new Vector3(Position.X, mapHeight, Position.Z);
        }

        /// <summary>
        /// Shoot
        /// </summary>              
        private void Shoot(GameTime gameTime)
        {
            weapon.Update(gameTime);
        }

        /// <summary>
        /// Crouch
        /// </summary>
        private void Crouch()
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
        private void Jump(HeightMap map)
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