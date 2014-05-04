using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameEngine
{
    public static class DebugConfig
    {
        public static bool DisplayBox = true;

        public static void HandleInput(GameTime gameTime, InputState input)
        {
            if (input.IsPressed(Keys.D2) || input.IsPressed(Buttons.Y))
                DisplayBox = !DisplayBox;
        }
    }
}
