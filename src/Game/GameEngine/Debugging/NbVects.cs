using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameEngine
{
#if DEBUG
    public static class NbVects
    {
        private static int Count;

        public static string Debug(GameTime gameTime)
        {
            return ("N_Vects: " + Count);
        }

        public static void Add(int n)
        {
            Count += n;
        }

        public static void Reset()
        {
            Count = 0;
        }
    }
#endif
}
