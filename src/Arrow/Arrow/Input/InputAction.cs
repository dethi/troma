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
        public Vector3 PlayerMove()
        {
            Vector3 moveVector = Vector3.Zero;

            if (isGamePadConnected)
            {
                if (CurrentGamePadState.ThumbSticks.Left.X != 0)
                    moveVector.X -= CurrentGamePadState.ThumbSticks.Left.X;
                if (CurrentGamePadState.ThumbSticks.Left.Y != 0)
                    moveVector.Z += CurrentGamePadState.ThumbSticks.Left.Y;
            }
            else
            {
                if (IsDown(mapping.KB_UP))
                    moveVector.Z += 1;
                if (IsDown(mapping.KB_BOTTOM))
                    moveVector.Z += -1;
                if (IsDown(mapping.KB_LEFT))
                    moveVector.X += 1;
                if (IsDown(mapping.KB_RIGHT))
                    moveVector.X += -1;
            }

            if (moveVector != Vector3.Zero)
                moveVector.Normalize();

            return moveVector;
        }
    }
}
