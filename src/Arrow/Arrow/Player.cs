using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Arrow
{
    class Player
    {
        #region Attributes

        private float playerSpeed;
        private float height;

        private float mapHeight;

        private MouseState currentMouseState;
        private Vector2 originMouse;
        private Vector3 rotationBuffer;

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
            : this(game, pos, 7) { }

        public Player(Game game, Vector3 pos, float height)
            : this(game, pos, height, Vector3.Zero) { }

        public Player(Game game, Vector3 pos, float height, Vector3 rot)
            : this(game, pos, height, rot, 40f) { }

        public Player(Game game, Vector3 pos, float height, Vector3 rot, float playerSpeed)
        {
            this.playerSpeed = playerSpeed;
            this.height = height;

            this.cam = Camera.Instance;
            this.cam.New(game, new Vector3(pos.X, pos.Y + height + mapHeight, pos.Z), rot);

            int centerX = game.GraphicsDevice.Viewport.Width / 2;
            int centerY = game.GraphicsDevice.Viewport.Height / 2;
            originMouse = new Vector2(centerX, centerY);
            Mouse.SetPosition(centerX, centerY);
        }

        #endregion

        public void Update(GameTime gameTime, HeightMap map)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Fire(gameTime);
            CameraOrientation(dt);
            Walk(dt);
            MapCollision(map);
        }

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

                if (kbs.IsKeyDown(Keys.W))
                    moveVector.Z += 1;
                if (kbs.IsKeyDown(Keys.S))
                    moveVector.Z += -1;
                if (kbs.IsKeyDown(Keys.A))
                    moveVector.X += 1;
                if (kbs.IsKeyDown(Keys.D))
                    moveVector.X += -1;

                #endregion
            }

            if (moveVector != Vector3.Zero)
            {
                moveVector.Normalize();
                moveVector *= dtSeconds * playerSpeed;
            }

            // Effectue le mouvement
            cam.Move(moveVector);
        }

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

        private void MapCollision(HeightMap map)
        {
            float actualMapHeight = map.GetHeight(Position.X, Position.Z);

            if (mapHeight != actualMapHeight)
            {
                Position = new Vector3(Position.X, mapHeight, Position.Z);
                mapHeight = actualMapHeight;
            }
        }

        private void Fire(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).IsConnected)
            {
                GamePadState gps = GamePad.GetState(PlayerIndex.One);

                if (gps.IsButtonDown(Buttons.RightTrigger))
                    SFXManager.Play("Springfield", gameTime);
            }
            else
            {
                MouseState mouseState = Mouse.GetState();

                if (mouseState.RightButton == ButtonState.Pressed)
                    SFXManager.Play("Springfield", gameTime);
            }
        }
    }
}
