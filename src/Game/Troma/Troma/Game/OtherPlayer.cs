﻿using System;
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

            entity = OtherPlayerObject.BuildAnimatedEntity(
                State.Position, State.Rotation, "soldier_M1");

            entity.GetComponent<AnimatedModel3D>().PlayClip(new AnimInfo(0, 24), 51);
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

    public struct OtherPlayerAnim
    {
        public int Bone;

        public AnimInfo Shoot;
        //etc je te laisse en faire autant qu'il faut
    }
}
