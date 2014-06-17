using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Lidgren.Network;

namespace GameServer
{
    class Player
    {
        public int Slot;
        public NetConnection Connection;

        public Vector3 Position;
        public Vector3 Rotation;

        public Player(int slot, NetConnection co)
        {
            Slot = slot;
            Connection = co;
        }

        public void Reset(Vector3 pos, Vector3 rot)
        {
            Position = pos;
            Rotation = rot;
        }
    }
}
