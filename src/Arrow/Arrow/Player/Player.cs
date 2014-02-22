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

        private MouseState currentMouseState;
        private Vector2 originMouse;
        private Vector3 rotationBuffer;
        private bool leftButtonPressed;

        private Vector3 velocity;
        private bool jumped;
        private int recharge = 0;

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

            int centerX = game.GraphicsDevice.Viewport.Width / 2;
            int centerY = game.GraphicsDevice.Viewport.Height / 2;
            originMouse = new Vector2(centerX, centerY);
            Mouse.SetPosition(centerX, centerY);

            leftButtonPressed = false;

            velocity = new Vector3(0, 1, 0);
            jumped = false;
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

        /// <summary>
        /// Move player
        /// </summary>
        /// <param name="dtSeconds">Total seconds elapsed since last update</param>
        private void Walk(float dtSeconds)
        {
            Vector3 moveVector = Vector3.Zero;

            if (GamePad.GetState(PlayerIndex.One).IsConnected)
            {
                #region ThumbStickLeft

                GamePadState gps = GamePad.GetState(PlayerIndex.One);

                if (gps.ThumbSticks.Left.X != 0)
                    moveVector.X -= gps.ThumbSticks.Left.X;
                if (gps.ThumbSticks.Left.Y != 0)
                    moveVector.Z += gps.ThumbSticks.Left.Y;

                #endregion
            }
            else
            {
                #region Keyboard

                KeyboardState kbs = Keyboard.GetState();

                if (kbs.IsKeyDown(KB_UP))
                    moveVector.Z += 1;
                if (kbs.IsKeyDown(KB_BOTTOM))
                    moveVector.Z += -1;
                if (kbs.IsKeyDown(KB_LEFT))
                    moveVector.X += 1;
                if (kbs.IsKeyDown(KB_RIGHT))
                    moveVector.X += -1;

                #endregion
            }

            if (moveVector != Vector3.Zero)
            {
                moveVector.Normalize();
                moveVector *= dtSeconds * WALK_SPEED;

                if (Keyboard.GetState().IsKeyDown(KB_RUN) ||
                    GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftStick))
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
        ///                
        private void Shoot(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).IsConnected)
            {
                GamePadState gps = GamePad.GetState(PlayerIndex.One);

                if (gps.IsButtonDown(Buttons.RightTrigger) || leftButtonPressed)
                {
                    if (!leftButtonPressed)
                    {
                        SFXManager.Play("Springfield");
                        leftButtonPressed = true;
                    }
                    else if (gps.IsButtonUp(Buttons.RightTrigger))
                        leftButtonPressed = false;
                }
            }
            else
            {
                MouseState mouseState = Mouse.GetState();

                if (mouseState.LeftButton == ButtonState.Pressed || leftButtonPressed)
                {
                    if (!leftButtonPressed)
                    {
                        if (recharge > 7)
                        {
                            SFXManager.Play("Empty Gun");

                        }
                        else
                        {
                            recharge++;
                            SFXManager.Play("Springfield");
                        }
                        leftButtonPressed = true;
                    }

                    else if (mouseState.LeftButton == ButtonState.Released)
                        leftButtonPressed = false;
                }
                else
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.R))
                    {
                        SFXManager.Play("Reload");
                        recharge = 0;
                    }
                }
            }
        }

        /// <summary>
        /// Crouch
        /// </summary>
        private void Crouch()
        {
            float currentHeight = height;

            if (GamePad.GetState(PlayerIndex.One).IsConnected)
            {
                GamePadState gps = GamePad.GetState(PlayerIndex.One);

                if (gps.IsButtonDown(Buttons.B))
                    currentHeight = CROUCH_HEIGHT;
                else
                    currentHeight = HEIGHT;
            }
            else
            {
                KeyboardState kbs = Keyboard.GetState();

                if (kbs.IsKeyDown(KB_CROUCH))
                    currentHeight = CROUCH_HEIGHT;
                else
                    currentHeight = HEIGHT;
            }

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

                if (Keyboard.GetState().IsKeyDown(KB_JUMP) ||
                    GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A))
                {
                    jumped = true;
                }
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