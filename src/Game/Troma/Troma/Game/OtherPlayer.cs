using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GameEngine;
using ClientServerExtension;

namespace Troma
{
    public class OtherPlayer
    {
        public string Name;
        public int ID;

        public STATE State;
        public INPUT Input;

        private Entity soldierM1;
        private Entity soldierM1911;

        private Entity current;
        private AnimatedModel3D currentModel;
        private OtherPlayerAnim info;

        public OtherPlayer(string name, int id)
        {
            Name = name;
            ID = id;

            State = new STATE();
            Input = new INPUT();

            soldierM1 = OtherPlayerObject.BuildAnimatedEntity(
                State.Position, State.Rotation, "Soldier_M1");
            soldierM1.Initialize();

            soldierM1911 = OtherPlayerObject.BuildAnimatedEntity(
                State.Position, State.Rotation, "Soldier_M1911");
            soldierM1911.Initialize();

            current = soldierM1;
            currentModel = current.GetComponent<AnimatedModel3D>();
            info = Constants.M1;

            currentModel.PlayClip(info.Accroupi_Marche_Rechargement, info.Bone);
        }

        public void Update(GameTime gameTime)
        {
            if (State.Alive)
            {
                current.GetComponent<Transform>().Position = State.Position;
                current.GetComponent<Transform>().Rotation = new Vector3(
                    1.57f - State.Rotation.X,
                    3.14f + State.Rotation.Y,
                    State.Rotation.Z);

                current.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime, ICamera camera)
        {
            if (State.Alive)
                current.Draw(gameTime, camera);
        }

        #region Actions

        private void Move()
        {
        }

        private void Crouch()
        {
        }

        private void WeaponAction()
        {
        }

        private void ChangeWeapon()
        {
        }

        public void Shoot()
        {
            // play shoot animation
        }

        #endregion

        public void Spawn(STATE state)
        {
            State = state;
            Input = new INPUT();
        }
    }

    public struct OtherPlayerAnim
    {
        public int Bone;

        public AnimInfo Debout_Arret_Vise_Haut;
        public AnimInfo Debout_Arret_Vise_Bas;
        public AnimInfo Debout_Arret_Rechargement;
        public AnimInfo Debout_Marche;
        public AnimInfo Debout_Marche_Vise_Haut;
        public AnimInfo Debout_Marche_vise;
        public AnimInfo Debout_Marche_Vise_Bas;
        public AnimInfo Debout_Marche_Rechargement;
        public AnimInfo Mise_Accroupi;
        public AnimInfo Accroupi_Arret_Vise_Haut;
        public AnimInfo Accroupi_Arret_Vise_Bas;
        public AnimInfo Accroupi_Arret_Rechargement;
        public AnimInfo Accroupi_Marche;
        public AnimInfo Accroupi_Marche_Vise_Haut;
        public AnimInfo Accroupi_Marche_vise;
        public AnimInfo Accroupi_Marche_Vise_Bas;
        public AnimInfo Accroupi_Marche_Rechargement;
        public AnimInfo Mise_debout;
        public AnimInfo Course;
    }
}
