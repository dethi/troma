using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Lidgren.Network;
using ClientServerExtension;

namespace GameServer
{
    public class Player
    {
        public string Name;
        public int ID;
        public NetConnection Connection;

        public STATE State;
        public INPUT Input;

        private bool Alive;

        public Player(string name, int id, NetConnection co)
        {
            Name = name;
            ID = id;
            Connection = co;

            State = new STATE();
            Input = new INPUT();

            Alive = false;
        }

        public void Spawn(STATE state)
        {
            Alive = true;

            State = state;
            Input = new INPUT();
        }

        public void Killed()
        {
            Alive = false;
        }
    }
}
