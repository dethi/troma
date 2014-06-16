using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameEngine;

namespace Troma
{
    public class Weapon : EntityComponent
    {
        public Entity Arms;
        public MuzzleFlash Muzzle;

        private WeaponInfo _info;
        private bool _isRespectROF;
        private bool _isLoading;
        private int _munitionUsed;

        public bool SightPosition;

        private Random random;

        public float CameraXRecoil { get; private set; }
        public float CameraYRecoil { get; private set; }
        public float ZRecoil { get; private set; }

        public WeaponInfo Info
        {
            get { return _info; }
        }

        public int MunitionUsed
        {
            get { return _munitionUsed; }
        }

        public Vector3 RecoilTranslation
        {
            get { return new Vector3(0, 0, ZRecoil); }
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
            random = new Random();
        }

        public override void Start()
        {
            base.Start();

            Texture2D[] f = new Texture2D[24];

            for (int i = 0; i < f.Length; i++)
                f[i] = FileManager.Load<Texture2D>(String.Format("MuzzleFlash/Muzzle_{0:D5}", i));

            Muzzle = new MuzzleFlash(f, _info.MuzzleOffset);

            Entity.GetComponent<AnimatedModel3D>().PlayClip(_info.ChangeUp, _info.Weapon_nb_bone);
            Arms.GetComponent<AnimatedModel3D>().PlayClip(_info.ChangeUp, _info.Arms_nb_bone);
        }

        public void UpdateRecoil(GameTime gameTime)
        {
            float amplitudeZ = 6f;
            float amount = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;

            if (ZRecoil < 0) 
                ZRecoil += amplitudeZ * amount * Math.Abs(ZRecoil);
            if (ZRecoil > 0) 
                ZRecoil = 0;

            CameraXRecoil = 0;
            CameraYRecoil = 0;
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

                Muzzle.Activate();
                AddRecoil();

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
            if (_isLoading)
                return;

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

        private void AddRecoil()
        {
            CameraXRecoil = 0.0001f * (random.Next(100) - random.Next(100));
            CameraYRecoil = 0.0003f * random.Next(100);

            ZRecoil -= 0.4f;
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
