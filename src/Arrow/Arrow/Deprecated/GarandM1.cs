using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Arrow
{
    class GarandM1 : Weapon
    {
        public GarandM1(Game game)
            : base(game)
        {
            nb_munition_per_loader = 8;
            nb_loader = 10;

            automatic_weapon = false;
            rof = 0.5f;

            sfx_shoot = "GarandM1";
            sfx_empty_loader = "GarandM1_empty";
            sfx_reload = "GarandM1_reload";

            model = new Entity(game, "m1", Vector3.Zero);
            Initialize();
        }
    }
}
