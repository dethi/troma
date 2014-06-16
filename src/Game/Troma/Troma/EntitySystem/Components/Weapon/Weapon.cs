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
        public Entity Arms;
        private WeaponInfo _info;
        private bool _isRespectROF;
        private bool _isLoading;
        private int _munitionUsed;

        public bool SightPosition;

        public WeaponInfo Info
        {
            get { return _info; }
        }

        public int MunitionUsed
        {
            get { return _munitionUsed; }
        }

        private bool LoaderIsEmpty
        {
            get { return (_info.Munition == 0); }
        }

        public Weapon(Entity aParent, Entity arms, WeaponInfo weaponInfo)
            : base(aParent)
        {
            Name = "Weapon";
            _requiredComponents.Add("AnimatedModel3D");

            Arms = arms;
            _info = weaponInfo;

            SightPosition = false;
            _info.Munition = _info.MunitionPerLoader;
            _info.Loader--;

            _isRespectROF = true;
            _isLoading = false;

            _munitionUsed = 0;
        }

        public override void Start()
        {
            base.Start();

            Entity.GetComponent<AnimatedModel3D>().PlayClip(_info.ChangeUp, _info.Weapon_nb_bone);
            Arms.GetComponent<AnimatedModel3D>().PlayClip(_info.ChangeUp, _info.Arms_nb_bone);
        }

        public bool Shoot()
        {
            if (LoaderIsEmpty)
                SFXManager.Play(Info.SFXEmpty);
            else if (_isRespectROF && !_isLoading)
            {
                SFXManager.Play(Info.SFXShoot);
                _info.Munition--;
                _munitionUsed++;

                if (!_info.Automatic)
                {
                    TimerManager.Add(_info.ROF, ROFTimerEnded);
                    _isRespectROF = false;
                }

                if (SightPosition)
                {
                    Entity.GetComponent<AnimatedModel3D>().PlayClip(_info.AimShoot, _info.Weapon_nb_bone);
                    Arms.GetComponent<AnimatedModel3D>().PlayClip(_info.AimShoot, _info.Arms_nb_bone);
                }
                else
                {
                    Entity.GetComponent<AnimatedModel3D>().PlayClip(_info.Shoot, _info.Weapon_nb_bone);
                    Arms.GetComponent<AnimatedModel3D>().PlayClip(_info.Shoot, _info.Arms_nb_bone);
                }

                return true;
            }

            return false;
        }

        public void Reload()
        {
            if (_info.Loader > 0 && !_isLoading)
            {
                SightPosition = false;
                _isLoading = true;
                _info.Loader--;
                _info.Munition = _info.MunitionPerLoader;

                TimerManager.Add(_info.StartReloadSFX, PlaySoundLoading);
                TimerManager.Add(_info.TimeToReload, LoadingTimerEnded);

                Entity.GetComponent<AnimatedModel3D>().PlayClip(_info.Reload, _info.Weapon_nb_bone);
                Arms.GetComponent<AnimatedModel3D>().PlayClip(_info.Reload, _info.Arms_nb_bone);
            }
        }

        public void ChangeSight()
        {
            SightPosition = !SightPosition;

            if (SightPosition)
            {
                Entity.GetComponent<AnimatedModel3D>().PlayClip(_info.AimUp, _info.Weapon_nb_bone);
                Arms.GetComponent<AnimatedModel3D>().PlayClip(_info.AimUp, _info.Arms_nb_bone);
            }
            else
            {
                Entity.GetComponent<AnimatedModel3D>().PlayClip(_info.AimDown, _info.Weapon_nb_bone);
                Arms.GetComponent<AnimatedModel3D>().PlayClip(_info.AimDown, _info.Arms_nb_bone);
            }
        }

        public void ChangeUp()
        {
            Entity.GetComponent<AnimatedModel3D>().PlayClip(_info.ChangeUp, _info.Weapon_nb_bone);
            Arms.GetComponent<AnimatedModel3D>().PlayClip(_info.ChangeUp, _info.Arms_nb_bone);
        }

        public void ChangeDown()
        {
            Entity.GetComponent<AnimatedModel3D>().PlayClip(_info.ChangeDown, _info.Weapon_nb_bone);
            Arms.GetComponent<AnimatedModel3D>().PlayClip(_info.ChangeDown, _info.Arms_nb_bone);
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

        #endregion
    }
}
