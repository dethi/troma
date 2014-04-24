using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameEngine
{
    public enum MouseButtons
    {
        Left,
        Middle,
        Right
    }

    public enum KeyActions
    {
        Up,
        Bottom,
        Left,
        Right,
        Crouch,
        Run,
        Jump,
        Reload
    }

    public class InputState
    {
        #region Fields

        public InputConfiguration mapping { get; private set; }

        public GamePadState CurrentGamePadState { get; private set; }
        public GamePadState LastGameState { get; private set; }

        public KeyboardState CurrentKeyboardState { get; private set; }
        public KeyboardState LastKeyboardState { get; private set; }

        public MouseState CurrentMouseState { get; private set; }
        public MouseState LastMouseState { get; private set; }

        bool isGamePadConnected;
        Vector2 mouseOrigin;

        public Vector2 MouseOrigin
        {
            get { return mouseOrigin; }
            set
            {
                mouseOrigin = value;
                MouseResetPos();
            }
        }

        #endregion

        public InputState()
        {
            mapping = new InputConfiguration();

            CurrentGamePadState = new GamePadState();
            isGamePadConnected = false;
            CurrentKeyboardState = new KeyboardState();
            CurrentMouseState = new MouseState();
            mouseOrigin = Vector2.Zero;
        }

        public void Update()
        {
            LastGameState = CurrentGamePadState;
            CurrentGamePadState = GamePad.GetState(PlayerIndex.One);
            isGamePadConnected = CurrentGamePadState.IsConnected;

            LastKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = Keyboard.GetState();

            LastMouseState = CurrentMouseState;
            CurrentMouseState = Mouse.GetState();
        }

        #region GamePad

        public bool IsPressed(Buttons button)
        {
            return isGamePadConnected &&
                CurrentGamePadState.IsButtonDown(button) &&
                LastGameState.IsButtonUp(button);
        }

        public bool IsDown(Buttons button)
        {
            return isGamePadConnected &&
                CurrentGamePadState.IsButtonDown(button);
        }

        public bool IsUp(Buttons button)
        {
            return isGamePadConnected &&
                CurrentGamePadState.IsButtonUp(button);
        }

        #endregion

        #region Keyboard

        public bool IsPressed(KeyActions keyAction)
        {
            return IsPressed(mapping.Parse(keyAction));
        }

        public bool IsPressed(Keys key)
        {
            return CurrentKeyboardState.IsKeyDown(key) &&
                LastKeyboardState.IsKeyUp(key);
        }

        public bool IsDown(KeyActions keyAction)
        {
            return IsDown(mapping.Parse(keyAction));
        }

        public bool IsDown(Keys key)
        {
            return CurrentKeyboardState.IsKeyDown(key);
        }

        public bool IsUp(KeyActions keyAction)
        {
            return IsUp(mapping.Parse(keyAction));
        }

        public bool IsUp(Keys key)
        {
            return CurrentKeyboardState.IsKeyUp(key);
        }

        #endregion

        #region Mouse

        public bool IsPressed(MouseButtons mouseButton)
        {
            if (mouseButton == MouseButtons.Left)
                return _IsPressed(CurrentMouseState.LeftButton, LastMouseState.LeftButton);
            else if (mouseButton == MouseButtons.Middle)
                return _IsPressed(CurrentMouseState.MiddleButton, LastMouseState.MiddleButton);
            else
                return _IsPressed(CurrentMouseState.RightButton, LastMouseState.RightButton);
        }

        private bool _IsPressed(ButtonState currentState, ButtonState lastState)
        {
            return currentState == ButtonState.Pressed &&
                lastState == ButtonState.Released;
        }

        public bool IsDown(MouseButtons mouseButton)
        {
            if (mouseButton == MouseButtons.Left)
                return _IsDown(CurrentMouseState.LeftButton);
            else if (mouseButton == MouseButtons.Middle)
                return _IsDown(CurrentMouseState.MiddleButton);
            else
                return _IsDown(CurrentMouseState.RightButton);
        }

        private bool _IsDown(ButtonState currentState)
        {
            return currentState == ButtonState.Pressed;
        }

        public bool IsUp(MouseButtons mouseButton)
        {
            if (mouseButton == MouseButtons.Left)
                return _IsUp(CurrentMouseState.LeftButton);
            else if (mouseButton == MouseButtons.Middle)
                return _IsUp(CurrentMouseState.MiddleButton);
            else
                return _IsUp(CurrentMouseState.RightButton);
        }

        private bool _IsUp(ButtonState currentState)
        {
            return currentState == ButtonState.Released;
        }

        public void MouseResetPos()
        {
            Mouse.SetPosition((int)mouseOrigin.X, (int)mouseOrigin.Y);
        }

        #endregion

        #region Helper Methods

        #region Player

        /// <summary>
        /// Check if the player should rotate
        /// </summary>
        /// <param name="dt">Time elapsed in seconds</param>
        /// <returns>True if there is a rotation</returns>
        public bool PlayerRotate(ref Vector3 rotationBuffer, float dt)
        {
            if (isGamePadConnected && (CurrentGamePadState.ThumbSticks.Right.X != 0 ||
                CurrentGamePadState.ThumbSticks.Right.Y != 0))
            {
                rotationBuffer.X -= 1.5f * CurrentGamePadState.ThumbSticks.Right.X * dt;
                rotationBuffer.Y += 1.5f * CurrentGamePadState.ThumbSticks.Right.Y * dt;

                return true;
            }
            else if (CurrentMouseState.X != mouseOrigin.X || CurrentMouseState.Y != mouseOrigin.Y)
            {
                rotationBuffer.X -= 0.05f * (CurrentMouseState.X - mouseOrigin.X) * dt;
                rotationBuffer.Y -= 0.05f * (CurrentMouseState.Y - mouseOrigin.Y) * dt;
                Mouse.SetPosition((int)mouseOrigin.X, (int)mouseOrigin.Y);

                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Check if the player should move
        /// </summary>
        /// <returns>True if there is a move</returns>
        public bool PlayerMove(out Vector3 moveVector)
        {
            moveVector = Vector3.Zero;

            if (isGamePadConnected)
            {
                if (CurrentGamePadState.ThumbSticks.Left.X != 0)
                    moveVector.X -= CurrentGamePadState.ThumbSticks.Left.X;
                if (CurrentGamePadState.ThumbSticks.Left.Y != 0)
                    moveVector.Z += CurrentGamePadState.ThumbSticks.Left.Y;
            }
            else
            {
                if (IsDown(KeyActions.Up))
                    moveVector.Z += 1;
                if (IsDown(KeyActions.Bottom))
                    moveVector.Z += -1;
                if (IsDown(KeyActions.Left))
                    moveVector.X += 1;
                if (IsDown(KeyActions.Right))
                    moveVector.X += -1;
            }

            if (moveVector != Vector3.Zero)
            {
                moveVector.Normalize();
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Check if the player should run
        /// </summary>
        public bool PlayerRun()
        {
            return IsDown(KeyActions.Run) || IsDown(Buttons.LeftStick);
        }

        /// <summary>
        /// Check if the player should crouch
        /// </summary>
        public bool PlayerCrouch()
        {
            return IsDown(KeyActions.Crouch) || IsDown(Buttons.B);
        }

        /// <summary>
        /// Check if the player should jump
        /// </summary>
        public bool PlayerJump()
        {
            return IsDown(KeyActions.Jump) || IsDown(Buttons.A);
        }

        #endregion

        #endregion
    }
}
