using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GameEngine;

namespace Troma
{
    public class Weapon : EntityComponent
    {
        private WeaponInfo _info;
        private double dt_lastShoot;

        public bool SightPosition;

        public WeaponInfo Info
        {
            get { return _info; }
        }

        private bool LoaderIsEmpty
        {
            get { return (_info.Munition == 0); }
        }

        public Weapon(Entity aParent, WeaponInfo weaponInfo)
            : base(aParent)
        {
            Name = "Weapon";

            _info = weaponInfo;

            dt_lastShoot = 0;
            SightPosition = false;
            _info.Munition = _info.MunitionPerLoader;
            _info.Loader--;
        }

        public bool Shoot(double elapsedTime)
        {
            if (LoaderIsEmpty)
                SFXManager.Play(Info.SFXEmpty);
            else if (IsRespectROF(elapsedTime))
            {
                SFXManager.Play(Info.SFXShoot);

                dt_lastShoot = elapsedTime;
                _info.Munition--;
                return true;
            }

            return false;
        }

        public void Reload()
        {
            if (_info.Loader > 0)
            {
                SFXManager.Play(Info.SFXReload);
                _info.Loader--;
                _info.Munition = _info.MunitionPerLoader;
            }
        }

        public void ChangeSight()
        {
            SightPosition = !SightPosition;
        }

        private bool IsRespectROF(double dt_current)
        {
            return _info.Automatic || 
                (Math.Abs(dt_current - dt_lastShoot) >= _info.ROF);
        }
    }
}
