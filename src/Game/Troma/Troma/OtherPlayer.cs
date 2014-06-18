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

        private Entity entity;
        private bool Alive;

        public OtherPlayer(string name, int id)
        {
            Name = name;
            ID = id;

            State = new STATE();
            Input = new INPUT();

            entity = OtherPlayerObject.BuildAndReturnEntity(
                State.Position, State.Rotation, "Soldier");

            Alive = false;
        }

        public void Update(GameTime gameTime)
        {
            if (Alive)
            {
            }
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
            Alive = true;
            State = state;
            Input = new INPUT();

            EntityManager.AddEntity(entity);            
        }

        public void Killed()
        {
            Alive = false;
            EntityManager.Remove(entity);
        }
    }
}
