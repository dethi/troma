using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameEngine
{
    public static class CheatKey
    {
        public static SkyType skyType = SkyType.CloudField;

        public static void Update(InputState input)
        {
            if (input.IsPressed(Buttons.LeftShoulder))
            {
                if (skyType == SkyType.CloudField)
                    skyType = SkyType.CloudSplatter;
                else
                    skyType = SkyType.CloudField;
            }
        }
    }
}
