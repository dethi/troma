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

        public OtherPlayer(string name, int id)
        {
            Name = name;
            ID = id;

            State = new STATE();
            Input = new INPUT();

            entity = OtherPlayerObject.BuildEntity(
                State.Position, State.Rotation, "cible");
        }

        public void Update(GameTime gameTime)
        {
            if (State.Alive)
            {
                entity.GetComponent<Transform>().Position = State.Position;
                entity.GetComponent<Transform>().Rotation = State.Rotation;

                entity.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime, ICamera camera)
        {
            if (State.Alive)
                entity.Draw(gameTime, camera);
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
}
