using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arrow
{
    class Springfield : Weapon
    {
        public Springfield(Game game)
            : base(game)
        {
            nb_munition_per_loader = 8;
            nb_loader = 2;

            automatic_weapon = false;
            rof = 0.5f;

            sfx_shoot = "Springfield";
            sfx_empty_loader = "Empty_Gun";
            sfx_reload = "Reload";

            Initialize();
        }
    }
}
