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
        private bool _isRespectROF;
        private bool _isLoading;

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

            SightPosition = false;
            _info.Munition = _info.MunitionPerLoader;
            _info.Loader--;

            _isRespectROF = true;
            _isLoading = false;
        }

        public bool Shoot()
        {
            if (LoaderIsEmpty)
            {
                SFXManager.Play(Info.SFXEmpty);
            }
            else if (_isRespectROF && !_isLoading)
            {
                SFXManager.Play(Info.SFXShoot);
                _info.Munition--;

                if (!_info.Automatic)
                {
                    TimerManager.Add(_info.ROF, ROFTimerEnded);
                    _isRespectROF = false;
                }

                TimerManager.Add(30, GunAnimationOn);
                TimerManager.Add(40, GunAnimationOn);
                TimerManager.Add(50, GunAnimationOn);
                TimerManager.Add(60, GunAnimationOff);
                TimerManager.Add(70, GunAnimationOff);
                TimerManager.Add(80, GunAnimationOff);

                return true;
            }

            return false;
        }

        public void Reload()
        {
            if (_info.Loader > 0 && !_isLoading)
            {
                TimerManager.Add(500, PlaySoundLoading);
                TimerManager.Add(1200, LoadingTimerEnded);
                _isLoading = true;
                _info.Loader--;
                _info.Munition = _info.MunitionPerLoader;
            }
        }

        public void ChangeSight()
        {
            SightPosition = !SightPosition;
        }

        #region Event

        public void ROFTimerEnded(object o, EventArgs e)
        {
            _isRespectROF = true;
        }

        public void PlaySoundLoading(object o, EventArgs e)
        {
            SFXManager.Play(Info.SFXReload);
        }

        public void LoadingTimerEnded(object o, EventArgs e)
        {
            _isLoading = false;
        }

        public void GunAnimationOn(object o, EventArgs e)
        {
            _info.PositionSight.Z -= 0.1f;
        }

        public void GunAnimationOff(object o, EventArgs e)
        {
            _info.PositionSight.Z += 0.1f;
        }

        #endregion
    }
}
