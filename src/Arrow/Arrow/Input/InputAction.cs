﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Arrow
{
    public partial class InputState
    {
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
    }
}
