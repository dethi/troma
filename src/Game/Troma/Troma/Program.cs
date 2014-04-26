using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Troma
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Troma game = new Troma())
            {
                game.Run();
            }
        }
    }
#endif
}

