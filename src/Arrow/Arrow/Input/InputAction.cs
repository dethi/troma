using System;
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

        public bool PlayerCrouch()
        {
            return IsDown(KeyActions.Crouch) || IsDown(Buttons.B);
        }

        public bool PlayerJump()
        {
            return IsDown(KeyActions.Jump) || IsDown(Buttons.A);
        }

        #endregion
    }
}
