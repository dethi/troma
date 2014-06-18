using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Lidgren.Network;

namespace GameServer
{
    public struct STATE
    {
        public Vector3 Position;
        public Vector3 Rotation;
    }

    public struct INPUT
    {
        public bool IsMove;
        public bool IsRun;
        public bool IsCrouch;

        public bool IsShoot;
        public bool IsReload;
        public bool InSightPosition;
        public Weapons Weapon;
    }

    public enum Weapons
    {
        M1,
        M1911
    }

    class Player
    {
        public string Name;
        public int ID;
        public NetConnection Connection;

        public STATE State;
        public INPUT Input;

        public Player(string name, int id, NetConnection co)
        {
            Name = name;
            ID = id;
            Connection = co;

            State = new STATE();
            Input = new INPUT();
        }

        public void Reset(Vector3 pos, Vector3 rot)
        {
            State.Position = pos;
            State.Rotation = rot;

            Input = new INPUT();
        }
    }
}
